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

using System.Linq;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Extensions.Task;

namespace Elpida.Backend.Services.Extensions.Benchmark
{
    public static class BenchmarkDataExtensions
    {
        public static BenchmarkDto ToDto(this BenchmarkModel benchmarkModel)
        {
            return new BenchmarkDto
            {
                Id = benchmarkModel.Id,
                Uuid = benchmarkModel.Uuid,
                Name = benchmarkModel.Name,
                TaskSpecifications = benchmarkModel.Tasks
                    .Select(t => t.ToDto())
                    .ToList()
            };
        }
        
        public static BenchmarkModel ToDto(this BenchmarkDto benchmarkDto)
        {
            return new BenchmarkModel
            {
                Id = benchmarkDto.Id,
                Uuid = benchmarkDto.Uuid,
                Name = benchmarkDto.Name,
                Tasks = benchmarkDto.TaskSpecifications.Select(t => t.ToModel()).ToList()
            };
        }
    }
}