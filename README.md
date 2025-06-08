# StudentTestingApp

This repository contains an example implementation for a desktop application that allows automatic testing of student code submissions. The application is built with **C#**, **WPF**, and uses **PostgreSQL** via **Entity Framework Core**. The code here only provides the initial project structure and setup instructions.

## Step 1: Project Structure and Environment Setup

1. **Install .NET SDK** (version 8.0 or later) and the desktop development workload. On Ubuntu you can add the Microsoft package feed and install the SDK:
   ```bash
   sudo apt-get update
   sudo apt-get install -y wget apt-transport-https
   wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-8.0
   ```
   On Windows you can install the .NET SDK and "Desktop development with C#" workload using the Visual Studio Installer.

2. **Create a WPF project** (the template is available on Windows):
   ```bash
   dotnet new wpf -n StudentTestingApp
   ```
   This repository contains a basic project structure with `App.xaml`, `MainWindow.xaml`, and the corresponding C# files. The project file uses the `Microsoft.NET.Sdk.WindowsDesktop` SDK with `UseWPF` enabled.

3. **Add Entity Framework Core and Npgsql** for PostgreSQL access:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
   ```

4. **Solution file**: `StudentTestingApp.sln` includes the WPF project. You can open this solution in Visual Studio or run `dotnet build` to compile on a Windows machine.

The next steps will implement the database models, user interface screens, code evaluation logic, and secure execution environment.
