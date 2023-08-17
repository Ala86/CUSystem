namespace WinTcare
{
    partial class updPayID
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(updPayID));
            this.panel15 = new System.Windows.Forms.Panel();
            this.label113 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cusGrid = new System.Windows.Forms.DataGridView();
            this.dcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dfname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dlname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dmempay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel15.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cusGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Navy;
            this.panel15.Controls.Add(this.label113);
            this.panel15.Location = new System.Drawing.Point(357, 205);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(297, 26);
            this.panel15.TabIndex = 339;
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
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.button6);
            this.panel4.Controls.Add(this.button4);
            this.panel4.Controls.Add(this.saveButton);
            this.panel4.Location = new System.Drawing.Point(2, 159);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(202, 68);
            this.panel4.TabIndex = 340;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Gainsboro;
            this.button2.Location = new System.Drawing.Point(108, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 50);
            this.button2.TabIndex = 255;
            this.button2.Text = "Exit Form";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Gainsboro;
            this.button6.Location = new System.Drawing.Point(505, 560);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(84, 50);
            this.button6.TabIndex = 260;
            this.button6.Text = "Exit Form";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Gainsboro;
            this.button4.Location = new System.Drawing.Point(405, 560);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(84, 50);
            this.button4.TabIndex = 258;
            this.button4.Text = "Exit Form";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(12, 9);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(78, 50);
            this.saveButton.TabIndex = 212;
            this.saveButton.Text = "Update Payroll ID";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cusGrid
            // 
            this.cusGrid.AllowUserToAddRows = false;
            this.cusGrid.AllowUserToDeleteRows = false;
            this.cusGrid.AllowUserToResizeColumns = false;
            this.cusGrid.AllowUserToResizeRows = false;
            this.cusGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.cusGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cusGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dcode,
            this.dfname,
            this.dlname,
            this.dmempay});
            this.cusGrid.Location = new System.Drawing.Point(0, -3);
            this.cusGrid.Name = "cusGrid";
            this.cusGrid.RowHeadersVisible = false;
            this.cusGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.cusGrid.Size = new System.Drawing.Size(637, 139);
            this.cusGrid.TabIndex = 341;
            this.cusGrid.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.cusGrid_CellValidated);
            this.cusGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.cusGrid_CurrentCellDirtyStateChanged);
            // 
            // dcode
            // 
            this.dcode.HeaderText = "Member Code";
            this.dcode.Name = "dcode";
            this.dcode.ReadOnly = true;
            // 
            // dfname
            // 
            this.dfname.HeaderText = "First Name";
            this.dfname.Name = "dfname";
            this.dfname.ReadOnly = true;
            this.dfname.Width = 200;
            // 
            // dlname
            // 
            this.dlname.HeaderText = "Last Name";
            this.dlname.Name = "dlname";
            this.dlname.ReadOnly = true;
            this.dlname.Width = 200;
            // 
            // dmempay
            // 
            this.dmempay.HeaderText = "Member Payroll ID";
            this.dmempay.Name = "dmempay";
            this.dmempay.Width = 125;
            // 
            // updPayID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(649, 231);
            this.Controls.Add(this.cusGrid);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel15);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "updPayID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "updPayID";
            this.Load += new System.EventHandler(this.updPayID_Load);
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cusGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.DataGridView cusGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dfname;
        private System.Windows.Forms.DataGridViewTextBoxColumn dlname;
        private System.Windows.Forms.DataGridViewTextBoxColumn dmempay;
    }
}