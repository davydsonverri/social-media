using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Comment")]
    public class CommentEntity
    {
        [Key]
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string CommentDate { get; set; }
        public required string Comment { get; set; }
        public required bool Edited { get; set; }
        public required Guid PostId { get; set; }

        [JsonIgnore]
        public virtual required PostEntity Post { get; set; }

    }
}