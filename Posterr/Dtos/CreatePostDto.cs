using System.ComponentModel.DataAnnotations;

namespace Posterr.Dtos;

public class CreatePostDto
{
    public string? Content { get; init; }
    
    public long? RepostedPostId { get; init; }
    
    [Required]
    public string PostedByUsername { get; init; }

}