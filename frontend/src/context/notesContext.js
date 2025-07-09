// A global state management

import { createContext, useReducer, useContext } from "react";

// Create and export a context to use as a global state
export const NotesContext = createContext(); // A global store to hold all note-related data

// Custom hook for using the Notes context
export const useNotesContext = () => {
    const context = useContext(NotesContext);
    if (!context) {
        throw Error('useNotesContext must be used inside a NotesContextProvider');
    }
    return context;
};

// Reducer for managing note-related actions
export const notesReducer = (state, action) => {
    switch (action.type) {
        case "SET_NOTES":
            return {
                ...state,
                notes: action.payload,
                allNotes: action.payload
            };

        case "CREATE_NOTE":
            return {
                ...state,
                notes: [action.payload, ...state.notes],
                allNotes: [action.payload, ...state.allNotes]
            };

        case "DELETE_NOTE":
            return {
                ...state,
                notes: state.notes.filter((note) => note.id !== action.payload.id),
                allNotes: state.allNotes.filter((note) => note.id !== action.payload.id)
            };

        case "UPDATE_NOTE":
            const updatedNote = action.payload;
            const updatedNotes = state.notes.map((note) =>
                note.id === updatedNote.id ? updatedNote : note
            );
            const updatedAllNotes = state.allNotes.map((note) =>
                note.id === updatedNote.id ? updatedNote : note
            );
            return {
                ...state,
                notes: updatedNotes,
                allNotes: updatedAllNotes
            };

        case "SEARCH_NOTES":
            const searchTerm = action.payload.toLowerCase();
            const filteredNotes = state.allNotes.filter(note =>
                note.title.toLowerCase().includes(searchTerm) ||
                note.description.toLowerCase().includes(searchTerm)
            );
            return {
                ...state,
                notes: filteredNotes
            };

        default:
            return state;
    }
};

// Provider component
export const NotesContextProvider = ({ children }) => {
    const [state, dispatch] = useReducer(notesReducer, {
        notes: [],
        allNotes: []
    });

    return (
        <NotesContext.Provider value={{ ...state, dispatch }}>
            {children}
        </NotesContext.Provider>
    );
};
