using CommunityToolkit.Mvvm.Messaging;
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

        // Сповіщаємо ViewModel: ОС може будь-якої миті завершити процес у фоні,
        // тож незбережений стан треба записати зараз
        WeakReferenceMessenger.Default.Send(new AppSleepingMessage());
    }

    // Застосунок повернувся на передній план
    protected override void OnResume()
    {
        LifecycleLog.Write("App: OnResume");
        WeakReferenceMessenger.Default.Send(new AppResumedMessage());
    }
}
