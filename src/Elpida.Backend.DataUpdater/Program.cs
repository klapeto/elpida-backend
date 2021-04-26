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
using System.IO;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataUpdater
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var servicesConfiguration = new ServicesConfigurator(args);
            var serviceProvider = servicesConfiguration.ServiceProvider;

            var baseLogger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Main");
            
            try
            {
                using (baseLogger.BeginScope("Database update"))
                {
                    var context = serviceProvider.GetRequiredService<ElpidaContext>();

                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();

                    var benchmarkSeeder = new BenchmarkDataSeeder(serviceProvider);

                    await benchmarkSeeder.SeedAsync(new [] {"benchmark-data.json"});

                    var resultSeeder = new ResultDataSeeder(serviceProvider);

                    await resultSeeder.SeedAsync(Directory.EnumerateFiles("ResultData"));
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