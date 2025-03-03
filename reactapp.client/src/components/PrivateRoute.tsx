import { useSelector } from 'react-redux';
import { Navigate, Outlet } from 'react-router-dom';
import { RootState } from '../app/store'; // Importer RootState

const PrivateRoute = () => {
     // Utiliser RootState pour typer state
     const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);
      // Rediriger vers /login si l'utilisateur n'est pas authentifié
    return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};

export default PrivateRoute;