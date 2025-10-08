namespace PluginSystem.Core.Tests;

public class PluginContextTests
{
    [Fact]
    public void Constructor_WithCategoryAndData_SetsPropertiesCorrectly()
    {
        // Arrange
        var category = "TestCategory";
        var data = "test data";

        // Act
        var context = new PluginContext(category, data);

        // Assert
        Assert.NotNull(context.ContextId);
        Assert.Equal(category, context.Category);
        Assert.Equal(data, context.Data);
        Assert.Empty(context.Metadata);
    }

    [Fact]
    public void Constructor_WithCustomContextId_UsesProvidedId()
    {
        // Arrange
        var contextId = "custom-id-123";
        var category = "TestCategory";
        var data = "test data";

        // Act
        var context = new PluginContext(category, data, contextId);

        // Assert
        Assert.Equal(contextId, context.ContextId);
    }

    [Fact]
    public void Constructor_WithMetadata_SetsMetadataCorrectly()
    {
        // Arrange
        var category = "TestCategory";
        var data = "test data";
        var metadata = new Dictionary<string, object>
        {
            { "key1", "value1" },
            { "key2", 42 }
        };

        // Act
        var context = new PluginContext(category, data, metadata);

        // Assert
        Assert.Equal(2, context.Metadata.Count);
        Assert.Equal("value1", context.Metadata["key1"]);
        Assert.Equal(42, context.Metadata["key2"]);
    }

    [Fact]
    public void Constructor_WithNullCategory_ThrowsArgumentNullException()
    {
        // Arrange
        string? category = null;
        var data = "test data";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PluginContext(category!, data));
    }

    [Fact]
    public void Constructor_WithNullData_ThrowsArgumentNullException()
    {
        // Arrange
        var category = "TestCategory";
        object? data = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PluginContext(category, data!));
    }

    [Fact]
    public void GetData_WithCorrectType_ReturnsTypedData()
    {
        // Arrange
        var testData = new TestDataClass { Value = "test" };
        var context = new PluginContext("TestCategory", testData);

        // Act
        var result = context.GetData<TestDataClass>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test", result.Value);
    }

    [Fact]
    public void GetData_WithIncorrectType_ThrowsInvalidCastException()
    {
        // Arrange
        var testData = "string data";
        var context = new PluginContext("TestCategory", testData);

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => context.GetData<TestDataClass>());
    }

    [Fact]
    public void GetMetadata_WithExistingKey_ReturnsValue()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "key1", "value1" } };
        var context = new PluginContext("TestCategory", "data", metadata);

        // Act
        var result = context.GetMetadata<string>("key1");

        // Assert
        Assert.Equal("value1", result);
    }

    [Fact]
    public void GetMetadata_WithNonExistingKey_ReturnsDefault()
    {
        // Arrange
        var context = new PluginContext("TestCategory", "data");

        // Act
        var result = context.GetMetadata<string>("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetMetadata_WithWrongType_ReturnsDefault()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "key1", "string value" } };
        var context = new PluginContext("TestCategory", "data", metadata);

        // Act
        var result = context.GetMetadata<int>("key1");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void SetMetadata_AddsNewMetadata()
    {
        // Arrange
        var context = new PluginContext("TestCategory", "data");

        // Act
        context.SetMetadata("newKey", "newValue");

        // Assert
        Assert.Equal("newValue", context.GetMetadata<string>("newKey"));
    }

    [Fact]
    public void SetMetadata_UpdatesExistingMetadata()
    {
        // Arrange
        var metadata = new Dictionary<string, object> { { "key1", "oldValue" } };
        var context = new PluginContext("TestCategory", "data", metadata);

        // Act
        context.SetMetadata("key1", "newValue");

        // Assert
        Assert.Equal("newValue", context.GetMetadata<string>("key1"));
    }

    private class TestDataClass
    {
        public string Value { get; set; } = string.Empty;
    }
}

