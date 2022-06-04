using System.ComponentModel.DataAnnotations;

namespace Posterr.Entities;

// Using record instead of class so it becomes safier since we aren't going to change
// the object structure.
public record Post
{
    public long Id { get; init; }
    
    [StringLength(777, ErrorMessage = "The post cannot have more than 777 characters. ")]  
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    public string PostedByUsername { get; init; }
    
    public DateTime CreatedOn { get; init; }

}