using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using SecretFriendOrganizer.Infrastructure.Persistence;
using SecretFriendOrganizer.Infrastructure;
using Serilog;
using SecretFriendOrganizer.Application.Interfaces.Services;
using SecretFriendOrganizer.Application.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();

var keycloakConfig = builder.Configuration.GetSection("Keycloak");

// Configuração de Autenticação JWT com Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{keycloakConfig["BaseUrl"]}/realms/{keycloakConfig["Realm"]}";
        options.Audience = keycloakConfig["ClientId"];
        options.RequireHttpsMetadata = false;

        // Configuração para validar o token JWT
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{keycloakConfig["BaseUrl"]}/realms/{keycloakConfig["Realm"]}",
            ValidateAudience = true,
            ValidAudience = keycloakConfig["ClientId"],
            ValidateLifetime = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
            {
                // Buscar as chaves de assinatura publicadas pelo Keycloak
                var client = new System.Net.Http.HttpClient();
                var response = client.GetAsync($"{keycloakConfig["BaseUrl"]}/realms/{keycloakConfig["Realm"]}/protocol/openid-connect/certs").Result;
                var json = response.Content.ReadAsStringAsync().Result;
                var keys = new JsonWebKeySet(json);
                return keys.GetSigningKeys();
            }
        };

        // Adicionar logs detalhados
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Exception, "Authentication failed.");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Token validated successfully.");
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Token received: {Token}", context.Token);
                return Task.CompletedTask;
            }
        };
    });

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug() // Adicionar sink de debug para Visual Studio
    .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Adicionar autenticação
app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy");

app.Run();
