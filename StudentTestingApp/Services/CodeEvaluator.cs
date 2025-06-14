using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StudentTestingApp.Models;
using System.Diagnostics;

namespace StudentTestingApp.Services
{
    public class CodeEvaluator
    {
        public async Task<List<CodeEvaluationResult>> EvaluateAsync(string code, IEnumerable<TaskTestCase> testCases)
        {
            var results = new List<CodeEvaluationResult>();

            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            // Gather metadata references from the currently loaded assemblies so
            // that user code can reference common namespaces such as
            // System.Linq without manually specifying every assembly.
            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .Select(a => a.Location)
                .Distinct()
                .Select(l => MetadataReference.CreateFromFile(l))
                .Cast<MetadataReference>()
                .ToList();
            var compilation = CSharpCompilation.Create(
                "Submission",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            var exePath = Path.Combine(Path.GetTempPath(), $"submission_{Guid.NewGuid():N}.exe");
            var emitResult = compilation.Emit(exePath);

            if (!emitResult.Success)
            {
                string errors = string.Join(System.Environment.NewLine, emitResult.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.ToString()));
                foreach (var _ in testCases)
                {
                    results.Add(new CodeEvaluationResult { Success = false, Errors = errors });
                }
                return results;
            }

            foreach (var testCase in testCases)
            {
                var startInfo = new ProcessStartInfo(exePath)
                {
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using var process = Process.Start(startInfo);
                if (process == null)
                {
                    results.Add(new CodeEvaluationResult { Success = false, Errors = "Failed to start process." });
                    continue;
                }

                if (!string.IsNullOrEmpty(testCase.Input))
                {
                    await process.StandardInput.WriteAsync(testCase.Input);
                }
                process.StandardInput.Close();

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                if (!process.WaitForExit(5000))
                {
                    process.Kill();
                    results.Add(new CodeEvaluationResult { Success = false, Errors = "Timeout" });
                    continue;
                }
                string trimmedOutput = output.Trim();
                results.Add(new CodeEvaluationResult
                {
                    Success = trimmedOutput == testCase.ExpectedOutput.Trim(),
                    Output = trimmedOutput,
                    Errors = error
                });
            }

            try { File.Delete(exePath); } catch { }
            return results;
        }
    }
}
