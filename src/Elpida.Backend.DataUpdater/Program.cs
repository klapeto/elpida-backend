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
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Services.Extensions.Benchmark;
using Elpida.Backend.Services.Extensions.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
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

				using (baseLogger.BeginScope("Database update"))
				{
					using (var context = new UpdateElpidaDbContext(connectionString, loggerFactory))
					{
						await context.Database.EnsureCreatedAsync();

						var addedTasks = new List<TaskModel>();
						foreach (var task in data.Tasks
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
										.Select(t => addedTasks.First(a => a.Uuid == t))
										.ToList()
								});
							}
							else
							{
								updatedBenchmark.Update(new BenchmarkModel
								{
									Id = updatedBenchmark.Id,
									Uuid = benchmark.Uuid,
									Name = benchmark.Name,
									Tasks = benchmark.TaskSpecifications
										.Select(t => addedTasks.First(a => a.Uuid == t))
										.ToList()
								});
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