namespace LastTrade.Domain.Entities
{
    public class Instrument:BaseEntity
    {
        public string Shortname { get; set; }
        public  ICollection<Trade> Trades { get; set; } 
    }
}
