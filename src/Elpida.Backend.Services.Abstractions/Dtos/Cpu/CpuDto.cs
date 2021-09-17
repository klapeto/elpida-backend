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
		///     Initializes a new instance of the <see cref="CpuDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Cpu.</param>
		/// <param name="architecture">The architecture of this Cpu.</param>
		/// <param name="vendor">The vendor of this Cpu.</param>
		/// <param name="modelName">The model name of this Cpu.</param>
		/// <param name="frequency">The frequency of this Cpu.</param>
		/// <param name="smt">Whether this cpu supports Simultaneously Multi Threading.</param>
		/// <param name="additionalInfo">Additional cpu specific information of this Cpu.</param>
		/// <param name="caches">The caches of this cpu.</param>
		/// <param name="features">Features that this Cpu supports.</param>
		public CpuDto(
			long id,
			string architecture,
			string vendor,
			string modelName,
			long frequency,
			bool smt,
			IReadOnlyDictionary<string, string>? additionalInfo,
			CpuCacheDto[]? caches,
			string[]? features
		)
			: base(id)
		{
			Architecture = architecture;
			Vendor = vendor;
			ModelName = modelName;
			Frequency = frequency;
			Smt = smt;
			AdditionalInfo = additionalInfo;
			Caches = caches;
			Features = features;
		}

		/// <summary>
		///     The architecture of this Cpu.
		/// </summary>
		/// <example>ARM</example>
		[Required]
		[MaxLength(20)]
		public string Architecture { get; }

		/// <summary>
		///     The vendor of this Cpu.
		/// </summary>
		/// <example>ARM</example>
		[Required]
		[MaxLength(50)]
		public string Vendor { get; }

		/// <summary>
		///     The model name of this Cpu.
		/// </summary>
		/// <example>Cortex A7</example>
		[Required]
		[MaxLength(50)]
		public string ModelName { get; }

		/// <summary>
		///     The frequency of this Cpu.
		/// </summary>
		[Required]
		[Range(0, long.MaxValue)]
		public long Frequency { get; }

		/// <summary>
		///     Whether this cpu supports Simultaneously Multi Threading.
		/// </summary>
		[Required]
		public bool Smt { get; }

		/// <summary>
		///     Additional cpu specific information of this Cpu.
		/// </summary>
		public IReadOnlyDictionary<string, string>? AdditionalInfo { get; }

		/// <summary>
		///     The caches of this cpu.
		/// </summary>
		public CpuCacheDto[]? Caches { get; }

		/// <summary>
		///     Features that this Cpu supports.
		/// </summary>
		public string[]? Features { get; }
	}
}