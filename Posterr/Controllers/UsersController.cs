using Microsoft.AspNetCore.Mvc;
using Posterr.Application;
using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Repositories;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    public User sessionUser = new User{
        Id = 1,
        Username = "TheJoshua",
        CreatedOn = DateTime.Parse("01/01/2015"),
        Followers = new List<string>{"Biden", "Obama", "Trump"},
        Following = new List<string>{"Biden", "Obama", "Trump", "JhonnyUchiha"},
        PostsCount = 12
    };
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersApplication _usersApplication;

    public UsersController(ILogger<UsersController> logger, IUsersApplication usersApplication)
    {
        _logger = logger;
        _usersApplication = usersApplication;
    }

    // This method receives the userName we want to search in the database
    [HttpGet("GetUser")]
    public async Task<ActionResult<UserDto>> GetAsync(string userName)
    {
        try{
            return Ok(await _usersApplication.GetAsync(userName));
        }
        catch (Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }

    }

    // This method receives a boolean that indicates if it is to follow (true) or to unfollow (False) the targetUsername received too.
    [HttpPut("FollowUnfollowAsync")]
    public async Task<ActionResult<Response<string>>> FollowUnfollowAsync(bool followUnfollow, string targetUsername)
    {
        try{
            return Ok(await _usersApplication.FollowUnfollowAsync(followUnfollow, targetUsername));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }

    

    // This method will receive the targetUsername that we want to look for in the "Following" list of the sessionUser "TheJoshua".
    [HttpGet("FollowedByUserAsync")]
    public async Task<ActionResult<bool>> FollowedByUserAsync(string targetUsername)
    {
        try{
            return Ok(await _usersApplication.FollowedByUserAsync(targetUsername));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }
}
