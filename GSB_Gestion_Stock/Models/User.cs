using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSB_Gestion_Stock.Models
{
    //ceci est une propriété 
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public bool Role { get; set; }
        //ceci est le constructeur par défaut
        //il permet de créer une instance de la classe qui nous permettra d'acceder aux méthodes et properties de celle ci
        public User() { }

        //ceci est une surcharde du constructeur, elle permettra la création d'objet User qui sera instancié avec les valeurs passées en parametres 

        public User(int id, string name, string firstname, bool role)
        {
            this.Id = id;
            this.Name = name;
            this.Firstname = firstname;
            this.Role = role;
        }
    }
}