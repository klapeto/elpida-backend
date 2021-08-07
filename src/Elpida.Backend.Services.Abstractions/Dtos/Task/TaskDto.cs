/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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

namespace Elpida.Backend.Services.Abstractions.Dtos.Task
{
	/// <summary>
	///     Details of Task.
	/// </summary>
	public class TaskDto : FoundationDto
	{
		/// <summary>
		///     The UUID of this Task.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		///     The name of this Task.
		/// </summary>
		/// <example>Allocate Memory</example>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		///     The description of this Task.
		/// </summary>
		/// <example>Allocated memory to be used by the next Tasks.</example>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		///     The result specification of this Task.
		/// </summary>
		public ResultSpecificationDto Result { get; set; } = new ();

		/// <summary>
		///     The input data specification of this Task.
		/// </summary>
		public DataSpecificationDto? Input { get; set; }

		/// <summary>
		///     The output data specification of this Task.
		/// </summary>
		public DataSpecificationDto? Output { get; set; }
	}
}