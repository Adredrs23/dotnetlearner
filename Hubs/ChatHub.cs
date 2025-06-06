namespace dotnetlearner.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name;
        Console.WriteLine($"User connected: {userName}");
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(string receiverId, string message)
    {
        var senderId = Context.User?.FindFirst("sub")?.Value;
        var senderName = Context.User?.Identity?.Name;

        // Save message to DB here (you'll need to inject a scoped service via Hub context)
        // Then forward message to receiver if online
        await Clients.User(receiverId).SendAsync("ReceiveMessage", new
        {
            senderId,
            senderName,
            message,
            timestamp = DateTime.UtcNow
        });
    }
}