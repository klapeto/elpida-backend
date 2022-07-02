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
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Elpida.Backend
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }

		public IWebHostEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddScoped<IBenchmarkResultService, BenchmarkResultService>();
			services.AddScoped<IBenchmarkService, BenchmarkService>();
			services.AddScoped<ICpuService, CpuService>();
			services.AddScoped<IElpidaVersionService, ElpidaVersionService>();
			services.AddScoped<IOperatingSystemService, OperatingSystemService>();
			services.AddScoped<IBenchmarkStatisticsService, BenchmarkStatisticsService>();
			services.AddScoped<ITaskService, TaskService>();
			services.AddScoped<ITopologyService, TopologyService>();

			services.AddTransient<IBenchmarkResultRepository, BenchmarkResultRepository>();
			services.AddTransient<ICpuRepository, CpuRepository>();
			services.AddTransient<ITopologyRepository, TopologyRepository>();
			services.AddTransient<IBenchmarkRepository, BenchmarkRepository>();
			services.AddTransient<ITaskRepository, TaskRepository>();
			services.AddTransient<IElpidaVersionRepository, ElpidaVersionRepository>();
			services.AddTransient<IOperatingSystemRepository, OperatingSystemRepository>();
			services.AddTransient<IBenchmarkStatisticsRepository, BenchmarkStatisticsRepository>();

			services.AddDbContext<ElpidaContext>(
				builder =>
				{
					if (Environment.IsDevelopment())
					{
						var path = Path.Combine(Path.GetTempPath(), "Elpida");
						Directory.CreateDirectory(path);
						builder.UseSqlite(
							$"Data Source={Path.Combine(path, "ElpidaDB.db")}",
							b => b.MigrationsAssembly("Elpida.Backend.Data.Sqlite")
						);
					}
					else
					{
						builder.UseSqlServer(
							Configuration.GetConnectionString("ElpidaDB"),
							b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
						);
					}
				}
			);

			services.Configure<ApiKeys>(Configuration.GetSection("ApiKeys"));

			services.AddCors();

			services.AddSwaggerGen(
				c =>
				{
					c.SwaggerDoc("v1", new OpenApiInfo { Title = "Elpida HTTP Rest Api", Version = "1" });

					var assembly = Assembly.GetExecutingAssembly();

					foreach (var assemblyName in assembly
						.GetReferencedAssemblies()
						.Where(a => a.Name?.Contains("Elpida") ?? false)
						.Concat(new[] { assembly.GetName() }))
					{
						var docFile = $"{assemblyName.Name}.xml";

						var filePath = Path.Combine(AppContext.BaseDirectory, docFile);
						c.IncludeXmlComments(filePath);
					}
				}
			);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHttpsRedirection();
				app.UseExceptionHandler(builder => builder.Run(ErrorHandler));
			}

			app.UseCors(
				builder =>
				{
					if (env.IsProduction())
					{
						builder.WithOrigins(
							"https://www.elpida.dev"
						);
					}
					else if (env.IsStaging())
					{
						builder.WithOrigins(
							"https://staging.elpida.dev"
						);
					}
					else
					{
						builder.WithOrigins(
							"*"
						);
					}

					builder.WithMethods(HttpMethods.Get, HttpMethods.Post)
						.WithHeaders(HeaderNames.ContentType, HeaderNames.Accept)
						.WithExposedHeaders(
							HeaderNames.ContentLength,
							HeaderNames.ContentRange
						);
				}
			);

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());

			app.UseSwagger();

			app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "Elpida HTTP Rest Api V1"));
		}

		private static async Task ErrorHandler(HttpContext context)
		{
			var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

			context.Response.ContentType = "text/plain";

			switch (exceptionHandlerPathFeature.Error)
			{
				case NotFoundException nfe:
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					await context.Response.WriteAsync($"The resource with id '{nfe.Id}' was not found");
					break;
				default:
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}
		}
	}
}