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
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (databaseUrl == null)
        {
            throw new ArgumentNullException(
                databaseUrl,
                "environment var DATABASE_URL should not be null."
            );
        }

        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/'),
            // SslMode = SslMode.Require,
            // TrustServerCertificate = true
        };

        var connString = builder.ToString();

        options.UseNpgsql(connString);
    }

    public DbSet<PredictionFeedback> PredictionFeedback { get; set; }
}
