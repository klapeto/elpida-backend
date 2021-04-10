using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Topology;

namespace Elpida.Backend.Services
{
    public class TopologyService : Service<TopologyDto, TopologyModel>, ITopologyService
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

        protected override IEnumerable<FilterExpression> GetFilterExpressions()
        {
            return FilterExpressions;
        }

        protected override TopologyDto ToDto(TopologyModel model)
        {
            return model.ToDto();
        }

        protected override TopologyModel ToModel(TopologyDto dto)
        {
            return dto.ToModel();
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