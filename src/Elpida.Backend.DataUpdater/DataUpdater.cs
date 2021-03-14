using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Data.Abstractions.Repositories;
using Elpida.Backend.Services.Extensions.Benchmark;
using Elpida.Backend.Services.Extensions.Task;
using Microsoft.Extensions.Logging;

namespace Elpida.Backend.DataUpdater
{
	public class DataUpdater
	{
		private readonly IBenchmarkRepository _benchmarkRepository;
		private readonly ITaskRepository _taskRepository;
		private readonly Data _data;
		private readonly ILogger<DataUpdater> _logger;

		public DataUpdater(Data data,
			IBenchmarkRepository benchmarkRepository,
			ITaskRepository taskRepository,
			ILogger<DataUpdater> logger)
		{
			_data = data;
			_benchmarkRepository = benchmarkRepository;
			_taskRepository = taskRepository;
			_logger = logger;
		}

		public async Task EnsureUpdatedAsync(CancellationToken cancellationToken)
		{
			using (_logger.BeginScope("Ensure Updated"))
			{
				using (_logger.BeginScope("Updating tasks"))
				{
					foreach (var task in _data.Tasks)
					{
						var existing = await _taskRepository.GetSingleAsync(task.Id, cancellationToken);
						if (existing == null)
						{
							_logger.LogInformation($"Creating new task: '{task.Name}'");
							await _taskRepository.CreateAsync(task.ToModel(), cancellationToken);
						}
						else
						{
							_logger.LogInformation($"Updating task: '{task.Name}'");
							await _taskRepository.UpdateAsync(task.ToModel(), cancellationToken);
						}
					}
				}

				using (_logger.BeginScope("Seeding benchmarks"))
				{
					foreach (var benchmark in _data.Benchmarks)
					{
						var existing = await _benchmarkRepository.GetSingleAsync(benchmark.Id, cancellationToken);
						if (existing == null)
						{
							_logger.LogInformation($"Creating new benchmark: '{benchmark.Name}'");
							await _benchmarkRepository.CreateAsync(benchmark.ToModel(), cancellationToken);
						}
						else
						{
							_logger.LogInformation($"Updating benchmark: '{benchmark.Name}'");
							await _benchmarkRepository.UpdateAsync(benchmark.ToModel(), cancellationToken);
						}
					}
				}
			}
		}
	}
}