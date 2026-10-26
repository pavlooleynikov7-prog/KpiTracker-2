namespace KpiTracker.Lifecycle;

// Повідомлення, якими App сповіщає ViewModel про перехід у фон / повернення,
// не знаючи нічого про конкретні ViewModel (WeakReferenceMessenger з CommunityToolkit.Mvvm).

/// <summary>Застосунок переходить у фон (App.OnSleep).</summary>
public sealed class AppSleepingMessage
{
}

/// <summary>Застосунок повернувся на передній план (App.OnResume).</summary>
public sealed class AppResumedMessage
{
}
