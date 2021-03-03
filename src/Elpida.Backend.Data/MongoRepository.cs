/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021  Ioannis Panagiotopoulos
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
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Elpida.Backend.Data
{
	public class MongoRepository<T> : IRepository<T> where T : IEntity
	{
		protected readonly IMongoCollection<T> Collection;

		public MongoRepository(IMongoCollection<T> collection)
		{
			Collection = collection ?? throw new ArgumentNullException(nameof(collection));
		}

		#region IRepository<T> Members

		public Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default)
		{
			return Collection.CountDocumentsAsync(FilterDefinition<T>.Empty, cancellationToken: cancellationToken);
		}

		public async Task<T> GetSingleAsync(string id, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("'Id' cannot be empty", nameof(id));
			}

			return (await Collection.FindAsync(e=> e.Id == id, cancellationToken: cancellationToken)).FirstOrDefault();
		}

		public async Task<string> CreateAsync(T model, CancellationToken cancellationToken = default)
		{
			if (model == null)
			{
				throw new ArgumentNullException(nameof(model));
			}

			await Collection.InsertOneAsync(model, cancellationToken: cancellationToken);
			return model.Id;
		}

		public Task<List<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> filters,
			CancellationToken cancellationToken = default)
		{
			var query = Collection.AsQueryable();

			if (filters != null)
			{
				query = filters.Aggregate(query, (current, filter) => current.Where(filter));
			}
			
			return query.ToListAsync(cancellationToken);
		}

		public Task DeleteAllAsync(CancellationToken cancellationToken = default)
		{
			return Collection.DeleteManyAsync(FilterDefinition<T>.Empty, cancellationToken);
		}

		public async Task<PagedQueryResult<T>> GetPagedAsync<TOrderKey>(
			int from,
			int count,
			bool descending,
			Expression<Func<T, TOrderKey>> orderBy,
			IEnumerable<Expression<Func<T, bool>>> filters,
			bool calculateTotalCount,
			CancellationToken cancellationToken = default)
		{
			if (from < 0)
			{
				throw new ArgumentException("'from' must be positive or 0", nameof(from));
			}

			if (count <= 0)
			{
				throw new ArgumentException("'count' must be positive", nameof(count));
			}

			var query = Collection.AsQueryable();

			if (filters != null)
			{
				query = filters.Aggregate(query, (current, filter) => current.Where(filter));
			}
			
			var totalCount = calculateTotalCount ? await query.CountAsync(cancellationToken) : 0;

			if (orderBy != null)
			{
				query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
			}

			var results = await query.Skip(from)
				.Take(count)
				.ToListAsync(cancellationToken);

			return new PagedQueryResult<T>(totalCount, results);
		}

		#endregion
	}
}