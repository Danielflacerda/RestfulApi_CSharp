using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IPostsRepository _postsRepository;
    private readonly ICountersRepository _countersRepository;
    private readonly IUsersRepository _usersRepository;

    public PostsController(ILogger<PostsController> logger, IPostsRepository postsRepository, ICountersRepository countersRepository, IUsersRepository usersRepository)
    {
        _logger = logger;
        _postsRepository = postsRepository;
        _countersRepository = countersRepository;
        _usersRepository = usersRepository;
    }


    [HttpGet("GetPostsHomePage")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] PaginationFilter filter, bool filteredByFollowing)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, 10);
        try{
            var posts = (await _postsRepository.GetPostsAsync(validFilter, sessionUser.Username, filteredByFollowing)).Select(post => post.PostAsDto());
            if (posts.Count() == 0){
                return Ok(new PagedResponse<string>("", validFilter.PageNumber, validFilter.PageSize,"No more posts were found, follow more users for more posts", false));
            }
            else
                return Ok(new PagedResponse<List<PostDto>>(posts.ToList(), validFilter.PageNumber, validFilter.PageSize));
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
            var posts = (await _postsRepository.GetPostsUserPageAsync(username, validFilter)).Select(post => post.PostAsDto());
            if (posts.Count() == 0){
                return Ok(new PagedResponse<string>("", validFilter.PageNumber, validFilter.PageSize,"No more posts were found for the current user.", false));
            }
            else
                return Ok(new PagedResponse<List<PostDto>>(posts.ToList(), validFilter.PageNumber, validFilter.PageSize));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }

    [HttpPost("CreatePostAsync")]
    public async Task<IActionResult> CreatePostAsync(CreatePostDto value)
    {
        try{
            if(value.Content.Length <= 777){
                var postWriter = _usersRepository.GetUserAsync(value.PostedByUsername).Result;
                if(postWriter != null){ // Check if username informed as post writer exists
                    if(_postsRepository.GetTodayUserPostsCounterAsync(value.PostedByUsername).Result < 5){
                        long counterPosts =  _countersRepository.GetCounterAsync("Posts").Result.Value;
                        Post post = new(){
                            Id = counterPosts + 1,
                            Content = value.Content,
                            RepostedPostId = value.RepostedPostId,
                            PostedByUsername = value.PostedByUsername,
                            CreatedOn = DateTime.Now
                        };

                        await _postsRepository.CreatePostAsync(post);
                        // Normally in a relational database i would use a trigger for that
                        await _usersRepository.UpdateUserPostCounter(post.PostedByUsername);
                        // Normally in a relational database i would use a trigger for that
                        _countersRepository.UpdateCounterAsync("Posts", post.Id);

                        return Ok(new Response<Post>(post, "Post created succesfully", true));
                    }
                    else
                        return Ok(new Response<string>("", "Daily post limit achieved", false));
                }
                else{
                        return Ok(new Response<string>("", "User informed as post writer does not exist!", false));
                }
            }
            else
            {
                return Ok(new Response<string>("", "Content should not have more than 777 Characters!", false));
            }
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }
}
