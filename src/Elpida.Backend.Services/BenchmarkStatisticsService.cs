/*
 * Elpida HTTP Rest API
 *
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Lock;
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
		: Service<BenchmarkStatisticsDto, BenchmarkStatisticsModel, IBenchmarkStatisticsRepository>,
			IBenchmarkStatisticsService
	{
		private readonly IBenchmarkResultsRepository _benchmarkResultsRepository;
		private readonly IBenchmarkService _benchmarkService;
		private readonly ICpuService _cpuService;

		public BenchmarkStatisticsService(
			IBenchmarkService benchmarkService,
			IBenchmarkStatisticsRepository benchmarkStatisticsRepository,
			ICpuService cpuService,
			IBenchmarkResultsRepository benchmarkResultsRepository,
			ILockFactory lockFactory
		)
			: base(benchmarkStatisticsRepository, lockFactory)
		{
			_cpuService = cpuService;
			_benchmarkResultsRepository = benchmarkResultsRepository;
			_benchmarkService = benchmarkService;
		}

		private static IEnumerable<FilterExpression> StatisticsExpressions { get; } = new List<FilterExpression>
		{
			CreateFilter("cpuId", model => model.CpuId),
			CreateFilter("benchmarkId", model => model.BenchmarkId),
			CreateFilter("benchmarkScoreMean", model => model.Mean),
		};

		public async Task UpdateTaskStatisticsAsync(
			long benchmarkId,
			long cpuId,
			CancellationToken cancellationToken = default
		)
		{
			var stats = await GetStatisticsModelAsync(benchmarkId, cpuId, cancellationToken);

			var basicStatistics =
				await _benchmarkResultsRepository.GetStatisticsAsync(benchmarkId, cpuId, cancellationToken);

			stats.Max = basicStatistics.Max;
			stats.Min = basicStatistics.Min;
			stats.SampleSize = basicStatistics.Count;
			stats.Mean = basicStatistics.Mean;
			stats.StandardDeviation = basicStatistics.StandardDeviation;
			stats.MarginOfError = basicStatistics.MarginOfError;
			stats.Tau = StatisticsHelpers.CalculateTau(basicStatistics.Count);

			var actualClasses = GetDefaultClasses(stats.SampleSize, stats.Min, stats.Max)
				.ToArray();

			foreach (var cls in actualClasses)
			{
				cls.Count = await _benchmarkResultsRepository.GetCountWithScoreBetween(
					stats.BenchmarkId,
					stats.CpuId,
					cls.Low,
					cls.High,
					cancellationToken
				);
			}

			stats.FrequencyClasses = JsonConvert.SerializeObject(actualClasses);

			await Repository.SaveChangesAsync(cancellationToken);
		}

		public Task<PagedResult<BenchmarkStatisticsPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return GetPagedProjectionsAsync(
				queryRequest,
				GetPreviewConstructionExpression(),
				cancellationToken
			);
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

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			return StatisticsExpressions
				.Concat(_cpuService.GetFilters<BenchmarkStatisticsModel, CpuModel>(m => m.Cpu))
				.Concat(_benchmarkService.GetFilters<BenchmarkStatisticsModel, BenchmarkModel>(m => m.Benchmark));
		}

		protected override BenchmarkStatisticsDto ToDto(BenchmarkStatisticsModel model)
		{
			return new ()
			{
				Id = model.Id,
				Cpu = model.Cpu.ToDto(),
				Benchmark = model.Benchmark.ToDto(),
				Max = model.Max,
				Mean = model.Mean,
				Min = model.Min,
				Tau = model.Tau,
				SampleSize = model.SampleSize,
				StandardDeviation = model.StandardDeviation,
				MarginOfError = model.MarginOfError,
				Classes = JsonConvert.DeserializeObject<List<FrequencyClassDto>>(model.FrequencyClasses),
			};
		}

		private static Expression<Func<BenchmarkStatisticsModel, BenchmarkStatisticsPreviewDto>>
			GetPreviewConstructionExpression()
		{
			return m => new BenchmarkStatisticsPreviewDto
			{
				Id = m.Id,
				CpuVendor = m.Cpu.Vendor,
				CpuModelName = m.Cpu.ModelName,
				BenchmarkName = m.Benchmark.Name,
				BenchmarkUuid = m.Benchmark.Uuid,
				SampleSize = m.SampleSize,
				BenchmarkScoreUnit = m.Benchmark.ScoreUnit,
				Mean = m.Mean,
				Comparison = m.Benchmark.ScoreComparison,
			};
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
					i => new FrequencyClassDto
					{
						Low = min + (i * classWidth),
						High = min + (i * classWidth) + classWidth,
					}
				)
				.ToArray();

			return cls;
		}

		private async Task<BenchmarkStatisticsModel> GetStatisticsModelAsync(
			long benchmarkId,
			long cpuId,
			CancellationToken cancellationToken
		)
		{
			var stats = await Repository.GetSingleAsync(
				t => t.BenchmarkId == benchmarkId
				     && t.CpuId == cpuId,
				cancellationToken
			);

			if (stats != null)
			{
				return stats;
			}

			stats = new BenchmarkStatisticsModel
			{
				Id = 0,
				CpuId = cpuId,
				BenchmarkId = benchmarkId,
			};

			await Repository.CreateAsync(stats, cancellationToken);

			return stats;
		}
	}
}