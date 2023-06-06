<div class="article">
<a class="brand">
<img src="images/logo.png" width=70 /> 
<span class="brand-title">CodeFactory for Windows</span>
</a>

## Overview
This is the offical home of version 2.0 of CodeFactory. 
The reason for the change is future planning for the usage of CodeFactory. 
In the future CodeFactory will run on multiple IDE environments.
Right now we are focusing on VisualStudio for Windows.

The following are some future IDE's we are looking in to.
- **Microsoft Visual Studio Code**
- **Jet Brains Rider**

## In Preview Mode
The 2.0 version of CodeFactory is fully functional and in preview mode since all documentation has not been completed. 

Additional functionality is being added to the product. 

## Getting Started
To jump right in goto our [Getting Started](./gettingstarted/intro.md) section to create new project and continue to use CodeFactory automation.

## SDK Libraries
The following are the new libraries and utilties that are used for building CodeFactory automation.

### CodeFactory.dll
This is the core contracts used by all version's of CodeFactory regardless of IDE. This has been ported to **.Net Standard 2.0**.

### CodeFactory.WinVs.dll
This is the entire implementation of the SDK related to Visual Studio for Windows. This contains a streamlined SDK over the version 1.0 of CodeFactory. 
The SDK has been ported to **.Net Standard 2.0**.

### CodeFactory.WinVs.Wpf.dll
This is the user interface controls library for Visual Studio for Windows this hostes the view control.
The SDK has been upgraded to **.Net Framework 4.8** requirement of Visual Studio for Windows.

### CodeFactory.Packager.WinVs.exe
The packager is a command line utility that packages up CodeFactory automation. 
This utility is automatically called after the build of a CodeFactory library.
The packager has been upgraded to **.Net Framework 4.8**.


