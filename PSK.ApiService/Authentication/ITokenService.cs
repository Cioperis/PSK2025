using PSK.ServiceDefaults.Models;

namespace PSK.ApiService.Authentication
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}