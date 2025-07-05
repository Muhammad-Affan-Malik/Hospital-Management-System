
# Hospital Management System - Console App (.NET 8)

A fully functional Hospital Management System built using .NET 8 Console App. This project is designed for learning and demonstration purposes, showcasing practical implementation of:

Entity Framework Core (Database Handling)
CRUD operations on Patient Records
JSON-based Data Backup & Import
Human-readable Logging with Serilog
Clean code, modular functions, and professional practices

Features Implemented
Core Functionality
Add New Patient
View All Patients
Update Existing Patient
Delete Patient Record**
earch Patient by Name or Disease
Export patient's database to Json file for backup
import the database of system looses or crashes

Each feature interacts directly with a SQL Server database using Entity Framework Core for safe and efficient data management.

Logging (Serilog)
All operations are logged with professional, human-readable messages, such as:
```
[2025-07-06] INFORMATION: Patient "Leo" (ID 15) updated by Admin. Data refreshed. 🎉
[2025-07-06] ERROR: Tried to delete non-existent patient. *sad trombone*
```

Logging types:
- `Information`: For successful operations
- `Warning`: For empty results, invalid input, or skipped actions
- `Error`: For unexpected or failed operations

Logs are stored in a daily log file under a `Logs/` directory.

---

Backup & Restore (Dump Feature)
Export Patient Data to JSON** (`/backup/patients_backup.json`)
Import Data from Backup** to restore all records

This uses JSON serialization and ensures you can make a full database snapshot for later use.

---

Tech Stack
- `.NET 8` Console App
- `Entity Framework Core` (Code First Approach)
- `SQL Server`
- `Serilog` for logging
- `Newtonsoft.Json` for backup/restore
- Modular and Clean Codebase

---

Folder Structure

```
/Logs/                 => Daily human-readable logs
/backup/               => JSON backup files
HospitalContext.cs     => EF DbContext
Program.cs             => Entry point with menu
Models/                => Patient.cs (data model)
Utils/                 => Backup and Import methods
```

---

How to Run

1. Clone the repository
2. Set your SQL Server connection in `HospitalContext.cs`
3. Run the app via Visual Studio or CLI
4. Use the menu to interact with the system



Author

Made by Affan Malik for learning and personal development.  
Feel free to contribute, fork, or explore!
