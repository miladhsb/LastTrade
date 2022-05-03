using LastTrade.Application.DTOs;
using LastTrade.Application.RepoContract;
using System.Text;

namespace LastTrade.Application.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepo _tradeRepo;

        public TradeService(ITradeRepo tradeRepo)
        {
            this._tradeRepo = tradeRepo;
          
        }
        public  async Task<IEnumerable<LastTradsDTOs>> GetLastTradeAsync(DateTime? startDate)
        {
            _tradeRepo.CreateTable();

           
            var res = await _tradeRepo.GetLastTradeAsync(startDate);


            return res.Select(p => new LastTradsDTOs() {id=p.id, InstrumentId = p.InstrumentId, Close = p.Close, DateTimeEn = p.DateTimeEn, Low = p.Low, High = p.High, Open = p.Open, Shortname = p.Instrument.Shortname });
        }

        public async Task<bool> SaveLastTrades(IEnumerable<LastTradsDTOs> LastTradsDTOs)
        {
            string LastTradsJson = "Null Data";
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "LastTrades.txt");
            if (LastTradsDTOs.Count() > 0)
            {
                LastTradsJson= System.Text.Json.JsonSerializer.Serialize(LastTradsDTOs);
            }
           await File.WriteAllBytesAsync(FilePath, Encoding.UTF8.GetBytes(LastTradsJson));

            return true;
          
        }
    }
}
