using BlazorLearning.Shared.Clients;
using Microsoft.AspNetCore.SignalR;

namespace BlazorLearning.WebApi.Hubs
{
    public class ChatHub : Hub<IChatHubClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
    }
}
