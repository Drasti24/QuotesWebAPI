//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    //represent a Tag entity, used to categorize or label Quotes
    public class Tag
    {
        //primary key for Tag
        [Key]
        public int Id { get; set; }

        //name of tag
        [Required]
        public string Name { get; set; }

        //navigation property for many-to-many relationship with Quotes.
        public List<TagAssignment> TagAssignments { get; set; } = new List<TagAssignment>();
    }
}
