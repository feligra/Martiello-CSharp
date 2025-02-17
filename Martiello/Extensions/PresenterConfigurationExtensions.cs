using System.Text.Json.Serialization;
using Martiello.Domain.UseCase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Martiello.Extensions
{
    public static class PresenterConfigurationExtensions
    {
        public static IServiceCollection AddPresenter(this IServiceCollection services, Action<PresenterOptions> configureOptions = null)
        {

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddOptions();
            services.AddScoped<IPresenter, Presenter>(); // Mudei para Scoped para manter consistência

            PresenterOptions options = new PresenterOptions();
            configureOptions?.Invoke(options);
            services.Configure<PresenterOptions>(o =>
            {
                o.WrapResult = options.WrapResult;
                o.ResultKeyDescription = options.ResultKeyDescription;
                o.ErrorKeyDescription = options.ErrorKeyDescription;
                o.ErrorCodeKeyDescription = options.ErrorCodeKeyDescription;
            });

            return services;
        }

        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, Action<IHealthCheck> healthChecks = null)
        {
            services.AddControllers().AddJsonOptions(delegate (JsonOptions options)
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            return services;
        }
    }
}
