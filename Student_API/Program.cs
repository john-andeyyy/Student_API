using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Student_API.Data;
using Student_API.Migrations;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Get database connection settings
var dbHost = builder.Configuration["DB_HOST"] ?? "localhost";
var dbPort = builder.Configuration["DB_PORT"] ?? "3306";
var dbName = builder.Configuration["DB_NAME"] ?? "EmployeesDb";
var dbUser = builder.Configuration["DB_USER"] ?? "root";
var dbPass = builder.Configuration["DB_PASS"] ?? "";

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPass};";

Console.WriteLine($"Using Connection String: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Register FluentMigrator
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddMySql5() 
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(Program).Assembly).For.Migrations()); 



var app = builder.Build();

// Run FluentMigrator migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Test DB Connection
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    try
    {
        Console.WriteLine("Testing database connection...");
        dbContext.Database.OpenConnection();
        Console.WriteLine("✅ Database connection successful!");
        dbContext.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error connecting to database: " + ex.Message);
    }

    // Run migrations automatically on startup
    var migrator = services.GetRequiredService<IMigrationRunner>();
    // migrator.MigrateUp();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Urls.Add("http://0.0.0.0:5000"); // Allow access from all devices on the network

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Json(new { message = "Hello world!!, using git action test 3" }));

app.Run();
