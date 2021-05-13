/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.Services
{
    public class StatisticsUpdaterService : IHostedService, IStatisticsUpdaterService
    {
        private readonly List<Thread> _consumerThreads = new List<Thread>();

        private BlockingCollection<StatisticsUpdateRequest> _updateRequests = default!;
        private readonly ILogger<StatisticsUpdaterService> _logger;
        private CancellationTokenSource _cancellationTokenSource = default!;
        private readonly IServiceProvider _serviceProvider;

        private readonly object _addLocker = new object();

        private const int ConsumerThreadsCount = 1;

        public StatisticsUpdaterService(IServiceProvider serviceProvider, ILogger<StatisticsUpdaterService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start requested");
            if (_consumerThreads.Any(t => t.IsAlive))
            {
                _logger.LogWarning("Start request did not do anything since it is already started");
                return Task.CompletedTask;
            }

            _consumerThreads.Clear();

            _cancellationTokenSource = new CancellationTokenSource();
            _updateRequests = new BlockingCollection<StatisticsUpdateRequest>();

            for (var i = 0; i < ConsumerThreadsCount; i++)
            {
                var id = i;
                var thread = new Thread(() => UpdateProcedure(id))
                {
                    IsBackground = true,
                    Name = $"[{nameof(StatisticsUpdaterService)} ({id})]"
                };
                thread.Start();
                _consumerThreads.Add(thread);
            }

            _logger.LogInformation("Started successfully");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop requested");

            if (_consumerThreads.Any(t => t.IsAlive))
            {
                _updateRequests.CompleteAdding();
                //_cancellationTokenSource.Cancel();

                _logger.LogInformation("Stopping...");
                foreach (var thread in _consumerThreads)
                {
                    thread.Join();
                }

                _logger.LogInformation("Stopped successfully");
                _updateRequests.Dispose();
                return Task.CompletedTask;
            }

            _logger.LogWarning("Stop request did not do anything since it is already stopped");
            return Task.CompletedTask;
        }

        private void UpdateProcedure(int id)
        {
            using (_logger.BeginScope("Updater worker: {Id}", id))
            using (var scope = _serviceProvider.CreateScope())
            {
                var benchmarkStatisticsService =
                    scope.ServiceProvider.GetRequiredService<IBenchmarkStatisticsService>();
                try
                {
                    while (!_updateRequests.IsCompleted)
                    {
                        StatisticsUpdateRequest currentRequest = null!;
                        try
                        {
                            currentRequest = _updateRequests.Take();
                            benchmarkStatisticsService.UpdateTaskStatisticsAsync(
                                    currentRequest.BenchmarkId,
                                    currentRequest.TopologyId,
                                    _cancellationTokenSource.Token)
                                .GetAwaiter().GetResult();
                        }
                        catch (UpdateConcurrencyException uce)
                        {
                            _logger.LogInformation(uce, "Update failed due to concurrency. Enqueueing request again");
                            _updateRequests.Add(currentRequest);
                        }
                        catch (OperationCanceledException oce)
                        {
                            _logger.LogInformation(oce, "Collection was probably marked complete. Terminating...");
                        }
                        catch (InvalidOperationException ioe)
                        {
                            _logger.LogInformation(ioe, "Collection was probably marked complete. Terminating...");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Unexpected error. Worker is terminating...");
                }
            }
        }

        private void AddRequest(StatisticsUpdateRequest request, CancellationToken cancellationToken)
        {
            lock (_addLocker)
            {
                if (_updateRequests.Any(x => x.Equals(request))) return;
                _updateRequests.Add(request, cancellationToken);
            }
        }

        public Task EnqueueUpdateAsync(StatisticsUpdateRequest request, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => AddRequest(request, cancellationToken), cancellationToken);
        }
    }
}