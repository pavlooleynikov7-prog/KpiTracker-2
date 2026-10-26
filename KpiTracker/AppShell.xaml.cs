using KpiTracker.Navigation;
using KpiTracker.Views;

namespace KpiTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Маршрут другого екрана (не входить до візуальної ієрархії Shell,
        // відкривається лише через GoToAsync)
        Routing.RegisterRoute(Routes.KpiDetails, typeof(KpiDetailPage));
    }
}
