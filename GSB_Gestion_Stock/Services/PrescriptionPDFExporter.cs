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
    /// Service pour exporter les prescriptions en PDF
    /// </summary>
    public class PrescriptionPDFExporter
    {
        private readonly PatientDAO patientDAO = new PatientDAO();
        private readonly MedicineDAO medicineDAO = new MedicineDAO();
        private readonly AppartientDAO appartientDAO = new AppartientDAO();

        /// <summary>
        /// Exporte une prescription en PDF avec les informations du patient, la validité et les médicaments prescrits
        /// </summary>
        /// <param name="prescription">La prescription à exporter</param>
        /// <param name="filePath">Le chemin du fichier PDF à créer</param>
        /// <returns>True si l'export a réussi, False sinon</returns>
        public bool ExportToPDF(Prescription prescription, string filePath)
        {
            try
            {
                // Récupérer les informations du patient
                var patient = patientDAO.GetById(prescription.PatientId);
                if (patient == null)
                {
                    throw new Exception("Patient introuvable");
                }

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
                            .Text("ORDONNANCE MÉDICALE")
                            .FontSize(20)
                            .Bold();

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                column.Spacing(0.5f, Unit.Centimetre);

                                // Section Patient
                                column.Item()
                                    .Background(Colors.Grey.Lighten3)
                                    .Padding(10)
                                    .Column(col =>
                                    {
                                        col.Item().Text("PATIENT").FontSize(14).Bold();
                                        col.Item().Text($"Nom : {patient.Name}");
                                        col.Item().Text($"Prénom : {patient.Firstname}");
                                        col.Item().Text($"Âge : {patient.Age} ans");
                                    });

                                // Section Validité
                                column.Item()
                                    .Background(Colors.Blue.Lighten4)
                                    .Padding(10)
                                    .Column(col =>
                                    {
                                        col.Item().Text("VALIDITÉ").FontSize(14).Bold();
                                        col.Item().Text($"Date de validité : {prescription.Validity:dd/MM/yyyy}");
                                    });

                                // Section Médicaments
                                column.Item()
                                    .Background(Colors.Green.Lighten4)
                                    .Padding(10)
                                    .Column(col =>
                                    {
                                        col.Item().Text("MÉDICAMENTS PRESCRITS").FontSize(14).Bold();

                                        if (medicinesList.Count > 0)
                                        {
                                            // Tableau des médicaments
                                            col.Item().Table(table =>
                                            {
                                                // En-têtes
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

                                                // Lignes de données
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
                                            col.Item().Text("Aucun médicament prescrit").FontColor(Colors.Grey.Medium);
                                        }
                                    });
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
                Console.WriteLine($"Erreur lors de l'export PDF : {ex.Message}");
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
