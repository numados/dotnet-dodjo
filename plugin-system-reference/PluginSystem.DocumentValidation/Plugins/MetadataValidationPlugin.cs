using PluginSystem.Core;
using PluginSystem.DocumentValidation.Models;

namespace PluginSystem.DocumentValidation.Plugins;

/// <summary>
/// Plugin that validates document metadata and tags.
/// </summary>
public class MetadataValidationPlugin : PluginBase
{
    private const int MaxTags = 10;
    private const int MaxTagLength = 50;
    private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50 MB

    public override string PluginId => "DOC-VAL-004";
    public override string Name => "Metadata Validation";
    public override string Description => "Validates document metadata, tags, and file properties";
    public override string Version => "1.0.0";
    public override IReadOnlyList<string> Categories => new[] { "DocumentValidation", "PostProcessing" };
    public override int Priority => 40;

    public override Task<PluginResult> ExecuteAsync(IPluginContext context)
    {
        var document = context.GetData<Document>();
        var issues = new List<PluginIssue>();

        // Validate tags
        if (document.Tags.Count > MaxTags)
        {
            issues.Add(CreateIssue(
                ResultSeverity.Warning,
                "TOO_MANY_TAGS",
                $"Document has {document.Tags.Count} tags, which exceeds the recommended maximum of {MaxTags}",
                "Document.Tags"
            ));
        }

        foreach (var tag in document.Tags)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Warning,
                    "EMPTY_TAG",
                    "Document contains empty or whitespace-only tags",
                    "Document.Tags"
                ));
            }
            else if (tag.Length > MaxTagLength)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Warning,
                    "TAG_TOO_LONG",
                    $"Tag '{tag}' exceeds maximum length of {MaxTagLength} characters",
                    "Document.Tags"
                ));
            }
        }

        // Validate file size
        if (document.FileSizeBytes.HasValue)
        {
            if (document.FileSizeBytes.Value <= 0)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Error,
                    "INVALID_FILE_SIZE",
                    "File size must be greater than 0",
                    "Document.FileSizeBytes"
                ));
            }
            else if (document.FileSizeBytes.Value > MaxFileSizeBytes)
            {
                issues.Add(CreateIssue(
                    ResultSeverity.Error,
                    "FILE_TOO_LARGE",
                    $"File size ({document.FileSizeBytes.Value / 1024 / 1024:F2} MB) exceeds maximum allowed size of {MaxFileSizeBytes / 1024 / 1024} MB",
                    "Document.FileSizeBytes"
                ));
            }
        }

        // Validate document type
        if (string.IsNullOrWhiteSpace(document.DocumentType))
        {
            issues.Add(CreateIssue(
                ResultSeverity.Warning,
                "MISSING_DOCUMENT_TYPE",
                "Document type is not specified",
                "Document.DocumentType"
            ));
        }

        if (issues.Count == 0)
        {
            return Task.FromResult(Success("Metadata validation passed"));
        }

        return Task.FromResult(WithIssues(issues, $"Found {issues.Count} metadata validation issue(s)"));
    }
}

