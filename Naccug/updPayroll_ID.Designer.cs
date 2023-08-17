namespace WinTcare
{
    partial class updPayroll_IDcs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(updPayroll_IDcs));
            this.memGrid = new System.Windows.Forms.DataGridView();
            this.panel15 = new System.Windows.Forms.Panel();
            this.label113 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.actnumb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acttype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.memGrid)).BeginInit();
            this.panel15.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // memGrid
            // 
            this.memGrid.AllowUserToAddRows = false;
            this.memGrid.AllowUserToDeleteRows = false;
            this.memGrid.AllowUserToResizeColumns = false;
            this.memGrid.AllowUserToResizeRows = false;
            this.memGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.memGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.memGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.actnumb,
            this.actname,
            this.acttype});
            this.memGrid.Location = new System.Drawing.Point(12, 12);
            this.memGrid.Name = "memGrid";
            this.memGrid.ReadOnly = true;
            this.memGrid.RowHeadersVisible = false;
            this.memGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.memGrid.Size = new System.Drawing.Size(565, 139);
            this.memGrid.TabIndex = 340;
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.Navy;
            this.panel15.Controls.Add(this.label113);
            this.panel15.Location = new System.Drawing.Point(293, 216);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(297, 26);
            this.panel15.TabIndex = 341;
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
            this.panel4.Location = new System.Drawing.Point(12, 171);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(222, 68);
            this.panel4.TabIndex = 342;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Gainsboro;
            this.button2.Location = new System.Drawing.Point(122, 9);
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
            // 
            // actnumb
            // 
            this.actnumb.HeaderText = "Member Name";
            this.actnumb.Name = "actnumb";
            this.actnumb.ReadOnly = true;
            this.actnumb.Width = 300;
            // 
            // actname
            // 
            this.actname.HeaderText = "Member Code";
            this.actname.Name = "actname";
            this.actname.ReadOnly = true;
            this.actname.Width = 125;
            // 
            // acttype
            // 
            this.acttype.HeaderText = "Payroll ID";
            this.acttype.Name = "acttype";
            this.acttype.ReadOnly = true;
            this.acttype.Width = 125;
            // 
            // updPayroll_IDcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(587, 243);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel15);
            this.Controls.Add(this.memGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "updPayroll_IDcs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "updPayroll_IDcs";
            ((System.ComponentModel.ISupportInitialize)(this.memGrid)).EndInit();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView memGrid;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn actnumb;
        private System.Windows.Forms.DataGridViewTextBoxColumn actname;
        private System.Windows.Forms.DataGridViewTextBoxColumn acttype;
    }
}