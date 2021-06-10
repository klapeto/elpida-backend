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

using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions
{
	public class PagedResult<T>
	{
		public PagedResult(IList<T> list, PageRequest pageRequest)
		{
			Count = list.Count;
			List = list;
			TotalCount = pageRequest.TotalCount;
			NextPage = list.Count == pageRequest.Count
				? new PageRequest { Count = pageRequest.Count, Next = pageRequest.Next + list.Count }
				: null;
		}

		public IList<T> List { get; set; }

		public int Count { get; set; }

		public long TotalCount { get; set; }

		public PageRequest? NextPage { get; set; }

		public static PagedResult<T> Empty()
		{
			return new PagedResult<T>(new List<T>(), new PageRequest());
		}
	}
}