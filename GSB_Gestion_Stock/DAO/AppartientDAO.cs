using System;
using System.Collections.Generic;
using GSB_Gestion_Stock.Models;
using MySql.Data.MySqlClient;

namespace GSB_Gestion_Stock.DAO
{
    public class AppartientDAO
    {
        private readonly Database db = new Database();

        public Appartient GetById(int idPrescription, int idMedicine)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Appartient 
                                            WHERE id_prescription = @idPrescription AND id_medicine = @idMedicine;";
                    myCommand.Parameters.AddWithValue("@idPrescription", idPrescription);
                    myCommand.Parameters.AddWithValue("@idMedicine", idMedicine);


                    using var myReader = myCommand.ExecuteReader();
                    {
                        if (myReader.Read())
                        {
                            int prescriptionId = myReader.GetInt32("id_prescription");
                            int medicineId = myReader.GetInt32("id_medicine");
                            int quantity = 1; // Valeur par défaut
                            
                            // Lire la quantité si la colonne existe
                            try
                            {
                                int quantityIndex = myReader.GetOrdinal("quantity");
                                if (!myReader.IsDBNull(quantityIndex))
                                {
                                    quantity = myReader.GetInt32(quantityIndex);
                                }
                            }
                            catch
                            {
                                // La colonne quantity n'existe pas encore, utiliser la valeur par défaut
                            }

                            
                            connection.Close();
                            Appartient appartient = new Appartient(prescriptionId, medicineId, quantity);
                            return appartient;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AppartientDAO.GetById: {ex.Message}");
                }
            }
            return null;
        }

        public List<Appartient> GetAll()
        {
            List<Appartient> appartients = new List<Appartient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Appartient;";

                    using var myReader = myCommand.ExecuteReader();
                    {
                        while (myReader.Read())
                        {
                            int prescriptionId = myReader.GetInt32("id_prescription");
                            int medicineId = myReader.GetInt32("id_medicine");
                            int quantity = 1; // Valeur par défaut
                            
                            // Lire la quantité si la colonne existe
                            try
                            {
                                int quantityIndex = myReader.GetOrdinal("quantity");
                                if (!myReader.IsDBNull(quantityIndex))
                                {
                                    quantity = myReader.GetInt32(quantityIndex);
                                }
                            }
                            catch
                            {
                                // La colonne quantity n'existe pas encore, utiliser la valeur par défaut
                            }

                            Appartient appartient = new Appartient(prescriptionId, medicineId, quantity);
                            appartients.Add(appartient);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AppartientDAO.GetAll: {ex.Message}");
                }
            }
            return appartients;
        }

        public List<Appartient> GetByPrescriptionId(int idPrescription)
        {
            List<Appartient> appartients = new List<Appartient>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Appartient WHERE id_prescription = @idPrescription;";
                    myCommand.Parameters.AddWithValue("@idPrescription", idPrescription);

                    using var myReader = myCommand.ExecuteReader();
                    {
                        while (myReader.Read())
                        {
                            int prescriptionId = myReader.GetInt32("id_prescription");
                            int medicineId = myReader.GetInt32("id_medicine");
                            int quantity = 1; // Valeur par défaut
                            
                            // Lire la quantité si la colonne existe
                            try
                            {
                                int quantityIndex = myReader.GetOrdinal("quantity");
                                if (!myReader.IsDBNull(quantityIndex))
                                {
                                    quantity = myReader.GetInt32(quantityIndex);
                                }
                            }
                            catch
                            {
                                // La colonne quantity n'existe pas encore, utiliser la valeur par défaut
                            }

                            Appartient appartient = new Appartient(prescriptionId, medicineId, quantity);
                            appartients.Add(appartient);
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AppartientDAO.GetByPrescriptionId: {ex.Message}");
                }
            }
            return appartients;
        }

        public bool Create(Appartient appartient)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"INSERT INTO Appartient (id_prescription, id_medicine, quantity) 
                                            VALUES (@idPrescription, @idMedicine, @quantity);";
                    myCommand.Parameters.AddWithValue("@idPrescription", appartient.IdPrescription);
                    myCommand.Parameters.AddWithValue("@idMedicine", appartient.IdMedicine);
                    myCommand.Parameters.AddWithValue("@quantity", appartient.Quantity > 0 ? appartient.Quantity : 1);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Appartient créé avec succès: Prescription ID={appartient.IdPrescription}, Medicine ID={appartient.IdMedicine}, Quantity={appartient.Quantity}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Aucune ligne affectée lors de la création de Appartient");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AppartientDAO.Create: {ex.Message}");
                    Console.WriteLine($"Prescription ID: {appartient.IdPrescription}, Medicine ID: {appartient.IdMedicine}, Quantity: {appartient.Quantity}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    // Propager l'exception pour avoir plus de détails dans le formulaire
                    throw new Exception($"Erreur lors de l'ajout du médicament: {ex.Message}", ex);
                }
            }
        }

        public bool Delete(int idPrescription, int idMedicine)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"DELETE FROM Appartient 
                                            WHERE id_prescription = @idPrescription AND id_medicine = @idMedicine;";
                    myCommand.Parameters.AddWithValue("@idPrescription", idPrescription);
                    myCommand.Parameters.AddWithValue("@idMedicine", idMedicine);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AppartientDAO.Delete: {ex.Message}");
                    return false;
                }
            }
        }

        public bool DeleteByPrescriptionId(int idPrescription)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"DELETE FROM Appartient WHERE id_prescription = @idPrescription;";
                    myCommand.Parameters.AddWithValue("@idPrescription", idPrescription);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur AppartientDAO.DeleteByPrescriptionId: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
