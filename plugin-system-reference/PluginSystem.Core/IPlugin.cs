namespace PluginSystem.Core;

/// <summary>
/// Core interface that all plugins must implement.
/// This is the contract between the plugin system and individual plugins.
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Unique identifier for the plugin.
    /// Used for logging, configuration, and plugin management.
    /// </summary>
    string PluginId { get; }

    /// <summary>
    /// Human-readable name of the plugin.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Description of what the plugin does.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Version of the plugin.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Categories or contexts where this plugin should be executed.
    /// Allows filtering plugins based on execution context.
    /// </summary>
    IReadOnlyList<string> Categories { get; }

    /// <summary>
    /// Priority for plugin execution order within the same category.
    /// Lower numbers execute first. Default is 100.
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Executes the plugin logic against the provided context.
    /// </summary>
    /// <param name="context">The execution context containing data to process.</param>
    /// <returns>Result of the plugin execution.</returns>
    Task<PluginResult> ExecuteAsync(IPluginContext context);
}

