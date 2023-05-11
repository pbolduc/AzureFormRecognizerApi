using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class FormRecognizerExtensions
{
    public static IServiceCollection AddFormRecognizer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FormRecognizerConfiguration>(cfg =>
        {
            configuration.GetSection("FormRecognizer").Bind(cfg);
        });

        services.AddTransient<IFormRecognizerClient, FormRecognizerClientWrapper>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(FormRecognizerExtensions).Assembly);
        });

        return services;
    }
}
