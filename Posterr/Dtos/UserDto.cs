using System.ComponentModel.DataAnnotations;

namespace Posterr.Dtos;

public class UserDto
{
    public long Id { get; init; }
    
    [StringLength(14, ErrorMessage = "The username cannot have more than 14 characters. ")]  
    public string Username { get; init; }

    public string JoinedDate { get; init; }
    
    public long Followers { get; set; }
    
    public long Following { get; set; }
    
    public long PostsCount { get; set; }

}