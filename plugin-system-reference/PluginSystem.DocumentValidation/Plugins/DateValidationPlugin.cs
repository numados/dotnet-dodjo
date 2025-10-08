using PluginSystem.Core;
using PluginSystem.DocumentValidation.Models;

namespace PluginSystem.DocumentValidation.Plugins;

/// <summary>
/// Plugin that validates document dates.
/// </summary>
public class DateValidationPlugin : PluginBase
{
    public override string PluginId => "DOC-VAL-003";
    public override string Name => "Date Validation";
    public override string Description => "Validates document creation and modification dates";
    public override string Version => "1.0.0";
    public override IReadOnlyList<string> Categories => new[] { "DocumentValidation" };
    public override int Priority => 30;

    public override Task<PluginResult> ExecuteAsync(IPluginContext context)
    {
        var document = context.GetData<Document>();
        var issues = new List<PluginIssue>();

        // Validate created date
        if (document.CreatedDate.HasValue)
        {
            if (document.CreatedDate.Value > DateTime.UtcNow)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Error,
                    "INVALID_CREATED_DATE",
                    $"Document creation date cannot be in the future. Date: {document.CreatedDate.Value:yyyy-MM-dd HH:mm:ss}",
                    "Document.CreatedDate"
                ));
            }

            // Check if date is too old (more than 10 years)
            if (document.CreatedDate.Value < DateTime.UtcNow.AddYears(-10))
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Warning,
                    "OLD_DOCUMENT",
                    $"Document was created more than 10 years ago. Date: {document.CreatedDate.Value:yyyy-MM-dd}",
                    "Document.CreatedDate"
                ));
            }
        }

        // Validate modified date
        if (document.ModifiedDate.HasValue)
        {
            if (document.ModifiedDate.Value > DateTime.UtcNow)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Error,
                    "INVALID_MODIFIED_DATE",
                    $"Document modification date cannot be in the future. Date: {document.ModifiedDate.Value:yyyy-MM-dd HH:mm:ss}",
                    "Document.ModifiedDate"
                ));
            }

            // Modified date should be after or equal to created date
            if (document.CreatedDate.HasValue && document.ModifiedDate.Value < document.CreatedDate.Value)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Error,
                    "INVALID_DATE_SEQUENCE",
                    $"Document modification date ({document.ModifiedDate.Value:yyyy-MM-dd}) cannot be before creation date ({document.CreatedDate.Value:yyyy-MM-dd})",
                    "Document.ModifiedDate"
                ));
            }
        }

        if (issues.Count == 0)
        {
            return Task.FromResult(Success("Date validation passed"));
        }

        return Task.FromResult(WithIssues(issues, $"Found {issues.Count} date validation issue(s)"));
    }
}

