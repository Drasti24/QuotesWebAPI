using Azure;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Models;

namespace QuotesWebAPI.Data
{
    public class QuotesDbContext : DbContext
    {
        public QuotesDbContext(DbContextOptions<QuotesDbContext> options) : base(options) { }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagAssignment> TagAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TagAssignment>()
                .HasKey(t => new { t.QuoteId, t.TagId });

            modelBuilder.Entity<TagAssignment>()
                .HasOne(q => q.Quote)
                .WithMany(q => q.TagAssignments)
                .HasForeignKey(q => q.QuoteId);

            modelBuilder.Entity<TagAssignment>()
                .HasOne(t => t.Tag)
                .WithMany(t => t.TagAssignments)
                .HasForeignKey(t => t.TagId);
        }
    }
}
