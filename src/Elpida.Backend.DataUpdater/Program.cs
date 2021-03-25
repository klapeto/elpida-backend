using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Services.Extensions.Benchmark;
using Elpida.Backend.Services.Extensions.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
	public class ElpidaCtx : ElpidaContext
	{
		private string _connectionString = @"Data Source=results.db";
		private ILoggerFactory _loggerFactory;

		public ElpidaCtx(string connectionString, ILoggerFactory loggerFactory)
			: base(new DbContextOptions<ElpidaContext>())
		{
			_connectionString = connectionString;
			_loggerFactory = loggerFactory;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
#if DEBUG
			options.UseLoggerFactory(_loggerFactory);
			options.UseSqlite(_connectionString);
#else
			#error WTF?
#endif
		}
	}

	class Program
	{
		private static LoggerFactory CreateLoggerFactory()
		{
			var loggerFactory = new LoggerFactory();
			loggerFactory.AddProvider(new ConsoleLoggerProvider(
				new OptionsMonitor<ConsoleLoggerOptions>(
					new OptionsFactory<ConsoleLoggerOptions>(new IConfigureOptions<ConsoleLoggerOptions>[0],
						new IPostConfigureOptions<ConsoleLoggerOptions>[0],
						new IValidateOptions<ConsoleLoggerOptions>[0]),
					new IOptionsChangeTokenSource<ConsoleLoggerOptions>[0], new OptionsCache<ConsoleLoggerOptions>())));

			return loggerFactory;
		}

		private static async Task Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile($"appsettings.Development.json", false, true)
				.Build();

			Console.WriteLine(Environment.CurrentDirectory);

			var connectionString = @"Data Source=../../../../Elpida.Backend/results.db";

			using var loggerFactory = CreateLoggerFactory();

			var baseLogger = loggerFactory.CreateLogger("Main");

			baseLogger.LogInformation("Reading data");
			try
			{
				var data = JsonConvert.DeserializeObject<Data>(await File.ReadAllTextAsync("data.json"));

				baseLogger.LogInformation("Updating database");

				using (var context = new ElpidaCtx(connectionString, loggerFactory))
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
							await context.Benchmarks.FirstOrDefaultAsync(t => t.Uuid == benchmark.Uuid);
					
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

					// var query = context.Results
					// 		.Include(m => m.Topology)
					// 		.ThenInclude(m => m.Cpu)
					// 		.Include(m => m.Benchmark)
					// 		.Include(m => m.TaskResults)
					// 		.ThenInclude(m => m.Task);
					//
					// var x = await query.FirstAsync();
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