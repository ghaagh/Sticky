using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Sticky.Infrastructure.Sql
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            var configuration = configurationBuilder.Build();
            var connectionString = configuration
                        .GetConnectionString("Sticky");

            var optionsBuilder = new DbContextOptionsBuilder<Context>()
                .UseSqlServer(connectionString,
                    b
                        => b.MigrationsAssembly("Sticky.Infrastructure"));


            return new Context(optionsBuilder.Options);
        }
    }
}
