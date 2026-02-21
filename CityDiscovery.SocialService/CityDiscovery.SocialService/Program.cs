using CityDiscovery.SocialService.API.Consumers;
using CityDiscovery.SocialService.Infrastructure.Messaging.Consumers;
using CityDiscovery.SocialService.SocialService.Infrastructure.Messaging.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SocialService.Application;
using SocialService.Application.Interfaces;
using SocialService.Infrastructure.Data;
using SocialService.Infrastructure.Extensions;
using SocialService.Infrastructure.HttpClients;
using SocialService.Infrastructure.Messaging.Consumers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SocialDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MassTransit ve RabbitMQ yapılandırması
builder.Services.AddMassTransit(x =>
{
    // Consumer'ları kaydet
    x.AddConsumer<UserLoggedInConsumer>();
    x.AddConsumer<VenueDeletedConsumer>();
    x.AddConsumer<UserDeletedConsumer>();
    x.AddConsumer<SocialService.API.Consumers.PostDeletedConsumer>();
    x.AddConsumer<CityDiscovery.SocialService.SocialService.API.Consumers.ContentRemovedConsumer>();
    x.AddConsumer<VenueUpdatedConsumer>();

    x.AddConsumer<UserUpdatedConsumer>();


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

        cfg.ReceiveEndpoint("venue-deleted-queue", e =>
        {
            e.ConfigureConsumer<VenueDeletedConsumer>(context);
        });

        cfg.ReceiveEndpoint("user-deleted-queue", e =>
        {
            e.ConfigureConsumer<UserDeletedConsumer>(context);
        });

        
        // Bu kuyruk sayesinde mekan güncellemelerini dinleyeceğiz
        cfg.ReceiveEndpoint("venue-updated-queue", e =>
        {
            e.ConfigureConsumer<VenueUpdatedConsumer>(context);
        });
        // --------------------------------------
        cfg.ReceiveEndpoint("content-removed-queue", e =>
        {
            e.ConfigureConsumer<CityDiscovery.SocialService.SocialService.API.Consumers.ContentRemovedConsumer>(context);
        });

        cfg.ReceiveEndpoint("social-user-updated-queue", e =>
        {
            e.ConfigureConsumer<UserUpdatedConsumer>(context);
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();



// JWT Authentication yapılandırması
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "IdentityService",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "SocialService",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourSecretKeyThatShouldBeAtLeast32CharactersLong!"))
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
   
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CityDiscovery Sosyal Servis API",
        Version = "v1",
        Description = "CityDiscovery platformunda sosyal etkileşimleri yönetmek için API.",
    });

  
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// VenueServiceClient'ı HttpClient olarak kaydediyoruz ve adresini appsettings'den alıyoruz.
builder.Services.AddHttpClient<IVenueServiceClient, VenueServiceClient>(client =>
{
    // appsettings.json'daki "ServiceUrls:VenueService" değerini okur
    var url = builder.Configuration["ServiceUrls:VenueService"];
    client.BaseAddress = new Uri(url);
});

builder.Services.AddHttpClient<IIdentityServiceClient, SocialService.Infrastructure.HttpClients.IdentityServiceClient>(client =>
{
    // appsettings.json'da ServiceUrls altında IdentityService tanımlı olmalı
    var url = builder.Configuration["ServiceUrls:IdentityService"];
    client.BaseAddress = new Uri(url);
});

// Health Checks
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// Health Check Endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();