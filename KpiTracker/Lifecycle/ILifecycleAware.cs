namespace KpiTracker.Lifecycle;

/// <summary>
/// Контракт життєвого циклу для ViewModel. Сторінка в code-behind лише
/// делегує свої події OnAppearing/OnDisappearing однойменним методам ViewModel,
/// тож уся логіка ініціалізації та звільнення ресурсів живе у ViewModel.
/// </summary>
public interface ILifecycleAware
{
    /// <summary>Сторінка ось-ось стане видимою.</summary>
    void OnAppearing();

    /// <summary>Сторінка перестає бути видимою (перехід на інший екран).</summary>
    void OnDisappearing();
}
