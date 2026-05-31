# CodeFactory.WinVs.Wpf

[![NuGet](https://img.shields.io/nuget/v/CodeFactory.WinVs.Wpf.svg)](https://www.nuget.org/packages/CodeFactory.WinVs.Wpf)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

The **CodeFactory User Interface WPF for Visual Studio - Windows** library provides custom WPF dialog screens and user controls hosted inside Visual Studio for Windows. It is used alongside CodeFactory automation commands to present interactive UI to the developer at automation runtime.

---

## Requirements

| Requirement | Value |
|---|---|
| Target Framework | .NET Framework 4.8 |
| Visual Studio | Visual Studio 2022 / Visual Studio 2026 (Windows) |
| CodeFactory Runtime | [CodeFactory for Windows](https://marketplace.visualstudio.com/items?itemName=CodeFactoryLLC.CodeFactoryForWindows) ≥ 2.26151.0.1 |
| Command Library Project | .NET Framework 4.8 |

> ⚠️ **Important:** When updating to a new version of this SDK you must **recompile** your automation (command library) projects and ensure the CodeFactory runtime installed in Visual Studio is at the same version or higher.

---

## Installation

Install via the NuGet Package Manager, Package Manager Console, or the .NET CLI:
```
dotnet add package CodeFactory.WinVs.Wpf
```


Or in your `.csproj`:
```xml
<PackageReference Include="CodeFactory.WinVs.Wpf" Version="2.26151.0.1-PreRelease" />
```


---

## Overview

`CodeFactory.WinVs.Wpf` is part of the **CodeFactory SDK for Visual Studio for Windows** (version 2.0). It provides the WPF UI layer for CodeFactory automation and includes:

- **`ViewUserControl`** — A base WPF `UserControl` class extended with direct access to Visual Studio actions (`IVsActions`) and a structured logger (`ILogger`). Inherit from this class to build custom dialog content for your automation.
- **Dialog Window Hosting** — Views are hosted in a thread-safe dialog window managed by CodeFactory via `IVsUIActions.ShowDialogWindowAsync`.
- **Document Panel Hosting** — Views can also be embedded in a Visual Studio document panel via `IVsUIActions.ShowDocumentPanelAsync`.
- **Host Window Integration** — The `ViewUserControl` base class automatically subscribes to and responds to hosting window lifecycle events (`Activated`, `Closing`).

---

## Key Concepts

### `ViewUserControl`

`ViewUserControl` is the single public class in the `CodeFactory.WinVs.Wpf` namespace. Inherit from it to create a custom WPF user control that participates in the CodeFactory UI hosting pipeline.

Key members:

| Member | Description |
|---|---|
| `Title` | A dependency property for the title shown in the hosting dialog window. |
| `_visualStudioActions` | Provides access to all Visual Studio automation actions at runtime. |
| `_logger` | Structured logger scoped to this user control. |
| `Close()` | Raises the `CloseHost` event to signal the hosting window to close. |
| `CloseHost` | Event raised when the user control requests its hosting window to close. |
| `WindowActivated` | Virtual — override to react when the hosting window is activated. |
| `WindowClosing` | Virtual — override to react when the hosting window is closing. |

### Launching a Dialog from a Command

Use `IVsUIActions` (available on every CodeFactory command via `_visualStudioActions`) to create and display your view:


5. Launch the dialog from within a CodeFactory command using `CreateViewAsync<T>` and `ShowDialogWindowAsync`.

---

## Related Packages

| Package | Description |
|---|---|
| [`CodeFactory`](https://www.nuget.org/packages/CodeFactory) | Core contracts shared across all CodeFactory IDE targets (.NET Standard 2.0) |
| [`CodeFactory.WinVs`](https://www.nuget.org/packages/CodeFactory.WinVs) | Core SDK for building CodeFactory automation for Visual Studio for Windows |

---

## Release Notes

### 2.26151.0.1-PreRelease

- **Recompile Required:** Automation projects must be recompiled against this version of the SDK.
- Aligned with `CodeFactory` and `CodeFactory.WinVs` package version `2.26151.0.1-PreRelease`.

---

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).  
Copyright © 2026 CodeFactory, LLC.

---

## Links

- 🏠 [GitHub Repository](https://github.com/CodeFactoryLLC/CodeFactoryForWindows)
- 🛒 [Visual Studio Marketplace — CodeFactory for Windows](https://marketplace.visualstudio.com/items?itemName=CodeFactoryLLC.CodeFactoryForWindows)
- 📦 [NuGet — CodeFactory.WinVs](https://www.nuget.org/packages/CodeFactory.WinVs)
- 📦 [NuGet — CodeFactory](https://www.nuget.org/packages/CodeFactory)