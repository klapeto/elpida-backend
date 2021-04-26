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
using Elpida.Backend.Data.Abstractions;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Topology;

namespace Elpida.Backend.Services
{
    public class TopologyService : Service<TopologyDto, TopologyModel, ITopologyRepository>, ITopologyService
    {
        public TopologyService(ITopologyRepository topologyRepository)
            : base(topologyRepository)
        {
        }

        private static IEnumerable<FilterExpression> FilterExpressions { get; } = new List<FilterExpression>
        {
            CreateFilter("cpuCores", model => model.TotalPhysicalCores),
            CreateFilter("cpuLogicalCores", model => model.TotalLogicalCores)
        };

        protected override Task<TopologyModel> ProcessDtoAndCreateModelAsync(TopologyDto dto, CancellationToken cancellationToken)
        {
            return Task.FromResult(dto.ToModel());
        }

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return FilterExpressions;
        }

        protected override TopologyDto ToDto(TopologyModel model)
        {
            return model.ToDto();
        }

        protected override Expression<Func<TopologyModel, bool>> GetCreationBypassCheckExpression(TopologyDto dto)
        {
            var topologyHash = dto.ToModel().TopologyHash;
            return t =>
                t.CpuId == dto.CpuId
                && t.TopologyHash == topologyHash;
        }
    }
}