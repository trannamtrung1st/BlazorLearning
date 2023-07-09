namespace BlazorLearning.WebClient.Services
{
    public class SampleScopedService : IDisposable
    {
        private readonly ILogger<SampleScopedService> _logger;

        public SampleScopedService(ILogger<SampleScopedService> logger)
        {
            _logger = logger;
        }

        public DateTime CreatedTime { get; } = DateTime.Now;

        public void Dispose()
        {
            _logger.LogInformation($"Disposing scoped service created at {CreatedTime}");
        }
    }
}
