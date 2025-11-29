using App.API.Extensions;
using App.Application;
using App.Persistence.Extensions;
using App.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllerExtensions();

builder.Services.AddSwaggerExtensions();

builder.Services.AddExceptionHandlerExtensions();

builder.Services.AddCachingExtensions();

builder.Services.AddServiceBusExtensions(builder.Configuration);

builder.Services.AddRepositories().AddServices();

var app = builder.Build();

app.ConfigurePipeline(app.Environment);

app.MapControllers();

app.Run();