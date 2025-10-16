using Microsoft.AspNetCore.Mvc;
using PruebaBff.Core.Interfaces;

namespace PruebaBff.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : ControllerBase
    {

    private readonly IRickAndMortyService _service;
        private readonly ILogger<EpisodesController> _logger;
        public EpisodesController(IRickAndMortyService service, ILogger<EpisodesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetEpisodes([FromQuery] int page = 1)
        {
            try
            {
                var (episodes, totalPages, totalCount) = await _service.GetEpisodesAsync(page);
                var result = new
                {
                    Data = episodes,
                    Pagination = new { Page = page, TotalPages = totalPages, TotalCount = totalCount }
                };
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling external API");
                return StatusCode(502, new { Message = "Error retrieving data from external service." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEpisode(int id)
        {
            var ep = await _service.GetEpisodeByIdAsync(id);
            if (ep == null) return NotFound();
            return Ok(ep);
        }
    }
}

