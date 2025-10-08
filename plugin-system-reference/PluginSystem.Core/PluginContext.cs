namespace PluginSystem.Core;

/// <summary>
/// Default implementation of IPluginContext.
/// </summary>
public class PluginContext : IPluginContext
{
    private readonly Dictionary<string, object> _metadata;

    public string ContextId { get; }
    public string Category { get; }
    public object Data { get; }
    public IReadOnlyDictionary<string, object> Metadata => _metadata;

    public PluginContext(string category, object data, string? contextId = null)
    {
        ContextId = contextId ?? Guid.NewGuid().ToString();
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Data = data ?? throw new ArgumentNullException(nameof(data));
        _metadata = new Dictionary<string, object>();
    }

    public PluginContext(string category, object data, Dictionary<string, object> metadata, string? contextId = null)
    {
        ContextId = contextId ?? Guid.NewGuid().ToString();
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Data = data ?? throw new ArgumentNullException(nameof(data));
        _metadata = metadata ?? new Dictionary<string, object>();
    }

    public T GetData<T>() where T : class
    {
        if (Data is T typedData)
        {
            return typedData;
        }

        throw new InvalidCastException($"Cannot cast data of type {Data.GetType().Name} to {typeof(T).Name}");
    }

    public T? GetMetadata<T>(string key)
    {
        if (_metadata.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    /// <summary>
    /// Adds or updates metadata.
    /// </summary>
    public void SetMetadata(string key, object value)
    {
        _metadata[key] = value;
    }
}

