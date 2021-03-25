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

using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Topology;

namespace Elpida.Backend.Data.Abstractions.Models.Result
{
	public class ResultModel : Entity
	{
		public DateTime TimeStamp { get; set; }
		public string Affinity { get; set; } = default!;

		public string ElpidaVersion { get; set; } = default!;
		
		public string CompilerVersion { get; set; } = default!;
		public string CompilerName { get; set; } = default!;
		
		public string OsCategory { get; set; } = default!;
		public string OsName { get; set; } = default!;
		public string OsVersion { get; set; } = default!;
		
		public long MemorySize { get; set; }
		public long PageSize { get; set; }
		
		public TopologyModel Topology { get; set; } = null!;

		public double NotifyOverhead { get; set; }
		public double WakeupOverhead { get; set; }
		public double SleepOverhead { get; set; }
		public double NowOverhead { get; set; }
		public double LockOverhead { get; set; }
		public double LoopOverhead { get; set; }
		public double JoinOverhead { get; set; }
		public double TargetTime { get; set; }
		
		public BenchmarkModel Benchmark { get; set; } = null!;
		public ICollection<TaskResultModel> TaskResults { get; set; } = null!;
	}
}