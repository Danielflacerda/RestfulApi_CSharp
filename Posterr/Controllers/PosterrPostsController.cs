using Microsoft.AspNetCore.Mvc;

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

    [HttpGet(Name = "GetPosts")]
    public IEnumerable<Post> Get()
    {
    }

    [HttpGet(Name = "GetPosts")]
    public IEnumerable<Post> Get()
    {
    }

    [HttpGet(Name = "CreatePosts")]
    public IEnumerable<Post> Post()
    {
    }
}
