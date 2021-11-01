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
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	internal class BenchmarkResultServiceTests
	{
		private Mock<IBenchmarkResultsRepository> _benchmarkResultRepo = default!;

		private Mock<IBenchmarkStatisticsService> _benchmarkStatisticsService = default!;

		private Mock<ICpuService> _cpuService = default!;

		private Mock<ITopologyService> _topologyService = default!;

		private Mock<IElpidaVersionService> _elpidaService = default!;

		private Mock<IOsService> _osService = default!;

		private Mock<IBenchmarkService> _benchmarkService = default!;

		private BenchmarkResultService _service = default!;

		[SetUp]
		public void Setup()
		{
			_benchmarkResultRepo = new Mock<IBenchmarkResultsRepository>(MockBehavior.Strict);
			_cpuService = new Mock<ICpuService>(MockBehavior.Strict);
			_topologyService = new Mock<ITopologyService>(MockBehavior.Strict);
			_elpidaService = new Mock<IElpidaVersionService>(MockBehavior.Strict);
			_osService = new Mock<IOsService>(MockBehavior.Strict);
			_benchmarkService = new Mock<IBenchmarkService>(MockBehavior.Strict);
			_benchmarkStatisticsService = new Mock<IBenchmarkStatisticsService>(MockBehavior.Strict);

			_service = new BenchmarkResultService(
				_benchmarkResultRepo.Object,
				_cpuService.Object,
				_topologyService.Object,
				_elpidaService.Object,
				_osService.Object,
				_benchmarkService.Object,
				_benchmarkStatisticsService.Object
			);
		}

		[Test]
		public async Task AddBatchAsync_Success()
		{
			var batch = DtoGenerators.NewBenchmarkResultsBatch();

			var returnCpu = DtoGenerators.NewCpu(946);
			var returnTopology = DtoGenerators.NewTopology(1332);
			var returnOs = DtoGenerators.NewOs(653);
			var returnElpida = DtoGenerators.NewElpida(357);
			var returnBenchmarks = GetBenchmarks(batch.BenchmarkResults)
				.ToArray();

			var expectedIds = new List<long>();

			_cpuService.Setup(s => s.GetOrAddAsync(batch.System.Cpu, default))
				.ReturnsAsync(returnCpu);

			_topologyService.Setup(s => s.GetOrAddTopologyAsync(returnCpu.Id, batch.System.Topology, default))
				.ReturnsAsync(returnTopology);

			_osService.Setup(s => s.GetOrAddAsync(batch.System.Os, default))
				.ReturnsAsync(returnOs);

			_elpidaService.Setup(s => s.GetOrAddAsync(batch.ElpidaVersion, default))
				.ReturnsAsync(returnElpida);

			_benchmarkResultRepo.Setup(r => r.CreateAsync(It.Is<BenchmarkResultModel>(x => x.Id == 0), default))
				.Returns<BenchmarkResultModel, CancellationToken>(
					(x, _) =>
					{
						var id = new Random().Next(20, 50);
						expectedIds.Add(id);
						x.Id = id;
						return Task.FromResult(x);
					}
				);

			_benchmarkResultRepo.Setup(r => r.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			for (var i = 0; i < batch.BenchmarkResults.Length; i++)
			{
				var index = i;
				_benchmarkService.Setup(s => s.GetSingleAsync(batch.BenchmarkResults[index].Uuid, default))
					.ReturnsAsync(returnBenchmarks[index]);
			}

			for (var i = 0; i < returnBenchmarks.Length; i++)
			{
				var index = i;
				_benchmarkStatisticsService.Setup(
						s => s.UpdateTaskStatisticsAsync(returnBenchmarks[index].Id, returnCpu.Id, default)
					)
					.Returns(Task.CompletedTask);
			}

			var ids = await _service.AddBatchAsync(batch);

			for (var i = 0; i < expectedIds.Count; i++)
			{
				Assert.AreEqual(expectedIds[i], ids[i]);
			}

			_benchmarkService.VerifyAll();
			_benchmarkStatisticsService.VerifyAll();
			_elpidaService.VerifyAll();
			_osService.VerifyAll();
			_topologyService.VerifyAll();
			_cpuService.VerifyAll();
		}

		[Test]
		public void AddBatchAsync_InvalidTask_ThrowsNotFoundException()
		{
			var batch = DtoGenerators.NewBenchmarkResultsBatch();

			var returnCpu = DtoGenerators.NewCpu(946);
			var returnTopology = DtoGenerators.NewTopology(1332);
			var returnOs = DtoGenerators.NewOs(653);
			var returnElpida = DtoGenerators.NewElpida(357);

			_cpuService.Setup(s => s.GetOrAddAsync(batch.System.Cpu, default))
				.ReturnsAsync(returnCpu);

			_topologyService.Setup(s => s.GetOrAddTopologyAsync(returnCpu.Id, batch.System.Topology, default))
				.ReturnsAsync(returnTopology);

			_osService.Setup(s => s.GetOrAddAsync(batch.System.Os, default))
				.ReturnsAsync(returnOs);

			_elpidaService.Setup(s => s.GetOrAddAsync(batch.ElpidaVersion, default))
				.ReturnsAsync(returnElpida);

			_benchmarkResultRepo.Setup(r => r.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			for (var i = 0; i < batch.BenchmarkResults.Length; i++)
			{
				var index = i;
				_benchmarkService.Setup(s => s.GetSingleAsync(batch.BenchmarkResults[index].Uuid, default))
					.ReturnsAsync(DtoGenerators.NewBenchmark());
			}

			Assert.ThrowsAsync<NotFoundException>(() => _service.AddBatchAsync(batch));
		}

		[Test]
		public async Task GetSingleAsync_ReturnsUnifiedResult()
		{
			const int id = 5;
			var model = ModelGenerators.NewBenchmarkResult();

			_benchmarkResultRepo.Setup(r => r.GetSingleAsync(id, default))
				.ReturnsAsync(model);

			var dto = await _service.GetSingleAsync(id);

			Assert.AreEqual(model.Id, dto.Id);
			Assert.AreEqual(model.Benchmark.Uuid, dto.Uuid);
			Assert.AreEqual(model.TimeStamp, dto.TimeStamp);
			Assert.AreEqual(model.Score, dto.Score);
			Assert.AreEqual(model.MemorySize, dto.System.Memory.TotalSize);
			Assert.AreEqual(model.PageSize, dto.System.Memory.PageSize);
			Assert.AreEqual(model.Benchmark.Name, dto.Name);
			Assert.AreEqual(model.Benchmark.ScoreComparison, dto.ScoreSpecification.Comparison);
			Assert.AreEqual(model.Benchmark.ScoreUnit, dto.ScoreSpecification.Unit);

			Assert.AreEqual(model.JoinOverhead, dto.System.Timing.JoinOverhead);
			Assert.AreEqual(model.LockOverhead, dto.System.Timing.LockOverhead);
			Assert.AreEqual(model.LoopOverhead, dto.System.Timing.LoopOverhead);
			Assert.AreEqual(model.NotifyOverhead, dto.System.Timing.NotifyOverhead);
			Assert.AreEqual(model.NowOverhead, dto.System.Timing.NowOverhead);
			Assert.AreEqual(model.SleepOverhead, dto.System.Timing.SleepOverhead);
			Assert.AreEqual(model.TargetTime, dto.System.Timing.TargetTime);
			Assert.AreEqual(model.WakeupOverhead, dto.System.Timing.WakeupOverhead);

			model.ElpidaVersion.AssertEqual(dto.ElpidaVersion);
			model.Os.AssertEqual(dto.System.Os);
			model.Topology.Cpu.AssertEqual(dto.System.Cpu);
			model.Topology.AssertEqual(dto.System.Topology);

			JsonConvert.DeserializeObject<long[]>(model.Affinity)
				!.AssertCollectionsEqual(dto.Affinity);

			AssertTasksEqual(model, dto);
		}

		private static void AssertTasksEqual(BenchmarkResultModel model, BenchmarkResultDto dto)
		{
			var modelsArray = model.TaskResults
				.OrderBy(m => m.Order)
				.ToArray();

			var dtosArray = dto.TaskResults.ToArray();

			Assert.AreEqual(modelsArray.Length, dtosArray.Length);

			for (var i = 0; i < modelsArray.Length; i++)
			{
				var a = modelsArray[i];
				var b = dtosArray[i];

				Assert.AreEqual(model.Topology.Cpu.Id, b.CpuId);
				Assert.AreEqual(model.Topology.Id, b.TopologyId);
				Assert.AreEqual(model.Id, b.BenchmarkResultId);

				Assert.AreEqual(a.Max, b.Statistics.Max);
				Assert.AreEqual(a.Min, b.Statistics.Min);
				Assert.AreEqual(a.Mean, b.Statistics.Mean);
				Assert.AreEqual(a.Tau, b.Statistics.Tau);
				Assert.AreEqual(a.SampleSize, b.Statistics.SampleSize);
				Assert.AreEqual(a.StandardDeviation, b.Statistics.StandardDeviation);
				Assert.AreEqual(a.MarginOfError, b.Statistics.MarginOfError);

				Assert.AreEqual(a.Time, b.Time);
				Assert.AreEqual(a.Value, b.Value);
				Assert.AreEqual(a.InputSize, b.InputSize);

				Assert.AreEqual(a.Task.Description, b.Description);
				Assert.AreEqual(a.Task.Name, b.Name);
				Assert.AreEqual(a.Task.Uuid, b.Uuid);

				Assert.AreEqual(a.Task.InputName, b.Input!.Name);
				Assert.AreEqual(a.Task.InputDescription, b.Input.Description);
				Assert.AreEqual(a.Task.InputUnit, b.Input.Unit);
				JsonConvert.DeserializeObject<string[]>(a.Task!.InputProperties!)
					!.AssertCollectionsEqual(b.Input.RequiredProperties);

				Assert.AreEqual(a.Task.OutputName, b.Output!.Name);
				Assert.AreEqual(a.Task.OutputDescription, b.Output.Description);
				Assert.AreEqual(a.Task.OutputUnit, b.Output.Unit);
				JsonConvert.DeserializeObject<string[]>(a.Task!.OutputProperties!)
					!.AssertCollectionsEqual(b.Output.RequiredProperties);

				Assert.AreEqual(a.Task.ResultAggregation, b.Result.Aggregation);
				Assert.AreEqual(a.Task.ResultDescription, b.Result.Description);
				Assert.AreEqual(a.Task.ResultName, b.Result.Name);
				Assert.AreEqual(a.Task.ResultType, b.Result.Type);
				Assert.AreEqual(a.Task.ResultUnit, b.Result.Unit);
			}
		}

		private static IEnumerable<BenchmarkDto> GetBenchmarks(IEnumerable<BenchmarkResultSlimDto> benchmarkResults)
		{
			return benchmarkResults.Select(
				s => new BenchmarkDto{
					Id = 89,
					Uuid = s.Uuid,
					Name = "test benchmark",
					Tasks = DtoGenerators.NewBenchmarkScoreSpecification(),
					s.TaskResults.Select(
							x => new BenchmarkTaskDto{
								Uuid = x.Uuid,
								Task = DtoGenerators.NewTask(664, x.Uuid),
								CanBeDisabled = true,
								CanBeMultiThreaded = true,
								IterationsToRun = 5,
								IsCountedOnResults = true
							}
						)
						.ToArray()
				}
			);
		}
	}
}