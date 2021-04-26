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

using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Abstractions.Dtos
{
    public class TaskStatisticsDto : FountationDto
    {
        public CpuDto Cpu { get; set; } = default!;
        public TaskDto Task { get; set; } = default!;
        public TopologyDto Topology { get; set; } = default!;
        
        public long SampleSize { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public double Mean { get; set; }
        public double StandardDeviation { get; set; }
        public double Tau { get; set; }
        public double MarginOfError { get; set; }
    }
}