namespace PSK.ApiService.Chatting;

public interface IChatHubServer
{
    Task JoinChat(string chatId);

    Task SendMessage(string chatId, string message);

    Task LeaveChat();
}
