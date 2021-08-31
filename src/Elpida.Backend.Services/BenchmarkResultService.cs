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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
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
	public class BenchmarkResultService
		: Service<BenchmarkResultDto, BenchmarkResultPreviewDto, BenchmarkResultModel, IBenchmarkResultsRepository>,
			IBenchmarkResultsService
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
			: base(benchmarkResultsRepository)
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

		public async Task<IList<long>> AddBatchAsync(
			BenchmarkResultsBatchDto batch,
			CancellationToken cancellationToken = default
		)
		{
			var cpu = await _cpuService.GetOrAddAsync(batch.System.Cpu, cancellationToken);
			var topology = await _topologyService.GetOrAddTopologyAsync(
				cpu.Id,
				batch.System.Topology,
				cancellationToken
			);

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

		protected override BenchmarkResultDto ToDto(BenchmarkResultModel model)
		{
			return model.ToDto();
		}

		protected override Expression<Func<BenchmarkResultModel, BenchmarkResultPreviewDto>>
			GetPreviewConstructionExpression()
		{
			return m => new BenchmarkResultPreviewDto(
				m.Id,
				m.Benchmark.Uuid,
				m.TimeStamp,
				m.Benchmark.Name,
				m.Os.Name,
				m.Cpu.Vendor,
				m.Cpu.ModelName,
				m.Benchmark.ScoreUnit,
				m.Score
			);
		}

		protected override Task<BenchmarkResultModel> ProcessDtoAndCreateModelAsync(
			BenchmarkResultDto dto,
			CancellationToken cancellationToken
		)
		{
			throw new NotSupportedException(
				$"You cannot add a result in the usual way. Please use '{nameof(AddBatchAsync)}'"
			);
		}

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
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

		private async Task<long> AddAsync(
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
						StandardDeviation = taskResult.Statistics.StandardDeviation,
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
	}
}