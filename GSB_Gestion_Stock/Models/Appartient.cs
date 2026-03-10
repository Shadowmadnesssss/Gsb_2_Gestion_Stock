using System;

namespace GSB_Gestion_Stock.Models
{
    // ceci est une propriété
    public class Appartient
    {
        public int IdPrescription { get; set; } // id_prescription
        public int IdMedicine { get; set; }     // id_medicine
        public int Quantity { get; set; }       // quantity

        // ceci est le constructeur par défaut
        // il permet de créer une instance de la classe sans initialiser les propriétés
        public Appartient() { }

        // ceci est une surcharge du constructeur
        // elle permet la création d'un objet Appartient avec tous les paramètres
        public Appartient(int idPrescription, int idMedicine)
        {
            this.IdPrescription = idPrescription;
            this.IdMedicine = idMedicine;
        }

        // constructeur avec quantity
        public Appartient(int idPrescription, int idMedicine, int quantity)
        {
            this.IdPrescription = idPrescription;
            this.IdMedicine = idMedicine;
            this.Quantity = quantity;
        }
    }
}