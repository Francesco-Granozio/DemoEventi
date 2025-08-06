using DemoEventi.Application.Interfaces;
using DemoEventi.Application.Mapping;
using DemoEventi.Application.Services;
using DemoEventi.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;                // per AddAutoMapper(this IServiceCollection, params Assembly[])
using System.Reflection;         // per typeof().Assembly
using Microsoft.Extensions.Configuration;
using System.IO;


namespace DemoEventi.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.Configuration
   .SetBasePath(Directory.GetCurrentDirectory())   // o Directory.GetCurrentDirectory()
   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            .Services
            // Infrastructure
            .AddInfrastructure(builder.Configuration)

            .AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>())

            // MediatR 12+: registrazione via lambda
            .AddMediatR(cfg =>
            {
                // Scansiona l'assembly dove risiedono i tuoi handler/profili
                cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
            })
            .AddScoped<IUserService, UserService>()
            .AddScoped<IEventService, EventService>()

            // BlazorWebView
            .AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif





        return builder.Build();
    }
}

