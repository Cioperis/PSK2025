using PSK.ApiService.Authentication;
using PSK.ApiService.Caching.Interfaces;
using PSK.ApiService.Caching;
using PSK.ApiService.Messaging.Interfaces;
using PSK.ApiService.Messaging;
using PSK.ApiService.Repositories.Interfaces;
using PSK.ApiService.Repositories;
using PSK.ApiService.Services.Interfaces;
using PSK.ApiService.Services;

namespace PSK.ApiService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPskApiServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAutoMessageRepository, AutoMessageRepository>();
            services.AddScoped<IAutoMessageService, AutoMessageService>();
            services.AddScoped<IDiscussionRepository, DiscussionRepository>();
            services.AddScoped<IUserMessageRepository, UserMessageRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IDiscussionService, DiscussionService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<IRabbitMQueue, RabbitMQueue>();
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }
    }

}
