namespace TclassLibrary
{
    partial class Requisition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Requisition));
            this.prodGrid = new System.Windows.Forms.DataGridView();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.iselect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avlAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purAmt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lpetty = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lasset = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.prdCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.prodGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // prodGrid
            // 
            this.prodGrid.AllowUserToAddRows = false;
            this.prodGrid.AllowUserToDeleteRows = false;
            this.prodGrid.AllowUserToResizeColumns = false;
            this.prodGrid.AllowUserToResizeRows = false;
            this.prodGrid.BackgroundColor = System.Drawing.Color.White;
            this.prodGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.prodGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iselect,
            this.Column2,
            this.avlAmt,
            this.reqAmt,
            this.purAmt,
            this.lpetty,
            this.lasset,
            this.prdCode});
            this.prodGrid.Location = new System.Drawing.Point(12, 172);
            this.prodGrid.Name = "prodGrid";
            this.prodGrid.RowHeadersVisible = false;
            this.prodGrid.Size = new System.Drawing.Size(872, 341);
            this.prodGrid.TabIndex = 0;
            this.prodGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.prodGrid_CellValueChanged);
            // 
            // textBox12
            // 
            this.textBox12.Enabled = false;
            this.textBox12.Location = new System.Drawing.Point(108, 107);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(280, 20);
            this.textBox12.TabIndex = 332;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 110);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.label11.TabIndex = 331;
            this.label11.Text = "Position";
            // 
            // textBox11
            // 
            this.textBox11.Enabled = false;
            this.textBox11.Location = new System.Drawing.Point(108, 84);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(280, 20);
            this.textBox11.TabIndex = 330;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 13);
            this.label9.TabIndex = 329;
            this.label9.Text = "Person Requesting";
            // 
            // textBox10
            // 
            this.textBox10.Enabled = false;
            this.textBox10.Location = new System.Drawing.Point(108, 60);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(280, 20);
            this.textBox10.TabIndex = 328;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 327;
            this.label8.Text = "Department";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 42);
            this.groupBox1.TabIndex = 333;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Requisition Type";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(127, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(66, 17);
            this.radioButton2.TabIndex = 284;
            this.radioButton2.Text = "Services";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(14, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(73, 17);
            this.radioButton1.TabIndex = 283;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Produccts";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.label22);
            this.panel2.Location = new System.Drawing.Point(478, 556);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 26);
            this.panel2.TabIndex = 338;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(5, 7);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(238, 13);
            this.label22.TabIndex = 154;
            this.label22.Text = "Copyright T-care Systems Ltd, all rights Reserved";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.button7);
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Location = new System.Drawing.Point(3, 519);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(251, 59);
            this.panel3.TabIndex = 337;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(150, 10);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(94, 38);
            this.button7.TabIndex = 41;
            this.button7.Text = "Exit Form";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(10, 10);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(131, 38);
            this.SaveButton.TabIndex = 30;
            this.SaveButton.Text = "Confirm Requisition";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(348, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(129, 20);
            this.textBox1.TabIndex = 340;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 339;
            this.label1.Text = "Item Req. No.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Location = new System.Drawing.Point(591, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(122, 80);
            this.groupBox2.TabIndex = 334;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Requisition Scope";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(14, 57);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(56, 17);
            this.radioButton3.TabIndex = 284;
            this.radioButton3.Text = "Assets";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(14, 19);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(53, 17);
            this.radioButton4.TabIndex = 283;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Stock";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(108, 146);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(489, 20);
            this.textBox2.TabIndex = 342;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 341;
            this.label2.Text = "Stock Item Search";
            // 
            // iselect
            // 
            this.iselect.HeaderText = "Select";
            this.iselect.Name = "iselect";
            this.iselect.ReadOnly = true;
            this.iselect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.iselect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.iselect.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Item Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 220;
            // 
            // avlAmt
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.avlAmt.DefaultCellStyle = dataGridViewCellStyle1;
            this.avlAmt.HeaderText = "Quantity Available";
            this.avlAmt.Name = "avlAmt";
            this.avlAmt.ReadOnly = true;
            this.avlAmt.Width = 120;
            // 
            // reqAmt
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.reqAmt.DefaultCellStyle = dataGridViewCellStyle2;
            this.reqAmt.HeaderText = "Required";
            this.reqAmt.Name = "reqAmt";
            this.reqAmt.Width = 120;
            // 
            // purAmt
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.purAmt.DefaultCellStyle = dataGridViewCellStyle3;
            this.purAmt.HeaderText = "Purchase";
            this.purAmt.Name = "purAmt";
            this.purAmt.ReadOnly = true;
            this.purAmt.Width = 120;
            // 
            // lpetty
            // 
            this.lpetty.HeaderText = "Petty Cash";
            this.lpetty.Name = "lpetty";
            this.lpetty.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lpetty.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lasset
            // 
            this.lasset.HeaderText = "Asset";
            this.lasset.Name = "lasset";
            this.lasset.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.lasset.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // prdCode
            // 
            this.prdCode.HeaderText = "Product Code";
            this.prdCode.MinimumWidth = 2;
            this.prdCode.Name = "prdCode";
            this.prdCode.Width = 2;
            // 
            // Requisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(891, 579);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.prodGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Requisition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Requisition";
            this.Load += new System.EventHandler(this.Requisition_Load);
            ((System.ComponentModel.ISupportInitialize)(this.prodGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView prodGrid;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn iselect;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn avlAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqAmt;
        private System.Windows.Forms.DataGridViewTextBoxColumn purAmt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lpetty;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lasset;
        private System.Windows.Forms.DataGridViewTextBoxColumn prdCode;
    }
}