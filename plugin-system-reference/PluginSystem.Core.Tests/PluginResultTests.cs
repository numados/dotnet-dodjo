namespace PluginSystem.Core.Tests;

public class PluginResultTests
{
    [Fact]
    public void Success_WithMessage_CreatesSuccessResult()
    {
        // Arrange
        var message = "Operation successful";

        // Act
        var result = PluginResult.Success(message);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultSeverity.Info, result.Severity);
        Assert.Equal(message, result.Message);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public void Success_WithData_IncludesData()
    {
        // Arrange
        var data = new { Count = 5 };

        // Act
        var result = PluginResult.Success("Success", data);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(data, result.Data);
    }

    [Fact]
    public void Success_WithoutParameters_CreatesSuccessResult()
    {
        // Act
        var result = PluginResult.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultSeverity.Info, result.Severity);
        Assert.Null(result.Message);
    }

    [Fact]
    public void Failure_WithMessage_CreatesFailureResult()
    {
        // Arrange
        var message = "Operation failed";

        // Act
        var result = PluginResult.Failure(message);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultSeverity.Error, result.Severity);
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void Failure_WithCriticalSeverity_SetsSeverityCorrectly()
    {
        // Arrange
        var message = "Critical failure";

        // Act
        var result = PluginResult.Failure(message, ResultSeverity.Critical);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultSeverity.Critical, result.Severity);
    }

    [Fact]
    public void WithIssues_WithErrorIssues_CreatesFailureResult()
    {
        // Arrange
        var issues = new List<PluginIssue>
        {
            new() { Severity = ResultSeverity.Error, Code = "ERR001", Message = "Error 1" },
            new() { Severity = ResultSeverity.Warning, Code = "WARN001", Message = "Warning 1" }
        };

        // Act
        var result = PluginResult.WithIssues(issues);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultSeverity.Error, result.Severity);
        Assert.Equal(2, result.Issues.Count);
    }

    [Fact]
    public void WithIssues_WithOnlyWarnings_CreatesSuccessResult()
    {
        // Arrange
        var issues = new List<PluginIssue>
        {
            new() { Severity = ResultSeverity.Warning, Code = "WARN001", Message = "Warning 1" },
            new() { Severity = ResultSeverity.Info, Code = "INFO001", Message = "Info 1" }
        };

        // Act
        var result = PluginResult.WithIssues(issues);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultSeverity.Warning, result.Severity);
        Assert.Equal(2, result.Issues.Count);
    }

    [Fact]
    public void WithIssues_WithMessage_IncludesMessage()
    {
        // Arrange
        var issues = new List<PluginIssue>
        {
            new() { Severity = ResultSeverity.Warning, Code = "WARN001", Message = "Warning" }
        };
        var message = "Found issues";

        // Act
        var result = PluginResult.WithIssues(issues, message);

        // Assert
        Assert.Equal(message, result.Message);
    }

    [Fact]
    public void WithIssues_WithEmptyList_CreatesSuccessResult()
    {
        // Arrange
        var issues = new List<PluginIssue>();

        // Act
        var result = PluginResult.WithIssues(issues);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Issues);
    }

    [Fact]
    public void PluginIssue_WithMetadata_StoresMetadataCorrectly()
    {
        // Arrange
        var metadata = new Dictionary<string, object>
        {
            { "Field", "Email" },
            { "Value", "invalid@" }
        };

        // Act
        var issue = new PluginIssue
        {
            Severity = ResultSeverity.Error,
            Code = "INVALID_EMAIL",
            Message = "Invalid email format",
            Location = "User.Email",
            Metadata = metadata
        };

        // Assert
        Assert.Equal(ResultSeverity.Error, issue.Severity);
        Assert.Equal("INVALID_EMAIL", issue.Code);
        Assert.Equal("Invalid email format", issue.Message);
        Assert.Equal("User.Email", issue.Location);
        Assert.NotNull(issue.Metadata);
        Assert.Equal(2, issue.Metadata.Count);
        Assert.Equal("Email", issue.Metadata["Field"]);
    }

    [Fact]
    public void ResultSeverity_HasCorrectValues()
    {
        // Assert
        Assert.Equal(0, (int)ResultSeverity.Info);
        Assert.Equal(1, (int)ResultSeverity.Warning);
        Assert.Equal(2, (int)ResultSeverity.Error);
        Assert.Equal(3, (int)ResultSeverity.Critical);
    }
}

