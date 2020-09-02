using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elpida.Backend
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ApiKeyAuthenticationAttribute : Attribute, IAsyncActionFilter
	{
		private const string ApiKeyHeaderName = "api_key";
		public string KeyName { get; set; }

		#region IAsyncActionFilter Members

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var key))
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			var validKey = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>()
				.GetValue<string>(KeyName);
			if (validKey == null)
				throw new ArgumentException("Provided key name does not exist in the configuration!", KeyName);

			if (!validKey.Equals(key))
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			await next();
		}

		#endregion
	}
}