//DRASTI PATEL
//MARCH 30, 2025
//PROLEM ANALYSIS 03

using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register EF Core with SQLite
builder.Services.AddDbContext<QuotesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("QuotesDb")));

// Add Swagger support for API documentation/testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow requestss from any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//Build app
var app = builder.Build();

app.UseCors("AllowAllOrigins");   //Apply CORS policy gloabally

//Enable Swagger UI and endpoint
app.UseSwagger(); 
app.UseSwaggerUI();

// Use authorization middleware
app.UseAuthorization();

//map controller routes to their respective endpoints
app.MapControllers();

// Redirect root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
//start application
app.Run();