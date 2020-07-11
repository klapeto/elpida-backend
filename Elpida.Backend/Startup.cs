using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
			services.AddControllers();

			services.Configure<ElpidaDatabaseSettings>(
				Configuration.GetSection(nameof(ElpidaDatabaseSettings)));

			services.AddSingleton<IElpidaDatabaseSettings>(sp =>
				sp.GetRequiredService<IOptions<ElpidaDatabaseSettings>>().Value);
			
			services.AddScoped<IResultsService, ResultService>();
			services.AddSingleton<IResultsRepository, MongoResultsRepository>();
			services.AddApiVersioning();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}