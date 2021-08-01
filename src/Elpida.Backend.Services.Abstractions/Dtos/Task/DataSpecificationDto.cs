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

using System.Collections.Generic;

namespace Elpida.Backend.Services.Abstractions.Dtos.Task
{
	/// <summary>
	/// Details of data used by a Task.
	/// </summary>
	public class DataSpecificationDto
	{
		/// <summary>
		/// The name of the data.
		/// </summary>
		/// <example>Allocated data</example>
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// The description of the data.
		/// </summary>
		/// <example>The memory that was allocated.</example>
		public string Description { get; set; } = string.Empty;

		/// <summary>
		/// The unit that describes this data.
		/// </summary>
		/// <example>Bytes</example>
		public string Unit { get; set; } = string.Empty;

		/// <summary>
		/// The required property names that this data has to carry in order
		/// to be valid.
		/// </summary>
		public IList<string> RequiredProperties { get; set; } = new List<string>();
	}
}