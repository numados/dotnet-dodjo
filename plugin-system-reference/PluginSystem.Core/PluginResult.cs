namespace PluginSystem.Core;

/// <summary>
/// Represents the result of a plugin execution.
/// </summary>
public class PluginResult
{
    /// <summary>
    /// Indicates whether the plugin execution was successful.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// The severity level of the result.
    /// </summary>
    public ResultSeverity Severity { get; init; }

    /// <summary>
    /// A message describing the result.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Additional details or data from the plugin execution.
    /// </summary>
    public object? Data { get; init; }

    /// <summary>
    /// Collection of issues or findings from the plugin execution.
    /// </summary>
    public IReadOnlyList<PluginIssue> Issues { get; init; } = Array.Empty<PluginIssue>();

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    public static PluginResult Success(string? message = null, object? data = null)
    {
        return new PluginResult
        {
            IsSuccess = true,
            Severity = ResultSeverity.Info,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Creates a result with issues.
    /// </summary>
    public static PluginResult WithIssues(IEnumerable<PluginIssue> issues, string? message = null)
    {
        var issueList = issues.ToList();
        var hasErrors = issueList.Any(i => i.Severity == ResultSeverity.Error);

        return new PluginResult
        {
            IsSuccess = !hasErrors,
            Severity = hasErrors ? ResultSeverity.Error : ResultSeverity.Warning,
            Message = message,
            Issues = issueList
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    public static PluginResult Failure(string message, ResultSeverity severity = ResultSeverity.Error)
    {
        return new PluginResult
        {
            IsSuccess = false,
            Severity = severity,
            Message = message
        };
    }
}

/// <summary>
/// Represents an issue found during plugin execution.
/// </summary>
public class PluginIssue
{
    /// <summary>
    /// The severity of the issue.
    /// </summary>
    public ResultSeverity Severity { get; init; }

    /// <summary>
    /// A code or identifier for the issue type.
    /// </summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>
    /// Description of the issue.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The location or context where the issue was found.
    /// </summary>
    public string? Location { get; init; }

    /// <summary>
    /// Additional metadata about the issue.
    /// </summary>
    public IReadOnlyDictionary<string, object>? Metadata { get; init; }
}

/// <summary>
/// Severity levels for plugin results and issues.
/// </summary>
public enum ResultSeverity
{
    /// <summary>
    /// Informational message.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning that doesn't prevent success.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Error that indicates failure.
    /// </summary>
    Error = 2,

    /// <summary>
    /// Critical error.
    /// </summary>
    Critical = 3
}

