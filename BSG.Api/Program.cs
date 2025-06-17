using System.Net;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BSG.BackEnd.Common.Model;
using BSG.BackEnd.Services;
using BSG.BackEnd.Services.Encryption;
using BSG.BackEnd.Services.Jwt;
using BSG.BackEnd.Services.Mail;
using BSG.Database;
using BSG.Database.Mappings;
using BSG.Features;
using BSG.Repository;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;

namespace BSG.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File(@"C:\Logs\BSG\Api\log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();
        
        var builder = WebApplication.CreateBuilder(args);
        
        /*builder.WebHost.ConfigureKestrel(so =>
        {
            so.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
            so.Limits.MaxConcurrentConnections = 100;
            so.Limits.MaxConcurrentUpgradedConnections = 100;
            so.Limits.MaxRequestBodySize = 100_000_000;
            so.ListenAnyIP(5130);
            so.Listen(IPAddress.Loopback, 7260, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                listenOptions.UseHttps();
            });
        });*/
        
        // Add services to the container.

        builder.Services
            .AddAutoMapper((sp, am) =>
            {
                am.AddCollectionMappers();
                am.UseEntityFrameworkCoreModel<BsgDbContext>(sp);
                am.AddProfile<SqlMappingsProfile>();
            }, typeof(BsgDbContext).Assembly)
            .AddHttpContextAccessor();

        #region Configs

        var keyConfig = new Keys();
        builder.Configuration.Bind("Keys", keyConfig);

        var mailParameter = new MailParameters();
        builder.Configuration.Bind("MailParameters", mailParameter);

        var frontEndParameter = new FrontEndParameters();
        builder.Configuration.Bind("FrontEndParameters", frontEndParameter);

        var passwordSettings = new PasswordSettings();
        builder.Configuration.Bind("PasswordSettings", passwordSettings);

        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind("JwtSettings", jwtSettings);

        var userSettings = new UserSettings();
        builder.Configuration.Bind("UserSettings", userSettings);

        builder.Services
            .AddSingleton(jwtSettings)
            .AddSingleton(keyConfig)
            .AddSingleton(mailParameter)
            .AddSingleton(frontEndParameter)
            .AddSingleton(passwordSettings)
            .AddSingleton(userSettings);

        #endregion

        builder.Services.AddDbContext<BsgDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("BsgDbContext")));
            options.UseSqlite(builder.Configuration.GetConnectionString("BsgDbContext")));

        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(o=> o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        
        #region Swagger
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Description = "BSG Web API",
                Title = "BSG Web API",
                Version = "v1"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization Header using Bearer scheme."
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
                    []
                }
            });
        });

        #endregion

        builder.Services
            // C
            .AddScoped<IComponentRepository, ComponentRepository>()
            // D
            .AddScoped<IDateConverterService, DateConverterService>()
            // E
            .AddScoped<IElementRepository, ElementRepository>()
            .AddScoped<IEncryptionService, EncryptionService>()
            // J
            .AddScoped<IJwtUtils, JwtUtils>()
            // M
            .AddScoped<IMailService, MailService>()
            // P
            .AddScoped<IProductRepository, ProductRepository>()
            .AddScoped<IProductTypeRepository, ProductTypeRepository>()
            // U
            .AddScoped<IUserAuthRepository, UserAuthRepository>()
            .AddScoped<IUserFeature, UserFeature>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IUserPasswordRepository, UserPasswordRepository>();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        try
        {
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors(o => o
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
            );
            
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            Log.Information("Host started");
            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Host terminated unexpectedly");
            throw;
        }
        
    }
}