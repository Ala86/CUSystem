namespace WinTcare
{
    partial class radtemplate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(radtemplate));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tempCont = new System.Windows.Forms.RichTextBox();
            this.tempGrid = new System.Windows.Forms.DataGridView();
            this.tempName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tempCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tempid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button15 = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.newButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tempGrid)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(359, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Template Name";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(103, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(359, 21);
            this.comboBox1.TabIndex = 21;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.comboBox1_PreviewKeyDown);
            // 
            // tempCont
            // 
            this.tempCont.Location = new System.Drawing.Point(103, 44);
            this.tempCont.Name = "tempCont";
            this.tempCont.Size = new System.Drawing.Size(359, 185);
            this.tempCont.TabIndex = 2;
            this.tempCont.Text = "";
            this.tempCont.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tempCont_PreviewKeyDown);
            // 
            // tempGrid
            // 
            this.tempGrid.BackgroundColor = System.Drawing.Color.White;
            this.tempGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tempGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tempName,
            this.tempCode,
            this.tempid});
            this.tempGrid.Location = new System.Drawing.Point(103, 250);
            this.tempGrid.Name = "tempGrid";
            this.tempGrid.RowHeadersVisible = false;
            this.tempGrid.Size = new System.Drawing.Size(359, 282);
            this.tempGrid.TabIndex = 5;
            // 
            // tempName
            // 
            this.tempName.HeaderText = "Template Title";
            this.tempName.Name = "tempName";
            this.tempName.Width = 250;
            // 
            // tempCode
            // 
            this.tempCode.HeaderText = "Template code";
            this.tempCode.Name = "tempCode";
            // 
            // tempid
            // 
            this.tempid.HeaderText = "Template ID";
            this.tempid.MinimumWidth = 2;
            this.tempid.Name = "tempid";
            this.tempid.Width = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Template Content";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 232);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Defined Templates";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.newButton);
            this.panel3.Controls.Add(this.button15);
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.button7);
            this.panel3.Location = new System.Drawing.Point(101, 541);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(361, 54);
            this.panel3.TabIndex = 296;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(201, 7);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(69, 40);
            this.button15.TabIndex = 131;
            this.button15.Text = "Edit";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(106, 7);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(79, 40);
            this.SaveButton.TabIndex = 30;
            this.SaveButton.Text = "Save Details";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(287, 8);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(65, 40);
            this.button7.TabIndex = 130;
            this.button7.Text = "Exit Form";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Navy;
            this.panel1.Controls.Add(this.label22);
            this.panel1.Location = new System.Drawing.Point(274, 621);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(252, 26);
            this.panel1.TabIndex = 297;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(5, 7);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(239, 13);
            this.label22.TabIndex = 154;
            this.label22.Text = "Copyright T-care Systems Ltd, All rights Reserved";
            // 
            // newButton
            // 
            this.newButton.Location = new System.Drawing.Point(15, 7);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(79, 40);
            this.newButton.TabIndex = 132;
            this.newButton.Text = "New Template";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // radtemplate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 677);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tempGrid);
            this.Controls.Add(this.tempCont);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "radtemplate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "radtemplate";
            this.Load += new System.EventHandler(this.radtemplate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tempGrid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RichTextBox tempCont;
        private System.Windows.Forms.DataGridView tempGrid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.DataGridViewTextBoxColumn tempName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tempCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn tempid;
        private System.Windows.Forms.Button newButton;
    }
}