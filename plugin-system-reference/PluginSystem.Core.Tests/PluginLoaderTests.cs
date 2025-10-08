using PluginSystem.Core.Services;

namespace PluginSystem.Core.Tests;

public class PluginLoaderTests
{
    [Fact]
    public void LoadFromAssembly_WithValidAssembly_LoadsPlugins()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act
        var plugins = loader.LoadFromAssembly("PluginSystem.DocumentValidation").ToList();

        // Assert
        Assert.NotEmpty(plugins);
        Assert.All(plugins, p => Assert.NotNull(p.PluginId));
        Assert.All(plugins, p => Assert.NotNull(p.Name));
    }

    [Fact]
    public void LoadFromAssembly_WithInvalidAssembly_ThrowsPluginLoadException()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act & Assert
        Assert.Throws<PluginLoadException>(() => 
            loader.LoadFromAssembly("NonExistentAssembly").ToList());
    }

    [Fact]
    public void LoadFromAssemblyPath_WithValidPath_LoadsPlugins()
    {
        // Arrange
        var loader = new PluginLoader();
        var assemblyPath = typeof(PluginSystem.DocumentValidation.Models.Document).Assembly.Location;

        // Act
        var plugins = loader.LoadFromAssemblyPath(assemblyPath).ToList();

        // Assert
        Assert.NotEmpty(plugins);
    }

    [Fact]
    public void LoadFromAssemblyPath_WithInvalidPath_ThrowsPluginLoadException()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act & Assert
        Assert.Throws<PluginLoadException>(() => 
            loader.LoadFromAssemblyPath("/invalid/path/assembly.dll").ToList());
    }

    [Fact]
    public void LoadFromDirectory_WithValidDirectory_LoadsPlugins()
    {
        // Arrange
        var loader = new PluginLoader();
        var assemblyLocation = typeof(PluginSystem.DocumentValidation.Models.Document).Assembly.Location;
        var directory = Path.GetDirectoryName(assemblyLocation);

        // Act
        var plugins = loader.LoadFromDirectory(directory!).ToList();

        // Assert
        Assert.NotEmpty(plugins);
    }

    [Fact]
    public void LoadFromDirectory_WithNonExistentDirectory_ThrowsDirectoryNotFoundException()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act & Assert
        Assert.Throws<DirectoryNotFoundException>(() => 
            loader.LoadFromDirectory("/nonexistent/directory").ToList());
    }

    [Fact]
    public void LoadFromDirectory_WithSearchPattern_FiltersCorrectly()
    {
        // Arrange
        var loader = new PluginLoader();
        var assemblyLocation = typeof(PluginSystem.DocumentValidation.Models.Document).Assembly.Location;
        var directory = Path.GetDirectoryName(assemblyLocation);

        // Act
        var plugins = loader.LoadFromDirectory(directory!, "PluginSystem.DocumentValidation.dll").ToList();

        // Assert
        Assert.NotEmpty(plugins);
    }

    [Fact]
    public void LoadedPlugins_HaveValidMetadata()
    {
        // Arrange
        var loader = new PluginLoader();

        // Act
        var plugins = loader.LoadFromAssembly("PluginSystem.DocumentValidation").ToList();

        // Assert
        Assert.All(plugins, p =>
        {
            Assert.False(string.IsNullOrWhiteSpace(p.PluginId));
            Assert.False(string.IsNullOrWhiteSpace(p.Name));
            Assert.NotNull(p.Categories);
            Assert.NotEmpty(p.Categories);
            Assert.True(p.Priority >= 0);
        });
    }
}

