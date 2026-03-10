using GSB_Gestion_Stock.DAO;
using GSB_Gestion_Stock.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GSB_Gestion_Stock.Services
{
    /// <summary>
    /// Service pour exporter le dossier complet d'un patient en PDF
    /// </summary>
    public class PatientDossierPDFExporter
    {
        private readonly PatientDAO patientDAO = new PatientDAO();
        private readonly PrescriptionDAO prescriptionDAO = new PrescriptionDAO();
        private readonly MedicineDAO medicineDAO = new MedicineDAO();
        private readonly AppartientDAO appartientDAO = new AppartientDAO();
        private readonly UserDAO userDAO = new UserDAO();

        /// <summary>
        /// Exporte le dossier complet d'un patient en PDF avec toutes ses prescriptions
        /// </summary>
        /// <param name="patientId">L'ID du patient</param>
        /// <param name="filePath">Le chemin du fichier PDF à créer</param>
        /// <returns>True si l'export a réussi, False sinon</returns>
        public bool ExportToPDF(int patientId, string filePath)
        {
            try
            {
                // Récupérer les informations du patient
                var patient = patientDAO.GetById(patientId);
                if (patient == null)
                {
                    throw new Exception("Patient introuvable");
                }

                // Récupérer toutes les prescriptions du patient
                var prescriptions = prescriptionDAO.GetByPatientId(patientId);

                // Créer le document PDF
                QuestPDF.Settings.License = LicenseType.Community;

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);

                        page.Header()
                            .AlignCenter()
                            .Text("DOSSIER PATIENT")
                            .FontSize(20)
                            .Bold();

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                column.Spacing(0.5f, Unit.Centimetre);

                                // Section Informations Patient
                                column.Item()
                                    .Background(Colors.Grey.Lighten3)
                                    .Padding(10)
                                    .Column(col =>
                                    {
                                        col.Item().Text("INFORMATIONS PATIENT").FontSize(16).Bold();
                                        col.Item().Text($"Nom : {patient.Name}");
                                        col.Item().Text($"Prénom : {patient.Firstname}");
                                        col.Item().Text($"Âge : {patient.Age} ans");
                                        col.Item().Text($"Genre : {patient.Gender}");
                                    });

                                // Section Prescriptions
                                column.Item()
                                    .Background(Colors.Blue.Lighten4)
                                    .Padding(10)
                                    .Column(col =>
                                    {
                                        col.Item().Text("PRESCRIPTIONS").FontSize(16).Bold();
                                        col.Item().Text($"Nombre total de prescriptions : {prescriptions.Count}").FontSize(12).Bold();
                                    });

                                // Pour chaque prescription
                                foreach (var prescription in prescriptions)
                                {
                                    column.Item()
                                        .PageBreak();
                                    
                                    column.Item()
                                        .Background(Colors.White)
                                        .Border(1)
                                        .BorderColor(Colors.Grey.Medium)
                                        .Padding(10)
                                        .Column(presCol =>
                                        {
                                            // Récupérer le médecin prescripteur
                                            var user = userDAO.GetById(prescription.UserId);
                                            string doctorName = user != null ? $"{user.Firstname} {user.Name}" : "Médecin inconnu";

                                            presCol.Item().Text($"Prescription #{prescription.Id}").FontSize(14).Bold();
                                            presCol.Item().Text($"Date de validité : {prescription.Validity:dd/MM/yyyy}");
                                            presCol.Item().Text($"Prescrit par : {doctorName}");

                                            // Récupérer les médicaments associés à cette prescription
                                            var appartients = appartientDAO.GetByPrescriptionId(prescription.Id);
                                            var medicinesList = new List<(Medicine medicine, int quantity)>();

                                            if (appartients != null && appartients.Count > 0)
                                            {
                                                foreach (var appartient in appartients)
                                                {
                                                    var medicine = medicineDAO.GetById(appartient.IdMedicine);
                                                    if (medicine != null)
                                                    {
                                                        medicinesList.Add((medicine, appartient.Quantity));
                                                    }
                                                }
                                            }

                                            presCol.Item().PaddingTop(5).Text("Médicaments prescrits :").FontSize(12).Bold();

                                            if (medicinesList.Count > 0)
                                            {
                                                // Tableau des médicaments
                                                presCol.Item().Table(table =>
                                                {
                                                    table.ColumnsDefinition(columns =>
                                                    {
                                                        columns.RelativeColumn(3); // Nom
                                                        columns.RelativeColumn(2); // Dosage
                                                        columns.RelativeColumn(1); // Quantité
                                                        columns.RelativeColumn(2); // Molécule
                                                    });

                                                    table.Header(header =>
                                                    {
                                                        header.Cell().Element(CellStyle).Text("Nom du médicament").Bold();
                                                        header.Cell().Element(CellStyle).Text("Dosage").Bold();
                                                        header.Cell().Element(CellStyle).Text("Quantité").Bold();
                                                        header.Cell().Element(CellStyle).Text("Molécule").Bold();
                                                    });

                                                    foreach (var (medicine, quantity) in medicinesList)
                                                    {
                                                        table.Cell().Element(CellStyle).Text(medicine.Name);
                                                        table.Cell().Element(CellStyle).Text(medicine.Dosage);
                                                        table.Cell().Element(CellStyle).Text(quantity.ToString());
                                                        table.Cell().Element(CellStyle).Text(medicine.Molecule ?? "N/A");
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                presCol.Item().Text("Aucun médicament prescrit").FontColor(Colors.Grey.Medium);
                                            }
                                        });
                                }

                                if (prescriptions.Count == 0)
                                {
                                    column.Item()
                                        .Background(Colors.Grey.Lighten4)
                                        .Padding(10)
                                        .Text("Aucune prescription enregistrée pour ce patient.")
                                        .FontColor(Colors.Grey.Medium);
                                }
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text($"Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm}");
                    });
                })
                .GeneratePdf(filePath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'export du dossier patient : {ex.Message}");
                return false;
            }
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Medium)
                .Padding(5)
                .Background(Colors.White);
        }
    }
}
