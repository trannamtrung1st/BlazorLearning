namespace BlazorLearning.WebClient.Services
{
    public interface INotiferService
    {
        event Func<Task> NotifyFunc;
        Task Start();
    }

    public class NotiferService : INotiferService, IDisposable
    {
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        public event Func<Task> NotifyFunc;
        private PeriodicTimer timer;

        public async Task Start()
        {
            try
            {
                await _semaphore.WaitAsync();

                if (timer is null)
                {
                    timer = new(TimeSpan.FromSeconds(5));

                    using (timer)
                    {
                        while (await timer.WaitForNextTickAsync())
                        {
                            if (NotifyFunc != null) await NotifyFunc();
                        }
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
