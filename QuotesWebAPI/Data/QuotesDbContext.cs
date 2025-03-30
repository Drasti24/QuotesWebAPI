//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

using Azure;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Models;

namespace QuotesWebAPI.Data
{
    //this class represents the database context for Entity Framework Core.
    //if defines which models are used and how relationships between them are configured.
    public class QuotesDbContext : DbContext
    {
        //constructor that accepts DbContext options and passes them to the base class.
        public QuotesDbContext(DbContextOptions<QuotesDbContext> options) : base(options) { }

        public DbSet<Quote> Quotes { get; set; }    //BdSet for Quotes Table
        public DbSet<Tag> Tags { get; set; }     //DbSet for Tags table
        public DbSet<TagAssignment> TagAssignments { get; set; }      //DbSet for TagAssignments linking table (many-to-many relationships)

        //this method is called by EF Core to configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configure the composite primary key for the TagAssignment linking table
            modelBuilder.Entity<TagAssignment>()
                .HasKey(t => new { t.QuoteId, t.TagId });

            //define relationship: one Quote can have many TagAssignments
            modelBuilder.Entity<TagAssignment>()
                .HasOne(q => q.Quote)
                .WithMany(q => q.TagAssignments)
                .HasForeignKey(q => q.QuoteId);

            //define relationship: one Tag can have many TagAssignments
            modelBuilder.Entity<TagAssignment>()
                .HasOne(t => t.Tag)
                .WithMany(t => t.TagAssignments)
                .HasForeignKey(t => t.TagId);
        }
    }
}
