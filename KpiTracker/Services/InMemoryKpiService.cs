using KpiTracker.Models;

namespace KpiTracker.Services;

/// <summary>
/// Зберігає показники KPI лише в оперативній пам'яті (до перезапуску застосунку).
/// </summary>
public class InMemoryKpiService : IKpiService
{
    private readonly List<KpiRecord> _records;

    public InMemoryKpiService()
    {
        var period = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        // Початкові демонстраційні дані
        _records =
        [
            new KpiRecord { EmployeeName = "Ковальчук Олена", MetricName = "Закриті угоди", TargetValue = 20, ActualValue = 23, Period = period },
            new KpiRecord { EmployeeName = "Шевченко Андрій", MetricName = "Дзвінки клієнтам", TargetValue = 300, ActualValue = 240, Period = period },
            new KpiRecord { EmployeeName = "Мельник Ірина", MetricName = "Обсяг продажів, тис. грн", TargetValue = 500, ActualValue = 310, Period = period }
        ];
    }

    public IReadOnlyList<KpiRecord> GetAll() => _records.AsReadOnly();

    public KpiRecord? GetById(Guid id) => _records.FirstOrDefault(r => r.Id == id);

    public void Add(KpiRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        _records.Add(record);
    }

    /// <summary>Оновлює редаговані поля існуючого показника (за Id).</summary>
    public bool Update(KpiRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        var existing = GetById(record.Id);
        if (existing is null)
            return false;

        existing.EmployeeName = record.EmployeeName;
        existing.MetricName = record.MetricName;
        existing.TargetValue = record.TargetValue;
        existing.ActualValue = record.ActualValue;
        return true;
    }

    public bool Remove(Guid id) => _records.RemoveAll(r => r.Id == id) > 0;
}
