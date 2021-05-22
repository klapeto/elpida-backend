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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Task;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            using var servicesConfiguration = new ServicesConfigurator(args);
            using var scope = servicesConfiguration.ServiceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var baseLogger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Main");

            var targetDirectory = Path.Combine(Path.GetTempPath(), "Elpida");

            baseLogger.LogInformation("Creating temporary directory for split files: {Directory}", targetDirectory);
            Directory.CreateDirectory(targetDirectory);
            try
            {
                await SplitFiles(serviceProvider, "ResultData", targetDirectory);
                await UpdateDbAsync(serviceProvider, "BenchmarkData", targetDirectory);

                baseLogger.LogInformation("All operations completed successfully");
            }
            finally
            {
                baseLogger.LogInformation("Deleting temporary directory for split files: {Directory}", targetDirectory);
                Directory.Delete(targetDirectory, true);
            }
        }

        private static async Task SplitFiles(IServiceProvider serviceProvider, string sourceDirectory,
            string targetDirectory)
        {
            using var scope = serviceProvider.CreateScope();
            serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("File splitter");

            var filesProduced = 0;
            var filesRead = 0;
            logger.LogInformation("Begin splitting files");
            var stopWatch = Stopwatch.StartNew();
            await ParallelExecutor.ParallelExecAsync(Directory.EnumerateFiles(sourceDirectory), async (file, token) =>
            {
                var resultData =
                    JsonConvert.DeserializeObject<List<ResultDto>?>(await File.ReadAllTextAsync(file, token));

                if (resultData == null) return;

                Interlocked.Increment(ref filesRead);

                file = Path.GetFileName(file);

                await ParallelExecutor.ParallelExecAsync(resultData, async (dto, ct) =>
                {
                    var id = Interlocked.Increment(ref filesProduced);
                    await File.WriteAllTextAsync(Path.Combine(targetDirectory, $"{file}_{id}.json"),
                        JsonConvert.SerializeObject(new[] {dto}), ct);
                }, token);
            });

            stopWatch.Stop();
            logger.LogInformation(
                "Split complete. {FilesRead} files were split into {FilesSplit} and took {Time} ",
                filesRead,
                filesProduced,
                stopWatch.Elapsed);
        }

        private static async Task UpdateDbAsync(IServiceProvider serviceProvider, string benchmarkDataDirectory,
            string seedDataDirectory)
        {
            using var scope = serviceProvider.CreateScope();
            serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Database updater");

            try
            {
                var context = serviceProvider.GetRequiredService<ElpidaContext>();

                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                await SeedBenchmarks(serviceProvider, benchmarkDataDirectory);
                await SeedResults(serviceProvider, seedDataDirectory);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Failed to update");
            }
        }

        private static long _resultsAddedCount;
        private static long _resultsProcessedCount;

        private static async Task ProcessResultAsync(
            IServiceProvider serviceProvider,
            ResultDto resultDto,
            CancellationToken cancellationToken)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Result seeder");

            try
            {
                Interlocked.Increment(ref _resultsProcessedCount);
                var resultService = serviceProvider.GetRequiredService<IBenchmarkResultsService>();
                logger.LogTrace("Seeding result: '{Name}': '{Uuid}'", resultDto.Result.Name,
                    resultDto.Result.Uuid);
                await resultService.GetOrAddAsync(resultDto, cancellationToken);
                var currentResults = Interlocked.Increment(ref _resultsAddedCount);
                if (currentResults % 500 == 0)
                {
                    logger.LogInformation("Added so far: {Added}", currentResults);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to add result");
            }
        }

        private static async Task ProcessBenchmarkAsync(
            IServiceProvider serviceProvider,
            BenchmarkDto benchmarkDto,
            CancellationToken cancellationToken)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Benchmarks updater");

            try
            {
                var benchmarkService = serviceProvider.GetRequiredService<IBenchmarkService>();
                logger.LogTrace("Updating benchmark data: '{Name}': '{Uuid}'", benchmarkDto.Name, benchmarkDto.Uuid);
                await benchmarkService.GetOrAddAsync(benchmarkDto, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to add benchmark");
            }
        }

        private static async Task ProcessTaskAsync(
            IServiceProvider serviceProvider,
            TaskDto taskDto,
            CancellationToken cancellationToken)
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Tasks updater");

            try
            {
                var taskService = serviceProvider.GetRequiredService<ITaskService>();
                logger.LogTrace("Updating task data: '{Name}': '{Uuid}'", taskDto.Name, taskDto.Uuid);
                await taskService.GetOrAddAsync(taskDto, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failed to add task");
            }
        }

        private static async Task SeedResults(IServiceProvider serviceProvider, string resultsDirectory)
        {
            using var scope = serviceProvider.CreateScope();
            serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Results seeder");

            await TimeProcedure(ParallelExecutor.ProcessFilesInDirectoryAsync<ResultDto>(
                resultsDirectory,
                serviceProvider,
                ProcessResultAsync), logger);

            logger.LogInformation("Results processed: {Processed}. Results successfully added: {Added}",
                _resultsProcessedCount, _resultsAddedCount);
        }

        private static async Task SeedBenchmarks(IServiceProvider serviceProvider, string benchmarksDirectory)
        {
            using var scope = serviceProvider.CreateScope();
            serviceProvider = scope.ServiceProvider;

            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Benchmarks updater");

            await TimeProcedure(Task.Run(async () =>
            {
                var taskData =
                    JsonConvert.DeserializeObject<List<TaskDto>?>(
                        await File.ReadAllTextAsync(Path.Combine(benchmarksDirectory, "tasks.json")));

                if (taskData != null)
                {
                    await ParallelExecutor.ProcessItemsAsync(taskData, serviceProvider, ProcessTaskAsync);
                }
            }), logger, "Tasks update");


            await TimeProcedure(Task.Run(async () =>
            {
                var taskData =
                    JsonConvert.DeserializeObject<List<BenchmarkDto>?>(
                        await File.ReadAllTextAsync(Path.Combine(benchmarksDirectory, "benchmarks.json")));

                if (taskData != null)
                {
                    await ParallelExecutor.ProcessItemsAsync(taskData, serviceProvider, ProcessBenchmarkAsync);
                }
            }), logger, "Benchmarks update");
        }

        private static async Task TimeProcedure(Task operation, ILogger logger, [CallerMemberName] string name = "")
        {
            using (logger.BeginScope("Operation: '{Name}'", name))
            {
                logger.LogInformation("Operation started");

                var stopWatch = Stopwatch.StartNew();

                await operation;

                stopWatch.Stop();

                logger.LogInformation("Operation completed: {Time}", stopWatch.Elapsed);
            }
        }
    }
}