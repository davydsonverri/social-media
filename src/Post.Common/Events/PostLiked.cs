using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record PostCreated : BaseEvent
    {
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }

        public PostCreated() : base()
        {
            
        }
    }
}
