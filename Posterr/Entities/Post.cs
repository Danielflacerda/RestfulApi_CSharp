namespace Posterr.Entities;

// Using record instead of class so it becomes safier since we aren't going to change
// the object structure.
public record Post
{
    public long PostId { get; init; }
    
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    public long PostedByUserId { get; init; }
    
    public DateTime CreatedOn { get; init; }

}