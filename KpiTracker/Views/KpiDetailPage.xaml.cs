using KpiTracker.Lifecycle;
using KpiTracker.ViewModels;

namespace KpiTracker.Views;

public partial class KpiDetailPage : ContentPage
{
    public KpiDetailPage(KpiDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Code-behind лише делегує події ViewModel — без бізнес-логіки
    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ILifecycleAware)?.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as ILifecycleAware)?.OnDisappearing();
    }
}
