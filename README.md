# KPI Tracker — трекер виконання KPI співробітників відділу

Лабораторні роботи №1–2 з дисципліни «Технології розробки мобільних додатків на C#».
Варіант 8: **Трекер виконання KPI співробітників відділу**.

Кросплатформний застосунок на **.NET MAUI (.NET 10)**, побудований за патерном **MVVM**
з використанням **CommunityToolkit.Mvvm**. Дані зберігаються в пам'яті
(збереження в БД з'явиться в ЛР №3).

## Можливості

- список показників KPI (співробітник, показник, план, факт, % виконання, статус);
- кольорова індикація: ≥ 100 % — виконано, 70–99 % — під загрозою, < 70 % — не виконано;
- форма додавання показника з валідацією (кнопка неактивна, доки поля не заповнені);
- видалення показника, очищення форми;
- підсумки по відділу: кількість показників, кількість виконаних, середній % виконання;
- **(ЛР №2)** екран деталей / редагування показника з попереднім розрахунком % виконання;
- **(ЛР №2)** чернетка форми нового показника зберігається між сеансами (Preferences API).

## Екрани та навігація (ЛР №2)

| Екран | View / ViewModel | Маршрут |
|---|---|---|
| Список KPI і форма додавання | `MainPage` / `MainViewModel` | `//MainPage` (корінь Shell) |
| Деталі / редагування показника | `KpiDetailPage` / `KpiDetailViewModel` | `kpidetails` |

```
MainPage ──(натискання на картку: OpenDetailsCommand)──► kpidetails?id=<Guid>
   ▲                                                         │
   └──────────────(«Зберегти» / «Скасувати»: GoToAsync(".."))─┘
```

- Маршрут реєструється в `AppShell.xaml.cs`: `Routing.RegisterRoute(Routes.KpiDetails, typeof(KpiDetailPage))`.
- `MainViewModel` викликає `INavigationService.GoToAsync("kpidetails?id=...")` — ViewModel не залежить від `Shell.Current`.
- `KpiDetailViewModel` реалізує `IQueryAttributable`: у `ApplyQueryAttributes` зчитує `id` і завантажує показник із `IKpiService`.
- Після збереження `MainViewModel.OnAppearing` перечитує список, тож зміни одразу видно.

## Життєвий цикл і стан (ЛР №2)

- `Lifecycle/ILifecycleAware` — контракт `OnAppearing()` / `OnDisappearing()`, який реалізують обидві ViewModel;
  code-behind сторінок лише делегує виклики.
- `App.xaml.cs` журналює `CreateWindow`, `OnStart`, `OnSleep`, `OnResume`
  (Debug Console або `adb logcat -s Lifecycle`) і через `WeakReferenceMessenger` сповіщає ViewModel.
- Чернетка форми (`KpiDraft`) зберігається через `IAppStateService` → `Preferences` в `App.OnSleep` і
  `MainViewModel.OnDisappearing`, а відновлюється в конструкторі `MainViewModel` при запуску.

## Структура

```
KpiTracker/
├── Models/          KpiRecord, KpiStatus, KpiDraft
├── ViewModels/      MainViewModel, KpiDetailViewModel
├── Views/           MainPage, KpiDetailPage
├── Services/        IKpiService, InMemoryKpiService, IAppStateService, PreferencesAppStateService
├── Navigation/      Routes, INavigationService, ShellNavigationService
├── Lifecycle/       ILifecycleAware, LifecycleLog, повідомлення App → ViewModel
├── Helpers/         NumberParser, конвертери статусу у колір/текст
├── Resources/       стилі, кольори, іконка, splash
├── Platforms/       Android, iOS, MacCatalyst, Windows
├── MauiProgram.cs   реєстрація Service, ViewModel і View у DI
└── AppShell.xaml    навігація (Shell)
```

## Запуск

Потрібно: .NET 10 SDK, робоче навантаження MAUI, Android SDK / емулятор.

```bash
dotnet workload install maui
dotnet build KpiTracker/KpiTracker.csproj -f net10.0-android
dotnet build KpiTracker/KpiTracker.csproj -t:Run -f net10.0-android   # запуск на емуляторі
adb logcat -s Lifecycle                                                # журнал подій життєвого циклу
```

Або відкрийте папку у VS Code з розширеннями C# Dev Kit і .NET MAUI (чи `KpiTracker.sln` у Visual Studio 2026) і запустіть на Android Emulator.

## Журнал взаємодії з LLM

Див. [PROMPTS.md](PROMPTS.md).
