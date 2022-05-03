using LastTrade.Application.DTOs;
using LastTrade.Domain.Entities;

namespace LastTrade.Application.RepoContract
{
    public interface ITradeRepo
    {
        Task<IEnumerable<Trade>> GetLastTradeAsync(DateTime? startDate);
   
        void CreateTable();
    }
}
