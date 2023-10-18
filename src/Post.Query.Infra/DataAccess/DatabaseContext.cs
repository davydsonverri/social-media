using Domain.Identity.ULID;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infra.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {            
            configurationBuilder
                .Properties<Did>()
                .HaveConversion<DidToStringConverter>()
                .HaveConversion<DidToBytesConverter>();            
        }
    }
}
