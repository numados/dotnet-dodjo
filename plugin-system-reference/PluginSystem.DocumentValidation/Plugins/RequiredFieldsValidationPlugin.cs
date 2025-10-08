using PluginSystem.Core;
using PluginSystem.DocumentValidation.Models;

namespace PluginSystem.DocumentValidation.Plugins;

/// <summary>
/// Plugin that validates required fields in a document.
/// Demonstrates basic validation logic with error and warning severity levels.
/// </summary>
public class RequiredFieldsValidationPlugin : PluginBase
{
    public override string PluginId => "DOC-VAL-001";
    public override string Name => "Required Fields Validation";
    public override string Description => "Validates that all required fields are present and not empty";
    public override string Version => "1.0.0";
    public override IReadOnlyList<string> Categories => new[] { "DocumentValidation", "PreProcessing" };
    public override int Priority => 10; // Execute early

    public override Task<PluginResult> ExecuteAsync(IPluginContext context)
    {
        var document = context.GetData<Document>();
        var issues = new List<PluginIssue>();

        // Validate required fields
        if (string.IsNullOrWhiteSpace(document.Id))
        {
            issues.Add(CreateIssue(
                ResultSeverity.Error,
                "REQUIRED_FIELD_MISSING",
                "Document ID is required",
                "Document.Id"
            ));
        }

        if (string.IsNullOrWhiteSpace(document.Title))
        {
            issues.Add(CreateIssue(
                ResultSeverity.Error,
                "REQUIRED_FIELD_MISSING",
                "Document Title is required",
                "Document.Title"
            ));
        }

        if (string.IsNullOrWhiteSpace(document.Content))
        {
            issues.Add(CreateIssue(
                ResultSeverity.Error,
                "REQUIRED_FIELD_MISSING",
                "Document Content is required",
                "Document.Content"
            ));
        }

        if (string.IsNullOrWhiteSpace(document.Author))
        {
            issues.Add(CreateIssue(
                ResultSeverity.Warning,
                "RECOMMENDED_FIELD_MISSING",
                "Document Author is recommended but not required",
                "Document.Author"
            ));
        }

        if (issues.Count == 0)
        {
            return Task.FromResult(Success("All required fields are present"));
        }

        return Task.FromResult(WithIssues(issues, $"Found {issues.Count} validation issue(s)"));
    }
}

