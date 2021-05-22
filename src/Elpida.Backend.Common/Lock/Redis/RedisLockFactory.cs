/*
 * Elpida HTTP Rest API
 *   
 * Copyright (C) 2021 Ioannis Panagiotopoulos
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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Elpida.Backend.Common.Lock.Redis
{
    public class RedisLockFactory : ILockFactory, IDisposable
    {
        private readonly RedLockFactory _factory;
        private readonly RedisOptions _redisOptions;

        public RedisLockFactory(IOptions<RedisOptions> redisOptions, ILoggerFactory loggerFactory)
        {
            _redisOptions = redisOptions.Value;
            _factory = RedLockFactory.Create(new RedLockMultiplexer[]
            {
                ConnectionMultiplexer.Connect(_redisOptions.ConnectionString)
            }, loggerFactory);
        }

        public IDisposable Acquire(string name)
        {
            return _factory.CreateLock(name,
                TimeSpan.FromMilliseconds(_redisOptions.ExpireTimeout),
                TimeSpan.FromMilliseconds(_redisOptions.WaitTimeout),
                TimeSpan.FromMilliseconds(_redisOptions.RetryInterval));
        }
        
        public async Task<IDisposable> AcquireAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _factory.CreateLockAsync(name,
                TimeSpan.FromMilliseconds(_redisOptions.ExpireTimeout), 
                TimeSpan.FromMilliseconds(_redisOptions.WaitTimeout),
                TimeSpan.FromMilliseconds(_redisOptions.RetryInterval),
                cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _factory?.Dispose();
        }
    }
}