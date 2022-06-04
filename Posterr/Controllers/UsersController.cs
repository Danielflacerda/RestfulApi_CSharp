using Microsoft.AspNetCore.Mvc;
using Posterr.Entities;
using Posterr.Repositories;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _usersRepository;

    public UsersController(ILogger<UsersController> logger, IUsersRepository usersRepository)
    {
        _logger = logger;
        _usersRepository = usersRepository;
    }



    [HttpGet()]
    public async Task<ActionResult<string>> GetAsync(string userName)
    {
        try{
            var post = await _usersRepository.GetUserAsync(userName);
            
            if (post is null){
                return NotFound();
            }
            else
                return Ok(post);

        }
        catch (Exception ex){
            return BadRequest(ex.Message);
        }

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
