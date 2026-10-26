using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KpiTracker.Helpers;
using KpiTracker.Lifecycle;
using KpiTracker.Models;
using KpiTracker.Navigation;
using KpiTracker.Services;

namespace KpiTracker.ViewModels;

/// <summary>
/// ViewModel другого екрана: деталі та редагування обраного показника KPI.
/// Отримує ідентифікатор показника через query-параметр навігації (IQueryAttributable).
/// </summary>
public partial class KpiDetailViewModel : ObservableObject, IQueryAttributable, ILifecycleAware
{
    private readonly IKpiService _kpiService;
    private readonly INavigationService _navigation;
    private Guid _recordId;

    public KpiDetailViewModel(IKpiService kpiService, INavigationService navigation)
    {
        _kpiService = kpiService;
        _navigation = navigation;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private bool _isLoaded;

    [ObservableProperty]
    private string _periodText = string.Empty;

    [ObservableProperty]
    private string _employeeName = string.Empty;

    [ObservableProperty]
    private string _metricName = string.Empty;

    [ObservableProperty]
    private string _targetValueText = string.Empty;

    [ObservableProperty]
    private string _actualValueText = string.Empty;

    // Попередній розрахунок виконання, що оновлюється під час редагування
    [ObservableProperty]
    private double _completionPreview;

    [ObservableProperty]
    private double _progressPreview;

    [ObservableProperty]
    private KpiStatus _statusPreview;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Викликається Shell перед показом сторінки з параметрами з рядка GoToAsync("kpidetails?id=...").
    /// </summary>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        LifecycleLog.Write($"KpiDetailViewModel: ApplyQueryAttributes ({string.Join(", ", query.Select(q => $"{q.Key}={q.Value}"))})");

        if (query.TryGetValue(Routes.IdParameter, out var value) &&
            Guid.TryParse(value?.ToString(), out var id))
        {
            Load(id);
        }
        else
        {
            IsLoaded = false;
            ErrorMessage = "Не передано ідентифікатор показника";
        }
    }

    private void Load(Guid id)
    {
        var record = _kpiService.GetById(id);
        if (record is null)
        {
            IsLoaded = false;
            ErrorMessage = "Показник не знайдено (можливо, його видалено)";
            return;
        }

        _recordId = record.Id;
        EmployeeName = record.EmployeeName;
        MetricName = record.MetricName;
        TargetValueText = record.TargetValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        ActualValueText = record.ActualValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
        PeriodText = record.Period.ToString("MM.yyyy");
        ErrorMessage = string.Empty;
        IsLoaded = true;
    }

    // ---------- Життєвий цикл (ILifecycleAware) ----------

    public void OnAppearing()
    {
        LifecycleLog.Write($"KpiDetailViewModel: OnAppearing (завантажено: {IsLoaded})");
    }

    public void OnDisappearing()
    {
        LifecycleLog.Write("KpiDetailViewModel: OnDisappearing");
    }

    partial void OnTargetValueTextChanged(string value) => UpdatePreview();
    partial void OnActualValueTextChanged(string value) => UpdatePreview();

    private void UpdatePreview()
    {
        ErrorMessage = string.Empty;
        NumberParser.TryParseNonNegative(TargetValueText, out var target);
        NumberParser.TryParseNonNegative(ActualValueText, out var actual);

        // Правила розрахунку беремо з моделі, щоб не дублювати їх у ViewModel
        var preview = new KpiRecord { TargetValue = target, ActualValue = actual };
        CompletionPreview = preview.CompletionPercent;
        ProgressPreview = preview.Progress;
        StatusPreview = preview.Status;
    }

    /// <summary>Зберігає зміни і повертається до списку.</summary>
    [RelayCommand(CanExecute = nameof(IsLoaded))]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(EmployeeName) || string.IsNullOrWhiteSpace(MetricName))
        {
            ErrorMessage = "Заповніть співробітника і назву показника";
            return;
        }

        if (!NumberParser.TryParseNonNegative(TargetValueText, out var target) || target == 0)
        {
            ErrorMessage = "План має бути додатним числом";
            return;
        }

        if (!NumberParser.TryParseNonNegative(ActualValueText, out var actual))
        {
            ErrorMessage = "Факт має бути невід'ємним числом";
            return;
        }

        _kpiService.Update(new KpiRecord
        {
            Id = _recordId,
            EmployeeName = EmployeeName.Trim(),
            MetricName = MetricName.Trim(),
            TargetValue = target,
            ActualValue = actual
        });

        await _navigation.GoBackAsync();
    }

    [RelayCommand]
    private Task CancelAsync() => _navigation.GoBackAsync();
}
