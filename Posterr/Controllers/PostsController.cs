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

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostAsync(long id)
    {
        try{
            var post = await _postsRepository.GetPostAsync(id);
            
            if (post is null){
                return NotFound();
            }
            else
                return Ok(post);

        }
        catch (Exception ex){
            return BadRequest(ex.Message);
        }

    }

    [HttpGet()]
    [Route("HomePage")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] PaginationFilter filter, bool filteredByFollowing)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, 10);
        try{
            var posts = (await _postsRepository.GetPostsAsync(validFilter, filteredByFollowing)).Select(post => post.PostAsDto());
            if (posts.Count() == 0){
                return Ok(new PagedResponse<List<PostDto>>(posts.ToList(), validFilter.PageNumber, validFilter.PageSize,"No more posts were found, follow more users for more posts", false));
            }
            else
                return Ok(new PagedResponse<List<PostDto>>(posts.ToList(), validFilter.PageNumber, validFilter.PageSize));
        }
        catch(Exception ex){
            return BadRequest(new PagedResponse<string>("", validFilter.PageNumber, validFilter.PageSize, ex.Message, false));
        }
    }

    [HttpGet()]
    [Route("UserPage")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] PaginationFilter filter, string username)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, 5);
        try{
            var posts = (await _postsRepository.GetPostsUserPageAsync(username, validFilter)).Select(post => post.PostAsDto());
            if (posts.Count() == 0){
                return Ok(new PagedResponse<List<PostDto>>(posts.ToList(), validFilter.PageNumber, validFilter.PageSize,"No more posts were found for the current user.", false));
            }
            else
                return Ok(new PagedResponse<List<PostDto>>(posts.ToList(), validFilter.PageNumber, validFilter.PageSize));
        }
        catch(Exception ex){
            return BadRequest(new Response<string>("", ex.Message, false));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePostAsync(CreatePostDto value)
    {
        try{
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
                await _usersRepository.UpdateUserPostCounter(post.PostedByUsername);
                _countersRepository.UpdateCounterAsync("Posts", post.Id);

                return Ok(new Response<Post>(post, "Post created succesfully", true));
            }
            else
                return Ok(new Response<string>("", "Daily post limit achieved", false));
        }
        catch(Exception ex){
            return BadRequest(new Response<CreatePostDto>(value, ex.Message, false));
        }
    }
}
