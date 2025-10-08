namespace PluginSystem.Core.Services;

/// <summary>
/// High-level service that manages the complete plugin lifecycle.
/// Coordinates plugin loading, registration, and execution.
/// </summary>
public class PluginManager
{
    private readonly IPluginLoader _loader;
    private readonly IPluginExecutor _executor;
    private readonly List<IPlugin> _loadedPlugins;
    private bool _isInitialized;

    public IReadOnlyList<IPlugin> LoadedPlugins => _loadedPlugins.AsReadOnly();

    public PluginManager(IPluginLoader? loader = null, IPluginExecutor? executor = null)
    {
        _loader = loader ?? new PluginLoader();
        _executor = executor ?? new PluginExecutor();
        _loadedPlugins = new List<IPlugin>();
        _isInitialized = false;
    }

    /// <summary>
    /// Initializes the plugin manager by loading plugins from the specified assembly.
    /// </summary>
    public void Initialize(string assemblyName)
    {
        if (_isInitialized)
        {
            return;
        }

        try
        {
            var plugins = _loader.LoadFromAssembly(assemblyName);
            _loadedPlugins.AddRange(plugins);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to initialize plugin manager with assembly '{assemblyName}'", ex);
        }
    }

    /// <summary>
    /// Initializes the plugin manager by loading plugins from the specified directory.
    /// </summary>
    public void InitializeFromDirectory(string directoryPath, string searchPattern = "*.dll")
    {
        if (_isInitialized)
        {
            return;
        }

        try
        {
            var plugins = _loader.LoadFromDirectory(directoryPath, searchPattern);
            _loadedPlugins.AddRange(plugins);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to initialize plugin manager from directory '{directoryPath}'", ex);
        }
    }

    /// <summary>
    /// Manually registers a plugin instance.
    /// </summary>
    public void RegisterPlugin(IPlugin plugin)
    {
        if (plugin == null)
        {
            throw new ArgumentNullException(nameof(plugin));
        }

        if (_loadedPlugins.Any(p => p.PluginId == plugin.PluginId))
        {
            throw new InvalidOperationException($"Plugin with ID '{plugin.PluginId}' is already registered");
        }

        _loadedPlugins.Add(plugin);
    }

    /// <summary>
    /// Executes all applicable plugins for the given context.
    /// </summary>
    public async Task<ExecutionResult> ExecuteAsync(IPluginContext context, bool parallel = false)
    {
        if (!_isInitialized && _loadedPlugins.Count == 0)
        {
            throw new InvalidOperationException("Plugin manager is not initialized. Call Initialize() first.");
        }

        if (parallel)
        {
            return await _executor.ExecuteParallelAsync(_loadedPlugins, context);
        }

        return await _executor.ExecuteAsync(_loadedPlugins, context);
    }

    /// <summary>
    /// Executes plugins with filtering options.
    /// </summary>
    public async Task<ExecutionResult> ExecuteAsync(
        IPluginContext context,
        Func<IPlugin, bool>? filter = null,
        bool parallel = false)
    {
        if (!_isInitialized && _loadedPlugins.Count == 0)
        {
            throw new InvalidOperationException("Plugin manager is not initialized. Call Initialize() first.");
        }

        var pluginsToExecute = filter != null
            ? _loadedPlugins.Where(filter)
            : _loadedPlugins;

        if (parallel)
        {
            return await _executor.ExecuteParallelAsync(pluginsToExecute, context);
        }

        return await _executor.ExecuteAsync(pluginsToExecute, context);
    }

    /// <summary>
    /// Gets plugins by category.
    /// </summary>
    public IEnumerable<IPlugin> GetPluginsByCategory(string category)
    {
        return _loadedPlugins.Where(p => p.Categories.Contains(category, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets a plugin by its ID.
    /// </summary>
    public IPlugin? GetPluginById(string pluginId)
    {
        return _loadedPlugins.FirstOrDefault(p => p.PluginId.Equals(pluginId, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Clears all loaded plugins and resets initialization state.
    /// </summary>
    public void Reset()
    {
        _loadedPlugins.Clear();
        _isInitialized = false;
    }
}

