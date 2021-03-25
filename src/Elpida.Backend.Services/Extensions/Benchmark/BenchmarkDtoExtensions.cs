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

using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services.Extensions.Benchmark
{
	public static class BenchmarkDtoExtensions
	{
		public static BenchmarkDto ToDto(this BenchmarkModel model)
		{
			return new BenchmarkDto
			{
				Id = model.Id,
				Uuid = model.Uuid,
				Name = model.Name,
				TaskSpecifications = model.Tasks.Select(t => t.ToDto()).ToList()
			};
		}

		public static void Update(this BenchmarkModel model, BenchmarkModel other)
		{
			model.Name = other.Name;
			model.Uuid = other.Uuid;
			model.Tasks = other.Tasks.ToList();
		}

		public static BenchmarkModel ToModel(this BenchmarkDto dto)
		{
			return new BenchmarkModel
			{
				Id = dto.Id,
				Uuid = dto.Uuid,
				Name = dto.Name,
				Tasks = dto.TaskSpecifications.Select(t => t.ToModel()).ToList()
			};
		}
	}
}