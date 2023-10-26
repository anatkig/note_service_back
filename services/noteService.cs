using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Options;

public class StorageConfig
{
    public string? ConnectionString { get; set; }
}

public interface INoteService
{
    public List<Note> GetAllNotes();
    void CreateNote(Note note);
    void UpdateNote(Note note);
    void DeleteNote(string id);
    Note? GetNoteById(string id);
    public bool NoteExists(string projectName);
}

public class NoteService : INoteService
{
    private readonly TableClient _noteTableClient;
    private const string NotePartitionKey = "NotePartitionKey";

    public NoteService(IOptions<StorageConfig> storageConfig)
    {
        var connectionString = storageConfig.Value.ConnectionString;
        _noteTableClient = new TableClient(connectionString, "notes");
        _noteTableClient.CreateIfNotExists();
    }

    public List<Note> GetAllNotes()
    {
        var queryResultsFilter = _noteTableClient.Query<Note>(filter: $"PartitionKey eq '{NotePartitionKey}'");
        return queryResultsFilter.ToList();
    }
    public void CreateNote(Note note)
    {

        if (string.IsNullOrEmpty(note.RowKey))
        {
            note.RowKey = note.Id;
        }

        _noteTableClient.AddEntity(note);
    }

    public void UpdateNote(Note note)
    {
        note.PartitionKey = NotePartitionKey;

        _noteTableClient.UpdateEntity(note, ETag.All, TableUpdateMode.Merge);
    }

    public Note? GetNoteById(string id)
    {
        try
        {
            return _noteTableClient.GetEntity<Note>(NotePartitionKey, id).Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public void DeleteNote(string id)
    {
        _noteTableClient.DeleteEntity(NotePartitionKey, id);
    }

    public bool NoteExists(string noteTitle)
    {

        var projects = _noteTableClient.Query<Note>(filter: $"PartitionKey eq '{NotePartitionKey}'");
        return projects.Any(p => p.Title == noteTitle);
    }
}
