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

using Elpida.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataUpdater
{
    public class UpdateElpidaDbContext : ElpidaContext
    {
        private readonly string _connectionString;
        private readonly ILoggerFactory _loggerFactory;

        public UpdateElpidaDbContext()
            : base(new DbContextOptions<ElpidaContext>())
        {
        }

        public UpdateElpidaDbContext(string connectionString, ILoggerFactory loggerFactory)
            : base(new DbContextOptions<ElpidaContext>())
        {
            _connectionString = connectionString;
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=../../../../Elpida.Backend/results.db");
            options.UseLoggerFactory(_loggerFactory);
            //options.UseSqlite(_connectionString);
        }
    }
}