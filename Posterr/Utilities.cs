using Posterr.Dtos;
using Posterr.Entities;

namespace Posterr;
public static class Utilities{

    public static PostDto PostAsDto(this Post post){
        return new PostDto{
                Id = post.Id,
                Content = post.Content,
                RepostedPostId = post.RepostedPostId,
                PostedByUserId = post.PostedByUserId,
                CreatedOn = post.CreatedOn
        };
    }
}