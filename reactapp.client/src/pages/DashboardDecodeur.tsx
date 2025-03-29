import React, { useRef } from 'react';
import { useGetDecodeursQuery } from '../features/authApi'; // Import du hook
import Sidenav from '../components/Sidenav';
import DashboardNavbar from '../components/DashboardNavbar';

const DashboardDecodeur = () => {
  const { data, error, isLoading } = useGetDecodeursQuery(); // Utilisation du hook

  console.log(data);

  const asideRef = useRef<HTMLElement | null>(null);

  const handleClick = (event: React.MouseEvent<HTMLAnchorElement>) => {
    event.preventDefault();

    // Basculer la classe 'g-sidenav-pinned' sur l'élément <body>
    document.body.classList.toggle('g-sidenav-pinned');

    // Modifier la classe de l'élément <aside> via la référence
    if (asideRef.current) {
      asideRef.current.classList.toggle('ps');
      asideRef.current.classList.toggle('ps--active-y');
      asideRef.current.classList.toggle('bg-white');
    }

    console.log("Lien cliqué !");
  }

  const handleClickModifierClient = (id: number) => {
    console.log(`Modifier le décodeur avec ID : ${id}`);
    // Ajouter votre logique de modification ici
  }

  return (
    <>
      <Sidenav ref={asideRef} />
      <main className="main-content position-relative max-height-vh-100 h-100 border-radius-lg">
        <DashboardNavbar handleClick={handleClick} />
        <div className="container-fluid py-4">
          <div className="row">
            <div className="card mb-4">
              <div className="card-header pb-0 d-flex justify-content-between align-items-center">
                <h6>La liste des Décodeurs</h6>

                <button
                  className="btn btn-primary mb-3"
                  onClick={() => setShowModal(true)}
                >
                  <i className="fas fa-plus me-1"></i> Ajouter Décodeur
                </button>
              </div>
              <div className="card-body px-0 pt-0 pb-2">
                <div className="table-responsive p-0">
                  <table className="table align-items-center mb-0">
                    <thead>
                      <tr>
                        <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">Adresse IP</th>
                        <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Modèle</th>
                        <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Numéro de Série</th>
                        <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Date d'Installation</th>
                        <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Dernière Mise à Jour</th>
                        <th className="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7 ps-2">Statut</th>
                      </tr>
                    </thead>
                    <tbody>
                      {data?.map((decodeur) => (
                        <tr key={decodeur.id_decodeur}>
                          <td>
                            <div className="d-flex px-2 py-1">
                              <div className="d-flex flex-column justify-content-center">
                                <h6 className="mb-0 text-sm">{decodeur.adresse_ip}</h6>
                              </div>
                            </div>
                          </td>
                          <td>
                            <p className="text-xs font-weight-bold mb-0">{decodeur.modele}</p>
                          </td>
                          <td>
                            <p className="text-xs font-weight-bold mb-0">{decodeur.numero_serie}</p>
                          </td>
                          <td>
                            <p className="text-xs font-weight-bold mb-0">
                              {new Date(decodeur.date_installation).toLocaleDateString()}
                            </p>
                          </td>
                          <td>
                            <p className="text-xs font-weight-bold mb-0">
                              {new Date(decodeur.derniere_mise_a_jour).toLocaleDateString()}
                            </p>
                          </td>
                          <td className="align-middle text-center text-sm">
                            <span
                              className={`badge badge-sm ${
                                decodeur.etat === 'Actif' ? 'bg-gradient-success' : 'bg-gradient-secondary'
                              }`}
                            >
                              {decodeur.etat}
                            </span>
                          </td>
                          <td className="align-middle">
                            <a
                              href="#"
                              onClick={() => handleClickModifierClient(decodeur.id_decodeur)}
                              className="text-secondary font-weight-bold text-xs"
                              data-toggle="tooltip"
                              data-original-title="Modifier le décodeur"
                            >
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

          </div>
        </div>
      </main>
    </>
  );
};

export default DashboardDecodeur;
