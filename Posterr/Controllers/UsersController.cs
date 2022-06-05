using Microsoft.AspNetCore.Mvc;
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
    private readonly IUsersRepository _usersRepository;

    public UsersController(ILogger<UsersController> logger, IUsersRepository usersRepository)
    {
        _logger = logger;
        _usersRepository = usersRepository;
    }



    [HttpGet("GetPost")]
    public async Task<ActionResult<UserDto>> GetAsync(string userName)
    {
        try{
            var user = (await _usersRepository.GetUserAsync(userName)).UserAsDto();
            
            if (user is null){
                return NotFound(new Response<string>("", "User was not found, try another username", false));
            }
            else
                return Ok(new Response<UserDto>(user, null, true));

        }
        catch (Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }

    }

    [HttpPut("FollowUnfollowAsync")]
    public async Task<ActionResult<Response<string>>> FollowUnfollowAsync(bool followUnfollow, string targetUsername)
    {
        try{
            if (sessionUser.Username == targetUsername)
                return BadRequest(new Response<string>("", "User cannot follow himself!", false));
            
            var targetUser = _usersRepository.GetUserAsync(targetUsername).Result;
            if(targetUser != null){ // Check if TargetUser exists
                if (followUnfollow){ // Check if it's a follow action or a unfollow action
                    if(targetUser.Followers.Contains(sessionUser.Username)){ // Checks if user already follows target
                        return BadRequest(new Response<string>("", "@" + sessionUser.Username + " already follows @" + targetUsername + "!", false));
                    }
                    else{
                        _usersRepository.FollowUnfollowUser(followUnfollow, sessionUser.Username, targetUser);
                        return Ok(new Response<string>("", "@" + sessionUser.Username + " followed @" + targetUsername + " succesfully!", true));
                    }
                }
                else{
                    if(!targetUser.Followers.Contains(sessionUser.Username)){ // Checks if user already do not follow target
                        return BadRequest(new Response<string>("", "@" + sessionUser.Username + " do not follow @" + targetUsername + "!", false));
                    }
                    else{
                        _usersRepository.FollowUnfollowUser(followUnfollow, sessionUser.Username, targetUser);
                        return Ok(new Response<string>("", "@" + sessionUser.Username + " unfollowed @" + targetUsername + " succesfully!", true));
                    }
                }
            }
            else                    
                return BadRequest(new Response<string>("", "@" + targetUsername + " does not exist!", false));
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
            var targetUser = await _usersRepository.GetUserAsync(targetUsername);
            
            if (targetUser is null){
                return NotFound(new Response<string>("", "User was not found, try another username", false));
            }
            else{
                if(targetUser.Followers.Contains(sessionUser.Username))
                    return Ok(new Response<bool>(true, "@" + sessionUser.Username + " follows @" + targetUsername + "!", true));
                else
                    return BadRequest(new Response<bool>(false, "@" + sessionUser.Username + " do not follow @" + targetUsername + "!", false));
            }
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }
}
