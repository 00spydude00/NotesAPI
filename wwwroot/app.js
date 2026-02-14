const API_URL = "/api/notes";

async function loadNotes() {
  const res = await fetch(API_URL);
  const notes = await res.json();

  const ul = document.getElementById("notes");
  ul.innerHTML = "";

  for (const note of notes) {
    const li = document.createElement("li");
    li.textContent = note.text;

    const btn = document.createElement("button");
    btn.textContent = "Delete";
    btn.onclick = () => deleteNote(note.id);

    li.appendChild(btn);
    ul.appendChild(li);
  }
}

async function addNote() {
  const input = document.getElementById("noteInput");

  await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ text: input.value })
  });

  input.value = "";
  loadNotes();
}

async function deleteNote(id) {
  await fetch(`${API_URL}/${id}`, { method: "DELETE" });
  loadNotes();
}

loadNotes();
