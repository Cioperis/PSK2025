namespace PSK.ApiService.Chatting;

public interface IChatHubServer
{
    Task SendMessage(string chatId, string message);
}
