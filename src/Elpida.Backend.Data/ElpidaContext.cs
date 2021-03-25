using Elpida.Backend.Data.Abstractions.Models;
using Elpida.Backend.Data.Abstractions.Models.Cpu;
using Elpida.Backend.Data.Abstractions.Models.Result;
using Elpida.Backend.Data.Abstractions.Models.Task;
using Elpida.Backend.Data.Abstractions.Models.Topology;
using Microsoft.EntityFrameworkCore;

namespace Elpida.Backend.Data
{
	public class ElpidaContext : DbContext
	{
		public DbSet<BenchmarkModel> Benchmarks { get; set; } = default!;
		public DbSet<TaskModel> Tasks { get; set; } = default!;
		public DbSet<CpuModel> Cpus { get; set; } = default!;
		public DbSet<TopologyModel> Topologies { get; set; } = default!;
		public DbSet<ResultModel> Results { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BenchmarkModel>()
				.HasMany(m => m.Tasks)
				.WithMany(m => m.Benchmarks);
			
			modelBuilder.Entity<TaskResultModel>()
				.HasOne(m => m.Task);

			modelBuilder.Entity<ResultModel>()
				.HasMany(m => m.TaskResults)
				.WithOne(m => m.Result)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<ResultModel>()
				.HasOne(m => m.Topology);

			modelBuilder.Entity<ResultModel>()
				.HasOne(m => m.Benchmark);
		}

		public ElpidaContext(DbContextOptions<ElpidaContext> contextOptions)
			: base(contextOptions)
		{
		}
	}
}