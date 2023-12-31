﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Post.Query.Domain.Entities
{
    [Table("Post")]
    public class PostEntity
    {
        [Key]
        public required Did Id { get; set; }
        public required string Author { get; set; }
        public required DateTime PostDate { get; set; }
        public required string Message { get; set; }
        public int Likes { get; set; }
        public virtual ICollection<CommentEntity>? Comments { get; set; }
    }
}
