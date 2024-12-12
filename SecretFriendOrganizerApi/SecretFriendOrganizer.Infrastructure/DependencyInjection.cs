using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecretFriendOrganizer.Application.Interfaces.Services;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Infrastructure.Keycloak;
using SecretFriendOrganizer.Infrastructure.Persistence;
using SecretFriendOrganizer.Infrastructure.Repositories;
using SecretFriendOrganizer.Infrastructure.UnitOfWork;

namespace SecretFriendOrganizer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
   
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

         
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IGroupMembershipRepository, GroupMembershipRepository>();
            
            services.AddHttpClient<IKeycloakService, KeycloakService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Keycloak:BaseUrl"]);
            });


            return services;
        }
    }
}
