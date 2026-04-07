using Microsoft.EntityFrameworkCore;
using TeamTasksApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<TeamTasksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS para permitir llamadas desde Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy
            .WithOrigins("http://localhost:4200") // origen de tu frontend Angular
            .AllowAnyMethod()
            .AllowAnyHeader());
});

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

// Activar CORS antes de MapControllers
app.UseCors("AllowAngular");

// Importante: NO usar autenticación si no la necesitas
// app.UseAuthentication();  <-- quítalo si estaba agregado
// app.UseAuthorization();   <-- quítalo si no usas roles/claims

app.MapControllers();

app.Run();
