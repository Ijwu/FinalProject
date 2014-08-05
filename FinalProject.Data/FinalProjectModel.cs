using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data;

namespace FinalProject.Data
{
    public class FinalProjectModel : DbContext
    {
        // Your context has been configured to use a 'FinalProjectModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'FinalProject.Data.FinalProjectModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'FinalProjectModel' 
        // connection string in the application configuration file.
        public FinalProjectModel()
            : base("name=FinalProjectModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<BudgetBox> BudgetBoxs { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(4), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }
        public string ProfilePhotoPath { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User User { get; set; }
    }

    public class BudgetBox
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int StartingAmount { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } 
    }

    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int BudgetBoxId { get; set; }

        [Required]
        public int UnitPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        public virtual BudgetBox BudgetBox { get; set; }
    }
}