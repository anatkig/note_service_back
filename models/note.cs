using Azure;
using Azure.Data.Tables;
using System.Text.Json;

public class Note : ITableEntity
{

    public Note()
    {
        PartitionKey = partitionKey;
        Id = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
        Status = string.Empty;
        Priority = string.Empty;
    }
    const string partitionKey = "NotePartitionKey";
    public string PartitionKey { get; set; }
    public string RowKey { get { return Id; } set { Id = value; } }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
    public string Status { get; set; }
}
