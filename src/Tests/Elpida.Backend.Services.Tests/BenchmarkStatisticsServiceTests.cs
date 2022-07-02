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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Interfaces;
using Elpida.Backend.Data.Abstractions.Models.Statistics;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Moq;
using NUnit.Framework;

namespace Elpida.Backend.Services.Tests
{
	[TestFixture]
	internal class BenchmarkStatisticsServiceTests
	{
		private BenchmarkStatisticsService _service = default!;
		private Mock<IBenchmarkService> _benchmarkService = default!;
		private Mock<IBenchmarkStatisticsRepository> _benchmarkStatisticsRepository = default!;
		private Mock<ICpuService> _cpuService = default!;
		private Mock<IBenchmarkResultRepository> _benchmarkResultsRepository = default!;
		private Mock<ITransaction> _transaction = default!;

		[SetUp]
		public void Setup()
		{
			_benchmarkService = new Mock<IBenchmarkService>(MockBehavior.Strict);
			_benchmarkStatisticsRepository = new Mock<IBenchmarkStatisticsRepository>(MockBehavior.Strict);
			_cpuService = new Mock<ICpuService>(MockBehavior.Strict);
			_benchmarkResultsRepository = new Mock<IBenchmarkResultRepository>(MockBehavior.Strict);
			_transaction = new Mock<ITransaction>(MockBehavior.Strict);

			_service = new BenchmarkStatisticsService(
				_benchmarkService.Object,
				_benchmarkStatisticsRepository.Object,
				_cpuService.Object,
				_benchmarkResultsRepository.Object
			);
		}

		[Test]
		public async Task Success()
		{
			const int benchmarkId = 8;
			const int cpuId = 4;

			var returnStatistics = ModelGenerators.NewBenchmarkStatistics();
			var returnBasicStatistics = ModelGenerators.NewBasicStatistics();

			_transaction.Setup(t => t.CommitAsync(default))
				.Returns(Task.CompletedTask);

			_transaction.Setup(t => t.Dispose());

			_benchmarkStatisticsRepository.Setup(r => r.BeginTransactionAsync(default))
				.ReturnsAsync(_transaction.Object);

			_benchmarkStatisticsRepository.Setup(
					r => r.GetSingleAsync(It.IsAny<Expression<Func<BenchmarkStatisticsModel, bool>>>(), default)
				)
				.ReturnsAsync(returnStatistics);

			_benchmarkResultsRepository.Setup(r => r.GetStatisticsAsync(benchmarkId, cpuId, default))
				.ReturnsAsync(returnBasicStatistics);

			_benchmarkResultsRepository.Setup(
					r => r.GetCountWithScoreBetween(
						returnStatistics.Benchmark.Id,
						returnStatistics.Cpu.Id,
						It.IsAny<double>(),
						It.IsAny<double>(),
						default
					)
				)
				.ReturnsAsync(2);

			_benchmarkStatisticsRepository.Setup(r => r.SaveChangesAsync(default))
				.Returns(Task.CompletedTask);

			var previousMax = returnStatistics.Max;
			var previousMean = returnStatistics.Mean;
			var previousMin = returnStatistics.Min;
			var previousSampleSize = returnStatistics.SampleSize;

			await _service.UpdateTaskStatisticsAsync(benchmarkId, cpuId);

			Assert.AreNotEqual(previousMax, returnStatistics.Max);
			Assert.AreNotEqual(previousMean, returnStatistics.Mean);
			Assert.AreNotEqual(previousMin, returnStatistics.Min);
			Assert.AreNotEqual(previousSampleSize, returnStatistics.SampleSize);
		}
	}
}