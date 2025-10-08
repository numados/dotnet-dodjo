namespace PluginSystem.Core;

/// <summary>
/// Represents the execution context passed to plugins.
/// Contains data and metadata for plugin processing.
/// </summary>
public interface IPluginContext
{
    /// <summary>
    /// Unique identifier for this execution context.
    /// </summary>
    string ContextId { get; }

    /// <summary>
    /// The category or type of processing being performed.
    /// </summary>
    string Category { get; }

    /// <summary>
    /// The main data object to be processed by plugins.
    /// </summary>
    object Data { get; }

    /// <summary>
    /// Additional metadata that can be used by plugins.
    /// </summary>
    IReadOnlyDictionary<string, object> Metadata { get; }

    /// <summary>
    /// Gets a strongly-typed data object from the context.
    /// </summary>
    /// <typeparam name="T">The expected type of the data.</typeparam>
    /// <returns>The data cast to the specified type.</returns>
    T GetData<T>() where T : class;

    /// <summary>
    /// Gets a metadata value by key.
    /// </summary>
    /// <typeparam name="T">The expected type of the metadata value.</typeparam>
    /// <param name="key">The metadata key.</param>
    /// <returns>The metadata value, or default if not found.</returns>
    T? GetMetadata<T>(string key);
}

