using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Posterr.Application;
using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Filter;
using Posterr.Repositories;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IPostsApplication _postsApplication;

    public PostsController(ILogger<PostsController> logger, IPostsApplication postsApplication)
    {
        _logger = logger;
        _postsApplication = postsApplication;
    }


    // This method receives the number of the page from the pagination of registers, the size of data in the page and a boolean that indicates if the results will be 
    // filtered by followed users posts or not at the homePage, in our case it will always be users followed by Joshua.
    [HttpGet("GetPostsHomePage")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] PaginationFilter filter, bool filteredByFollowing)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, 10);
        try{
            return Ok(await _postsApplication.GetPostsHomePage(validFilter, filteredByFollowing));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }

    // This method receives the number of the page from the pagination of registers, the size of data in the page  and the username of the user we want to recover the 
    // post in userPage, in our case it will always be Joshua.
    [HttpGet("GetPostsUserPage")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] PaginationFilter filter, string username)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, 5);
        try{
            return Ok(await _postsApplication.GetPostsUserPage(validFilter, username));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }
    // This method receives the content of the new post, the id of the post it is reposting or quote posting and the username of the post writer, in our case
    // it will always be Joshua.
    [HttpPost("CreatePostAsync")]
    public async Task<IActionResult> CreatePostAsync(CreatePostDto value)
    {
        try{
            return Ok(await _postsApplication.CreatePostAsync(value));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }    
    
    // This method receives the number of the page we of register we want to receive, number of records per page and the content that will be searched
    // in posts database.
    [HttpGet("Search")]
    public async Task<IActionResult> SearchAsync([FromQuery] PaginationFilter filter, string searchContent)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageNumber);
        try{
            return Ok(await _postsApplication.SearchAsync(validFilter, searchContent));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }
}
