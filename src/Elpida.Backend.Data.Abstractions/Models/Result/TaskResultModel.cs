/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020  Ioannis Panagiotopoulos
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

using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models.Task;

namespace Elpida.Backend.Data.Abstractions.Models.Result
{
	public class TaskResultModel : Entity
	{
		public ResultModel Result { get; set; } = default!;
		public TaskModel Task { get; set; } = default!;
		public double Value { get; set; }
		public double Time { get; set; }
		public long InputSize { get; set; }
		public long SampleSize { get; set; }
		public double Max { get; set; }
		public double Min { get; set; }
		public double Mean { get; set; }
		public double StandardDeviation { get; set; }
		public double Tau { get; set; }
		public double MarginOfError { get; set; }
	}
}