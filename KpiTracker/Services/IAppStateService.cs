using KpiTracker.Models;

namespace KpiTracker.Services;

/// <summary>
/// Легковагий стан застосунку, що переживає перезапуск (ключ-значення).
/// Дані предметної області сюди не потрапляють — для них буде SQLite у ЛР №3.
/// </summary>
public interface IAppStateService
{
    KpiDraft LoadDraft();
    void SaveDraft(KpiDraft draft);
    void ClearDraft();
}
