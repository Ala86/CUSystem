namespace TclassLibrary
{
    partial class staffAllowance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(staffAllowance));
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.alloAmt = new System.Windows.Forms.MaskedTextBox();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.stDate = new System.Windows.Forms.DateTimePicker();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.alloGrid = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.radioButton18 = new System.Windows.Forms.RadioButton();
            this.radioButton22 = new System.Windows.Forms.RadioButton();
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
            this.textBox53 = new System.Windows.Forms.TextBox();
            this.label69 = new System.Windows.Forms.Label();
            this.label116 = new System.Windows.Forms.Label();
            this.textBox45 = new System.Windows.Forms.TextBox();
            this.ClientGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.hisNewButton = new System.Windows.Forms.Button();
            this.aloSaveButton = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dstDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alloGrid)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "End Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Start Date";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(219)))), ((int)(((byte)(141)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.alloAmt);
            this.panel1.Controls.Add(this.endDate);
            this.panel1.Controls.Add(this.stDate);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(29, 528);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(303, 130);
            this.panel1.TabIndex = 545;
            // 
            // alloAmt
            // 
            this.alloAmt.Location = new System.Drawing.Point(68, 37);
            this.alloAmt.Name = "alloAmt";
            this.alloAmt.Size = new System.Drawing.Size(100, 20);
            this.alloAmt.TabIndex = 24;
            this.alloAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.alloAmt.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.alloAmt.Validated += new System.EventHandler(this.alloAmt_Validated);
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(68, 100);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(230, 20);
            this.endDate.TabIndex = 23;
            this.endDate.ValueChanged += new System.EventHandler(this.endDate_ValueChanged);
            // 
            // stDate
            // 
            this.stDate.Location = new System.Drawing.Point(68, 70);
            this.stDate.Name = "stDate";
            this.stDate.Size = new System.Drawing.Size(230, 20);
            this.stDate.TabIndex = 22;
            this.stDate.ValueChanged += new System.EventHandler(this.stDate_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(204, 40);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(64, 17);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "Taxable";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(68, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(230, 21);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Amount ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Allowance";
            // 
            // alloGrid
            // 
            this.alloGrid.AllowUserToAddRows = false;
            this.alloGrid.AllowUserToDeleteRows = false;
            this.alloGrid.AllowUserToResizeColumns = false;
            this.alloGrid.AllowUserToResizeRows = false;
            this.alloGrid.BackgroundColor = System.Drawing.Color.White;
            this.alloGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.alloGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.alAmt,
            this.Column9,
            this.dstDate,
            this.dedDate});
            this.alloGrid.Location = new System.Drawing.Point(400, 520);
            this.alloGrid.Name = "alloGrid";
            this.alloGrid.ReadOnly = true;
            this.alloGrid.RowHeadersVisible = false;
            this.alloGrid.Size = new System.Drawing.Size(617, 181);
            this.alloGrid.TabIndex = 550;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.radioButton18);
            this.panel4.Controls.Add(this.radioButton22);
            this.panel4.Location = new System.Drawing.Point(651, 433);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(89, 65);
            this.panel4.TabIndex = 643;
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
            this.panel5.Location = new System.Drawing.Point(6, 433);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(633, 65);
            this.panel5.TabIndex = 642;
            // 
            // radioButton16
            // 
            this.radioButton16.AutoSize = true;
            this.radioButton16.Location = new System.Drawing.Point(338, 40);
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
            this.comboBox17.Location = new System.Drawing.Point(9, 33);
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
            this.radioButton9.Location = new System.Drawing.Point(147, 10);
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
            // textBox53
            // 
            this.textBox53.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.textBox53.Enabled = false;
            this.textBox53.Location = new System.Drawing.Point(52, 505);
            this.textBox53.Name = "textBox53";
            this.textBox53.ReadOnly = true;
            this.textBox53.Size = new System.Drawing.Size(280, 20);
            this.textBox53.TabIndex = 640;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(6, 508);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(49, 13);
            this.label69.TabIndex = 641;
            this.label69.Text = "Staff No.";
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.Location = new System.Drawing.Point(930, 435);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(89, 13);
            this.label116.TabIndex = 639;
            this.label116.Text = "Total Staff Found";
            // 
            // textBox45
            // 
            this.textBox45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.textBox45.Enabled = false;
            this.textBox45.Location = new System.Drawing.Point(931, 453);
            this.textBox45.Name = "textBox45";
            this.textBox45.ReadOnly = true;
            this.textBox45.Size = new System.Drawing.Size(86, 20);
            this.textBox45.TabIndex = 638;
            // 
            // ClientGrid
            // 
            this.ClientGrid.AllowUserToAddRows = false;
            this.ClientGrid.AllowUserToDeleteRows = false;
            this.ClientGrid.AllowUserToResizeColumns = false;
            this.ClientGrid.AllowUserToResizeRows = false;
            this.ClientGrid.BackgroundColor = System.Drawing.Color.White;
            this.ClientGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ClientGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Column5,
            this.dataGridViewTextBoxColumn5});
            this.ClientGrid.Location = new System.Drawing.Point(6, 6);
            this.ClientGrid.Name = "ClientGrid";
            this.ClientGrid.RowHeadersVisible = false;
            this.ClientGrid.RowTemplate.ReadOnly = true;
            this.ClientGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClientGrid.Size = new System.Drawing.Size(1011, 421);
            this.ClientGrid.TabIndex = 637;
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
            // Column5
            // 
            this.Column5.HeaderText = "Age";
            this.Column5.Name = "Column5";
            this.Column5.Width = 60;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Gender";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 70;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(840, 479);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(177, 34);
            this.button1.TabIndex = 634;
            this.button1.Text = "Allowance Type Setup";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(781, 707);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 26);
            this.panel2.TabIndex = 644;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(7, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(238, 13);
            this.label11.TabIndex = 156;
            this.label11.Text = "Copyright T-care Systems Ltd, all rights Reserved";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel6.Controls.Add(this.hisNewButton);
            this.panel6.Controls.Add(this.aloSaveButton);
            this.panel6.Controls.Add(this.button25);
            this.panel6.Location = new System.Drawing.Point(39, 668);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(296, 65);
            this.panel6.TabIndex = 588;
            // 
            // hisNewButton
            // 
            this.hisNewButton.Enabled = false;
            this.hisNewButton.Location = new System.Drawing.Point(112, 7);
            this.hisNewButton.Name = "hisNewButton";
            this.hisNewButton.Size = new System.Drawing.Size(88, 50);
            this.hisNewButton.TabIndex = 135;
            this.hisNewButton.Text = "New Allowance Details";
            this.hisNewButton.UseVisualStyleBackColor = true;
            // 
            // aloSaveButton
            // 
            this.aloSaveButton.Enabled = false;
            this.aloSaveButton.Location = new System.Drawing.Point(10, 7);
            this.aloSaveButton.Name = "aloSaveButton";
            this.aloSaveButton.Size = new System.Drawing.Size(94, 50);
            this.aloSaveButton.TabIndex = 132;
            this.aloSaveButton.Text = "Save Allowance Details";
            this.aloSaveButton.UseVisualStyleBackColor = true;
            this.aloSaveButton.Click += new System.EventHandler(this.aloSaveButton_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(207, 7);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(84, 50);
            this.button25.TabIndex = 130;
            this.button25.Text = "Exit Form";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Existing Allowance";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 200;
            // 
            // alAmt
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.alAmt.DefaultCellStyle = dataGridViewCellStyle1;
            this.alAmt.HeaderText = "Amount";
            this.alAmt.Name = "alAmt";
            this.alAmt.ReadOnly = true;
            this.alAmt.Width = 120;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Taxable";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column9.Width = 70;
            // 
            // dstDate
            // 
            this.dstDate.HeaderText = "Start Date";
            this.dstDate.Name = "dstDate";
            this.dstDate.ReadOnly = true;
            // 
            // dedDate
            // 
            this.dedDate.HeaderText = "End Date";
            this.dedDate.Name = "dedDate";
            this.dedDate.ReadOnly = true;
            // 
            // staffAllowance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1023, 732);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.textBox53);
            this.Controls.Add(this.label69);
            this.Controls.Add(this.label116);
            this.Controls.Add(this.textBox45);
            this.Controls.Add(this.ClientGrid);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.alloGrid);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "staffAllowance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "staffAllowance";
            this.Load += new System.EventHandler(this.staffAllowance_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alloGrid)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.DateTimePicker stDate;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView alloGrid;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton radioButton18;
        private System.Windows.Forms.RadioButton radioButton22;
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
        private System.Windows.Forms.TextBox textBox53;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.TextBox textBox45;
        private System.Windows.Forms.DataGridView ClientGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button hisNewButton;
        private System.Windows.Forms.Button aloSaveButton;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.MaskedTextBox alloAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn alAmt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dstDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dedDate;
    }
}