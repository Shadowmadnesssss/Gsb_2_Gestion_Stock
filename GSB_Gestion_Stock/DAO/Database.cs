using System;
using System.IO;
using System.Text.Json;
using MySql.Data.MySqlClient;

namespace GSB_Gestion_Stock.DAO
{
    public class Database
    {
        private string myConnectionString;

        public Database()
        {
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException(
                    $"Fichier de configuration introuvable : {configPath}\n" +
                    "Copiez appsettings.example.json en appsettings.json et renseignez vos credentials.");
            }

            try
            {
                string jsonContent = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<JsonElement>(jsonContent);

                if (config.TryGetProperty("ConnectionStrings", out var connectionStrings) &&
                    connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection))
                {
                    myConnectionString = defaultConnection.GetString()
                        ?? throw new InvalidOperationException("La clé ConnectionStrings:DefaultConnection est vide dans appsettings.json.");
                }
                else
                {
                    throw new InvalidOperationException(
                        "La clé ConnectionStrings:DefaultConnection est absente de appsettings.json.");
                }
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException(
                    $"appsettings.json est mal formé : {ex.Message}", ex);
            }
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(myConnectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using var connection = GetConnection();
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de connexion à la base de données : {ex.Message}");
                return false;
            }
        }
    }
}