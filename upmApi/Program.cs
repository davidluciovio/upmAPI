using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using upmData.Context;
using upmDomain.Auth;
using upmDomain.DomainModel;
using upmDomain.Lider;
using upmDomain.LineDomain;
using upmDomain.ProductionReport;
using upmDomain.Shift;
using upmDomain.UserTools;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];


if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT SecretKey no está configurada en appsettings.json");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validar el emisor
        ValidateAudience = true, // Validar la audiencia
        ValidateLifetime = true, // Validar tiempo de expiración
        ValidateIssuerSigningKey = true, // Validar la firma
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
        // Permite un pequeño margen de tiempo (Clock Skew) por si los relojes no están perfectamente sincronizados
        ClockSkew = TimeSpan.Zero // O un valor pequeño como TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddDbContext<UpmwebContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// *** IMPORTANTE: Configura CORS para permitir peticiones desde tu app Angular ***
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", // Un nombre de política unificado
       policyBuilder =>
       {
           policyBuilder.WithOrigins(
                "http://localhost:4200",    // Angular dev
                "http://localhost:80",      // Otro local
                "http://upmap04:8002",     // Angular desplegado en upmap04
                "http://www.uniprestst.com",    // Angular desplegado en UPMTEST
                "http://www.unipresweb.com/"     // Angular desplegado en UPMTEST
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
       });

});

// Add services to the container.
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LiderService>();
builder.Services.AddScoped<ModelService>();
builder.Services.AddScoped<WorkShiftService>();
builder.Services.AddScoped<ProductionReportService>();
builder.Services.AddScoped<LineService>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
