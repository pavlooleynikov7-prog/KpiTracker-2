using KpiTracker.Models;
using Microsoft.Maui.Storage;

namespace KpiTracker.Services;

/// <summary>
/// Реалізація IAppStateService на Preferences API
/// (Android — SharedPreferences, iOS — NSUserDefaults, Windows — ApplicationData).
/// </summary>
public class PreferencesAppStateService : IAppStateService
{
    private const string EmployeeKey = "draft_employee";
    private const string MetricKey = "draft_metric";
    private const string TargetKey = "draft_target";
    private const string ActualKey = "draft_actual";

    private readonly IPreferences _preferences;

    public PreferencesAppStateService(IPreferences preferences)
    {
        _preferences = preferences;
    }

    public KpiDraft LoadDraft() => new(
        _preferences.Get(EmployeeKey, string.Empty),
        _preferences.Get(MetricKey, string.Empty),
        _preferences.Get(TargetKey, string.Empty),
        _preferences.Get(ActualKey, string.Empty));

    public void SaveDraft(KpiDraft draft)
    {
        if (draft.IsEmpty)
        {
            ClearDraft();
            return;
        }

        _preferences.Set(EmployeeKey, draft.EmployeeName);
        _preferences.Set(MetricKey, draft.MetricName);
        _preferences.Set(TargetKey, draft.TargetValueText);
        _preferences.Set(ActualKey, draft.ActualValueText);
    }

    public void ClearDraft()
    {
        _preferences.Remove(EmployeeKey);
        _preferences.Remove(MetricKey);
        _preferences.Remove(TargetKey);
        _preferences.Remove(ActualKey);
    }
}
