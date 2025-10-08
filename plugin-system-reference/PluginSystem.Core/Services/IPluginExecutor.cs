namespace PluginSystem.Core.Services;

/// <summary>
/// Service responsible for executing plugins against contexts.
/// </summary>
public interface IPluginExecutor
{
    /// <summary>
    /// Executes all applicable plugins for the given context.
    /// </summary>
    /// <param name="plugins">Collection of plugins to execute.</param>
    /// <param name="context">The execution context.</param>
    /// <returns>Aggregated results from all plugin executions.</returns>
    Task<ExecutionResult> ExecuteAsync(IEnumerable<IPlugin> plugins, IPluginContext context);

    /// <summary>
    /// Executes all applicable plugins for the given context in parallel.
    /// </summary>
    /// <param name="plugins">Collection of plugins to execute.</param>
    /// <param name="context">The execution context.</param>
    /// <returns>Aggregated results from all plugin executions.</returns>
    Task<ExecutionResult> ExecuteParallelAsync(IEnumerable<IPlugin> plugins, IPluginContext context);
}

/// <summary>
/// Represents the aggregated result of executing multiple plugins.
/// </summary>
public class ExecutionResult
{
    /// <summary>
    /// Indicates whether all plugin executions were successful.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Total number of plugins executed.
    /// </summary>
    public int TotalPlugins { get; init; }

    /// <summary>
    /// Number of plugins that executed successfully.
    /// </summary>
    public int SuccessfulPlugins { get; init; }

    /// <summary>
    /// Number of plugins that failed.
    /// </summary>
    public int FailedPlugins { get; init; }

    /// <summary>
    /// Individual results from each plugin execution.
    /// </summary>
    public IReadOnlyList<PluginExecutionResult> PluginResults { get; init; } = [];

    /// <summary>
    /// All issues collected from all plugin executions.
    /// </summary>
    public IReadOnlyList<PluginIssue> AllIssues { get; init; } = [];

    /// <summary>
    /// Exceptions that occurred during execution.
    /// </summary>
    public IReadOnlyList<Exception> Exceptions { get; init; } = [];
}

/// <summary>
/// Represents the result of a single plugin execution.
/// </summary>
public class PluginExecutionResult
{
    /// <summary>
    /// The plugin that was executed.
    /// </summary>
    public required IPlugin Plugin { get; init; }

    /// <summary>
    /// The result from the plugin execution.
    /// </summary>
    public required PluginResult Result { get; init; }

    /// <summary>
    /// Time taken to execute the plugin.
    /// </summary>
    public TimeSpan ExecutionTime { get; init; }

    /// <summary>
    /// Exception that occurred during execution, if any.
    /// </summary>
    public Exception? Exception { get; init; }
}

