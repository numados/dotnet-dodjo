using System.Diagnostics;

namespace PluginSystem.Core.Services;

/// <summary>
/// Default implementation of IPluginExecutor.
/// Supports both sequential and parallel plugin execution with comprehensive result aggregation.
/// </summary>
public class PluginExecutor : IPluginExecutor
{
    /// <inheritdoc />
    public async Task<ExecutionResult> ExecuteAsync(IEnumerable<IPlugin> plugins, IPluginContext context)
    {
        var applicablePlugins = FilterPluginsByCategory(plugins, context.Category)
            .OrderBy(p => p.Priority)
            .ToList();

        var results = new List<PluginExecutionResult>();
        var exceptions = new List<Exception>();

        foreach (var plugin in applicablePlugins)
        {
            var executionResult = await ExecutePluginAsync(plugin, context);
            results.Add(executionResult);

            if (executionResult.Exception != null)
            {
                exceptions.Add(executionResult.Exception);
            }
        }

        return BuildExecutionResult(results, exceptions);
    }

    /// <inheritdoc />
    public async Task<ExecutionResult> ExecuteParallelAsync(IEnumerable<IPlugin> plugins, IPluginContext context)
    {
        var applicablePlugins = FilterPluginsByCategory(plugins, context.Category).ToList();

        var results = new List<PluginExecutionResult>();
        var exceptions = new List<Exception>();
        var lockObject = new object();

        // Execute plugins in parallel using Parallel.ForEachAsync
        await Parallel.ForEachAsync(applicablePlugins, async (plugin, cancellationToken) =>
        {
            var executionResult = await ExecutePluginAsync(plugin, context);

            lock (lockObject)
            {
                results.Add(executionResult);
                if (executionResult.Exception != null)
                {
                    exceptions.Add(executionResult.Exception);
                }
            }
        });

        return BuildExecutionResult(results, exceptions);
    }

    private static IEnumerable<IPlugin> FilterPluginsByCategory(IEnumerable<IPlugin> plugins, string category)
    {
        return plugins.Where(p => p.Categories.Contains(category, StringComparer.OrdinalIgnoreCase));
    }

    private static async Task<PluginExecutionResult> ExecutePluginAsync(IPlugin plugin, IPluginContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        PluginResult? result = null;
        Exception? exception = null;

        try
        {
            result = await plugin.ExecuteAsync(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            result = PluginResult.Failure($"Plugin execution failed: {ex.Message}", ResultSeverity.Critical);
        }
        finally
        {
            stopwatch.Stop();
        }

        return new PluginExecutionResult
        {
            Plugin = plugin,
            Result = result,
            ExecutionTime = stopwatch.Elapsed,
            Exception = exception
        };
    }

    private static ExecutionResult BuildExecutionResult(
        List<PluginExecutionResult> results,
        List<Exception> exceptions)
    {
        var allIssues = results
            .SelectMany(r => r.Result.Issues)
            .ToList();

        var successfulCount = results.Count(r => r.Result.IsSuccess && r.Exception == null);
        var failedCount = results.Count - successfulCount;

        return new ExecutionResult
        {
            IsSuccess = failedCount == 0,
            TotalPlugins = results.Count,
            SuccessfulPlugins = successfulCount,
            FailedPlugins = failedCount,
            PluginResults = results,
            AllIssues = allIssues,
            Exceptions = exceptions
        };
    }
}

