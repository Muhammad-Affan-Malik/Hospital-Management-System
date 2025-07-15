using Microsoft.EntityFrameworkCore;
using HMS.WebAPI.Controllers;
using HMS.WebAPI.Models; // Ensure this matches where your Patient.cs is

namespace HMS.WebAPI
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
    }
}
