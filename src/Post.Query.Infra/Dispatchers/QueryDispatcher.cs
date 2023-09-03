using CQRS.Core.Infra;
using CQRS.Core.Queries;
using Post.Query.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Query.Infra.Dispatchers
{
    public class QueryDispatcher : IQueryDispatcher<PostEntity>
    {
        private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();

        public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
        {
            if (_handlers.ContainsKey(typeof(TQuery)))
            {
                throw new InvalidOperationException($"Handler {typeof(TQuery)} is already registered");
            }
            _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
        }

        public async Task<List<PostEntity>> SendAsync(BaseQuery query)
        {
            if(_handlers.TryGetValue(query.GetType(), out Func<BaseQuery, Task<List<PostEntity>>>? handlers))
            {
                return await handlers(query);
            } else
            {
                throw new ArgumentNullException(nameof(handlers), "Query handler not registered");
            }            
        }
    }
}
