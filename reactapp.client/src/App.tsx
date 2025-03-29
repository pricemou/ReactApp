import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import Dashboard from './pages/Dashboard';
import PrivateRoute from './components/PrivateRoute';
import DashboardUtilisateur from './pages/DashboardUtilisateur';
import DashboardDecodeur from './pages/DashboardDecodeur';


function App() {
    return (
        <Router>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route element={<PrivateRoute />}>
                    <Route path="/dashboard" element={<Dashboard />} />
                    <Route path="/utilisateur" element={<DashboardUtilisateur />} />
                    <Route path="/decodeur" element={<DashboardDecodeur />} />
                </Route>
            </Routes>
        </Router>
    );
}

export default App;