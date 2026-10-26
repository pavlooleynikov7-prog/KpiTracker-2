namespace KpiTracker.Navigation;

/// <summary>
/// Обгортка над Shell-навігацією: ViewModel не звертається до Shell.Current напряму,
/// тому не залежить від UI і може бути протестована.
/// </summary>
public interface INavigationService
{
    Task GoToAsync(string route);
    Task GoBackAsync();
}
