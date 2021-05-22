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
using Elpida.Backend.Common.Extensions;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Topology;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
    public class TopologyService : Service<TopologyDto, TopologyModel, ITopologyRepository>, ITopologyService
    {
        public TopologyService(ITopologyRepository topologyRepository, ILockFactory lockFactory)
            : base(topologyRepository, lockFactory)
        {
        }

        private static IEnumerable<FilterExpression> FilterExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("machines", model => model.TotalMachines),
            CreateFilter("cpuPackages", model => model.TotalPackages),
            CreateFilter("cpuNumaNodes", model => model.TotalNumaNodes),
            CreateFilter("cpuCores", model => model.TotalPhysicalCores),
            CreateFilter("cpuLogicalCores", model => model.TotalLogicalCores)
        };

        protected override Task<TopologyModel> ProcessDtoAndCreateModelAsync(TopologyDto dto,
            CancellationToken cancellationToken)
        {
            var serializedRoot = JsonConvert.SerializeObject(dto.Root);
            return Task.FromResult(new TopologyModel
            {
                Id = dto.Id,
                CpuId = dto.CpuId,
                TopologyHash = GetTopologyHash(dto, serializedRoot),
                TotalDepth = dto.TotalDepth,
                TotalLogicalCores = dto.TotalLogicalCores,
                TotalPhysicalCores = dto.TotalPhysicalCores,
                TotalMachines = dto.TotalMachines,
                TotalNumaNodes = dto.TotalNumaNodes,
                TotalPackages = dto.TotalPackages,
                Root = serializedRoot
            });
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return FilterExpressions;
        }

        protected override TopologyDto ToDto(TopologyModel model)
        {
            return model.ToDto();
        }

        private static string GetTopologyHash(TopologyDto dto, string? serializedRoot = null)
        {
            serializedRoot ??= JsonConvert.SerializeObject(dto.Root);

            return serializedRoot.ToHashString();
        }

        protected override Expression<Func<TopologyModel, bool>> GetCreationBypassCheckExpression(TopologyDto dto)
        {
            var topologyHash = GetTopologyHash(dto);
            return t =>
                t.CpuId == dto.CpuId
                && t.TopologyHash == topologyHash;
        }

        public Task<PagedResult<TopologyPreviewDto>> GetPagedPreviewsAsync(QueryRequest queryRequest,
            CancellationToken cancellationToken = default)
        {
            return GetPagedProjectionsAsync(queryRequest, m => new TopologyPreviewDto
            {
                Id = m.Id,
                CpuId = m.CpuId,
                CpuVendor = m.Cpu.Vendor,
                CpuBrand = m.Cpu.Brand,
                TotalDepth = m.TotalDepth,
                TotalMachines = m.TotalMachines,
                TotalPackages = m.TotalPackages,
                TotalNumaNodes = m.TotalNumaNodes,
                TotalPhysicalCores = m.TotalPhysicalCores,
                TotalLogicalCores = m.TotalLogicalCores,
                Hash = m.TopologyHash
            }, cancellationToken);
        }
    }
}