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
	public class TopologyService
		: Service<TopologyDto, TopologyPreviewDto, TopologyModel, ITopologyRepository>, ITopologyService
	{
		private readonly ICpuService _cpuService;

		public TopologyService(ITopologyRepository topologyRepository, ICpuService cpuService)
			: base(topologyRepository)
		{
			_cpuService = cpuService;
		}

		private static FilterExpression[]? FilterExpressions { get; set; }

		public async Task<TopologyDto> GetOrAddTopologyAsync(
			long cpuId,
			TopologyDto topology,
			CancellationToken cancellationToken = default
		)
		{
			var cpu = await _cpuService.GetSingleAsync(cpuId, cancellationToken);
			return await GetOrAddAsync(
				new TopologyDto(
					0,
					cpuId,
					cpu.Vendor,
					cpu.ModelName,
					topology.TotalLogicalCores,
					topology.TotalPhysicalCores,
					topology.TotalNumaNodes,
					topology.TotalPackages,
					topology.Root
				),
				cancellationToken
			);
		}

		protected override Expression<Func<TopologyModel, TopologyPreviewDto>> GetPreviewConstructionExpression()
		{
			return m => new TopologyPreviewDto(
				m.Id,
				m.Cpu.Id,
				m.Cpu.Vendor,
				m.Cpu.ModelName,
				m.TotalLogicalCores,
				m.TotalPhysicalCores,
				m.TotalNumaNodes,
				m.TotalPackages,
				m.TopologyHash
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
					FiltersTransformer.CreateFilter<TopologyModel, int>(
						"cpuLogicalCores",
						model => model.TotalLogicalCores
					),
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
			var topologyHash = GetTopologyHash(dto);
			return t =>
				t.Cpu.Id == dto.CpuId
				&& t.TopologyHash == topologyHash;
		}

		private static string GetTopologyHash(TopologyDto dto, string? serializedRoot = null)
		{
			serializedRoot ??= JsonConvert.SerializeObject(dto.Root);

			return serializedRoot.ToHashString();
		}
	}
}