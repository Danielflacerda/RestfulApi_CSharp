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

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostAsync(Guid id)
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
    public async Task<ActionResult<List<PostDto>>> GetPostsAsync(bool filtered)
    {
        try{
            var posts = (await _postsRepository.GetPostsAsync()).Select(post => post.PostAsDto());
            if (posts.Count() == 0){
                return NotFound();
            }
            else
                return Ok(posts);
        }
        catch(Exception ex){
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task CreatePostAsync(CreatePostDto value)
    {
        Post post = new(){
                Id = Guid.NewGuid(),
                Content = value.Content,
                RepostedPostId = value.RepostedPostId,
                PostedByUserId = value.PostedByUserId,
                CreatedOn = DateTime.Now
        };

        await _postsRepository.CreatePostAsync(post);

        CreatedAtAction(nameof(GetPostAsync) , new { id = post.Id }, post.PostAsDto());
    }
}
