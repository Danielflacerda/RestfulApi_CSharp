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
    // If i had a session control i would use a sessionUser variable for when current logged user data is necessary,
    // because of that i'm going to hardcode current user so the api methods works as it should.
    public User sessionUser = new User{
        Id = 1,
        Username = "TheJoshua",
        CreatedOn = DateTime.Parse("01/01/2015"),
        Followers = new List<string>{"Biden", "Obama", "Trump"},
        Following = new List<string>{"Biden", "Obama", "Trump", "JhonnyUchiha"},
        PostsCount = 12
    };
    private readonly ILogger<PostsController> _logger;
    private readonly IPostsApplication _postsApplication;
    private readonly ICountersRepository _countersRepository;
    private readonly IUsersRepository _usersRepository;

    public PostsController(ILogger<PostsController> logger, IPostsApplication postsApplication, ICountersRepository countersRepository, IUsersRepository usersRepository)
    {
        _logger = logger;
        _postsApplication = postsApplication;
        _countersRepository = countersRepository;
        _usersRepository = usersRepository;
    }


    [HttpGet("GetPostsHomePage")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] PaginationFilter filter, bool filteredByFollowing)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, 10);
        try{
            return Ok(_postsApplication.GetPostsHomePage(validFilter, filteredByFollowing));
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
            return Ok(_postsApplication.GetPostsUserPage(validFilter, username));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }

    [HttpPost("CreatePostAsync")]
    public async Task<IActionResult> CreatePostAsync(CreatePostDto value)
    {
        try{
            return Ok(_postsApplication.CreatePostAsync(value));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }
}
