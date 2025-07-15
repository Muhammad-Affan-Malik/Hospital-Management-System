using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.WebAPI.Models

{
    public class Patient
    {
        public int PatientId { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Gender { get; set; }
        public required string Disease { get; set; }
        public DateTime AdmissionDate { get; set; }
    }

}
