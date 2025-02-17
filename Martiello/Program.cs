using Martiello.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
WebApplication app = builder.Build();
app.ConfigureApp();
app.Run();
