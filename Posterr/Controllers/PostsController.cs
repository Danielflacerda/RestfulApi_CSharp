using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Repositories;

namespace Posterr.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    //This can become a session variable in future for better implementation.
    public int _lastPostsAmountRecovered; 

    private readonly ILogger<PostsController> _logger;
    private readonly IPostsRepository _postsRepository;

    public PostsController(ILogger<PostsController> logger, IPostsRepository postsRepository)
    {
        _logger = logger;
        _postsRepository = postsRepository;
    }

    [HttpGet("{filtered}")]
    public async Task<ActionResult<List<PostDto>>> GetPost(bool filtered)
    {
        try{
            var posts = _postsRepository.GetPosts().Select(post => post.PostAsDto());
            if (posts.Count() == 0){
                return NotFound();
            }
            else
                return Ok(posts);
        }
        catch(Exception ex){
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task PostAsync(CreatePostDto value)
    {
        Post post = new(){
                PostId = _postsRepository.GetMaxId() + 1,
                Content = value.Content,
                RepostedPostId = value.RepostedPostId,
                PostedByUserId = value.PostedByUserId,
                CreatedOn = DateTime.Now
        };

        _postsRepository.CreatePost(post);

        CreatedAtAction(nameof(GetPost) , new { id = post.PostId }, post.PostAsDto());
    }
}
