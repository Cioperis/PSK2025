using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services.Interfaces
{
    public interface IAutoMessageService
    {
        Task<bool> EnqueueRandomPositiveMessageAsync(string userEmail);
    }
}
