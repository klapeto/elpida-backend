using System;
using System.Net.Http;
using System.Threading.Tasks;
using Elpida.Web.Frontend.Interfaces;
using Elpida.Web.Frontend.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Elpida.Web.Frontend
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			if (builder.HostEnvironment.IsDevelopment())
			{
				builder.Services.AddScoped(
					sp => new HttpClient
					{
						BaseAddress = new Uri("http://localhost:5002/api/v1/"),
					}
				);
			}
			else if (builder.HostEnvironment.IsStaging())
			{
				builder.Services.AddScoped(
					sp => new HttpClient
					{
						BaseAddress = new Uri("https://staging.api.elpida.dev/api/v1/"),
					}
				);
			}
			else
			{
				builder.Services.AddScoped(
					sp => new HttpClient
					{
						BaseAddress = new Uri("https://api.elpida.dev/api/v1/"),
					}
				);
			}

			builder.Services.AddSingleton<ElpidaIconsService>();
			builder.Services.AddSingleton<DownloadService>();

			builder.Services.AddScoped<ResultsFrontEndService>();
			builder.Services.AddScoped<IFrontEndCpuService, CpuFrontEndService>();

			await builder.Build().RunAsync();
		}
	}
}