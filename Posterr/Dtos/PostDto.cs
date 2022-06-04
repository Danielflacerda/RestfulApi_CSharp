namespace Posterr.Dtos;

public record PostDto
{
    public long Id { get; init; }
    
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    public string PostedByUsername { get; init; }
    
    public DateTime CreatedOn { get; init; }

}