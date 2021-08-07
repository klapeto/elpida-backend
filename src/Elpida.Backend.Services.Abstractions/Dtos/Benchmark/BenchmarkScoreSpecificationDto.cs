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

using Elpida.Backend.Common;

namespace Elpida.Backend.Services.Abstractions.Dtos.Benchmark
{
	/// <summary>
	///     Specification details for a Benchmark score.
	/// </summary>
	public class BenchmarkScoreSpecificationDto
	{
		/// <summary>
		///     The uint of this Benchmark's score.
		/// </summary>
		/// <example>Pixels/s</example>
		public string Unit { get; set; } = default!;

		/// <summary>
		///     The comparison type of this Benchmark's score.
		/// </summary>
		public ValueComparison Comparison { get; set; }
	}
}