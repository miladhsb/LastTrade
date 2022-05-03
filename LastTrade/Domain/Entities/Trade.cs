namespace LastTrade.Domain.Entities
{
    public class Trade:BaseEntity
    {
        public int InstrumentId { get; set; }
        public DateTime DateTimeEn { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }

        public Instrument Instrument { get; set; }
    }
}
