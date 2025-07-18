﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using static PracticeProject1.HospitalContext; // Assuming you have a HospitalContext class in the same namespace
using static PracticeProject1.Patient; // Assuming you have a Patient class in the same namespace

namespace PracticeProject1
{
    public partial class HMSForm : Form
    {

        private readonly HospitalContext _context;

        private int selectedPatientId = -1;

        public HMSForm()
        {
            InitializeComponent();
            _context = new HospitalContext();
            LoadPatientsIntoGrid();
        }

        private void LoadPatientsIntoGrid()
        {
            try
            {
                var patients = _context.Patients.ToList();
                dataGridView1.DataSource = patients;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Failed to load patients: {ex.Message}");
            }
        }

        private void HMSForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newPatient = new Patient
                {
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    Gender = BoxGender.SelectedItem?.ToString() ?? "Unspecified",
                    Disease = txtDisease.Text,
                    AdmissionDate = dateTimePicker.Value.Date,
                };

                _context.Patients.Add(newPatient);
                _context.SaveChanges();

                MessageBox.Show("✅ Patient added successfully!");
                LoadPatientsIntoGrid(); // Refresh grid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Failed to add patient: {ex.Message}");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ignore header row
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedPatientId = Convert.ToInt32(row.Cells["PatientId"].Value); // Ensure PatientId column exists

                txtName.Text = row.Cells["Name"].Value.ToString();
                txtAge.Text = row.Cells["Age"].Value.ToString();
                BoxGender.Text = row.Cells["Gender"].Value.ToString();
                txtDisease.Text = row.Cells["Disease"].Value.ToString();
                dateTimePicker.Value = Convert.ToDateTime(row.Cells["AdmissionDate"].Value);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedPatientId == -1)
            {
                MessageBox.Show("Please select a patient to update.");
                return;
            }

            var patient = _context.Patients.Find(selectedPatientId);
            if (patient != null)
            {
                patient.Name = txtName.Text;
                patient.Age = int.Parse(txtAge.Text);
                patient.Gender = BoxGender.Text;
                patient.Disease = txtDisease.Text;
                patient.AdmissionDate = dateTimePicker.Value;

                _context.SaveChanges();

                MessageBox.Show("✅ Patient updated successfully!");
                LoadPatientsIntoGrid();
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            var results = _context.Patients
                .Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Disease.ToLower().Contains(searchTerm))
                .ToList();

            if (results.Count == 0)
            {
                MessageBox.Show("No matching patients found.");
            }

            dataGridView1.DataSource = results;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient to delete.");
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete the selected patient?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes)
                return;

            int selectedPatientId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PatientId"].Value);
            var patientToDelete = _context.Patients.FirstOrDefault(p => p.PatientId == selectedPatientId);

            if (patientToDelete != null)
            {
                _context.Patients.Remove(patientToDelete);
                _context.SaveChanges();
                MessageBox.Show("Patient deleted successfully.");
                LoadPatientsIntoGrid(); // Refresh the grid
            }
            else
            {
                MessageBox.Show("Selected patient not found in database.");
            }
        }

        private void btnExportJson_Click(object sender, EventArgs e)
        {
            var patients = _context.Patients.ToList();

            if (patients.Count == 0)
            {
                MessageBox.Show("No patient data to export.");
                return;
            }

            string json = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "patients_backup.json");
            File.WriteAllText(filePath, json);

            MessageBox.Show($"Patient data exported successfully to:\n{filePath}");
        }

        private void btnImportJson_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backup", "patients_backup.json");

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Backup file not found.");
                return;
            }

            string json = File.ReadAllText(filePath);
            var importedPatients = JsonSerializer.Deserialize<List<Patient>>(json);

            if (importedPatients == null || importedPatients.Count == 0)
            {
                MessageBox.Show("No data found in the backup file.");
                return;
            }

            int addedCount = 0;
            foreach (var patient in importedPatients)
            {
                // Ignore the original ID and let DB generate a new one
                patient.PatientId = 0;

                bool exists = _context.Patients.Any(p =>
                    p.Name == patient.Name &&
                    p.Age == patient.Age &&
                    p.Gender == patient.Gender &&
                    p.Disease == patient.Disease &&
                    p.AdmissionDate == patient.AdmissionDate);

                if (!exists)
                {
                    _context.Patients.Add(patient);
                    addedCount++;
                }
            }

            _context.SaveChanges();
            LoadPatientsIntoGrid();

            MessageBox.Show($"{addedCount} patients imported successfully.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
