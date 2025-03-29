import LogoutButton from '../components/logoutButton';
import React, { useRef} from 'react';
import Sidenav from '../components/Sidenav';
import { useGetClientsQuery } from '../features/authApi';
import ClientPage from '../components/ClientPage';

import DashboardNavbar from '../components/DashboardNavbar';

// Définir un type ou une interface pour un client
  
  function Dashboard() {

    const { data: clients, error, isLoading } = useGetClientsQuery(); // Utiliser le hook pour récupérer les clients
   

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


    if (isLoading) return <p>Chargement des clients...</p>;

    // Vérifier si l'erreur est un objet FetchBaseQueryError
    if (error) {
      // Vérifier s'il s'agit d'un objet d'erreur de type FetchBaseQueryError
      if ('status' in error) {
        return <p>Erreur lors de la récupération des clients. Statut: {error.status}</p>;
      }
      // Sinon, si c'est une erreur sérialisée
      return <p>Erreur inconnue.</p>;
    }
  

  

    const handleClickModiffierClient = (id: number) => {
        console.log(`Modifier le client avec ID : ${id}`);
        // Ajoute ta logique de modification ic
        
    }
    
    

    return (
        <>
            {/* <LogoutButton /> */}
            <Sidenav ref={asideRef} />
            <main className="main-content position-relative max-height-vh-100 h-100 border-radius-lg ">
                <DashboardNavbar handleClick={handleClick} />
                <div className="container-fluid py-4">
                <div className="row">
                    <div className="col-12">
                    <ClientPage
                        clients={clients} 
                        handleClickModiffierClient={handleClickModiffierClient}
                        />

                 </div>
                </div>
                
                </div>
            </main>
        </>
    );
};

export default Dashboard;