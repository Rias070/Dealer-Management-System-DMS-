using CompanyDealer.BLL.Services;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Repository;
using CompanyDealer.DAL.Repository.VehicleRepo;
using CompanyDealer.DataInitalizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using CompanyDealer.BLL.Utils;
using CompanyDealer.DAL.Repository.RestockRepo;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.Configure<CompanyDealer.BLL.DTOs.Settings.JwtSettings>(configuration.GetSection("AppSettings"));
builder.Services.AddSingleton<IConfiguration>(configuration);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Authentication (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization policies (giữ nguyên nếu bạn đang dùng ở nơi khác)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireCompanyAdminRole", policy => policy.RequireRole("CompanyAdmin"));
    options.AddPolicy("RequireCompanyStaffRole", policy => policy.RequireRole("CompanyStaff"));
    options.AddPolicy("RequireDealerAdminRole", policy => policy.RequireRole("DealerAdmin"));
    options.AddPolicy("RequireDealerStaffRole", policy => policy.RequireRole("DealerStaff"));
    options.AddPolicy("RequireDealerManagerRole", policy => policy.RequireRole("DealerManager"));
    options.AddPolicy("RequireCompanyManagerRole", policy => policy.RequireRole("CompanyManager"));
});

// Controllers + JsonOptions (GỘP làm 1, tránh khai báo trùng)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175")
              .AllowAnyHeader()
              .AllowAnyMethod();
        // Nếu bạn dùng cookie/session: bật dòng dưới
        // .AllowCredentials();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OneUron API", Version = "v1" });

    c.MapType<TimeOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new OpenApiString("08:30:00")
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// DI
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.UserRepo.IAccountRepository, CompanyDealer.DAL.Repository.UserRepo.AccountRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.RoleRepo.IRoleRepository, CompanyDealer.DAL.Repository.RoleRepo.RoleRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.TokenRepo.ITokenRepository, CompanyDealer.DAL.Repository.TokenRepo.TokenRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.VehicleRepo.IVehicleRepository, CompanyDealer.DAL.Repository.VehicleRepo.VehicleRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.InventoryRepo.IInventoryRepository, CompanyDealer.DAL.Repository.InventoryRepo.InventoryRepository>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<IRestockRequestRepository, RestockRequestRepository>();
builder.Services.AddScoped<RestockRequestService>();

// Dealer services
builder.Services.AddScoped<CompanyDealer.DAL.Repository.DealerRepo.IDealerRepository, CompanyDealer.DAL.Repository.DealerRepo.DealerRepository>();
builder.Services.AddScoped<DealerService>();


// Register repository interface
//


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "http://localhost:5175") // Vite default port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Migrate + Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
        await DataInitializer.SeedAsync(db);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Nếu chưa cấu hình HTTPS port/dev cert, giữ tắt để tránh lỗi redirect
// app.UseHttpsRedirection();

app.UseCors("AllowReactApp");
app.UseAuthentication();     
app.UseAuthorization();

app.MapControllers();

app.Run();