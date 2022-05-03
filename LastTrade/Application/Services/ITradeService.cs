using LastTrade.Application.DTOs;

namespace LastTrade.Application.Services
{
    public interface ITradeService
    {

        Task<IEnumerable<LastTradsDTOs>> GetLastTradeAsync(DateTime? startDate);

        Task<bool> SaveLastTrades(IEnumerable<LastTradsDTOs> lastTradsDTOs);
    }
}
