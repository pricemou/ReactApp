import { useDispatch } from 'react-redux';
import { logout } from '../features/authSlice'; // Importer l'action de déconnexion
import { useNavigate } from 'react-router-dom';

const LogoutButton = () => {
    const dispatch = useDispatch(); // Hook pour dispatcher des actions Redux
    const navigate = useNavigate();

    const handleLogout = () => {
        dispatch(logout()); // Dispatcher l'action de déconnexion
        navigate('/login'); // Rediriger vers la page de connexion
    };

    return (
        <button onClick={handleLogout}>Déconnexion</button>
    );
};

export default LogoutButton;