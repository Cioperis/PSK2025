namespace PSK.ApiService.Repositories.Interfaces
{
    public interface IUserMessageRepository
    {
        Task<UserMessage> AddAsync(UserMessage entity);
        Task<UserMessage?> GetWithUserAsync(Guid id);
        Task UpdateAsync(UserMessage entity);
    }
}
