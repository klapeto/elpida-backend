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

using Elpida.Backend.Common;

namespace Elpida.Backend.Services.Abstractions.Dtos.Task
{
	/// <summary>
	///     Details of the result of a Task.
	/// </summary>
	public sealed class ResultSpecificationDto
	{
		/// <summary>
		///     The name of the result.
		/// </summary>
		/// <example>Allocation rate</example>
		public string Name { get; init; }

		/// <summary>
		///     The description of the result.
		/// </summary>
		/// <example>The rate the cpu can allocate memory.</example>
		public string Description { get; init; }

		/// <summary>
		///     The unit of the result.
		/// </summary>
		/// <example>B/s</example>
		public string Unit { get; init; }

		/// <summary>
		///     The type of aggregation for this result.
		/// </summary>
		public AggregationType Aggregation { get; init; }

		/// <summary>
		///     The type of this result.
		/// </summary>
		public ResultType Type { get; init; }
	}
}