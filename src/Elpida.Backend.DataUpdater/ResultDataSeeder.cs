using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions.Dtos.Result;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elpida.Backend.DataUpdater
{
    public class ResultDataSeeder
    {
        private readonly ILogger<ResultDataSeeder> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ResultDataSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<ResultDataSeeder>();
        }

        public async Task SeedAsync(IEnumerable<string> resultsFiles)
        {
            var resultService = _serviceProvider.GetRequiredService<IBenchmarkResultsService>();

            foreach (var file in resultsFiles)
            {
                using (_logger.BeginScope($"Seeding data from file: '{file}'"))
                {
                    try
                    {
                        var resultData = JsonConvert.DeserializeObject<List<ResultDto>>(await File.ReadAllTextAsync(file));

                        foreach (var resultDto in resultData)
                        {
                            _logger.LogInformation($"Seeding result: '{resultDto.Result.Name}': '{resultDto.Result.Uuid}'");
                            await resultService.GetOrAddAsync(resultDto);
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