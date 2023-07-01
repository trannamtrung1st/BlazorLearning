using BlazorLearning.WebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddServerComponents();
    //.AddWebAssemblyComponents();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(provider =>
{
    IHttpContextAccessor httpContext = provider.GetRequiredService<IHttpContextAccessor>();
    var request = httpContext.HttpContext?.Request;
    var http = new HttpClient();
    http.BaseAddress = new Uri($"{request.Scheme}://{request.Host}");
    return http;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapGet("/api/weathers", async () =>
{
    // Simulate retrieving the data asynchronously.
    await Task.Delay(250);

    var startDate = DateOnly.FromDateTime(DateTime.Now);
    var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = startDate.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    }).ToArray();

    return forecasts;
});

app.MapRazorComponents<App>();

app.Run();

partial class Program
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
}