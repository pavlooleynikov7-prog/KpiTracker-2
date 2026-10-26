using KpiTracker.Models;

namespace KpiTracker.Services;

/// <summary>
/// Абстракція джерела даних KPI. Зараз — реалізація в пам'яті,
/// у ЛР №3 її буде замінено реалізацією з SQLite без змін у ViewModel.
/// </summary>
public interface IKpiService
{
    IReadOnlyList<KpiRecord> GetAll();
    KpiRecord? GetById(Guid id);
    void Add(KpiRecord record);
    bool Update(KpiRecord record);
    bool Remove(Guid id);
}
