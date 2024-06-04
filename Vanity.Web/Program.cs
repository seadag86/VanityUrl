using Carter;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Vanity.Web.Data;
using Vanity.Web.Features.Urls;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Vanity URL API",
        Description = "An ASP.NET Core Web API for beautifying and shortening URLs",
        Contact = new OpenApiContact
        {
            Name = "Petrichor Digital Solutions",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "GNU - General Public License",
            Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html")
        }
    });
});

builder.Services.AddOutputCache(options =>
{
    // Set the default cache duration to 10 minutes
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(15);
    options.AddBasePolicy(x => x.With(c => c.HttpContext.Request.Query["nocache"] == "true").NoCache());
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddCarter();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<UrlService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.MapCarter();

app.UseOutputCache();

app.Run();

// Required for API functional tests
public partial class Program { }