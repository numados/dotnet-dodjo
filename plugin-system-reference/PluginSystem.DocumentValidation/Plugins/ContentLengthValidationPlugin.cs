using PluginSystem.Core;
using PluginSystem.DocumentValidation.Models;

namespace PluginSystem.DocumentValidation.Plugins;

/// <summary>
/// Plugin that validates document content length constraints.
/// </summary>
public class ContentLengthValidationPlugin : PluginBase
{
    private const int MinContentLength = 10;
    private const int MaxContentLength = 100000;
    private const int MaxTitleLength = 200;

    public override string PluginId => "DOC-VAL-002";
    public override string Name => "Content Length Validation";
    public override string Description => "Validates that document content and title meet length requirements";
    public override string Version => "1.0.0";
    public override IReadOnlyList<string> Categories => new[] { "DocumentValidation" };
    public override int Priority => 20;

    public override Task<PluginResult> ExecuteAsync(IPluginContext context)
    {
        var document = context.GetData<Document>();
        var issues = new List<PluginIssue>();

        // Validate title length
        if (!string.IsNullOrEmpty(document.Title) && document.Title.Length > MaxTitleLength)
        {
            issues.Add(CreateIssue(
                ResultSeverity.Error,
                "TITLE_TOO_LONG",
                $"Document title exceeds maximum length of {MaxTitleLength} characters. Current length: {document.Title.Length}",
                "Document.Title",
                new Dictionary<string, object>
                {
                    { "MaxLength", MaxTitleLength },
                    { "ActualLength", document.Title.Length }
                }
            ));
        }

        // Validate content length
        if (!string.IsNullOrEmpty(document.Content))
        {
            if (document.Content.Length < MinContentLength)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Warning,
                    "CONTENT_TOO_SHORT",
                    $"Document content is shorter than recommended minimum of {MinContentLength} characters. Current length: {document.Content.Length}",
                    "Document.Content",
                    new Dictionary<string, object>
                    {
                        { "MinLength", MinContentLength },
                        { "ActualLength", document.Content.Length }
                    }
                ));
            }

            if (document.Content.Length > MaxContentLength)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Error,
                    "CONTENT_TOO_LONG",
                    $"Document content exceeds maximum length of {MaxContentLength} characters. Current length: {document.Content.Length}",
                    "Document.Content",
                    new Dictionary<string, object>
                    {
                        { "MaxLength", MaxContentLength },
                        { "ActualLength", document.Content.Length }
                    }
                ));
            }
        }

        if (issues.Count == 0)
        {
            return Task.FromResult(Success("Content length validation passed"));
        }

        return Task.FromResult(WithIssues(issues, $"Found {issues.Count} length validation issue(s)"));
    }
}

