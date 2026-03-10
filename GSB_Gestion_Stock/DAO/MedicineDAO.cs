using System;
using System.Collections.Generic;
using GSB_Gestion_Stock.Models;
using MySql.Data.MySqlClient;

namespace GSB_Gestion_Stock.DAO
{
    public class MedicineDAO
    {
        private readonly Database db = new Database();

        public Medicine GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Medicine WHERE id_medicine = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    using var myReader = myCommand.ExecuteReader();
                    {
                        if (myReader.Read())
                        {
                            int medicineId = myReader.GetInt32("id_medicine");
                            int userId = myReader.GetInt32("id_users");
                            // Lire dosage comme int depuis la DB puis convertir en string
                            string dosage = "";
                            try
                            {
                                int dosageInt = myReader.GetInt32("dosage");
                                dosage = dosageInt.ToString();
                            }
                            catch
                            {
                                dosage = "";
                            }
                            string name = myReader.GetString("name");
                            string description = myReader.GetString("description");
                            string molecule = myReader.GetString("molecule");
                            
                            connection.Close();
                            Medicine medicine = new Medicine(medicineId, userId, dosage, name, description, molecule);
                            return medicine;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur MedicineDAO.GetAll: {ex.Message}");
                    throw; // Relancer l'exception pour qu'elle soit visible
                }
            }
            return null;
        }

        public List<Medicine> GetAll()
        {
            List<Medicine> medicines = new List<Medicine>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Medicine;";

                    using var myReader = myCommand.ExecuteReader();
                    {
                        while (myReader.Read())
                        {
                            int medicineId = myReader.GetInt32("id_medicine");
                            int userId = myReader.GetInt32("id_users");
                            // Lire dosage comme int depuis la DB puis convertir en string
                            string dosage = "";
                            try
                            {
                                int dosageInt = myReader.GetInt32("dosage");
                                dosage = dosageInt.ToString();
                            }
                            catch
                            {
                                dosage = "";
                            }
                            string name = myReader.GetString("name");
                            string description = myReader.GetString("description");
                            string molecule = myReader.GetString("molecule");

                            Medicine medicine = new Medicine(medicineId, userId, dosage, name, description, molecule);
                            medicines.Add(medicine);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur MedicineDAO.GetAll: {ex.Message}");
                }
            }
            return medicines;
        }

        public bool Create(Medicine medicine)
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
                    maxIdCommand.CommandText = "SELECT COALESCE(MAX(id_medicine), 0) + 1 FROM Medicine;";
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
                    myCommand.CommandText = @"INSERT INTO Medicine (id_medicine, id_users, dosage, name, description, molecule) 
                                            VALUES (@id, @userId, @dosage, @name, @description, @molecule);";
                    myCommand.Parameters.AddWithValue("@id", newId);
                    myCommand.Parameters.AddWithValue("@userId", medicine.UserId);
                    // Convertir dosage string en int pour la base de données (si la colonne est de type INT)
                    int dosageInt = 0;
                    if (!string.IsNullOrEmpty(medicine.Dosage) && int.TryParse(medicine.Dosage, out int parsedDosage))
                    {
                        dosageInt = parsedDosage;
                    }
                    myCommand.Parameters.AddWithValue("@dosage", dosageInt);
                    myCommand.Parameters.AddWithValue("@name", medicine.Name ?? "");
                    myCommand.Parameters.AddWithValue("@description", medicine.Description ?? "");
                    myCommand.Parameters.AddWithValue("@molecule", medicine.Molecule ?? "");

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        // Mettre à jour l'ID dans l'objet medicine
                        medicine.Id = newId;
                        connection.Close();
                        return true;
                    }
                    
                    connection.Close();
                    throw new Exception("Aucune ligne n'a été insérée dans la base de données.");
                }
                catch (MySqlException mysqlEx)
                {
                    Console.WriteLine($"Erreur MedicineDAO.Create: {mysqlEx.Message}");
                    string errorMessage = "Erreur lors de l'ajout du médicament.";
                    
                    // Messages d'erreur spécifiques selon le code d'erreur MySQL
                    if (mysqlEx.Number == 1452) // Foreign key constraint fails
                    {
                        errorMessage = "Erreur : L'ID utilisateur sélectionné n'existe pas dans la base de données.";
                    }
                    else if (mysqlEx.Number == 1062) // Duplicate entry
                    {
                        errorMessage = "Erreur : Un médicament avec ce nom existe déjà.";
                    }
                    else if (mysqlEx.Number == 1048) // Column cannot be null
                    {
                        errorMessage = "Erreur : Certains champs obligatoires sont manquants.";
                    }
                    else
                    {
                        errorMessage = $"Erreur lors de l'ajout du médicament : {mysqlEx.Message}";
                    }
                    
                    throw new Exception(errorMessage, mysqlEx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur MedicineDAO.Create: {ex.Message}");
                    throw new Exception($"Erreur lors de l'ajout du médicament : {ex.Message}", ex);
                }
            }
        }

        public bool Update(Medicine medicine)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"UPDATE Medicine 
                                            SET id_users = @userId, dosage = @dosage, name = @name, 
                                                description = @description, molecule = @molecule 
                                            WHERE id_medicine = @id;";
                    myCommand.Parameters.AddWithValue("@id", medicine.Id);
                    myCommand.Parameters.AddWithValue("@userId", medicine.UserId);
                    // Convertir dosage string en int pour la base de données (si la colonne est de type INT)
                    int dosageInt = 0;
                    if (!string.IsNullOrEmpty(medicine.Dosage) && int.TryParse(medicine.Dosage, out int parsedDosage))
                    {
                        dosageInt = parsedDosage;
                    }
                    myCommand.Parameters.AddWithValue("@dosage", dosageInt);
                    myCommand.Parameters.AddWithValue("@name", medicine.Name);
                    myCommand.Parameters.AddWithValue("@description", medicine.Description);
                    myCommand.Parameters.AddWithValue("@molecule", medicine.Molecule);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur MedicineDAO.Update: {ex.Message}");
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
                    myCommand.CommandText = @"DELETE FROM Medicine WHERE id_medicine = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur MedicineDAO.Delete: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
