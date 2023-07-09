using Blazored.LocalStorage;
using BlazorLearning.WebClient;
using BlazorLearning.WebClient.Library;
using BlazorLearning.WebClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ExampleJsInterop>()
    .AddScoped<SampleScopedService>();

builder.Services.AddBlazoredLocalStorage();

//builder.Services.AddOidcAuthentication(options =>
//    builder.Configuration.Bind("Local", options.ProviderOptions)); // [TODO]

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Configuration.AddInMemoryCollection(); // [NOTE] can use 3rd-party then set as in-memory collection

var apiUrl = builder.Configuration["ApiUrl"];

Console.WriteLine($"ApiUrl: {apiUrl}");

Console.WriteLine($"Base address: {builder.HostEnvironment.BaseAddress}, environment: {builder.HostEnvironment.Environment}");

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    await InitializeConfiguration(scope.ServiceProvider);
}

await app.RunAsync();

static async Task InitializeConfiguration(IServiceProvider provider)
{
    var localStorage = provider.GetRequiredService<ILocalStorageService>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var keys = await localStorage.KeysAsync();
    var prefix = "appsettings:";
    var configKeys = keys.Where(k => k.StartsWith(prefix));

    foreach (var key in configKeys)
    {
        var configValue = await localStorage.GetItemAsStringAsync(key);
        configuration[key.Substring(prefix.Length)] = configValue;
    }
}