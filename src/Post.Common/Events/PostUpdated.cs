using CQRS.Core.Messages;

namespace Post.Common.Events
{
    public record PostUpdated : BaseEvent
    {        
        public string Message { get; set; }        

        public PostUpdated() : base()
        {
            
        }

    }
}
