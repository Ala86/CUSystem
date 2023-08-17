using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Drawing.Printing;


namespace TclassLibrary
{
    public partial class GenLedgerMgt : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        int nBranchid = 0;
        string dloca = string.Empty;
        int gnNodeIndex = 0;
        string gcUserid = string.Empty;
        string tcStartupDirectory = string.Empty;
        string tcCompanyLogo = string.Empty;

       // string cs = string.Empty;
       // int ncompid = 0;
        string cLocalCaption = string.Empty;


        DataTable treeview = new DataTable();
        DataTable treeview1 = new DataTable();
        DataTable subtreeview = new DataTable();
        DataTable subtreeview1 = new DataTable();
        DataTable subgrpAcctView = new DataTable();
        DataTable trnView = new DataTable();

        GlDataSet gds = new GlDataSet();
        ReportDocument rprt = new ReportDocument();

        string gcSubMain = "";

        public GenLedgerMgt(string tncos, int tncompid, string tclocation,int tnBranchid,string tcStartupDir,string tcCompLogo,string tcUserid)
        {
            InitializeComponent();
            cs = tncos;
            ncompid = tncompid;
            dloca = tclocation;
            nBranchid = tnBranchid;
            tcStartupDirectory = tcStartupDir;
            tcCompanyLogo = tcCompLogo.Trim();
            gcUserid = tcUserid;
        }


