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

using Elpida.Backend.Common.Lock.Local;
using Elpida.Backend.Common.Lock.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Elpida.Backend.Common.Lock
{
	public static class ServicesCollectionExtensions
	{
		public static IServiceCollection AddLocalLocks(this IServiceCollection collection)
		{
			collection.AddSingleton<ILockFactory, LocalLockFactory>();

			return collection;
		}

		public static IServiceCollection AddRedisLocks(this IServiceCollection collection)
		{
			collection.AddOptions();
			collection.AddSingleton<ILockFactory, RedisLockFactory>();
			return collection;
		}
	}
}