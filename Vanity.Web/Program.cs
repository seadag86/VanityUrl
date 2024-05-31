using Carter;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateSlimBuilder(args);

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

builder.Services.AddCarter();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapCarter();

app.Run();