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
using Elpida.Backend.Services.Abstractions.Dtos;

namespace Elpida.Backend.Services.Abstractions.Interfaces
{
	public interface IService<TDto, TPreview>
		where TDto : FoundationDto
		where TPreview : FoundationDto
	{
		Task<TDto> GetSingleAsync(long id, CancellationToken cancellationToken = default);

		Task<PagedResult<TPreview>> GetPagedPreviewsAsync(
			QueryRequest queryRequest,
			CancellationToken cancellationToken = default
		);

		Task<TDto> GetOrAddAsync(TDto dto, CancellationToken cancellationToken = default);

		IEnumerable<FilterExpression> ConstructCustomFilters<T, TR>(Expression<Func<T, TR>> baseExpression);
	}
}