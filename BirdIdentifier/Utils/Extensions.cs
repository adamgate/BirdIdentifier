using BirdIdentifier.Data;
using Microsoft.EntityFrameworkCore;

namespace BirdIdentifier.Utils;

public static class Extensions
{
    public static WebApplication MigrateDatabase(this WebApplication webApplication)
    {
        // Manually run any pending migrations if configured to do so.
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (env == "Production")
        {
            var serviceScopeFactory = (IServiceScopeFactory)webApplication.Services.GetService(typeof(IServiceScopeFactory));
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<DataContext>();
                dbContext.Database.Migrate();
            }
        }
        return webApplication;
    }
}
