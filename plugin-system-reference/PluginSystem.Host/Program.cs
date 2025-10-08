using PluginSystem.Core;
using PluginSystem.Core.Services;
using PluginSystem.DocumentValidation.Models;

namespace PluginSystem.Host;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Plugin System Reference Implementation ===");
        Console.WriteLine("Demonstrating a flexible plugin-based architecture\n");

        // Initialize the plugin manager
        var pluginManager = new PluginManager();

        // Load plugins from the DocumentValidation assembly
        Console.WriteLine("Loading plugins from PluginSystem.DocumentValidation assembly...");
        pluginManager.Initialize("PluginSystem.DocumentValidation");

        Console.WriteLine($"Loaded {pluginManager.LoadedPlugins.Count} plugin(s):\n");
        foreach (var plugin in pluginManager.LoadedPlugins.OrderBy(p => p.Priority))
        {
            Console.WriteLine($"  [{plugin.PluginId}] {plugin.Name}");
            Console.WriteLine($"    Description: {plugin.Description}");
            Console.WriteLine($"    Version: {plugin.Version}");
            Console.WriteLine($"    Priority: {plugin.Priority}");
            Console.WriteLine($"    Categories: {string.Join(", ", plugin.Categories)}");
            Console.WriteLine();
        }

        // Create sample documents to validate
        var documents = new[]
        {
            CreateValidDocument(),
            CreateInvalidDocument(),
            CreatePartiallyValidDocument()
        };

        // Process each document
        foreach (var document in documents)
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine($"Processing Document: {document.Title}");
            Console.WriteLine(new string('=', 80));

            // Create execution context
            var context = new PluginContext("DocumentValidation", document);

            // Execute plugins (parallel execution for better performance)
            var result = await pluginManager.ExecuteAsync(context, parallel: true);

            // Display results
            DisplayExecutionResult(result);
            Console.WriteLine();
        }

        Console.WriteLine("\n=== Demonstration Complete ===");
    }

    static Document CreateValidDocument()
    {
        return new Document
        {
            Id = "DOC-001",
            Title = "Valid Document Example",
            Content = "This is a valid document with sufficient content that meets all validation requirements.",
            Author = "John Doe",
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            ModifiedDate = DateTime.UtcNow.AddDays(-5),
            DocumentType = "Article",
            Tags = new List<string> { "example", "valid", "test" },
            FileSizeBytes = 1024 * 50, // 50 KB
            FileFormat = "txt"
        };
    }

    static Document CreateInvalidDocument()
    {
        return new Document
        {
            Id = "", // Missing required field
            Title = new string('A', 250), // Too long
            Content = "Short", // Too short
            Author = "",
            CreatedDate = DateTime.UtcNow.AddDays(10), // Future date - invalid
            ModifiedDate = DateTime.UtcNow.AddDays(-100), // Before created date
            DocumentType = "",
            Tags = Enumerable.Range(1, 15).Select(i => $"Tag{i}").ToList(), // Too many tags
            FileSizeBytes = 100 * 1024 * 1024, // 100 MB - too large
            FileFormat = "pdf"
        };
    }

    static Document CreatePartiallyValidDocument()
    {
        return new Document
        {
            Id = "DOC-003",
            Title = "Partially Valid Document",
            Content = "This document has some issues but is mostly valid.",
            Author = "", // Missing recommended field
            CreatedDate = DateTime.UtcNow.AddYears(-11), // Very old
            ModifiedDate = DateTime.UtcNow.AddDays(-1),
            DocumentType = "Report",
            Tags = new List<string> { "test", "", "example" }, // Contains empty tag
            FileSizeBytes = 2048,
            FileFormat = "docx"
        };
    }

    static void DisplayExecutionResult(ExecutionResult result)
    {
        Console.WriteLine($"\nExecution Summary:");
        Console.WriteLine($"  Status: {(result.IsSuccess ? "SUCCESS" : "FAILED")}");
        Console.WriteLine($"  Total Plugins: {result.TotalPlugins}");
        Console.WriteLine($"  Successful: {result.SuccessfulPlugins}");
        Console.WriteLine($"  Failed: {result.FailedPlugins}");
        Console.WriteLine($"  Total Issues: {result.AllIssues.Count}");

        if (result.Exceptions.Count > 0)
        {
            Console.WriteLine($"\n  Exceptions: {result.Exceptions.Count}");
            foreach (var ex in result.Exceptions)
            {
                Console.WriteLine($"    - {ex.Message}");
            }
        }

        // Display individual plugin results
        Console.WriteLine("\nPlugin Results:");
        foreach (var pluginResult in result.PluginResults.OrderBy(r => r.Plugin.Priority))
        {
            var statusIcon = pluginResult.Result.IsSuccess ? "✓" : "✗";
            var severityColor = pluginResult.Result.Severity switch
            {
                ResultSeverity.Error => "ERROR",
                ResultSeverity.Warning => "WARN",
                ResultSeverity.Critical => "CRIT",
                _ => "INFO"
            };

            Console.WriteLine($"  {statusIcon} [{pluginResult.Plugin.PluginId}] {pluginResult.Plugin.Name}");
            Console.WriteLine($"    Execution Time: {pluginResult.ExecutionTime.TotalMilliseconds:F2}ms");
            Console.WriteLine($"    Severity: {severityColor}");

            if (!string.IsNullOrEmpty(pluginResult.Result.Message))
            {
                Console.WriteLine($"    Message: {pluginResult.Result.Message}");
            }

            if (pluginResult.Result.Issues.Count > 0)
            {
                Console.WriteLine($"    Issues ({pluginResult.Result.Issues.Count}):");
                foreach (var issue in pluginResult.Result.Issues)
                {
                    var issueIcon = issue.Severity == ResultSeverity.Error ? "⚠" : "ℹ";
                    Console.WriteLine($"      {issueIcon} [{issue.Code}] {issue.Message}");
                    if (!string.IsNullOrEmpty(issue.Location))
                    {
                        Console.WriteLine($"        Location: {issue.Location}");
                    }
                }
            }

            Console.WriteLine();
        }
    }
}
