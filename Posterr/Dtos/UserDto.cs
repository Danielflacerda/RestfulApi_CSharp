using System.ComponentModel.DataAnnotations;

namespace Posterr.Dtos;

// Using record instead of class so it becomes more safe since we aren't going to change
// the object structure.
public record UserDto
{
    public long Id { get; init; }
    
    [StringLength(14, ErrorMessage = "The username cannot have more than 14 characters. ")]  
    public string Username { get; init; }
    
    public DateTime CreatedOn { get; init; }
    
    public List<string> Followers { get; set; }
    
    public List<string> Following { get; set; }
    
    public long PostsCount { get; set; }

}