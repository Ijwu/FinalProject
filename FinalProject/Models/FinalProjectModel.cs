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
        public ApplicationUser User { get; set; }
    }

    public class BudgetBox
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double StartingAmount { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
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

        public virtual BudgetBox BudgetBox { get; set; }
    }
}