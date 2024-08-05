
using EventManagement.Events.Application;
using EventManagement.Events.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication([EventManagement.Events.Application.AssemblyReference.Assembly])
    .AddInfrastructure(builder.Configuration);



builder.Services.AddCarter();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.Run();
