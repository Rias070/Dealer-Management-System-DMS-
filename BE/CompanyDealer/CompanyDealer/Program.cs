
using CompanyDealer.BLL.Services;
using CompanyDealer.DAL.Data;
using CompanyDealer.DAL.Repository;
using CompanyDealer.DAL.Repository.VehicleRepo;
using CompanyDealer.DataInitalizer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



// DI for repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.UserRepo.IAccountRepository, CompanyDealer.DAL.Repository.UserRepo.AccountRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.RoleRepo.IRoleRepository, CompanyDealer.DAL.Repository.RoleRepo.RoleRepository>();
builder.Services.AddScoped<CompanyDealer.DAL.Repository.TokenRepo.ITokenRepository, CompanyDealer.DAL.Repository.TokenRepo.TokenRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();



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
// Ensure database is up-to-date and seed demo data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        // apply migrations (safe to call; if you prefer EnsureCreated replace this)
        await db.Database.MigrateAsync();

        // seed demo data (idempotent)
        await DataInitializer.SeedAsync(db);
    }
    catch (Exception ex)
    {
        // Log and rethrow so the host fails to start when seeding fails
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
        throw;
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
