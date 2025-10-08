namespace PluginSystem.Core;

/// <summary>
/// Abstract base class for plugins that provides common functionality.
/// Plugin implementations can inherit from this class to reduce boilerplate.
/// </summary>
public abstract class PluginBase : IPlugin
{
    public abstract string PluginId { get; }
    public abstract string Name { get; }
    public virtual string Description => string.Empty;
    public virtual string Version => "1.0.0";
    public abstract IReadOnlyList<string> Categories { get; }
    public virtual int Priority => 100;

    public abstract Task<PluginResult> ExecuteAsync(IPluginContext context);

    /// <summary>
    /// Helper method to create a success result.
    /// </summary>
    protected PluginResult Success(string? message = null, object? data = null)
    {
        return PluginResult.Success(message, data);
    }

    /// <summary>
    /// Helper method to create a failure result.
    /// </summary>
    protected PluginResult Failure(string message, ResultSeverity severity = ResultSeverity.Error)
    {
        return PluginResult.Failure(message, severity);
    }

    /// <summary>
    /// Helper method to create a result with issues.
    /// </summary>
    protected PluginResult WithIssues(IEnumerable<PluginIssue> issues, string? message = null)
    {
        return PluginResult.WithIssues(issues, message);
    }

    /// <summary>
    /// Helper method to create a plugin issue.
    /// </summary>
    protected PluginIssue CreateIssue(
        ResultSeverity severity,
        string code,
        string message,
        string? location = null,
        Dictionary<string, object>? metadata = null)
    {
        return new PluginIssue
        {
            Severity = severity,
            Code = code,
            Message = message,
            Location = location,
            Metadata = metadata
        };
    }
}

