using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data;

namespace FinalProject.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }

    public class BudgetBox
    {
        [Key]
        public int Id { get; set; }
        public string PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double StartingAmount { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }

    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int BudgetBoxId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual BudgetBox BudgetBox { get; set; }
    }
    public class PostComment
    {
        [Key]
        public int Id {get; set;}
        [Required]
        public String PostId {get; set;}
        [Required]
        public string UserId {get; set;}
        [Required]
        public string Text {get; set;}

        public virtual Post Post {get; set;}
    }
    public class ProfileComment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String ProfileId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Text { get; set; }

        [ForeignKey("ProfileId")]
        public virtual ApplicationUser Profile { get; set; }
    }
    public class Rating
    {
        [Key]
        public int Id {get; set;}
        [Required]
        public int Stars {get; set;}
        [Required]
        public int PostId {get; set;}
        [Required]
        public string UserId {get; set;}

    }
}