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

using System.ComponentModel.DataAnnotations;

namespace Elpida.Backend.Services.Abstractions.Dtos.Cpu
{
	/// <summary>
	///     Details of a Cpu cache.
	/// </summary>
	public sealed class CpuCacheDto
	{
		/// <summary>
		///     The name of this cache.
		/// </summary>
		/// <example>L1D</example>
		[Required]
		[MaxLength(50)]
		public string Name { get; init; }

		/// <summary>
		///     The associativity of this cache.
		/// </summary>
		/// <example>8-Way</example>
		[Required]
		[MaxLength(50)]
		public string Associativity { get; init; }

		/// <summary>
		///     The size of the cache in bytes.
		/// </summary>
		[Required]
		[Range(0, long.MaxValue)]
		public long Size { get; init; }

		/// <summary>
		///     The size of the line.
		/// </summary>
		[Required]
		[Range(0, long.MaxValue)]
		public int LineSize { get; init; }
	}
}