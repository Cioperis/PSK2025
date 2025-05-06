using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Services.Interfaces;
using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Services
{
    public class AutoMessageService : IAutoMessageService
    {
        private readonly IAutoMessageRepository _repository;

        public AutoMessageService(IAutoMessageRepository repository)
        {
            _repository = repository;
        }

        public async Task<AutoMessage?> GetRandomMessageAsync()
        {
            return await _repository.GetRandomActiveMessageAsync();
        }

        public async Task<IEnumerable<AutoMessage>> SearchMessagesAsync(string keyword)
        {
            return await _repository.SearchByKeywordAsync(keyword);
        }
    }
}
