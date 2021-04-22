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

namespace Elpida.Backend.Services.Abstractions.Dtos
{
    public class TaskStatisticsPreviewDto
    {
        public string CpuVendor { get; set; } = default!;
        public string CpuBrand { get; set; } = default!;
        public string TaskName { get; set; } = default!;
        public string TaskResultUnit { get; set; } = default!;
        public int CpuCores { get; set; }
        public int CpuLogicalCores { get; set; }
        public string TopologyHash { get; set; } = default!;
        public double Mean { get; set; }
        public long SampleSize { get; set; }
    }
}