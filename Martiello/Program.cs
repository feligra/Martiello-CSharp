using Martiello.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.RegisterUseCases();
WebApplication app = builder.Build();
app.ConfigureApp();
app.Run();
