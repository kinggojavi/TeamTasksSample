using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();

// Swagger con Swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<TeamTasksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamTasks API v1");
        c.RoutePrefix = "swagger";
    });
}


app.MapControllers();

app.Run();