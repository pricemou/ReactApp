import { configureStore } from '@reduxjs/toolkit';
import authReducer from '../features/authSlice'; // Importer le slice d'authentification


// Charger l'état initial depuis localStorage
const preloadedState = {
    auth: {
        token: localStorage.getItem('token') || null,
        isAuthenticated: !!localStorage.getItem('token'),
    },
};

const store = configureStore({
    reducer: {
        auth: authReducer,
    },
    preloadedState, // Utiliser l'état initial chargé
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;