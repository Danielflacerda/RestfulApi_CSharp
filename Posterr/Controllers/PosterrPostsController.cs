using Microsoft.AspNetCore.Mvc;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class PosterrPostsController : ControllerBase
{

    private readonly ILogger<PosterrPostsController> _logger;

    public PosterrPostsController(ILogger<PosterrPostsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetPosts")]
    public IEnumerable<Post> Get()
    {
    }
}
