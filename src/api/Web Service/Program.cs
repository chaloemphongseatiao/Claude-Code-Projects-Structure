using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

using TemplateWebService.Data;
using TemplateWebService.Extensions;
using TemplateWebService.Models.Settings;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load configuration files based on environment
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add Forwarded Headers Middleware
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownProxies.Add(IPAddress.Parse("10.0.0.10"));
});

// Register strongly typed config
builder.Services.Configure<WebServiceSettings>(
    builder.Configuration.GetSection("WebServiceSettings"));

var webSettings = builder.Configuration
    .GetSection("WebServiceSettings")
    .Get<WebServiceSettings>()
    ?? throw new InvalidOperationException("Missing `WebServiceSettings`.");

// Register DbContexts
builder.Services.AddDbContext<LomaLottoContext>(options =>
{
    options.UseSqlServer(webSettings.Database.LOMA_LOTTO.ConnectionString);
    // เน€เธเธดเธ”เน€เธกเธทเนเธญเธ”เธตเธเธฑเธ
    // options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// เนเธเน DbContextFactory เธชเธณเธซเธฃเธฑเธเธเธฒเธฃเธชเธฃเนเธฒเธ DbContext เนเธ BackgroundService เธซเธฃเธทเธญเธ—เธตเนเนเธกเนเธชเธฒเธกเธฒเธฃเธ–เนเธเน DI เนเธเธเธเธเธ•เธดเนเธ”เน
builder.Services.AddDbContextFactory<LomaLottoContext>(
    options =>
    {
        options.UseSqlServer(webSettings.Database.LOMA_LOTTO.ConnectionString);
    },
    ServiceLifetime.Scoped);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200",
                "http://ga-next.com",
                "https://ga-next.com",
                "http://www.ga-next.com",
                "https://www.ga-next.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition");
    });
});

// Add Controllers (API JSON Config)
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        // options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add SignalR (Real-time Config)
builder.Services
    .AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.PayloadSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });

// Add HttpClient
builder.Services.AddHttpClient();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build app
var app = builder.Build();

// Development Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sale Smart API Documentation v1");
        c.RoutePrefix = string.Empty;
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

// Static Files (File Server)
app.UseConfiguredStaticFiles(
    builder.Configuration,
    app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("StaticFiles"));

// Forwarded Headers Middleware
app.UseForwardedHeaders();

// Middlewares (CORS, Auth, etc)
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Endpoint mappings
app.MapControllers();

// Run app
app.Run();