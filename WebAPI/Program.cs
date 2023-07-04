using Library;
using Logic;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddSingleton<IWorker, Worker>()
    .AddSingleton<IHelloWorld, HelloWorld>()
    .AddHostedService<HostedService>()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
