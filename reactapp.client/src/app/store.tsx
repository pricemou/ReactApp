import { configureStore } from '@reduxjs/toolkit';
import authReducer from '../features/authSlice'; // Importer le slice d'authentification
import { authApi } from '../features/authApi'; // Importer authApi

// Charger l'état initial depuis localStorage
const preloadedState = {
  auth: {
    token: localStorage.getItem('token') || null,
    isAuthenticated: !!localStorage.getItem('token'),
  },
};

const store = configureStore({
  reducer: {
    auth: authReducer,                // Reducer pour l'authentification
    [authApi.reducerPath]: authApi.reducer, // Ajoute le reducer d'authApi
  },
  preloadedState, // L'état initial est chargé depuis le localStorage
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(authApi.middleware), 
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
