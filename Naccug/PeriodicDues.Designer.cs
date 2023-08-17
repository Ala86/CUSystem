namespace WinTcare
{
    partial class PeriodicDues
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PeriodicDues));
            this.duesGrid = new System.Windows.Forms.DataGridView();
            this.selPrd = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dmainCat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcMembName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.intRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appldate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button21 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dProcessDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label113 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ptype = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.duesGrid)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // duesGrid
            // 
            this.duesGrid.AllowUserToAddRows = false;
            this.duesGrid.AllowUserToDeleteRows = false;
            this.duesGrid.AllowUserToResizeColumns = false;
            this.duesGrid.AllowUserToResizeRows = false;
            this.duesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.duesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.duesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selPrd,
            this.dmainCat,
            this.tcMembName,
            this.intRate,
            this.appldate});
            this.duesGrid.Location = new System.Drawing.Point(21, 11);
            this.duesGrid.Margin = new System.Windows.Forms.Padding(4);
            this.duesGrid.Name = "duesGrid";
            this.duesGrid.RowHeadersVisible = false;
            this.duesGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.duesGrid.Size = new System.Drawing.Size(1137, 443);
            this.duesGrid.TabIndex = 393;
            this.duesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.duesGrid_CellContentClick);
            this.duesGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.duesGrid_CellValueChanged);
            this.duesGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.duesGrid_CurrentCellDirtyStateChanged);
            // 
            // selPrd
            // 
            this.selPrd.HeaderText = "Select";
            this.selPrd.Name = "selPrd";
            this.selPrd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.selPrd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.selPrd.Width = 50;
            // 
            // dmainCat
            // 
            this.dmainCat.HeaderText = "Subscription Description";
            this.dmainCat.Name = "dmainCat";
            this.dmainCat.ReadOnly = true;
            this.dmainCat.Width = 350;
            // 
            // tcMembName
            // 
            this.tcMembName.HeaderText = "Payment Frequency";
            this.tcMembName.Name = "tcMembName";
            this.tcMembName.ReadOnly = true;
            this.tcMembName.Width = 150;
            // 
            // intRate
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.NullValue = null;
            this.intRate.DefaultCellStyle = dataGridViewCellStyle1;
            this.intRate.HeaderText = "Processing Frequency";
            this.intRate.Name = "intRate";
            this.intRate.ReadOnly = true;
            this.intRate.Width = 150;
            // 
            // appldate
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.appldate.DefaultCellStyle = dataGridViewCellStyle2;
            this.appldate.HeaderText = "Amount";
            this.appldate.Name = "appldate";
            this.appldate.ReadOnly = true;
            this.appldate.Width = 140;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.button21);
            this.panel3.Controls.Add(this.button12);
            this.panel3.Controls.Add(this.saveButton);
            this.panel3.Location = new System.Drawing.Point(21, 530);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(495, 74);
            this.panel3.TabIndex = 390;
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(176, 9);
            this.button21.Margin = new System.Windows.Forms.Padding(4);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(144, 54);
            this.button21.TabIndex = 47;
            this.button21.Text = "Subscription Report";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(336, 9);
            this.button12.Margin = new System.Windows.Forms.Padding(4);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(144, 54);
            this.button12.TabIndex = 41;
            this.button12.Text = "Exit Form";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(13, 9);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(144, 54);
            this.saveButton.TabIndex = 30;
            this.saveButton.Text = "Run Process";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dProcessDate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(11, 459);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(375, 65);
            this.groupBox1.TabIndex = 385;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Processing";
            // 
            // dProcessDate
            // 
            this.dProcessDate.Location = new System.Drawing.Point(85, 20);
            this.dProcessDate.Margin = new System.Windows.Forms.Padding(4);
            this.dProcessDate.Name = "dProcessDate";
            this.dProcessDate.Size = new System.Drawing.Size(265, 22);
            this.dProcessDate.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Run Date";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.ForeColor = System.Drawing.Color.White;
            this.label113.Location = new System.Drawing.Point(4, 9);
            this.label113.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(387, 17);
            this.label113.TabIndex = 154;
            this.label113.Text = "Copyright NACCUG & T-care Systems Ltd, all rights Reserved";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.progressBar1);
            this.groupBox5.Location = new System.Drawing.Point(425, 460);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(733, 64);
            this.groupBox5.TabIndex = 388;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Progress Counter";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(11, 22);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(712, 28);
            this.progressBar1.TabIndex = 343;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Navy;
            this.panel15.Controls.Add(this.label113);
            this.panel15.Location = new System.Drawing.Point(777, 575);
            this.panel15.Margin = new System.Windows.Forms.Padding(4);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(396, 32);
            this.panel15.TabIndex = 389;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(825, 542);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Process Type";
            // 
            // ptype
            // 
            this.ptype.AutoSize = true;
            this.ptype.BackColor = System.Drawing.Color.Red;
            this.ptype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ptype.ForeColor = System.Drawing.Color.White;
            this.ptype.Location = new System.Drawing.Point(929, 539);
            this.ptype.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ptype.Name = "ptype";
            this.ptype.Size = new System.Drawing.Size(243, 20);
            this.ptype.TabIndex = 394;
            this.ptype.Text = "                                       ";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(561, 530);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(153, 21);
            this.checkBox2.TabIndex = 554;
            this.checkBox2.Text = "Check to send SMS";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // PeriodicDues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 610);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.ptype);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.duesGrid);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.panel15);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PeriodicDues";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PeriodicDues";
            this.Load += new System.EventHandler(this.PeriodicDues_Load);
            ((System.ComponentModel.ISupportInitialize)(this.duesGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView duesGrid;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dProcessDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selPrd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dmainCat;
        private System.Windows.Forms.DataGridViewTextBoxColumn tcMembName;
        private System.Windows.Forms.DataGridViewTextBoxColumn intRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn appldate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ptype;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}