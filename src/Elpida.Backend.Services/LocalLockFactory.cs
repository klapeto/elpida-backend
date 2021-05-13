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
using System.Collections.Generic;
using Elpida.Backend.Services.Abstractions.Interfaces;

namespace Elpida.Backend.Services
{
    public class LocalLockFactory : ILockFactory
    {
        private readonly object _locker = new object();
        private readonly Dictionary<string, LocalLock> _locks = new Dictionary<string, LocalLock>();

        public IDisposable Acquire(string name)
        {
            LocalLock returnLock;
            lock (_locker)
            {
                if (!_locks.TryGetValue(name, out returnLock))
                {
                    returnLock = new LocalLock();
                    _locks.Add(name, returnLock);
                }
            }

            returnLock.Acquire();

            return returnLock;
        }
    }
}