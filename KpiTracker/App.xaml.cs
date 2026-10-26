using KpiTracker.Lifecycle;

namespace KpiTracker;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        LifecycleLog.Write("App: constructor");
    }

    // Створення головного вікна (з .NET 9 замість властивості MainPage)
    protected override Window CreateWindow(IActivationState? activationState)
    {
        LifecycleLog.Write("App: CreateWindow");
        return new Window(new AppShell());
    }

    // Застосунок запущено (вікно створене й активоване вперше)
    protected override void OnStart()
    {
        LifecycleLog.Write("App: OnStart");
    }

    // Застосунок переходить у фон (згорнуто, відкрито інший застосунок, вимкнено екран)
    protected override void OnSleep()
    {
        LifecycleLog.Write("App: OnSleep");
    }

    // Застосунок повернувся на передній план
    protected override void OnResume()
    {
        LifecycleLog.Write("App: OnResume");
    }
}
