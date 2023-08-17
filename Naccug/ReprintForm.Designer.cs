
namespace WinTcare
{
    partial class ReprintForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReprintForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cashviewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.copyright = new System.Windows.Forms.Label();
            this.dateFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.TransGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.dreceipt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dreceipt1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tkns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WinTcare.Properties.Resources.CULogo;
            this.pictureBox1.Location = new System.Drawing.Point(124, 441);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 663;
            this.pictureBox1.TabStop = false;
            // 
            // cashviewer
            // 
            this.cashviewer.ActiveViewIndex = -1;
            this.cashviewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cashviewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.cashviewer.DisplayStatusBar = false;
            this.cashviewer.Location = new System.Drawing.Point(437, 12);
            this.cashviewer.Name = "cashviewer";
            this.cashviewer.Size = new System.Drawing.Size(543, 521);
            this.cashviewer.TabIndex = 662;
            this.cashviewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, -100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 654;
            this.label6.Text = "From";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 653;
            this.label5.Text = "Member ID";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel5.Controls.Add(this.SaveButton);
            this.panel5.Controls.Add(this.button7);
            this.panel5.Location = new System.Drawing.Point(437, 539);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(213, 54);
            this.panel5.TabIndex = 650;
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(8, 7);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(110, 40);
            this.SaveButton.TabIndex = 30;
            this.SaveButton.Text = "Print";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(131, 7);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 40);
            this.button7.TabIndex = 130;
            this.button7.Text = "Exit Form";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Navy;
            this.panel6.Controls.Add(this.copyright);
            this.panel6.Location = new System.Drawing.Point(25, 567);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(252, 26);
            this.panel6.TabIndex = 651;
            // 
            // copyright
            // 
            this.copyright.AutoSize = true;
            this.copyright.ForeColor = System.Drawing.Color.White;
            this.copyright.Location = new System.Drawing.Point(5, 7);
            this.copyright.Name = "copyright";
            this.copyright.Size = new System.Drawing.Size(0, 13);
            this.copyright.TabIndex = 154;
            // 
            // dateFrom
            // 
            this.dateFrom.Location = new System.Drawing.Point(93, -104);
            this.dateFrom.Name = "dateFrom";
            this.dateFrom.Size = new System.Drawing.Size(200, 20);
            this.dateFrom.TabIndex = 647;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-46, -101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 645;
            this.label2.Text = "Collection Date";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(252, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 664;
            this.button2.Text = "Find Member";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(251)))), ((int)(((byte)(142)))));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(76, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Validated += new System.EventHandler(this.textBox1_Validated);
            // 
            // TransGrid
            // 
            this.TransGrid.AllowUserToAddRows = false;
            this.TransGrid.AllowUserToDeleteRows = false;
            this.TransGrid.AllowUserToResizeColumns = false;
            this.TransGrid.AllowUserToResizeRows = false;
            this.TransGrid.BackgroundColor = System.Drawing.Color.White;
            this.TransGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TransGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dreceipt,
            this.rdate,
            this.dreceipt1,
            this.rSelect,
            this.tkns});
            this.TransGrid.Location = new System.Drawing.Point(18, 115);
            this.TransGrid.Name = "TransGrid";
            this.TransGrid.RowHeadersVisible = false;
            this.TransGrid.Size = new System.Drawing.Size(395, 320);
            this.TransGrid.TabIndex = 666;
            this.TransGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.TransGrid_CellBeginEdit);
            this.TransGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.TransGrid_CellEndEdit);
            this.TransGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.TransGrid_CellValueChanged);
            this.TransGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.TransGrid_CurrentCellDirtyStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 667;
            this.label1.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 668;
            this.label3.Text = "To";
            // 
            // fromDate
            // 
            this.fromDate.Location = new System.Drawing.Point(76, 57);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(251, 20);
            this.fromDate.TabIndex = 669;
            // 
            // toDate
            // 
            this.toDate.Location = new System.Drawing.Point(76, 84);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(251, 20);
            this.toDate.TabIndex = 670;
            // 
            // dreceipt
            // 
            this.dreceipt.HeaderText = "Receipt Number";
            this.dreceipt.Name = "dreceipt";
            this.dreceipt.ReadOnly = true;
            this.dreceipt.Width = 105;
            // 
            // rdate
            // 
            this.rdate.HeaderText = "Receipt Date";
            this.rdate.Name = "rdate";
            this.rdate.ReadOnly = true;
            this.rdate.Width = 110;
            // 
            // dreceipt1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.dreceipt1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dreceipt1.HeaderText = "Receipt Amount";
            this.dreceipt1.Name = "dreceipt1";
            this.dreceipt1.ReadOnly = true;
            this.dreceipt1.Width = 110;
            // 
            // rSelect
            // 
            this.rSelect.HeaderText = "Select";
            this.rSelect.Name = "rSelect";
            this.rSelect.Width = 50;
            // 
            // tkns
            // 
            this.tkns.HeaderText = "Token";
            this.tkns.MinimumWidth = 2;
            this.tkns.Name = "tkns";
            this.tkns.Width = 2;
            // 
            // ReprintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(985, 600);
            this.Controls.Add(this.toDate);
            this.Controls.Add(this.fromDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TransGrid);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.cashviewer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.dateFrom);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReprintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReprintForm";
            this.Load += new System.EventHandler(this.ReprintForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer cashviewer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label copyright;
        private System.Windows.Forms.DateTimePicker dateFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView TransGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dreceipt;
        private System.Windows.Forms.DataGridViewTextBoxColumn rdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dreceipt1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn rSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn tkns;
    }
}