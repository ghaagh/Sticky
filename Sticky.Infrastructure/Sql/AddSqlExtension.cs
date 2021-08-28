using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sticky.Domain.CategoryAggrigate;
using Sticky.Domain.HostAggrigate;
using Sticky.Domain.PartnerAggrigate;
using Sticky.Domain.SegmentAggrigate;
using Sticky.Domain.Shared;
using Sticky.Domain.UserAggrigate;
using Sticky.Infrastructure.Sql.Repositories;
namespace Sticky.Infrastructure.Sql
{
    public static class AddSqlExtension
    {
        public static IServiceCollection AddSql(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<Context>(options => { 
                options.UseSqlServer(connectionString);
            });
            services.AddIdentity<User,IdentityRole>(options => { 
                options.SignIn.RequireConfirmedAccount = true;
            
           }).AddEntityFrameworkStores<Context>();

            services.AddScoped<ICategoryRepository, CategoryRepository>(c => new CategoryRepository(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<IHostRepository, HostRepository>();
            services.AddScoped<ISegmentRepository, SegmentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }

}
