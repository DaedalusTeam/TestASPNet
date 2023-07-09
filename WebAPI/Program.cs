using Library;
using Logic;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//var dataSourceBuilder = new NpgsqlDataSourceBuilder(
//    builder.Configuration.GetConnectionString("DefaultConnection"));
////dataSourceBuilder.MapEnum();

//using var dataSource = dataSourceBuilder.Build();
//builder.Services.AddDbContext<MyDbContext>(options =>
//    options
//        .UseNpgsql(dataSource)
//        .EnableSensitiveDataLogging()
//        .UseSnakeCaseNamingConvention());

builder.Host.UseSerilog((ctx, cfg) => 
    cfg.ReadFrom.Configuration(ctx.Configuration));

builder.Services
    .AddSingleton<IWorker, Worker>()
    .AddSingleton<IHelloWorld, HelloWorld>()
    .AddHostedService<HostedService>()
    .AddHealthChecks()
    .Services
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc");

app.Run();
