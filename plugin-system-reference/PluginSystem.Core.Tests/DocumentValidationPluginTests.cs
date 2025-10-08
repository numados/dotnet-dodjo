using PluginSystem.DocumentValidation.Models;
using PluginSystem.DocumentValidation.Plugins;

namespace PluginSystem.Core.Tests;

public class RequiredFieldsValidationPluginTests
{
    [Fact]
    public async Task ExecuteAsync_WithAllRequiredFields_ReturnsSuccess()
    {
        // Arrange
        var plugin = new RequiredFieldsValidationPlugin();
        var document = new Document
        {
            Id = "DOC-001",
            Title = "Test Document",
            Content = "Test content",
            Author = "Test Author"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public async Task ExecuteAsync_WithMissingId_ReturnsError()
    {
        // Arrange
        var plugin = new RequiredFieldsValidationPlugin();
        var document = new Document
        {
            Id = "",
            Title = "Test Document",
            Content = "Test content"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.Code == "REQUIRED_FIELD_MISSING" && i.Location == "Document.Id");
    }

    [Fact]
    public async Task ExecuteAsync_WithMissingAuthor_ReturnsWarning()
    {
        // Arrange
        var plugin = new RequiredFieldsValidationPlugin();
        var document = new Document
        {
            Id = "DOC-001",
            Title = "Test Document",
            Content = "Test content",
            Author = ""
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess); // Warnings don't fail
        Assert.Contains(result.Issues, i => i.Code == "RECOMMENDED_FIELD_MISSING");
    }
}

public class ContentLengthValidationPluginTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidLengths_ReturnsSuccess()
    {
        // Arrange
        var plugin = new ContentLengthValidationPlugin();
        var document = new Document
        {
            Title = "Valid Title",
            Content = "This is valid content with sufficient length."
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public async Task ExecuteAsync_WithTitleTooLong_ReturnsError()
    {
        // Arrange
        var plugin = new ContentLengthValidationPlugin();
        var document = new Document
        {
            Title = new string('A', 250),
            Content = "Valid content"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.Code == "TITLE_TOO_LONG");
    }

    [Fact]
    public async Task ExecuteAsync_WithContentTooShort_ReturnsWarning()
    {
        // Arrange
        var plugin = new ContentLengthValidationPlugin();
        var document = new Document
        {
            Title = "Title",
            Content = "Short"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess); // Warning doesn't fail
        Assert.Contains(result.Issues, i => i.Code == "CONTENT_TOO_SHORT");
    }

    [Fact]
    public async Task ExecuteAsync_WithContentTooLong_ReturnsError()
    {
        // Arrange
        var plugin = new ContentLengthValidationPlugin();
        var document = new Document
        {
            Title = "Title",
            Content = new string('A', 150000)
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.Code == "CONTENT_TOO_LONG");
    }
}

public class DateValidationPluginTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidDates_ReturnsSuccess()
    {
        // Arrange
        var plugin = new DateValidationPlugin();
        var document = new Document
        {
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            ModifiedDate = DateTime.UtcNow.AddDays(-5)
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public async Task ExecuteAsync_WithFutureCreatedDate_ReturnsError()
    {
        // Arrange
        var plugin = new DateValidationPlugin();
        var document = new Document
        {
            CreatedDate = DateTime.UtcNow.AddDays(10)
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.Code == "INVALID_CREATED_DATE");
    }

    [Fact]
    public async Task ExecuteAsync_WithModifiedBeforeCreated_ReturnsError()
    {
        // Arrange
        var plugin = new DateValidationPlugin();
        var document = new Document
        {
            CreatedDate = DateTime.UtcNow.AddDays(-5),
            ModifiedDate = DateTime.UtcNow.AddDays(-10)
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.Code == "INVALID_DATE_SEQUENCE");
    }

    [Fact]
    public async Task ExecuteAsync_WithVeryOldDocument_ReturnsWarning()
    {
        // Arrange
        var plugin = new DateValidationPlugin();
        var document = new Document
        {
            CreatedDate = DateTime.UtcNow.AddYears(-15)
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess); // Warning doesn't fail
        Assert.Contains(result.Issues, i => i.Code == "OLD_DOCUMENT");
    }
}

public class MetadataValidationPluginTests
{
    [Fact]
    public async Task ExecuteAsync_WithValidMetadata_ReturnsSuccess()
    {
        // Arrange
        var plugin = new MetadataValidationPlugin();
        var document = new Document
        {
            Tags = new List<string> { "tag1", "tag2" },
            FileSizeBytes = 1024,
            DocumentType = "Article"
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public async Task ExecuteAsync_WithTooManyTags_ReturnsWarning()
    {
        // Arrange
        var plugin = new MetadataValidationPlugin();
        var document = new Document
        {
            Tags = Enumerable.Range(1, 15).Select(i => $"tag{i}").ToList()
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.True(result.IsSuccess); // Warning doesn't fail
        Assert.Contains(result.Issues, i => i.Code == "TOO_MANY_TAGS");
    }

    [Fact]
    public async Task ExecuteAsync_WithEmptyTag_ReturnsWarning()
    {
        // Arrange
        var plugin = new MetadataValidationPlugin();
        var document = new Document
        {
            Tags = new List<string> { "tag1", "", "tag2" }
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.Contains(result.Issues, i => i.Code == "EMPTY_TAG");
    }

    [Fact]
    public async Task ExecuteAsync_WithFileTooLarge_ReturnsError()
    {
        // Arrange
        var plugin = new MetadataValidationPlugin();
        var document = new Document
        {
            FileSizeBytes = 100 * 1024 * 1024 // 100 MB
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Issues, i => i.Code == "FILE_TOO_LARGE");
    }

    [Fact]
    public async Task ExecuteAsync_WithMissingDocumentType_ReturnsWarning()
    {
        // Arrange
        var plugin = new MetadataValidationPlugin();
        var document = new Document
        {
            DocumentType = ""
        };
        var context = new PluginContext("DocumentValidation", document);

        // Act
        var result = await plugin.ExecuteAsync(context);

        // Assert
        Assert.Contains(result.Issues, i => i.Code == "MISSING_DOCUMENT_TYPE");
    }
}

