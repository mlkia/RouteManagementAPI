using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RouteManagementTutorial.Authenticate;
using RouteManagementTutorial.Entities;
using RouteManagementTutorial.Models;
using RouteManagementTutorial.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteManagementTutorialDataBaseSettings>(
builder.Configuration.GetSection("RouteManagementTutorialDataBase"));

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DriverSelf", policy =>
        policy.RequireAssertion(context =>
        {
            var emailClaim = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var route = context.Resource as Driver; 
            return emailClaim == route?.Email;
        }));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
    }});
});


builder.Services.AddSingleton<DriversService>();
builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<RouteService>();
builder.Services.AddSingleton<CreateAuthentication>();

var app = builder.Build();

/// <summary>
/// Configures Cross-Origin Resource Sharing (CORS) for the application.
/// This allows external web applications to access the API while ensuring security by restricting origins and request methods.
/// The configuration allows:
/// - Any HTTP headers to be sent in the request.
/// - Requests from the specified origin: "http://localhost:5173" (for the React app).
/// - Any HTTP methods (GET, POST, PUT, DELETE, etc.) to be used in the request.
/// 
/// This is essential for enabling communication between the React frontend and the ASP.NET Web API backend running on different ports.
/// Remove this before production.
/// </summary>
app.UseCors(c => c.AllowAnyHeader().WithOrigins("http://localhost:5173").AllowAnyMethod());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
