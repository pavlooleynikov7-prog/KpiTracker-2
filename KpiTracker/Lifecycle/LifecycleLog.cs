using System.Diagnostics;

namespace KpiTracker.Lifecycle;

/// <summary>
/// Діагностичний журнал подій життєвого циклу.
/// Debug.WriteLine — у вікно Debug Console (VS Code) / Output (Visual Studio);
/// на Android дублюється в logcat з тегом "Lifecycle": adb logcat -s Lifecycle
/// </summary>
public static class LifecycleLog
{
    public static void Write(string message)
    {
        var line = $"{DateTime.Now:HH:mm:ss.fff} {message}";
        Debug.WriteLine($"[Lifecycle] {line}");
#if ANDROID
        Android.Util.Log.Info("Lifecycle", line);
#endif
    }
}
