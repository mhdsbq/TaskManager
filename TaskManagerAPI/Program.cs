using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using TaskManagerAPI.Data;
using TaskManagerAPI.Data.Providers;
using TaskManagerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// DI for config
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Add Connection string to DI Container.
builder.Services.AddSingleton<string>(_ =>
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new ApplicationException("Database connection string is not configured"));

// DI for HttpContextAccessor.
builder.Services.AddHttpContextAccessor();

//  Add DbInitializer to DI Container
builder.Services.AddSingleton<IDbInitializer, DbInitializer>();

// DI for providers
builder.Services.AddScoped<IUserProvider, UserProvider>();

// DI for services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthHelperService, AuthHelperService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new List<string>()
        }
    });
});

// DI for auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException()))
    };
});
builder.Services.AddAuthorization();
var app = builder.Build();

// Global Exception handler
app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
    if (exception == null)
    {
        return;
    }

    var message = exception.Message;
    var statusCode = StatusCodes.Status500InternalServerError;

    switch (exception)
    {
        case ValidationException:
        case SecurityException:
            statusCode = StatusCodes.Status400BadRequest;
            break;
        default:
            message = "Internal server error.";
            break;
    }

    context.Response.StatusCode = statusCode;
    var errorResponseMessage = new { error = message };
    await context.Response.WriteAsJsonAsync(errorResponseMessage);
}));

// Database configure.
var databaseInitializer = app.Services.GetRequiredService<IDbInitializer>();
databaseInitializer.InitializeDb();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();