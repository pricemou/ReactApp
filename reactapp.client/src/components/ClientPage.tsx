// ClientPage.tsx

import React from 'react';
import { useAddClientMutation, useGetClientsQuery } from '../features/authApi';
import { useState } from 'react';

interface Client {
    id_Client: number;
    nom: string;
    adresse: string;
    telephone: string;
    email: string;
    date_Inscription: string;
    actif: boolean;
  }

  interface ClientPageProps {
    clients: Client[];
    handleClickModiffierClient: (id: number) => void;
  }
  

// ✅ Accepter les props dans le composant
const ClientPage: React.FC<ClientPageProps> = ({ clients, handleClickModiffierClient }) => {
    const [nom, setNom] = useState('');
    const [adresse, setAdresse] = useState('');
    const [telephone, setTelephone] = useState('');
    const [email, setEmail] = useState('');
    const [showModal, setShowModal] = useState(false);

 
     // Utiliser le hook pour ajouter un client
  const [addClient, { isLoading: isAddingClient, error: addClientError }] = useAddClientMutation();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const nouveauClient = {
      nom,
      adresse,
      telephone,
      email,
    };

    try {
      // Appel à la mutation Redux pour ajouter un client
      console.log(nouveauClient);
      await addClient(nouveauClient).unwrap();

      alert('Client ajouté avec succès !');
      setShowModal(false);  // Fermer le modal après l'ajout
      // Optionnellement, tu peux rafraîchir la liste des clients si nécessaire
    } catch (error) {
      console.error('Erreur:', error);
      alert('Erreur lors de l\'ajout du client.');
    }
  };


    // Extraction de l'erreur pour l'affichage
    const renderErrorMessage = (error: any) => {
        if (error) {
          if ('data' in error && error.data) {
            return error.data;
          }
          if ('message' in error && error.message) {
            return error.message;
          }
          return 'Erreur inconnue';
        }
        return null;
      };


  return (
    <>
          <div className="card mb-4">
                <div className="card-header pb-0 d-flex justify-content-between align-items-center">
                <h6>La liste des Clients</h6>
        
                <button
                    className="btn btn-primary mb-3"
                    onClick={() => setShowModal(true)}
                >
                    <i className="fas fa-plus me-1"></i> Ajouter Client
                </button>
                </div>
                    <div className="card-body px-0 pt-0 pb-2">
                        <div className="table-responsive p-0">
                        <table className="table align-items-center mb-0">
                            <thead>
                            <tr>
                                <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Nom</th>
                                <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Adresse</th>
                                <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Téléphone</th>
                                <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Email</th>
                                <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Date d'inscription</th>
                                <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Statut</th>
                            </tr>
                            </thead>
                            <tbody>
                            {clients?.map((client) => (
                                <tr key={client.id_Client}>
                                <td>
                                    <div className="d-flex px-2 py-1">
                                    <div className="d-flex flex-column justify-content-center">
                                        <h6 className="mb-0 text-sm">{client.nom}</h6>
                                    </div>
                                    </div>
                                </td>
                                <td>
                                    <p className="text-xs font-weight-bold mb-0">{client.adresse}</p>
                                </td>
                                <td>
                                    <p className="text-xs font-weight-bold mb-0">{client.telephone}</p>
                                </td>
                                <td>
                                    <p className="text-xs font-weight-bold mb-0">{client.email}</p>
                                </td>
                                <td>
                                    <p className="text-xs font-weight-bold mb-0">{new Date(client.date_Inscription).toLocaleDateString()}</p>
                                </td>
                                <td className="align-middle text-center text-sm">
                                    <span className={`badge badge-sm ${client.actif ? 'bg-gradient-success' : 'bg-gradient-secondary'}`}>
                                    {client.actif ? 'Actif' : 'Inactif'}
                                    </span>
                                </td>
                                <td className="align-middle">
                                    <a href="#" onClick={handleClickModiffierClient} className="text-secondary font-weight-bold text-xs" data-toggle="tooltip" data-original-title="Modifier le client">
                                    <i className="fas fa-edit"></i>
                                    </a>
                                </td>
                                </tr>
                            ))}
                            </tbody>
                        </table>
                        </div>
                    </div>
                </div>
                
                {/* Modal Bootstrap */}
       
      {/* Modal Bootstrap */}
      {showModal && (
        <div className="modal fade show d-block" tabIndex={-1} style={{ backgroundColor: 'rgba(0, 0, 0, 0.5)' }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content">
            <div className="modal-header d-flex justify-content-center align-items-center">
                <h5 className="modal-title text-center">Ajouter un nouveau client</h5>
            </div>
              <div className="modal-body">
                <form onSubmit={handleSubmit}>
                  <div className="mb-3">
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Nom"
                      value={nom}
                      onChange={(e) => setNom(e.target.value)}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Adresse"
                      value={adresse}
                      onChange={(e) => setAdresse(e.target.value)}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <input
                      type="text"
                      className="form-control"
                      placeholder="Téléphone"
                      value={telephone}
                      onChange={(e) => setTelephone(e.target.value)}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <input
                      type="email"
                      className="form-control"
                      placeholder="Email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      required
                    />
                  </div>
                  
                  <div className="d-flex justify-content-between">
                    <button type="submit" className="btn btn-success" disabled={isAddingClient}>
                      {isAddingClient ? 'Ajout en cours...' : 'Ajouter'}
                    </button>
                    <button
                      type="button"
                      className="btn btn-secondary"
                      onClick={() => setShowModal(false)}
                    >
                      Fermer
                    </button>
                  </div>
                </form>
                {addClientError && (
                  <div className="alert alert-danger mt-3" role="alert">
                    {renderErrorMessage(addClientError)}
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}


    </>
  );
};

export default ClientPage;
