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
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Statistics;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Benchmark;
using Elpida.Backend.Services.Extensions.Cpu;
using Elpida.Backend.Services.Utilities;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
	public class BenchmarkStatisticsService
		: Service<BenchmarkStatisticsDto,
				BenchmarkStatisticsPreviewDto,
				BenchmarkStatisticsModel,
				IBenchmarkStatisticsRepository>,
			IBenchmarkStatisticsService
	{
		private readonly IBenchmarkService _benchmarkService;
		private readonly ICpuService _cpuService;
		private readonly IResultRepository _resultRepository;

		public BenchmarkStatisticsService(
			IBenchmarkService benchmarkService,
			IBenchmarkStatisticsRepository benchmarkStatisticsRepository,
			ICpuService cpuService,
			IResultRepository resultRepository
		)
			: base(benchmarkStatisticsRepository)
		{
			_cpuService = cpuService;
			_resultRepository = resultRepository;
			_benchmarkService = benchmarkService;
		}

		private static FilterExpression[]? StatisticsExpressions { get; set; }

		public async Task UpdateTaskStatisticsAsync(
			long benchmarkId,
			long cpuId,
			CancellationToken cancellationToken = default
		)
		{
			using var transaction = await Repository.BeginTransactionAsync(cancellationToken);

			var stats = await GetStatisticsModelAsync(benchmarkId, cpuId, cancellationToken);

			var basicStatistics =
				await _resultRepository.GetStatisticsAsync(benchmarkId, cpuId, cancellationToken);

			stats.Max = basicStatistics.Max;
			stats.Min = basicStatistics.Min;
			stats.SampleSize = basicStatistics.Count;
			stats.Mean = basicStatistics.Mean;
			stats.StandardDeviation = basicStatistics.StandardDeviation;
			stats.MarginOfError = basicStatistics.MarginOfError;
			stats.Tau = StatisticsHelpers.CalculateTau(basicStatistics.Count);

			var previousClasses = GetDefaultClasses(stats.SampleSize, stats.Min, stats.Max)
				.ToArray();

			var newClasses = new List<FrequencyClassDto>(previousClasses.Length);
			foreach (var frequencyClass in previousClasses)
			{
				var count = await _resultRepository.GetCountWithScoreBetween(
					stats.Benchmark.Id,
					stats.Cpu.Id,
					frequencyClass.Low,
					frequencyClass.High,
					cancellationToken
				);

				newClasses.Add(
					new FrequencyClassDto(
						frequencyClass.Low,
						frequencyClass.High,
						count
					)
				);
			}

			stats.FrequencyClasses = JsonConvert.SerializeObject(newClasses);

			await Repository.SaveChangesAsync(cancellationToken);

			await transaction.CommitAsync(cancellationToken);
		}

		public override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			if (StatisticsExpressions != null)
			{
				return StatisticsExpressions;
			}

			StatisticsExpressions = new[]
				{
					FiltersTransformer.CreateFilter<BenchmarkStatisticsModel, long>("cpuId", model => model.Cpu.Id),
					FiltersTransformer.CreateFilter<BenchmarkStatisticsModel, long>(
						"benchmarkId",
						model => model.Benchmark.Id
					),
					FiltersTransformer.CreateFilter<BenchmarkStatisticsModel, double>(
						"benchmarkScoreMean",
						model => model.Mean
					),
				}
				.Concat(_cpuService.ConstructCustomFilters<BenchmarkStatisticsModel, CpuModel>(m => m.Cpu))
				.Concat(
					_benchmarkService.ConstructCustomFilters<BenchmarkStatisticsModel, BenchmarkModel>(m => m.Benchmark)
				)
				.ToArray();

			return StatisticsExpressions;
		}

		protected override Task<BenchmarkStatisticsModel> ProcessDtoAndCreateModelAsync(
			BenchmarkStatisticsDto dto,
			CancellationToken cancellationToken
		)
		{
			return Task.FromResult(
				new BenchmarkStatisticsModel
				{
					Id = dto.Id,
					CpuId = dto.Cpu.Id,
					BenchmarkId = dto.Benchmark.Id,
					Max = dto.Max,
					Mean = dto.Mean,
					Min = dto.Min,
					Tau = dto.Tau,
					SampleSize = dto.SampleSize,
					StandardDeviation = dto.StandardDeviation,
					MarginOfError = dto.MarginOfError,
					FrequencyClasses = JsonConvert.SerializeObject(dto.Classes),
				}
			);
		}

		protected override BenchmarkStatisticsDto ToDto(BenchmarkStatisticsModel model)
		{
			return new BenchmarkStatisticsDto(
				model.Id,
				model.Cpu.ToDto(),
				model.Benchmark.ToDto(),
				model.SampleSize,
				model.Max,
				model.Min,
				model.Mean,
				model.StandardDeviation,
				model.Tau,
				model.MarginOfError,
				JsonConvert.DeserializeObject<FrequencyClassDto[]>(model.FrequencyClasses)!
			);
		}

		protected override Expression<Func<BenchmarkStatisticsModel, BenchmarkStatisticsPreviewDto>>
			GetPreviewConstructionExpression()
		{
			return m => new BenchmarkStatisticsPreviewDto(
				m.Id,
				m.Cpu.Vendor,
				m.Cpu.ModelName,
				m.Benchmark.Uuid,
				m.Benchmark.Name,
				m.Benchmark.ScoreUnit,
				m.Mean,
				m.SampleSize,
				m.Benchmark.ScoreComparison
			);
		}

		private static IEnumerable<FrequencyClassDto> GetDefaultClasses(long count, double min, double max)
		{
			var classes = (int)Math.Round(1 + (3.3 * Math.Log10(count)));
			var range = Math.Abs(max - min);
			var classWidth = range / classes;

			// widen the range
			min -= classWidth * 2;
			max += classWidth * 2;
			classes += 2;

			range = Math.Abs(max - min);
			classWidth = range / classes;

			var cls = Enumerable
				.Range(0, classes)
				.Select(
					i => new FrequencyClassDto(
						min + (i * classWidth),
						min + (i * classWidth) + classWidth,
						0
					)
				)
				.ToArray();

			return cls;
		}

		private Task<BenchmarkStatisticsModel> GetStatisticsModelAsync(
			long benchmarkId,
			long cpuId,
			CancellationToken cancellationToken
		)
		{
			return QueryUtilities.GetOrAddSafeAsync(
				Repository,
				new BenchmarkStatisticsModel
				{
					Id = 0,
					CpuId = cpuId,
					BenchmarkId = benchmarkId,
					FrequencyClasses = string.Empty,
				},
				t => t.Benchmark.Id == benchmarkId && t.Cpu.Id == cpuId,
				cancellationToken
			);
		}
	}
}