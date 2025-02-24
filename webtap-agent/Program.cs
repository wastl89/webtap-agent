using webtap_agent.Services;
using webtap_agent.Middlewares;
using OpenTap;

await PluginManager.SearchAsync();
SessionLogs.Initialize();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ErrorHandlingMiddleware>();

// Add services to the container.
builder.Services.AddHostedService<LogService>();
builder.Services.AddSingleton<TestplanService>(provider =>
{
    var config = provider.GetService<IConfiguration>();
    Console.WriteLine(config.ToString());
    var basePath = config["Testplans:BasePath"];
    return new TestplanService(basePath);
});
builder.Services.AddSingleton<TestRunService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthorization();
app.UseWebSockets();
app.MapControllers();

app.Run();
