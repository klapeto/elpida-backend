/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2020 Ioannis Panagiotopoulos
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
    public class TaskResultDto : TaskDto
    {
        public long TaskId { get; set; }
        public long BenchmarkResultId { get; set; }
        public long CpuId { get; set; }
        public long TopologyId { get; set; }
        public double Value { get; set; }
        public double Time { get; set; }
        public long InputSize { get; set; }
        public TaskStatisticsDto Statistics { get; set; } = new TaskStatisticsDto();
    }
}