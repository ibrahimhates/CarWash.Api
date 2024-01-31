
using CarWash.Core.Settings;
using CarWash.Repository.ConfigureRepositories;
using CarWash.Repository.Context;
using CarWash.Repository.UnitOfWork;
using CarWash.Service.ConfigureServices;
using CarWash.Service.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
if (environment.Equals("Development"))
{
    builder.Configuration.AddJsonFile($"appsettings.Development.json", true, true);
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins, builder =>
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

}
else
{
    builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true);
    builder.Services.AddLogging().AddSerilog();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyHeader()
                              .AllowAnyMethod()
                              .WithOrigins("https://localhost:3000", "http://localhost:3000");
                          });
    });

}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("localDb"));
});

builder.Configuration.AddJsonFile("jwtSettings.json");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));




builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();





builder.Services.AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "CarWash API",
            Version = "v1",
        });
    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        }
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new string[]{}
            }
        }
    );
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters.RoleClaimType = "role";
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true
    };
});
var app = builder.Build();
if (environment == "Development" || string.IsNullOrEmpty(environment))
    app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();