using System;
using System.Security.Cryptography;
using System.Text;

namespace GSB_Gestion_Stock.Utils
{
    /// <summary>
    /// Classe utilitaire pour le hachage des mots de passe avec SHA-256
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hash un mot de passe en utilisant SHA-256
        /// </summary>
        /// <param name="password">Le mot de passe en clair</param>
        /// <returns>Le hash SHA-256 du mot de passe en format hexadécimal (minuscules)</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Le mot de passe ne peut pas être vide.", nameof(password));
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashString = BitConverter.ToString(hashValue).Replace("-", "").ToLowerInvariant();
                return hashString;
            }
        }
    }
}

