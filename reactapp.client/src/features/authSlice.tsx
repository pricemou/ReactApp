import { createSlice } from '@reduxjs/toolkit';

// État initial
const initialState = {
    token: localStorage.getItem('token') || null, // Récupérer le token depuis localStorage
    isAuthenticated: !!localStorage.getItem('token'), // Vérifier si un token existe
};

// Créer un slice pour l'authentification
const authSlice = createSlice({
    name: 'auth', // Nom du slice
    initialState, // État initial
    reducers: {
        // Action pour gérer la connexion réussie
        loginSuccess(state, action) {
            state.token = action.payload; // Stocker le token
            state.isAuthenticated = true; // Mettre à jour l'état de connexion
            localStorage.setItem('token', action.payload); // Sauvegarder le token dans localStorage
        },
        // Action pour gérer la déconnexion
        logout(state) {
            state.token = null; // Supprimer le token
            state.isAuthenticated = false; // Mettre à jour l'état de connexion
            localStorage.removeItem('token'); // Supprimer le token de localStorage
        },
    },
});

// Exporter les actions
export const { loginSuccess, logout } = authSlice.actions;

// Exporter le reducer
export default authSlice.reducer;