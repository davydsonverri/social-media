using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record PostCreated : BaseEvent
    {
        public required string Author { get; set; }
        public required string Message { get; set; }
        public required DateTime PostDate { get; set; }

        public PostCreated() : base()
        {
            
        }
    }
}
