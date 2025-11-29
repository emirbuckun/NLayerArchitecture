using App.API.Extensions;
using App.Application;
using App.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllerExtensions();

builder.Services.AddSwaggerExtensions();

builder.Services.AddExceptionHandlerExtensions();

builder.Services.AddCachingExtensions();

builder.Services.AddRepositories().AddServices();

var app = builder.Build();

app.ConfigurePipeline(app.Environment);

app.MapControllers();

app.Run();