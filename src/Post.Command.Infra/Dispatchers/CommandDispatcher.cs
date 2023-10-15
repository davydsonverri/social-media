using CQRS.Core.Commands;
using CQRS.Core.Infra;

namespace Post.Command.Infra.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers;
        public CommandDispatcher()
        {
            _handlers = new Dictionary<Type, Func<BaseCommand, Task>>();
        }

        public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
        {
            if (_handlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"Handler {typeof(T)} is already registered.");
            }

            _handlers.Add(typeof(T), x => handler((T)x));
        }

        public async Task SendAsync(BaseCommand command)
        {
            if (_handlers.TryGetValue(command.GetType(), out Func<BaseCommand, Task>? handler))
            {
                await handler(command);
            } else
            {
                throw new ArgumentNullException(command.GetType().ToString(), "Command handler not registered");
            }
        }
    }
}
