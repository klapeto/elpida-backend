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
	internal class ServicesConfiguration
	{
		public ServicesConfiguration(SqlProvider provider, string connectionStringName)
		{
			var services = new ServiceCollection();

			Configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile("appsettings.Development.json", true, true)
				.Build();

			services.AddLogging(
				builder =>
				{
					builder.AddSimpleConsole(b => b.IncludeScopes = true);
					builder.AddConfiguration(Configuration.GetSection("Logging"));
				}
			);

			services.AddTransient<IBenchmarkResultsService, BenchmarkResultService>();
			services.AddTransient<IBenchmarkService, BenchmarkService>();
			services.AddTransient<ICpuService, CpuService>();
			services.AddTransient<IElpidaVersionService, ElpidaVersionService>();
			services.AddTransient<IOsService, OsService>();
			services.AddTransient<IBenchmarkStatisticsService, BenchmarkStatisticsService>();
			services.AddTransient<ITaskService, TaskService>();
			services.AddTransient<ITopologyService, TopologyService>();

			services.AddTransient<IBenchmarkResultsRepository, BenchmarkResultsRepository>();
			services.AddTransient<ICpuRepository, CpuRepository>();
			services.AddTransient<ITopologyRepository, TopologyRepository>();
			services.AddTransient<IBenchmarkRepository, BenchmarkRepository>();
			services.AddTransient<ITaskRepository, TaskRepository>();
			services.AddTransient<IElpidaVersionRepository, ElpidaVersionRepository>();
			services.AddTransient<IOsRepository, OsRepository>();
			services.AddTransient<IBenchmarkStatisticsRepository, BenchmarkStatisticsRepository>();

			services.AddDbContext<ElpidaContext>(
				builder =>
				{
					switch (provider)
					{
						case SqlProvider.Sqlite:
							builder.UseSqlite(Configuration.GetConnectionString(connectionStringName));
							break;
						case SqlProvider.SqlServer:
							builder.UseSqlServer(Configuration.GetConnectionString(connectionStringName), b => b.CommandTimeout(120));
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
					}
				}
			);

			ServiceProvider = services.BuildServiceProvider();
		}

		public IServiceProvider ServiceProvider { get; }

		private IConfigurationRoot Configuration { get; }
	}
}