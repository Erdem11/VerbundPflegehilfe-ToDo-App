using Serilog;
using VerbundPflegehilfe.API.Middlewares;
using VerbundPflegehilfe.Application;
using VerbundPflegehilfe.Infrastructure;
using VerbundPflegehilfe.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Serilog Setup
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (allowedOrigins == null)
{
    var originsString = builder.Configuration["Cors:AllowedOrigins"];
    if (!string.IsNullOrWhiteSpace(originsString))
    {
        allowedOrigins = originsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim()).ToArray();
    }
}
allowedOrigins ??= ["http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        corsBuilder => corsBuilder
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

// Integration Test Target
public abstract partial class Program;