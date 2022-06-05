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
}
