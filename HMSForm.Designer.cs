namespace PracticeProject1
{
    partial class HMSForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            txtName = new TextBox();
            txtAge = new TextBox();
            BoxGender = new ComboBox();
            txtDisease = new TextBox();
            dateTimePicker = new DateTimePicker();
            btnAdd = new Button();
            AddPatientLabel = new Label();
            btnUpdate = new Button();
            Search = new Button();
            txtSearch = new TextBox();
            SearchLabel = new Label();
            btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(270, 338);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(721, 188);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // txtName
            // 
            txtName.Location = new Point(309, 55);
            txtName.Name = "txtName";
            txtName.Size = new Size(250, 27);
            txtName.TabIndex = 1;
            txtName.Text = "Name";
            // 
            // txtAge
            // 
            txtAge.Location = new Point(309, 99);
            txtAge.Name = "txtAge";
            txtAge.Size = new Size(250, 27);
            txtAge.TabIndex = 2;
            txtAge.Text = "Age";
            // 
            // BoxGender
            // 
            BoxGender.DropDownStyle = ComboBoxStyle.DropDownList;
            BoxGender.FormattingEnabled = true;
            BoxGender.Items.AddRange(new object[] { "Male", "Female", "Others" });
            BoxGender.Location = new Point(309, 148);
            BoxGender.Name = "BoxGender";
            BoxGender.Size = new Size(151, 28);
            BoxGender.TabIndex = 3;
            // 
            // txtDisease
            // 
            txtDisease.Location = new Point(309, 193);
            txtDisease.Name = "txtDisease";
            txtDisease.Size = new Size(250, 27);
            txtDisease.TabIndex = 4;
            txtDisease.Text = "Disease";
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(309, 240);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(250, 27);
            dateTimePicker.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = SystemColors.ActiveCaption;
            btnAdd.Location = new Point(169, 293);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(165, 29);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "ADD PATIENT";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // AddPatientLabel
            // 
            AddPatientLabel.AutoSize = true;
            AddPatientLabel.Location = new Point(356, 23);
            AddPatientLabel.Name = "AddPatientLabel";
            AddPatientLabel.Size = new Size(148, 20);
            AddPatientLabel.TabIndex = 7;
            AddPatientLabel.Text = "ADD PATIENT'S INFO";
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = SystemColors.ActiveCaption;
            btnUpdate.Location = new Point(351, 293);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(165, 29);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "Update Patient";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // Search
            // 
            Search.BackColor = SystemColors.ActiveCaption;
            Search.Location = new Point(911, 305);
            Search.Name = "Search";
            Search.Size = new Size(80, 29);
            Search.TabIndex = 9;
            Search.Text = "Search";
            Search.UseVisualStyleBackColor = false;
            Search.Click += Search_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(673, 304);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(232, 27);
            txtSearch.TabIndex = 10;
            // 
            // SearchLabel
            // 
            SearchLabel.AutoSize = true;
            SearchLabel.Location = new Point(563, 307);
            SearchLabel.Name = "SearchLabel";
            SearchLabel.Size = new Size(104, 20);
            SearchLabel.TabIndex = 11;
            SearchLabel.Text = "Search Record";
            // 
            // btnDelete
            // 
            btnDelete.BackColor = SystemColors.ActiveCaption;
            btnDelete.Location = new Point(607, 255);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(165, 29);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // HMSForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1378, 538);
            Controls.Add(btnDelete);
            Controls.Add(SearchLabel);
            Controls.Add(txtSearch);
            Controls.Add(Search);
            Controls.Add(btnUpdate);
            Controls.Add(AddPatientLabel);
            Controls.Add(btnAdd);
            Controls.Add(dateTimePicker);
            Controls.Add(txtDisease);
            Controls.Add(BoxGender);
            Controls.Add(txtAge);
            Controls.Add(txtName);
            Controls.Add(dataGridView1);
            Name = "HMSForm";
            Text = "HMSForm";
            Load += HMSForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox txtName;
        private TextBox txtAge;
        private ComboBox BoxGender;
        private TextBox txtDisease;
        private DateTimePicker dateTimePicker;
        private Button btnAdd;
        private Label AddPatientLabel;
        private Button btnUpdate;
        private Button Search;
        private TextBox txtSearch;
        private Label SearchLabel;
        private Button btnDelete;
    }
}