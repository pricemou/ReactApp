using Microsoft.EntityFrameworkCore;
using ReactApp.Server.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration de JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

// Configuration du DbContext avec SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
            .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), 
            opts => opts.CommandTimeout(60))); // Augmentez le délai d'attente à 60 secondes

// Ajouter les services CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("https://localhost:51574") // Autoriser l'origine de votre application React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Ajouter les services de contrôleurs
builder.Services.AddControllers();

// Configurer Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Utiliser CORS
app.UseCors("AllowReactApp");

app.UseDefaultFiles();
app.UseStaticFiles();

// Configurer le pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Forcer HTTPS en production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
