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

using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
    public class ElpidaContext : DbContext
    {
        public ElpidaContext(DbContextOptions contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<BenchmarkModel> Benchmarks { get; set; } = default!;
        public DbSet<TaskModel> Tasks { get; set; } = default!;
        public DbSet<CpuModel> Cpus { get; set; } = default!;
        public DbSet<TopologyModel> Topologies { get; set; } = default!;
        public DbSet<BenchmarkResultModel> BenchmarkResults { get; set; } = default!;
        public DbSet<TaskResultModel> TaskResults { get; set; } = default!;
        public DbSet<ElpidaModel> Elpidas { get; set; } = default!;
        public DbSet<OsModel> Oses { get; set; } = default!;
        public DbSet<TaskStatisticsModel> TaskStatistics { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BenchmarkModel>()
                .HasMany(m => m.Tasks)
                .WithMany(m => m.Benchmarks);

            modelBuilder.Entity<TaskResultModel>()
                .HasOne(m => m.Task);

            modelBuilder.Entity<TaskResultModel>()
                .HasOne(m => m.Topology);

            modelBuilder.Entity<TaskResultModel>()
                .HasOne(m => m.Cpu);

            modelBuilder.Entity<BenchmarkResultModel>()
                .HasMany(m => m.TaskResults)
                .WithOne(m => m.BenchmarkResult)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BenchmarkResultModel>()
                .HasOne(m => m.Topology);

            modelBuilder.Entity<BenchmarkResultModel>()
                .HasOne(m => m.Benchmark);

            modelBuilder.Entity<CpuModel>()
                .HasMany(m => m.TaskStatistics)
                .WithOne(m => m.Cpu);

            modelBuilder.Entity<TaskStatisticsModel>()
                .HasOne(m => m.Cpu);

            modelBuilder.Entity<TaskStatisticsModel>()
                .HasOne(m => m.Topology);
            
            modelBuilder.Entity<TaskStatisticsModel>()
                .HasOne(m => m.Task);
        }
    }
}