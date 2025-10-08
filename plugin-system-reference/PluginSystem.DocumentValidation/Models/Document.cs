namespace PluginSystem.DocumentValidation.Models;

/// <summary>
/// Represents a document to be validated.
/// This is the domain model that plugins will process.
/// </summary>
public class Document
{
    /// <summary>
    /// Unique identifier for the document.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Title of the document.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Content of the document.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Author of the document.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Date when the document was created.
    /// </summary>
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// Date when the document was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Document type/category.
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Tags associated with the document.
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Additional metadata.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// File size in bytes (if applicable).
    /// </summary>
    public long? FileSizeBytes { get; set; }

    /// <summary>
    /// File format/extension.
    /// </summary>
    public string? FileFormat { get; set; }
}

