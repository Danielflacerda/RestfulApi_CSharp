namespace Posterr.Entities;

// Using record instead of class so it becomes more safe since we aren't going to change
// the object structure.
public record User
{
    public Guid Id { get; init; }
    
    public string Username { get; init; }
    
    public DateTime CreatedOn { get; init; }
    
    public List<string> Followers { get; set; }
    
    public List<string> Following { get; set; }
    
    public long PostsCount { get; set; }

}