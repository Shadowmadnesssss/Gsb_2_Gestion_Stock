using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSB_Gestion_Stock.Models
{
    public class Prescription(int id, int userId, int patientId, DateTime validity)
    {
        public int Id { get; set; } = id;
        public int UserId { get; set; } = userId;
        public int PatientId { get; set; } = patientId;
        public DateTime Validity { get; set; } = validity;

        // Associations (optionnelles si tu veux naviguer entre objets)
        public User? User { get; set; }
        public Patient? Patient { get; set; }
    }
}
