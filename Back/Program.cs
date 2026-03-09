using Microsoft.EntityFrameworkCore;
using Back.Infrastructure.Data;
using Back.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de JWT (Asegúrate que coincida con tu appsettings.json)
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

// 2. Base de Datos PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 3. Política de CORS (¡Vital para que React pueda conectar!)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => {
        policy.WithOrigins("http://localhost:5173") // Puerto típico de Vite/React
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 4. Servicios (Dependency Injection)
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<SupplierService>();

// 5. Configurar Autenticación JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// --- MIDDLEWARE PIPELINE ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// El orden aquí es sagrado:
app.UseHttpsRedirection();
app.UseCors("AllowReactApp"); // Activa CORS antes de Auth

app.UseAuthentication(); // ¿Quién eres?
app.UseAuthorization();  // ¿Tienes permiso?

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Esto aplica las migraciones pendientes y crea la BD si no existe
        context.Database.Migrate(); 
        Console.WriteLine("¡Base de datos sincronizada correctamente!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al migrar la base de datos: {ex.Message}");
    }
}

app.Run();

// Al final de Program.cs
public partial class Program { }