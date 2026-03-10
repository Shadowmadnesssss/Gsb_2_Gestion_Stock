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
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                
                if (File.Exists(configPath))
                {
                    string jsonContent = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<JsonElement>(jsonContent);
                    
                    if (config.TryGetProperty("ConnectionStrings", out var connectionStrings))
                    {
                        if (connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection))
                        {
                            myConnectionString = defaultConnection.GetString();
                        }
                    }
                }
                
                // Si le fichier n'existe pas ou si la lecture échoue, utiliser la valeur par défaut
                if (string.IsNullOrEmpty(myConnectionString))
                {
                    myConnectionString = "server=127.0.0.1;port=3306;uid=root;pwd=root;database=GSB_Gestion_Stock;charset=utf8mb4;";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement de la configuration : {ex.Message}");
                // Utiliser la valeur par défaut en cas d'erreur
                myConnectionString = "server=127.0.0.1;port=3306;uid=root;pwd=root;database=GSB_Gestion_Stock;charset=utf8mb4;";
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