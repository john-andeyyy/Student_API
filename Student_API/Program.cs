using Microsoft.EntityFrameworkCore;
using Student_API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// the if else statement is used to check if the environment is development or production
//var env = builder.Environment;

//string connectionString;

//if(env.IsDevelopment())
//{
//    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//}
//else if (env.IsProduction())
//{
//    connectionString = builder.Configuration.GetConnectionString("ProductionConnection");
//}
//else
//{
//    throw new Exception("Invalid Environment Configuration!");
//}

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseMySql(
//        connectionString,
//        ServerVersion.AutoDetect(connectionString)
//    ));

string connectionString = "Server=192.168.100.109;Database=EmployeesDb;Port=3306;User=root;Password=Andrei_123!;";
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





//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) 
//));
// add-migration "Initial Migration"
// to run the migration --- update-database


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Json(new { message = connectionString }));

app.Run();
