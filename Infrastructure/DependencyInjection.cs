using Contracts.Common;
using Contracts.Database;
using Contracts.Managers;
using Contracts.Repositories;

using Domain.Entities;

using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Common;
using Infrastructure.Database;
using Infrastructure.Mapping;
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

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
            services.AddFluentValidationAutoValidation();

            services.AddScoped<IMainContext, MainContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            AddRepositories(services);
            AddManagers(services);
        }
    }
}