using System.Security.Claims;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrdersBackend.DataAccess.Factories;
using OrdersBackend.Api.Factories;
using OrdersBackend.Domain.Repositories;
using OrdersBackend.DataAccess.Repositories;
using OrdersBackend.Api.AuthorizationRequirements;
using OrdersBackend.Business.Services.Abstractions;
using OrdersBackend.Business.Services.Concrete;
using Microsoft.AspNetCore.Authorization;
using OrdersBackend.Api.AuthorizationHandlers;
using FluentValidation;
using OrdersBackend.Api.Validators;

namespace OrdersBackend.Api.Extensions;

public static class ServiceStorageExtensions
{
    public static void AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<IStatusService, StatusService>();
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<GetUserAllOrdersValidator>();
        services.AddValidatorsFromAssemblyContaining<ChangeOrderStatusRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<GetUserAllOrdersValidator>();
        services.AddValidatorsFromAssemblyContaining<CancelOrderRequestValidator>();
    }

    public static void AddSwaggerAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
            {
                Name = "Jwt Authentication",
                Description = "Type in a valid JWT Bearer",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "Jwt",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            }
                ;
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });
        });
    }

    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),

                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization(conf =>
        {
            conf.AddPolicy("AdminPolicy", options =>
            {
                options.AddRequirements(new AdminPolicyAuthorizationRequirements());
            });

            conf.AddPolicy("GetOrdersPolicy", options =>
            {
                options.AddRequirements(new GetOrdersPolicyAuthorizationRequirements());
            });
        });

        services.AddScoped<IAuthorizationHandler, AdminPolicyAuthorizationHandler>();
        services.AddScoped<IAuthorizationHandler, GetOrdersPolicyAuthorzationHandler>();
    }

    public static void AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Product Microservice API",
                Version = "v1"
            });
        });
    }
}
