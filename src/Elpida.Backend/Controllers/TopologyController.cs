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
    public class TopologyController : ControllerBase
    {
        private readonly ITopologyService _topologyService;

        public TopologyController(ITopologyService topologyService)
        {
            _topologyService = topologyService;
        }
        
        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            return Ok(await _topologyService.GetSingleAsync(id, cancellationToken));
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPagedPreviews([FromQuery] PageRequest pageRequest,
            CancellationToken cancellationToken)
        {
            return Ok(await _topologyService.GetPagedPreviewsAsync(new QueryRequest {PageRequest = pageRequest},
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
            QueryRequestUtilities.PreprocessQuery(queryRequest);

            return Ok(await _topologyService.GetPagedPreviewsAsync(queryRequest, cancellationToken));
        }
    }
}