using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infra.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
            _configureDbContext(optionsBuilder);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
