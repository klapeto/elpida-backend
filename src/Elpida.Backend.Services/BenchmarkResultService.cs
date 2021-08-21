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
using Elpida.Backend.Data.Abstractions.Models.Benchmark;
using Elpida.Backend.Data.Abstractions.Models.Elpida;
using Elpida.Backend.Data.Abstractions.Models.Os;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Dtos.Result.Batch;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Result;
using Elpida.Backend.Services.Utilities;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
	public class BenchmarkResultService : IBenchmarkResultsService
	{
		private readonly IBenchmarkResultsRepository _benchmarkResultsRepository;
		private readonly IBenchmarkService _benchmarkService;
		private readonly IBenchmarkStatisticsService _benchmarkStatisticsService;
		private readonly ICpuService _cpuService;
		private readonly IElpidaService _elpidaService;

		private readonly IOsService _osService;
		private readonly ITaskService _taskService;
		private readonly ITopologyService _topologyService;

		public BenchmarkResultService(
			IBenchmarkResultsRepository benchmarkResultsRepository,
			ICpuService cpuService,
			ITopologyService topologyService,
			IElpidaService elpidaService,
			IOsService osService,
			IBenchmarkService benchmarkService,
			ITaskService taskService,
			IBenchmarkStatisticsService benchmarkStatisticsService
		)
		{
			_taskService = taskService;
			_benchmarkStatisticsService = benchmarkStatisticsService;
			_cpuService = cpuService;
			_topologyService = topologyService;
			_elpidaService = elpidaService;
			_osService = osService;
			_benchmarkService = benchmarkService;
			_benchmarkResultsRepository = benchmarkResultsRepository;
		}

		private static FilterExpression[]? ResultFilters { get; set; }

		public Task<PagedResult<BenchmarkResultPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return QueryUtilities.GetPagedProjectionsAsync(
				_benchmarkResultsRepository,
				GetFilterExpressions(),
				queryRequest,
				m => new BenchmarkResultPreviewDto
				{
					Id = m.Id,
					TimeStamp = m.TimeStamp,
					BenchmarkUuid = m.Benchmark.Uuid,
					BenchmarkName = m.Benchmark.Name,
					CpuVendor = m.Topology.Cpu.Vendor,
					CpuModelName = m.Topology.Cpu.ModelName,
					CpuCores = m.Topology.TotalPhysicalCores,
					CpuLogicalCores = m.Topology.TotalLogicalCores,
					CpuFrequency = m.Topology.Cpu.Frequency,
					MemorySize = m.MemorySize,
					OsName = m.Os.Name,
					OsVersion = m.Os.Version,
					ElpidaVersionBuild = m.Elpida.VersionBuild,
					ElpidaVersionMajor = m.Elpida.VersionMajor,
					ElpidaVersionMinor = m.Elpida.VersionMinor,
					ElpidaVersionRevision = m.Elpida.VersionRevision,
				},
				cancellationToken
			);
		}

		public async Task<BenchmarkResultDto> GetSingleAsync(long id, CancellationToken cancellationToken = default)
		{
			var entity = await _benchmarkResultsRepository.GetSingleAsync(id, cancellationToken);

			if (entity == null)
			{
				throw new NotFoundException($"{nameof(BenchmarkResultDto)} was not found", id);
			}

			return entity.ToDto();
		}

		public async Task<long> AddAsync(
			long cpuId,
			long topologyId,
			long osId,
			long elpidaId,
			BenchmarkResultSlimDto benchmarkResult,
			MemoryDto memory,
			TimingDto timing,
			CancellationToken cancellationToken = default
		)
		{
			var benchmark = await _benchmarkService
				.GetSingleAsync(benchmarkResult.Uuid, cancellationToken);

			var model = new BenchmarkResultModel
			{
				BenchmarkId = benchmark.Id,
				ElpidaId = elpidaId,
				TopologyId = topologyId,
				OsId = osId,
				CpuId = cpuId,
				Affinity = JsonConvert.SerializeObject(benchmarkResult.Affinity),
				JoinOverhead = timing.JoinOverhead,
				LockOverhead = timing.LockOverhead,
				LoopOverhead = timing.LoopOverhead,
				NotifyOverhead = timing.NotifyOverhead,
				NowOverhead = timing.NowOverhead,
				SleepOverhead = timing.SleepOverhead,
				TargetTime = timing.TargetTime,
				WakeupOverhead = timing.WakeupOverhead,
				MemorySize = memory.TotalSize,
				PageSize = memory.PageSize,
				Score = benchmarkResult.Score,
				TaskResults = new List<TaskResultModel>(),
				TimeStamp = DateTime.UtcNow,
			};

			var order = 0;
			foreach (var taskResult in benchmarkResult.TaskResults)
			{
				var task = await _taskService.GetSingleAsync(taskResult.Uuid, cancellationToken);

				model.TaskResults.Add(
					new TaskResultModel
					{
						CpuId = cpuId,
						TaskId = task.Id,
						TopologyId = topologyId,
						BenchmarkResult = model,
						Max = taskResult.Statistics.Max,
						Mean = taskResult.Statistics.Mean,
						Min = taskResult.Statistics.Min,
						Tau = taskResult.Statistics.Tau,
						SampleSize = taskResult.Statistics.SampleSize,
						StandardDeviation = taskResult.Statistics.Sd,
						MarginOfError = taskResult.Statistics.MarginOfError,
						InputSize = taskResult.InputSize,
						Value = taskResult.Value,
						Time = taskResult.Time,
						Order = order++,
					}
				);
			}

			model.Id = 0;
			model = await _benchmarkResultsRepository.CreateAsync(model, cancellationToken);

			await _benchmarkResultsRepository.SaveChangesAsync(cancellationToken);

			await _benchmarkStatisticsService.UpdateTaskStatisticsAsync(benchmark.Id, cpuId, cancellationToken);

			return model.Id;
		}

		public async Task<IList<long>> AddBatchAsync(
			BenchmarkResultsBatchDto batch,
			CancellationToken cancellationToken = default
		)
		{
			var cpu = await _cpuService.GetOrAddAsync(batch.System.Cpu, cancellationToken);
			batch.System.Topology.CpuId = cpu.Id;
			var topology = await _topologyService.GetOrAddAsync(batch.System.Topology, cancellationToken);
			var os = await _osService.GetOrAddAsync(batch.System.Os, cancellationToken);
			var elpida = await _elpidaService.GetOrAddAsync(batch.Elpida, cancellationToken);

			var ids = new List<long>();

			foreach (var benchmarkResult in batch.BenchmarkResults)
			{
				ids.Add(
					await AddAsync(
						cpu.Id,
						topology.Id,
						os.Id,
						elpida.Id,
						benchmarkResult,
						batch.System.Memory,
						batch.System.Timing,
						cancellationToken
					)
				);
			}

			return ids;
		}

		private IEnumerable<FilterExpression> GetFilterExpressions()
		{
			if (ResultFilters != null)
			{
				return ResultFilters;
			}

			ResultFilters = new[]
				{
					FiltersTransformer.CreateFilter<BenchmarkResultModel, long>(
						"memorySize",
						model => model.MemorySize
					),
					FiltersTransformer.CreateFilter<BenchmarkResultModel, DateTime>(
						"timeStamp",
						model => model.TimeStamp
					),
				}
				.Concat(_topologyService.ConstructCustomFilters<BenchmarkResultModel, TopologyModel>(m => m.Topology))
				.Concat(_elpidaService.ConstructCustomFilters<BenchmarkResultModel, ElpidaModel>(m => m.Elpida))
				.Concat(_osService.ConstructCustomFilters<BenchmarkResultModel, OsModel>(m => m.Os))
				.Concat(
					_benchmarkService.ConstructCustomFilters<BenchmarkResultModel, BenchmarkModel>(m => m.Benchmark)
				)
				.Distinct()
				.ToArray();

			return ResultFilters;
		}
	}
}