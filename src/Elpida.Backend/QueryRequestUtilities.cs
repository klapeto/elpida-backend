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
using System.Linq;
using System.Text.Json;
using Elpida.Backend.Services.Abstractions;

namespace Elpida.Backend
{
	internal static class QueryRequestUtilities
	{
		public static QueryRequest PreProcessQuery(QueryRequest queryRequest)
		{
			if (queryRequest.Filters == null)
			{
				return queryRequest;
			}

			return new QueryRequest(
				queryRequest.PageRequest,
				queryRequest.Filters.Select(ConvertValues).ToArray(),
				queryRequest.OrderBy,
				queryRequest.Descending
			);
		}

		private static FilterInstance ConvertValues(FilterInstance instance)
		{
			var element = (JsonElement)instance.Value;
			switch (element.ValueKind)
			{
				case JsonValueKind.String:
					return DateTime.TryParse(element.GetString(), out var date)
						? new FilterInstance(instance.Name, date, instance.Comparison)
						: new FilterInstance(
							instance.Name,
							element.GetString() ?? throw new ArgumentException("The filter value cannot be null"),
							instance.Comparison
						);
				case JsonValueKind.Number:
					return new FilterInstance(instance.Name, element.GetDouble(), instance.Comparison);
				case JsonValueKind.False:
				case JsonValueKind.True:
					return new FilterInstance(instance.Name, element.GetBoolean(), instance.Comparison);
				case JsonValueKind.Object:
				case JsonValueKind.Array:
					return instance;
				default:
					throw new ArgumentOutOfRangeException(
						nameof(instance),
						$"The JSON member type of '{element.ValueKind}' is not acceptable"
					);
			}
		}
	}
}