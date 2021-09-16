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
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Elpida.Backend.Services.Tests.Helpers
{
	internal class DummyBasicService : Service<DummyDto, DummyPreviewDto, DummyModel, IDummyRepository>
	{
		public DummyBasicService(IDummyRepository repository)
			: base(repository)
		{
		}

		public Expression<Func<DummyModel, DummyPreviewDto>> GetPreviewConstructionExpressionImpl()
		{
			return GetPreviewConstructionExpression();
		}

		protected override DummyDto ToDto(DummyModel model)
		{
			return new (model.Id, model.Data);
		}

		protected override Expression<Func<DummyModel, DummyPreviewDto>> GetPreviewConstructionExpression()
		{
			return m => new DummyPreviewDto(m.Id, m.Data);
		}

		protected override Task<DummyModel> ProcessDtoAndCreateModelAsync(
			DummyDto dto,
			CancellationToken cancellationToken
		)
		{
			return Task.FromResult(
				new DummyModel
				{
					Id = dto.Id,
					Data = dto.Data,
				}
			);
		}
	}
}