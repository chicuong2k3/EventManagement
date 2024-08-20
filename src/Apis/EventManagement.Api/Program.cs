using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Serilog;
using EventManagement.Api.ExceptionHandlers;
using EventManagement.Common.Application;
using EventManagement.Common.Infrastructure;
using EventManagement.Events.Infrastructure;
using EventManagement.Users.Infrastructure;
using EventManagement.Api.Extensions;
using EventManagement.Ticketing.Infrastructure;
using EventManagement.Attendance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

List<string> modules = ["users", "events", "ticketing", "attendance"];
foreach (string module in modules)
{
    builder.Configuration.AddJsonFile($"modules.{module}.json", false, true);
    builder.Configuration.AddJsonFile($"modules.{module}.Development.json", true, true);
}

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
    .AddRedis(cacheConnectionString)
    .AddUrlGroup(new Uri(builder.Configuration.GetValue<string>("KeyCloak:HealthUrl")!), 
    HttpMethod.Get, 
    "key-cloak");

// Add Module's Services
builder.Services.AddCommonApplication([
    EventManagement.Events.Application.AssemblyReference.Assembly,
    EventManagement.Users.Application.AssemblyReference.Assembly,
    EventManagement.Ticketing.Application.AssemblyReference.Assembly,
    EventManagement.Attendance.Application.AssemblyReference.Assembly
])
.AddCommonInfrastructure(
    [EventManagement.Ticketing.Infrastructure.DependencyInjection.ConfigureConsumers], 
    dbConnectionString, 
    cacheConnectionString
)
.AddEventsInfrastructure(builder.Configuration)
.AddUsersInfrastructure(builder.Configuration)
.AddTicketingInfrastructure(builder.Configuration)
.AddAttendanceInfrastructure(builder.Configuration);


// Add Carter
builder.Services.AddCarter();

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


// CORS
builder.Services.AddCors();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.Services.ApplyMigrations();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173");
});

// this must be come before Health Checks
app.MapCarter();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
