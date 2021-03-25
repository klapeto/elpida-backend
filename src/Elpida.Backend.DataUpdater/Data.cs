/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.DataUpdater
{
	internal class BenchmarkData
	{
		public Guid Uuid { get; set; }
		public string Name { get; set; } = string.Empty;

		public IReadOnlyList<Guid> TaskSpecifications { get; set; } = Array.Empty<Guid>();
	}

	internal class Data
	{
		public IReadOnlyList<BenchmarkData> Benchmarks { get; set; } = Array.Empty<BenchmarkData>();
		public IReadOnlyList<TaskDto> Tasks { get; set; } = Array.Empty<TaskDto>();
	}
}