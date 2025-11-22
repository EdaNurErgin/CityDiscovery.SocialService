using Microsoft.Extensions.DependencyInjection;
using SocialService.Application.Interfaces;
using SocialService.Infrastructure.HttpClients;
using SocialService.Infrastructure.Repositories;
using SocialService.Infrastructure.Messaging;
using Microsoft.Extensions.Http;
using CityDiscovery.SocialService.SocialService.Infrastructure.Messaging;


namespace SocialService.Infrastructure.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // IPostRepository istendiğinde, ona PostRepository'nin bir örneğini ver.
            // Scoped: Her bir HTTP isteği için bir tane örnek oluşturur.
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();

            // HttpClient'ı ve VenueServiceClient'ı kaydediyoruz.
            services.AddHttpClient<IVenueServiceClient, VenueServiceClient>(client =>
            {
                // VenueService'in adresini appsettings.json'dan alacağız.
                // Şimdilik buraya bir örnek adres yazalım.
                client.BaseAddress = new Uri("http://localhost:5001");
            });

            // HttpClient'ı ve IdentityServiceClient'ı kaydediyoruz.
            services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(client =>
            {
                // IdentityService'in adresini appsettings.json'dan alacağız.
                // Şimdilik buraya bir örnek adres yazalım.
                client.BaseAddress = new Uri("http://localhost:5000");
            });

            // MessageBus'ı kaydediyoruz.
            services.AddScoped<IMessageBus, MessageBus>();

            return services;
        }
    }
}