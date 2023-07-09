using BlazorLearning.Shared.Clients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorLearning.WebClient.Pages
{
    public partial class Chat : IChatHubClient, IAsyncDisposable
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        private HubConnection hubConnection;
        private List<string> messages = new List<string>();
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            string apiUrl = Configuration.GetValue<string>("ApiUrl");

            hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{apiUrl}/hub/chat"))
                .Build();

            hubConnection.On<string, string>(nameof(ReceiveMessage), ReceiveMessage);

            await hubConnection.StartAsync();
        }

        public Task ReceiveMessage(string user, string message)
        {
            var encodedMsg = $"{user}: {message}";
            messages.Add(encodedMsg);

            StateHasChanged();

            return Task.CompletedTask;
        }

        public async Task OnSend()
        {
            await SendMessage(userInput, messageInput);
        }

        public async Task SendMessage(string userInput, string messageInput)
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync(nameof(SendMessage), userInput, messageInput);
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
