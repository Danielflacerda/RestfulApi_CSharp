using Microsoft.AspNetCore.Mvc;
using Posterr.Model;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class PosterrPostsController : ControllerBase
{
    //This can become a session variable in future for better implementation.
    public int _lastPostsAmountRecovered; 

    private readonly ILogger<PosterrPostsController> _logger;

    public PosterrPostsController(ILogger<PosterrPostsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{filtered}")]
    public async Task<ActionResult<List<Post>>> GetAsync(bool filtered)
    {
        return Ok(new Post());
    }

    [HttpPost("{value}")]
    public async Task PostAsync([FromBody] Post value)
    {
        
    }
}
