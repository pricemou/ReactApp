import  { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { loginSuccess } from '../features/authSlice'; // Importer l'action de connexion

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const dispatch = useDispatch(); // Hook pour dispatcher des actions Redux
    const navigate = useNavigate();

   
    
    const handleLogin = async () => {
        const raw = JSON.stringify({
            "nomUtilisateur": "user_residence1",
            "motDePasse": "residence789"
          });
        
        const response = await fetch('http://localhost:5295/api/Auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: raw,
        });
        
        if (response.ok) {
            const data = await response.json();
            // localStorage.setItem('user', JSON.stringify(data.token)); // Stocker le token
            // localStorage.setItem('userEmail', email);  // Stocker l'e-mail
            
            // Dispatcher l'action de connexion réussie
            dispatch(loginSuccess(data.token));
            console.log(data);
            navigate('/dashboard'); // Rediriger vers le tableau de bord
            
        } else {
            alert('Email ou mot de passe incorrect');
        }
    };

    return (
        <div>
            <h2>Connexion</h2>
            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button onClick={handleLogin}>Login</button>
        </div>
    );
};

export default LoginPage;