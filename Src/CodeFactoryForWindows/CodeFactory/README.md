# CodeFactory Base Library

[![NuGet](https://img.shields.io/nuget/v/CodeFactory.svg)](https://www.nuget.org/packages/CodeFactory)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

The base contracts and logic used by CodeFactory automation. This library provides the core abstractions that are shared across all CodeFactory implementations, regardless of the target IDE.

---

## 📦 Package Information

| Property         | Value                          |
|------------------|-------------------------------|
| **Package ID**   | `CodeFactory`                 |
| **Version**      | `2.26151.0.1-PreRelease`      |
| **Target Framework** | .NET Standard 2.0         |
| **Authors**      | CodeFactory, LLC.             |
| **License**      | MIT                           |
| **Copyright**    | Copyright © 2026 CodeFactory, LLC. |

---

## 📋 Description

`CodeFactory` is the foundational library for the CodeFactory SDK. It defines the core contracts, interfaces, and shared logic consumed by all CodeFactory IDE integrations. By targeting **.NET Standard 2.0**, this library is compatible across a wide range of .NET runtimes and frameworks.

This package is a dependency of higher-level CodeFactory SDK packages such as `CodeFactory.WinVs`, which provides the full Visual Studio for Windows implementation.

---

## 🚀 Getting Started

### Installation

Install via the NuGet Package Manager:
```
Install-Package CodeFactory -Version 2.26151.0.1-PreRelease
```


Or via the .NET CLI:
```
dotnet add package CodeFactory
```


---

## 🗂️ Namespaces

| Namespace | Description |
|-----------|-------------|
| `CodeFactory` | Root namespace containing core CodeFactory types and base contracts. |
| `CodeFactory.SourceCode` | Models and abstractions for representing source code constructs, such as `SourceLocation`. |

---

## 🔑 Key Types

### `CodeFactory.SourceCode.SourceLocation`
Represents a specific location within a source code file, used throughout the CodeFactory model system to identify where code constructs exist in source.

---

## 🔗 Related Packages

| Package | Description |
|---------|-------------|
| [`CodeFactory.WinVs`](https://www.nuget.org/packages/CodeFactory.WinVs) | Full SDK implementation for Visual Studio for Windows, including project system models, C# source models, and automation commands. |
| [`CodeFactory.WinVs.Wpf`](https://www.nuget.org/packages/CodeFactory.WinVs.Wpf) | WPF UI controls library for Visual Studio for Windows automation. Targets .NET Framework 4.8. |

---

## 📄 Release Notes

### 2.26151.0.1

Recompile Release:
- When you update your automation to this version of the SDK.
- You will need to recompile your automation projects to the new version of the SDK, and make sure you have the CodeFactory runtime at least the same version or higher.

---

## 📚 Documentation & Resources

- [CodeFactory for Windows – Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=CodeFactoryLLC.CodeFactoryForWindows)
- [GitHub Repository](https://github.com/CodeFactoryLLC/CodeFactoryForWindows)
- [API Documentation](https://codefactoryllc.github.io/CodeFactoryForWindows/api/CodeFactory.html)

---

## 🤝 Contributing

Contributions are welcome! Please review the repository's contribution guidelines on [GitHub](https://github.com/CodeFactoryLLC/CodeFactoryForWindows) before submitting a pull request.

---

## 📃 License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).  
Copyright © 2026 CodeFactory, LLC.