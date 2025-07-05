using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeProject1
{
    using Microsoft.EntityFrameworkCore;

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Paste your connection string here
            optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER02;Database=HMS;Trusted_Connection=True;Encrypt=False;");
        }
    }

}
