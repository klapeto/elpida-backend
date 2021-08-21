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
using System.Threading;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Common.Lock.Redis;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataUpdater
{
	internal class ServicesConfigurator : IDisposable
	{
		public ServicesConfigurator(string[] args)
		{
			var services = new ServiceCollection();

			Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
				.AddCommandLine(args)
				.Build();

			services.AddLogging(
				builder =>
				{
					builder.AddConsole();
					builder.AddConfiguration(Configuration.GetSection("Logging"));
				}
			);

			services.AddTransient<IBenchmarkResultsService, BenchmarkResultService>();
			services.AddTransient<IBenchmarkService, BenchmarkService>();
			services.AddTransient<ICpuService, CpuService>();
			services.AddTransient<IElpidaService, ElpidaService>();
			services.AddTransient<IOsService, OsService>();
			services.AddTransient<IBenchmarkStatisticsService, BenchmarkStatisticsService>();
			services.AddTransient<ITaskService, TaskService>();
			services.AddTransient<ITopologyService, TopologyService>();

			var redisOptions = Configuration.GetSection("Redis");
			if (redisOptions.Exists())
			{
				services.AddRedisLocks();
				services.Configure<RedisOptions>(redisOptions);
			}
			else
			{
				services.AddLocalLocks();
			}

			services.AddTransient<IBenchmarkResultsRepository, BenchmarkResultsRepository>();
			services.AddTransient<ICpuRepository, CpuRepository>();
			services.AddTransient<ITopologyRepository, TopologyRepository>();
			services.AddTransient<IBenchmarkRepository, BenchmarkRepository>();
			services.AddTransient<ITaskRepository, TaskRepository>();
			services.AddTransient<IElpidaRepository, ElpidaRepository>();
			services.AddTransient<IOsRepository, OsRepository>();
			services.AddTransient<IBenchmarkStatisticsRepository, BenchmarkStatisticsRepository>();

			services.AddDbContext<ElpidaContext>(
				builder =>
				{
					builder.UseSqlite(
						Configuration.GetConnectionString("Local"),
						optionsBuilder => optionsBuilder.CommandTimeout(60)
					);
				}
			);

			ServiceProvider = services.BuildServiceProvider();
		}

		public IServiceProvider ServiceProvider { get; }

		public IConfigurationRoot Configuration { get; }

		public void Dispose()
		{
			
		}
	}
}