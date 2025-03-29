import React, { useRef, useState } from 'react';
import Sidenav from '../components/Sidenav';
import { useGetUtilisateursClientsQuery, useAjouterUtilisateurClientMutation, useSupprimerUtilisateurClientMutation } from '../features/authApi';
import DashboardNavbar from '../components/DashboardNavbar';

// Interface pour les utilisateurs et clients
interface Utilisateur {
  idUtilisateur: number;
  nomUtilisateur: string;
  emailUtilisateur: string;
  estAdministrateur: boolean;
  dateCreationUtilisateur: string;
  derniereConnexion: string;
  idClient?: number;
  nomClient?: string;
  adresseClient?: string;
  telephoneClient?: string;
  emailClient?: string;
  dateInscriptionClient?: string;
  actifClient?: boolean;
}

const DashboardUtilisateur: React.FC = () => {
  const { data, error, isLoading } = useGetUtilisateursClientsQuery();
  const [ajouterUtilisateurClient] = useAjouterUtilisateurClientMutation();
  const [supprimerUtilisateurClient] = useSupprimerUtilisateurClientMutation();

  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState({
    nom_utilisateur: '',
    mot_de_passe: '',
    email: '',
    est_administrateur: false,
    nom: '',
    adresse: '',
    telephone: '',
    date_inscription: new Date().toISOString(),
    actif: true,
  });

  const asideRef = useRef<HTMLElement | null>(null);

  if (isLoading) return <p>Chargement des données...</p>;
  if (error) return <p>Erreur lors de la récupération des données</p>;

  const utilisateurs = data || [];
  const administrateurs = utilisateurs.filter((utilisateur: Utilisateur) => utilisateur.estAdministrateur);
  const clients = utilisateurs.filter((utilisateur: Utilisateur) => !utilisateur.estAdministrateur);

  // Gestion des changements du formulaire
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  // Enregistrer un nouvel utilisateur/client
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await ajouterUtilisateurClient({
        utilisateur: {
          id_utilisateur: 0,
          nom_utilisateur: formData.nom_utilisateur,
          mot_de_passe: formData.mot_de_passe,
          email: formData.email,
          est_administrateur: formData.est_administrateur,
          date_creation: new Date().toISOString(),
          derniere_connexion: new Date().toISOString(),
        },
        client: {
          id_Client: 0,
          nom: formData.nom,
          adresse: formData.adresse,
          telephone: formData.telephone,
          email: formData.email,
          date_Inscription: formData.date_inscription,
          actif: formData.actif,
        },
      });
      alert('Utilisateur ou client ajouté avec succès ! ');
      setShowModal(false);
    } catch (error) {
      console.error('Erreur lors de l’ajout :', error);
    }
  };

  const handleClickSupprimerUtilisateur = async (email: string) => {
    if (window.confirm('Êtes-vous sûr de vouloir supprimer cet utilisateur ?')) {
      try {
        console.log(email);
        
        const response = await supprimerUtilisateurClient({ email }).unwrap();
    
        // Vérifier si la réponse est un succès
        if (response.success) {
          alert(response.message);  // Affiche le message de succès
        } else {
          alert(response.message);  // Affiche le message d'erreur
        }
      } catch (error) {
        console.error('Erreur lors de la suppression :', error);
        alert('Erreur lors de la suppression de l\'utilisateur.');
      }
    }
  };

  // Fonction pour ouvrir et fermer la modale
  const handleClickToggleAside = (event: React.MouseEvent<HTMLAnchorElement>) => {
    event.preventDefault();
    document.body.classList.toggle('g-sidenav-pinned');
    if (asideRef.current) {
      asideRef.current.classList.toggle('ps');
      asideRef.current.classList.toggle('ps--active-y');
      asideRef.current.classList.toggle('bg-white');
    }
  };

  return (
    <>
      <Sidenav ref={asideRef} />
      <main className="main-content position-relative max-height-vh-100 h-100 border-radius-lg">
        <DashboardNavbar handleClick={handleClickToggleAside} />
        <div className="container-fluid py-4">
          {/* Tableau des Administrateurs */}
          <div className="row mb-4">
            <div className="col-12">
              <div className="card">
                <div className="card-header pb-0 d-flex justify-content-between align-items-center">
                  <h6>Liste des Administrateurs</h6>
                </div>
                <div className="card-body px-0 pt-0 pb-2">
                  <div className="table-responsive p-0">
                    <table className="table align-items-center mb-0">
                      <thead>
                        <tr>
                          <th>Nom</th>
                          <th>Email</th>
                          <th>Date de Création</th>
                          <th>Actions</th>
                        </tr>
                      </thead>
                      <tbody>
                        {administrateurs.map((utilisateur: Utilisateur) => (
                          <tr key={utilisateur.idUtilisateur}>
                            <td>{utilisateur.nomUtilisateur}</td>
                            <td>{utilisateur.emailUtilisateur}</td>
                            <td>{new Date(utilisateur.dateCreationUtilisateur).toLocaleDateString()}</td>
                            <td>
                              <a href="#" className="action-icon modify-icon">
                                <i className="fas fa-edit"></i>
                              </a>
                              <a
                                href="#"
                                onClick={() => handleClickSupprimerUtilisateur(utilisateur.emailUtilisateur)}
                                className="action-icon delete-icon"
                              >
                                <i className="fas fa-trash-alt"></i>
                              </a>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Tableau des Clients */}
          <div className="row">
            <div className="col-12">
              <div className="card">
                <div className="card-header pb-0 d-flex justify-content-between align-items-center">
                  <h6>Liste des Clients</h6>
                  <button className="btn btn-primary mb-3" onClick={() => setShowModal(true)}>
                    <i className="fas fa-plus me-1"></i> Ajouter Client
                  </button>
                </div>
                <div className="card-body px-0 pt-0 pb-2">
                  <div className="table-responsive p-0">
                    <table className="table align-items-center mb-0">
                      <thead>
                        <tr>
                          <th>Nom</th>
                          <th>Email</th>
                          <th>Adresse</th>
                          <th>Date d'Inscription</th>
                          <th>Actions</th>
                        </tr>
                      </thead>
                      <tbody>
                        {clients.map((utilisateur: Utilisateur) => (
                          <tr key={utilisateur.idUtilisateur}>
                            <td>{utilisateur.nomClient}</td>
                            <td>{utilisateur.emailClient}</td>
                            <td>{utilisateur.adresseClient}</td>
                            <td>
                                {utilisateur.dateInscriptionClient
                                    ? new Date(utilisateur.dateInscriptionClient).toLocaleDateString()
                                    : 'Non défini'}
                                </td>
                            <td>
                              <a href="#" className="action-icon modify-icon">
                                <i className="fas fa-edit"></i>
                              </a>
                              <a
                                href="#"
                                onClick={() => handleClickSupprimerUtilisateur(utilisateur.emailClient)}
                                className="action-icon delete-icon"
                              >
                                <i className="fas fa-trash-alt"></i>
                              </a>
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Modal d'ajout d'utilisateur/client */}
          {showModal && (
            <div
              className="modal fade show d-block"
              tabIndex={-1}
              role="dialog"
              style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}
            >
              <div className="modal-dialog modal-dialog-centered" role="document">
                <div className="modal-content">
                  <div className="modal-header">
                    <h5 className="modal-title">Ajouter un client</h5>
                    <button
                      type="button"
                      className="btn-close"
                      onClick={() => setShowModal(false)}
                    />
                  </div>
                  <form onSubmit={handleSubmit}>
                    <div className="modal-body">
                      {/* Champs pour l'utilisateur */}
                      <h6 className="mb-3">Informations Utilisateur</h6>
                      <div className="form-group mb-3">
                        <label>Nom d'utilisateur</label>
                        <input
                          type="text"
                          name="nom_utilisateur"
                          className="form-control"
                          value={formData.nom_utilisateur}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-group mb-3">
                        <label>Mot de passe</label>
                        <input
                          type="password"
                          name="mot_de_passe"
                          className="form-control"
                          value={formData.mot_de_passe}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-group mb-3">
                        <label>Email</label>
                        <input
                          type="email"
                          name="email"
                          className="form-control"
                          value={formData.email}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-check mb-3">
                        <input
                          type="checkbox"
                          name="est_administrateur"
                          className="form-check-input"
                          checked={formData.est_administrateur}
                          onChange={handleChange}
                        />
                        <label className="form-check-label">Est Administrateur ?</label>
                      </div>

                      {/* Champs pour le client */}
                      <h6 className="mb-3 mt-4">Informations Client</h6>
                      <div className="form-group mb-3">
                        <label>Nom du client</label>
                        <input
                          type="text"
                          name="nom"
                          className="form-control"
                          value={formData.nom}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-group mb-3">
                        <label>Adresse</label>
                        <input
                          type="text"
                          name="adresse"
                          className="form-control"
                          value={formData.adresse}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-group mb-3">
                        <label>Téléphone</label>
                        <input
                          type="text"
                          name="telephone"
                          className="form-control"
                          value={formData.telephone}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-group mb-3">
                        <label>Date d'inscription</label>
                        <input
                          type="date"
                          name="date_inscription"
                          className="form-control"
                          value={formData.date_inscription.split('T')[0]}
                          onChange={handleChange}
                          required
                        />
                      </div>
                      <div className="form-check mb-3">
                        <input
                          type="checkbox"
                          name="actif"
                          className="form-check-input"
                          checked={formData.actif}
                          onChange={handleChange}
                        />
                        <label className="form-check-label">Client Actif</label>
                      </div>
                    </div>

                    {/* Boutons de la modale */}
                    <div className="modal-footer">
                      <button
                        type="button"
                        className="btn btn-secondary"
                        onClick={() => setShowModal(false)}
                      >
                        Annuler
                      </button>
                      <button type="submit" className="btn btn-primary">
                        Enregistrer
                      </button>
                    </div>
                  </form>
                </div>
              </div>
            </div>
          )}
        </div>
      </main>
    </>
  );
};

export default DashboardUtilisateur;
