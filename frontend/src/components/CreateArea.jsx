import { useState } from "react";
import { useNotesContext } from "../hooks/useNotesContext";
import Icon from '@mdi/react';
import { mdiPlus } from '@mdi/js';
import Fab from '@mui/material/Fab';
import Zoom from '@mui/material/Zoom';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { yellow } from '@mui/material/colors';

const color = yellow[800];

const theme = createTheme({
  palette: {
    primary: {
      main: color
    }
  },
});

function CreateArea(props) {
  const { dispatch } = useNotesContext();

  const [newNote, updateNewNote] = useState({
    title: "",
    description: ""
  });


  const [error, setError] = useState(null);
  const [expanded, setExpanded] = useState(false);

  const expand = () => setExpanded(true);

  const handleChange = (event) => {
    const { name, value } = event.target;
    updateNewNote(prevNote => ({
      ...prevNote,
      [name]: value
    }));
  };

  const handleClick = async () => {
    if (!newNote.title.trim() || !newNote.description.trim()) {
      setError("Title and description cannot be empty.");
      return;
    }

    try {
      const response = await fetch("http://localhost:5213/api/todos", {
        method: "POST",
        body: JSON.stringify(newNote),
        headers: {
          "Content-Type": "application/json"
        }
      });

      const json = await response.json();

      if (response.ok) {
        dispatch({ type: "CREATE_NOTE", payload: json });
        updateNewNote({ title: "", description: "" });
        setError(null);
      } else {
        setError(json.error || "Failed to create note.");
      }
    } catch (err) {
      console.error("Network error:", err);
      setError("Network error - please try again.");
    }
  };

  return (
    <ThemeProvider theme={theme}>
      <div>
        <form className="create-note" onSubmit={(e) => e.preventDefault()}>
          {expanded && (
            <input
              onChange={handleChange}
              name="title"
              placeholder="Title"
              value={newNote.title}
            />
          )}
          <textarea
            onClick={expand}
            onChange={handleChange}
            name="description"
            placeholder="Take a note..."
            rows={expanded ? 3 : 1}
            value={newNote.description}
          />
          <Zoom in={expanded}>
            <Fab
              sx={{ '&:hover': { backgroundColor: 'primary.main' } }}
              onClick={handleClick}
            >
              <Icon path={mdiPlus} size={1} />
            </Fab>
          </Zoom>
          {error && <div className="error">{error}</div>}
        </form>
      </div>
    </ThemeProvider>
  );
}

export default CreateArea;
