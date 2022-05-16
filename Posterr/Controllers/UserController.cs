using Microsoft.AspNetCore.Mvc;
using Posterr.Model;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{value}")]
    public async Task<ActionResult<string>> GetAsync(string value)
    {
        return Ok(new User());
    }

    // This method will receive an username, compare with sessionUser username so it can return
    // an boolean that will inform if the sessionUser follows received user.
    [HttpGet("{username}")]
    public async Task<ActionResult<string>> FollowUserAsync(string username)
    {
        return Ok();
    }

    [HttpPost("{value}")]
    public async Task FollowUnfollowAsync([FromBody] string value)
    {
        
    }
}
