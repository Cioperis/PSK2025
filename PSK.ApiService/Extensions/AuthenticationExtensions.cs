using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PSK.ApiService.Authentication;
using System.Text;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddPskJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtKey = config["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT Key not found.");
        var jwtIssuer = "PSK.ApiService";
        var jwtAudience = "PSK.Client";

        services.Configure<JwtSettings>(options =>
        {
            options.Key = jwtKey;
            options.Issuer = jwtIssuer;
            options.Audience = jwtAudience;
            options.ExpiresInMinutes = 60;
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        return services;
    }
}
