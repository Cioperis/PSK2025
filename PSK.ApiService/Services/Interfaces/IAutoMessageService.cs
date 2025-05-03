using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services.Interfaces
{
    public interface IAutoMessageService
    {
        Task<AutoMessage?> GetRandomMessageAsync();
        Task<IEnumerable<AutoMessage>> SearchMessagesAsync(string keyword);
    }
}
