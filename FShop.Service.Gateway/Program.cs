using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Add OcelotConfig
builder.Configuration.AddJsonFile("ocelotConfig.json", false, true);
builder.Services.AddOcelot(builder.Configuration);
var configuration = builder.Configuration;
var service = builder.Services;


service.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

#region Authen
var jwtSerect = Encoding.ASCII.GetBytes(configuration.GetSection("JWTSerect").Value);
service.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(jwtSerect),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
#endregion

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.UseCors("MyPolicy");
app.UseAuthentication();
await app.UseOcelot();
app.Run();
