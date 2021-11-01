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

namespace Elpida.Backend.Services.Abstractions.Dtos.Task
{
	/// <summary>
	///     A preview of a Task of a Benchmark.
	/// </summary>
	public class TaskPreviewDto : FoundationDto
	{
		/// <summary>
		///     The UUID of this Task.
		/// </summary>
		public Guid Uuid { get; init; }

		/// <summary>
		///     The name of this Task.
		/// </summary>
		/// <example>Allocate Memory</example>
		public string Name { get; init; }

		/// <summary>
		///     The unit of the result.
		/// </summary>
		/// <example>B/s</example>
		public string ResultUnit { get; init; }
	}
}