using Microsoft.EntityFrameworkCore;
using Student_API.Data;

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



// the if else statement is used to check if the environment is development or production
var env = builder.Environment;


//for ubuntu
//"ConnectionStrings": {
//    "ProductionConnection": "Server=192.168.100.109;Database=EmployeesDb;Port=3306;User=root;Password=Andrei_123!;"
//},

string connectionString;

if (env.IsDevelopment())
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}
else if (env.IsProduction())
{
    connectionString = builder.Configuration.GetConnectionString("ProductionConnection");
}
else
{
    throw new Exception("Invalid Environment Configuration!");
}

connectionString = "Server=172.31.89.26;Database=EmployeesDb;Port=3306;User=root;Password=Andrei_123!";
Console.WriteLine("the Enviroment is:" + env);
Console.WriteLine("THE CONNECTION STRING IS: " + connectionString);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        Console.WriteLine("Testing database connection...");
        dbContext.Database.OpenConnection();
        Console.WriteLine("Database connection successful!");
        dbContext.Database.CloseConnection();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error connecting to database: " + ex.Message);
    }
}


// add-migration "Initial Migration"
// to run the migration --- update-database


var app = builder.Build();

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
app.MapGet("/", () => Results.Json(new { message = connectionString }));

app.Run();
