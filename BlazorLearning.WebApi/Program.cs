using BlazorLearning.WebApi.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddCors();

builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseResponseCompression();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.MapHub<ChatHub>("/hub/chat");

app.Run();
