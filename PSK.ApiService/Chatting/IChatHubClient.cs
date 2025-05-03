namespace PSK.ApiService.Chatting;

public interface IChatHubClient
{
    Task ReceiveMessage(string senderId, string message, DateTime sentTime);

    Task ReceiveSystemMessage(string message, DateTime sentTime);
}
