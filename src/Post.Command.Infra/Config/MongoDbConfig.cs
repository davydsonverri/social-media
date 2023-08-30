using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Command.Infra.Config
{
    public class MongoDbConfig
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }
        public required string Collection { get; set; }
    }
}
