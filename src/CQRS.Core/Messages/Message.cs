using Swashbuckle.AspNetCore.Annotations;

namespace CQRS.Core.Messages
{
    public record class Message
    {
        [SwaggerSchema(ReadOnly = true)]
        public Did Id { get; set; }
    }
}
