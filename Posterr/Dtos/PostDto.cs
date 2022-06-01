namespace Posterr.Dtos;

public record PostDto
{
    public Guid Id { get; init; }
    
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    public long PostedByUserId { get; init; }
    
    public DateTimeOffset CreatedOn { get; init; }

}