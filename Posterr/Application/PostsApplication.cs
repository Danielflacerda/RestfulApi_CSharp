using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Posterr.Dtos;
using Posterr.Entities;
using Posterr.Filter;
using Posterr.Repositories;

namespace Posterr.Application;

public class PostsApplication : IPostsApplication
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
    private readonly IPostsRepository _postsRepository;
    private readonly ICountersRepository _countersRepository;
    private readonly IUsersRepository _usersRepository;

    public PostsApplication(IPostsRepository postsRepository, ICountersRepository countersRepository, IUsersRepository usersRepository)
    {
        _postsRepository = postsRepository;
        _countersRepository = countersRepository;
        _usersRepository = usersRepository;
    }


    public async Task<PagedResponse<List<PostDto>>> GetPostsHomePage(PaginationFilter filter, bool filteredByFollowing)
    {
        var posts = (await _postsRepository.GetPostsAsync(filter, sessionUser.Username, filteredByFollowing)).Select(post => post.PostAsDto());
        if (posts.Count() == 0){
            return new PagedResponse<List<PostDto>>(null, filter.PageNumber, filter.PageSize,"No posts were found, follow more users for more posts", false);
        }
        else
            return new PagedResponse<List<PostDto>>(posts.ToList(), filter.PageNumber, filter.PageSize);
    }

    public async Task<PagedResponse<List<PostDto>>> GetPostsUserPage(PaginationFilter filter, string username)
    {
        var posts = (await _postsRepository.GetPostsUserPageAsync(username, filter)).Select(post => post.PostAsDto());
        if (posts.Count() == 0){
            return new PagedResponse<List<PostDto>>(null, filter.PageNumber, filter.PageSize,"No posts were found for the current user.", false);
        }
        else
            return new PagedResponse<List<PostDto>>(posts.ToList(), filter.PageNumber, filter.PageSize);
    }
    public async Task<Response<Post>> CreatePostAsync(CreatePostDto value)
    {
        bool structurePost = true;
        string message = string.Empty;

        if(string.IsNullOrEmpty(value.Content)){ // Check the repostedpostid when there is no content because, there cannot be a post without a content and a repostedpostid at the same time
            if(value.RepostedPostId == 0 || value.RepostedPostId == null){
                structurePost = false;
                message = "Create post request do not accept a post without a content and without a refer to other post at the same time.";
            }
        }

            
        if(structurePost) {   
            if(value.Content.Length <= 777){ // Check if contents length is lower than or equal to 777
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

                        return new Response<Post>(post, "Post created succesfully", true);
                    }
                    else
                        return new Response<Post>(null, "Daily post limit achieved", false);
                }
                else{
                        return new Response<Post>(null, "User informed as post writer does not exist!", false);
                }
            }
            else
            {
                return new Response<Post>(null, "Content should not have more than 777 Characters!", false);
            }
        }
        else{
                return new Response<Post>(null, message, false);
        }
    }
    
    public async Task<PagedResponse<List<PostDto>>> SearchAsync(PaginationFilter filter, string searchContent)
    {
        var posts = (await _postsRepository.SearchAsync(searchContent, filter)).Select(post => post.PostAsDto());
        if (posts.Count() == 0){
            return new PagedResponse<List<PostDto>>(null, filter.PageNumber, filter.PageSize,"No posts were found for the searched content.", false);
        }
        else
            return new PagedResponse<List<PostDto>>(posts.ToList(), filter.PageNumber, filter.PageSize);
    }
}
