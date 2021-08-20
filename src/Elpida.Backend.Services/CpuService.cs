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
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Cpu;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Cpu;
using Newtonsoft.Json;

namespace Elpida.Backend.Services
{
	public class CpuService : Service<CpuDto, CpuModel, ICpuRepository>, ICpuService
	{
		public CpuService(ICpuRepository cpuRepository, ILockFactory lockFactory)
			: base(cpuRepository, lockFactory)
		{
		}

		private static IEnumerable<FilterExpression> CpuExpressions { get; } = new List<FilterExpression>
		{
			CreateFilter("cpuModelName", model => model.ModelName),
			CreateFilter("cpuVendor", model => model.Vendor),
			CreateFilter("cpuFrequency", model => model.Frequency),
		};

		public Task<PagedResult<CpuPreviewDto>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		)
		{
			return GetPagedProjectionsAsync(
				queryRequest,
				m => new CpuPreviewDto
				{
					Id = m.Id,
					Vendor = m.Vendor,
					ModelName = m.ModelName,
					TopologiesCount = m.Topologies.Count,
					TaskStatisticsCount = m.BenchmarkStatistics.Count,
				},
				cancellationToken
			);
		}

		protected override Task<CpuModel> ProcessDtoAndCreateModelAsync(CpuDto dto, CancellationToken cancellationToken)
		{
			return Task.FromResult(
				new CpuModel
				{
					Id = dto.Id,
					Architecture = dto.Architecture,
					ModelName = dto.ModelName,
					Caches = JsonConvert.SerializeObject(dto.Caches),
					Features = JsonConvert.SerializeObject(dto.Features),
					Frequency = dto.Frequency,
					Smt = dto.Smt,
					Vendor = dto.Vendor,
					AdditionalInfo = JsonConvert.SerializeObject(dto.AdditionalInfo),
				}
			);
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
			var additionalInfo = JsonConvert.SerializeObject(dto.AdditionalInfo);
			return model =>
				model.Vendor == dto.Vendor
				&& model.ModelName == dto.ModelName
				&& model.AdditionalInfo == additionalInfo;
		}
	}
}