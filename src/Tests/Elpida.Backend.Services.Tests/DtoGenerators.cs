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
using System.Linq;
using Elpida.Backend.Common;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Elpida;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Dtos.Statistics;
using Elpida.Backend.Services.Abstractions.Dtos.Task;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;

namespace Elpida.Backend.Services.Tests
{
	public static class DtoGenerators
	{
		public static BenchmarkDto NewBenchmark(long? id = null)
		{
			return new (id ?? NewId(), Guid.NewGuid(), "Test Benchmark", NewBenchmarkScoreSpecification(), new[] { BenchmarkTaskDto() });
		}

		public static BenchmarkPreviewDto NewBenchmarkPreview(long? id = null)
		{
			return new (id ?? NewId(), Guid.NewGuid(), "Test Benchmark");
		}

		public static TaskDto NewTask(long? id = null, Guid? uuid = null)
		{
			return new (id ?? NewId(), uuid ?? Guid.NewGuid(), "Test Task", "Test descdription",
				NewResultSpecification(),
				NewDataSpecification(), NewDataSpecification());
		}

		public static BenchmarkTaskDto BenchmarkTaskDto()
		{
			return new (Guid.NewGuid(), NewTask(), true, true, 5, true);
		}

		public static QueryRequest NewQueryRequest()
		{
			return new (
				NewPage(),
				new[] { new FilterInstance("data", "test", "equal") },
				"data",
				true
			);
		}

		public static BenchmarkResultPreviewDto NewBenchmarkResultPreview(long? id = null)
		{
			return new (id ?? NewId(), Guid.NewGuid(), DateTime.UtcNow, "Test benchmark", "Gentoo", "ARM", "Cortex A7",
				"b/s", 46);
		}

		public static BenchmarkResultDto NewBenchmarkResult(long? id = null)
		{
			return new (
				id ?? NewId(),
				DateTime.UtcNow,
				Guid.NewGuid(),
				"Test Benchmark",
				new long[] { 1, 2 },
				NewElpida(),
				NewSystem(),
				654,
				NewBenchmarkScoreSpecification(),
				new[] { NewTaskResult() }
			);
		}

		public static TaskResultDto NewTaskResult(long? id = null)
		{
			return new (
				id ?? NewId(),
				2,
				6,
				7,
				Guid.NewGuid(),
				"Test task",
				"Test description",
				NewResultSpecification(),
				NewDataSpecification(),
				NewDataSpecification(),
				4564,
				13,
				6546,
				NewTaskRunStatistics()
			);
		}

		public static TaskRunStatisticsDto NewTaskRunStatistics()
		{
			return new (56, 2, 6, 54, 4, 4, 6);
		}

		public static DataSpecificationDto NewDataSpecification()
		{
			return new ("Test Data", "Test description", "bytes", new[] { "Size" });
		}

		public static ResultSpecificationDto NewResultSpecification()
		{
			return new (
				"Result",
				"Result description",
				"b/s",
				AggregationType.Average,
				ResultType.Throughput
			);
		}

		public static BenchmarkScoreSpecificationDto NewBenchmarkScoreSpecification()
		{
			return new ("b/s", ValueComparison.Greater);
		}

		public static PageRequest NewPage()
		{
			return new (10, 10);
		}

		public static VersionDto NewVersion()
		{
			return new (5, 1, 2, 0);
		}

		public static CompilerDto NewCompiler()
		{
			return new ("GCC", "10.0");
		}

		public static ElpidaVersionDto NewElpida(long? id = null)
		{
			return new (id ?? NewId(), NewVersion(), NewCompiler());
		}

		public static MemoryDto NewMemory()
		{
			return new (465454, 4096);
		}

		public static TimingDto NewTiming()
		{
			return new (123, 456, 789, 1569, 8799, 132, 64, 789);
		}

		public static SystemDto NewSystem()
		{
			return new (
				NewCpu(),
				NewOs(),
				NewTopology(),
				NewMemory(),
				NewTiming()
			);
		}

		public static CpuNodeDto NewRootCpuNode()
		{
			return new (ProcessorNodeType.Machine, "Machine", 0, 0, Enumerable.Repeat(NewChildNode(), 4).ToArray(),
				Enumerable.Repeat(NewChildNode(), 4).ToArray());
		}

		public static CpuNodeDto NewChildNode()
		{
			return new (ProcessorNodeType.Core, "Core", 0, 0, null, null);
		}

		public static TopologyDto NewTopology(long? id = null)
		{
			return new (id ?? NewId(), 3, "x86_64", "Ryzen TR", 128, 64, 4, 1, NewRootCpuNode());
		}

		public static TopologyPreviewDto NewTopologyPreviewDto(long? id = null)
		{
			return new (id ?? NewId(), 2, "ARM", "Exynos", 16, 8, 2, 2, "sdfdsf");
		}

		public static OsDto NewOs(long? id = null)
		{
			return new (id ?? NewId(), "Linux", "KDE Neon", "21.1");
		}

		public static CpuCacheDto NewCache()
		{
			return new ("L5", "Satoko Hojo", 4654641, 1320);
		}

		public static CpuDto NewCpu(long? id = null)
		{
			return new (
				id ?? NewId(),
				"ARM",
				"Samsung",
				"Exynos",
				52654684351,
				true,
				new Dictionary<string, string>(),
				Enumerable.Repeat(NewCache(), 4).ToArray(),
				Enumerable.Repeat("LOL", 4).ToArray()
			);
		}

		public static CpuPreviewDto NewCpuPreview(long? id = null)
		{
			return new(){Id = id ?? NewId(), Vendor= "Samsung", ModelName= "Exynos"};
		}

		public static BenchmarkStatisticsDto NewBenchmarkStatistic(long? id = null)
		{
			return new (
				id ?? NewId(),
				NewCpu(),
				NewBenchmark(),
				52,
				46,
				59,
				12,
				6,
				5,
				8,
				new[] { NewFrequencyClass() }
			);
		}

		public static BenchmarkStatisticsPreviewDto NewBenchmarkStatisticsPreview(long? id = null)
		{
			return new (
				id ?? NewId(),
				"ARM",
				"Cortex A7",
				Guid.NewGuid(),
				"Test Benchmark",
				"b/s",
				65,
				46,
				ValueComparison.Greater
			);
		}

		public static FrequencyClassDto NewFrequencyClass()
		{
			return new (56, 45, 6);
		}

		public static TaskResultSlimDto NewTaskResultSlim()
		{
			return new (Guid.NewGuid(), 46, 123, 646, NewTaskRunStatistics());
		}

		public static BenchmarkResultSlimDto NewBenchmarkResultSlim()
		{
			return new (
				Guid.NewGuid(),
				DateTime.UtcNow,
				new long[] { 1, 23 },
				465,
				new[] { NewTaskResultSlim() }
			);
		}

		public static BenchmarkResultsBatchDto NewBenchmarkResultsBatch(long? id = null)
		{
			return new (id ?? NewId(), NewElpida(), NewSystem(), new[] { NewBenchmarkResultSlim(), NewBenchmarkResultSlim() });
		}

		public static long NewId()
		{
			return new Random().Next(1, 50);
		}
	}
}