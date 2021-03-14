/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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
using Elpida.Backend.Data.Abstractions.Interfaces;

namespace Elpida.Backend.Data.Abstractions.Models.Cpu
{
	public class CpuModel : IEntity
	{
		public string Id { get; set; } = string.Empty;
		public string Vendor { get; set; } = string.Empty;
		public string Brand { get; set; } = string.Empty;
		public long Frequency { get; set; }
		public bool Smt { get; set; }
		public IDictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
		public IList<CpuCacheModel> Caches { get; set; } = new List<CpuCacheModel>();
		public IList<string> Features { get; set; } = new List<string>();
	}
}