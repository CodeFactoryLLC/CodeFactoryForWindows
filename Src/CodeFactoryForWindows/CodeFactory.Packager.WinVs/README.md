# CodeFactory Software Development Kit for Visual Studio - Windows

[![NuGet](https://img.shields.io/nuget/v/CodeFactory.WinVs.SDK)](https://www.nuget.org/packages/CodeFactory.WinVs.SDK)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Overview

**CodeFactory.WinVs.SDK** is the complete, aggregated NuGet package for building **CodeFactory automation** that runs inside **Visual Studio for Windows**. Installing this single package gives you everything you need — the core runtime, Visual Studio integration, WPF UI support, and the MSBuild packaging toolchain — to author, build, and deploy CodeFactory automation libraries (`.cfa` files).

This package bundles and coordinates three foundational CodeFactory packages:

| Package | Purpose |
|---|---|
| `CodeFactory` | Core CodeFactory runtime and model APIs |
| `CodeFactory.WinVs` | Visual Studio for Windows automation APIs and command infrastructure |
| `CodeFactory.WinVs.Wpf` | WPF-based UI components for CodeFactory commands |

---

## Requirements

- **Visual Studio** 2022 or later (Windows)
- **.NET Framework 4.8** (automation host target)
- **.NET Standard 2.0** (library compatibility)
- **CodeFactory Runtime** — must be installed in Visual Studio at a version equal to or greater than this SDK version

---

## Getting Started

### Installation

Install the SDK via the NuGet Package Manager or the .NET CLI:
```
dotnet add package CodeFactory.WinVs.SDK
```


Or search for **`CodeFactory.WinVs.SDK`** in the Visual Studio NuGet Package Manager UI.

### Creating a CodeFactory Automation Project

1. Install the **CodeFactory for Visual Studio** extension from the Visual Studio Marketplace.
2. Use the CodeFactory project templates to scaffold a new automation library.
3. Add this NuGet package to your automation project.
4. Implement your automation commands by inheriting from the appropriate command base classes provided in `CodeFactory.WinVs`.
5. Build your project — the included MSBuild targets automatically invoke the **CodeFactory Packager** to produce a `.cfa` automation package file.

---

## What's Included

This SDK package delivers the following to your project:

### MSBuild Integration
- **`CodeFactory.WinVs.SDK.targets`** — MSBuild targets that wire the packaging step into your build pipeline (`build/net48/`).
- **`PackagerWinVs.targets`** — Supporting MSBuild targets for the packager tool (`tools/`).

### CodeFactory Packager Tool
- **`CodeFactory.Packager.WinVs.exe`** — A command-line utility (targeting `net48`) that inspects your compiled automation assembly, validates SDK version compatibility, and bundles all required dependencies into a `.cfa` package file ready for deployment into Visual Studio.

### Runtime Assemblies (`tools/net48/`)
The following assemblies are included and deployed alongside the packager:

| Assembly | Description |
|---|---|
| `CodeFactory*.dll` | CodeFactory core and Visual Studio automation APIs |
| `MessagePack*.dll` | High-performance binary serialization |
| `Microsoft.Extensions.CommandLineUtils.dll` | CLI argument parsing |
| `Microsoft.Bcl.AsyncInterfaces.dll` | Async interface backport for .NET Framework |
| `Microsoft.NET.StringTools.dll` | MSBuild string utilities |
| `Serilog.dll` | Structured logging |
| `System.Buffers.dll` | Memory buffer utilities |
| `System.Collections.Immutable.dll` | Immutable collection types |
| `System.IO.Packaging.dll` | Package I/O support |
| `System.Memory.dll` | Memory span support |
| `System.Numerics.Vectors.dll` | SIMD vector support |
| `System.Runtime.CompilerServices.Unsafe.dll` | Low-level memory operations |
| `System.Threading.Tasks.Extensions.dll` | Task extension APIs |

---

## Command Configuration

CodeFactory commands are configured via the **ExternalConfig** system.

> ⚠️ **Note:** The `CommandManager` class is deprecated. All command configuration must now be done through the `ExternalConfig` class. `ConfigParameter` supports the following value types: `string`, `bool`, `datetime`, and `list`.

### External Configuration Editor

An external configuration editor is available directly in the IDE:

1. Right-click the **Solution** node in Solution Explorer.
2. Select **[cF] Automation Configuration**.
3. Use the editor to configure all commands hosted in your `.cfa` file.

---

## Transaction History

CodeFactory tracks historical statistics for all commands that write to files in projects or the solution. The following data is recorded per transaction:

- Name of the file updated
- Name of the project the file resides in (or `null` if not part of a project)
- Name of the solution
- Relative path of the file from the project or solution root
- Number of characters written (excluding whitespace and line returns)
- Number of lines written (excluding empty lines)

Transaction results are returned directly from any insert or replace operation.

---

## Upgrading

When updating your automation project to a new version of this SDK:

1. Update the `CodeFactory.WinVs.SDK` NuGet package reference to the new version.
2. **Recompile** all automation projects targeting the new SDK version.
3. Ensure the **CodeFactory runtime** installed in Visual Studio is at least the same version as the SDK or higher.

---

## License

This package is licensed under the [MIT License](https://opensource.org/licenses/MIT).  
Copyright © 2026 CodeFactory, LLC.

---

## Resources

- 🌐 [CodeFactory for Windows — GitHub Repository](https://github.com/CodeFactoryLLC/CodeFactoryForWindows)
- 📦 [NuGet Gallery — CodeFactory.WinVs.SDK](https://www.nuget.org/packages/CodeFactory.WinVs.SDK)
- 📄 [CodeFactory Core Package](https://www.nuget.org/packages/CodeFactory)
- 📄 [CodeFactory.WinVs Package](https://www.nuget.org/packages/CodeFactory.WinVs)
- 📄 [CodeFactory.WinVs.Wpf Package](https://www.nuget.org/packages/CodeFactory.WinVs.Wpf)