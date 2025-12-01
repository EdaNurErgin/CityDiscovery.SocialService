using Microsoft.EntityFrameworkCore;
using MassTransit;
using CityDiscovery.SocialService.SocialService.Application;
using SocialService.Infrastructure.Data;
using SocialService.Application;
using SocialService.Infrastructure.Extensions;
using CityDiscovery.SocialService.SocialService.Infrastructure.Messaging.Consumers;
using SocialService.Infrastructure.Messaging.Consumers;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CityDiscovery Sosyal Servis API",
        Version = "v1",
        Description = "CityDiscovery platformunda sosyal etkileşimleri (gönderiler, yorumlar ve beğeniler) yönetmek için API.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "CityDiscovery Ekibi",
            Email = "support@citydiscovery.com"
        }
    });

    // JWT Security Definition ekle
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Örnek: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

    // XML comments'i dahil et
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Tüm XML dosyalarını dahil et (diğer projeler için)
    var applicationXml = Path.Combine(AppContext.BaseDirectory, "SocialService.Application.xml");
    if (File.Exists(applicationXml))
    {
        c.IncludeXmlComments(applicationXml);
    }

    var sharedXml = Path.Combine(AppContext.BaseDirectory, "SocialServiceShared.Common.xml");
    if (File.Exists(sharedXml))
    {
        c.IncludeXmlComments(sharedXml);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// Authentication middleware'i Authorization'dan ÖNCE olmalı
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();