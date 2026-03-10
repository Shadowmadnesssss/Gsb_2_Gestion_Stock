using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSB_Gestion_Stock.Models
{
    // ceci est une propriété
    public class Patient
    {
        public int Id { get; set; }
        public int UserId { get; set; }   // fait référence à un User
        public string Name { get; set; }
        public string Firstname { get; set; }
        public int Age { get; set; }      // ou calculé à partir d'une date de naissance si tu veux
        public string Gender { get; set; } // "H" ou "F"

        // ceci est le constructeur par défaut
        // il permet de créer une instance de la classe sans initialiser les propriétés
        public Patient() { }

        // ceci est une surcharge du constructeur
        // elle permet la création d'un objet Patient avec tous les paramètres
        public Patient(int id, int userId, string name, string firstname, int age, string gender)
        {
            this.Id = id;
            this.UserId = userId;
            this.Name = name;
            this.Firstname = firstname;
            this.Age = age;
            this.Gender = gender;
        }
    }
}