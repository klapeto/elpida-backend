using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
    public class BenchmarkDataSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BenchmarkDataSeeder> _logger;

        public BenchmarkDataSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<BenchmarkDataSeeder>();
        }

        public async Task SeedAsync(IEnumerable<string> dataFiles)
        {
            var benchmarkService = _serviceProvider.GetRequiredService<IBenchmarkService>();

            foreach (var file in dataFiles)
            {
                using (_logger.BeginScope($"Seeding data from file: '{file}'"))
                {
                    try
                    {
                        var benchmarkData = JsonConvert.DeserializeObject<BenchmarkData>(await File.ReadAllTextAsync(file));

                        foreach (var benchmark in benchmarkData.Benchmarks)
                        {
                            _logger.LogInformation($"Seeding benchmark data: '{benchmark.Name}': '{benchmark.Uuid}'");
                            await benchmarkService.GetOrAddAsync(benchmark);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Failed to seed data");
                    }
                }
            }
        }
    }
}