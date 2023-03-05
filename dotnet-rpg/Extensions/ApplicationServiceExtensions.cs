using dotnet_rpg.Persistence;
using dotnet_rpg.Services.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_rpg.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("WWW-Authenticate", "Pagination")
                        .WithOrigins("http://localhost:3000", "https://localhost:3000");
                });
            });
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value!)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            return services;
        }
    }
}
