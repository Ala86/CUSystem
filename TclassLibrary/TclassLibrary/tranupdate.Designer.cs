namespace TclassLibrary
{
    partial class tranupdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tranupdate));
            this.transGrid = new System.Windows.Forms.DataGridView();
            this.dupt = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.duser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actnumb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.postdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trandate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ddebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dcredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtrans = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.djvno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dchq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cvou = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jstack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tempUpd = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label20 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.transDate = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBox2 = new System.Windows.Forms.MaskedTextBox();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.transGrid)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // transGrid
            // 
            this.transGrid.AllowUserToAddRows = false;
            this.transGrid.AllowUserToDeleteRows = false;
            this.transGrid.AllowUserToResizeColumns = false;
            this.transGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.transGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.transGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.transGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.transGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dupt,
            this.duser,
            this.actnumb,
            this.postdate,
            this.trandate,
            this.ddebit,
            this.dcredit,
            this.dtrans,
            this.djvno,
            this.dchq,
            this.cvou,
            this.jstack,
            this.tempUpd});
            this.transGrid.Location = new System.Drawing.Point(27, 124);
            this.transGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.transGrid.Name = "transGrid";
            this.transGrid.RowHeadersVisible = false;
            this.transGrid.Size = new System.Drawing.Size(1387, 537);
            this.transGrid.TabIndex = 0;
            this.transGrid.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.transGrid_CellValidated);
            this.transGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.transGrid_CellValueChanged);
            this.transGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.transGrid_CurrentCellDirtyStateChanged);
            // 
            // dupt
            // 
            this.dupt.HeaderText = "Upd";
            this.dupt.Name = "dupt";
            this.dupt.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dupt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dupt.Width = 50;
            // 
            // duser
            // 
            this.duser.HeaderText = "User ID";
            this.duser.Name = "duser";
            this.duser.ReadOnly = true;
            this.duser.Width = 60;
            // 
            // actnumb
            // 
            this.actnumb.HeaderText = "Posting A/C";
            this.actnumb.Name = "actnumb";
            this.actnumb.ReadOnly = true;
            // 
            // postdate
            // 
            this.postdate.HeaderText = "Post Date";
            this.postdate.Name = "postdate";
            this.postdate.ReadOnly = true;
            // 
            // trandate
            // 
            this.trandate.HeaderText = "Trans Date";
            this.trandate.Name = "trandate";
            this.trandate.ReadOnly = true;
            // 
            // ddebit
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.ddebit.DefaultCellStyle = dataGridViewCellStyle3;
            this.ddebit.HeaderText = "Debit";
            this.ddebit.Name = "ddebit";
            this.ddebit.ReadOnly = true;
            // 
            // dcredit
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.dcredit.DefaultCellStyle = dataGridViewCellStyle4;
            this.dcredit.HeaderText = "Credit";
            this.dcredit.Name = "dcredit";
            this.dcredit.ReadOnly = true;
            // 
            // dtrans
            // 
            this.dtrans.HeaderText = "Transaction Narrative";
            this.dtrans.Name = "dtrans";
            this.dtrans.ReadOnly = true;
            this.dtrans.Width = 200;
            // 
            // djvno
            // 
            this.djvno.HeaderText = "J.V. No.";
            this.djvno.Name = "djvno";
            this.djvno.ReadOnly = true;
            this.djvno.Width = 110;
            // 
            // dchq
            // 
            this.dchq.HeaderText = "ChqNo./Ref.";
            this.dchq.Name = "dchq";
            this.dchq.ReadOnly = true;
            this.dchq.Width = 95;
            // 
            // cvou
            // 
            this.cvou.HeaderText = "voucherno";
            this.cvou.MinimumWidth = 2;
            this.cvou.Name = "cvou";
            this.cvou.ReadOnly = true;
            this.cvou.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cvou.Width = 2;
            // 
            // jstack
            // 
            this.jstack.HeaderText = "Journal stack";
            this.jstack.MinimumWidth = 2;
            this.jstack.Name = "jstack";
            this.jstack.ReadOnly = true;
            this.jstack.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.jstack.Width = 2;
            // 
            // tempUpd
            // 
            this.tempUpd.HeaderText = "Temporary Update";
            this.tempUpd.MinimumWidth = 2;
            this.tempUpd.Name = "tempUpd";
            this.tempUpd.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tempUpd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.tempUpd.Width = 2;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(1157, 697);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(119, 17);
            this.label20.TabIndex = 188;
            this.label20.Text = "Transaction Type";
            this.label20.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 101);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 17);
            this.label10.TabIndex = 185;
            this.label10.Text = "Select Date";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 62);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 17);
            this.label7.TabIndex = 184;
            this.label7.Text = "Date Range";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Controls.Add(this.radioButton3);
            this.groupBox2.Controls.Add(this.radioButton4);
            this.groupBox2.Location = new System.Drawing.Point(145, 44);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(427, 44);
            this.groupBox2.TabIndex = 190;
            this.groupBox2.TabStop = false;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(236, 16);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(176, 21);
            this.radioButton5.TabIndex = 2;
            this.radioButton5.Text = "Select Journal Voucher";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(120, 16);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(102, 21);
            this.radioButton3.TabIndex = 1;
            this.radioButton3.Text = "Select Date";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Checked = true;
            this.radioButton4.Location = new System.Drawing.Point(9, 16);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(85, 21);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "All Dates";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(1161, 681);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(223, 44);
            this.groupBox1.TabIndex = 191;
            this.groupBox1.TabStop = false;
            this.groupBox1.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(113, 16);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(91, 21);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "Payments";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(27, 16);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(76, 21);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Journal";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // transDate
            // 
            this.transDate.Enabled = false;
            this.transDate.Location = new System.Drawing.Point(145, 96);
            this.transDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.transDate.Name = "transDate";
            this.transDate.Size = new System.Drawing.Size(425, 22);
            this.transDate.TabIndex = 192;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(635, 53);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 64);
            this.button1.TabIndex = 193;
            this.button1.Text = "Continue";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Navy;
            this.panel1.Controls.Add(this.label22);
            this.panel1.Location = new System.Drawing.Point(1091, 741);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 32);
            this.panel1.TabIndex = 211;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(7, 9);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(261, 17);
            this.label22.TabIndex = 154;
            this.label22.Text = "Copyright NACCUG , All rights Reserved";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.saveButton);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Location = new System.Drawing.Point(16, 690);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(395, 65);
            this.panel2.TabIndex = 230;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(144, 9);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(117, 49);
            this.button5.TabIndex = 35;
            this.button5.Text = "Print";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(11, 9);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(125, 49);
            this.saveButton.TabIndex = 30;
            this.saveButton.Text = "Save Details";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(273, 9);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(108, 49);
            this.button4.TabIndex = 209;
            this.button4.Text = "Exit Form";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(831, 87);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(325, 29);
            this.label12.TabIndex = 447;
            this.label12.Text = "Search by Journal Voucher";
            this.label12.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 448;
            this.label1.Text = "Source";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(117, 16);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(83, 21);
            this.radioButton6.TabIndex = 1;
            this.radioButton6.Text = "Journals";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton6_CheckedChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(263, 16);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(84, 21);
            this.radioButton7.TabIndex = 2;
            this.radioButton7.Text = "Deposits";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton7_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton11);
            this.groupBox3.Controls.Add(this.radioButton10);
            this.groupBox3.Controls.Add(this.radioButton9);
            this.groupBox3.Controls.Add(this.radioButton8);
            this.groupBox3.Controls.Add(this.radioButton6);
            this.groupBox3.Controls.Add(this.radioButton7);
            this.groupBox3.Location = new System.Drawing.Point(145, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(859, 44);
            this.groupBox3.TabIndex = 449;
            this.groupBox3.TabStop = false;
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Checked = true;
            this.radioButton11.Location = new System.Drawing.Point(9, 16);
            this.radioButton11.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(44, 21);
            this.radioButton11.TabIndex = 6;
            this.radioButton11.TabStop = true;
            this.radioButton11.Text = "All";
            this.radioButton11.UseVisualStyleBackColor = true;
            this.radioButton11.CheckedChanged += new System.EventHandler(this.radioButton11_CheckedChanged);
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Enabled = false;
            this.radioButton10.Location = new System.Drawing.Point(721, 16);
            this.radioButton10.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(120, 21);
            this.radioButton10.TabIndex = 5;
            this.radioButton10.Text = "Batch Process";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Enabled = false;
            this.radioButton9.Location = new System.Drawing.Point(541, 16);
            this.radioButton9.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(137, 21);
            this.radioButton9.TabIndex = 4;
            this.radioButton9.Text = "Loan Repayment";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Enabled = false;
            this.radioButton8.Location = new System.Drawing.Point(393, 16);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(107, 21);
            this.radioButton8.TabIndex = 3;
            this.radioButton8.Text = "WithDrawals";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBox1.ForeColor = System.Drawing.Color.Black;
            this.maskedTextBox1.Location = new System.Drawing.Point(583, 679);
            this.maskedTextBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.ReadOnly = true;
            this.maskedTextBox1.Size = new System.Drawing.Size(124, 26);
            this.maskedTextBox1.TabIndex = 450;
            this.maskedTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(485, 692);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 451;
            this.label2.Text = "Total";
            // 
            // maskedTextBox2
            // 
            this.maskedTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maskedTextBox2.ForeColor = System.Drawing.Color.Black;
            this.maskedTextBox2.Location = new System.Drawing.Point(711, 679);
            this.maskedTextBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.maskedTextBox2.Name = "maskedTextBox2";
            this.maskedTextBox2.ReadOnly = true;
            this.maskedTextBox2.Size = new System.Drawing.Size(124, 26);
            this.maskedTextBox2.TabIndex = 452;
            this.maskedTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(996, 676);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(125, 49);
            this.button3.TabIndex = 211;
            this.button3.Text = "Select All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tranupdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1425, 773);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.maskedTextBox2);
            this.Controls.Add(this.maskedTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.transDate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.transGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "tranupdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "tranupdate";
            this.Load += new System.EventHandler(this.tranupdate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.transGrid)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView transGrid;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.DateTimePicker transDate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.RadioButton radioButton11;
        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dupt;
        private System.Windows.Forms.DataGridViewTextBoxColumn duser;
        private System.Windows.Forms.DataGridViewTextBoxColumn actnumb;
        private System.Windows.Forms.DataGridViewTextBoxColumn postdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn trandate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ddebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dcredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtrans;
        private System.Windows.Forms.DataGridViewTextBoxColumn djvno;
        private System.Windows.Forms.DataGridViewTextBoxColumn dchq;
        private System.Windows.Forms.DataGridViewTextBoxColumn cvou;
        private System.Windows.Forms.DataGridViewTextBoxColumn jstack;
        private System.Windows.Forms.DataGridViewCheckBoxColumn tempUpd;
    }
}