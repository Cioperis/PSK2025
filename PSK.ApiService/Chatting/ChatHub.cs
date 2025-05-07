using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace PSK.ApiService.Chatting;

public sealed class ChatHub : Hub<IChatHubClient>, IChatHubServer
{
    private static readonly ConcurrentDictionary<string, byte> activeConnections = new();
    private static readonly ConcurrentQueue<string> helperQueue = new();
    private static readonly ConcurrentQueue<string> patientQueue = new();
    private static readonly ConcurrentDictionary<string, List<string>> chatGroups = new();
    private static readonly SemaphoreSlim _matchingLock = new(1, 1);

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (!httpContext!.Request.Query.TryGetValue("userType", out var userType))
        {
            await ImmediateDisconnect("Missing userType parameter");
            return;
        }

        if (!Enum.TryParse<UserType>(userType, out var type))
        {
            await ImmediateDisconnect($"Invalid userType. Valid values: {string.Join(",", Enum.GetNames<UserType>())}");
            return;
        }

        await base.OnConnectedAsync();

        if (type == UserType.Helper)
            helperQueue.Enqueue(Context.ConnectionId);
        else
            patientQueue.Enqueue(Context.ConnectionId);

        activeConnections.TryAdd(Context.ConnectionId, 0);
        await TryMatchPairs();
    }

    private async Task ImmediateDisconnect(string errorMessage)
    {
        await Clients.Caller.ReceiveSystemMessage(errorMessage, DateTime.Now);

        Context.Abort();
    }

    private async Task TryMatchPairs()
    {
        await _matchingLock.WaitAsync();

        try
        {
            while (helperQueue.TryPeek(out var helper) &&
                   patientQueue.TryPeek(out var patient))
            {
                if (!IsConnectionActive(helper)) { helperQueue.TryDequeue(out _); continue; }
                if (!IsConnectionActive(patient)) { patientQueue.TryDequeue(out _); continue; }

                var chatId = Guid.NewGuid().ToString();
                chatGroups[chatId] = new List<string> { helper, patient };

                await Task.WhenAll(
                    Groups.AddToGroupAsync(helper, chatId),
                    Groups.AddToGroupAsync(patient, chatId)
                );

                await Task.WhenAll(
                    Clients.Client(helper).ReceiveChatId(chatId),
                    Clients.Client(patient).ReceiveChatId(chatId)
                );

                await Task.WhenAll(
                    Clients.Client(helper).ReceiveSystemMessage("Connected to patient", DateTime.Now),
                    Clients.Client(patient).ReceiveSystemMessage("Connected to helper", DateTime.Now)
                );

                helperQueue.TryDequeue(out _);
                patientQueue.TryDequeue(out _);
            }
        }
        finally
        {
            _matchingLock.Release();
        }
    }

    private bool IsConnectionActive(string connectionId)
    {
        Console.WriteLine("check connection: " + activeConnections.ContainsKey(connectionId));
        return activeConnections.ContainsKey(connectionId);
    }

    public async Task SendMessage(string chatId, string msg)
    {
        if (!chatGroups.ContainsKey(chatId))
        {
            await Clients.Caller.ReceiveSystemMessage("The provided chat room does not exist", DateTime.Now);
        }
        else if (!chatGroups[chatId].Contains(Context.ConnectionId))
        {
            await Clients.Caller.ReceiveSystemMessage("You do not belong in this chat room", DateTime.Now);
        }
        else
        {
            await Clients.OthersInGroup(chatId).ReceiveMessage(Context.ConnectionId, msg, DateTime.Now);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var chat = chatGroups.FirstOrDefault(kvp => kvp.Value.Contains(Context.ConnectionId));
            if (chat.Key != null && chatGroups.TryRemove(chat.Key, out var users))
            {
                var otherUser = users.FirstOrDefault(u => u != Context.ConnectionId);
                if (otherUser != null)
                {
                    await Task.WhenAll(
                            Clients.Client(otherUser).ReceiveSystemMessage("The other user has left the chat room", DateTime.Now),
                            Clients.Client(otherUser).ForceDisconnect()
                        );
                }
            }
        }
        finally
        {
            activeConnections.TryRemove(Context.ConnectionId, out var _);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
