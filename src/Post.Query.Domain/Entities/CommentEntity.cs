using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Comment")]
    public class CommentEntity
    {
        [Key]
        public required Did Id { get; set; }
        public required string Username { get; set; }
        public required DateTime CommentDate { get; set; }
        public required string Comment { get; set; }
        public required bool Edited { get; set; }
        public required Did PostId { get; set; }

        [JsonIgnore]
        public virtual PostEntity? Post { get; set; }

    }
}