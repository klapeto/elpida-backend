using Elpida.Backend.Data;
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Services;
using Elpida.Backend.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
			services.AddSingleton<IResultsRepository, MongoResultsRepository>();
			services.AddTransient<IAssetsRepository, AzureBlobsAssetsRepository>();
			
			services.AddApiVersioning();

			services.AddCors();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder =>
				builder.WithOrigins("https://beta.elpida.dev", "https://elpida.dev").WithMethods("GET"));

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}