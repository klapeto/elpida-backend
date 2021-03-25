/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions.Exceptions;
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

namespace Elpida.Backend
{
	public class Startup
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
				.AddFluentValidation(configuration =>
				{
					configuration.ImplicitlyValidateChildProperties = true;
					configuration.RegisterValidatorsFromAssemblyContaining<ResultValidator>();
				});

			services.AddScoped<IResultsService, ResultService>();

			services.AddTransient<IResultsRepository, ResultsRepository>();
			services.AddTransient<ICpuRepository, CpuRepository>();
			services.AddTransient<ITopologyRepository, TopologyRepository>();
			services.AddTransient<IBenchmarkRepository, BenchmarkRepository>();
			services.AddTransient<ITaskRepository, TaskRepository>();

			services.AddDbContext<ElpidaContext>(builder =>
			{
#if DEBUG
				builder.UseSqlite("Data Source=results.db",
					b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
#else
				// Use something else, eg SQL Server
				#error
#endif
			});

			services.AddApiVersioning();

			services.AddCors();
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

			app.UseCors(builder =>
				builder.WithOrigins("https://beta.elpida.dev",
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
		}

		private static async Task ErrorHandler(HttpContext context)
		{
			var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

			context.Response.ContentType = "text/plain";

			switch (exceptionHandlerPathFeature.Error)
			{
				case ArgumentException ae:
					context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
					await context.Response.WriteAsync($"{ae.Message}: '{ae.ParamName}'");
					break;
				case ConflictException ce:
					context.Response.StatusCode = (int) HttpStatusCode.Conflict;
					await context.Response.WriteAsync($"Conflict detected for id: '{ce.Id}' Reason: {ce.Message}");
					break;
				case NotFoundException _:
					context.Response.StatusCode = (int) HttpStatusCode.NotFound;
					break;
				case CorruptedRecordException cre:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					await context.Response.WriteAsync(
						$"The requested record is corrupted!. Please report this to Elpida Backend repository along with this id: {cre.Id}");
					break;
				default:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					break;
			}
		}
	}
}