namespace PluginSystem.Core.Services;

/// <summary>
/// Service responsible for discovering and loading plugins from assemblies.
/// </summary>
public interface IPluginLoader
{
    /// <summary>
    /// Loads all plugins from the specified assembly.
    /// </summary>
    /// <param name="assemblyName">The name of the assembly to load plugins from.</param>
    /// <returns>Collection of loaded plugin instances.</returns>
    IEnumerable<IPlugin> LoadFromAssembly(string assemblyName);

    /// <summary>
    /// Loads all plugins from the specified assembly path.
    /// </summary>
    /// <param name="assemblyPath">The file path to the assembly.</param>
    /// <returns>Collection of loaded plugin instances.</returns>
    IEnumerable<IPlugin> LoadFromAssemblyPath(string assemblyPath);

    /// <summary>
    /// Loads all plugins from assemblies in the specified directory.
    /// </summary>
    /// <param name="directoryPath">The directory containing plugin assemblies.</param>
    /// <param name="searchPattern">Optional search pattern for assembly files (default: "*.dll").</param>
    /// <returns>Collection of loaded plugin instances.</returns>
    IEnumerable<IPlugin> LoadFromDirectory(string directoryPath, string searchPattern = "*.dll");
}

