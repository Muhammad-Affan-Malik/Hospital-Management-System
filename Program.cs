using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PracticeProject1;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;




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
            Log.Warning("Invalid age entered while adding a patient.");
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
            Log.Warning("Invalid admission date format entered while adding a patient.");
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

        try
        {
            using (var db = new HospitalContext())
            {
                db.Patients.Add(patient);
                db.SaveChanges();
            }

            Console.WriteLine("✅ Patient added successfully!");
            Log.Information("Patient \"{Name}\" (ID {Id}) added by Admin. Happiness +100. 🎉",
                            patient.Name, patient.PatientId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Failed to add patient. Please try again.");
            Log.Error(ex, "Exception occurred while adding patient \"{Name}\".", patient.Name);
        }
    }



    static void ViewAllPatients()
    {
        using (var context = new HospitalContext())
        {
            Log.Information("Viewing all patients");

            var patients = context.Patients.ToList();

            if (patients.Count == 0)
            {
                Console.WriteLine("❗ No patient records found.");
                Log.Warning("No patient records found in the database. 🕵️‍♂️🔍");
                return;
            }

            Console.WriteLine("📋 List of All Patients:");
            foreach (var patient in patients)
            {
                Console.WriteLine($"🧾 ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}, Disease: {patient.Disease}");
            }

            Log.Information("Displayed {Count} patient records. 📊", patients.Count);
            Log.ForContext("Patients", patients, destructureObjects: true)
               .Information("👀 Successfully retrieved all patient data.");
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
            Log.Error("🚫 Invalid patient ID entered for update attempt.");
            return;
        }

        using (var db = new HospitalContext())
        {
            var patient = db.Patients.FirstOrDefault(p => p.PatientId == patientId);

            if (patient == null)
            {
                Console.WriteLine("❌ Patient not found.");
                Log.Error("❓ Tried to update patient with ID {PatientId}, but no record was found. *sad trombone*", patientId);
                return;
            }

            Log.Information("✏️ Editing patient record for \"{OldName}\" (ID {PatientId})", patient.Name, patientId);

            string oldName = patient.Name;

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
            Log.ForContext("UpdatedPatient", patient, destructureObjects: true)
               .Information("✅ Patient \"{OldName}\" (ID {PatientId}) updated successfully by Admin. 🎉", oldName, patientId);
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
            Log.Error("🚫 Invalid patient ID entered for deletion attempt.");
            return;
        }

        Console.Write("Are you sure you want to delete this patient? (y/n): ");
        string confirm = Console.ReadLine()?.ToLower();
        if (confirm != "y")
        {
            Console.WriteLine("❎ Deletion cancelled.");
            Log.Warning("⚠️ Deletion of patient ID {PatientId} cancelled by user.", patientId);
            return;
        }

        using (var db = new HospitalContext())
        {
            Log.Information("Attempting to delete patient with ID: {PatientId}", patientId);

            var patient = db.Patients.FirstOrDefault(p => p.PatientId == patientId);
            if (patient == null)
            {
                Console.WriteLine("❌ Patient not found.");
                Log.Error("❓ Tried to delete patient with ID {PatientId}, but no record was found. *sad trombone*", patientId);
                return;
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            Console.WriteLine("✅ Patient deleted successfully.");
            Log.ForContext("DeletedPatient", patient, destructureObjects: true)
               .Information("🗑️ Patient \"{Name}\" (ID {PatientId}) deleted by Admin. Goodbye and good luck. 🌈", patient.Name, patientId);
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
            Log.Warning("🚫 Search aborted: empty input provided by user.");
            return;
        }

        using (var db = new HospitalContext())
        {
            Log.Information("🔍 Search initiated for term: \"{SearchTerm}\"", searchTerm);

            var matchingPatients = db.Patients
                .Where(p => p.Name.ToLower().Contains(searchTerm))
                .ToList();

            if (matchingPatients.Count == 0)
            {
                Console.WriteLine("❌ No patients found matching your search.");
                Log.Warning("🔎 No patients found for search term: \"{SearchTerm}\"", searchTerm);
            }
            else
            {
                Console.WriteLine($"✅ Found {matchingPatients.Count} patient(s):");

                foreach (var patient in matchingPatients)
                {
                    Console.WriteLine($"🧾 ID: {patient.PatientId}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}, Disease: {patient.Disease}, Admission Date: {patient.AdmissionDate.ToShortDateString()}");
                }

                Log.ForContext("SearchResults", matchingPatients, destructureObjects: true)
                    .Information("📄 Search completed: {Count} patient(s) matched term \"{SearchTerm}\"", matchingPatients.Count, searchTerm);
            }
        }
    }



    static void ExportPatientsToJson()
    {
        Console.Clear();
        Console.WriteLine("💾 Exporting Patients Data to JSON File...");

        try
        {
            using (var db = new HospitalContext())
            {
                var patients = db.Patients.ToList();

                if (patients.Count == 0)
                {
                    Console.WriteLine("❗ No patients found to export.");
                    Log.Warning("No patient data found. Export aborted.");
                    return;
                }

                string backupDirectory = "backup";

                // Ensure the directory exists
                if (!Directory.Exists(backupDirectory))
                    Directory.CreateDirectory(backupDirectory);

                string fileName = $"patients_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                string fullPath = Path.Combine(backupDirectory, fileName);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // Makes JSON easy to read
                };

                string json = JsonSerializer.Serialize(patients, options);
                File.WriteAllText(fullPath, json);

                Console.WriteLine($"✅ Data exported successfully to {fullPath}");
                Log.Information("Exported {Count} patients to JSON file: {FilePath}", patients.Count, fullPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error exporting data. See logs for details.");
            Log.Error(ex, "Failed to export patients to JSON file.");
        }
    }


    static void ImportPatientsFromJson()
    {
        Console.Clear();
        Console.WriteLine("📂 Import Patients Data from JSON File");

        Console.Write("Enter the JSON filename (inside 'backup' folder): ");
        string fileName = Console.ReadLine();

        string backupDirectory = "backup";
        string fullPath = Path.Combine(backupDirectory, fileName);

        if (!File.Exists(fullPath))
        {
            Console.WriteLine("❌ File not found.");
            Log.Warning("Import failed. File not found: {FilePath}", fullPath);
            return;
        }

        try
        {
            string json = File.ReadAllText(fullPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var importedPatients = JsonSerializer.Deserialize<List<Patient>>(json, options);

            if (importedPatients == null || importedPatients.Count == 0)
            {
                Console.WriteLine("❗ No patient records found in the file.");
                Log.Warning("Import aborted. No valid patient data found in: {FilePath}", fullPath);
                return;
            }

            using (var db = new HospitalContext())
            {
                foreach (var patient in importedPatients)
                {
                    // Avoid duplicating based on unique constraints (optional logic)
                    bool exists = db.Patients.Any(p =>
                        p.Name == patient.Name &&
                        p.Age == patient.Age &&
                        p.Gender == patient.Gender &&
                        p.Disease == patient.Disease &&
                        p.AdmissionDate == patient.AdmissionDate);

                    if (!exists)
                    {
                        db.Patients.Add(patient);
                    }
                }

                db.SaveChanges();
            }

            Console.WriteLine($"✅ Successfully imported {importedPatients.Count} patients from file.");
            Log.Information("Imported {Count} patients from JSON file: {FilePath}", importedPatients.Count, fullPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error occurred while importing.");
            Log.Error(ex, "Failed to import patients from JSON file: {FilePath}", fullPath);
        }
    }

    static void LaunchWinFormUI()
    {
        Thread formThread = new Thread(() =>
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HMSForm()); // ✅ Replace with your form's class name
        });

        formThread.SetApartmentState(ApartmentState.STA); // 🧠 Important for WinForms
        formThread.Start();

        Log.Information("🪟 WinForm launched from console.");
        Console.WriteLine("✅ WinForm launched in a new window.");
    }


    static void Main(string[] args)
    {

        LoggerConfig.Configure();
        Log.Information("🚀 Test log at app startup");


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
                Console.WriteLine("6. Export Patients to JSON");
                Console.WriteLine("7. Import Patients from JSON");
                Console.WriteLine("8. Launch WinForms UI");
                Console.WriteLine("9. Exit");
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
                        ExportPatientsToJson();
                        break;
                    case "7":
                        ImportPatientsFromJson();
                        break;
                    case "8":
                        LaunchWinFormUI(); 
                        break;
                    case "9":
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

