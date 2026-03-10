using System;
using System.Collections.Generic;
using GSB_Gestion_Stock.Models;
using MySql.Data.MySqlClient;

namespace GSB_Gestion_Stock.DAO
{
    public class PrescriptionDAO
    {
        private readonly Database db = new Database();

        public Prescription GetById(int id)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Prescription WHERE id_prescription = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    using var myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        int prescriptionId = myReader.GetInt32("id_prescription");
                        int userId = myReader.GetInt32("id_users");
                        int patientId = myReader.GetInt32("id_patients");
                        DateTime validity = myReader.GetDateTime("validity");

                        return new Prescription(prescriptionId, userId, patientId, validity);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.GetById: {ex.Message}");
                }
            }
            return null;
        }

        public List<Prescription> GetAll()
        {
            List<Prescription> prescriptions = new List<Prescription>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Prescription;";

                    using var myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        int prescriptionId = myReader.GetInt32("id_prescription");
                        int userId = myReader.GetInt32("id_users");
                        int patientId = myReader.GetInt32("id_patients");
                        DateTime validity = myReader.GetDateTime("validity");

                        Prescription prescription = new Prescription(prescriptionId, userId, patientId, validity);
                        prescriptions.Add(prescription);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.GetAll: {ex.Message}");
                }
            }
            return prescriptions;
        }

        public List<Prescription> GetByUserId(int userId)
        {
            List<Prescription> prescriptions = new List<Prescription>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Prescription WHERE id_users = @userId;";
                    myCommand.Parameters.AddWithValue("@userId", userId);

                    using var myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        int prescriptionId = myReader.GetInt32("id_prescription");
                        int user = myReader.GetInt32("id_users");
                        int patientId = myReader.GetInt32("id_patients");
                        DateTime validity = myReader.GetDateTime("validity");

                        Prescription prescription = new Prescription(prescriptionId, user, patientId, validity);
                        prescriptions.Add(prescription);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.GetByUserId: {ex.Message}");
                }
            }
            return prescriptions;
        }

        public bool Create(Prescription prescription)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    // Insérer la prescription
                    MySqlCommand insertCommand = new MySqlCommand();
                    insertCommand.Connection = connection;
                    insertCommand.CommandText = @"INSERT INTO Prescription (id_users, id_patients, validity) 
                                            VALUES (@userId, @patientId, @validity);";
                    insertCommand.Parameters.AddWithValue("@userId", prescription.UserId);
                    insertCommand.Parameters.AddWithValue("@patientId", prescription.PatientId);
                    insertCommand.Parameters.AddWithValue("@validity", prescription.Validity);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        // Récupérer l'ID généré
                        MySqlCommand selectCommand = new MySqlCommand();
                        selectCommand.Connection = connection;
                        selectCommand.CommandText = "SELECT LAST_INSERT_ID();";
                        
                        object result = selectCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            prescription.Id = Convert.ToInt32(result);
                            return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.Create: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    // Propager l'exception pour avoir plus de détails dans le formulaire
                    throw new Exception($"Erreur lors de la création de la prescription: {ex.Message}", ex);
                }
            }
        }

        public bool Update(Prescription prescription)
        {
            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"UPDATE Prescription 
                                            SET id_users = @userId, id_patients = @patientId, 
                                            validity = @validity 
                                            WHERE id_prescription = @id;";
                    myCommand.Parameters.AddWithValue("@id", prescription.Id);
                    myCommand.Parameters.AddWithValue("@userId", prescription.UserId);
                    myCommand.Parameters.AddWithValue("@patientId", prescription.PatientId);
                    myCommand.Parameters.AddWithValue("@validity", prescription.Validity);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.Update: {ex.Message}");
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
                    myCommand.CommandText = @"DELETE FROM Prescription WHERE id_prescription = @id;";
                    myCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = myCommand.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.Delete: {ex.Message}");
                    return false;
                }
            }
        }

        public List<Prescription> GetByPatientId(int patientId)
        {
            List<Prescription> prescriptions = new List<Prescription>();

            using (var connection = db.GetConnection())
            {
                try
                {
                    connection.Open();

                    MySqlCommand myCommand = new MySqlCommand();
                    myCommand.Connection = connection;
                    myCommand.CommandText = @"SELECT * FROM Prescription WHERE id_patients = @patientId ORDER BY validity DESC;";
                    myCommand.Parameters.AddWithValue("@patientId", patientId);

                    using var myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        int prescriptionId = myReader.GetInt32("id_prescription");
                        int userId = myReader.GetInt32("id_users");
                        int patient = myReader.GetInt32("id_patients");
                        DateTime validity = myReader.GetDateTime("validity");

                        Prescription prescription = new Prescription(prescriptionId, userId, patient, validity);
                        prescriptions.Add(prescription);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur PrescriptionDAO.GetByPatientId: {ex.Message}");
                }
            }
            return prescriptions;
        }
    }
}
