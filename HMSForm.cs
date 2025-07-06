using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}
