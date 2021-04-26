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

using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Models.Topology;

namespace Elpida.Backend.Data.Abstractions.Models.Cpu
{
    public class CpuModel : Entity
    {
        public string Vendor { get; set; } = default!;
        public string Brand { get; set; } = default!;
        public long Frequency { get; set; }
        public bool Smt { get; set; }
        public string AdditionalInfo { get; set; } = default!;
        public string Caches { get; set; } = default!;
        public string Features { get; set; } = default!;

        public ICollection<TaskStatisticsModel> TaskStatistics { get; set; } = default!;
        public ICollection<TopologyModel> Topologies { get; set; } = default!;
    }
}