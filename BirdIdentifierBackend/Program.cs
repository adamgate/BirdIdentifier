
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable CORS for sites listed in an env var
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Production") ?? false)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: "_myAllowSpecificOrigins",
            policy =>
            {
                policy.WithOrigins(Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? "");
            }
        );
    });

    app.UseCors("_myAllowSpecificOrigins");
}
else
{
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

    app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
