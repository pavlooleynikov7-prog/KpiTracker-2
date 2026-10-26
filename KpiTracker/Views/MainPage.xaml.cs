using KpiTracker.Lifecycle;
using KpiTracker.ViewModels;

namespace KpiTracker.Views;

public partial class MainPage : ContentPage
{
    // ViewModel надходить із контейнера DI (див. MauiProgram.cs)
    public MainPage(MainViewModel viewModel)
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
