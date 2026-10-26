using KpiTracker.ViewModels;

namespace KpiTracker.Views;

public partial class KpiDetailPage : ContentPage
{
    public KpiDetailPage(KpiDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
