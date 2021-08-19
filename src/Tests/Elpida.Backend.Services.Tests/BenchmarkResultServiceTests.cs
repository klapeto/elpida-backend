using System.Threading.Tasks;
using Elpida.Backend.Common.Lock;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	public class BenchmarkResultServiceTests
	{
		private class Pack
		{
			public Pack()
			{
				BenchmarkResultRepo = new Mock<IBenchmarkResultsRepository>(MockBehavior.Strict);
				CpuRepo = new Mock<ICpuRepository>(MockBehavior.Strict);
				BenchmarkRepo = new Mock<IBenchmarkRepository>(MockBehavior.Strict);
				TopologyRepo = new Mock<ITopologyRepository>(MockBehavior.Strict);
				ElpidaRepo = new Mock<IElpidaRepository>(MockBehavior.Strict);
				OsRepo = new Mock<IOsRepository>(MockBehavior.Strict);
				TaskRepo = new Mock<ITaskRepository>(MockBehavior.Strict);

				StatisticsService = new Mock<IStatisticsUpdaterService>(MockBehavior.Strict);
				CpuService = new Mock<ICpuService>(MockBehavior.Strict);
				TopologyService = new Mock<ITopologyService>(MockBehavior.Strict);
				ElpidaService = new Mock<IElpidaService>(MockBehavior.Strict);
				OsService = new Mock<IOsService>(MockBehavior.Strict);
				BenchmarkService = new Mock<IBenchmarkService>(MockBehavior.Strict);
				LockFactory = new Mock<ILockFactory>(MockBehavior.Strict);

				Service = new BenchmarkResultService(
					BenchmarkResultRepo.Object,
					StatisticsService.Object,
					CpuRepo.Object,
					BenchmarkRepo.Object,
					TopologyRepo.Object,
					ElpidaRepo.Object,
					OsRepo.Object,
					TaskRepo.Object,
					CpuService.Object,
					TopologyService.Object,
					ElpidaService.Object,
					OsService.Object,
					BenchmarkService.Object,
					LockFactory.Object
				);
			}

			public Mock<IBenchmarkResultsRepository> BenchmarkResultRepo { get; }

			public Mock<ICpuRepository> CpuRepo { get; }

			public Mock<IBenchmarkRepository> BenchmarkRepo { get; }

			public Mock<ITopologyRepository> TopologyRepo { get; }

			public Mock<IElpidaRepository> ElpidaRepo { get; }

			public Mock<IOsRepository> OsRepo { get; }

			public Mock<ITaskRepository> TaskRepo { get; }

			public Mock<IStatisticsUpdaterService> StatisticsService { get; }

			public Mock<ICpuService> CpuService { get; }

			public Mock<ITopologyService> TopologyService { get; }

			public Mock<IElpidaService> ElpidaService { get; }

			public Mock<IOsService> OsService { get; }

			public Mock<IBenchmarkService> BenchmarkService { get; }

			public Mock<ILockFactory> LockFactory { get; }

			public BenchmarkResultService Service { get; }
		}
	}
}