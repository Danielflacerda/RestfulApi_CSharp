using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Repositories;
public interface ICountersRepository
{
    Task<Counter> GetCounterAsync(string id);
    Task UpdateCounterAsync(string id);
    Task UpdateCounterAsync(string id, long postCounter);
}