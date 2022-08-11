using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add prediction pool for ml model
// builder.Services.AddPredictionEnginePool<MLModel.ModelInput, MLModel.ModelOutput>()
//     .FromFile(@"MLModel/MLModel.zip");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

    
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // app.UseHttpLogging();
}

//Set the content root path and make it available to every class
AppDomain.CurrentDomain.SetData("ContentRootPath", Directory.GetCurrentDirectory());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();