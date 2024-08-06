
using EventManagement.Events.Api.ExceptionHandlers;
using EventManagement.Events.Application;
using EventManagement.Events.Infrastructure;
using EventManagement.Events.Infrastructure.Extensions;
using Serilog;

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

builder.Services.AddApplication([EventManagement.Events.Application.AssemblyReference.Assembly])
    .AddInfrastructure(builder.Configuration);


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

app.MapCarter();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.Run();
