using System.Threading;
using System.Threading.Tasks;
using Elpida.Backend.Services.Abstractions;
using Elpida.Backend.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elpida.Backend.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BenchmarkStatisticsController : ControllerBase
    {
        private readonly IBenchmarkStatisticsService _benchmarkStatisticsService;

        public BenchmarkStatisticsController(IBenchmarkStatisticsService benchmarkStatisticsService)
        {
            _benchmarkStatisticsService = benchmarkStatisticsService;
        }
        
        [HttpGet("{id}", Name = nameof(GetSingleStatistic))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleStatistic([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (long.TryParse(id, out var lid))
            {
                return Ok(await _benchmarkStatisticsService.GetSingleAsync(lid, cancellationToken));
            }

            return BadRequest("Id must be an Integer number");
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaged([FromQuery] PageRequest pageRequest,
            CancellationToken cancellationToken)
        {
            return Ok(await _benchmarkStatisticsService.GetPagedPreviewsAsync(new QueryRequest {PageRequest = pageRequest},
                cancellationToken));
        }
        
        [HttpPost("Search")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] QueryRequest queryRequest,
            CancellationToken cancellationToken)
        {
            ValueUtilities.PreprocessQuery(queryRequest);

            return Ok(await _benchmarkStatisticsService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
        }
    }
}