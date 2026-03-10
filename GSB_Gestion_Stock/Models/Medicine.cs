using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSB_Gestion_Stock.Models
{
    // ceci est une propriété
    public class Medicine
    {
        public int Id { get; set; }             // id_medicine
        public int UserId { get; set; }         // id_users
        public string Dosage { get; set; }      // exemple : "500mg"
        public string Name { get; set; }        // nom du médicament
        public string Description { get; set; } // description courte ou détaillée
        public string Molecule { get; set; }    // principe actif

        // ceci est le constructeur par défaut
        // il permet de créer une instance de la classe sans initialiser les propriétés
        public Medicine() { }

        // ceci est une surcharge du constructeur
        // elle permet la création d'un objet Medicine avec tous les paramètres
        public Medicine(int id, int userId, string dosage, string name, string description, string molecule)
        {
            this.Id = id;
            this.UserId = userId;
            this.Dosage = dosage;
            this.Name = name;
            this.Description = description;
            this.Molecule = molecule;
        }
    }
}