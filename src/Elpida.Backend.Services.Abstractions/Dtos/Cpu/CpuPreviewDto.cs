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

namespace Elpida.Backend.Services.Abstractions.Dtos.Cpu
{
	/// <summary>
	///     Represents a Cpu.
	/// </summary>
	public sealed class CpuPreviewDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="CpuPreviewDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Cpu.</param>
		/// <param name="vendor">The vendor name of the cpu.</param>
		/// <param name="modelName">The model name of the cpu.</param>
		public CpuPreviewDto(
			long id,
			string vendor,
			string modelName
		)
			: base(id)
		{
			Vendor = vendor;
			ModelName = modelName;
		}

		/// <summary>
		///     The vendor name of the cpu.
		/// </summary>
		/// <example>ARM</example>
		public string Vendor { get; }

		/// <summary>
		///     The model name of the cpu.
		/// </summary>
		/// <example>Cortex A7</example>
		public string ModelName { get; }
	}
}