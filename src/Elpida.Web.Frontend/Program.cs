using System;
using System.Net.Http;
using System.Threading.Tasks;
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

			builder.Services.AddScoped(
				sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }
			);

			builder.Services.AddSingleton<ElpidaIconsService>();
			builder.Services.AddSingleton<DownloadService>();

			await builder.Build().RunAsync();
		}
	}
}