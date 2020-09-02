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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
			Console.WriteLine($"{string.Join("|", Configuration.AsEnumerable().Select(x => x.Key))}");
			services.AddControllers()
				.AddFluentValidation(configuration =>
				{
					configuration.ImplicitlyValidateChildProperties = true;
					configuration.RegisterValidatorsFromAssemblyContaining<ResultValidator>();
				});

			services.Configure<DocumentRepositorySettings>(
				Configuration.GetSection(nameof(DocumentRepositorySettings)));

			services.Configure<AzureBlobAssetsRepositorySettings>(
				Configuration.GetSection(nameof(AzureBlobAssetsRepositorySettings)));

			services.AddSingleton<IDocumentRepositorySettings>(sp =>
				sp.GetRequiredService<IOptions<DocumentRepositorySettings>>().Value);

			services.AddSingleton<IAssetsRepositorySettings>(sp =>
				sp.GetRequiredService<IOptions<AzureBlobAssetsRepositorySettings>>().Value);

			services.AddScoped<IResultsService, ResultService>();
			services.AddTransient<IAssetsService, AssetsService>();

			services.AddTransient(MongoResultsCollection_ImplementationFactory);

			services.AddTransient<IBlobClientFactory, AzureAssetsBlobClientFactory>();

			services.AddTransient<IResultsRepository, MongoResultsRepository>();
			services.AddTransient<IAssetsRepository, AzureBlobsAssetsRepository>();

			services.AddApiVersioning();

			services.AddCors();
		}

		private static IMongoCollection<ResultModel> MongoResultsCollection_ImplementationFactory(
			IServiceProvider serviceProvider)
		{
			var settings = serviceProvider.GetRequiredService<IDocumentRepositorySettings>();

			if (string.IsNullOrWhiteSpace(settings.ConnectionString))
				throw new ArgumentException("Documents Connection string is empty", nameof(settings.ConnectionString));
			if (string.IsNullOrWhiteSpace(settings.DatabaseName))
				throw new ArgumentException("Database name for documents is empty", nameof(settings.DatabaseName));
			if (string.IsNullOrWhiteSpace(settings.ResultsCollectionName))
				throw new ArgumentException("Collection name for Results documents is empty",
					nameof(settings.ResultsCollectionName));

			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);

			return database.GetCollection<ResultModel>(settings.ResultsCollectionName);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseExceptionHandler(builder => builder.Run(ErrorHandler));

			app.UseCors(builder =>
				builder.WithOrigins("https://beta.elpida.dev", "https://elpida.dev").WithMethods("GET"));

			app.UseHttpsRedirection();

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
				case NotFoundException _:
					context.Response.StatusCode = (int) HttpStatusCode.NotFound;
					break;
				default:
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					break;
			}
		}
	}
}