namespace WinTcare
{
    partial class Specimen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Specimen));
            this.clientgrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specGrid = new System.Windows.Forms.DataGridView();
            this.testName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.testSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button15 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.clientgrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.specGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // clientgrid
            // 
            this.clientgrid.AllowUserToAddRows = false;
            this.clientgrid.BackgroundColor = System.Drawing.Color.White;
            this.clientgrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientgrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.clientgrid.Location = new System.Drawing.Point(12, 12);
            this.clientgrid.Name = "clientgrid";
            this.clientgrid.ReadOnly = true;
            this.clientgrid.RowHeadersVisible = false;
            this.clientgrid.Size = new System.Drawing.Size(909, 217);
            this.clientgrid.TabIndex = 0;
            this.clientgrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientgrid_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Client ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 60;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Client Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 270;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Test";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Indication";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 150;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Req. by";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Req. Date";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 70;
            // 
            // specGrid
            // 
            this.specGrid.BackgroundColor = System.Drawing.Color.White;
            this.specGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.specGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.testName,
            this.specName,
            this.testSelect});
            this.specGrid.Location = new System.Drawing.Point(437, 246);
            this.specGrid.Name = "specGrid";
            this.specGrid.RowHeadersVisible = false;
            this.specGrid.Size = new System.Drawing.Size(484, 77);
            this.specGrid.TabIndex = 1;
            this.specGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.specGrid_CellValueChanged);
            this.specGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.specGrid_CurrentCellDirtyStateChanged);
            // 
            // testName
            // 
            this.testName.HeaderText = "Test Name";
            this.testName.Name = "testName";
            this.testName.Width = 200;
            // 
            // specName
            // 
            this.specName.HeaderText = "Specimen Name";
            this.specName.Name = "specName";
            this.specName.Width = 200;
            // 
            // testSelect
            // 
            this.testSelect.HeaderText = "Received";
            this.testSelect.Name = "testSelect";
            this.testSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.testSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.testSelect.Width = 75;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(30, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(70, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "All Clients";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 246);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 46);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sort by";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(120, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(74, 17);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "One Client";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 323);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 276;
            this.label2.Text = "Search by Client ID";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(112, 322);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 275;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(359, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 277;
            this.label1.Text = "Drawn By";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(437, 341);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(484, 21);
            this.comboBox1.TabIndex = 278;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.button15);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.button7);
            this.panel3.Location = new System.Drawing.Point(12, 388);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(437, 54);
            this.panel3.TabIndex = 279;
            // 
            // button15
            // 
            this.button15.Enabled = false;
            this.button15.Location = new System.Drawing.Point(237, 7);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(110, 40);
            this.button15.TabIndex = 131;
            this.button15.Text = "Specimen Setup";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(144, 7);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(87, 40);
            this.button5.TabIndex = 35;
            this.button5.Text = "Un-archive";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(8, 7);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(130, 40);
            this.SaveButton.TabIndex = 30;
            this.SaveButton.Text = "Specimen Receipt Print";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(354, 7);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 40);
            this.button7.TabIndex = 130;
            this.button7.Text = "Exit Form";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // Specimen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(933, 446);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.specGrid);
            this.Controls.Add(this.clientgrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Specimen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specimen";
            this.Load += new System.EventHandler(this.Specimen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.clientgrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.specGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView clientgrid;
        private System.Windows.Forms.DataGridView specGrid;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn testName;
        private System.Windows.Forms.DataGridViewTextBoxColumn specName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn testSelect;
    }
}