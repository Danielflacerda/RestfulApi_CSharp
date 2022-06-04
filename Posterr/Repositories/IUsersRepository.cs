using Posterr.Entities;

namespace Posterr.Repositories;
public interface IUsersRepository
{
    Task<User> GetUserAsync(string userName);
    Task<IEnumerable<User>> GetUsersAsync();
    Task UpdateUserPostCounter(string username);
}