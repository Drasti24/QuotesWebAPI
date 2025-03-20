namespace QuotesWebAPI.Models
{
    public class TagAssignment
    {
        public int QuoteId { get; set; }
        public Quote Quote { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
