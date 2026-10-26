using KpiTracker.Navigation;
using KpiTracker.Services;
using KpiTracker.ViewModels;
using KpiTracker.Views;
using Microsoft.Extensions.Logging;

namespace KpiTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Services: одне сховище на весь час роботи застосунку
        builder.Services.AddSingleton<IKpiService, InMemoryKpiService>();
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

        // ViewModels
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<KpiDetailViewModel>();   // новий екземпляр на кожне відкриття деталей

        // Views (Shell створює сторінку через DI, тому MainViewModel
        // автоматично передається в конструктор MainPage)
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<KpiDetailPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
