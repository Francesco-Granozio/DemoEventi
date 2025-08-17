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
            // For Android emulator, use 10.0.2.2 which maps to host's localhost
            // This is the standard way for Android emulator to access host machine
            client.BaseAddress = new Uri("http://10.0.2.2:5163/");
            client.Timeout = TimeSpan.FromSeconds(15); // Reduced timeout for faster failure detection
#else
            client.BaseAddress = new Uri("https://localhost:7042/");
            client.Timeout = TimeSpan.FromSeconds(30);
#endif
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
#if DEBUG
                // Allow all certificates in debug mode
                return true;
#else
                return errors == System.Net.Security.SslPolicyErrors.None;
#endif
            }
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
