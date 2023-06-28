using System.IO.Compression;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Todo.Configurations;
using Todo.Data;
using Todo.Models;
using Todo.Repositories;
using Todo.Repositories.Contracts;
using Todo.Services;

class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ControllersConfigurations(builder);
        JWTConfigurations(builder);
        DatabaseConfigurations(builder);
        DependencyInjectionConfigurations(builder);
        CompressionConfigurations(builder);
        CorsConfigurations(builder);
        SwaggerConfigurations(builder);

        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        JwtConfiguration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
        SmtpConfiguration.Host = app.Configuration.GetSection("SmtpConfiguration").GetValue<string>("Host");
        SmtpConfiguration.Port = app.Configuration.GetSection("SmtpConfiguration").GetValue<int>("Port");
        SmtpConfiguration.Username = app.Configuration.GetSection("SmtpConfiguration").GetValue<string>("Username");
        SmtpConfiguration.Password = app.Configuration.GetSection("SmtpConfiguration").GetValue<string>("Password");

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
        );

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseResponseCompression();
        app.MapControllers();

        app.Run();
    }

    public static void ControllersConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

    }
    public static void JWTConfigurations(WebApplicationBuilder builder)
    {
        var key = System.Text.Encoding.ASCII.GetBytes(JwtConfiguration.JwtKey);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }
    public static void SwaggerConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "JWTToken_Auth_API",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
            });
        });
    }
    public static void CorsConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddCors();

    }
    public static void DependencyInjectionConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<TokenService>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddTransient<EmailService>();
    }
    public static void CompressionConfigurations(WebApplicationBuilder builder)
    {
        builder.Services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });
    }
    public static void DatabaseConfigurations(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("SqlServer");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

}