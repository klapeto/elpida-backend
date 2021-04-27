using System.Linq;
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
    public class CpuController: ControllerBase
    {
        private readonly ICpuService _cpuService;
        private readonly ITaskStatisticsService _taskStatisticsService;

        public CpuController(ICpuService cpuService, ITaskStatisticsService taskStatisticsService)
        {
            _cpuService = cpuService;
            _taskStatisticsService = taskStatisticsService;
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
            return Ok(await _cpuService.GetPagedPreviewsAsync(new QueryRequest {PageRequest = pageRequest},
                cancellationToken));
        }
        
        [HttpGet("{id}", Name = nameof(GetSingleCpu))]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleCpu([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (long.TryParse(id, out var lid))
            {
                return Ok(await _cpuService.GetSingleAsync(lid, cancellationToken));
            }

            return BadRequest("Id must be an Integer number");
        }
        
        [HttpGet("{id}/TaskStatistics")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTaskStatistics(
            [FromRoute] string id, 
            [FromQuery] PageRequest pageRequest, 
            CancellationToken cancellationToken)
        {
            if (long.TryParse(id, out var lid))
            {
                var queryRequest = new QueryRequest
                {
                    Filters = new[]
                    {
                        GetQueryInstanceForId(lid)
                    },
                    Descending = false,
                    OrderBy = "taskId",
                    PageRequest = pageRequest
                };
                    
                return Ok(await _taskStatisticsService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
            }

            return BadRequest("Id must be an Integer number");
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
            
            return Ok(await _cpuService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
        }
        
        [HttpPost("{id}/TaskStatistics/Search")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchTaskStatistics(
            [FromRoute] string id,
            [FromBody] QueryRequest queryRequest,
            CancellationToken cancellationToken)
        {
            ValueUtilities.PreprocessQuery(queryRequest);
            
            if (long.TryParse(id, out var lid))
            {
                var additionalFilters = new[] {GetQueryInstanceForId(lid)};
                queryRequest.Filters = queryRequest.Filters?.Concat(additionalFilters).ToArray() ?? additionalFilters;
                
                return Ok(await _taskStatisticsService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
            }

            return BadRequest("Id must be an Integer number");
        }
        
        private static QueryInstance GetQueryInstanceForId(long cpuId)
        {
            return new()
            {
                Comp = FilterHelpers.ComparisonMap[FilterHelpers.Comparison.Equal],
                Name = "cpuId",
                Value = cpuId
            };
        }
    }
}