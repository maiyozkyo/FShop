using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Add OcelotConfig
builder.Configuration.AddJsonFile("ocelotConfig.json", false, true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.MapControllers();
await app.UseOcelot();
app.Run();
