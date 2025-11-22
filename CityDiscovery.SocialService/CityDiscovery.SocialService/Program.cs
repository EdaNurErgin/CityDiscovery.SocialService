using Microsoft.EntityFrameworkCore;
using MassTransit;
using CityDiscovery.SocialService.SocialService.Application;
using SocialService.Infrastructure.Data;
using SocialService.Application;
using SocialService.Infrastructure.Extensions;
using CityDiscovery.SocialService.SocialService.Infrastructure.Messaging.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SocialDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MassTransit ve RabbitMQ yapılandırması
builder.Services.AddMassTransit(x =>
{
    // Consumer'ları kaydet
    x.AddConsumer<UserLoggedInConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMQHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitMQPort = builder.Configuration["RabbitMQ:Port"] ?? "5672";
        var rabbitMQUsername = builder.Configuration["RabbitMQ:Username"] ?? "guest";
        var rabbitMQPassword = builder.Configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(rabbitMQHost, ushort.Parse(rabbitMQPort), "/", h =>
        {
            h.Username(rabbitMQUsername);
            h.Password(rabbitMQPassword);
        });

        // Consumer endpoint'lerini yapılandır
        cfg.ReceiveEndpoint("user-logged-in-queue", e =>
        {
            e.ConfigureConsumer<UserLoggedInConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();