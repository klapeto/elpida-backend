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
using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Cpu
{
	/// <summary>
	///     Details for a Cpu.
	/// </summary>
	[Serializable]
	public class CpuDto : FoundationDto
	{
		/// <summary>
		///     The architecture of this Cpu.
		/// </summary>
		/// <example>ARM</example>
		public string Architecture { get; set; } = string.Empty;

		/// <summary>
		///     The vendor of this Cpu.
		/// </summary>
		/// <example>ARM</example>
		public string Vendor { get; set; } = string.Empty;

		/// <summary>
		///     The model name of this Cpu.
		/// </summary>
		/// <example>Cortex A7</example>
		public string ModelName { get; set; } = string.Empty;

		/// <summary>
		///     The frequency of this Cpu.
		/// </summary>
		public long Frequency { get; set; }

		/// <summary>
		///     Whether this cpu supports Simultaneously Mutli Threading.
		/// </summary>
		public bool Smt { get; set; }

		/// <summary>
		///     Additional cpu specific information of this Cpu.
		/// </summary>
		public IDictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();

		/// <summary>
		///     The caches of this cpu.
		/// </summary>
		public IList<CpuCacheDto> Caches { get; set; } = new List<CpuCacheDto>();

		/// <summary>
		///     Features that this Cpu supports.
		/// </summary>
		public IList<string> Features { get; set; } = new List<string>();
	}
}