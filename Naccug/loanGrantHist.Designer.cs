namespace WinTcare
{
    partial class LoanGrantHist
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoanGrantHist));
            this.panel2 = new System.Windows.Forms.Panel();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.clientgrid = new System.Windows.Forms.DataGridView();
            this.dcuscode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loanAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appldate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loanid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guarcode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button29 = new System.Windows.Forms.Button();
            this.guarGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dloanamt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dguaramt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label113 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientgrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guarGrid)).BeginInit();
            this.panel15.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.button10);
            this.panel2.Controls.Add(this.button9);
            this.panel2.Controls.Add(this.SaveButton);
            this.panel2.Location = new System.Drawing.Point(12, 579);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(425, 81);
            this.panel2.TabIndex = 377;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(303, 7);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(112, 71);
            this.button10.TabIndex = 390;
            this.button10.Text = "Exit Form";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Enabled = false;
            this.button9.Location = new System.Drawing.Point(153, 7);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(136, 71);
            this.button9.TabIndex = 370;
            this.button9.Text = "Generate Reports";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(4, 7);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(136, 71);
            this.SaveButton.TabIndex = 300;
            this.SaveButton.Text = "Confirm Selection";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // clientgrid
            // 
            this.clientgrid.AllowUserToAddRows = false;
            this.clientgrid.AllowUserToDeleteRows = false;
            this.clientgrid.AllowUserToResizeColumns = false;
            this.clientgrid.AllowUserToResizeRows = false;
            this.clientgrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.clientgrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientgrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcuscode,
            this.dataGridViewTextBoxColumn7,
            this.loanAmt,
            this.appldate,
            this.loanid});
            this.clientgrid.Location = new System.Drawing.Point(12, 102);
            this.clientgrid.Name = "clientgrid";
            this.clientgrid.ReadOnly = true;
            this.clientgrid.RowHeadersVisible = false;
            this.clientgrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.clientgrid.Size = new System.Drawing.Size(747, 279);
            this.clientgrid.TabIndex = 376;
            this.clientgrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.clientgrid_CellClick);
            this.clientgrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.clientgrid_CurrentCellDirtyStateChanged);
            // 
            // dcuscode
            // 
            this.dcuscode.HeaderText = "Member Code";
            this.dcuscode.Name = "dcuscode";
            this.dcuscode.ReadOnly = true;
            this.dcuscode.Width = 70;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Member Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 380;
            // 
            // loanAmt
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.loanAmt.DefaultCellStyle = dataGridViewCellStyle1;
            this.loanAmt.HeaderText = "Loan Amount";
            this.loanAmt.Name = "loanAmt";
            this.loanAmt.ReadOnly = true;
            this.loanAmt.Width = 150;
            // 
            // appldate
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.appldate.DefaultCellStyle = dataGridViewCellStyle2;
            this.appldate.HeaderText = "Application Date";
            this.appldate.Name = "appldate";
            this.appldate.ReadOnly = true;
            this.appldate.Width = 140;
            // 
            // loanid
            // 
            this.loanid.HeaderText = "Loan ID";
            this.loanid.MinimumWidth = 2;
            this.loanid.Name = "loanid";
            this.loanid.ReadOnly = true;
            this.loanid.Width = 2;
            // 
            // guarcode
            // 
            this.guarcode.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guarcode.Location = new System.Drawing.Point(531, 19);
            this.guarcode.Margin = new System.Windows.Forms.Padding(2);
            this.guarcode.Name = "guarcode";
            this.guarcode.Size = new System.Drawing.Size(80, 21);
            this.guarcode.TabIndex = 385;
            this.guarcode.Validated += new System.EventHandler(this.guarcode_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(412, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 379;
            this.label4.Text = "Guarantor ID";
            // 
            // button29
            // 
            this.button29.Location = new System.Drawing.Point(616, 19);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(79, 23);
            this.button29.TabIndex = 391;
            this.button29.Text = "Find Member";
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // guarGrid
            // 
            this.guarGrid.AllowUserToAddRows = false;
            this.guarGrid.AllowUserToDeleteRows = false;
            this.guarGrid.AllowUserToResizeColumns = false;
            this.guarGrid.AllowUserToResizeRows = false;
            this.guarGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.guarGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.guarGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dloanamt,
            this.dguaramt,
            this.dataGridViewTextBoxColumn4});
            this.guarGrid.Location = new System.Drawing.Point(12, 428);
            this.guarGrid.Name = "guarGrid";
            this.guarGrid.ReadOnly = true;
            this.guarGrid.RowHeadersVisible = false;
            this.guarGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.guarGrid.Size = new System.Drawing.Size(747, 143);
            this.guarGrid.TabIndex = 392;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Guarantor Code";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Guarantor  Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 250;
            // 
            // dloanamt
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.dloanamt.DefaultCellStyle = dataGridViewCellStyle3;
            this.dloanamt.HeaderText = "Loan Amount";
            this.dloanamt.Name = "dloanamt";
            this.dloanamt.ReadOnly = true;
            this.dloanamt.Width = 120;
            // 
            // dguaramt
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.dguaramt.DefaultCellStyle = dataGridViewCellStyle4;
            this.dguaramt.HeaderText = "Amt. Guaranteed";
            this.dguaramt.Name = "dguaramt";
            this.dguaramt.ReadOnly = true;
            this.dguaramt.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Guar. Date";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 130;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Navy;
            this.panel15.Controls.Add(this.label113);
            this.panel15.Location = new System.Drawing.Point(476, 644);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(297, 26);
            this.panel15.TabIndex = 394;
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.ForeColor = System.Drawing.Color.White;
            this.label113.Location = new System.Drawing.Point(3, 7);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(289, 13);
            this.label113.TabIndex = 154;
            this.label113.Text = "Copyright NACCUG & T-care Systems Ltd, all rights Reserved";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Lime;
            this.textBox2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(614, 597);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(144, 21);
            this.textBox2.TabIndex = 398;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(508, 601);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 397;
            this.label3.Text = "Total Guaranteed";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Red;
            this.textBox3.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(614, 618);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(144, 21);
            this.textBox3.TabIndex = 400;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(508, 622);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 399;
            this.label5.Text = "Guarantee Required";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 404;
            this.label1.Text = "Process  by";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Location = new System.Drawing.Point(55, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(157, 25);
            this.panel1.TabIndex = 406;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(64, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(77, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Member ID";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(10, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(36, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "All";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // textBox10
            // 
            this.textBox10.Enabled = false;
            this.textBox10.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox10.Location = new System.Drawing.Point(279, 16);
            this.textBox10.Margin = new System.Windows.Forms.Padding(2);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(80, 21);
            this.textBox10.TabIndex = 408;
            this.textBox10.Validated += new System.EventHandler(this.textBox10_Validated);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(216, 19);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 407;
            this.label9.Text = "Member ID";
            // 
            // LoanGrantHist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(767, 666);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel15);
            this.Controls.Add(this.guarGrid);
            this.Controls.Add(this.button29);
            this.Controls.Add(this.guarcode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.clientgrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoanGrantHist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "loan Guarantee History";
            this.Load += new System.EventHandler(this.LoanGrantHist_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientgrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guarGrid)).EndInit();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.DataGridView clientgrid;
        internal System.Windows.Forms.TextBox guarcode;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button29;
        private System.Windows.Forms.DataGridView guarGrid;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label113;
        internal System.Windows.Forms.TextBox textBox2;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox textBox3;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dloanamt;
        private System.Windows.Forms.DataGridViewTextBoxColumn dguaramt;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcuscode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn loanAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn appldate;
        private System.Windows.Forms.DataGridViewTextBoxColumn loanid;
        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        internal System.Windows.Forms.TextBox textBox10;
        internal System.Windows.Forms.Label label9;
    }
}