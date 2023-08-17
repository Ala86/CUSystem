﻿namespace TclassLibrary
{
    partial class ExitService
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExitService));
            this.exitGrid = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dexcel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgood = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dfair = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dpoor = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.exitDate = new System.Windows.Forms.DateTimePicker();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.exitDetails = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton18 = new System.Windows.Forms.RadioButton();
            this.radioButton22 = new System.Windows.Forms.RadioButton();
            this.label116 = new System.Windows.Forms.Label();
            this.textBox45 = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButton16 = new System.Windows.Forms.RadioButton();
            this.radioButton15 = new System.Windows.Forms.RadioButton();
            this.radioButton14 = new System.Windows.Forms.RadioButton();
            this.textBox47 = new System.Windows.Forms.TextBox();
            this.radioButton13 = new System.Windows.Forms.RadioButton();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.comboBox17 = new System.Windows.Forms.ComboBox();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.ClientGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox53 = new System.Windows.Forms.TextBox();
            this.label69 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.hisNewButton = new System.Windows.Forms.Button();
            this.extSaveButton = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.exitGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientGrid)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // exitGrid
            // 
            this.exitGrid.AllowUserToAddRows = false;
            this.exitGrid.AllowUserToDeleteRows = false;
            this.exitGrid.AllowUserToResizeColumns = false;
            this.exitGrid.AllowUserToResizeRows = false;
            this.exitGrid.BackgroundColor = System.Drawing.Color.White;
            this.exitGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.exitGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.dexcel,
            this.dgood,
            this.dfair,
            this.dpoor});
            this.exitGrid.Location = new System.Drawing.Point(366, 463);
            this.exitGrid.Name = "exitGrid";
            this.exitGrid.RowHeadersVisible = false;
            this.exitGrid.Size = new System.Drawing.Size(657, 161);
            this.exitGrid.TabIndex = 475;
            this.exitGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.exitGrid_CellContentClick);
            this.exitGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.exitGrid_CellValueChanged);
            this.exitGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.exitGrid_CurrentCellDirtyStateChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Question";
            this.Column1.Name = "Column1";
            this.Column1.Width = 300;
            // 
            // dexcel
            // 
            this.dexcel.HeaderText = "Excellent";
            this.dexcel.Name = "dexcel";
            this.dexcel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dexcel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dexcel.Width = 85;
            // 
            // dgood
            // 
            this.dgood.HeaderText = "Good";
            this.dgood.Name = "dgood";
            this.dgood.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgood.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgood.Width = 85;
            // 
            // dfair
            // 
            this.dfair.HeaderText = "Fair";
            this.dfair.Name = "dfair";
            this.dfair.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dfair.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dfair.Width = 85;
            // 
            // dpoor
            // 
            this.dpoor.HeaderText = "Poor";
            this.dpoor.Name = "dpoor";
            this.dpoor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dpoor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dpoor.Width = 85;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(372, 445);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 476;
            this.label3.Text = "Exit Interview Result";
            // 
            // exitDate
            // 
            this.exitDate.Location = new System.Drawing.Point(79, 493);
            this.exitDate.Name = "exitDate";
            this.exitDate.Size = new System.Drawing.Size(280, 20);
            this.exitDate.TabIndex = 472;
            this.exitDate.Validated += new System.EventHandler(this.exitDate_Validated);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(79, 467);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(280, 21);
            this.comboBox2.TabIndex = 471;
            this.comboBox2.SelectedValueChanged += new System.EventHandler(this.comboBox2_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 471);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 470;
            this.label5.Text = "Exit Reason";
            // 
            // exitDetails
            // 
            this.exitDetails.Location = new System.Drawing.Point(79, 519);
            this.exitDetails.Name = "exitDetails";
            this.exitDetails.Size = new System.Drawing.Size(280, 80);
            this.exitDetails.TabIndex = 474;
            this.exitDetails.Text = "";
            this.exitDetails.Validated += new System.EventHandler(this.exitDetails_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 497);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 469;
            this.label1.Text = "Exit Date";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(786, 642);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 26);
            this.panel2.TabIndex = 467;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(7, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(238, 13);
            this.label11.TabIndex = 155;
            this.label11.Text = "Copyright T-care Systems Ltd, all rights Reserved";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 524);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 473;
            this.label2.Text = "Exit Details";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(850, 425);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 22);
            this.button1.TabIndex = 463;
            this.button1.Text = "Exit factors setup";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioButton18);
            this.panel1.Controls.Add(this.radioButton22);
            this.panel1.Location = new System.Drawing.Point(653, 368);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(89, 65);
            this.panel1.TabIndex = 629;
            // 
            // radioButton18
            // 
            this.radioButton18.AutoSize = true;
            this.radioButton18.Location = new System.Drawing.Point(3, 11);
            this.radioButton18.Name = "radioButton18";
            this.radioButton18.Size = new System.Drawing.Size(59, 17);
            this.radioButton18.TabIndex = 573;
            this.radioButton18.Text = "Female";
            this.radioButton18.UseVisualStyleBackColor = true;
            this.radioButton18.CheckedChanged += new System.EventHandler(this.radioButton18_CheckedChanged);
            // 
            // radioButton22
            // 
            this.radioButton22.AutoSize = true;
            this.radioButton22.Location = new System.Drawing.Point(3, 39);
            this.radioButton22.Name = "radioButton22";
            this.radioButton22.Size = new System.Drawing.Size(48, 17);
            this.radioButton22.TabIndex = 1;
            this.radioButton22.Text = "Male";
            this.radioButton22.UseVisualStyleBackColor = true;
            this.radioButton22.CheckedChanged += new System.EventHandler(this.radioButton22_CheckedChanged);
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.Location = new System.Drawing.Point(930, 373);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(89, 13);
            this.label116.TabIndex = 628;
            this.label116.Text = "Total Staff Found";
            // 
            // textBox45
            // 
            this.textBox45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.textBox45.Enabled = false;
            this.textBox45.Location = new System.Drawing.Point(933, 391);
            this.textBox45.Name = "textBox45";
            this.textBox45.ReadOnly = true;
            this.textBox45.Size = new System.Drawing.Size(86, 20);
            this.textBox45.TabIndex = 627;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.radioButton16);
            this.panel5.Controls.Add(this.radioButton15);
            this.panel5.Controls.Add(this.radioButton14);
            this.panel5.Controls.Add(this.textBox47);
            this.panel5.Controls.Add(this.radioButton13);
            this.panel5.Controls.Add(this.radioButton11);
            this.panel5.Controls.Add(this.comboBox17);
            this.panel5.Controls.Add(this.radioButton10);
            this.panel5.Controls.Add(this.radioButton9);
            this.panel5.Controls.Add(this.radioButton8);
            this.panel5.Location = new System.Drawing.Point(8, 368);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(633, 65);
            this.panel5.TabIndex = 626;
            // 
            // radioButton16
            // 
            this.radioButton16.AutoSize = true;
            this.radioButton16.Location = new System.Drawing.Point(340, 41);
            this.radioButton16.Name = "radioButton16";
            this.radioButton16.Size = new System.Drawing.Size(79, 17);
            this.radioButton16.TabIndex = 575;
            this.radioButton16.TabStop = true;
            this.radioButton16.Text = "By Staff No";
            this.radioButton16.UseVisualStyleBackColor = true;
            // 
            // radioButton15
            // 
            this.radioButton15.AutoSize = true;
            this.radioButton15.Location = new System.Drawing.Point(530, 10);
            this.radioButton15.Name = "radioButton15";
            this.radioButton15.Size = new System.Drawing.Size(95, 17);
            this.radioButton15.TabIndex = 574;
            this.radioButton15.TabStop = true;
            this.radioButton15.Text = "By Cost Centre";
            this.radioButton15.UseVisualStyleBackColor = true;
            this.radioButton15.CheckedChanged += new System.EventHandler(this.radioButton15_CheckedChanged);
            // 
            // radioButton14
            // 
            this.radioButton14.AutoSize = true;
            this.radioButton14.Location = new System.Drawing.Point(57, 10);
            this.radioButton14.Name = "radioButton14";
            this.radioButton14.Size = new System.Drawing.Size(74, 17);
            this.radioButton14.TabIndex = 573;
            this.radioButton14.TabStop = true;
            this.radioButton14.Text = "By Branch";
            this.radioButton14.UseVisualStyleBackColor = true;
            this.radioButton14.CheckedChanged += new System.EventHandler(this.radioButton14_CheckedChanged);
            // 
            // textBox47
            // 
            this.textBox47.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.textBox47.Location = new System.Drawing.Point(421, 40);
            this.textBox47.Name = "textBox47";
            this.textBox47.Size = new System.Drawing.Size(127, 20);
            this.textBox47.TabIndex = 572;
            // 
            // radioButton13
            // 
            this.radioButton13.AutoSize = true;
            this.radioButton13.Location = new System.Drawing.Point(229, 11);
            this.radioButton13.Name = "radioButton13";
            this.radioButton13.Size = new System.Drawing.Size(96, 17);
            this.radioButton13.TabIndex = 555;
            this.radioButton13.Text = "By Designation";
            this.radioButton13.UseVisualStyleBackColor = true;
            this.radioButton13.CheckedChanged += new System.EventHandler(this.radioButton13_CheckedChanged);
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Location = new System.Drawing.Point(433, 10);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(80, 17);
            this.radioButton11.TabIndex = 553;
            this.radioButton11.TabStop = true;
            this.radioButton11.Text = "By Ethnicity";
            this.radioButton11.UseVisualStyleBackColor = true;
            this.radioButton11.CheckedChanged += new System.EventHandler(this.radioButton11_CheckedChanged);
            // 
            // comboBox17
            // 
            this.comboBox17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.comboBox17.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox17.FormattingEnabled = true;
            this.comboBox17.Location = new System.Drawing.Point(3, 29);
            this.comboBox17.Name = "comboBox17";
            this.comboBox17.Size = new System.Drawing.Size(230, 21);
            this.comboBox17.TabIndex = 552;
            this.comboBox17.SelectedIndexChanged += new System.EventHandler(this.comboBox17_SelectedIndexChanged);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Location = new System.Drawing.Point(338, 10);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(65, 17);
            this.radioButton10.TabIndex = 2;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "By Band";
            this.radioButton10.UseVisualStyleBackColor = true;
            this.radioButton10.CheckedChanged += new System.EventHandler(this.radioButton10_CheckedChanged);
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(137, 10);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(63, 17);
            this.radioButton9.TabIndex = 1;
            this.radioButton9.Text = "By Dept";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton9_CheckedChanged);
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Checked = true;
            this.radioButton8.Location = new System.Drawing.Point(9, 10);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(36, 17);
            this.radioButton8.TabIndex = 0;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "All";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // ClientGrid
            // 
            this.ClientGrid.BackgroundColor = System.Drawing.Color.White;
            this.ClientGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ClientGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.Column6});
            this.ClientGrid.Location = new System.Drawing.Point(8, 6);
            this.ClientGrid.Name = "ClientGrid";
            this.ClientGrid.RowHeadersVisible = false;
            this.ClientGrid.RowTemplate.ReadOnly = true;
            this.ClientGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClientGrid.Size = new System.Drawing.Size(1011, 355);
            this.ClientGrid.TabIndex = 625;
            this.ClientGrid.Click += new System.EventHandler(this.ClientGrid_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Staff No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Staff Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 230;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Department";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 200;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Designation";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 320;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Age";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Gender";
            this.Column6.Name = "Column6";
            this.Column6.Width = 70;
            // 
            // textBox53
            // 
            this.textBox53.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.textBox53.Enabled = false;
            this.textBox53.Location = new System.Drawing.Point(79, 440);
            this.textBox53.Name = "textBox53";
            this.textBox53.ReadOnly = true;
            this.textBox53.Size = new System.Drawing.Size(280, 20);
            this.textBox53.TabIndex = 630;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(8, 443);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(49, 13);
            this.label69.TabIndex = 631;
            this.label69.Text = "Staff No.";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel6.Controls.Add(this.hisNewButton);
            this.panel6.Controls.Add(this.extSaveButton);
            this.panel6.Controls.Add(this.button25);
            this.panel6.Location = new System.Drawing.Point(24, 605);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(335, 65);
            this.panel6.TabIndex = 632;
            // 
            // hisNewButton
            // 
            this.hisNewButton.Location = new System.Drawing.Point(116, 7);
            this.hisNewButton.Name = "hisNewButton";
            this.hisNewButton.Size = new System.Drawing.Size(88, 50);
            this.hisNewButton.TabIndex = 135;
            this.hisNewButton.Text = "New Exit Details";
            this.hisNewButton.UseVisualStyleBackColor = true;
            // 
            // extSaveButton
            // 
            this.extSaveButton.Enabled = false;
            this.extSaveButton.Location = new System.Drawing.Point(3, 3);
            this.extSaveButton.Name = "extSaveButton";
            this.extSaveButton.Size = new System.Drawing.Size(91, 50);
            this.extSaveButton.TabIndex = 132;
            this.extSaveButton.Text = "Save Exit Details";
            this.extSaveButton.UseVisualStyleBackColor = true;
            this.extSaveButton.Click += new System.EventHandler(this.extSaveButton_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(216, 7);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(107, 50);
            this.button25.TabIndex = 130;
            this.button25.Text = "Exit Form";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(543, 627);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(162, 40);
            this.button2.TabIndex = 136;
            this.button2.Text = "Save Interview Questions";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ExitService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1031, 669);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.textBox53);
            this.Controls.Add(this.label69);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label116);
            this.Controls.Add(this.textBox45);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.ClientGrid);
            this.Controls.Add(this.exitGrid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.exitDate);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.exitDetails);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExitService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExitService";
            this.Load += new System.EventHandler(this.ExitService_Load);
            ((System.ComponentModel.ISupportInitialize)(this.exitGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientGrid)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView exitGrid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker exitDate;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox exitDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton18;
        private System.Windows.Forms.RadioButton radioButton22;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.TextBox textBox45;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton radioButton16;
        private System.Windows.Forms.RadioButton radioButton15;
        private System.Windows.Forms.RadioButton radioButton14;
        private System.Windows.Forms.TextBox textBox47;
        private System.Windows.Forms.RadioButton radioButton13;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.ComboBox comboBox17;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.DataGridView ClientGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.TextBox textBox53;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button hisNewButton;
        private System.Windows.Forms.Button extSaveButton;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dexcel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgood;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dfair;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dpoor;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label11;
    }
}