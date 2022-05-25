namespace Posterr.Dtos;

public record PostDto
{
    public long PostId { get; init; }
    
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    public long PostedByUserId { get; init; }
    
    public DateTime CreatedOn { get; init; }

}