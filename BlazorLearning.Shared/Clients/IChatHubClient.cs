namespace BlazorLearning.Shared.Clients
{
    public interface IChatHubClient
    {
        Task ReceiveMessage(string user, string message);
        Task SendMessage(string user, string message);
    }
}
