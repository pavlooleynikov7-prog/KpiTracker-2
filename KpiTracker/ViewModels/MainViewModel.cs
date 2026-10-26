using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KpiTracker.Helpers;
using KpiTracker.Lifecycle;
using KpiTracker.Models;
using KpiTracker.Navigation;
using KpiTracker.Services;

namespace KpiTracker.ViewModels;

/// <summary>
/// ViewModel головного екрана: список показників KPI, форма введення та підсумки.
/// </summary>
public partial class MainViewModel : ObservableObject, ILifecycleAware
{
    private readonly IKpiService _kpiService;
    private readonly INavigationService _navigation;

    public MainViewModel(IKpiService kpiService, INavigationService navigation)
    {
        _kpiService = kpiService;
        _navigation = navigation;
        Records = new ObservableCollection<KpiRecord>(_kpiService.GetAll());
        Records.CollectionChanged += (_, _) => UpdateSummary();
        UpdateSummary();
    }

    /// <summary>Колекція показників, до якої прив'язано CollectionView.</summary>
    public ObservableCollection<KpiRecord> Records { get; }

    // ---------- Поля форми введення ----------

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddKpiCommand))]
    private string _employeeName = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddKpiCommand))]
    private string _metricName = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddKpiCommand))]
    private string _targetValueText = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddKpiCommand))]
    private string _actualValueText = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    // Після виправлення числових полів повідомлення про помилку зникає
    partial void OnTargetValueTextChanged(string value) => ErrorMessage = string.Empty;
    partial void OnActualValueTextChanged(string value) => ErrorMessage = string.Empty;

    // ---------- Підсумкові показники відділу ----------

    [ObservableProperty]
    private int _totalCount;

    [ObservableProperty]
    private int _achievedCount;

    [ObservableProperty]
    private double _averageCompletion;

    [ObservableProperty]
    private string _statusMessage = "Заповніть форму, щоб додати показник";

    // ---------- Команди ----------

    /// <summary>Додає новий показник KPI до колекції.</summary>
    [RelayCommand(CanExecute = nameof(CanAddKpi))]
    private void AddKpi()
    {
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

        var record = new KpiRecord
        {
            EmployeeName = EmployeeName.Trim(),
            MetricName = MetricName.Trim(),
            TargetValue = target,
            ActualValue = actual
        };

        _kpiService.Add(record);
        Records.Insert(0, record);

        StatusMessage = $"Додано: {record.EmployeeName} — {record.MetricName} ({record.CompletionPercent}%)";
        ClearForm();
    }

    private bool CanAddKpi() =>
        !string.IsNullOrWhiteSpace(EmployeeName) &&
        !string.IsNullOrWhiteSpace(MetricName) &&
        !string.IsNullOrWhiteSpace(TargetValueText) &&
        !string.IsNullOrWhiteSpace(ActualValueText);

    /// <summary>Видаляє показник зі списку.</summary>
    [RelayCommand]
    private void DeleteKpi(KpiRecord? record)
    {
        if (record is null)
            return;

        if (_kpiService.Remove(record.Id))
        {
            Records.Remove(record);
            StatusMessage = $"Видалено: {record.EmployeeName} — {record.MetricName}";
        }
    }

    /// <summary>Відкриває екран деталей обраного показника, передаючи його Id у query-параметрі.</summary>
    [RelayCommand]
    private async Task OpenDetailsAsync(KpiRecord? record)
    {
        if (record is null)
            return;

        await _navigation.GoToAsync($"{Routes.KpiDetails}?{Routes.IdParameter}={record.Id}");
    }

    // ---------- Життєвий цикл (ILifecycleAware) ----------

    public void OnAppearing()
    {
        LifecycleLog.Write("MainViewModel: OnAppearing");

        // Після повернення з екрана деталей показник міг змінитися — оновлюємо список і підсумки
        ReloadRecords();
    }

    public void OnDisappearing()
    {
        LifecycleLog.Write("MainViewModel: OnDisappearing");
    }

    /// <summary>Перечитує список із сервісу (після редагування на екрані деталей).</summary>
    private void ReloadRecords()
    {
        Records.Clear();
        foreach (var record in _kpiService.GetAll())
            Records.Add(record);
    }

    /// <summary>Очищує поля форми.</summary>
    [RelayCommand]
    private void ClearForm()
    {
        EmployeeName = string.Empty;
        MetricName = string.Empty;
        TargetValueText = string.Empty;
        ActualValueText = string.Empty;
        ErrorMessage = string.Empty;
    }

    private void UpdateSummary()
    {
        TotalCount = Records.Count;
        AchievedCount = Records.Count(r => r.Status == KpiStatus.Achieved);
        AverageCompletion = Records.Count == 0
            ? 0
            : Math.Round(Records.Average(r => r.CompletionPercent), 1);
    }
}
