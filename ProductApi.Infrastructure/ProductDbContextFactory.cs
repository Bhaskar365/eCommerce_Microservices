using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure
{
    internal class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            try
            {
                var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "ProductApi.Presentation"));
                Console.WriteLine("Looking for config at: " + basePath);

                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();
                var connectionString = config.GetConnectionString("eCommerceConnection");

                Console.WriteLine("Connection string: " + connectionString);

                optionsBuilder.UseSqlServer(connectionString,
                    x => x.MigrationsAssembly("ProductApi.Infrastructure"));

                return new ProductDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                Console.WriteLine("💥 ERROR CREATING DB CONTEXT:");
                Console.WriteLine(ex.ToString());  // <- shows full stack trace
                throw;
            }
        }
    }
}