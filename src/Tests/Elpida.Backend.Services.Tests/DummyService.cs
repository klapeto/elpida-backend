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
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Utilities;

namespace Elpida.Backend.Services.Tests
{
	public class DummyService : Service<DummyDto, DummyModel, IDummyRepository>
	{
		private readonly Expression<Func<DummyModel, bool>>? _bypassCheck;

		public DummyService(IDummyRepository repository, ILockFactory lockFactory, Expression<Func<DummyModel, bool>>? bypassCheck = null)
			: base(repository, lockFactory)
		{
			_bypassCheck = bypassCheck;
		}

		public IReadOnlyDictionary<string, LambdaExpression> GetImplementedFilters()
		{
			return new Dictionary<string, LambdaExpression>();
		}

		protected override IEnumerable<FilterExpression> GetFilterExpressions()
		{
			yield return FiltersTransformer.CreateFilter<DummyModel, long>("id", m => m.Id);
			yield return FiltersTransformer.CreateFilter<DummyModel, string>("data", m => m.Data);
		}

		protected override Expression<Func<DummyModel, bool>>? GetCreationBypassCheckExpression(DummyDto dto)
		{
			return _bypassCheck;
		}

		protected override DummyDto ToDto(DummyModel model)
		{
			return new ()
			{
				Id = model.Id,
				Data = model.Data,
			};
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