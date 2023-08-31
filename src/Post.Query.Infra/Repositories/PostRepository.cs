using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infra.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infra.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(PostEntity post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();      
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();            
        }

        public async Task DeleteAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);

            if (post == null) return;

            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<PostEntity?> GetByIdAsync(Guid postId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts
                            .Include(p => p.Comments)
                            .FirstOrDefaultAsync(x => x.Id == postId);            
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListByAuthor(string author)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Posts
                .AsNoTracking()
                .Include(p => p.Comments)
                .AsNoTracking()
                .Where(x => x.Author == author)
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Posts.Update(post);
            _ = await context.SaveChangesAsync();
        }
    }
}
