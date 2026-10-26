namespace KpiTracker.Navigation;

/// <summary>Імена маршрутів Shell в одному місці, щоб уникнути помилок у рядках.</summary>
public static class Routes
{
    /// <summary>Екран деталей / редагування показника KPI. Параметр: id (Guid).</summary>
    public const string KpiDetails = "kpidetails";

    /// <summary>Ім'я query-параметра з ідентифікатором показника.</summary>
    public const string IdParameter = "id";
}
