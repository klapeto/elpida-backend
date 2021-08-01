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

using Elpida.Backend.Data.Abstractions.Models.Benchmark;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Elpida;
using Elpida.Backend.Data.Abstractions.Models.Os;
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

		public DbSet<BenchmarkStatisticsModel> BenchmarkStatistics { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BenchmarkModel>()
				.HasMany(m => m.Tasks)
				.WithOne(m => m.Benchmark)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<BenchmarkTaskModel>()
				.HasOne(m => m.Task);

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
				.HasOne(m => m.Cpu);

			modelBuilder.Entity<BenchmarkResultModel>()
				.HasOne(m => m.Benchmark);

			modelBuilder.Entity<CpuModel>()
				.HasMany(m => m.BenchmarkStatistics)
				.WithOne(m => m.Cpu);

			modelBuilder.Entity<CpuModel>()
				.HasMany(m => m.Topologies)
				.WithOne(m => m.Cpu);

			modelBuilder.Entity<BenchmarkStatisticsModel>()
				.HasOne(m => m.Cpu);

			modelBuilder.Entity<BenchmarkStatisticsModel>()
				.HasOne(m => m.Benchmark);

			modelBuilder.Entity<BenchmarkStatisticsModel>()
				.Property(m => m.RowVersion)
				.IsRowVersion();
		}
	}
}