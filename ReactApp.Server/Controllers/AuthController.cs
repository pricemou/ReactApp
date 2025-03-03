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
    // // Définit la route de base pour ce contrôleur : "api/auth"
    // [Route("api/auth")]
    // [ApiController] // Indique que ce contrôleur est un contrôleur d'API
    // public class AuthController : ControllerBase
    // {
    //     // Chemin du fichier JSON contenant les utilisateurs
    //     private readonly string _userFilePath = "user.json";


    //     private readonly IConfiguration _configuration; // Configuration pour JWT

    //     public AuthController(IConfiguration configuration)
    //     {
    //         _configuration = configuration; // Injection de la configuration
    //     }


    //     // Charge la liste des utilisateurs depuis le fichier JSON
    //     private List<User> LoadUsers()
    //     {
    //         // Vérifie si le fichier existe
    //         if (!System.IO.File.Exists(_userFilePath))
    //         {
    //             // Si le fichier n'existe pas, retourne une liste vide
    //             return new List<User>();
    //         }

    //         // Lit tout le contenu du fichier JSON
    //         string json = System.IO.File.ReadAllText(_userFilePath);

    //         // Désérialise le contenu JSON en une liste d'objets User
    //         // Si la désérialisation échoue, retourne une liste vide
    //         return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    //     }


    //     // Génère un token JWT pour un utilisateur
    //     private string GenerateJwtToken(string email)
    //     {
    //         // Crée les claims (informations sur l'utilisateur)
    //         var claims = new[]
    //         {
    //             new Claim(ClaimTypes.Email, email) // Ajoute l'email comme claim
    //         };

    //         // Récupère la clé secrète depuis la configuration
    //         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

    //         // Crée les credentials pour signer le token
    //         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //         // Configure le token JWT
    //         var token = new JwtSecurityToken(
    //             issuer: _configuration["Jwt:Issuer"], // Émetteur du token
    //             audience: _configuration["Jwt:Audience"], // Public cible du token
    //             claims: claims, // Claims (informations sur l'utilisateur)
    //             expires: DateTime.Now.AddMinutes(30), // Durée de validité du token (30 minutes)
    //             signingCredentials: creds // Signature du token
    //         );

    //         // Génère et retourne le token sous forme de chaîne
    //         return new JwtSecurityTokenHandler().WriteToken(token);
    //     }
    //     // Endpoint POST pour gérer la connexion des utilisateurs
    //     [HttpPost("login")]
    //     public IActionResult Login([FromBody] User loginUser)
    //     {
    //         // Affiche les informations de connexion dans la console (pour le débogage)
    //         Console.WriteLine($"Tentative de connexion: Email = {loginUser.Email}, Password = {loginUser.Password}");

    //         // Charge la liste des utilisateurs depuis le fichier JSON
    //         var users = LoadUsers();

    //         // Affiche la liste des utilisateurs chargés dans la console (pour le débogage)
    //         Console.WriteLine($"{users}");

    //         Console.WriteLine("Liste des utilisateurs chargés :");
    //         foreach (var u in users)
    //         {
    //             Console.WriteLine($"- Email: {u.Email}, Password: {u.Password}");
    //         }

    //         // Recherche l'utilisateur correspondant à l'email et au mot de passe fournis
    //         var user = users.FirstOrDefault(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

    //         // Si aucun utilisateur n'est trouvé, retourne une réponse 401 Unauthorized
    //         if (user == null)
    //         {
    //             return Unauthorized(new { message = "Email ou mot de passe incorrect" });
    //         }

    //         var token = GenerateJwtToken(user.Email);
    //         Console.WriteLine($"++++++++++++++++++++++++++ {token}");

    //         // Si l'utilisateur est trouvé, retourne une réponse 200 OK avec un message de succès et l'email de l'utilisateur
    //         return Ok(new { message = "Authentification réussie", user.Email, token });
    //     }


        
    // }



    

    // Attribut de routage, définit l'URL d'accès pour ce contrôleur (api/clients)
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        // Déclare une instance de ApplicationDbContext pour accéder à la base de données
        private readonly ApplicationDbContext _context;

        // Le constructeur initialise _context avec une instance d'ApplicationDbContext,
        // qui est fourni par l'injection de dépendances de ASP.NET Core.
        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/clients - Cette méthode est appelée lorsque le client fait une requête GET sur l'URL "api/clients".
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            // Utilise ToListAsync() pour récupérer tous les clients de manière asynchrone depuis la base de données.
            // Cette méthode retourne une tâche qui sera terminée lorsque la liste des clients sera récupérée.
            var clients = await _context.Clients.ToListAsync();

            // Retourne les clients récupérés sous forme de réponse HTTP avec le statut 200 OK.
            // La méthode Ok() transforme les données (la liste des clients) en JSON pour être envoyées au client.
            return Ok(clients);
        }
    }



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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Valider les données de la requête
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Vérifier l'utilisateur dans la base de données
            var utilisateur = VerifierUtilisateur(loginRequest.NomUtilisateur, loginRequest.MotDePasse);
            if (utilisateur == null)
            {
                return Unauthorized("Nom d'utilisateur ou mot de passe incorrect.");
            }

            // Générer un token JWT
            var token = GenererTokenJWT(utilisateur);

            // Retourner la réponse avec le token
            return Ok( new
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireMinutes")),
                EstAdministrateur = utilisateur.est_administrateur // Ajout du rôle
            } );
        }

        private Utilisateur VerifierUtilisateur(string nomUtilisateur, string motDePasse)
        {
            // Requête SQL pour vérifier l'utilisateur
            var query = @"
                SELECT * FROM Utilisateurs
                WHERE nom_utilisateur = @nomUtilisateur AND mot_de_passe = @motDePasse";

            // Exécuter la requête SQL avec SqliteParameter
            var utilisateur = _context.Utilisateurs
                .FromSqlRaw(query,
                    new SqliteParameter("@nomUtilisateur", nomUtilisateur),
                    new SqliteParameter("@motDePasse", motDePasse))
                .FirstOrDefault();

            return utilisateur;
        }


        private string GenererTokenJWT(Utilisateur utilisateur)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, utilisateur.id_utilisateur.ToString()),
                    new Claim(ClaimTypes.Name, utilisateur.nom_utilisateur),
                    new Claim(ClaimTypes.Email, utilisateur.email),
                    new Claim(ClaimTypes.Role, utilisateur.est_administrateur ? "Admin" : "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpireMinutes")),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }


}