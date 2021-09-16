// =========================================================================
//
// Elpida HTTP Rest API
//
// Copyright (C) 2021 Ioannis Panagiotopoulos
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// =========================================================================

using System;
using System.Collections.Generic;
using Elpida.Backend.Data.Abstractions.Models.Benchmark;
using Elpida.Backend.Data.Abstractions.Models.ElpidaVersion;
using Elpida.Backend.Data.Abstractions.Models.Os;
using Elpida.Backend.Data.Abstractions.Models.Topology;

namespace Elpida.Backend.Data.Abstractions.Models.Result
{
	public class BenchmarkResultModel : Entity
	{
		public long ElpidaVersionId { get; set; }

		public ElpidaVersionModel ElpidaVersion { get; set; } = default!;

		public long OsId { get; set; }

		public OsModel Os { get; set; } = default!;

		public long TopologyId { get; set; }

		public TopologyModel Topology { get; set; } = null!;

		public long BenchmarkId { get; set; }

		public BenchmarkModel Benchmark { get; set; } = null!;

		public DateTime TimeStamp { get; set; }

		public string Affinity { get; set; } = default!;

		public long MemorySize { get; set; }

		public long PageSize { get; set; }

		public double NotifyOverhead { get; set; }

		public double WakeupOverhead { get; set; }

		public double SleepOverhead { get; set; }

		public double NowOverhead { get; set; }

		public double LockOverhead { get; set; }

		public double LoopOverhead { get; set; }

		public double JoinOverhead { get; set; }

		public double TargetTime { get; set; }

		public double Score { get; set; }

		public ICollection<TaskResultModel> TaskResults { get; set; } = null!;
	}
}