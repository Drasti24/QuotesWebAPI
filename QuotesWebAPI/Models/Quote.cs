using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]  // SQLite does not support nvarchar(max), so we set a max length
        public string Text { get; set; }

        [MaxLength(255)]  // Set max length for Author
        public string? Author { get; set; }

        public int Likes { get; set; } = 0;

        public List<TagAssignment> TagAssignments { get; set; } = new List<TagAssignment>();
    }
}
