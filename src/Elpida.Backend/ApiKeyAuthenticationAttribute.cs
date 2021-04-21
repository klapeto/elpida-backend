/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020 Ioannis Panagiotopoulos
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