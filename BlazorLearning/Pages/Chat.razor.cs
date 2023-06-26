using BlazorLearning.Shared.Clients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorLearning.Pages
{
    public partial class Chat : IChatHubClient, IAsyncDisposable
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        private List<string> Messages { get; } = new List<string>();
        private HubConnection HubConnection { get; set; }
        private string UserInput { get; set; }
        private string MessageInput { get; set; }

        protected override async Task OnInitializedAsync()
        {
            string apiUrl = Configuration.GetValue<string>("ApiUrl");

            HubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{apiUrl}/hub/chat"))
                .Build();

            HubConnection.On<string, string>(nameof(ReceiveMessage), ReceiveMessage);

            await HubConnection.StartAsync();
        }

        public async Task OnSend()
        {
            await SendMessage(UserInput, MessageInput);
        }

        public async Task SendMessage(string userInput, string messageInput)
        {
            if (HubConnection is not null)
            {
                await HubConnection.SendAsync(nameof(SendMessage), userInput, messageInput);
            }
        }

        public bool IsConnected =>
            HubConnection?.State == HubConnectionState.Connected;

        public Task ReceiveMessage(string user, string message)
        {
            var encodedMsg = $"{user}: {message}";
            Messages.Add(encodedMsg);

            StateHasChanged();

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }
    }
}
