import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { useLoginMutation } from '../features/authApi'; // Importer la mutation
import { loginSuccess } from '../features/authSlice';

const LoginPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const [login, { isLoading, error }] = useLoginMutation(); // Utilisation de la mutation login

  const handleLogin = async () => {
    try {
      const credentials = { nomUtilisateur: email, motDePasse: password };
      const response = await login(credentials).unwrap(); // Exécution de la mutation

      // Si la connexion réussie, dispatcher l'action et rediriger
      dispatch(loginSuccess(response.token));
      navigate('/dashboard'); // Rediriger vers le dashboard
    } catch (err) {
      console.error(err);
      alert('Email ou mot de passe incorrect');
    }
  };

  return (
    <div>
      <h2>Connexion</h2>
      <input
        type="text"
        placeholder="Nom Utilisateur"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <input
        type="password"
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button onClick={handleLogin} disabled={isLoading}>
        {isLoading ? 'Chargement...' : 'Login'}
      </button>
      {error && <p style={{ color: 'red' }}>Erreur : {error.message}</p>}
    </div>
  );
};

export default LoginPage;
