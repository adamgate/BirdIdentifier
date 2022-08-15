using Microsoft.EntityFrameworkCore;

using BirdIdentifier.Models;
using Npgsql;

namespace BirdIdentifier.Data;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!.ToLower().Equals("development"))
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }
        else if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!.ToLower().Equals("production"))
        {
            // https://stackoverflow.com/questions/37276821/connecting-to-heroku-postgres-database-with-asp-net
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require, 
                TrustServerCertificate = true
            };
            
            var connString = builder.ToString();

            options.UseNpgsql(Configuration.GetConnectionString(connString));
        }
    }

    public DbSet<PredictionRating> PredictionRatings { get; set; }
}