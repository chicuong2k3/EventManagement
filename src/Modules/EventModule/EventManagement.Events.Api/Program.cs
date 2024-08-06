
using EventManagement.Events.Api.ExceptionHandlers;
using EventManagement.Events.Application;
using EventManagement.Events.Infrastructure;
using EventManagement.Events.Infrastructure.Extensions;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile($"file-name", false, true);

// Add Logging
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);    
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// Connection Strings
var dbConnectionString = builder.Configuration.GetConnectionString("Database")!;
var cacheConnectionString = builder.Configuration.GetConnectionString("Cache")!;

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(dbConnectionString)
    .AddRedis(cacheConnectionString); 

// Add Modules' Services
builder.Services.AddApplication([EventManagement.Events.Application.AssemblyReference.Assembly])
    .AddInfrastructure(dbConnectionString, cacheConnectionString);


// Add Carter
builder.Services.AddCarter();

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();




var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.Services.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// this must be come before Health Checks
app.MapCarter();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.Run();
