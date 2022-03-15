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
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Data.Abstractions.Models.Benchmark;
using Elpida.Backend.Data.Abstractions.Models.ElpidaVersion;
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
	public class ResultService
		: Service<ResultDto, ResultPreviewDto, ResultModel, IResultRepository>,
			IResultService
	{
		private readonly IResultRepository _resultRepository;
		private readonly IBenchmarkService _benchmarkService;
		private readonly IBenchmarkStatisticsService _benchmarkStatisticsService;
		private readonly ICpuService _cpuService;
		private readonly IElpidaVersionService _elpidaVersionService;
		private readonly IOsService _osService;
		private readonly ITopologyService _topologyService;

		public ResultService(
			IResultRepository resultRepository,
			ICpuService cpuService,
			ITopologyService topologyService,
			IElpidaVersionService elpidaVersionService,
			IOsService osService,
			IBenchmarkService benchmarkService,
			IBenchmarkStatisticsService benchmarkStatisticsService
		)
			: base(resultRepository)
		{
			_benchmarkStatisticsService = benchmarkStatisticsService;
			_cpuService = cpuService;
			_topologyService = topologyService;
			_elpidaVersionService = elpidaVersionService;
			_osService = osService;
			_benchmarkService = benchmarkService;
			_resultRepository = resultRepository;
		}

		private static FilterExpression[]? ResultFilters { get; set; }

		public async Task<IList<long>> AddBatchAsync(
			ResultBatchDto batch,
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
			var elpida = await _elpidaVersionService.GetOrAddAsync(batch.ElpidaVersion, cancellationToken);

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

		public override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			if (ResultFilters != null)
			{
				return ResultFilters;
			}

			ResultFilters = new[]
				{
					FiltersTransformer.CreateFilter<ResultModel, long>(
						"memorySize",
						model => model.MemorySize
					),
					FiltersTransformer.CreateFilter<ResultModel, DateTime>(
						"timeStamp",
						model => model.TimeStamp
					),
				}
				.Concat(_topologyService.ConstructCustomFilters<ResultModel, TopologyModel>(m => m.Topology))
				.Concat(
					_elpidaVersionService.ConstructCustomFilters<ResultModel, ElpidaVersionModel>(
						m => m.ElpidaVersion
					)
				)
				.Concat(_osService.ConstructCustomFilters<ResultModel, OsModel>(m => m.Os))
				.Concat(
					_benchmarkService.ConstructCustomFilters<ResultModel, BenchmarkModel>(m => m.Benchmark)
				)
				.Distinct()
				.ToArray();

			return ResultFilters;
		}

		protected override ResultDto ToDto(ResultModel model)
		{
			return model.ToDto();
		}

		protected override Expression<Func<ResultModel, ResultPreviewDto>>
			GetPreviewConstructionExpression()
		{
			return m => new ResultPreviewDto
			{
				Id = m.Id,
				BenchmarkUuid = m.Benchmark.Uuid,
				TimeStamp = m.TimeStamp,
				BenchmarkName = m.Benchmark.Name,
				OsName = m.Os.Name,
				CpuVendor = m.Topology.Cpu.Vendor,
				CpuModelName = m.Topology.Cpu.ModelName,
				BenchmarkScoreUnit = m.Benchmark.ScoreUnit,
				Score = m.Score,
			};
		}

		protected override Task<ResultModel> ProcessDtoAndCreateModelAsync(
			ResultDto dto,
			CancellationToken cancellationToken
		)
		{
			throw new NotSupportedException(
				$"You cannot add a result in the usual way. Please use '{nameof(AddBatchAsync)}'"
			);
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

			var model = new ResultModel
			{
				BenchmarkId = benchmark.Id,
				ElpidaVersionId = elpidaId,
				OsId = osId,
				TopologyId = topologyId,
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
				var task = benchmark.Tasks.FirstOrDefault(t => t.Uuid == taskResult.Uuid);

				if (task is null)
				{
					throw new NotFoundException("The task was not found", taskResult.Uuid);
				}

				model.TaskResults.Add(
					new TaskResultModel
					{
						BenchmarkResultId = benchmark.Id,
						TaskId = task.Task!.Id,
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
			model = await _resultRepository.CreateAsync(model, cancellationToken);

			await _resultRepository.SaveChangesAsync(cancellationToken);

			await _benchmarkStatisticsService.UpdateTaskStatisticsAsync(benchmark.Id, cpuId, cancellationToken);

			return model.Id;
		}
	}
}