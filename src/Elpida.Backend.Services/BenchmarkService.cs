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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Benchmark;

namespace Elpida.Backend.Services
{
    public class BenchmarkService : Service<BenchmarkDto, BenchmarkModel, IBenchmarkRepository>, IBenchmarkService
    {
        public BenchmarkService(IBenchmarkRepository benchmarkRepository)
            : base(benchmarkRepository)
        {
        }

        private static IEnumerable<FilterExpression> BenchmarkExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("benchmarkName", model => model.Name)
        };

        public async Task<BenchmarkDto> GetSingleAsync(Guid uuid, CancellationToken cancellationToken = default)
        {
            var model = await Repository.GetSingleAsync(b => b.Uuid == uuid, cancellationToken);

            if (model == null) throw new NotFoundException("Benchmark was not found.", uuid);

            return model.ToDto();
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return BenchmarkExpressions;
        }

        protected override BenchmarkDto ToDto(BenchmarkModel model)
        {
            return model.ToDto();
        }

        protected override BenchmarkModel ToModel(BenchmarkDto dto)
        {
            return dto.ToDto();
        }

        protected override Expression<Func<BenchmarkModel, bool>> GetCreationBypassCheckExpression(BenchmarkDto dto)
        {
            return model => model.Uuid == dto.Uuid;
        }
    }
}