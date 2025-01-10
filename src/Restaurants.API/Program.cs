using Microsoft.OpenApi.Models;
using Restaurants.API.Extensions;
using Restaurants.API.Middlerwares;
using Restaurants.Application.Extentions;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
    );

    var app = builder.Build();


    var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();

    await seeder.Seed();
    // Configure the HTTP request pipeline.

    app.UseMiddleware<ErrorHandlingMiddle>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>();
    app.UseSerilogRequestLogging();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();

    app.MapGroup("api/identity")
        .WithTags("Identity")
        .MapIdentityApi<User>();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

} 
catch(Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }