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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Elpida.Backend
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	internal class ApiKeyAuthenticationAttribute : Attribute, IAsyncActionFilter
	{
		private const string ApiKeyHeaderName = "api_key";

		public ApiKeyAuthenticationAttribute(string keyName)
		{
			KeyName = keyName;
		}

		public string KeyName { get; }

		public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (IsAuthorized(
				KeyName,
				context.HttpContext.Request.Headers,
				context.HttpContext.RequestServices
			))
			{
				return next();
			}

			context.Result = new UnauthorizedResult();
			return Task.CompletedTask;
		}

		private static bool IsAuthorized(
			string keyName,
			IHeaderDictionary headers,
			IServiceProvider serviceProvider
		)
		{
			if (!headers.TryGetValue(ApiKeyHeaderName, out var key))
			{
				return false;
			}

			var apiKeys = serviceProvider.GetRequiredService<IOptions<ApiKeys>>().Value;

			if (apiKeys.TryGetValue(keyName, out var validKey))
			{
				return validKey.Equals(key);
			}

			throw new ArgumentException("Provided key name does not exist in the configuration!", keyName);
		}
	}
}