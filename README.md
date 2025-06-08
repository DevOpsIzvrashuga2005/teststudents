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

4. **Solution file**: `StudentTestingApp.sln` includes the WPF project. You can open this solution in Visual Studio or run `dotnet build` on a Windows machine.

   **Note:** WPF applications rely on the `Microsoft.NET.Sdk.WindowsDesktop` workload. This SDK is only available on Windows, so attempting to build on Linux will fail.

The next steps will implement the database models, user interface screens, code evaluation logic, and secure execution environment.

## Step 2: Database and EF Core Models

1. **Add models** representing users, roles, tasks, test cases and submissions. These classes live in `StudentTestingApp/Models`.
2. **Create `StudentTestingContext`** derived from `DbContext` to access PostgreSQL. The context exposes `DbSet` properties for all entities and seeds the default roles.
3. **Update the project file** to reference `Microsoft.EntityFrameworkCore` and `Npgsql.EntityFrameworkCore.PostgreSQL` packages.
4. **Configure the connection string** when starting the application. `App.xaml.cs` reads the connection string from the `CONNECTION_STRING` environment variable (falling back to `Host=localhost;Database=testing;Username=postgres;Password=secret`). Example:
   ```csharp
   var conn = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
       ?? "Host=localhost;Database=testing;Username=postgres;Password=secret";
   var options = new DbContextOptionsBuilder<StudentTestingContext>()
       .UseNpgsql(conn)
       .Options;
   var db = new StudentTestingContext(options);
   db.Database.Migrate();
   ```
5. **Apply migrations** and create the database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   (You need the `dotnet-ef` tool installed with `dotnet tool install --global dotnet-ef`.)

These instructions set up the database layer so the next steps can implement the UI and code evaluation logic.

## Step 3: User Interface Screens

1. **LoginWindow** provides username and password fields with buttons to log in or register.
2. **TaskListWindow** now loads tasks from the database. Teachers see a "New Task" button that opens a form to create tasks.
3. Selecting a task opens **CodeEditorWindow** where students can write code and submit it for evaluation. The editor starts with a simple `Program` template so you can focus on your solution code immediately.

Open `App.xaml` to start the application with `LoginWindow`. Once authenticated you can navigate to the other windows.

## Step 4: Code Evaluation Logic

1. Add a reference to the Roslyn compiler package:
   ```bash
   dotnet add package Microsoft.CodeAnalysis.CSharp
   ```
2. The `Services/CodeEvaluator` class compiles student code using Roslyn and runs it against every test case. Each test case defines the input and expected output.
3. `CodeEvaluator` executes the compiled program in a separate process with a short timeout, captures the output, and returns a `CodeEvaluationResult` for each test case.
   Each submission is stored in the database and a 5 second limit prevents endless loops.
4. Example usage:
   ```csharp
   var evaluator = new CodeEvaluator();
   var results = await evaluator.EvaluateAsync(code, task.TestCases);
   foreach (var r in results)
   {
       Console.WriteLine($"Output: {r.Output}  Success: {r.Success}");
   }
   ```

This step lays the groundwork for automated grading of submissions. Security-hardening like sandboxing should be added in future steps.
