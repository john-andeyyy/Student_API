using Microsoft.EntityFrameworkCore;
using Student_API.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});



//for ubuntu
//"ConnectionStrings": {
//    "ProductionConnection": "Server=192.168.100.109;Database=EmployeesDb;Port=3306;User=root;Password=Andrei_123!;"
//},


var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"Environment: {environment}");

var dbHost = builder.Configuration["DB_HOST"] ?? "localhost";
var dbPort = builder.Configuration["DB_PORT"] ?? "3306";
var dbName = builder.Configuration["DB_NAME"] ?? "EmployeesDb";
var dbUser = builder.Configuration["DB_USER"] ?? "root";
var dbPass = builder.Configuration["DB_PASS"] ?? "";
//connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


var connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPass};";
Console.WriteLine($"Using Connection String: {connectionString}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

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
}

// add-migration "Initial Migration"
// to run the migration --- update-database


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}   
    
app.Urls.Add("http://0.0.0.0:5000"); // Allow all devices in the network
//app.Urls.Add("https://0.0.0.0:5001"); // If using HTTPS
//app.UseHttpsRedirection();

app.UseCors("AllowAll");



app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Json(new { message = "Hello world!" }));

app.Run();
