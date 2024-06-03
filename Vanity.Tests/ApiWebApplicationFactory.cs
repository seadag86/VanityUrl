using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Vanity.Web.Data;

public class ApiWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor =
                services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(dbContextDescriptor!);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
        });

        builder.UseEnvironment("development");
    }
}
