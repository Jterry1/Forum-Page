using System;
using System.ComponentModel.DataAnnotations;

namespace WallProj.Models
{
    public class Comment
    {
        [Key]
        public int CommentId {get;set;}
        [Required]
        [MinLength(25)]
        public string Content {get;set;}

        public int UserId {get;set;}
        public int MessageId {get;set;}
        public User NavUser {get;set;}
        public Message NavMessage {get;set;}
        // public DateTime CreatedAt { get; set; } = DateTime.Now;
        // public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}