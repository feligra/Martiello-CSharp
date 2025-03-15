using System.Reflection;
using Martiello.Application.Services;
using Martiello.Domain.Interface.Repository;
using Martiello.Domain.Interface.Service;
using Martiello.Infrastructure.Data;
using Martiello.Infrastructure.Repository;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Martiello.Extensions
{
    public static class ConfigureServiceExtensions
    {
        public static void ConfigureApp(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthorization();
            app.MapControllers();
        }
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebApi(configuration);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(ConfigureSwagger);
            services.AddPresenter(o =>
            {
                o.WrapResult = true;
                o.ResultKeyDescription = "Resposta";
                o.ErrorKeyDescription = "Erros";
                o.ErrorCodeKeyDescription = "Codigo";
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Database
            services.AddSingleton(sp => new DbContext(
                configuration.GetConnectionString("MongoDb"),
                configuration.GetValue<string>("DatabaseName")
            ));

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // Services
            services.AddScoped<IMercadoPagoService, MercadoPagoService>();
            services.AddScoped<OrderStatusUpdaterService>();
            services.AddHostedService<OrderStatusBackgroundService>();
        }
        private static void ConfigureSwagger(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Martiello", Version = "v1" });
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        }

    }
}
