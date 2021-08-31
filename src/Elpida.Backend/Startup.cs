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
using Elpida.Backend.Validators;
using FluentValidation.AspNetCore;
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
	internal class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddFluentValidation(
					configuration =>
					{
						configuration.ImplicitlyValidateChildProperties = true;
						configuration.RegisterValidatorsFromAssemblyContaining<BenchmarkResultSlimDtoValidator>();
					}
				);

			services.AddScoped<IBenchmarkResultsService, BenchmarkResultService>();
			services.AddScoped<IBenchmarkService, BenchmarkService>();
			services.AddScoped<ICpuService, CpuService>();
			services.AddScoped<IElpidaService, ElpidaService>();
			services.AddScoped<IOsService, OsService>();
			services.AddScoped<IBenchmarkStatisticsService, BenchmarkStatisticsService>();
			services.AddScoped<ITaskService, TaskService>();
			services.AddScoped<ITopologyService, TopologyService>();

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
#if DEBUG
					builder.UseSqlite(
						"Data Source=results.db",
						b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
					);
#else
				// Use something else, eg SQL Server
				#error
#endif
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
					builder.WithOrigins(
							"https://beta.elpida.dev",
							"https://elpida.dev",
							"https://www.elpida.dev"
						)
						.WithMethods(HttpMethods.Get, HttpMethods.Post)
						.WithHeaders(HeaderNames.ContentType, HeaderNames.Accept)
						.WithExposedHeaders(
							HeaderNames.ContentLength,
							HeaderNames.ContentRange
						)
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
				case ArgumentException ae:
					context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
					await context.Response.WriteAsync($"{ae.Message}: '{ae.ParamName}'");
					break;
				case NotFoundException _:
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					break;
				default:
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					break;
			}
		}
	}
}