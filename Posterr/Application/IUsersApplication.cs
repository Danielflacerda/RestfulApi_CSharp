using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Application;
public interface IUsersApplication
{
    Task<Response<UserDto>> GetAsync(string userName);
    Task<Response<string>> FollowUnfollowAsync(bool followUnfollow, string targetUsername);   
    Task<Response<bool?>> FollowedByUserAsync(string targetUsername);
}