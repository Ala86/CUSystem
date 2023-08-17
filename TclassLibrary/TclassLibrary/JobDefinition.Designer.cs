namespace TclassLibrary
{
    partial class JobDefinition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobDefinition));
            this.label7 = new System.Windows.Forms.Label();
            this.jobGrid = new System.Windows.Forms.DataGridView();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButton14 = new System.Windows.Forms.RadioButton();
            this.comboBox17 = new System.Windows.Forms.ComboBox();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.hisNewButton = new System.Windows.Forms.Button();
            this.jobSaveButton = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dGrade = new System.Windows.Forms.MaskedTextBox();
            this.dGpoint = new System.Windows.Forms.MaskedTextBox();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.jobGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 499;
            this.label7.Text = "All Jobs ";
            // 
            // jobGrid
            // 
            this.jobGrid.AllowUserToAddRows = false;
            this.jobGrid.AllowUserToDeleteRows = false;
            this.jobGrid.AllowUserToResizeColumns = false;
            this.jobGrid.AllowUserToResizeRows = false;
            this.jobGrid.BackgroundColor = System.Drawing.Color.White;
            this.jobGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column6,
            this.Column2,
            this.Column7});
            this.jobGrid.Location = new System.Drawing.Point(19, 137);
            this.jobGrid.Name = "jobGrid";
            this.jobGrid.RowHeadersVisible = false;
            this.jobGrid.Size = new System.Drawing.Size(737, 337);
            this.jobGrid.TabIndex = 498;
            // 
            // comboBox7
            // 
            this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point(88, 4);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(280, 21);
            this.comboBox7.TabIndex = 100;
            this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 501;
            this.label5.Text = "Job Function";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(88, 51);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(280, 21);
            this.comboBox1.TabIndex = 103;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 503;
            this.label13.Text = "Department";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(530, 560);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 26);
            this.panel2.TabIndex = 507;
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
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(88, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(280, 20);
            this.textBox1.TabIndex = 102;
            this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 508;
            this.label4.Text = "Job Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 513;
            this.label3.Text = "Reports to";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(88, 74);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(280, 21);
            this.comboBox3.TabIndex = 105;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.radioButton14);
            this.panel5.Controls.Add(this.comboBox17);
            this.panel5.Controls.Add(this.radioButton9);
            this.panel5.Controls.Add(this.radioButton8);
            this.panel5.Location = new System.Drawing.Point(427, 495);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(329, 60);
            this.panel5.TabIndex = 630;
            // 
            // radioButton14
            // 
            this.radioButton14.AutoSize = true;
            this.radioButton14.Location = new System.Drawing.Point(57, 6);
            this.radioButton14.Name = "radioButton14";
            this.radioButton14.Size = new System.Drawing.Size(74, 17);
            this.radioButton14.TabIndex = 573;
            this.radioButton14.TabStop = true;
            this.radioButton14.Text = "By Branch";
            this.radioButton14.UseVisualStyleBackColor = true;
            // 
            // comboBox17
            // 
            this.comboBox17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.comboBox17.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox17.FormattingEnabled = true;
            this.comboBox17.Location = new System.Drawing.Point(9, 28);
            this.comboBox17.Name = "comboBox17";
            this.comboBox17.Size = new System.Drawing.Size(304, 21);
            this.comboBox17.TabIndex = 552;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Location = new System.Drawing.Point(147, 6);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(63, 17);
            this.radioButton9.TabIndex = 1;
            this.radioButton9.Text = "By Dept";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Checked = true;
            this.radioButton8.Location = new System.Drawing.Point(9, 6);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(36, 17);
            this.radioButton8.TabIndex = 0;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "All";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel6.Controls.Add(this.hisNewButton);
            this.panel6.Controls.Add(this.jobSaveButton);
            this.panel6.Controls.Add(this.button25);
            this.panel6.Location = new System.Drawing.Point(19, 489);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(371, 66);
            this.panel6.TabIndex = 633;
            // 
            // hisNewButton
            // 
            this.hisNewButton.Location = new System.Drawing.Point(129, 6);
            this.hisNewButton.Name = "hisNewButton";
            this.hisNewButton.Size = new System.Drawing.Size(107, 50);
            this.hisNewButton.TabIndex = 135;
            this.hisNewButton.Text = "Edit Job Details";
            this.hisNewButton.UseVisualStyleBackColor = true;
            // 
            // jobSaveButton
            // 
            this.jobSaveButton.Enabled = false;
            this.jobSaveButton.Location = new System.Drawing.Point(8, 6);
            this.jobSaveButton.Name = "jobSaveButton";
            this.jobSaveButton.Size = new System.Drawing.Size(107, 50);
            this.jobSaveButton.TabIndex = 132;
            this.jobSaveButton.Text = "Save Job Details";
            this.jobSaveButton.UseVisualStyleBackColor = true;
            this.jobSaveButton.Click += new System.EventHandler(this.jobSaveButton_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(251, 6);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(107, 50);
            this.button25.TabIndex = 130;
            this.button25.Text = "Exit Form";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(426, 479);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 634;
            this.label6.Text = "Job Listing by";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 636;
            this.label1.Text = "Grade";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 638;
            this.label2.Text = "Grade Point";
            // 
            // dGrade
            // 
            this.dGrade.Location = new System.Drawing.Point(88, 101);
            this.dGrade.Mask = "00";
            this.dGrade.Name = "dGrade";
            this.dGrade.Size = new System.Drawing.Size(55, 20);
            this.dGrade.TabIndex = 639;
            this.dGrade.Validated += new System.EventHandler(this.dGrade_Validated);
            // 
            // dGpoint
            // 
            this.dGpoint.Location = new System.Drawing.Point(298, 101);
            this.dGpoint.Mask = "00";
            this.dGpoint.Name = "dGpoint";
            this.dGpoint.Size = new System.Drawing.Size(70, 20);
            this.dGpoint.TabIndex = 640;
            this.dGpoint.Validated += new System.EventHandler(this.dGpoint_Validated);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Job";
            this.Column1.Name = "Column1";
            this.Column1.Width = 180;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Job Function";
            this.Column6.Name = "Column6";
            this.Column6.Width = 180;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Department";
            this.Column2.Name = "Column2";
            this.Column2.Width = 180;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Reports To";
            this.Column7.Name = "Column7";
            this.Column7.Width = 180;
            // 
            // JobDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(774, 587);
            this.Controls.Add(this.dGpoint);
            this.Controls.Add(this.dGrade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.comboBox7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.jobGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JobDefinition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JobDefinition";
            this.Load += new System.EventHandler(this.JobDefinition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jobGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView jobGrid;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton radioButton14;
        private System.Windows.Forms.ComboBox comboBox17;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button hisNewButton;
        private System.Windows.Forms.Button jobSaveButton;
        private System.Windows.Forms.Button button25;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox dGrade;
        private System.Windows.Forms.MaskedTextBox dGpoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}