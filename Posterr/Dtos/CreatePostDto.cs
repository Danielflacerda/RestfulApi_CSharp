using System.ComponentModel.DataAnnotations;

namespace Posterr.Dtos;

public record CreatePostDto
{
    [Required]
    public string Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    [Required]
    public long PostedByUserId { get; init; }

}