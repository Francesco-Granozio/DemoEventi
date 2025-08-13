using Microsoft.Extensions.Logging;

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
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register HTTP Client
        builder.Services.AddHttpClient<Services.IApiService, Services.ApiService>(client =>
        {
#if DEBUG
            // For Android emulator, use 10.0.2.2 to access host machine
            // For physical devices, use your machine's actual IP address
            client.BaseAddress = new Uri("https://10.0.2.2:7042/");
#else
            client.BaseAddress = new Uri("https://localhost:7042/");
#endif
        });

        // Register ViewModels
        builder.Services.AddTransient<ViewModels.HomeViewModel>();
        builder.Services.AddTransient<ViewModels.UsersViewModel>();
        builder.Services.AddTransient<ViewModels.EventsViewModel>();
        builder.Services.AddTransient<ViewModels.InterestsViewModel>();
        builder.Services.AddTransient<ViewModels.UserFormViewModel>();
        builder.Services.AddTransient<ViewModels.EventFormViewModel>();

        // Register Views
        builder.Services.AddTransient<Views.HomePage>();
        builder.Services.AddTransient<Views.UsersPage>();
        builder.Services.AddTransient<Views.EventsPage>();
        builder.Services.AddTransient<Views.InterestsPage>();
        builder.Services.AddTransient<Views.UserFormPage>();
        builder.Services.AddTransient<Views.EventFormPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
