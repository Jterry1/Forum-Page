using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace WallProj.Models
{
    public class Message
    {
        [Key]
        public int MessageId {get;set;}
        [Required]
        [MinLength(25)]
        public string MessagePost {get;set;}
        
        public int UserId {get;set;}
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public User MessageCreator {get;set;}

        public List<Comment> Comments {get;set;}
    }
}