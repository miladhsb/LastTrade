namespace LastTrade.Application.DTOs
{
    public class LastTradsDTOs
    {
        public int id { get; set; }
        public int InstrumentId { get; set; }
        public string Shortname { get; set; }
        public DateTime DateTimeEn { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
