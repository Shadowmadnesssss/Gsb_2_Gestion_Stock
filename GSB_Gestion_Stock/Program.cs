using System;
using System.Windows.Forms;
using GSB_Gestion_Stock.Forms;

namespace GSB_Gestion_Stock
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Pour activer les styles visuels pour les contrôles Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Lancer le formulaire de connexion
            Application.Run(new LoginForm());
        }
    }
}

