using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PracticeProject1;
using Serilog;


class Program
{

    static IConfiguration config;



    static Program()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json");

        
        config = builder.Build();

    }

    static string GetConnectionString()
    {
        return config.GetConnectionString("DefaultConnection");
    }


    static void AddPatient()
    {
        Console.Clear();
        Log.Information("Adding new patient");

        Console.WriteLine("➕ Add New Patient");

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Age: ");
        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("❌ Invalid age.");
            return;
        }

        Console.Write("Enter Gender: ");
        string gender = Console.ReadLine();

        Console.Write("Enter Disease: ");
        string disease = Console.ReadLine();

        Console.Write("Enter Admission Date (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime admissionDate))
        {
            Console.WriteLine("❌ Invalid date format.");
            return;
        }

        var patient = new Patient
        {
            Name = name,
            Age = age,
            Gender = gender,
            Disease = disease,
            AdmissionDate = admissionDate
        };

        using (var db = new HospitalContext())
        {
            db.Patients.Add(patient);
            db.SaveChanges();
        }
        Log.ForContext("Patient", patient, destructureObjects: true)
        .Information("Patient added successfully.");

        Console.WriteLine("✅ Patient added successfully!");
    }


    static void ViewAllPatients()
    {
        using (var context = new HospitalContext())
        {
            var patients = context.Patients.ToList();

            Log.Information("Viewing all patients");

            if (patients.Count == 0)
            {
                Console.WriteLine("❗ No patient records found.");
                Log.Warning("No patient records found in the database.");
                return;
            }

            Console.WriteLine("📋 List of All Patients:");
            foreach (var patient in patients)
            {
                Console.WriteLine($"🧾 ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}, Disease: {patient.Disease}");
            }
            Log.ForContext("Patients", patients, destructureObjects: true)
           .Information("Displayed {Count} patient records.", patients.Count);
        }
    }


    static void UpdatePatient()
    {
        Console.Clear();
        Console.WriteLine("✏️ Update Patient Record");

        Console.Write("Enter Patient ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("❌ Invalid ID entered.");
            return;
        }

        using (var db = new HospitalContext())
        {
            Log.Information("Updating patient with ID {PatientId}", patientId);

            var patient = db.Patients.FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null)
            {
                Console.WriteLine("❌ Patient not found.");
                Log.Warning("Patient with ID {PatientId} not found for update.", patientId);
                return;
            }

            Console.Write($"Enter New Name (current: {patient.Name}): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                patient.Name = name;

            Console.Write($"Enter New Age (current: {patient.Age}): ");
            string ageInput = Console.ReadLine();
            if (int.TryParse(ageInput, out int age))
                patient.Age = age;

            Console.Write($"Enter New Gender (current: {patient.Gender}): ");
            string gender = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(gender))
                patient.Gender = gender;

            Console.Write($"Enter New Disease (current: {patient.Disease}): ");
            string disease = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(disease))
                patient.Disease = disease;

            Console.Write($"Enter New Admission Date (yyyy-MM-dd, current: {patient.AdmissionDate.ToShortDateString()}): ");
            string dateInput = Console.ReadLine();
            if (DateTime.TryParse(dateInput, out DateTime admissionDate))
                patient.AdmissionDate = admissionDate;

            db.SaveChanges();
            Console.WriteLine("✅ Patient updated successfully.");
            Log.ForContext("Patient", patient, destructureObjects: true)
                .Information("Patient updated successfully.");

        }
    }


    static void DeletePatient()
    {
        Console.Clear();
        Console.WriteLine("🗑️ Delete Patient Record");

        Console.Write("Enter Patient ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            Console.WriteLine("❌ Invalid ID entered.");

            Log.Warning("Patient with ID {PatientId} not found for deletion.");
            return;
        }

        Console.Write("Are you sure you want to delete this patient? (y/n): ");
        string confirm = Console.ReadLine()?.ToLower();
        if (confirm != "y")
        {
            Console.WriteLine("❎ Deletion cancelled.");
            return;
        }

        using (var db = new HospitalContext())
        {
            Log.Information("Deleting patient ID: {PatientId}", patientId);

            var patient = db.Patients.FirstOrDefault(p => p.PatientId == patientId);
            if (patient == null)
            {
                Console.WriteLine("❌ Patient not found.");
                return;
            }

            db.Patients.Remove(patient);
            db.SaveChanges();
            Console.WriteLine("✅ Patient deleted successfully.");
            Log.ForContext("PatientId", patientId)
                .Information("Patient deleted successfully.");

        }
    }


    static void SearchPatient()
    {
        Console.Clear();


        Console.WriteLine("🔍 Search Patient");

        Console.Write("Enter patient name or part of name to search: ");
        string searchTerm = Console.ReadLine()?.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {

            Console.WriteLine("❌ Search term cannot be empty.");
            Log.Warning("Search aborted: empty input.");
            return;
        }

        using (var db = new HospitalContext())
        {
            Log.Information("Search initiated with term: {SearchTerm}", searchTerm);
            

            var matchingPatients = db.Patients
                .Where(p => p.Name.ToLower().Contains(searchTerm))
                .ToList();

            Log.Information("Searching for patients with term: {SearchTerm}", searchTerm);

            if (matchingPatients.Count == 0)
            {
                Console.WriteLine("❌ No patients found matching your search.");

                Log.Warning("No patients found matching search term: {SearchTerm}", searchTerm);
            }
            else
            {
                Console.WriteLine($"✅ Found {matchingPatients.Count} patient(s):");

                Log.ForContext("SearchResults", matchingPatients, destructureObjects: true)
               .Information("Search returned {Count} results for term: {SearchTerm}", matchingPatients.Count, searchTerm);

                foreach (var patient in matchingPatients)
                {
                    Console.WriteLine($"ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}, Disease: {patient.Disease}, Admission Date: {patient.AdmissionDate.ToShortDateString()}");
                }
            }
        }
    }


    static void Main(string[] args)
    {

        LoggerConfig.Configure();

        try
        {   
            Log.Information("Application started.");
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🏥 Hospital Management System");
                Console.WriteLine("1. Add Patient");
                Console.WriteLine("2. View All Patients");
                Console.WriteLine("3. Update Patient");
                Console.WriteLine("4. Delete Patient");
                Console.WriteLine("5. Search Patient");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddPatient();
                        break;
                    case "2":
                        ViewAllPatients();
                        break;
                    case "3":
                        UpdatePatient();
                        break;
                    case "4":
                        DeletePatient();
                        break;
                    case "5":
                        SearchPatient();
                        break;
                    case "6":
                        Log.Information("Application exited by user.");
                        return;
                    default:
                        Console.WriteLine("❌ Invalid option, please try again.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Log.ForContext("Method", "Add/Update/Delete/Search")
            .Error(ex, "An error occurred while processing the patient data.");

            Console.WriteLine($"❌ An error occurred: {ex.Message}");
        }
        finally
        {
            Log.Information("❎ Application Ended");
            Log.CloseAndFlush();
        }
    }
}

