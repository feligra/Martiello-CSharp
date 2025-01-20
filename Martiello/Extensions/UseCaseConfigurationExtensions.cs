using Martiello.Domain.UseCase.Interface;

namespace Martiello.Extensions
{
    public static class UseCaseConfigurationExtensions
    {
        public static void RegisterUseCases(this IServiceCollection services)
        {
            List<Type> useCaseTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass &&
                           !t.IsAbstract &&
                           t.GetInterfaces().Any(i => i.IsGenericType &&
                                                    i.GetGenericTypeDefinition() == typeof(IUseCase<>)))
                .ToList();

            if (!useCaseTypes.Any())
            {
                Console.WriteLine("Nenhum caso de uso encontrado.");
                return;
            }

            foreach (Type? useCase in useCaseTypes)
            {
                IEnumerable<Type> interfaces = useCase.GetInterfaces()
                    .Where(i => i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IUseCase<>));

                foreach (Type? @interface in interfaces)
                {
                    services.AddScoped(@interface, useCase);
                }
            }
        }

    }
}
