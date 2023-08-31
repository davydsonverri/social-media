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
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Add(comment);

            _ = await context.SaveChangesAsync();
        }

        public async Task Delete(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            var comment = await GetByIdAsync(commentId);

            if (comment == null) return;

            context.Comments.Remove(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task<CommentEntity?> GetByIdAsync(Guid commentId)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            return await context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using DatabaseContext context = _contextFactory.CreateDbContext();
            context.Comments.Update(comment);

            _ = await context.SaveChangesAsync();
        }
    }
}
