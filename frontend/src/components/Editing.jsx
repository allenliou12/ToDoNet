import Icon from '@mdi/react';
import { mdiContentSavePlusOutline, mdiClose } from '@mdi/js';
import Fab from '@mui/material/Fab';
import { useState } from 'react';
import { useNotesContext } from '../hooks/useNotesContext';

const EditingNote = (props) => {
  const { dispatch } = useNotesContext();

  const [updatedNote, setUpdatedNote] = useState({
    id: props.id,
    title: props.title,
    description: props.description,
    error: null
  });

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setUpdatedNote(prevNote => ({
      ...prevNote,
      [name]: value
    }));
  };

  const handleSubmit = async () => {
    const response = await fetch(`http://localhost:5213/api/todos/${props.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(updatedNote)
    });

    const json = await response.json();

    if (response.ok) {
      dispatch({ type: "UPDATE_NOTE", payload: json });
      props.OnClose();
    } else {
      console.error("Failed to update note", json);
    }
  };

  return (
    <div className="note">
      {/* Close Button */}
      <Fab onClick={props.OnClose} sx={{ '&:hover': { backgroundColor: 'primary.main' } }}>
        <Icon path={mdiClose} size={1} />
      </Fab>

      <form className='edit-note' onSubmit={(e) => e.preventDefault()}>
        <input
          value={updatedNote.title}
          onChange={handleInputChange}
          name="title"
        />
        <textarea
          value={updatedNote.description}
          name="description"
          onChange={handleInputChange}
        />
        {/* Save Edit Button */}
        <Fab onClick={handleSubmit} sx={{ '&:hover': { backgroundColor: 'primary.main' } }}>
          <Icon path={mdiContentSavePlusOutline} size={1} />
        </Fab>
      </form>
    </div>
  );
};

export default EditingNote;
