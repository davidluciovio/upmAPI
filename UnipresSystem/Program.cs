using Entity.Models;
using LogicData.ContextAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT SecretKey no est� configurada en appsettings.json");
}

//***************************************************************************************************

// 1. Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// 2. Registrar el DbContext
builder.Services.AddDbContext<AuthContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Le decimos que guarde sus migraciones en una tabla con sufijo
        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Authentication");
    }));

// 3. Registrar .NET Identity
builder.Services.AddIdentity<AuthUser, AuthRole>(options =>
    {
        options.Password.RequireDigit = false; // Configura tus reglas
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AuthContext>()
    .AddDefaultTokenProviders();

// 4. Registrar Autenticación con JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Validar el emisor
            ValidateAudience = true, // Validar la audiencia
            ValidateLifetime = true, // Validar tiempo de expiraci�n
            ValidateIssuerSigningKey = true, // Validar la firma
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            // Permite un peque�o margen de tiempo (Clock Skew) por si los relojes no est�n perfectamente sincronizados
            ClockSkew = TimeSpan.Zero // O un valor peque�o como TimeSpan.FromMinutes(1)
        };
    });

// 4. ¡LA CLAVE! Registrar Autorización por Políticas
builder.Services.AddAuthorization(options =>
    {
        // Aquí registras CADA permiso de tu sistema
        // (Esto se puede automatizar, pero empecemos manualmente)

        //options.AddPolicy("Permisos.Ventas.Facturacion.Crear", policy =>
        //    policy.RequireClaim("Permiso", "Permisos.Ventas.Facturacion.Crear"));

        //options.AddPolicy("Permisos.Ventas.Facturacion.Leer", policy =>
        //    policy.RequireClaim("Permiso", "Permisos.Ventas.Facturacion.Leer"));

          // ... agrega todas tus políticas/permisos aquí
    });

//***************************************************************************************************

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
