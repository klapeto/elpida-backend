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
using System.Text.Json;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend
{
	internal static class QueryRequestUtilities
	{
		public static void PreprocessQuery(QueryRequest queryRequest)
		{
			if (queryRequest.Filters == null)
			{
				return;
			}

			foreach (var queryRequestFilter in queryRequest.Filters)
			{
				ConvertValues(queryRequestFilter);
			}
		}

		private static void ConvertValues(QueryInstance instance)
		{
			if (instance.Value == null)
			{
				return;
			}

			var element = (JsonElement)instance.Value;
			switch (element.ValueKind)
			{
				case JsonValueKind.String:
					instance.Value = element.GetString();
					break;
				case JsonValueKind.Number:
					instance.Value = element.GetDouble();
					break;
				case JsonValueKind.False:
				case JsonValueKind.True:
					instance.Value = element.GetBoolean();
					break;
				case JsonValueKind.Null:
				case JsonValueKind.Undefined:
					instance.Value = null;
					break;
				case JsonValueKind.Object:
				case JsonValueKind.Array:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}