using Microsoft.AspNetCore.Mvc;
using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Repositories;

namespace Posterr.Application;

public class UsersApplication : ControllerBase
{

    public User sessionUser = new User{
        Id = 1,
        Username = "TheJoshua",
        CreatedOn = DateTime.Parse("01/01/2015"),
        Followers = new List<string>{"Biden", "Obama", "Trump"},
        Following = new List<string>{"Biden", "Obama", "LucasRmax", "JhonnyUchiha"},
        PostsCount = 12
    };
    private readonly IUsersRepository _usersRepository;

    public UsersApplication(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }


    public async Task<Response<UserDto>> GetAsync(string userName)
    {
            var user = (await _usersRepository.GetUserAsync(userName)).UserAsDto();
            
            if (user is null){
                return new Response<UserDto>(null, "User was not found, try another username", false);
            }
            else
                return new Response<UserDto>(user, null, true);
    }

    public async Task<Response<string>> FollowUnfollowAsync(bool followUnfollow, string targetUsername)
    {
            if (sessionUser.Username == targetUsername)
                return new Response<string>("", "User cannot follow himself!", false);
            
            var targetUser = _usersRepository.GetUserAsync(targetUsername).Result;
            if(targetUser != null){ // Check if TargetUser exists
                if (followUnfollow){ // Check if it's a follow action or a unfollow action
                    if(targetUser.Followers.Contains(sessionUser.Username)){ // Checks if user already follows target
                        return new Response<string>("", "@" + sessionUser.Username + " already follows @" + targetUsername + "!", false);
                    }
                    else{
                        _usersRepository.FollowUnfollowUser(followUnfollow, sessionUser.Username, targetUser);
                        return new Response<string>("", "@" + sessionUser.Username + " followed @" + targetUsername + " succesfully!", true);
                    }
                }
                else{
                    if(!targetUser.Followers.Contains(sessionUser.Username)){ // Checks if user already do not follow target
                        return new Response<string>("", "@" + sessionUser.Username + " do not follow @" + targetUsername + "!", false);
                    }
                    else{
                        _usersRepository.FollowUnfollowUser(followUnfollow, sessionUser.Username, targetUser);
                        return new Response<string>("", "@" + sessionUser.Username + " unfollowed @" + targetUsername + " succesfully!", true);
                    }
                }
            }
            else                    
                return new Response<string>("", "@" + targetUsername + " does not exist!", false);
    }

    
    public async Task<Response<bool?>> FollowedByUserAsync(string targetUsername)
    {
            var targetUser = await _usersRepository.GetUserAsync(targetUsername);
            
            if (targetUser is null){
                return new Response<bool?>(null, "User was not found, try another username", false);
            }
            else{
                if(targetUser.Followers.Contains(sessionUser.Username))
                    return new Response<bool?>(true, "@" + sessionUser.Username + " follows @" + targetUsername + "!", true);
                else
                    return new Response<bool?>(false, "@" + sessionUser.Username + " do not follow @" + targetUsername + "!", false);
            }
    }
}
