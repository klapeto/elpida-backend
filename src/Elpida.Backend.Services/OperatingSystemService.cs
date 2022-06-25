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
using Elpida.Backend.Data.Abstractions.Models.Os;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Dtos.Os;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Elpida.Backend.Services.Extensions.Os;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services
{
	public class OperatingSystemService : Service<OperatingSystemDto, OperatingSystemDto, OperatingSystemModel, IOperatingSystemRepository>, IOperatingSystemService
	{
		private static readonly IEnumerable<FilterExpression> OsFilters = new List<FilterExpression>
		{
			FiltersTransformer.CreateFilter<OperatingSystemModel, string>("osCategory", model => model.Category),
			FiltersTransformer.CreateFilter<OperatingSystemModel, string>("osName", model => model.Name),
			FiltersTransformer.CreateFilter<OperatingSystemModel, string>("osVersion", model => model.Version),
		};

		public OperatingSystemService(IOperatingSystemRepository operatingSystemRepository)
			: base(operatingSystemRepository)
		{
		}

		public override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			return OsFilters;
		}

		protected override Expression<Func<OperatingSystemModel, OperatingSystemDto>> GetPreviewConstructionExpression()
		{
			return m => new OperatingSystemDto(m.Id, m.Category, m.Name, m.Version);
		}

		protected override Task<OperatingSystemModel> ProcessDtoAndCreateModelAsync(OperatingSystemDto dto, CancellationToken cancellationToken)
		{
			return Task.FromResult(
				new OperatingSystemModel
				{
					Id = dto.Id,
					Category = dto.Category,
					Name = dto.Name,
					Version = dto.Version,
				}
			);
		}

		protected override OperatingSystemDto ToDto(OperatingSystemModel model)
		{
			return model.ToDto();
		}

		protected override Expression<Func<OperatingSystemModel, bool>> GetCreationBypassCheckExpression(OperatingSystemDto dto)
		{
			return o =>
				o.Category == dto.Category
				&& o.Name == dto.Name
				&& o.Version == dto.Version;
		}
	}
}