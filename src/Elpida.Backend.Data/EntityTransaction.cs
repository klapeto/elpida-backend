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
using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Elpida.Backend.Data
{
	public class EntityTransaction : ITransaction
	{
		private readonly IDbContextTransaction _transaction;

		private EntityTransaction(IDbContextTransaction transaction)
		{
			_transaction = transaction;
		}

		public static async Task<ITransaction> CreateAsync(DbContext context, CancellationToken cancellationToken = default)
		{
			var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
			return new EntityTransaction(transaction);
		}

		public void Dispose()
		{
			_transaction.Dispose();
			GC.SuppressFinalize(this);
		}

		public Task CommitAsync(CancellationToken cancellationToken = default)
		{
			return _transaction.CommitAsync(cancellationToken);
		}
	}
}