using GestorMat.Application.Configuracion;
using GestorMat.Application.Interfaces;
using GestorMat.Application.Servicios;
using GestorMat.Infrastructure.Persistencia;
using GestorMat.Infrastructure.Repositorios;
using GestorMat.Infrastructure.Services;
using GestorMat.Infrastructure.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// Configuración de mapeos (Mapster)
// Se registran las conversiones entre entidades y DTOs
// =======================================================
MappingConfig.RegisterMappings();

// =======================================================
// Configuración de controladores (API REST)
// =======================================================
builder.Services.AddControllers();

// =======================================================
// Configuración de Swagger (documentación de la API)
// Incluye soporte para autenticación mediante JWT
// =======================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "GestorMat API",
        Version = "v1",
        Description = "API REST para la gestión de materiales"
    });

    // Inclusión de comentarios XML para documentar endpoints
    string xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Definición del esquema de seguridad JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresar el token JWT. Ejemplo: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    // Requerimiento global de autenticación para los endpoints
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

// =======================================================
// Configuración de base de datos (Entity Framework Core)
// =======================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// =======================================================
// Registro de servicios de aplicación (lógica de negocio)
// =======================================================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<UnidadMedidaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<DepositoService>();
builder.Services.AddScoped<MovimientoMaterialService>();
builder.Services.AddScoped<MaterialImportService>();
builder.Services.AddScoped<XmlService>();
builder.Services.AddScoped<PdfSaldoService>();

// =======================================================
// Registro de servicios de infraestructura
// (implementaciones técnicas externas)
// =======================================================
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IPdfSaldoService, PdfSaldoService>();

// =======================================================
// Registro de repositorios y patrón Unit of Work
// =======================================================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<IDepositoRepository, DepositoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IMovimientoMaterialRepository, MovimientoMaterialRepository>();
builder.Services.AddScoped<ISaldoRepository, SaldoRepository>();

// =======================================================
// Configuración de autenticación mediante JWT
// Incluye validaciones de seguridad y manejo de errores
// =======================================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer not configured");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience not configured");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    // Manejo personalizado de respuestas de seguridad
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
            {
                message = "Usted no está autenticado"
            }));
        },

        OnForbidden = context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
            {
                message = "Usted no tiene permiso para ejecutar esta acción"
            }));
        }
    };
});

// =======================================================
// Configuración de CORS (Cross-Origin Resource Sharing)
// Permite acceso desde cualquier origen (modo desarrollo)
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// =======================================================
// Construcción de la aplicación
// =======================================================
var app = builder.Build();

// =======================================================
// Configuración del pipeline HTTP (middleware)
// =======================================================
app.UseCors("AllowAll");

// Swagger habilitado solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestorMat API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Importante: orden correcto de middlewares de seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();