using Api;
using Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Brainbox API",
        Description = "Brainbox Inc is a retail company based in Nigeria that offers a range of products to their customers. Brainbox wants to leverage on the internet buzz to make their products available online via their mobile app and web app to the customers.",
        Contact = new OpenApiContact
        {
            Name = "Brainbox Inc",
            Email = "jetmorgan.jm@gmail.com",
            Url = new Uri("https://brainboxapi.azurewebsites.net/"),
        },
    });
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(config =>
    {
        config.AllowAnyMethod();
        config.AllowAnyOrigin();
        config.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => 
{ 
    c.SwaggerEndpoint("swagger/v1/swagger.json", "BrainboxAPI v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.ConfigureExceptionHandler();

app.UseRouting();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();