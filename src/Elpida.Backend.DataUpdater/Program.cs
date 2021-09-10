// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Dtos.Task;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
	internal static class Program
	{
		private const string TasksFile = "tasks.json";
		private const string BenchmarkDataDirectory = "BenchmarkData";
		private const string ResultDataDirectory = "ResultData";
		private const string BenchmarksFile = "benchmarks.json";
		private static int _batchesProcessedCount;
		private static int _resultsExpected;
		private static int _resultsProcessed;

		private static int Main(string[] args)
		{
			var app = new CommandLineApplication
			{
				Name = Assembly.GetExecutingAssembly().GetName().Name,
				Description = "Updates/migrates an Elpida database and seeds some data",
				ShortVersionGetter = () => Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
				LongVersionGetter = () => Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
			};

			app.Command(
				"clean",
				c =>
				{
					var (sqlProvider, connectionStringName) = ConfigureArguments(c);
					c.OnExecute(Invoke(sqlProvider, connectionStringName, Clean));
				}
			);

			app.Command(
				"drop",
				c =>
				{
					var (sqlProvider, connectionStringName) = ConfigureArguments(c);
					c.OnExecute(Invoke(sqlProvider, connectionStringName, Drop));
				}
			);

			app.Command(
				"dropResults",
				c =>
				{
					var (sqlProvider, connectionStringName) = ConfigureArguments(c);
					c.OnExecute(Invoke(sqlProvider, connectionStringName, DropResults));
				}
			);

			app.Command(
				"create",
				c =>
				{
					var (sqlProvider, connectionStringName) = ConfigureArguments(c);
					c.OnExecute(Invoke(sqlProvider, connectionStringName, Create));
				}
			);

			app.Command(
				"migrate",
				c =>
				{
					var (sqlProvider, connectionStringName) = ConfigureArguments(c);
					c.OnExecute(Invoke(sqlProvider, connectionStringName, Migrate));
				}
			);

			app.Command(
				"seed",
				c =>
				{
					var (sqlProvider, connectionStringName) = ConfigureArguments(c);
					c.OnExecute(Invoke(sqlProvider, connectionStringName, Seed));
				}
			);

			app.HelpOption("-h|--help");

			return app.Execute(args);
		}

		private static Func<Task<int>> Invoke(
			CommandOption? sqlProvider,
			CommandArgument? connectionStringName,
			Func<IServiceProvider, Task> action
		)
		{
			return async () =>
			{
				if (!TryGetServiceProvider(sqlProvider, connectionStringName, out var serviceProvider))
				{
					return 1;
				}

				try
				{
					await ScopedExecution(serviceProvider!, action);
					return 0;
				}
				catch (Exception e)
				{
					await Console.Error.WriteLineAsync(e.ToString());
					return 1;
				}
			};
		}

		private static bool TryGetServiceProvider(
			CommandOption? sqlProvider,
			CommandArgument? connectionStringName,
			out IServiceProvider? serviceProvider
		)
		{
			serviceProvider = null;
			if (sqlProvider == null || !sqlProvider.HasValue())
			{
				Console.Error.WriteLine("You have to define the sql provider to use.");
				return false;
			}

			if (!Enum.TryParse(sqlProvider.Value(), out SqlProvider provider))
			{
				Console.Error.WriteLine(
					$"'{sqlProvider.Value() ?? "null"}' is not a valid sql provider. See --help for available providers."
				);

				return false;
			}

			if (string.IsNullOrEmpty(connectionStringName?.Value))
			{
				Console.Error.WriteLine("You have to define the connection string name to use from 'appSettings.json'");
				return false;
			}

			serviceProvider = new ServicesConfiguration(provider, connectionStringName.Value).ServiceProvider;
			return true;
		}

		private static (CommandOption SqlProvider, CommandArgument ConnectionStringName) ConfigureArguments(
			CommandLineApplication application
		)
		{
			return (application.Option(
				"-s|--sql <provider>",
				$"The sql provider to use: [{string.Join(',', Enum.GetNames(typeof(SqlProvider)))}]",
				CommandOptionType.SingleValue
			), application.Argument(
				"<connectionStringName>",
				"The connection string name to use from 'appSettings.json'"
			));
		}

		private static async Task Drop(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Drop");

			try
			{
				var context = serviceProvider.GetRequiredService<ElpidaContext>();

				await context.Database.EnsureDeletedAsync();

				logger.LogInformation("All operations completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to drop the database");
			}
		}

		private static async Task DropResults(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("RemoveResults");

			try
			{
				var context = serviceProvider.GetRequiredService<ElpidaContext>();

				context.BenchmarkResults.RemoveRange(context.BenchmarkResults);
				context.BenchmarkStatistics.RemoveRange(context.BenchmarkStatistics);
				await context.SaveChangesAsync();

				logger.LogInformation("All operations completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to drop the database");
			}
		}

		private static async Task Clean(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Clean");

			try
			{
				var context = serviceProvider.GetRequiredService<ElpidaContext>();

				context.Cpus.RemoveRange(context.Cpus);
				await context.SaveChangesAsync();

				logger.LogInformation("All operations completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to clean the database");
			}
		}

		private static async Task Migrate(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Migrate");

			try
			{
				var context = serviceProvider.GetRequiredService<ElpidaContext>();

				await context.Database.MigrateAsync();

				logger.LogInformation("All operations completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to migrate the database");
			}
		}

		private static async Task Create(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Create");

			try
			{
				var context = serviceProvider.GetRequiredService<ElpidaContext>();

				//await context.Database.EnsureDeletedAsync();
				await context.Database.EnsureCreatedAsync();
				await SeedTasks(serviceProvider, logger);
				await SeedBenchmarks(serviceProvider, logger);

				logger.LogInformation("All operations completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to create the database");
			}
		}

		private static async Task Seed(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Seed");

			try
			{
				await SeedResults(serviceProvider, logger);

				logger.LogInformation("All operations completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to seed the database");
			}
		}

		private static async Task ScopedExecution(
			IServiceProvider serviceProvider,
			Func<IServiceProvider, Task> action
		)
		{
			using var scope = serviceProvider.CreateScope();
			var scopedServiceProvider = scope.ServiceProvider;

			await action(scopedServiceProvider);
		}

		private static async Task ProcessResultAsync(
			IServiceProvider serviceProvider,
			BenchmarkResultsBatchDto benchmarkResultBatchDto,
			CancellationToken cancellationToken
		)
		{
			var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Result seeder");

			try
			{
				var resultService = serviceProvider.GetRequiredService<IBenchmarkResultsService>();

				Interlocked.Add(ref _resultsExpected, benchmarkResultBatchDto.BenchmarkResults.Length);

				await resultService.AddBatchAsync(benchmarkResultBatchDto, cancellationToken);

				Interlocked.Add(ref _resultsProcessed, benchmarkResultBatchDto.BenchmarkResults.Length);

				logger.LogInformation("Successfully added batch with {ResultCount} benchmarks", benchmarkResultBatchDto.BenchmarkResults.Length);

				var processedBatches = Interlocked.Increment(ref _batchesProcessedCount);
				if (processedBatches % 10 == 0)
				{
					logger.LogInformation("Added batches so far: {Added}", processedBatches);
				}
			}
			catch (Exception e)
			{
				logger.LogError(e, "Failed to add result batch");
			}
		}

		private static async Task ProcessBenchmarkAsync(
			IServiceProvider serviceProvider,
			BenchmarkDto benchmarkDto,
			CancellationToken cancellationToken
		)
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
			CancellationToken cancellationToken
		)
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

		private static async Task SeedResults(IServiceProvider serviceProvider, ILogger logger)
		{
			await TimeProcedure(
				ParallelExecutor.ProcessFilesInDirectoryAsync<BenchmarkResultsBatchDto>(
					ResultDataDirectory,
					serviceProvider,
					ProcessResultAsync
				),
				logger
			);

			logger.LogInformation(
				"Finished. Results expected: {Expected}, Results successfully added: {Added}",
				_resultsExpected,
				_resultsProcessed
			);
		}

		private static Task SeedBenchmarks(
			IServiceProvider serviceProvider,
			ILogger logger
		)
		{
			return TimeProcedure(
				Task.Run(
					async () =>
					{
						var taskData =
							JsonConvert.DeserializeObject<List<BenchmarkDto>?>(
								await File.ReadAllTextAsync(Path.Combine(BenchmarkDataDirectory, BenchmarksFile))
							);

						if (taskData != null)
						{
							await ParallelExecutor.ProcessItemsAsync(
								taskData,
								serviceProvider,
								ProcessBenchmarkAsync
							);
						}
					}
				),
				logger
			);
		}

		private static Task SeedTasks(
			IServiceProvider serviceProvider,
			ILogger logger
		)
		{
			return TimeProcedure(
				Task.Run(
					async () =>
					{
						var taskData =
							JsonConvert.DeserializeObject<List<TaskDto>?>(
								await File.ReadAllTextAsync(Path.Combine(BenchmarkDataDirectory, TasksFile))
							);

						if (taskData != null)
						{
							await ParallelExecutor.ProcessItemsAsync(taskData, serviceProvider, ProcessTaskAsync);
						}
					}
				),
				logger
			);
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

		private static ILogger CreateLogger(this IServiceProvider serviceProvider, string category)
		{
			return serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(category);
		}
	}
}