//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

namespace QuotesWebAPI.Models
{
    //association class for many-to-many relationship between Quotes and Tags
    public class TagAssignment
    {
        //foreign key referencing the Quote
        public int QuoteId { get; set; }
        
        //navigation property for associated Quote
        public Quote Quote { get; set; }

        //foreign key referencing Tag
        public int TagId { get; set; }

        //navigation property for associated Tag
        public Tag Tag { get; set; }
    }
}
