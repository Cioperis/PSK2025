using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace PSK.ApiService.Chatting;

public sealed class ChatHub : Hub<IChatHubClient>, IChatHubServer
{
    private static readonly ConcurrentDictionary<string, List<string>> _chatGroups = new();

    public async Task JoinChat(string chatId)
    {

        if (_chatGroups.Where(kvp => kvp.Value.Contains(Context.ConnectionId)).Any() || (_chatGroups.ContainsKey(chatId) && _chatGroups[chatId].Count >= 2)){
            return;
        }

        if (!_chatGroups.TryGetValue(chatId, out _))
        {
            var groupUsers = new List<string> {Context.ConnectionId};
            _chatGroups[chatId] = groupUsers;
        }
        else
        {
            _chatGroups[chatId].Add(Context.ConnectionId);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        await Clients.Caller.ReceiveSystemMessage($"You joined chat {chatId}", DateTime.UtcNow);

        if (_chatGroups[chatId].Count > 1)
        {
            await Clients.OthersInGroup(chatId).ReceiveSystemMessage($"Another user joined chat {chatId}", DateTime.UtcNow);
        }
    }

    public async Task SendMessage(string chatId, string msg)
    {
        if (_chatGroups.ContainsKey(chatId) && _chatGroups[chatId].Contains(Context.ConnectionId))
        {
            await Clients.OthersInGroup(chatId).ReceiveMessage(Context.ConnectionId, msg, DateTime.UtcNow);
        }
    }

    public async Task LeaveChat()
    {
        var chat = _chatGroups.FirstOrDefault(kvp => kvp.Value.Contains(Context.ConnectionId));
        if (chat.Key == null || !_chatGroups.TryGetValue(chat.Key, out var users)) return;
        _chatGroups.TryRemove(chat.Key, out _);
        await Groups.RemoveFromGroupAsync(chat.Key, Context.ConnectionId);

        var otherUser = users.FirstOrDefault(u => u != Context.ConnectionId);
        if (otherUser != null) 
        {
            await Clients.Client(otherUser).ReceiveSystemMessage("Bro left", DateTime.UtcNow);
            await Groups.RemoveFromGroupAsync(chat.Key, otherUser);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            if (exception != null)
            {
                // Log error
            }
            await LeaveChat();
        }
        finally
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
