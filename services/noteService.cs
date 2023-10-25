using Azure;
using Azure.Data.Tables;
using System;

public class NoteService
{
    private readonly TableClient _tableClient;
    private const string NotePartitionKey = "NotePartitionKey";

    public NoteService(string connectionString)
    {
        _tableClient = new TableClient(connectionString, "Notes");
        _tableClient.CreateIfNotExists();
    }

    public void CreateNote(Note note)
    {
        note.PartitionKey = NotePartitionKey;

        if (string.IsNullOrEmpty(note.RowKey))
        {
            note.RowKey = note.Id;
        }

        _tableClient.AddEntity(note);
    }

    public void UpdateNote(Note note)
    {
        note.PartitionKey = NotePartitionKey;

        _tableClient.UpdateEntity(note, ETag.All, TableUpdateMode.Merge);
    }

    public void CompleteNote(Note note)
    {
        note.PartitionKey = NotePartitionKey;

        note.Status = "Completed";
        UpdateNote(note);
    }

    public void DeleteNote(string id)
    {
        _tableClient.DeleteEntity(NotePartitionKey, id);
    }
}
