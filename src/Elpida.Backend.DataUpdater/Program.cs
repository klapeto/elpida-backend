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
using System.Reflection;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.DataSeed;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataUpdater
{
	internal static class Program
	{
		private static readonly DbUpdater Updater = new (Environment.CurrentDirectory);

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
				c => c.OnExecute(Invoke(Clean))
			);

			app.Command(
				"drop",
				c => c.OnExecute(Invoke(Drop))
			);

			app.Command(
				"dropResults",
				c => c.OnExecute(Invoke(DropResults))
			);

			app.Command(
				"create",
				c => c.OnExecute(Invoke(Create))
			);

			app.Command(
				"migrate",
				c => c.OnExecute(Invoke(Updater.MigrateAsync))
			);

			app.Command(
				"seed",
				c => c.OnExecute(Invoke(Updater.SeedResultsAsync))
			);

			app.HelpOption("-h|--help");

			return app.Execute(args);
		}

		private static Func<Task<int>> Invoke(Func<IServiceProvider, Task> action)
		{
			return async () =>
			{
				try
				{
					await ScopedExecution(GetServiceProvider(), action);
					return 0;
				}
				catch (Exception e)
				{
					await Console.Error.WriteLineAsync(e.ToString());
					return 1;
				}
			};
		}

		private static IServiceProvider GetServiceProvider()
		{
			var services = new ServiceCollection();

			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile("appsettings.Development.json", true, true)
				.Build();

			services.AddLogging(
				builder =>
				{
					builder.AddSimpleConsole(b => b.IncludeScopes = true);
					builder.AddConfiguration(configuration.GetSection("Logging"));
				}
			);

			new Startup(configuration).ConfigureServices(services);

			return services.BuildServiceProvider();
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

		private static async Task Create(IServiceProvider serviceProvider)
		{
			await Updater.MigrateAsync(serviceProvider);
			await Updater.SeedBenchmarksAsync(serviceProvider);
			await Updater.SeedResultsAsync(serviceProvider);
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
	}
}