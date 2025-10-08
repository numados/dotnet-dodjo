using PluginSystem.Core.Services;

namespace PluginSystem.Core.Tests;

public class PluginManagerTests
{
    [Fact]
    public void Initialize_WithValidAssembly_LoadsPlugins()
    {
        // Arrange
        var manager = new PluginManager();

        // Act
        manager.Initialize("PluginSystem.DocumentValidation");

        // Assert
        Assert.NotEmpty(manager.LoadedPlugins);
    }

    [Fact]
    public void Initialize_CalledTwice_LoadsOnlyOnce()
    {
        // Arrange
        var manager = new PluginManager();

        // Act
        manager.Initialize("PluginSystem.DocumentValidation");
        var firstCount = manager.LoadedPlugins.Count;
        manager.Initialize("PluginSystem.DocumentValidation");
        var secondCount = manager.LoadedPlugins.Count;

        // Assert
        Assert.Equal(firstCount, secondCount);
    }

    [Fact]
    public void Initialize_WithInvalidAssembly_ThrowsInvalidOperationException()
    {
        // Arrange
        var manager = new PluginManager();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            manager.Initialize("NonExistentAssembly"));
    }

    [Fact]
    public void InitializeFromDirectory_WithValidDirectory_LoadsPlugins()
    {
        // Arrange
        var manager = new PluginManager();
        var assemblyLocation = typeof(PluginSystem.DocumentValidation.Models.Document).Assembly.Location;
        var directory = Path.GetDirectoryName(assemblyLocation);

        // Act
        manager.InitializeFromDirectory(directory!);

        // Assert
        Assert.NotEmpty(manager.LoadedPlugins);
    }

    [Fact]
    public void RegisterPlugin_WithValidPlugin_AddsPlugin()
    {
        // Arrange
        var manager = new PluginManager();
        var plugin = new TestPlugin("TEST-001", new[] { "TestCategory" });

        // Act
        manager.RegisterPlugin(plugin);

        // Assert
        Assert.Single(manager.LoadedPlugins);
        Assert.Equal("TEST-001", manager.LoadedPlugins[0].PluginId);
    }

    [Fact]
    public void RegisterPlugin_WithDuplicateId_ThrowsInvalidOperationException()
    {
        // Arrange
        var manager = new PluginManager();
        var plugin1 = new TestPlugin("TEST-001", new[] { "TestCategory" });
        var plugin2 = new TestPlugin("TEST-001", new[] { "TestCategory" });

        // Act
        manager.RegisterPlugin(plugin1);

        // Assert
        Assert.Throws<InvalidOperationException>(() => manager.RegisterPlugin(plugin2));
    }

    [Fact]
    public void RegisterPlugin_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        var manager = new PluginManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => manager.RegisterPlugin(null!));
    }

    [Fact]
    public async Task ExecuteAsync_WithoutInitialization_ThrowsInvalidOperationException()
    {
        // Arrange
        var manager = new PluginManager();
        var context = new PluginContext("TestCategory", "data");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            manager.ExecuteAsync(context, filter: null, parallel: false));
    }

    [Fact]
    public async Task ExecuteAsync_WithInitializedManager_ExecutesPlugins()
    {
        // Arrange
        var manager = new PluginManager();
        manager.Initialize("PluginSystem.DocumentValidation");
        var document = new PluginSystem.DocumentValidation.Models.Document
        {
            Id = "DOC-001",
            Title = "Test Document",
            Content = "Test content for validation"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await manager.ExecuteAsync(context, filter: null, parallel: false);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalPlugins > 0);
    }

    [Fact]
    public async Task ExecuteAsync_WithParallelTrue_ExecutesInParallel()
    {
        // Arrange
        var manager = new PluginManager();
        manager.Initialize("PluginSystem.DocumentValidation");
        var document = new PluginSystem.DocumentValidation.Models.Document
        {
            Id = "DOC-001",
            Title = "Test Document",
            Content = "Test content"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await manager.ExecuteAsync(context, filter: null, parallel: true);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalPlugins > 0);
    }

    [Fact]
    public async Task ExecuteAsync_WithFilter_ExecutesFilteredPlugins()
    {
        // Arrange
        var manager = new PluginManager();
        manager.RegisterPlugin(new TestPlugin("P1", new[] { "TestCategory" }));
        manager.RegisterPlugin(new TestPlugin("P2", new[] { "TestCategory" }));
        var context = new PluginContext("TestCategory", "data");

        // Act
        var result = await manager.ExecuteAsync(context, filter: p => p.PluginId == "P1");

        // Assert
        Assert.Equal(1, result.TotalPlugins);
    }

    [Fact]
    public void GetPluginsByCategory_ReturnsMatchingPlugins()
    {
        // Arrange
        var manager = new PluginManager();
        manager.RegisterPlugin(new TestPlugin("P1", new[] { "Category1" }));
        manager.RegisterPlugin(new TestPlugin("P2", new[] { "Category2" }));
        manager.RegisterPlugin(new TestPlugin("P3", new[] { "Category1" }));

        // Act
        var plugins = manager.GetPluginsByCategory("Category1").ToList();

        // Assert
        Assert.Equal(2, plugins.Count);
        Assert.All(plugins, p => Assert.Contains("Category1", p.Categories));
    }

    [Fact]
    public void GetPluginsByCategory_CaseInsensitive()
    {
        // Arrange
        var manager = new PluginManager();
        manager.RegisterPlugin(new TestPlugin("P1", new[] { "TestCategory" }));

        // Act
        var plugins = manager.GetPluginsByCategory("testcategory").ToList();

        // Assert
        Assert.Single(plugins);
    }

    [Fact]
    public void GetPluginById_ReturnsMatchingPlugin()
    {
        // Arrange
        var manager = new PluginManager();
        manager.RegisterPlugin(new TestPlugin("P1", new[] { "Category1" }));
        manager.RegisterPlugin(new TestPlugin("P2", new[] { "Category2" }));

        // Act
        var plugin = manager.GetPluginById("P1");

        // Assert
        Assert.NotNull(plugin);
        Assert.Equal("P1", plugin.PluginId);
    }

    [Fact]
    public void GetPluginById_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var manager = new PluginManager();
        manager.RegisterPlugin(new TestPlugin("P1", new[] { "Category1" }));

        // Act
        var plugin = manager.GetPluginById("NonExistent");

        // Assert
        Assert.Null(plugin);
    }

    [Fact]
    public void GetPluginById_CaseInsensitive()
    {
        // Arrange
        var manager = new PluginManager();
        manager.RegisterPlugin(new TestPlugin("TEST-001", new[] { "Category1" }));

        // Act
        var plugin = manager.GetPluginById("test-001");

        // Assert
        Assert.NotNull(plugin);
    }

    [Fact]
    public void Reset_ClearsPluginsAndResets()
    {
        // Arrange
        var manager = new PluginManager();
        manager.Initialize("PluginSystem.DocumentValidation");
        var initialCount = manager.LoadedPlugins.Count;

        // Act
        manager.Reset();

        // Assert
        Assert.Empty(manager.LoadedPlugins);
        Assert.NotEqual(initialCount, manager.LoadedPlugins.Count);
    }

    [Fact]
    public void Reset_AllowsReinitialization()
    {
        // Arrange
        var manager = new PluginManager();
        manager.Initialize("PluginSystem.DocumentValidation");
        manager.Reset();

        // Act
        manager.Initialize("PluginSystem.DocumentValidation");

        // Assert
        Assert.NotEmpty(manager.LoadedPlugins);
    }

    private class TestPlugin : PluginBase
    {
        public TestPlugin(string id, string[] categories)
        {
            PluginId = id;
            Categories = categories;
        }

        public override string PluginId { get; }
        public override string Name => PluginId;
        public override IReadOnlyList<string> Categories { get; }

        public override Task<PluginResult> ExecuteAsync(IPluginContext context)
        {
            return Task.FromResult(Success("Test executed"));
        }
    }
}

