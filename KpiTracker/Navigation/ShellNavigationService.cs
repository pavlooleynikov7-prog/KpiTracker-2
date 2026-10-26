namespace KpiTracker.Navigation;

public class ShellNavigationService : INavigationService
{
    public Task GoToAsync(string route) => Shell.Current.GoToAsync(route);

    // ".." — повернення на попередню сторінку стеку навігації
    public Task GoBackAsync() => Shell.Current.GoToAsync("..");
}
