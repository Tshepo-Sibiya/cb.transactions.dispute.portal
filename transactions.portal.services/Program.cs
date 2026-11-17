using FluentValidation;
using Microsoft.EntityFrameworkCore;
using transactions.portal.services.Data.Sql;
using transactions.portal.services.Middleware;
using transactions.portal.services.Repositories;
using transactions.portal.services.Services;
using transactions.portal.services.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateDisputeRequestValidator>();

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDisputeService, DisputeService>();

var sqlConnectionString = builder.Configuration.GetConnectionString("SqlServer")
    ?? builder.Configuration["Database:SqlServer"]
    ?? throw new InvalidOperationException(
        "Database connection string is required. Please configure 'Database:SqlServer' or 'ConnectionStrings:SqlServer' in appsettings or via the 'Database__SqlServer' environment variable.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

builder.Services.AddScoped<ITransactionRepository, SqlTransactionRepository>();
builder.Services.AddScoped<IDisputeRepository, SqlDisputeRepository>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions Dispute Portal API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    const int maxRetries = 10;
    var retryDelay = TimeSpan.FromSeconds(4);

    var dbContext = services.GetRequiredService<AppDbContext>();
    var connected = false;

    for (var attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            await dbContext.Database.MigrateAsync();
            connected = true;
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning("SQL Server connection attempt {Attempt}/{MaxRetries} failed: {Message}", attempt, maxRetries, ex.Message);
        }

        if (attempt == maxRetries)
        {
            throw new InvalidOperationException("Failed to connect to SQL Server after multiple attempts.");
        }

        await Task.Delay(retryDelay);
    }

    if (connected)
    {
        await SqlSeeder.SeedAsync(dbContext);
    }
}

app.Run();
