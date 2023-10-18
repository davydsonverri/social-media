using Post.Query.Domain.Entities;
using System;

namespace Post.Query.Domain.Repositories
{
    public interface IPostRepository
    {
        Task CreateAsync(PostEntity post);
        Task UpdateAsync(PostEntity post);
        Task DeleteAsync(Did postId);
        Task<PostEntity?> GetByIdAsync(Did postId);
        Task<List<PostEntity>> ListAllAsync();
        Task<List<PostEntity>> ListByAuthor(string author);        
    }
}
