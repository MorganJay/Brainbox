using Contracts.Common;
using Contracts.Database;
using Contracts.Managers;
using Contracts.Repositories;

using Domain.Entities;

using FluentValidation;
using FluentValidation.AspNetCore;

using Infrastructure.Common;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Infrastructure.UserManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }

        private static void AddManagers(IServiceCollection services)
        {
            services.AddScoped(typeof(IUserManager<>), typeof(CustomUserManager<>));
        }

        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, UserRole>()
                    .AddEntityFrameworkStores<MainContext>()
                    .AddDefaultTokenProviders();

            services.AddDbContext<IMainContext, MainContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MainContext"));
            });

            services.AddAutoMapper(typeof(DependencyInjection));

            services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMainContext, MainContext>();

            AddRepositories(services);
            AddManagers(services);
        }
    }
}