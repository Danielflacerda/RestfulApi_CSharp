using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Filter;

namespace Posterr.Application;
public interface IPostsApplication
{
    Task<PagedResponse<List<PostDto>>> GetPostsHomePage(PaginationFilter filter, bool filteredByFollowing);
    Task<PagedResponse<List<PostDto>>> GetPostsUserPage(PaginationFilter filter, string username);   
    Task<Response<Post>> CreatePostAsync(CreatePostDto value);
    Task<PagedResponse<List<PostDto>>> SearchAsync(PaginationFilter filter, string searchContent);
}