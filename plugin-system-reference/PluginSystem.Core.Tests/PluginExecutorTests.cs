using PluginSystem.Core.Services;

namespace PluginSystem.Core.Tests;

public class PluginExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_WithMatchingCategory_ExecutesPlugins()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new TestPlugin("P1", new[] { "TestCategory" }, 10),
            new TestPlugin("P2", new[] { "TestCategory" }, 20)
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteAsync(plugins, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.TotalPlugins);
        Assert.Equal(2, result.SuccessfulPlugins);
        Assert.Equal(0, result.FailedPlugins);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonMatchingCategory_ExecutesNoPlugins()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new TestPlugin("P1", new[] { "OtherCategory" }, 10)
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteAsync(plugins, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.TotalPlugins);
    }

    [Fact]
    public async Task ExecuteAsync_ExecutesInPriorityOrder()
    {
        // Arrange
        var executor = new PluginExecutor();
        var executionOrder = new List<string>();
        var plugins = new List<IPlugin>
        {
            new TestPlugin("P3", new[] { "TestCategory" }, 30, executionOrder),
            new TestPlugin("P1", new[] { "TestCategory" }, 10, executionOrder),
            new TestPlugin("P2", new[] { "TestCategory" }, 20, executionOrder)
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        await executor.ExecuteAsync(plugins, context);

        // Assert
        Assert.Equal(new[] { "P1", "P2", "P3" }, executionOrder);
    }

    [Fact]
    public async Task ExecuteAsync_WithPluginException_CapturesException()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new FailingTestPlugin("P1", new[] { "TestCategory" })
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteAsync(plugins, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(1, result.TotalPlugins);
        Assert.Equal(0, result.SuccessfulPlugins);
        Assert.Equal(1, result.FailedPlugins);
        Assert.Single(result.Exceptions);
    }

    [Fact]
    public async Task ExecuteParallelAsync_WithMatchingCategory_ExecutesPlugins()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new TestPlugin("P1", new[] { "TestCategory" }, 10),
            new TestPlugin("P2", new[] { "TestCategory" }, 20)
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteParallelAsync(plugins, context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.TotalPlugins);
        Assert.Equal(2, result.SuccessfulPlugins);
    }

    [Fact]
    public async Task ExecuteParallelAsync_WithPluginException_CapturesException()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new TestPlugin("P1", new[] { "TestCategory" }, 10),
            new FailingTestPlugin("P2", new[] { "TestCategory" })
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteParallelAsync(plugins, context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(2, result.TotalPlugins);
        Assert.Equal(1, result.SuccessfulPlugins);
        Assert.Equal(1, result.FailedPlugins);
        Assert.Single(result.Exceptions);
    }

    [Fact]
    public async Task ExecuteAsync_CollectsAllIssues()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new IssueGeneratingPlugin("P1", new[] { "TestCategory" }, 2),
            new IssueGeneratingPlugin("P2", new[] { "TestCategory" }, 3)
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteAsync(plugins, context);

        // Assert
        Assert.Equal(5, result.AllIssues.Count);
    }

    [Fact]
    public async Task ExecuteAsync_TracksExecutionTime()
    {
        // Arrange
        var executor = new PluginExecutor();
        var plugins = new List<IPlugin>
        {
            new TestPlugin("P1", new[] { "TestCategory" }, 10)
        };
        var context = new PluginContext("TestCategory", "test data");

        // Act
        var result = await executor.ExecuteAsync(plugins, context);

        // Assert
        Assert.Single(result.PluginResults);
        Assert.True(result.PluginResults[0].ExecutionTime.TotalMilliseconds >= 0);
    }

    private class TestPlugin : PluginBase
    {
        private readonly List<string>? _executionOrder;

        public TestPlugin(string id, string[] categories, int priority, List<string>? executionOrder = null)
        {
            _executionOrder = executionOrder;
            PluginId = id;
            Categories = categories;
            Priority = priority;
        }

        public override string PluginId { get; }
        public override string Name => PluginId;
        public override IReadOnlyList<string> Categories { get; }
        public override int Priority { get; }

        public override Task<PluginResult> ExecuteAsync(IPluginContext context)
        {
            _executionOrder?.Add(PluginId);
            return Task.FromResult(Success("Test plugin executed"));
        }
    }

    private class FailingTestPlugin : PluginBase
    {
        public FailingTestPlugin(string id, string[] categories)
        {
            PluginId = id;
            Categories = categories;
        }

        public override string PluginId { get; }
        public override string Name => PluginId;
        public override IReadOnlyList<string> Categories { get; }

        public override Task<PluginResult> ExecuteAsync(IPluginContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    private class IssueGeneratingPlugin : PluginBase
    {
        private readonly int _issueCount;

        public IssueGeneratingPlugin(string id, string[] categories, int issueCount)
        {
            PluginId = id;
            Categories = categories;
            _issueCount = issueCount;
        }

        public override string PluginId { get; }
        public override string Name => PluginId;
        public override IReadOnlyList<string> Categories { get; }

        public override Task<PluginResult> ExecuteAsync(IPluginContext context)
        {
            var issues = Enumerable.Range(1, _issueCount)
                .Select(i => CreateIssue(ResultSeverity.Warning, $"ISSUE{i}", $"Issue {i}"))
                .ToList();

            return Task.FromResult(WithIssues(issues));
        }
    }
}

