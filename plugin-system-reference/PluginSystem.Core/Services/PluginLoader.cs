using System.Reflection;

namespace PluginSystem.Core.Services;

/// <summary>
/// Default implementation of IPluginLoader using reflection to discover and instantiate plugins.
/// Uses assembly scanning to find all types implementing IPlugin and creates instances dynamically.
/// </summary>
public class PluginLoader : IPluginLoader
{
    /// <inheritdoc />
    public IEnumerable<IPlugin> LoadFromAssembly(string assemblyName)
    {
        try
        {
            var assembly = Assembly.Load(assemblyName);
            return LoadPluginsFromAssembly(assembly);
        }
        catch (Exception ex)
        {
            throw new PluginLoadException($"Failed to load plugins from assembly '{assemblyName}'", ex);
        }
    }

    /// <inheritdoc />
    public IEnumerable<IPlugin> LoadFromAssemblyPath(string assemblyPath)
    {
        try
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            return LoadPluginsFromAssembly(assembly);
        }
        catch (Exception ex)
        {
            throw new PluginLoadException($"Failed to load plugins from assembly path '{assemblyPath}'", ex);
        }
    }

    /// <inheritdoc />
    public IEnumerable<IPlugin> LoadFromDirectory(string directoryPath, string searchPattern = "*.dll")
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Plugin directory not found: {directoryPath}");
        }

        var plugins = new List<IPlugin>();
        var assemblyFiles = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);

        foreach (var assemblyFile in assemblyFiles)
        {
            try
            {
                var loadedPlugins = LoadFromAssemblyPath(assemblyFile);
                plugins.AddRange(loadedPlugins);
            }
            catch (Exception ex)
            {
                // Log warning but continue loading other assemblies
                Console.WriteLine($"Warning: Failed to load plugins from {assemblyFile}: {ex.Message}");
            }
        }

        return plugins;
    }

    private static IEnumerable<IPlugin> LoadPluginsFromAssembly(Assembly assembly)
    {
        var pluginType = typeof(IPlugin);
        var plugins = new List<IPlugin>();

        // Find all types that implement IPlugin interface
        var types = assembly.GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract && pluginType.IsAssignableFrom(t));

        foreach (var type in types)
        {
            try
            {
                // Create instance of the plugin
                if (Activator.CreateInstance(type) is IPlugin plugin)
                {
                    plugins.Add(plugin);
                }
            }
            catch (Exception ex)
            {
                throw new PluginLoadException($"Failed to instantiate plugin type '{type.FullName}'", ex);
            }
        }

        return plugins;
    }
}

/// <summary>
/// Exception thrown when plugin loading fails.
/// </summary>
public class PluginLoadException : Exception
{
    public PluginLoadException(string message) : base(message)
    {
    }

    public PluginLoadException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

