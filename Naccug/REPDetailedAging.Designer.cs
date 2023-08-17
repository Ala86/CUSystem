namespace WinTcare
{
    partial class REPDetailedAging
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
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.CMBTRANSACTION_TYPE = new System.Windows.Forms.ComboBox();
            this.ComboBox2 = new System.Windows.Forms.ComboBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.TextBox7 = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.TextBox8 = new System.Windows.Forms.TextBox();
            this.TextBox5 = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.TextBox6 = new System.Windows.Forms.TextBox();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.TextBox4 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayStatusBar = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(693, 460);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ReportRefresh += new CrystalDecisions.Windows.Forms.RefreshEventHandler(this.crystalReportViewer1_ReportRefresh);
            this.crystalReportViewer1.Load += new System.EventHandler(this.crystalReportViewer1_Load);
            // 
            // CMBTRANSACTION_TYPE
            // 
            this.CMBTRANSACTION_TYPE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMBTRANSACTION_TYPE.FormattingEnabled = true;
            this.CMBTRANSACTION_TYPE.Items.AddRange(new object[] {
            "Any Currency",
            "Dalasis"});
            this.CMBTRANSACTION_TYPE.Location = new System.Drawing.Point(145, 73);
            this.CMBTRANSACTION_TYPE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CMBTRANSACTION_TYPE.Name = "CMBTRANSACTION_TYPE";
            this.CMBTRANSACTION_TYPE.Size = new System.Drawing.Size(216, 24);
            this.CMBTRANSACTION_TYPE.TabIndex = 117;
            // 
            // ComboBox2
            // 
            this.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox2.FormattingEnabled = true;
            this.ComboBox2.Items.AddRange(new object[] {
            "Any Branch ",
            "Heah Office"});
            this.ComboBox2.Location = new System.Drawing.Point(443, 76);
            this.ComboBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ComboBox2.Name = "ComboBox2";
            this.ComboBox2.Size = new System.Drawing.Size(232, 24);
            this.ComboBox2.TabIndex = 120;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(51, 73);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(69, 17);
            this.Label9.TabIndex = 119;
            this.Label9.Text = "Currency:";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(376, 76);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(57, 17);
            this.Label10.TabIndex = 118;
            this.Label10.Text = "Branch:";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(449, 394);
            this.Button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(220, 39);
            this.Button1.TabIndex = 116;
            this.Button1.Text = "Print";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // TextBox7
            // 
            this.TextBox7.Location = new System.Drawing.Point(443, 294);
            this.TextBox7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox7.Name = "TextBox7";
            this.TextBox7.Size = new System.Drawing.Size(235, 22);
            this.TextBox7.TabIndex = 115;
            this.TextBox7.Text = "999999";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(377, 294);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(25, 17);
            this.Label8.TabIndex = 114;
            this.Label8.Text = "To";
            // 
            // TextBox8
            // 
            this.TextBox8.Location = new System.Drawing.Point(145, 294);
            this.TextBox8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox8.Name = "TextBox8";
            this.TextBox8.Size = new System.Drawing.Size(209, 22);
            this.TextBox8.TabIndex = 113;
            this.TextBox8.Text = "366";
            // 
            // TextBox5
            // 
            this.TextBox5.Location = new System.Drawing.Point(443, 242);
            this.TextBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox5.Name = "TextBox5";
            this.TextBox5.Size = new System.Drawing.Size(233, 22);
            this.TextBox5.TabIndex = 112;
            this.TextBox5.Text = "365";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(377, 242);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(25, 17);
            this.Label7.TabIndex = 111;
            this.Label7.Text = "To";
            // 
            // TextBox6
            // 
            this.TextBox6.Location = new System.Drawing.Point(145, 242);
            this.TextBox6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox6.Name = "TextBox6";
            this.TextBox6.Size = new System.Drawing.Size(212, 22);
            this.TextBox6.TabIndex = 110;
            this.TextBox6.Text = "181";
            // 
            // TextBox3
            // 
            this.TextBox3.Location = new System.Drawing.Point(445, 194);
            this.TextBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(231, 22);
            this.TextBox3.TabIndex = 109;
            this.TextBox3.Text = "180";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(377, 194);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(25, 17);
            this.Label6.TabIndex = 108;
            this.Label6.Text = "To";
            // 
            // TextBox4
            // 
            this.TextBox4.Location = new System.Drawing.Point(145, 194);
            this.TextBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox4.Name = "TextBox4";
            this.TextBox4.Size = new System.Drawing.Size(215, 22);
            this.TextBox4.TabIndex = 107;
            this.TextBox4.Text = "91";
            // 
            // TextBox2
            // 
            this.TextBox2.Location = new System.Drawing.Point(445, 148);
            this.TextBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(231, 22);
            this.TextBox2.TabIndex = 106;
            this.TextBox2.Text = "90";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(377, 148);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(25, 17);
            this.Label5.TabIndex = 105;
            this.Label5.Text = "To";
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(145, 148);
            this.TextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(215, 22);
            this.TextBox1.TabIndex = 104;
            this.TextBox1.Text = "31";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(51, 294);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(40, 17);
            this.Label4.TabIndex = 103;
            this.Label4.Text = "From";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(51, 242);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(40, 17);
            this.Label3.TabIndex = 102;
            this.Label3.Text = "From";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(51, 194);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(40, 17);
            this.Label2.TabIndex = 101;
            this.Label2.Text = "From";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(51, 148);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(40, 17);
            this.Label1.TabIndex = 100;
            this.Label1.Text = "From";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(443, 112);
            this.textBox9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(231, 22);
            this.textBox9.TabIndex = 124;
            this.textBox9.Text = "30";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(373, 112);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 17);
            this.label11.TabIndex = 123;
            this.label11.Text = "To";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(141, 112);
            this.textBox10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(215, 22);
            this.textBox10.TabIndex = 122;
            this.textBox10.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(47, 112);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 17);
            this.label12.TabIndex = 121;
            this.label12.Text = "From";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(357, 342);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 17);
            this.label21.TabIndex = 170;
            this.label21.Text = "Run Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(433, 341);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(1);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(239, 22);
            this.dateTimePicker1.TabIndex = 169;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Any Currency",
            "Dalasis"});
            this.comboBox1.Location = new System.Drawing.Point(133, 337);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(216, 24);
            this.comboBox1.TabIndex = 171;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 334);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 17);
            this.label13.TabIndex = 172;
            this.label13.Text = "Select Product";
            // 
            // REPDetailedAging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 460);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.CMBTRANSACTION_TYPE);
            this.Controls.Add(this.ComboBox2);
            this.Controls.Add(this.Label9);
            this.Controls.Add(this.Label10);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.TextBox7);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.TextBox8);
            this.Controls.Add(this.TextBox5);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.TextBox6);
            this.Controls.Add(this.TextBox3);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.TextBox4);
            this.Controls.Add(this.TextBox2);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.crystalReportViewer1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "REPDetailedAging";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "REPDetailedAging";
            this.Load += new System.EventHandler(this.REPDetailedAging_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        internal System.Windows.Forms.ComboBox CMBTRANSACTION_TYPE;
        internal System.Windows.Forms.ComboBox ComboBox2;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.TextBox TextBox7;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox TextBox8;
        internal System.Windows.Forms.TextBox TextBox5;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.TextBox TextBox6;
        internal System.Windows.Forms.TextBox TextBox3;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox TextBox4;
        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox textBox9;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.TextBox textBox10;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label21;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        internal System.Windows.Forms.ComboBox comboBox1;
        internal System.Windows.Forms.Label label13;
    }
}