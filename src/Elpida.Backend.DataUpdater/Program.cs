using System;
using System.IO;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
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
		
		static async Task Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile($"appsettings.Development.json", true, true)
				.Build();

			using var loggerFactory = CreateLoggerFactory();
			
			var baseLogger = loggerFactory.CreateLogger("Main");


			baseLogger.LogInformation("Reading settings");
			var documentSettings = new DocumentRepositorySettings();
			config.GetSection(nameof(DocumentRepositorySettings)).Bind(documentSettings);

			baseLogger.LogInformation("Reading data");
			
			try
			{
				var data = JsonConvert.DeserializeObject<Data>(await File.ReadAllTextAsync("data.json"));
			
				baseLogger.LogInformation("Getting database");
				
				var client = new MongoClient(documentSettings.ConnectionString);
				var database = client.GetDatabase(documentSettings.DatabaseName);
				
				baseLogger.LogInformation("Updating...");
				var dataUpdater = new DataUpdater(data, 
					new MongoBenchmarkRepository(database.GetCollection<BenchmarkModel>(documentSettings.BenchmarksCollectionName)), 
					new MongoTaskRepository(database.GetCollection<TaskModel>(documentSettings.TasksCollectionName)),
					loggerFactory.CreateLogger<DataUpdater>());

				await dataUpdater.EnsureUpdatedAsync(default);
			}
			catch (Exception ex)
			{
				baseLogger.LogCritical("Failed to update:", ex);
				return;
			}
			
			baseLogger.LogInformation("Sucess");
		}
	}
}