        public class myTreeView : TreeView
        {
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams parms = base.CreateParams;
                    parms.Style |= 0x80;
                    return parms;
                    //                    return base.CreateParams;
                }
            }
        }


        private void glman_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< General Ledger Management>>";
            acctGrid.ReadOnly = true;
            dtreeview1();
            acctGrid.Columns["nbkbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["nbkbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            finperiod();

            toDate.MinDate = fromDate.MinDate = Convert.ToDateTime("1980/01/01");

            fromDate.MaxDate = DateTime.Today;
            toDate.MaxDate = DateTime.Today; 

            fromDate.Value = DateTime.Today;
            toDate.Value = DateTime.Today;

            getcompanyLogo();       
        }

        private void getcompanyLogo()
        {
            bool dLogoExists = getReportFile.dLogofile(tcStartupDirectory, tcCompanyLogo);
            if (dLogoExists)
            {
                gds.compDetails.Clear();
                tcCompanyLogo = tcStartupDirectory + "\\" + tcCompanyLogo;
                FileStream fs;
                fs = new FileStream(tcCompanyLogo, FileMode.Open);
                DataRow drow;
                drow = gds.compDetails.NewRow();
                BinaryReader br;
                br = new BinaryReader(fs);
                byte[] imgbyte = new byte[fs.Length + 1];
                imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
                drow[2] = imgbyte;
                gds.compDetails.Rows.Add(drow);
                br.Close();
                fs.Close();
            }
            else
            {
                MessageBox.Show("The company header "+tcCompanyLogo+" does not exist in start up directory, pls inform IT DEPT");
                tcCompanyLogo = "";
            }
        }


        /*       public class myTreeView : TreeView
               {
                   protected override CreateParams CreateParams
                   {
                       get
                       {
                           CreateParams parms = base.CreateParams;
                           parms.Style |= 0x80;
                           return parms;
                           //                    return base.CreateParams;
                       }
                   }
               }*/
        /*
 private void dtreeview()
 {
     fintree.Nodes.Clear();
     string dsql = "select code,categoryname from fincategory order by code ";
    using (SqlConnection ndConnHandle = new SqlConnection(cs))
     {
         ndConnHandle.Open();
         SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
         da.Fill(treeview);
         if (treeview.Rows.Count > 0)
         {
           int nodecount = treeview.Rows.Count;
             for(int k=0;k<nodecount;k++)
             {
                 fintree.Nodes.Add(treeview.Rows[k]["categoryname"].ToString());
                 string dnode =treeview.Rows[k]["categoryname"].ToString();
                 subgrptree(Convert.ToInt32(treeview.Rows[k]["code"]), dnode, k);
             }
             fintree.ExpandAll();
             fintree.Nodes[0].EnsureVisible();
             fintree.ShowNodeToolTips = false; 
         }
     }
 }
 */
        private void dtreeview1()
        {
            treeview1.Clear();
            fintree.Nodes.Clear();
            string dsql1 = "select code,ltrim(rtrim(categoryname)) as categoryname from fincategory order by code ";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                da1.Fill(treeview1);
                if (treeview1.Rows.Count > 0)
                {
                    int nodecount = treeview1.Rows.Count;
                    for (int k = 0; k < nodecount; k++)
                    {
                        fintree.Nodes.Add(treeview1.Rows[k]["categoryname"].ToString());
                        string dnode = treeview1.Rows[k]["categoryname"].ToString();
                        subgrptree1(Convert.ToInt32(treeview1.Rows[k]["code"]), dnode, k);
                    }
                    fintree.CollapseAll();// .ExpandAll();
                    fintree.Nodes[0].EnsureVisible();
                    fintree.ShowNodeToolTips = false;
                }
            }
        }

        private void subgrptree1(int grpcode, string catname, int dcnt)
        {
            subtreeview1.Clear();
            string dsql13 = "select subgrpcode,subgrpname from subgrp where cgrpcode = " + grpcode;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da13 = new SqlDataAdapter(dsql13, ndConnHandle);
                da13.Fill(subtreeview1);
                if (subtreeview1.Rows.Count > 0)
                {
                    foreach (DataRow dsr in subtreeview1.Rows)
                    {
                        string stcode = dsr["subgrpcode"].ToString();
                        string stname = dsr["subgrpname"].ToString();
                        fintree.Nodes[dcnt].Nodes.Add(stcode, stname);
                    }
                }
            }
        }

        private void finperiod()
        {
            string dsql1 = "select st_date,ed_date,nperiods,currentperiod from Finperiod where compid = " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable finview = new DataTable();
                da1.Fill(finview);
                if (finview.Rows.Count > 0)
                {
                    textBox1.Text = Convert.ToDateTime(finview.Rows[0]["st_date"]).ToLongDateString();
                    textBox2.Text = Convert.ToDateTime(finview.Rows[0]["ed_date"]).ToLongDateString();
                    textBox3.Text = finview.Rows[0]["currentperiod"].ToString();
                    textBox11.Text = finview.Rows[0]["nperiods"].ToString();
                }//else { MessageBox.Show("nothing is here"); }
            }

        }


        private void subgrpAccounts(string subcode)
        {
            decimal sumtot = 0.00m;
            int rcount = 0;
            subgrpAcctView.Clear();
            string dsql12 = "select cacctnumb,ltrim(rtrim(cacctname)) as cacctname ,nbookbal,nbudamt,ncontamt from subgrp, glmast where subgrp.subgrpcode=glmast.acode and glmast.intcode=1 and subgrp.subgrpcode = '" + subcode+"'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql12, ndConnHandle);
                da1.Fill(subgrpAcctView);
                if (subgrpAcctView.Rows.Count > 0)
                {
                    rcount = ((18 - subgrpAcctView.Rows.Count) > 0 ? (18 - subgrpAcctView.Rows.Count) : 0);
                    decimal dbudamt = Convert.ToDecimal(subgrpAcctView.Rows[0]["nbudamt"]) > 0.00m ? Convert.ToDecimal(subgrpAcctView.Rows[0]["nbudamt"]) : 1.00m;
                    decimal dactamt = .00m;// Convert.ToDecimal(subgrpAcctView.Rows[0]["ncontamt"])> 0.00m ? Convert.ToDecimal(subgrpAcctView.Rows[0]["ncontamt"]) : 1.00m;
                    string tcFirstAcct = subgrpAcctView.Rows[0]["cacctnumb"].ToString().Trim();
                    acctGrid.AutoGenerateColumns = false;
                    acctGrid.DataSource = subgrpAcctView.DefaultView;
                    acctGrid.Columns[0].DataPropertyName = "cacctnumb";
                    acctGrid.Columns[1].DataPropertyName = "cacctname";
                    acctGrid.Columns[2].DataPropertyName = "nbookbal";
                    for (int i = 0; i < subgrpAcctView.Rows.Count; i++)
                    {
                        sumtot = sumtot + Convert.ToDecimal(subgrpAcctView.Rows[i]["nbookbal"]);
                    }
                    textBox10.Text = sumtot.ToString("N2");
                    textBox6.Text = dbudamt.ToString("N2");    
                    textBox7.Text = dactamt.ToString("N2");     
                    textBox8.Text = (dbudamt - dactamt).ToString("N2");
                    textBox9.Text = ((dactamt / dbudamt) * 100).ToString();
                    textBox9.BackColor = ((dactamt / dbudamt) * 100 >= 75.00m ? Color.Yellow : Color.White);
                    textBox9.ForeColor = ((dactamt / dbudamt) * 100 >= 75.00m ? Color.Red : Color.Black);

                    getAccountsTrans(tcFirstAcct);
                    acctGrid.Rows[0].Cells[0].Selected = true;
                    //for (int k = 0; k < rcount; k++)
                    //{
                    //    subgrpAcctView.Rows.Add();
                    //}
                }
                else
                {
//                    MessageBox.Show("No accounts found under this sub group ");
                    for (int k = 0; k < 18; k++)
                    {
                        subgrpAcctView.Rows.Add();
                    }
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            acctEnquiry act = new acctEnquiry(cs,ncompid,dloca);
            act.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //reconcile rec = new reconcile();
            //rec.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //acRec ap = new acRec();
            //ap.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //acPay dpay = new acPay(cs, nCompid, gcLocalCaption, globalvar.gcCashAccontName, globalvar.gcCashAccont, globalvar.gcCurrName, globalvar.gcCurrUnit, globalvar.gcStaffNo, globalvar.gcUserName, globalvar.gcUserid, globalvar.gnBranchid, globalvar.gcIntSuspense);
            //dpay.ShowDialog();
            //acPay ap = new acPay();
            //ap.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        //private void fintree_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    int nodeindex = Convert.ToInt32(e.Node.Name);
        //    subgrpAccounts(nodeindex);
        //}

        private void SaveButton_Click(object sender, EventArgs e)
        {
            tranupdate ver = new tranupdate();
            ver.ShowDialog();
        }

        private void fintree2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            subgrpAcctView.Clear();
            trnView.Clear();
            string nodename = e.Node.Name;
            gcSubMain = e.Node.Name.ToString().Trim();
            subgrpAccounts(nodename);
        }

        private void fintree2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Do you want to add an account", "Add account information", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                NewAccount addact = new NewAccount(cs,ncompid,dloca, gcSubMain,gcUserid);
                addact.ShowDialog();
                subgrpAccounts(gcSubMain);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fintree2_Click(object sender, EventArgs e)
        {
        }

        private void fintree2_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void repTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            gnNodeIndex = e.Node.Index;
        }

     

        private void TrialBalanceReport()  // string tcCode, string tcReceiptNumber, int tnRecOrder)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                gds.TrialBalData.Clear();
                int rcount = 0;

                DateTime ldFromDate = Convert.ToDateTime(fromDate.Text);
                DateTime ldToDate = Convert.ToDateTime(toDate.Text);

                string cpatquery = "Exec tsp_TrialBalance @tnCompID,@branch,@FromDate,@ToDate";
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@branch", SqlDbType.Int).Value = nBranchid;
                glinsCommand.SelectCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = ldFromDate;
                glinsCommand.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ldToDate;
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(gds.TrialBalData);
                rcount = gds.TrialBalData.Rows.Count;
                if (rcount > 0)
                {
                    rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\TrialBal.rpt");
                    rprt.SetDataSource(gds);
                    genViewer.ReportSource = rprt;
                    genViewer.Zoom(1);
                    rprt.Refresh();
                    rprt.SetParameterValue("stDate",fromDate.Value);
                    rprt.SetParameterValue("edDate", toDate.Value);
                }
                else { MessageBox.Show("No Trial Balance details found for selected date"); }
                dcon.Close();
            }
        } //end of printreceipt

        private void IncomeStatementReport()  
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                gds.PnlData.Clear();
                int rcount = 0;
                DateTime ldToDate = Convert.ToDateTime(toDate.Text);
                string cpatquery = "Exec tsp_IncomeStatements @tnCompID,@ToDate";
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value =Convert.ToDateTime(toDate.Value); 
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(gds.PnlData);
                rcount = gds.PnlData.Rows.Count;
                if (rcount > 0)
                {
                    rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\IncomeStatement.rpt");
                    rprt.SetDataSource(gds);
                    genViewer.ReportSource = rprt;
                    genViewer.Zoom(1);
                    rprt.Refresh();
                    rprt.SetParameterValue("edDate", toDate.Value);
                }
                else { MessageBox.Show("No Income Statement details found for selected conditions"); }
                dcon.Close();
            }
        } //end of printreceipt


        private void BalanceSheetReport()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                gds.PnlData.Clear();
                int rcount = 0;
                DateTime ldToDate = Convert.ToDateTime(toDate.Text);
                string cpatquery = "Exec tsp_BalanceSheet @tnCompID,@ToDate";
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = Convert.ToDateTime(toDate.Value);
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(gds.PnlData);
                rcount = gds.PnlData.Rows.Count;
                if (rcount > 0)
                {
                    rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\BalanceSheet.rpt");
                    rprt.SetDataSource(gds);
                    genViewer.ReportSource = rprt;
                    genViewer.Zoom(1);
                    rprt.Refresh();
                    rprt.SetParameterValue("edDate", toDate.Value);
                }
                else { MessageBox.Show("No Income Statement details found for selected conditions"); }
                dcon.Close();
            }
        } //end of printreceipt

        private void repTree_Enter(object sender, EventArgs e)
        {
            genViewer.Visible = true;
            acctGrid.Visible = transGrid.Visible = label11.Visible = textBox10.Visible = false;
        }

        private void fintree_Enter(object sender, EventArgs e)
        {
            genViewer.Visible = false;
            acctGrid.Visible = transGrid.Visible = label11.Visible = textBox10.Visible =   true;
        }

        private void getAccountsTrans(string tcAcct)
        {
            trnView.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string dsql1 = "exec  tsp_Transactions_one " + ncompid + ", '" + tcAcct + "'";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                da1.Fill(trnView);
                if (trnView.Rows.Count > 0)
                {
                    transGrid.AutoGenerateColumns = false;
                    transGrid.DataSource = trnView.DefaultView;
                    transGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    transGrid.Columns[0].DataPropertyName = "dpostdate";
                    transGrid.Columns[1].DataPropertyName = "ctrandesc";
                    transGrid.Columns[2].DataPropertyName = "ndebit";
                    transGrid.Columns[3].DataPropertyName = "ncredit";
                    transGrid.Columns[4].DataPropertyName = "nnewbal";
                    transGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                    transGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    transGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                    transGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    transGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                    transGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }//else { MessageBox.Show("No transactions found for Account = " + tcAcct); }
            }
        }


        private void acctGrid_Click(object sender, EventArgs e)
        {
            string lcAcct = Convert.ToString(acctGrid.CurrentRow.Cells["cacct"].Value).Trim();
            if(lcAcct !="")
            {
                getAccountsTrans(lcAcct);
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            //REPTrialBalance dpr = new REPTrialBalance(cs, ncompid, cLocalCaption);
            //dpr.ShowDialog();
            //switch (gnNodeIndex)
            //{
            //    case 0:                          //Trial balance 
            //        TrialBalanceReport();
            //        break;
            //    case 1:                          //Income Statement
            //        IncomeStatementReport();
            //        break;
            //    case 2:                          //Balance Sheet
            //        BalanceSheetReport();
            //        break;
            //    case 3:                          //Cash flow
            //        CashFlowReport();
            //        break;
            //    case 4:                          //Accounts Receivable
            //        AccountsReceivable();
            //        break;
            //    case 5:                          //Accounts Payable
            //        AccountsPayable();
            //        break;
            //    case 6:                          //Account Reconciliation 
            //        AccountReconciliation();
            //        break;

            //}
        }

        private void CashFlowReport()
        {

        }

        private void AccountReconciliation()
        {

        }


        private void AccountsReceivable()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {

                dcon.Open();
                gds.DebtAgeData.Clear();
                string lcReportFolder = tcStartupDirectory;
                string lcReportFile = "AgeReport.rpt";
                DateTime ldReceiptDate = DateTime.Today;
                string cpatquery = "Exec tsp_LoanAgeingReport_All @tnCompid ";// tsp_AccountsReceivable @tnCompid";
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(gds.DebtAgeData);
                int rcount = gds.DebtAgeData.Rows.Count;
                if (rcount > 0)
                {
                    MessageBox.Show("Number of records found is " + rcount);
                    ReportDocument DebtAgeDoc = new ReportDocument();
                    bool dRepExists = getReportFile.dReportfile(lcReportFolder, lcReportFile);
                    if (dRepExists)
                    {
                        string lcCashier = globalvar.gcUserName;
                        string lcTranDesc = string.Empty;
                        decimal tnTranAmt = 0.00m;
                        DebtAgeDoc.Load(lcReportFile);//   (@"c:\TcareData\CashReceipt.rpt");                                                                                                                  //                        DebtAgeDoc.SetDataSource(recview);                                                        //set the data source for the crystal document (content of stored procedure)
                        DebtAgeDoc.SetDataSource(gds);
                        try
                        {
                            genViewer.ReportSource = DebtAgeDoc;
                            genViewer.Zoom(1);
                            genViewer.Refresh();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        PrinterSettings getprinterName = new PrinterSettings();
                        DebtAgeDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                        DebtAgeDoc.PrintToPrinter(1, false, 0, 1);
                    }
                    else { MessageBox.Show("Report file <<" + lcReportFile + ">> could not be found , inform IT DEPT "); }
                }
                else { MessageBox.Show("No Accounts Receivable details found"); }
                dcon.Close();
            }
//            updRec.updClient(cs, "rec_no");
        } //end of printreceipt

//        private void AccountsReceivableOld()
//        {
//            using (SqlConnection dcon = new SqlConnection(cs))
//            {
//                dcon.Open();
//                gds.PnlData.Clear();
//                int rcount = 0;
//                DateTime ldToDate = Convert.ToDateTime(toDate.Text);
//                string cpatquery = "Exec tsp_AccountsReceivable @tnCompID";
//                SqlDataAdapter glinsCommand = new SqlDataAdapter();
//                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
//                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
////                glinsCommand.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = Convert.ToDateTime(toDate.Value);
//                glinsCommand.SelectCommand.ExecuteNonQuery();
//                glinsCommand.Fill(gds.PnlData);
//                rcount = gds.PnlData.Rows.Count;
//                if (rcount > 0)
//                {
//                    rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\BalanceSheet.rpt");
//                    rprt.SetDataSource(gds);
//                    genViewer.ReportSource = rprt;
//                    genViewer.Zoom(1);
//                    rprt.Refresh();
//                    rprt.SetParameterValue("edDate", toDate.Value);
//                }
//                else { MessageBox.Show("No Income Statement details found for selected conditions"); }
//                dcon.Close();
//            }
//        }

        private void AccountsPayable()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                gds.PnlData.Clear();
                int rcount = 0;
                DateTime ldToDate = Convert.ToDateTime(toDate.Text);
                string cpatquery = "Exec tsp_AccountsReceivable @tnCompID";
                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = Convert.ToDateTime(toDate.Value);
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(gds.PnlData);
                rcount = gds.PnlData.Rows.Count;
                if (rcount > 0)
                {
                    rprt.Load(System.Windows.Forms.Application.StartupPath + "\\Reports\\BalanceSheet.rpt");
                    rprt.SetDataSource(gds);
                    genViewer.ReportSource = rprt;
                    genViewer.Zoom(1);
                    rprt.Refresh();
                    rprt.SetParameterValue("edDate", toDate.Value);
                }
                else { MessageBox.Show("No Income Statement details found for selected conditions"); }
                dcon.Close();
            }
        }

        private void repTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (repTree.Focused)
            {
                TreeNode currentNode = e.Node;
                TreeNode dparent = currentNode.Parent;
                int parentIndex = currentNode.Parent != null ? currentNode.Parent.Index : -1;
                if (parentIndex > -1)
                {
                    if (currentNode.Checked)
                    {
                        int lnCurrentNodeIndex = currentNode.Index;
                        int lnNodesCount = dparent.Nodes.Count;
                        gnNodeIndex = e.Node.Index;
                        if (!dparent.Checked)
                        {
                            dparent.Checked = true;
                            dparent.BackColor = Color.Green;
                            dparent.ForeColor = Color.White;
                        }
                        currentNode.BackColor = Color.Green;
                        currentNode.ForeColor = Color.White; if (e.Node.Checked)
                            for (int i = 0; i < lnNodesCount; i++)
                            {
                                if (i != lnCurrentNodeIndex)
                                {
                                    e.Node.Parent.Nodes[i].Checked = false;
                                    e.Node.Parent.Nodes[i].BackColor = Color.White;
                                    e.Node.Parent.Nodes[i].ForeColor = Color.Black;
                                }
                            }
                    }
                    else
                    {
                        //rprt.Close();
                        //rprt.Dispose();// reportDocObject.Close() and Dispose()
                    }
                }
            }
        }

      
    }
}
