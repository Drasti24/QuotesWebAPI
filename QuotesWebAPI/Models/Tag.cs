using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<TagAssignment> TagAssignments { get; set; } = new List<TagAssignment>();
    }
}
