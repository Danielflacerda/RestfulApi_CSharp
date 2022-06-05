using Posterr.Entities;

namespace Posterr.Repositories;
public interface IUsersRepository
{
    Task<User> GetUserAsync(string userName);
    Task FollowUnfollowUser(bool followUnfollow, string sessionUsername, User targetUser);
    Task UpdateUserPostCounter(string username);
}