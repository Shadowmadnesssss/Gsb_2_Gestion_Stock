using GSB_Gestion_Stock.Models;

namespace GSB_Gestion_Stock.Models
{
    /// <summary>
    /// Classe statique pour stocker l'utilisateur actuellement connecté
    /// Permet d'accéder aux informations de l'utilisateur depuis n'importe quel formulaire
    /// </summary>
    public static class CurrentUser
    {
        private static User _user = null;

        /// <summary>
        /// L'utilisateur actuellement connecté
        /// </summary>
        public static User User
        {
            get { return _user; }
            set { _user = value; }
        }

        /// <summary>
        /// Vérifie si un utilisateur est connecté
        /// </summary>
        public static bool IsLoggedIn => _user != null;

        /// <summary>
        /// Déconnecte l'utilisateur actuel
        /// </summary>
        public static void Logout()
        {
            _user = null;
        }
    }
}

