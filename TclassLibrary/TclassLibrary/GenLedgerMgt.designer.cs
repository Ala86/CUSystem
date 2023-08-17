namespace TclassLibrary
{
    partial class GenLedgerMgt
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Trial Balance");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Income Statement");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Balance Sheet");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Cash Flow");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Accounts Receivable (Aging)");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Accounts Payable (Aging)");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Account Reconciliation");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Reports", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7});
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Main Categories");
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenLedgerMgt));
            this.panel3 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.runButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.repTree = new TclassLibrary.GenLedgerMgt.myTreeView();
            this.fintree = new TclassLibrary.GenLedgerMgt.myTreeView();
            this.genViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.transGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.begbal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.acctGrid = new System.Windows.Forms.DataGridView();
            this.cacct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nbkbal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label36 = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acctGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.SaveButton);
            this.panel3.Controls.Add(this.button7);
            this.panel3.Location = new System.Drawing.Point(2, 633);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(288, 54);
            this.panel3.TabIndex = 296;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(94, 7);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 40);
            this.button4.TabIndex = 135;
            this.button4.Text = "Account Enquiry";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(4, 7);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(68, 40);
            this.SaveButton.TabIndex = 30;
            this.SaveButton.Text = "Verification";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(224, 7);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(58, 40);
            this.button7.TabIndex = 130;
            this.button7.Text = "Exit Form";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(12, 55);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.runButton);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.toDate);
            this.splitContainer1.Panel1.Controls.Add(this.fromDate);
            this.splitContainer1.Panel1.Controls.Add(this.repTree);
            this.splitContainer1.Panel1.Controls.Add(this.fintree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.genViewer);
            this.splitContainer1.Panel2.Controls.Add(this.transGrid);
            this.splitContainer1.Panel2.Controls.Add(this.textBox10);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.acctGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1087, 572);
            this.splitContainer1.SplitterDistance = 380;
            this.splitContainer1.TabIndex = 297;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(47, 509);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(98, 40);
            this.runButton.TabIndex = 136;
            this.runButton.Text = "Run Report";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 478);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 374;
            this.label6.Text = "To";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 449);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 371;
            this.label5.Text = "From";
            // 
            // toDate
            // 
            this.toDate.Location = new System.Drawing.Point(47, 476);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(323, 20);
            this.toDate.TabIndex = 373;
            // 
            // fromDate
            // 
            this.fromDate.Location = new System.Drawing.Point(47, 446);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(323, 20);
            this.fromDate.TabIndex = 372;
            // 
            // repTree
            // 
            this.repTree.CheckBoxes = true;
            this.repTree.Location = new System.Drawing.Point(3, 300);
            this.repTree.Name = "repTree";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Trial Balance";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Income Statement";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Balance Sheet";
            treeNode4.Name = "Node7";
            treeNode4.Text = "Cash Flow";
            treeNode5.Name = "Node4";
            treeNode5.Text = "Accounts Receivable (Aging)";
            treeNode6.Name = "Node5";
            treeNode6.Text = "Accounts Payable (Aging)";
            treeNode7.Name = "Node6";
            treeNode7.Text = "Account Reconciliation";
            treeNode8.Name = "Node0";
            treeNode8.Text = "Reports";
            this.repTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode8});
            this.repTree.Size = new System.Drawing.Size(367, 134);
            this.repTree.TabIndex = 371;
            this.repTree.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.repTree_AfterCheck);
            this.repTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.repTree_AfterSelect);
            this.repTree.Enter += new System.EventHandler(this.repTree_Enter);
            // 
            // fintree
            // 
            this.fintree.Location = new System.Drawing.Point(3, 3);
            this.fintree.Name = "fintree";
            treeNode9.Name = "Node0";
            treeNode9.Text = "Main Categories";
            treeNode9.ToolTipText = "Double Click to Add an Account";
            this.fintree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.fintree.Size = new System.Drawing.Size(367, 291);
            this.fintree.TabIndex = 370;
            this.fintree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fintree2_AfterSelect);
            this.fintree.Enter += new System.EventHandler(this.fintree_Enter);
            this.fintree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.fintree2_MouseDoubleClick);
            // 
            // genViewer
            // 
            this.genViewer.ActiveViewIndex = -1;
            this.genViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.genViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.genViewer.Location = new System.Drawing.Point(6, 3);
            this.genViewer.Name = "genViewer";
            this.genViewer.Size = new System.Drawing.Size(689, 562);
            this.genViewer.TabIndex = 373;
            this.genViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.genViewer.Visible = false;
            // 
            // transGrid
            // 
            this.transGrid.BackgroundColor = System.Drawing.Color.White;
            this.transGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.transGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.begbal,
            this.dataGridViewTextBoxColumn4});
            this.transGrid.Location = new System.Drawing.Point(6, 303);
            this.transGrid.Name = "transGrid";
            this.transGrid.RowHeadersVisible = false;
            this.transGrid.Size = new System.Drawing.Size(689, 260);
            this.transGrid.TabIndex = 432;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 80;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewCellStyle1.NullValue = null;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn2.HeaderText = "Transaction Narrative";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 300;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn3.HeaderText = "Debit";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // begbal
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.begbal.DefaultCellStyle = dataGridViewCellStyle3;
            this.begbal.HeaderText = "Credit";
            this.begbal.Name = "begbal";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn4.HeaderText = "Balance";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(565, 274);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(130, 20);
            this.textBox10.TabIndex = 372;
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(487, 278);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 371;
            this.label11.Text = "Total Balance";
            // 
            // acctGrid
            // 
            this.acctGrid.BackgroundColor = System.Drawing.Color.White;
            this.acctGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.acctGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cacct,
            this.Column2,
            this.nbkbal});
            this.acctGrid.Location = new System.Drawing.Point(6, 3);
            this.acctGrid.Name = "acctGrid";
            this.acctGrid.RowHeadersVisible = false;
            this.acctGrid.Size = new System.Drawing.Size(689, 265);
            this.acctGrid.TabIndex = 1;
            this.acctGrid.Click += new System.EventHandler(this.acctGrid_Click);
            // 
            // cacct
            // 
            this.cacct.HeaderText = "A/c No.";
            this.cacct.Name = "cacct";
            this.cacct.Width = 150;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "A/c Name";
            this.Column2.Name = "Column2";
            this.Column2.Width = 370;
            // 
            // nbkbal
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.nbkbal.DefaultCellStyle = dataGridViewCellStyle5;
            this.nbkbal.HeaderText = "Closing Balance";
            this.nbkbal.Name = "nbkbal";
            this.nbkbal.Width = 150;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.textBox9);
            this.panel1.Controls.Add(this.textBox7);
            this.panel1.Controls.Add(this.textBox8);
            this.panel1.Controls.Add(this.textBox6);
            this.panel1.Location = new System.Drawing.Point(341, 638);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(488, 51);
            this.panel1.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 13);
            this.label10.TabIndex = 370;
            this.label10.Text = "Control Details";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 229;
            this.label1.Text = "Budget Amount";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(194, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 230;
            this.label7.Text = "Total Actuals";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(465, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 16);
            this.label9.TabIndex = 236;
            this.label9.Text = "%";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(303, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 231;
            this.label8.Text = "Variance";
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.Color.White;
            this.textBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox9.Location = new System.Drawing.Point(412, 22);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(51, 22);
            this.textBox9.TabIndex = 235;
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(197, 22);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 232;
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.White;
            this.textBox8.Location = new System.Drawing.Point(306, 22);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(100, 20);
            this.textBox8.TabIndex = 234;
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(81, 22);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 233;
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(896, 42);
            this.groupBox1.TabIndex = 299;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Financial Calendar";
            // 
            // textBox11
            // 
            this.textBox11.BackColor = System.Drawing.Color.White;
            this.textBox11.Location = new System.Drawing.Point(828, 14);
            this.textBox11.Name = "textBox11";
            this.textBox11.ReadOnly = true;
            this.textBox11.Size = new System.Drawing.Size(68, 20);
            this.textBox11.TabIndex = 307;
            this.textBox11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(731, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 13);
            this.label12.TabIndex = 306;
            this.label12.Text = "Number of Periods";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(535, 14);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 305;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(281, 14);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(154, 20);
            this.textBox2.TabIndex = 304;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(68, 14);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(154, 20);
            this.textBox1.TabIndex = 303;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(445, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 302;
            this.label4.Text = "Reporting Period";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(226, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 301;
            this.label3.Text = "End Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 300;
            this.label2.Text = "Start Date";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Navy;
            this.panel2.Controls.Add(this.label36);
            this.panel2.Location = new System.Drawing.Point(847, 661);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(252, 26);
            this.panel2.TabIndex = 369;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.ForeColor = System.Drawing.Color.White;
            this.label36.Location = new System.Drawing.Point(5, 7);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(192, 13);
            this.label36.TabIndex = 154;
            this.label36.Text = "Copyright NACCUG, all rights Reserved";
            // 
            // GenLedgerMgt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1111, 690);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenLedgerMgt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "glman";
            this.Load += new System.EventHandler(this.glman_Load);
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.transGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acctGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView acctGrid;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label12;
        private myTreeView fintree;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer genViewer;
        private myTreeView repTree;
        private System.Windows.Forms.DataGridView transGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn begbal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn cacct;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nbkbal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.Button runButton;
    }
}