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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Elpida.Backend.DataSeed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateHostBuilder(args)
				.Build();

			await UpdateDbAsync(host);
			await host.RunAsync();
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureLogging(builder => builder.AddConsole())
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
		}

		private static async Task UpdateDbAsync(IHost host)
		{
			using var scope = host.Services.CreateScope();

			var services = scope.ServiceProvider;
			try
			{
				var updater = new DbUpdater(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
				await updater.MigrateAsync(services);
				if (await updater.NeedsBenchmarkSeedingAsync(services))
				{
					await updater.SeedBenchmarksAsync(services);
				}
			}
			catch (Exception ex)
			{
				services
					.GetRequiredService<ILogger<Program>>()
					.LogError(ex, "An error occurred updating the database");

				throw;
			}
		}
	}
}