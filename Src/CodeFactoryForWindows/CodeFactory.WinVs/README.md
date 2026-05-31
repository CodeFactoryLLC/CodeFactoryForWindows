# CodeFactory.WinVs

[![NuGet](https://img.shields.io/nuget/v/CodeFactory.WinVs.svg)](https://www.nuget.org/packages/CodeFactory.WinVs)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

The **CodeFactory API for Visual Studio - Windows** is the core SDK library for building CodeFactory automation that runs inside Visual Studio for Windows. It provides the full set of APIs, models, commands, and utilities needed to author automation that interacts with the Visual Studio IDE, solution, and project system.

---

## Requirements

| Requirement | Value |
|---|---|
| Target Framework | .NET Standard 2.0 |
| Visual Studio | Visual Studio 2022 / Visual Studio 2026 (Windows) |
| CodeFactory Runtime | [CodeFactory for Windows](https://marketplace.visualstudio.com/items?itemName=CodeFactoryLLC.CodeFactoryForWindows) ≥ 2.26151.0.1 |
| Command Library Project | .NET Framework 4.8 |

> ⚠️ **Important:** When updating to a new version of this SDK you must **recompile** your automation (command library) projects and ensure the CodeFactory runtime installed in Visual Studio is at the same version or higher.

---

## Installation

Install via the NuGet Package Manager, Package Manager Console, or the .NET CLI:
```
dotnet add package CodeFactory.WinVs
```

Or in your `.csproj`:
```xml
<PackageReference Include="CodeFactory.WinVs" Version="2.26151.0.1-PreRelease" />
```


---

## Overview

`CodeFactory.WinVs` is part of the **CodeFactory SDK for Visual Studio for Windows** (version 2.0). It is the primary library for automation authors and provides:

- **Automation Commands** — Solution Explorer and IDE command hooks surfaced in Visual Studio.
- **Project System Models** — Strongly-typed models representing solutions, projects, folders, and source files.
- **C# Code Models** — A rich object model for inspecting and generating C# source code.
- **External Configuration** — A flexible configuration system (`ExternalConfig`) for parameterizing commands without code changes.
- **Transaction History** — Tracking of all file write operations performed by automation, including file name, project, path, character count, and line count statistics.
- **Source Formatting** — A `SourceFormatter` API for generating well-formatted source code (replaces T4 templates).
- **Logging** — Built-in structured logging via Serilog and Microsoft.Extensions.Logging abstractions.

---

## Key Concepts

### Automation Commands

Commands are the building blocks of CodeFactory automation. Two types are available:

- **Solution Explorer Commands** — Appear on context menus in Solution Explorer (e.g., on a project, folder, or file node).
- **IDE Commands** — Appear as top-level Visual Studio menu or toolbar actions.

Commands are authored in a **.NET Framework 4.8** Command Library project using the **CodeFactory for Windows - Command Library** project template.

### External Configuration (`ExternalConfig`)

The `ExternalConfig` class replaces the deprecated `CommandManager` for all command configuration. Configuration parameters (`ConfigParameter`) support the following value types:

- `string`
- `bool`
- `DateTime`
- `List`

Configuration is managed through the **External Configuration Editor**, accessible by right-clicking the Solution node in Solution Explorer and selecting **[cF] Automation Configuration**.

### Transaction History

CodeFactory tracks statistics for every file written by automation:

| Statistic | Description |
|---|---|
| File Name | Name of the file updated |
| Project Name | Project the file belongs to (`null` if solution-level) |
| Solution Name | Solution the file resides in |
| Relative Path | Path from the project or solution root |
| Characters Written | Non-whitespace, non-line-return characters written |
| Lines Written | Lines with content written |

Transaction results are returned from any insert or replace operation.

---

## Getting Started

1. Install the [CodeFactory for Windows](https://marketplace.visualstudio.com/items?itemName=CodeFactoryLLC.CodeFactoryForWindows) Visual Studio extension.
2. Create a new **CodeFactory for Windows - Command Library** project (.NET Framework 4.8).
3. Add this NuGet package to your command library project.
4. Add a new command using the **CodeFactoryWindows** item templates (**Add > New Item > CodeFactoryWindows**).
5. Implement your automation logic inside the generated command class.
6. Build and package your automation using the **CodeFactory.Packager.WinVs** utility (called automatically on build).

---

## Related Packages

| Package | Description |
|---|---|
| [`CodeFactory`](https://www.nuget.org/packages/CodeFactory) | Core contracts shared across all CodeFactory IDE targets (.NET Standard 2.0) |
| [`CodeFactory.WinVs.Wpf`](https://www.nuget.org/packages/CodeFactory.WinVs.Wpf) | WPF UI controls for Visual Studio for Windows (.NET Framework 4.8) |

---

## 📄 Release Notes

### 2.26151.0.1-PreRelease
*Released: May 2026 — Branch: 2026-March*

#### 🆕 New

- **Visual Studio 2026 support** — This release targets the Visual Studio 2026 (v18.6) runtime. The CodeFactory runtime must be at version `2.26151.0.1` or higher to load automation built against this SDK.
- **C# 13 language support** — The `CodeFactory` base library now compiles with `LangVersion 13`. C# 13 language features are available in automation projects via the [PolySharp](https://github.com/Sergio0694/PolySharp) polyfill package, which backfills required compiler attributes onto .NET Standard 2.0.

#### 🔧 Changes

- **Updated SDK version range** — The supported SDK version window has been updated:
  - Minimum supported SDK version: `2.24224.0.1`
  - Maximum supported SDK version: `2.26151.0.1`
- **Copyright year updated** — All library copyright notices updated to reflect 2026.

#### ⚠️ Breaking Changes / Recompile Required

> **Action Required:** You must recompile your automation projects against this version of the SDK before deploying to a Visual Studio 2026 environment.
>
> Ensure the CodeFactory runtime installed in Visual Studio is at least version `2.26151.0.1`.

---

### 2.23160.0.1

#### 🐛 Fixes

- **Source Manager – Add Source ordering** — Fixed the roll-up order so that constructor and field positions are correctly evaluated before the top of the container when determining insertion points for **Add Before** and **Add After** operations.

#### 🆕 New

- **`GenerateCSharpTypeName` extension** — Added a new C# generation extension on `CsSource` models that generates the fully qualified C# type name for a target container (`Class`, `Structure`, `Interface`, `Record`).

---

### 2.23157.0.1

Initial general availability release of the CodeFactory runtime and SDK (v2.0) for Visual Studio for Windows.

- Migration of libraries from .NET Framework to .NET Standard where applicable.
- Upgraded all automation libraries to target .NET Framework 4.8.
- Integrated ADK functionality into the core runtime.
- Streamlined API call naming for improved developer experience.
- First generation of Code Blocks and Builders incorporated into the SDK and runtime.

---

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).  
Copyright © 2026 CodeFactory, LLC.

---

## Links

- 🏠 [GitHub Repository](https://github.com/CodeFactoryLLC/CodeFactoryForWindows)
- 🛒 [Visual Studio Marketplace — CodeFactory for Windows](https://marketplace.visualstudio.com/items?itemName=CodeFactoryLLC.CodeFactoryForWindows)
- 📦 [NuGet — CodeFactory.WinVs](https://www.nuget.org/packages/CodeFactory.WinVs)