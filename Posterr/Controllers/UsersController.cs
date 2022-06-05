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


    [HttpGet("GetPost")]
    public async Task<ActionResult<UserDto>> GetAsync(string userName)
    {
        try{
            return Ok(await _usersApplication.GetAsync(userName));
        }
        catch (Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }

    }

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

    

    // This method will receive an username, compare with sessionUser username so it can return
    // an boolean that will inform if the sessionUser follows received user.
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
