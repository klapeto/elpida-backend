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
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Exceptions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions;
using Elpida.Backend.Services.Extensions.Cpu;

namespace Elpida.Backend.Services
{
    public class CpuService : Service<CpuDto, CpuModel, ICpuRepository>, ICpuService
    {
        private readonly ICpuRepository _cpuRepository;
        private readonly ITaskService _taskService;
        private readonly ITopologyRepository _topologyRepository;

        public CpuService(ICpuRepository cpuRepository,
            ITaskService taskService,
            ITopologyRepository topologyRepository)
            : base(cpuRepository)
        {
            _cpuRepository = cpuRepository;
            _taskService = taskService;
            _topologyRepository = topologyRepository;
        }

        private static IEnumerable<FilterExpression> CpuExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("cpuBrand", model => model.Brand),
            CreateFilter("cpuVendor", model => model.Vendor),
            CreateFilter("cpuFrequency", model => model.Frequency)
        };

        public Task<PagedResult<CpuPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionsAsync(queryRequest, m => new CpuPreviewDto
            {
                Id = m.Id,
                Vendor = m.Vendor,
                Brand = m.Brand,
                TopologiesCount = m.Topologies.Count(),
                TaskStatisticsCount = m.TaskStatistics.Count()
            }, cancellationToken);
        }

        public async Task<IEnumerable<TaskRunStatisticsDto>> GetStatisticsAsync(long cpuId,
            CancellationToken cancellationToken = default)
        {
            var cpuModel = await _cpuRepository.GetSingleAsync(cpuId, cancellationToken);

            if (cpuModel == null) throw new NotFoundException("Cpu was not found.", cpuId);

            return cpuModel.TaskStatistics.Select(s => s.ToDto());
        }

        protected override Task<CpuModel> ProcessDtoAndCreateModelAsync(CpuDto dto, CancellationToken cancellationToken)
        {
            return Task.FromResult(dto.ToModel());
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return CpuExpressions;
        }

        protected override CpuDto ToDto(CpuModel model)
        {
            return model.ToDto();
        }

        protected override Expression<Func<CpuModel, bool>> GetCreationBypassCheckExpression(CpuDto dto)
        {
            var additionalInfo = dto.ToModel().AdditionalInfo;
            return model =>
                model.Vendor == dto.Vendor
                && model.Brand == dto.Brand
                && model.AdditionalInfo == additionalInfo;
        }
    }
}