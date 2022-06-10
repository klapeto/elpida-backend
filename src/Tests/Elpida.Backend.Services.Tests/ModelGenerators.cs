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
using Elpida.Backend.Common;
using Elpida.Backend.Data.Abstractions.Models.Benchmark;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.ElpidaVersion;
using Elpida.Backend.Data.Abstractions.Models.Os;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services.Tests
{
	public static class ModelGenerators
	{
		public static BasicStatisticsModel NewBasicStatistics()
		{
			return new ()
			{
				Count = 2,
				Max = 500,
				Mean = 450,
				Min = 400,
				StandardDeviation = 5,
				MarginOfError = 8,
			};
		}

		public static BenchmarkStatisticsModel NewBenchmarkStatistics()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Benchmark = NewBenchmark(),
				Cpu = NewCpu(),
				Max = 400,
				Mean = 400,
				Min = 0,
				Tau = 5,
				MarginOfError = 8,
				SampleSize = 1,
				FrequencyClasses = JsonConvert.SerializeObject(null),
				StandardDeviation = 8,
			};
		}

		public static TaskModel NewTask()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Uuid = Guid.NewGuid(),

				Name = "Test Task",
				Description = "Test description",

				InputName = "Input name",
				InputDescription = "Input description",
				InputUnit = "bytes",
				InputProperties = JsonConvert.SerializeObject(new[] { "a", "b" }),

				OutputName = "Output name",
				OutputDescription = "Output description",
				OutputUnit = "bytes",
				OutputProperties = JsonConvert.SerializeObject(new[] { "LOOL" }),

				ResultName = "Result Name",
				ResultDescription = "Result Description",
				ResultAggregation = AggregationType.Accumulative,
				ResultType = ResultType.Throughput,
				ResultUnit = "bytes",
			};
		}

		public static BenchmarkTaskModel NewBenchmarkTask()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Task = NewTask(),
				CanBeDisabled = true,
				IterationsToRun = 5,
				CanBeMultiThreaded = true,
				IsCountedOnResults = true,
			};
		}

		public static BenchmarkModel NewBenchmark()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Name = "Test benchmark",
				Tasks = new List<BenchmarkTaskModel> { NewBenchmarkTask(), NewBenchmarkTask() },
				Uuid = Guid.NewGuid(),
				ScoreComparison = ValueComparison.Greater,
				ScoreUnit = "b/s",
			};
		}

		public static CpuModel NewCpu()
		{
			return new ()
			{
				Architecture = "ARM",
				Caches = JsonConvert.SerializeObject(new[] { DtoGenerators.NewCache() }),
				Features = JsonConvert.SerializeObject(new[] { "A", "B" }),
				Frequency = 465464,
				Id = DtoGenerators.NewId(),
				Smt = true,
				Vendor = "Samsung",
				AdditionalInfo = JsonConvert.SerializeObject(new { Haha = "haha" }),
				ModelName = "Cortex A7",
			};
		}

		public static ElpidaVersionModel NewElpida()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				CompilerName = "GCC",
				CompilerVersion = "10.0",
				VersionBuild = 5,
				VersionMajor = 8,
				VersionMinor = 9,
				VersionRevision = 1,
			};
		}

		public static OsModel NewOs()
		{
			return new ()
			{
				Category = "Linux",
				Id = DtoGenerators.NewId(),
				Name = "KDE Neon",
				Version = "15.2",
			};
		}

		public static TopologyModel NewTopology()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Cpu = NewCpu(),
				TopologyHash = "jsadfhgjkfdsg",
				TotalPackages = 8,
				TotalLogicalCores = 12,
				TotalPhysicalCores = 95,
				Root = JsonConvert.SerializeObject(DtoGenerators.NewTopology()),
				TotalNumaNodes = 87,
			};
		}

		public static TaskResultModel NewTaskResult()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Task = NewTask(),
				Max = 464,
				Mean = 36543,
				Min = 5456,
				Order = (int)DtoGenerators.NewId(),
				Tau = 6456,
				Time = 32435,
				Value = 543543,
				InputSize = 454,
				SampleSize = 45463,
				MarginOfError = 2454,
				StandardDeviation = 254534,
			};
		}

		public static ResultModel NewBenchmarkResult()
		{
			return new ()
			{
				Id = DtoGenerators.NewId(),
				Affinity = JsonConvert.SerializeObject(new long[] { 1, 2, 3 }),
				Benchmark = NewBenchmark(),
				ElpidaVersion = NewElpida(),
				Os = NewOs(),
				Score = 4984,
				Topology = NewTopology(),
				JoinOverhead = 987,
				LockOverhead = 646,
				LoopOverhead = 6546,
				MemorySize = 968763541,
				NotifyOverhead = 313,
				NowOverhead = 876,
				PageSize = 464,
				SleepOverhead = 64566,
				TargetTime = 635435,
				TimeStamp = DateTime.UtcNow,
				WakeupOverhead = 654,
				TaskResults = new List<TaskResultModel> { NewTaskResult(), NewTaskResult() },
			};
		}
	}
}