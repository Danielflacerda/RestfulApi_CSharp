namespace Posterr.Dtos;

public record CreatePostDto
{
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    public long PostedByUserId { get; init; }

}