using DemoEventi.Application.Interfaces;
using DemoEventi.Application.Services;
using DemoEventi.Application.Validators;
using DemoEventi.Application.Common.Behaviors;
using DemoEventi.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MediatR;

namespace DemoEventi.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Configuration
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)\\mssqllocaldb;Database=DemoEventiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
            })
            .Build();

        builder.Services.AddSingleton<IConfiguration>(configuration);

        // Infrastructure layer
        builder.Services.AddInfrastructure(configuration);

        // Application layer
        builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MauiProgram).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MauiProgram).Assembly));

        // FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();

        // MediatR Pipeline Behaviors
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Application services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IEventService, EventService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
