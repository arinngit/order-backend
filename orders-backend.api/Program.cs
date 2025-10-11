using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetEnv;
using Prometheus;
using Microsoft.AspNetCore.HttpOverrides;
using OrdersBackend.Api.Extensions;
using OrdersBackend.Insfrastructure.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5160");

Env.Load(".env");

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Database
string host = Env.GetString("HOST");
string port = Env.GetString("PORT");
string database = Env.GetString("DATABASE");
string username = Env.GetString("USERNAME");
string password = Env.GetString("PASSWORD");

string productHost = Env.GetString("PRODUCT_HOST");
string productPort = Env.GetString("PRODUCT_PORT");
string productDatabase = Env.GetString("PRODUCT_DATABASE");
string productUsername = Env.GetString("PRODUCT_USERNAME");
string productPassword = Env.GetString("PRODUCT_PASSWORD");


string connectionStringForAuthDb = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
string connectionStringForProductDb = $"Host={productHost};Port={productPort};Database={productDatabase};Username={productUsername};Password={productPassword};";

builder.Services.Configure<ConnectionStringOptions>(x =>
{
    x.ConnectionStringForAuthDb = connectionStringForAuthDb;
    x.ConnectionStringForProductDb = connectionStringForProductDb;
});

builder.Services.AddDatabase();

#endregion

#region Repositories
builder.Services.AddRepositories();
#endregion

#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
#endregion

#region Services
builder.Services.AddServices();
#endregion

#region Authentication

builder.Services.AddAuth(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerAuth();
}

#endregion

#region Validation

builder.Services.AddValidation();

#endregion


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpMetrics();
app.UseHttpsRedirection();

app.MapControllers();
app.MapMetrics();

app.Run();