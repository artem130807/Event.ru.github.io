using EventRep.Application;
using EventRep.Api.ExceptionHandling;
using EventRep.Infrastructure;
using EventRep.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<EventRepDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.Run();

public partial class Program;
