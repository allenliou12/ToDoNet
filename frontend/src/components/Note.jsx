import { useNotesContext } from '../hooks/useNotesContext';
import Icon from '@mdi/react';
import EditingNote from './Editing';
import { mdiDeleteEmptyOutline, mdiSquareEditOutline } from '@mdi/js';
import Fab from '@mui/material/Fab';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { yellow } from '@mui/material/colors';
import { useState } from 'react';

const color = yellow[800];

const theme = createTheme({
  palette: {
    primary: {
      main: color,
    },
  },
});

function Note(props) {
  const { dispatch } = useNotesContext();
  const [isEditing, setIsEditing] = useState(false);

  const handleDelete = async () => {
    const response = await fetch(`http://localhost:5213/api/todos/${props.id}`, {
      method: 'DELETE',
    });

    if (response.ok) {
      const json = await response.json();
      dispatch({ type: 'DELETE_NOTE', payload: json });
      console.log('Note deleted!', json);
    } else {
      console.error('Failed to delete note');
    }
  };

  const handleEdit = () => setIsEditing(true);
  const handleClose = () => setIsEditing(false);

  return (
    <ThemeProvider theme={theme}>
      {isEditing ? (
        <EditingNote
          key={props.id}
          id={props.id}
          title={props.title}
          description={props.description}
          createdAt={props.createdAt}
          OnClose={handleClose}
        />
      ) : (
        <div className="note">
          <h1>{props.title}</h1>
          <p>{props.description}</p>
          <p className="timestamp">{props.createdAt}</p>

          <Fab
            sx={{
              '&:hover': {
                backgroundColor: 'primary.main',
              },
            }}
            onClick={handleDelete}
          >
            <Icon path={mdiDeleteEmptyOutline} size={1} />
          </Fab>

          <Fab
            sx={{
              '&:hover': {
                backgroundColor: 'primary.main',
              },
            }}
            onClick={handleEdit}
          >
            <Icon path={mdiSquareEditOutline} size={1} />
          </Fab>
        </div>
      )}
    </ThemeProvider>
  );
}

export default Note;
