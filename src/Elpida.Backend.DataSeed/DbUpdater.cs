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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Dtos.Task;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataSeed
{
	public class DbUpdater
	{
		private const string TasksFile = "tasks.json";
		private const string BenchmarkDataDirectory = "BenchmarkData";
		private const string ResultDataDirectory = "ResultData";
		private const string BenchmarksFile = "benchmarks.json";

		private readonly string _contentRoot;
		private int _batchesProcessedCount;
		private int _resultsExpected;
		private int _resultsProcessed;

		public DbUpdater(string contentRoot)
		{
			_contentRoot = contentRoot;
		}

		public async Task MigrateAsync(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.GetRequiredService<ILogger<DbUpdater>>();

			try
			{
				logger.LogInformation("Migrating database...");

				var context = serviceProvider.GetRequiredService<ElpidaContext>();

				await context.Database.MigrateAsync();

				logger.LogInformation("Database migrated successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to migrate the database");
				throw;
			}
		}

		public async Task<bool> NeedsBenchmarkSeedingAsync(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<ElpidaContext>();
			return !await context.Benchmarks.AnyAsync();
		}

		public async Task SeedResultsAsync(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Results seed");

			try
			{
				await ParallelExecutor.ProcessFilesInDirectoryAsync<ResultBatchDto>(
						Path.Combine(_contentRoot, ResultDataDirectory),
						serviceProvider,
						ProcessResultAsync
					)
					.TimeProcedure(logger, "Results seed");

				logger.LogInformation(
					"Finished. Results expected: {Expected}, Results successfully added: {Added}",
					_resultsExpected,
					_resultsProcessed
				);
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to seed the database");
				throw;
			}
		}

		public async Task SeedBenchmarksAsync(IServiceProvider serviceProvider)
		{
			var logger = serviceProvider.CreateLogger("Benchmarks seed");

			try
			{
				await AddTasksAsync(serviceProvider)
					.TimeProcedure(logger, "Task seed");

				await AddBenchmarks(serviceProvider)
					.TimeProcedure(logger, "Benchmark seed");

				logger.LogInformation("Benchmarks seed completed successfully");
			}
			catch (Exception ex)
			{
				logger.LogCritical(ex, "Failed to seed the database");
				throw;
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
				throw;
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
				throw;
			}
		}

		private async Task AddBenchmarks(IServiceProvider serviceProvider)
		{
			var benchmarkFilename = Path.Combine(_contentRoot, BenchmarkDataDirectory, BenchmarksFile);
			var benchmarkData =
				JsonConvert.DeserializeObject<List<BenchmarkDto>?>(
					await File.ReadAllTextAsync(benchmarkFilename)
				);

			if (benchmarkData == null)
			{
				throw new InvalidOperationException(
					$"Benchmarks file has invalid data. File:'{benchmarkFilename}'"
				);
			}

			await ParallelExecutor.ProcessItemsAsync(benchmarkData, serviceProvider, ProcessBenchmarkAsync);
		}

		private async Task AddTasksAsync(IServiceProvider serviceProvider)
		{
			var tasksFilename = Path.Combine(_contentRoot, BenchmarkDataDirectory, TasksFile);
			var taskData =
				JsonConvert.DeserializeObject<List<TaskDto>?>(
					await File.ReadAllTextAsync(tasksFilename)
				);

			if (taskData == null)
			{
				throw new InvalidOperationException(
					$"Tasks file has invalid data. File: '{tasksFilename}'"
				);
			}

			await ParallelExecutor.ProcessItemsAsync(taskData, serviceProvider, ProcessTaskAsync);
		}

		private async Task ProcessResultAsync(
			IServiceProvider serviceProvider,
			ResultBatchDto resultBatchDto,
			CancellationToken cancellationToken
		)
		{
			var logger = serviceProvider.CreateLogger("Result seeder");

			try
			{
				var resultService = serviceProvider.GetRequiredService<IResultService>();

				Interlocked.Add(ref _resultsExpected, resultBatchDto.BenchmarkResults.Length);

				await resultService.AddBatchAsync(resultBatchDto, cancellationToken);

				Interlocked.Add(ref _resultsProcessed, resultBatchDto.BenchmarkResults.Length);

				logger.LogInformation(
					"Successfully added batch with {ResultCount} benchmarks",
					resultBatchDto.BenchmarkResults.Length
				);

				var processedBatches = Interlocked.Increment(ref _batchesProcessedCount);
				if (processedBatches % 10 == 0)
				{
					logger.LogInformation("Added batches so far: {Added}", processedBatches);
				}
			}
			catch (Exception e)
			{
				logger.LogError(e, "Failed to add result batch");
				throw;
			}
		}
	}
}