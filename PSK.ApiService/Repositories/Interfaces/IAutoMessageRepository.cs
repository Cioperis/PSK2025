using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces
{
    public interface IAutoMessageRepository : IBaseRepository<PositiveMessage>
    {
        Task<PositiveMessage?> GetRandomActiveMessageAsync();
        Task<IEnumerable<PositiveMessage>> SearchByKeywordAsync(string keyword);
        Task<int> CountActiveAsync();
        Task<int> CountInactiveAsync();
        Task<IEnumerable<PositiveMessage>> GetRecentAsync(int count = 10);
    }
}
