using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WallProj.Models {
    public class User {
        [Key]
        public int UserId { get; set; }
        [Required (ErrorMessage = "First Name Must be Entered")]
        [Display (Name = "First Name : ")]
        [MinLength (3)]
        public string FirstName { get; set; }

        [Required (ErrorMessage = "Last Name Must be Entered")]
        [Display (Name = "Last Name : ")]
        [MinLength (3)]
        public string LastName { get; set; }

        [Required]
        [DataType (DataType.Date)]
        [Display (Name = "Enter Youtr Date of Birth")]
        public DateTime DOB { get; set; }

        [Required (ErrorMessage = "Email Must be Entered")]
        [Display (Name = "Email  : ")]
        [DataType (DataType.EmailAddress)]
        public string Email { get; set; }

        [Required (ErrorMessage = "Password Must be Entered")]
        [Display (Name = "Password  : ")]
        [DataType (DataType.Password)]
        public string Password { get; set; }

        [Display (Name = "Confirm Password  : ")]
        [DataType (DataType.Password)]
        [Compare ("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Message> CreatedMessages {get;set;}
        public List<Comment> CreatedComments {get;set;}

    }
}