using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add SQLite DbContext

builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlite("Data Source=notes.db"));

var app = builder.Build();

// Endpoints

app.MapGet("/api/notes", async (NotesDbContext db) =>
{
    var notes = await db.Notes.ToListAsync();
    return Results.Ok(notes);
});

app.MapPost("/api/notes", async (NoteDto dto, NotesDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Text))
        return Results.BadRequest("Text is required.");

    var note = new Note { Text = dto.Text };
    db.Notes.Add(note);
    await db.SaveChangesAsync();
    return Results.Created($"/api/notes/{note.Id}", note);
});

app.MapDelete("/api/notes/{id:int}", async (int id, NotesDbContext db) =>
{
    var note = await db.Notes.FindAsync(id);
    if (note == null)
        return Results.NotFound();

    db.Notes.Remove(note);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

// Models

class Note
{
    public int Id { get; set; } // Primary key
    public string Text { get; set; } = "";
}

record NoteDto(string Text);

// DbContext

class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
}
