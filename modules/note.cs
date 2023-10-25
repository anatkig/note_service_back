using Azure;
using Azure.Data.Tables;

public class Note : ITableEntity
{
    public string PartitionKey { get; set; } = "NotePartitionKey";
    public required string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Priority { get; set; }
    public required string Status { get; set; }
}
