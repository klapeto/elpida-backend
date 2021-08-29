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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Common.Exceptions;
using Elpida.Backend.Data.Abstractions.Models.Benchmark;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Benchmark;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Benchmark;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
	public class BenchmarkService : Service<BenchmarkDto, BenchmarkModel, IBenchmarkRepository>, IBenchmarkService
	{
		private readonly ITaskRepository _taskRepository;

		public BenchmarkService(
			IBenchmarkRepository benchmarkRepository,
			ITaskRepository taskRepository
		)
			: base(benchmarkRepository)
		{
			_taskRepository = taskRepository;
		}

		private static IEnumerable<FilterExpression> BenchmarkExpressions { get; } = new[]
		{
			FiltersTransformer.CreateFilter<BenchmarkModel, string>("benchmarkName", model => model.Name),
		};

		public Task<PagedResult<BenchmarkPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return QueryUtilities.GetPagedProjectionsAsync(
				Repository,
				GetFilterExpressions(),
				queryRequest,
				m => new BenchmarkPreviewDto(m.Id, m.Uuid, m.Name),
				cancellationToken
			);
		}

		public async Task<BenchmarkDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
		{
			var benchmark = await Repository.GetSingleAsync(m => m.Uuid == uuid, cancellationToken);

			if (benchmark == null)
			{
				throw new NotFoundException("Benchmark was not found.", uuid);
			}

			return ToDto(benchmark);
		}

		protected override async Task<BenchmarkModel> ProcessDtoAndCreateModelAsync(
			BenchmarkDto dto,
			CancellationToken cancellationToken
		)
		{
			var returnModel = new BenchmarkModel
			{
				Id = dto.Id,
				Uuid = dto.Uuid,
				Name = dto.Name,
				ScoreUnit = dto.ScoreSpecification.Unit,
				ScoreComparison = dto.ScoreSpecification.Comparison,
				Tasks = new List<BenchmarkTaskModel>(),
			};

			foreach (var taskDto in dto.Tasks)
			{
				var task = await _taskRepository.GetSingleAsync(t => t.Uuid == taskDto.Uuid, cancellationToken);
				if (task == null)
				{
					throw new NotFoundException("Task was not found", taskDto.Uuid);
				}

				var taskModel = new BenchmarkTaskModel
				{
					TaskId = task.Id,
					CanBeDisabled = taskDto.CanBeDisabled,
					IterationsToRun = taskDto.IterationsToRun,
					CanBeMultiThreaded = taskDto.CanBeMultiThreaded,
					IsCountedOnResults = taskDto.IsCountedOnResults,
				};

				returnModel.Tasks.Add(taskModel);
			}

			return returnModel;
		}

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			return BenchmarkExpressions;
		}

		protected override BenchmarkDto ToDto(BenchmarkModel model)
		{
			return model.ToDto();
		}

		protected override Expression<Func<BenchmarkModel, bool>> GetCreationBypassCheckExpression(BenchmarkDto dto)
		{
			return model => model.Uuid == dto.Uuid;
		}
	}
}