using FShop.Business.Base;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
configuration.AddJsonFile("appsettings.json", true, true);
var service = builder.Services;
service.Configure<MongoDBSettings>(configuration.GetSection("MongoDatabase"));
service.AddSingleton<IMongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);
service.AddScoped<IMongoDBContext, MongoContext>();
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
//service.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false;
//        options.SaveToken = true;
//        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            IssuerSigningKey = new SymmetricSecurityKey(jwtSerect),
//            ValidateIssuerSigningKey = true,
//            ValidateIssuer = false,
//            ValidateAudience = false,
//        };
//    });
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthentication();
app.UseAuthorization();
app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
