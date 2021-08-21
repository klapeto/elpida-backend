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
using Elpida.Backend.Common.Extensions;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Topology;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Topology;
using Elpida.Backend.Services.Utilities;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
	public class TopologyService : Service<TopologyDto, TopologyModel, ITopologyRepository>, ITopologyService
	{
		private readonly ICpuService _cpuService;

		public TopologyService(ITopologyRepository topologyRepository, ICpuService cpuService, ILockFactory lockFactory)
			: base(topologyRepository, lockFactory)
		{
			_cpuService = cpuService;
		}

		private static FilterExpression[]? FilterExpressions { get; set; }

		public Task<PagedResult<TopologyPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return QueryUtilities.GetPagedProjectionsAsync(
				Repository,
				GetFilterExpressions(),
				queryRequest,
				m => new TopologyPreviewDto
				{
					Id = m.Id,
					CpuId = m.CpuId,
					CpuVendor = m.Cpu.Vendor,
					CpuModelName = m.Cpu.ModelName,
					TotalDepth = m.TotalDepth,
					TotalPackages = m.TotalPackages,
					TotalNumaNodes = m.TotalNumaNodes,
					TotalPhysicalCores = m.TotalPhysicalCores,
					TotalLogicalCores = m.TotalLogicalCores,
					Hash = m.TopologyHash,
				},
				cancellationToken
			);
		}

		protected override Task<TopologyModel> ProcessDtoAndCreateModelAsync(
			TopologyDto dto,
			CancellationToken cancellationToken
		)
		{
			var serializedRoot = JsonConvert.SerializeObject(dto.Root);
			return Task.FromResult(
				new TopologyModel
				{
					Id = dto.Id,
					CpuId = dto.CpuId,
					TopologyHash = GetTopologyHash(dto, serializedRoot),
					TotalDepth = dto.TotalDepth,
					TotalLogicalCores = dto.TotalLogicalCores,
					TotalPhysicalCores = dto.TotalPhysicalCores,
					TotalNumaNodes = dto.TotalNumaNodes,
					TotalPackages = dto.TotalPackages,
					Root = serializedRoot,
				}
			);
		}

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			if (FilterExpressions != null)
			{
				return FilterExpressions;
			}

			FilterExpressions = new[]
				{
					FiltersTransformer.CreateFilter<TopologyModel, int>("cpuPackages", model => model.TotalPackages),
					FiltersTransformer.CreateFilter<TopologyModel, int>("cpuNumaNodes", model => model.TotalNumaNodes),
					FiltersTransformer.CreateFilter<TopologyModel, int>("cpuCores", model => model.TotalPhysicalCores),
					FiltersTransformer.CreateFilter<TopologyModel, int>("cpuLogicalCores", model => model.TotalLogicalCores),
				}
				.Concat(_cpuService.ConstructCustomFilters<TopologyModel, CpuModel>(m => m.Cpu))
				.ToArray();

			return FilterExpressions;
		}

		protected override TopologyDto ToDto(TopologyModel model)
		{
			return model.ToDto();
		}

		protected override Expression<Func<TopologyModel, bool>> GetCreationBypassCheckExpression(TopologyDto dto)
		{
			SanitizeNode(dto.Root);
			var topologyHash = GetTopologyHash(dto);
			return t =>
				t.CpuId == dto.CpuId
				&& t.TopologyHash == topologyHash;
		}

		private static void SanitizeNode(CpuNodeDto node)
		{
			if (node.Children != null)
			{
				foreach (var child in node.Children)
				{
					SanitizeNode(child);
				}
			}

			if (node.MemoryChildren != null)
			{
				foreach (var child in node.MemoryChildren)
				{
					SanitizeNode(child);
				}
			}

			switch (node.NodeType)
			{
				case ProcessorNodeType.L1DCache:
				case ProcessorNodeType.L1ICache:
				case ProcessorNodeType.L2DCache:
				case ProcessorNodeType.L2ICache:
				case ProcessorNodeType.L3DCache:
				case ProcessorNodeType.L4Cache:
				case ProcessorNodeType.L5Cache:
					return;
			}

			node.Value = null;
		}

		private static string GetTopologyHash(TopologyDto dto, string? serializedRoot = null)
		{
			serializedRoot ??= JsonConvert.SerializeObject(dto.Root);

			return serializedRoot.ToHashString();
		}
	}
}