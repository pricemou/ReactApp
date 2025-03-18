using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using ReactApp.Server.Models;
using System.Security.Claims;
using System.Text;
using ReactApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace ReactApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
    
        // GET: api/clients - Cette méthode est appelée lorsque le client fait une requête GET sur l'URL "api/clients".
        [HttpGet("list-client")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            // Utilise ToListAsync() pour récupérer tous les clients de manière asynchrone depuis la base de données.
            // Cette méthode retourne une tâche qui sera terminée lorsque la liste des clients sera récupérée.
            var clients = await _context.Clients.ToListAsync();

            // Retourne les clients récupérés sous forme de réponse HTTP avec le statut 200 OK.
            // La méthode Ok() transforme les données (la liste des clients) en JSON pour être envoyées au client.
            return Ok(clients);
        }

       private string GenererTokenJWT(VueUtilisateurActivites utilisateur)
        {
            if (utilisateur == null)
            {
                throw new ArgumentNullException(nameof(utilisateur), "L'utilisateur ne peut pas être null");
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, utilisateur.Id_Utilisateur.ToString()),
                    new Claim(ClaimTypes.Name, utilisateur.Nom_Utilisateur),
                    new Claim(ClaimTypes.Email, utilisateur.Email),
                    new Claim(ClaimTypes.Role, utilisateur.EstAdministrateur ? "Admin" : "User")
                }),

                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpireMinutes")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //Genere un ensemble de donne
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Recherche de l'utilisateur dans la vue Vue_Utilisateur_Activites en fonction de l'email et du mot de passe
           var Vue_Utilisateur = await _context.Vue_Utilisateur_Activites
            .Where(u => u.Nom_Utilisateur == loginRequest.NomUtilisateur && u.Mot_de_passe == loginRequest.MotDePasse)
            .FirstOrDefaultAsync();



            Console.WriteLine($"NomUtilisateur: {Vue_Utilisateur}, MotDePasse: {loginRequest.MotDePasse}");




             var token = GenererTokenJWT(Vue_Utilisateur);

            // Si l'utilisateur n'est pas trouvé, retourner une erreur Unauthorized
            if (Vue_Utilisateur == null)
            {
                return Unauthorized("Email ou mot de passe incorrect");
            }

            // Retourner les informations de l'utilisateur
            return Ok(new 
            {
                Vue_Utilisateur.Id_Utilisateur,
                Vue_Utilisateur.Nom_Utilisateur,
                Vue_Utilisateur.Email,
                Vue_Utilisateur.Nombre_Decodeurs,
                Vue_Utilisateur.Nombre_Operations,
                Vue_Utilisateur.EstAdministrateur,
                Vue_Utilisateur.Adresse_Client,
                Vue_Utilisateur.Nom_Client,
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireMinutes")),
            });
        }

        [HttpPost("ajouter-utilisateur")]
        public async Task<IActionResult> AjouterUtilisateur([FromBody] Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ajout de l'utilisateur à la base de données
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AjouterUtilisateur), new { id = utilisateur.id_utilisateur }, utilisateur);
        }   


        [HttpPost("ajouter-utilisateur-client")]
        public async Task<IActionResult> AjouterUtilisateurClient([FromBody] UtilisateurClientRequest request)
        {
            if (request.Utilisateur == null || request.Client == null)
            {
                return BadRequest("Utilisateur et Client sont nécessaires.");
            }

            // Logique pour ajouter l'utilisateur et le client dans la base de données
            // Par exemple :
            var utilisateur = request.Utilisateur;
            var client = request.Client;

            // Ajouter l'utilisateur à la base de données
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            // Ajouter le client à la base de données
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Ajouter la relation UtilisateurClient
            var utilisateurClient = new UtilisateursClients
            {
                id_utilisateur = utilisateur.id_utilisateur,
                id_client = client.Id_Client
            };
            _context.UtilisateursClients.Add(utilisateurClient);
            await _context.SaveChangesAsync();

            return Ok("Utilisateur et client ajoutés avec succès.");
        }

        [HttpPost("supprimer-utilisateur-client")]
        public async Task<IActionResult> SupprimerUtilisateurClient([FromBody] SupprimerUtilisateurClientRequest request)
        {
            await _context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode = WAL;"); // Optionnel, pour les performances

            try
            {
                var emailUtilisateur = request.Email;

                // Supprimer le client et la relation avec l'utilisateur
                var result = await _context.Database.ExecuteSqlRawAsync(@"
                    DELETE FROM Clients
                    WHERE Id_Client IN (
                        SELECT id_client
                        FROM UtilisateursClients
                        INNER JOIN Utilisateurs ON UtilisateursClients.id_utilisateur = Utilisateurs.id_utilisateur
                        WHERE Utilisateurs.email = @EmailUtilisateur
                    );

                    DELETE FROM UtilisateursClients
                    WHERE id_utilisateur IN (
                        SELECT id_utilisateur
                        FROM Utilisateurs
                        WHERE email = @EmailUtilisateur
                    );

                    DELETE FROM Utilisateurs
                    WHERE email = @EmailUtilisateur;
                ", new SqliteParameter("@EmailUtilisateur", emailUtilisateur));

                // Vérification si la suppression a réussi
                if (result > 0)
                {
                    return Ok("Utilisateur et client supprimés avec succès.");
                }
                else
                {
                    return Conflict("L'utilisateur ou le client n'a pas pu être supprimé.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la suppression des données : {ex.Message}");
            }
        }


        [HttpGet("get-decodeurs/{idClient}")]
        public async Task<IActionResult> GetDecodeursByClient(int idClient)
        {
            try
            {
                // Récupérer tous les décodeurs pour un client spécifique
                var decodeurs = await _context.Decodeurs
                    .Where(d => d.id_decodeur == idClient)
                    .ToListAsync();

                if (decodeurs == null || decodeurs.Count == 0)
                {
                    return NotFound("Aucun décodeur trouvé pour ce client.");
                }

                return Ok(decodeurs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la récupération des décodeurs : {ex.Message}");
            }
        }


        [HttpPost("create-client")]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientRequest request)
        {
            // Vérification si la requête est vide (si des données sont manquantes)
            if (request == null)
            {
                return BadRequest("Données manquantes.");  // Retourne une erreur si la requête est vide
            }

            try
            {
                // Vérifier si un client avec le même email existe déjà dans la base de données
                var existingClient = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Email == request.Email);

                if (existingClient != null)
                {
                    return Conflict("Un client avec cet email existe déjà.");  // Retourne un conflit si l'email existe déjà
                }

                // Créer un nouvel objet Client avec les données de la requête
                var client = new Client
                {
                    Nom = request.Nom,
                    Adresse = request.Adresse,
                    Telephone = request.Telephone,
                    Email = request.Email,
                    Date_Inscription = DateTime.Now,  // La date d'inscription est la date actuelle
                    Actif = true // Par défaut, le client est actif
                };

                // Ajouter le client à la base de données
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();  // Sauvegarde les changements dans la base de données

                return Ok($"Client {client.Nom} créé avec succès.");  // Retourne un message de succès
            }
            catch (Exception ex)
            {
                // Gestion des erreurs en cas d'exception
                return StatusCode(500, $"Erreur lors de la création du client : {ex.Message}");  // Retourne une erreur interne du serveur
            }
        }


        [HttpPost("remove-decoder-from-client")]
        public async Task<IActionResult> RemoveDecoderFromClient([FromBody] RemoveDecoderRequest request)
        {
            // Vérifie si la requête est valide
            if (request == null || request.id_client <= 0 || request.id_decodeur <= 0)
            {
                return BadRequest("Données manquantes ou invalides.");
            }

            try
            {
                // Recherche du client par son identifiant
                var client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.Id_Client == request.id_client);

                // Si le client n'existe pas, retourne une erreur
                if (client == null)
                {
                    return NotFound("Client non trouvé.");
                }

                // Recherche du décodeur par son identifiant
                var decoder = await _context.Decodeurs
                    .FirstOrDefaultAsync(d => d.id_decodeur == request.id_decodeur && d.id_client == client.Id_Client);

                // Si le décodeur n'existe pas pour ce client, retourne une erreur
                if (decoder == null)
                {
                    return NotFound("Décodeur non trouvé pour ce client.");
                }

                // Retirer le décodeur de la liste des décodeurs du client
                _context.Decodeurs.Remove(decoder);

                // Sauvegarde les modifications dans la base de données
                await _context.SaveChangesAsync();

                // Retourne une réponse de succès
                return Ok($"Décodeur avec ID {decoder.id_decodeur} retiré du client avec succès.");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs générales
                return StatusCode(500, $"Erreur lors de la suppression du décodeur : {ex.Message}");
            }
        }


        [HttpPost("etat-decodeur")]
        public async Task<IActionResult> GetEtatDecodeur([FromBody] DecodeurRequest request)
        {
            if (request == null || request.id_decodeur <= 0)
            {
                return BadRequest("Données invalides. Veuillez fournir un ID de décodeur valide.");
            }

            try
            {
                // Recherche du décodeur dans la base de données
                var decodeur = await _context.Decodeurs
                    .AsNoTracking()
                    .Where(d => d.id_decodeur == request.id_decodeur)
                    .Select(d => new { d.id_decodeur, d.etat })
                    .FirstOrDefaultAsync();

                // Vérifier si le décodeur existe
                if (decodeur == null)
                {
                    return NotFound($"Aucun décodeur trouvé avec l'ID {request.id_decodeur}.");
                }

                // Retourner l'état du décodeur
                return Ok(new
                {
                    Message = "État du décodeur récupéré avec succès.",
                    Decodeur = decodeur
                });
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                return StatusCode(500, $"Erreur lors de la récupération de l'état du décodeur : {ex.Message}");
            }
        }


       [HttpPost("restart-decodeur")]
       public async Task<IActionResult> RestartDecodeur([FromBody] RestartDecodeurRequest request)
        {
            if (request.id_decodeur <= 0)   
            {
                return BadRequest("ID du décodeur invalide.");
            }

            try
            {
                // Trouver le décodeur
                var decodeur = await _context.Decodeurs
                    .Where(d => d.id_decodeur == request.id_decodeur)
                    .FirstOrDefaultAsync();

                if (decodeur == null)
                {
                    return NotFound("Décodeur non trouvé.");
                }

                // Récupérer l'opération "Redémarrer" dans TypesOperations
                var typeOperation = await _context.TypesOperations
                    .Where(t => t.nom == "Redémarrer")
                    .FirstOrDefaultAsync();

                if (typeOperation == null)
                {
                    return NotFound("Type d'opération 'Redémarrer' introuvable.");
                }

                // Insérer l'opération dans le Journal des opérations
                var operation = new JournalOperation
                {
                    id_decodeur = decodeur.id_decodeur,
                    id_type_operation = typeOperation.id_type_operation,
                    id_utilisateur = request.id_utilisateur > 0 ? request.id_utilisateur : 1, // Utilisateur par défaut si non fourni
                    statut = "En cours",
                    date_debut = DateTime.UtcNow
                };

                _context.JournalOperations.Add(operation);
                await _context.SaveChangesAsync();

                // Simuler le temps d'exécution de l'opération
                await Task.Delay(typeOperation.temps_execution_moyen * 500);

                // Mettre à jour l'état du décodeur après redémarrage
                decodeur.etat = "redémarré";
                _context.Decodeurs.Update(decodeur);

                // Mettre à jour l'état de l'opération dans le journal
                operation.statut = "Terminé";
                operation.date_fin = DateTime.UtcNow;
                operation.details = $"Décodeur {decodeur.id_decodeur} redémarré avec succès.";

                _context.JournalOperations.Update(operation);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"Le décodeur {decodeur.id_decodeur} a été redémarré avec succès.",
                    operation_id = operation.id_operation,
                    temps_execution = typeOperation.temps_execution_moyen
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors du redémarrage du décodeur : {ex.Message}");
            }
        }

        [HttpPost("ajouter-chaine-a-decodeur")]
        public async Task<IActionResult> AjouterChaineADecodeur([FromBody] AjouterChaineRequest request)
        {
            // Validation de la demande
            if (request.id_decodeur <= 0 || request.id_contenu <= 0)
            {
                return BadRequest("Les identifiants du décodeur et du contenu doivent être valides.");
            }

            // Vérifier si le décodeur et le contenu existent
            var decodeur = await _context.Decodeurs.FindAsync(request.id_decodeur);
            var contenu = await _context.Contenus.FindAsync(request.id_contenu);

            if (decodeur == null || contenu == null)
            {
                return NotFound("Le décodeur ou le contenu spécifié n'existe pas.");
            }

            // Ajouter la relation dans la table DecodeurContenu
            var decodeurContenu = new DecodeurContenu
            {
                id_decodeur = request.id_decodeur,
                id_contenu = request.id_contenu 
            };

            _context.DecodeurContenu.Add(decodeurContenu);
            await _context.SaveChangesAsync();

            return Ok("Chaîne ajoutée avec succès au décodeur.");
        }


        [HttpDelete("retirer-chaine")]
        public async Task<IActionResult> RetirerChaineDuDecodeur([FromBody] RetirerChaineRequest request)
        {
            // Validation de la demande
            if (request.id_decodeur <= 0 || request.id_contenu <= 0)
            {
                return BadRequest("Les identifiants du décodeur et du contenu doivent être valides.");
            }

            // Vérifier si le décodeur et le contenu existent
            var decodeur = await _context.Decodeurs.FindAsync(request.id_decodeur);
            var contenu = await _context.Contenus.FindAsync(request.id_contenu);

            if (decodeur == null || contenu == null)
            {
                return NotFound("Le décodeur ou le contenu spécifié n'existe pas.");
            }

            // Trouver l'association dans la table DecodeurContenu
            var decodeurContenu = await _context.DecodeurContenu
                .FirstOrDefaultAsync(dc => dc.id_decodeur == request.id_decodeur && dc.id_contenu == request.id_contenu);

            // Si l'association n'existe pas, renvoyer une erreur
            if (decodeurContenu == null)
            {
                return NotFound("L'association entre ce décodeur et ce contenu n'existe pas.");
            }

            // Supprimer l'association
            _context.DecodeurContenu.Remove(decodeurContenu);
            await _context.SaveChangesAsync();

            return Ok("Chaîne retirée avec succès du décodeur.");
        }

    }

}