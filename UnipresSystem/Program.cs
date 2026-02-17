using Entity.Dtos._00_DataUPM;
using Entity.Dtos._00_DataUPM.DataSecurityModule;
using Entity.Dtos._00_DataUPM.DataSecurityRole;
using Entity.Dtos._01_Auth.DataSecurityPermission;
using Entity.Dtos._01_Auth.DataSecurityRoleClaim;
using Entity.Dtos._01_Auth.DataSecuritySubmodule;
using Entity.Dtos.AssyProduction;
using Entity.Dtos.DataProduction.DataProductionLine;
using Entity.Dtos.ProductionControl;
using Entity.Interfaces;
using Entity.ModelDtos._02_DataProduction.DataProductionDowntime;
using Entity.Models.Auth;
using LogicData.Context;
using LogicDomain;
using LogicDomain._00_DataUPM;
using LogicDomain._01_Auth;
using LogicDomain.AssyProduction;
using LogicDomain.ApplicationServices;
using LogicDomain.DataProduction;
using LogicDomain.ModelServices._02_DataProduction;
using LogicDomain.ProductionControl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UnipresSystem.Data;
using LogicDomain.SystemServices;
using LogicDomain.ModelServices.ProductionControl;

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

builder.Services.AddDbContext<ProductionControlContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Le decimos que guarde sus migraciones en una tabla con sufijo
        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_ProductionControl");
    }));

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Le decimos que guarde sus migraciones en una tabla con sufijo
        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Data");
    }));

builder.Services.AddDbContext<AssyProductionContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Le decimos que guarde sus migraciones en una tabla con sufijo
        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_AssyProduction");
    }));

builder.Services.AddDbContext<TemporalContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Le decimos que guarde sus migraciones en una tabla con sufijo
        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Temporal");
    }));

//***************************************************************************************************
//***************************************************************************************************
// *** IMPORTANTE: Configura CORS para permitir peticiones desde tu app Angular ***
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", // Un nombre de pol�tica unificado
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
//***************************************************************************************************
//***************************************************************************************************

// 3. Registrar .NET Identity
builder.Services.AddIdentity<AuthUser, AuthRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 5;
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
//***************************************************************************************************
//***************************************************************************************************

// --- Carga dinámica de permisos para las Políticas ---
// Obtenemos una conexión a la BBDD SÓLO para leer los permisos
var tempServices = builder.Services.BuildServiceProvider();
var dbContext = tempServices.GetRequiredService<AuthContext>();

// Leemos todas las claves de permiso de la BBDD
// (Asegúrate de que el seeder ya haya corrido o la tabla exista)
List<string> permisosClave = new List<string>();
try
{
    permisosClave = dbContext.Permissions.Select(p => p.Clave).ToList();
}
catch (Exception ex)
{
    // La BBDD no está lista aún, quizás la migración no ha corrido.
    // Omitir por ahora, pero idealmente esto se hace después de la migración.
    // O puedes añadir los permisos 'SuperAdmin' manualmente.
    Console.WriteLine($"ADVERTENCIA: No se pudieron cargar permisos desde la BBDD. {ex.Message}");
}


// 4. ¡LA CLAVE! Registrar Autorización por Políticas
builder.Services.AddAuthorization(options =>
{
    // Política de SuperAdmin (siempre debe existir)
    options.AddPolicy("SuperAdminPolicy", policy =>
        policy.RequireRole("SuperAdmin"));

    // Registra cada permiso de la BBDD como una política
    foreach (var clave in permisosClave)
    {
        options.AddPolicy(clave, policy =>
            policy.RequireClaim("Permiso", clave));
    }
});

//***************************************************************************************************
//***************************************************************************************************

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DataProductionModelService>();
builder.Services.AddScoped<DataProductionAreaService>();
builder.Services.AddScoped<DataProductionLocationService>();
builder.Services.AddScoped<DataProductionPartNumberService>();

builder.Services.AddScoped<IServiceCrud<DataSecurityUserDto, DataSecurityUserCreateDto, DataSecurityUserUpdateDto>, DataSecurityUserService>();

builder.Services.AddScoped<IService<DataSecurityRoleResponseDto, DataSecurityRoleRequestDto>, DataSecurityRoleService>();
builder.Services.AddScoped<IService<DataSecurityModuleResponseDto, DataSecurityModuleRequestDto>, DataSecurityModuleService>();
builder.Services.AddScoped<IService<DataSecuritySubmoduleResponseDto, DataSecuritySubmoduleRequestDto>, DataSecuritySubmoduleService>();
builder.Services.AddScoped<IService<DataProductionDowntimeResponseDto, DataProductionDowntimeRequestDto>, DataProductionDowntimeService>();
builder.Services.AddScoped<IService<DataSecurityPermissionResponseDto, DataSecurityPermissionRequestDto>, DataSecurityPermissionService>();

builder.Services.AddScoped<IServiceRoleClaim<DataSecurityRoleClaimResponseDto, DataSecurityRoleClaimRequestDto>, DataSecurityRoleClaimService>();

builder.Services.AddScoped<IServiceCrud<ProductionLineDto, ProductionLineCreateDto, ProductionLineUpdateDto>, DataProductionLineService>();
builder.Services.AddScoped<IServiceCrud<PartNumberLogisticsResponseDto, PartNumberLogisticsCreateDto, PartNumberLogisticsUpdateDto>, PartNumberLogisticsService>();
builder.Services.AddScoped<IServiceCrud<PartNumberLocationDto, PartNumberLocationCreateDto, PartNumberLocationUpdateDto>, PartNumberLocationService>();
builder.Services.AddScoped<IServiceCrud<ProductionStationDto, ProductionStationCreateDto, ProductionStationUpdateDto>, ProductionStationService>();

builder.Services.AddScoped<OperationalAnalysisService>();
builder.Services.AddScoped<DowntimeCaptureService>();
builder.Services.AddScoped<LdapService>();

builder.Services.AddScoped<IComponentAlertService, LogicDomain.ModelServices.ComponentAlertService>();
builder.Services.AddScoped<MaterialSupplierService>();
builder.Services.AddScoped< PartNumberEstructureService>();


//**************************************************************** ***********************************
//***************************************************************************************************


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//***************************************************************************************************
//***************************************************************************************************

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Ejecutamos nuestro método Seeder
        await DbSeeder.SeedRolesAndSuperAdminAsync(services);
    }
    catch (Exception ex)
    {
        // Opcional: loggear el error
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error durante el sembrado de la BBDD");
    }
}

//***************************************************************************************************
//***************************************************************************************************

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
