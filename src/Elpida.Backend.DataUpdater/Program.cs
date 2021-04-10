/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
    internal static class Program
    {
        private static void Update(this TaskModel model, TaskModel other)
        {
            model.Uuid = other.Uuid;
            model.Name = other.Name;
            model.Description = other.Description;

            model.InputName = other.InputName;
            model.InputDescription = other.InputDescription;
            model.InputUnit = other.InputUnit;
            model.InputProperties = JsonConvert.SerializeObject(other.InputProperties);

            model.OutputName = other.OutputName;
            model.OutputDescription = other.OutputDescription;
            model.OutputUnit = other.OutputUnit;
            model.OutputProperties = JsonConvert.SerializeObject(other.OutputProperties);

            model.ResultName = other.ResultName;
            model.ResultDescription = other.Description;
            model.ResultAggregation = other.ResultAggregation;
            model.ResultType = other.ResultType;
            model.ResultUnit = other.ResultUnit;
        }

        private static TaskModel ToModel(this TaskDto dto)
        {
            return new TaskModel
            {
                Id = dto.Id,
                Uuid = dto.Uuid,
                Name = dto.Name,
                Description = dto.Description,

                InputName = dto.Input?.Name,
                InputDescription = dto.Input?.Description,
                InputUnit = dto.Input?.Unit,
                InputProperties = JsonConvert.SerializeObject(dto.Input?.RequiredProperties),

                OutputName = dto.Output?.Name,
                OutputDescription = dto.Output?.Description,
                OutputUnit = dto.Output?.Unit,
                OutputProperties = JsonConvert.SerializeObject(dto.Output?.RequiredProperties),

                ResultName = dto.Result.Name,
                ResultDescription = dto.Description,
                ResultAggregation = dto.Result.Aggregation,
                ResultType = dto.Result.Type,
                ResultUnit = dto.Result.Unit
            };
        }

        static void foo(IReadOnlyDictionary<string, LambdaExpression> expressions)
        {
            
        }
        
        private static async Task Main(string[] args)
        {

            var x = new Dictionary<string, Expression<Func<string, bool>>>();
            
            foo(x.ToDictionary(k => k.Key, v => (LambdaExpression)v.Value));
            

            return;
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddCommandLine(args)
                .Build();

            var connectionString = config.GetConnectionString("Results");

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddConfiguration(config);
            });

            var baseLogger = loggerFactory.CreateLogger("Main");

            baseLogger.LogInformation("Reading data");
            try
            {
                var data = JsonConvert.DeserializeObject<Data>(await File.ReadAllTextAsync("data.json"));

                var benchmarks = data.Benchmarks;
                var tasks = data.Benchmarks
                    .SelectMany(b => b.TaskSpecifications)
                    .DistinctBy(dto => dto.Uuid)
                    .ToList();

                using (baseLogger.BeginScope("Database update"))
                {
                    using (var context = new UpdateElpidaDbContext(connectionString, loggerFactory))
                    {
                        await context.Database.EnsureDeletedAsync();
                        await context.Database.EnsureCreatedAsync();
                    
                        var addedTasks = new List<TaskModel>();
                        foreach (var task in tasks
                            .Select(t => t.ToModel())
                            .ToList())
                        {
                            var updatedTask = await context.Tasks.FirstOrDefaultAsync(t => t.Uuid == task.Uuid);
                            if (updatedTask == null)
                            {
                                updatedTask = (await context.Tasks.AddAsync(task)).Entity;
                            }
                            else
                            {
                                updatedTask.Update(task);
                            }
                    
                            addedTasks.Add(updatedTask);
                        }
                    
                        await context.SaveChangesAsync();
                    
                        foreach (var benchmark in data.Benchmarks)
                        {
                            var updatedBenchmark =
                                await context.Benchmarks
                                    .Include(m => m.Tasks)
                                    .FirstOrDefaultAsync(t => t.Uuid == benchmark.Uuid);
                    
                            if (updatedBenchmark == null)
                            {
                                await context.Benchmarks.AddAsync(new BenchmarkModel
                                {
                                    Name = benchmark.Name,
                                    Uuid = benchmark.Uuid,
                                    Tasks = benchmark.TaskSpecifications
                                        .Select(t => addedTasks.First(a => a.Uuid == t.Uuid))
                                        .ToList()
                                });
                            }
                            else
                            {
                                updatedBenchmark.Name = benchmark.Name;
                                updatedBenchmark.Uuid = benchmark.Uuid;
                                updatedBenchmark.Tasks = benchmark.TaskSpecifications
                                    .Select(t => addedTasks.First(a => a.Uuid == t.Uuid))
                                    .ToList();
                            }
                        }
                    
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                baseLogger.LogCritical(ex, "Failed to update:");
                return;
            }

            baseLogger.LogInformation("Success");
        }
    }
}