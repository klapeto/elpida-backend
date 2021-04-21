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

using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;

namespace Elpida.Backend.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ICpuService _cpuService;

        public StatisticsService(ICpuService cpuService)
        {
            _cpuService = cpuService;
        }

        public Task AddBenchmarkResultAsync(ResultDto result, CancellationToken cancellationToken = default)
        {
            return _cpuService.UpdateBenchmarkStatisticsAsync(result.System.Cpu.Id, result.Result, cancellationToken);
        }

        public async Task<CpuStatisticPreviewDto> GetPreviewsByCpuAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            return new CpuStatisticPreviewDto();
            // var x = _cpuService
        }
    }
}