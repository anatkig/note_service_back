using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var notes = _noteService.GetAllNotes();
        return Ok(notes);
    }

    // GET: api/notes/{id}
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var note = _noteService.GetNoteById(id);
        if (note == null)
        {
            return NotFound();
        }
        return Ok(note);
    }

    // POST: api/notes
    [HttpPost]
    public IActionResult Create(Note note)
    {
        bool noteExists = _noteService.NoteExists(note.Title);

        if (noteExists)
        {
            return Conflict(new { message = "A note with the same name already exists." });
        }

        _noteService.CreateNote(note);
        return CreatedAtAction(nameof(Get), new { id = note.Id }, note);
    }

    // PUT: api/notes/{id}
    [HttpPut("{id}")]
    public IActionResult Update(string id, Note note)
    {
        if (id != note.Id)
        {
            return BadRequest();
        }

        _noteService.UpdateNote(note);
        return NoContent();
    }


    // DELETE: api/notes/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        _noteService.DeleteNote(id);
        return NoContent();
    }

}
