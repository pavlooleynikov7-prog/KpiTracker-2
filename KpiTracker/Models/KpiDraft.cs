namespace KpiTracker.Models;

/// <summary>
/// Незавершене введення у формі нового показника — зберігається між сеансами.
/// </summary>
public record KpiDraft(string EmployeeName, string MetricName, string TargetValueText, string ActualValueText)
{
    public static KpiDraft Empty { get; } = new(string.Empty, string.Empty, string.Empty, string.Empty);

    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(EmployeeName) &&
        string.IsNullOrWhiteSpace(MetricName) &&
        string.IsNullOrWhiteSpace(TargetValueText) &&
        string.IsNullOrWhiteSpace(ActualValueText);
}
