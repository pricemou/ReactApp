// src/features/authApi.ts
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const baseUrl = 'https://localhost:7181/api/Auth'; // L'URL de base de l'API

// Définir les endpoints (API slices)
export const authApi = createApi({
    reducerPath: 'authApi',
    baseQuery: fetchBaseQuery({ baseUrl }),
    endpoints: (builder) => ({

        // Requête de login
        login: builder.mutation({
            query: (credentials) => ({
            url: '/login',
            method: 'POST',
            body: credentials,
            }),
        }),

        // Point de terminaison pour récupérer la liste des clients
        getClients: builder.query({
                query: () => '/list-client', // URL pour la récupération des clients
        
        }),

        // Requête pour ajouter un client
        addClient: builder.mutation({
            query: (newClient) => ({
            url: '/creer-client',
            method: 'POST',
            body: newClient,
            }),
        }),

        //pour récupérer les utilisateurs et leurs clients
        getUtilisateursClients: builder.query({
            query: () => '/listes-utilisateurs-clients', // URL pour récupérer les utilisateurs et leurs clients
        }),

        ajouterUtilisateurClient: builder.mutation({
            query: (data) => ({
              url: '/ajouter-utilisateur-client',
              method: 'POST',
              body: data,
              headers: {
                'Content-Type': 'application/json'
              }
            })
          }),

        
       // Supprimer un utilisateur/client par email
       supprimerUtilisateurClient: builder.mutation<void, { email: string }>({
        query: ({ email }) => ({
          url: '/supprimer-utilisateur-client',
          method: 'DELETE',
          headers: {
            'Content-Type': 'application/json',
          },
          body: { email },
        }),
      }),


      // Définir un endpoint pour obtenir la liste des décodeurs
        getDecodeurs: builder.query({
            query: () => '/liste-decodeurs', // L'URL complète devient baseUrl + Auth/liste-decodeurs
        }),

    }),
  });
  
// Exportation des hooks pour utiliser dans les composants
export const {
    useLoginMutation,
    useGetClientsQuery,
    useAddClientMutation,
    useGetUtilisateursClientsQuery,
    useAjouterUtilisateurClientMutation,
    useSupprimerUtilisateurClientMutation,
    useGetDecodeursQuery,
} = authApi;
