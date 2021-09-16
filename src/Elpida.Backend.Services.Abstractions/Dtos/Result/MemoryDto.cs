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

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Details of the memory of a system.
	/// </summary>
	public sealed class MemoryDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="MemoryDto" /> class.
		/// </summary>
		/// <param name="totalSize">The total size of the memory.</param>
		/// <param name="pageSize">The page size of this system.</param>
		public MemoryDto(long totalSize, long pageSize)
		{
			TotalSize = totalSize;
			PageSize = pageSize;
		}

		/// <summary>
		///     The total size of the memory.
		/// </summary>
		public long TotalSize { get; }

		/// <summary>
		///     The page size of this system.
		/// </summary>
		public long PageSize { get; }
	}
}