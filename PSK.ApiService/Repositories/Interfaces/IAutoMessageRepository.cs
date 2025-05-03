using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Repositories.Interfaces
{
    public interface IAutoMessageRepository : IBaseRepository<AutoMessage>
    {
        Task<AutoMessage?> GetRandomActiveMessageAsync();
        Task<IEnumerable<AutoMessage>> SearchByKeywordAsync(string keyword);
        Task<int> CountActiveAsync();
        Task<int> CountInactiveAsync();
        Task<IEnumerable<AutoMessage>> GetRecentAsync(int count = 10);
    }
}
