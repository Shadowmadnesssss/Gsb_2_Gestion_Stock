using System;
using System.Collections.Generic;
using GSB_Gestion_Stock.Models;
using MySql.Data.MySqlClient;

namespace GSB_Gestion_Stock.DAO
{
    public class UserDAO
    {
        private readonly Database db = new();
        
        public User Login(string email, string password)
        {
            using var connection = db.GetConnection();
            try
            {
                connection.Open();

                // create a MySQL command and set the SQL statement with parameters
                MySqlCommand myCommand = new()
                {
                    Connection = connection,
                    CommandText = @"SELECT * FROM Users WHERE email = @email AND password = @password;"
                };
                myCommand.Parameters.AddWithValue("@email", email);
                myCommand.Parameters.AddWithValue("@password", password);


                // execute the command and read the results
                using var myReader = myCommand.ExecuteReader();
                {
                    if (myReader.Read())
                    {
                        int id = myReader.GetInt32("id_users");
                        string name = myReader.GetString("name");
                        string firstname = myReader.GetString("firstname");
                        bool role = myReader.GetBoolean("role");
                        
                        connection.Close();
                        User user = new(id, name, firstname, role);
                        return user;
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur UserDAO.Login: {ex.Message}");
            }
            return null;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();

            using (var connection = db.GetConnection())
            {
            try
            {
                connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Users;";

                using var myReader = myCommand.ExecuteReader();
                {
                    while (myReader.Read())
                    {
                        int id = myReader.GetInt32("id_users");
                        string name = myReader.GetString("name");
                        string firstname = myReader.GetString("firstname");
                        bool role = myReader.GetBoolean("role");
                        
                        User user = new User(id, name, firstname, role);
                        users.Add(user);
                    }
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur UserDAO.GetAll: {ex.Message}");
                }
            }
            return users;
        }

        public User GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Users WHERE id_users = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    using var myReader = myCommand.ExecuteReader();
                    {
                        if (myReader.Read())
                        {
                            int userId = myReader.GetInt32("id_users");
                            string name = myReader.GetString("name");
                            string firstname = myReader.GetString("firstname");
                            bool role = myReader.GetBoolean("role");
                            
                            connection.Close();
                            User user = new User(userId, name, firstname, role);
                            return user;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur UserDAO.GetById: {ex.Message}");
                }
            }
            return null;
        }

        public string GetEmailById(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT email FROM Users WHERE id_users = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    object result = myCommand.ExecuteScalar();
                    connection.Close();

                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur UserDAO.GetEmailById: {ex.Message}");
                }
            }
            return null;
        }

        public bool Create(User user, string email, string password)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    // Générer l'ID manuellement si nécessaire (si AUTO_INCREMENT n'est pas configuré)
                    int newId = 0;
                    MySqlCommand maxIdCommand = new MySqlCommand();
                    maxIdCommand.Connection = connection;
                    maxIdCommand.CommandText = "SELECT COALESCE(MAX(id_users), 0) + 1 FROM Users;";
                    object maxIdResult = maxIdCommand.ExecuteScalar();
                    if (maxIdResult != null && maxIdResult != DBNull.Value)
                    {
                        newId = Convert.ToInt32(maxIdResult);
                    }
                    else
                    {
                        newId = 1; // Valeur par défaut si la table est vide
                    }

                    // Insérer avec l'ID généré
                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"INSERT INTO Users (id_users, name, firstname, role, email, password) 
                                            VALUES (@id, @name, @firstname, @role, @email, @password);";
                    myCommand.Parameters.AddWithValue("@id", newId);
                    myCommand.Parameters.AddWithValue("@name", user.Name);
                    myCommand.Parameters.AddWithValue("@firstname", user.Firstname);
                    myCommand.Parameters.AddWithValue("@role", user.Role);
                    myCommand.Parameters.AddWithValue("@email", email);
                    myCommand.Parameters.AddWithValue("@password", password);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        // Mettre à jour l'ID dans l'objet user
                        user.Id = newId;
                        connection.Close();
                        return true;
                    }
                    
                    connection.Close();
                    throw new Exception("Aucune ligne n'a été insérée dans la base de données.");
                }
                catch (MySqlException mysqlEx)
                {
                    Console.WriteLine($"Erreur UserDAO.Create: {mysqlEx.Message}");
                    string errorMessage = "Erreur lors de l'ajout de l'utilisateur.";
                    
                    // Messages d'erreur spécifiques selon le code d'erreur MySQL
                    if (mysqlEx.Number == 1452) // Foreign key constraint fails
                    {
                        errorMessage = "Erreur : Une contrainte de clé étrangère a échoué.";
                    }
                    else if (mysqlEx.Number == 1062) // Duplicate entry
                    {
                        errorMessage = "Erreur : Un utilisateur avec cet email existe déjà.";
                    }
                    else if (mysqlEx.Number == 1048) // Column cannot be null
                    {
                        errorMessage = "Erreur : Certains champs obligatoires sont manquants.";
                    }
                    else
                    {
                        errorMessage = $"Erreur lors de l'ajout de l'utilisateur : {mysqlEx.Message}";
                    }
                    
                    throw new Exception(errorMessage, mysqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur UserDAO.Create: {ex.Message}");
                    throw new Exception($"Erreur lors de l'ajout de l'utilisateur : {ex.Message}", ex);
                }
            }
        }

        public bool Update(User user, string email, string password = null)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    
                    // Si un nouveau mot de passe est fourni, on le met à jour
                    if (!string.IsNullOrEmpty(password))
                    {
                        myCommand.CommandText = @"UPDATE Users 
                                                SET name = @name, firstname = @firstname, role = @role, 
                                                    email = @email, password = @password 
                                                WHERE id_users = @id;";
                        myCommand.Parameters.AddWithValue("@password", password);
                    }
                    else
                    {
                        myCommand.CommandText = @"UPDATE Users 
                                                SET name = @name, firstname = @firstname, role = @role, 
                                                    email = @email 
                                                WHERE id_users = @id;";
                    }
                    
                    myCommand.Parameters.AddWithValue("@id", user.Id);
                    myCommand.Parameters.AddWithValue("@name", user.Name);
                    myCommand.Parameters.AddWithValue("@firstname", user.Firstname);
                    myCommand.Parameters.AddWithValue("@role", user.Role);
                    myCommand.Parameters.AddWithValue("@email", email);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur UserDAO.Update: {ex.Message}");
                    return false;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"DELETE FROM Users WHERE id_users = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur UserDAO.Delete: {ex.Message}");
                    return false;
                }
            }
        }
    }
}