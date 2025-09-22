using bca.api.Enums.Permissions;
using bca.api.Helpers;
using bca.api.Infrastructure.IRepository;
using bca.api.Infrastructure.Repository;
using bca.api.Middlewares;
using bca.api.Security.Authorization;
using bca.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PropertyManage.Data;
using PropertyManage.Data.Entities;
using PropertyManage.Infrastructure.IRepository;
using PropertyManage.Infrastructure.Repository;
using PropertyManage.ServiceInfra.IServices;
using PropertyManage.ServiceInfra.Services;
using Serilog;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

string[] allowedOrigins = new string[] { "http://localhost:4200", "https://yourdomain.com" };

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), opts => opts.CommandTimeout(180)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        //builder => builder.WithOrigins(allowedOrigins)
        builder => builder.AllowAnyOrigin()    // Allows requests from any origin
                          .AllowAnyMethod()
                          .AllowAnyHeader());
    //   .AllowCredentials()); // Removed AllowCredentials() for development
});
builder.Services.AddHttpContextAccessor();
// Register Generic and Specific Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserDocumentRepository, UserDocumentRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();







// Register Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();





//builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    foreach (var perm in Enum.GetNames(typeof(OnboardingPermissions))
             .Concat(Enum.GetNames(typeof(UserPermissions)))
             .Concat(Enum.GetNames(typeof(MasterDataPermissions)))
             /* add other enums here */)
    {
        options.AddPolicy($"Permission:{perm}", policy =>
            policy.Requirements.Add(new PermissionRequirement(perm)));
    }
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs to console
    .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)  // Logs to file daily
    .Enrich.FromLogContext()
    .CreateLogger();

// Replace default logger with Serilog
builder.Host.UseSerilog();

// Configure JSON Serialization to Handle Circular References
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Scheme = "bearer"
    });

    c.SupportNonNullableReferenceTypes();

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
            new string[] { }
        }
    });

    //c.OperationFilter<FileUploadOperationFilter>();
});

// Set default timezone for the application
string timeZoneId;

if (OperatingSystem.IsWindows())
    timeZoneId = "India Standard Time";   // Windows
else
    timeZoneId = "Asia/Kolkata";          // Linux

TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-IN");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-IN");

var app = builder.Build();

// TODO: Uncomment for production build
//app.UseHttpsRedirection();
//if (app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BCA API V1")); // Enable middleware to serve swagger-ui.
//}

app.UseCors("CorsPolicy");

// Enable serving static files
app.UseStaticFiles();

// Serve files from "uploads" inside wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedData(services);
}

app.Run();

