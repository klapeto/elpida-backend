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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elpida.Backend.Services.Abstractions.Dtos.Cpu
{
	/// <summary>
	///     Details for a Cpu.
	/// </summary>
	public sealed class CpuDto : FoundationDto
	{
		/// <summary>
		///     The architecture of this Cpu.
		/// </summary>
		/// <example>ARM</example>
		[Required]
		[MaxLength(20)]
		public string Architecture { get; init; }

		/// <summary>
		///     The vendor of this Cpu.
		/// </summary>
		/// <example>ARM</example>
		[Required]
		[MaxLength(50)]
		public string Vendor { get; init; }

		/// <summary>
		///     The model name of this Cpu.
		/// </summary>
		/// <example>Cortex A7</example>
		[Required]
		[MaxLength(50)]
		public string ModelName { get; init; }

		/// <summary>
		///     The frequency of this Cpu.
		/// </summary>
		[Required]
		[Range(0, long.MaxValue)]
		public long Frequency { get; init; }

		/// <summary>
		///     Whether this cpu supports Simultaneously Multi Threading.
		/// </summary>
		[Required]
		public bool Smt { get; init; }

		/// <summary>
		///     Additional cpu specific information of this Cpu.
		/// </summary>
		public IReadOnlyDictionary<string, string>? AdditionalInfo { get; init; }

		/// <summary>
		///     The caches of this cpu.
		/// </summary>
		public CpuCacheDto[]? Caches { get; init; }

		/// <summary>
		///     Features that this Cpu supports.
		/// </summary>
		public string[]? Features { get; init; }
	}
}