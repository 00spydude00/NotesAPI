using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<NotesDbContext>(options =>
		options.UseSqlite("DataSource = notes.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    // app.MapOpenApi();
// }

// app.UseHttpsRedirection();

app.MapGet("/api/notes", async (NotesDbContex db) =>
{
	var notes = await db.Notes.ToListAsync();
	return Results.Ok(notes);
})
.WithName("GetNotes");

app.Run();
// class note
// {
	// public int Id { get; set; }
	// public string Note { get; set; } = "";
// }
class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options)
        : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
}
