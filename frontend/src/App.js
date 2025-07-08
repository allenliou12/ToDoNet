import Header from './components/Header';
import Footer from './components/Footer';
import Note from './components/Note';
import CreateArea from './components/CreateArea';
import { useEffect } from 'react';
import { useNotesContext } from './hooks/useNotesContext';

function App() {
  const { notes, dispatch } = useNotesContext();

  useEffect(() => {
    const fetchNotes = async () => {
      const response = await fetch("http://localhost:5213/api/todos"); // Updated URL for .NET
      const json = await response.json();

      if (response.ok) {
        // Update the context with fetched notes
        dispatch({ type: "SET_NOTES", payload: json });
      }
    };

    fetchNotes();
  }, [dispatch]);

  return (
    <div>
      <Header />
      <CreateArea />
      {notes &&
        notes.map((note) => (
          <Note
            key={note.id}               // .NET uses `id` not `_id`
            id={note.id}
            title={note.title}
            description={note.description}  // .NET uses `description` instead of `content`
            createdAt={note.createdAt}
          />
        ))}
      <Footer />
    </div>
  );
}

export default App;
