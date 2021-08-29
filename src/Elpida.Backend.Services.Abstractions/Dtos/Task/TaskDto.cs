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
	///     Details of Task.
	/// </summary>
	public class TaskDto : FoundationDto
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TaskDto" /> class.
		/// </summary>
		/// <param name="id">The id of the Task.</param>
		/// <param name="uuid">The UUID of this Task.</param>
		/// <param name="name">The name of this Task.</param>
		/// <param name="description">The description of this Task.</param>
		/// <param name="result">The result specification of this Task.</param>
		/// <param name="input">The input data specification of this Task.</param>
		/// <param name="output">The output data specification of this Task.</param>
		public TaskDto(
			long id,
			Guid uuid,
			string name,
			string description,
			ResultSpecificationDto result,
			DataSpecificationDto? input,
			DataSpecificationDto? output
		)
			: base(id)
		{
			Uuid = uuid;
			Name = name;
			Description = description;
			Result = result;
			Input = input;
			Output = output;
		}

		/// <summary>
		///     The UUID of this Task.
		/// </summary>
		public Guid Uuid { get; }

		/// <summary>
		///     The name of this Task.
		/// </summary>
		/// <example>Allocate Memory</example>
		public string Name { get; }

		/// <summary>
		///     The description of this Task.
		/// </summary>
		/// <example>Allocated memory to be used by the next Tasks.</example>
		public string Description { get; }

		/// <summary>
		///     The result specification of this Task.
		/// </summary>
		public ResultSpecificationDto Result { get; }

		/// <summary>
		///     The input data specification of this Task.
		/// </summary>
		public DataSpecificationDto? Input { get; }

		/// <summary>
		///     The output data specification of this Task.
		/// </summary>
		public DataSpecificationDto? Output { get; }
	}
}