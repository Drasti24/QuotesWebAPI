//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    //represent the quote entity in the system
    public class Quote
    {
        //primary key for quote
        [Key]
        public int Id { get; set; }

        //required content of quote
        [Required]
        [MaxLength(1000)]  // SQLite does not support nvarchar(max), so we set a max length
        public string Text { get; set; }

        [MaxLength(255)]  // Set max length for Author
        public string? Author { get; set; }

        //no of likes the quote has received
        public int Likes { get; set; } = 0;

        //navigation property to represent many-to-many relationships with Tags
        public List<TagAssignment> TagAssignments { get; set; } = new List<TagAssignment>();
    }
}
