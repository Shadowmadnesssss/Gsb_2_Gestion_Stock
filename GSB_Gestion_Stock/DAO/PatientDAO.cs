using System;
using System.Collections.Generic;
using GSB_Gestion_Stock.Models;
using MySql.Data.MySqlClient;

namespace GSB_Gestion_Stock.DAO
{
    public class PatientDAO
    {
        private readonly Database db = new Database();

        public Patient GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Patients WHERE id_patients = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    using var myReader = myCommand.ExecuteReader();
                    {
                        if (myReader.Read())
                        {
                            int patientId = myReader.GetInt32("id_patients");
                            int userId = myReader.GetInt32("id_users");
                            string name = myReader.GetString("name");
                            string firstname = myReader.GetString("firstname");
                            int age = myReader.GetInt32("age");
                            string gender = myReader.GetString("gender");
                            
                            connection.Close();
                            Patient patient = new Patient(patientId, userId, name, firstname, age, gender);
                            return patient;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PatientDAO.GetById: {ex.Message}");
                }
            }
            return null;
        }

        public List<Patient> GetAll()
        {
            List<Patient> patients = new List<Patient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Patients;";

                    using var myReader = myCommand.ExecuteReader();
                    {
                        while (myReader.Read())
                        {
                            int patientId = myReader.GetInt32("id_patients");
                            int userId = myReader.GetInt32("id_users");
                            string name = myReader.GetString("name");
                            string firstname = myReader.GetString("firstname");
                            int age = myReader.GetInt32("age");
                            string gender = myReader.GetString("gender");

                            Patient patient = new Patient(patientId, userId, name, firstname, age, gender);
                            patients.Add(patient);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return patients;
        }

        public List<Patient> GetByUserId(int userId)
        {
            List<Patient> patients = new List<Patient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Patients WHERE id_users = @userId;";
                    myCommand.Parameters.AddWithValue("@userId", userId);

                    using var myReader = myCommand.ExecuteReader();
                    {
                        while (myReader.Read())
                        {
                            int patientId = myReader.GetInt32("id_patients");
                            int user = myReader.GetInt32("id_users");
                            string name = myReader.GetString("name");
                            string firstname = myReader.GetString("firstname");
                            int age = myReader.GetInt32("age");
                            string gender = myReader.GetString("gender");

                            Patient patient = new Patient(patientId, user, name, firstname, age, gender);
                            patients.Add(patient);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PatientDAO.GetByUserId: {ex.Message}");
                }
            }
            return patients;
        }

        public bool Create(Patient patient)
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
                    maxIdCommand.CommandText = "SELECT COALESCE(MAX(id_patients), 0) + 1 FROM Patients;";
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
                    myCommand.CommandText = @"INSERT INTO Patients (id_patients, id_users, name, firstname, age, gender) 
                                            VALUES (@id, @userId, @name, @firstname, @age, @gender);";
                    myCommand.Parameters.AddWithValue("@id", newId);
                    myCommand.Parameters.AddWithValue("@userId", patient.UserId);
                    myCommand.Parameters.AddWithValue("@name", patient.Name ?? "");
                    myCommand.Parameters.AddWithValue("@firstname", patient.Firstname ?? "");
                    myCommand.Parameters.AddWithValue("@age", patient.Age);
                    myCommand.Parameters.AddWithValue("@gender", patient.Gender ?? "");

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        // Mettre à jour l'ID dans l'objet patient
                        patient.Id = newId;
                        connection.Close();
                        return true;
                    }
                    
                    connection.Close();
                    throw new Exception("Aucune ligne n'a été insérée dans la base de données.");
                }
                catch (MySqlException mysqlEx)
                {
                    Console.WriteLine($"Erreur PatientDAO.Create: {mysqlEx.Message}");
                    string errorMessage = "Erreur lors de l'ajout du patient.";
                    
                    // Messages d'erreur spécifiques selon le code d'erreur MySQL
                    if (mysqlEx.Number == 1452) // Foreign key constraint fails
                    {
                        errorMessage = "Erreur : L'ID utilisateur sélectionné n'existe pas dans la base de données.";
                    }
                    else if (mysqlEx.Number == 1062) // Duplicate entry
                    {
                        errorMessage = "Erreur : Un patient avec ces informations existe déjà.";
                    }
                    else if (mysqlEx.Number == 1048) // Column cannot be null
                    {
                        errorMessage = "Erreur : Certains champs obligatoires sont manquants.";
                    }
                    else
                    {
                        errorMessage = $"Erreur lors de l'ajout du patient : {mysqlEx.Message}";
                    }
                    
                    throw new Exception(errorMessage, mysqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PatientDAO.Create: {ex.Message}");
                    throw new Exception($"Erreur lors de l'ajout du patient : {ex.Message}", ex);
                }
            }
        }

        public bool Update(Patient patient)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"UPDATE Patients 
                                            SET id_users = @userId, name = @name, firstname = @firstname, 
                                                age = @age, gender = @gender 
                                            WHERE id_patients = @id;";
                    myCommand.Parameters.AddWithValue("@id", patient.Id);
                    myCommand.Parameters.AddWithValue("@userId", patient.UserId);
                    myCommand.Parameters.AddWithValue("@name", patient.Name);
                    myCommand.Parameters.AddWithValue("@firstname", patient.Firstname);
                    myCommand.Parameters.AddWithValue("@age", patient.Age);
                    myCommand.Parameters.AddWithValue("@gender", patient.Gender);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PatientDAO.Update: {ex.Message}");
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
                    myCommand.CommandText = @"DELETE FROM Patients WHERE id_patients = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PatientDAO.Delete: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
