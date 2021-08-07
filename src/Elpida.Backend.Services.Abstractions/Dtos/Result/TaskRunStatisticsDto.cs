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

namespace Elpida.Backend.Services.Abstractions.Dtos.Result
{
	/// <summary>
	///     Statistics of a Task run result.
	/// </summary>
	public class TaskRunStatisticsDto
	{
		/// <summary>
		///     How many times this task run.
		/// </summary>
		public long SampleSize { get; set; }

		/// <summary>
		///     The maximum value of the result.
		/// </summary>
		public double Max { get; set; }

		/// <summary>
		///     The minimum value of the result.
		/// </summary>
		public double Min { get; set; }

		/// <summary>
		///     The mean value of the result.
		/// </summary>
		public double Mean { get; set; }

		/// <summary>
		///     The standard deviation of the value of the result.
		/// </summary>
		public double Sd { get; set; }

		/// <summary>
		///     The tau of this sample size.
		/// </summary>
		public double Tau { get; set; }

		/// <summary>
		///     The margin of error of the value of the result.
		/// </summary>
		public double MarginOfError { get; set; }
	}
}