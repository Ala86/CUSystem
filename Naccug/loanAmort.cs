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
using TclassLibrary;

namespace WinTcare
{
    public partial class loanAmort : Form
    {
        DataTable clientview = new DataTable();
        DataTable amortview = new DataTable();
        DataTable chkamortview = new DataTable();
        string cs = globalvar.cos;
        double gnLoanAmt = 0.00;
        double gnPerPay = 0.00;
        bool glApproved = false;
        bool glIssued = false;
        bool glAmort = false;
        int ncompid = globalvar.gnCompid;
        int gnLoanID = 0;
        int gnLoantype = 0;
        DateTime gdStartDate;
        public loanAmort()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loanAmort_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Loan Amortization  >>";
            makegrid();
            amortGrid.DataSource = amortview.DefaultView;
            clientgrid.Columns["dloan"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["dloan"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["dinte"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["dinte"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["ddur"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["ddur"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["dintrate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["dintrate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["dtotpay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["dtotpay"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
//            clientgrid.ba

            amortGrid.Columns["outamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns["outamt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            amortGrid.Columns["dpay"].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns["dpay"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            amortGrid.Columns["dprin"].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns["dprin"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns["dint"].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns["dint"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns["begbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns["begbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns["cumInt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns["cumInt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

         

            getclientList(1);
            if(clientview.Rows.Count>0)
            {
                gnLoantype = Convert.ToInt16(clientview.Rows[0]["prd_id"]);
                clientgrid.Focus();
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);

                dobackground();
                clientgrid.Focus();
            }
        }

        private void dobackground()
        {
            for (int i = 0; i < clientview.Rows.Count; i++)
            {
                if (clientview.Rows[i]["lamort"] != null && clientview.Rows[i]["lamort"].ToString() != "")
                {
                    glAmort = Convert.ToBoolean(clientview.Rows[i]["lamort"]);
                    clientgrid.Rows[i].DefaultCellStyle.BackColor = glAmort ? Color.Green : Color.Yellow;
                    clientgrid.Rows[i].DefaultCellStyle.ForeColor = glAmort ? Color.White : Color.Black;
                }
                else
                {
                    clientgrid.Rows[i].DefaultCellStyle.ForeColor = Color.Black; 
                    clientgrid.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
//                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
  //              AllClear2Go();
            }
        }
        private void makegrid()
        {
            amortview.Columns.Add("dperiod");
            amortview.Columns.Add("begbal");
            amortview.Columns.Add("npayment");
            amortview.Columns.Add("nprinpay");
            amortview.Columns.Add("nintpay");
            amortview.Columns.Add("namount");
            amortview.Columns.Add("duedate");
            amortview.Columns.Add("cumInt");

            amortGrid.AutoGenerateColumns = false;
            amortGrid.DataSource = amortview.DefaultView;
            amortGrid.Columns[0].DataPropertyName = "duedate";
            amortGrid.Columns[1].DataPropertyName = "npayment";
            amortGrid.Columns[2].DataPropertyName = "nprinpay";
            amortGrid.Columns[3].DataPropertyName = "nintpay";
            amortGrid.Columns[4].DataPropertyName = "begbal";
            amortGrid.Columns[5].DataPropertyName = "namount";
            amortGrid.Columns[6].DataPropertyName = "cumInt";
            amortGrid.Columns[7].DataPropertyName = "dperiod";
        }

        private void getclientList(int tnRound)
        {
//            string dsql1 = "exec tsp_getLoans4Amort  " + ncompid;
            string dsql1 = "exec tsp_getLoansActive  " + ncompid;
            string dsql2 = "exec tsp_getLoans4Amort_Done " + ncompid;
            string dsql3 = "exec tsp_getLoans4Amort_pend " + ncompid;
            clientview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                switch(tnRound)
                {
                    case 1:
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                        da1.Fill(clientview);
                        break;
                    case 2:
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                        da2.Fill(clientview);
                        break;
                    case 3:
                        SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle);
                        da3.Fill(clientview);
                        break;
                }
                if (clientview.Rows.Count > 0)
                {
                    textBox4.Text = clientview.Rows.Count.ToString().Trim();
                    gnLoantype = Convert.ToInt16 (clientview.Rows[0]["prd_id"]);
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "loan_id";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "loanamt";
                    clientgrid.Columns[3].DataPropertyName = "loan_interest";
                    clientgrid.Columns[4].DataPropertyName = "loandur";
                    clientgrid.Columns[5].DataPropertyName = "totinterest";
                    clientgrid.Columns[6].DataPropertyName = "totpay";
                    ndConnHandle.Close();
                    dobackground();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                    clientgrid.Focus();
                    glAmort = Convert.ToBoolean(clientview.Rows[0]["lamort"]);

                    button1.Enabled = !glAmort ? true : false;
                    button1.BackColor = !glAmort ? Color.Green : Color.Gainsboro;
                    button1.ForeColor = !glAmort ? Color.White : Color.Black;
                }
            }
        }//end of getclientlist

        private void updateAmortAll()
        {
            for(int j=0;j<clientview.Rows.Count; j++)
            {
                if(Convert.ToInt32(clientview.Rows[j]["loan_id"])>0)
                {
                    gnLoanID = Convert.ToInt32(clientview.Rows[j]["loan_id"]);
                    checkamort(gnLoanID);
                    gnLoanID = 0;
                }
            }
        }

        private void updateAmortOne(int tnLoanID)
        {
            //for (int j = 0; j < clientview.Rows.Count; j++)
            //{
            //    if (Convert.ToInt32(clientview.Rows[j]["loan_id"]) > 0)
            //    {
            //        gnLoanID = Convert.ToInt32(clientview.Rows[j]["loan_id"]);
                    checkamort(tnLoanID);
            //        gnLoanID = 0;
            //    }
            //}
        }

        private void checkamort(int loanid)
        {
            chkamortview.Clear();
            string dsql = "select 1 from amortabl where loanid = " + loanid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daa = new SqlDataAdapter(dsql, ndConnHandle);
                daa.Fill(chkamortview);
                if (chkamortview != null && chkamortview.Rows.Count == 0)
                {
                    loanAmortization(loanid);
                    insertAmort();
                    clientview.Clear();
                    amortview.Clear();
                    getclientList(1);
                    if (clientview.Rows.Count > 0)
                    {
                        clientgrid.Focus();
                        gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                        loanAmortization(loanid);
                        glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                        glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);
                        glAmort = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lamort"]);
                        dobackground();
                        saveAmort.Enabled = glApproved && !glIssued && !glAmort ? true : false;
                        saveAmort.BackColor = glApproved && !glIssued && !glAmort ? Color.LawnGreen : Color.Gainsboro;
                    }
                }
                else
                {
                    MessageBox.Show("This loan already has amortization ");
                }
            }
        }
        private void loanAmortization(int loanid)
        {
            amortview.Clear();
            int nyrpay = 12;            //this is the number of payments per year
            int loanDur = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loandur"]);
            gnLoanAmt = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanamt"]);
            gnPerPay = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["payamt"]);
            gdStartDate = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanstart_date"]);
            double nTotalInterest = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["totinterest"]);
            double gnLoanInterest = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);
            double gnOrigLoan = Math.Round(gnLoanAmt, 2);
            double gnCumuInt = 0.00;
            double totalprin = 0.00;
            double totalint = 0.00;
            double nintpay = 0.00;
            double nprinpay = 0.00;
            double nfactor = gnLoanAmt / loanDur;
            double nPrincipal = gnLoanAmt;// - nfactor;
            double AnnInt = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);
            double nPeriodicInt = Math.Pow(1 + (AnnInt / 100), 1.0 / 12) - 1;
           
            if (gnLoantype == 3)
            {
                 for (int j = 1; j <= loanDur; j++)
                {
                    double newrate = AnnInt / 100 / nyrpay;
                 //  gnPerPay = gnLoanAmt/loanDur;  //loanCalculation.pmt(newrate, loanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
                   nintpay = nTotalInterest/loanDur; //loanCalculation.ipmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                   nprinpay = gnLoanAmt / loanDur;  /// loanCalculation.ppmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                   //gnPerPay = gnLoanAmt / loanDur;  //loanCalculation.pmt(newrate, loanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment

                    gnOrigLoan = Math.Round(gnOrigLoan, 2);
                    gnCumuInt = Math.Round((gnCumuInt + Math.Abs(nintpay)), 2);
                    nPrincipal = Math.Abs(nPrincipal) - Math.Abs(nprinpay);
                    totalprin = Math.Abs(totalprin) + Math.Abs(nprinpay);
                    totalint = Math.Abs(totalint) + Math.Abs(nintpay);
                    DataRow adrow = amortview.NewRow();
                    adrow["duedate"] = gdStartDate.AddMonths(j).ToShortDateString();
                    adrow["npayment"] = Math.Abs(gnPerPay).ToString("N2");
                    adrow["nprinpay"] = Math.Abs(nprinpay).ToString("N2");
                    adrow["nintpay"] = Math.Abs(nintpay).ToString("N2");
                    adrow["begbal"] = gnOrigLoan.ToString("N2");
                    adrow["namount"] = nPrincipal.ToString("N2");
                    adrow["cumInt"] = Math.Round(gnCumuInt,2);
                    adrow["dperiod"] = j;

                    amortview.Rows.Add(adrow);
                    gnOrigLoan = Math.Round(Math.Abs(gnOrigLoan), 2) - Math.Round(Math.Abs(nprinpay), 2);
                }
            }
            else {

                for (int j = 1; j <= loanDur; j++)
                {
                    double newrate = AnnInt / 100 / nyrpay;
                    if (gnLoanInterest > 0.00)
                    {
                        gnPerPay = loanCalculation.pmt(newrate, loanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
                        nintpay = loanCalculation.ipmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                        nprinpay = loanCalculation.ppmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                    }
                    else
                    {
                        gnPerPay = gnLoanAmt / loanDur;// loanCalculation.pmt(newrate, loanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
                        nintpay = 0.00;// loanCalculation.ipmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                        nprinpay = gnPerPay;// loanCalculation.ppmt(newrate, j, loanDur, gnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                    }

                    gnOrigLoan = Math.Round(gnOrigLoan, 2);
                    gnCumuInt = Math.Round((gnCumuInt + Math.Abs(nintpay)), 2);
                    nPrincipal = Math.Abs(nPrincipal) - Math.Abs(nprinpay);
                    totalprin = Math.Abs(totalprin) + Math.Abs(nprinpay);
                    totalint = Math.Abs(totalint) + Math.Abs(nintpay);
                    DataRow adrow = amortview.NewRow();
                    adrow["duedate"] = gdStartDate.AddMonths(j).ToShortDateString();
                    adrow["npayment"] = Math.Abs(gnPerPay).ToString("N2");
                    adrow["nprinpay"] = Math.Abs(nprinpay).ToString("N2");
                    adrow["nintpay"] = Math.Abs(nintpay).ToString("N2");
                    adrow["begbal"] = gnOrigLoan.ToString("N2");
                    adrow["namount"] = nPrincipal.ToString("N2");
                    adrow["cumInt"] = gnCumuInt.ToString("N2");
                    adrow["dperiod"] = j;

                    amortview.Rows.Add(adrow);
                    gnOrigLoan = Math.Round(Math.Abs(gnOrigLoan), 2) - Math.Round(Math.Abs(nprinpay), 2);
                }


            }
            textBox10.Text = Math.Abs(Convert.ToDouble(Math.Round(loanDur * gnPerPay, 2))).ToString("N2");
                textBox9.Text = Math.Abs(Math.Round(totalprin, 2)).ToString("N2");
                textBox11.Text = Math.Abs(Math.Round(totalint, 2)).ToString("N2");
            }
      
        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(clientview.Rows.Count>0)
            {
                gnLoantype = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["prd_id"]);
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                loanAmortization(gnLoanID);
                glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);
                glAmort = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lamort"]);
                saveAmort.Enabled = glApproved && !glIssued && !glAmort ? true : false;
                saveAmort.BackColor = glApproved && !glIssued && !glAmort ? Color.LawnGreen : Color.Gainsboro;
                //button1.Enabled = !glAmort;
                //glAmort = Convert.ToBoolean(clientview.Rows[0]["lamort"]);

                button1.Enabled = !glAmort ? true : false;
                button1.BackColor = !glAmort ? Color.Green : Color.Gainsboro;
                button1.ForeColor = !glAmort ? Color.White : Color.Black;

            }
        }


        private void clientgrid_Validated(object sender, EventArgs e)
        {
            MessageBox.Show("control validation");
        }

        private void saveAmort_Click(object sender, EventArgs e)
        {
            insertAmort();
            MessageBox.Show("Amortization Schedule Successfully Completed");
            clientview.Clear();
            amortview.Clear();
            getclientList(1);
            if (clientview.Rows.Count > 0)
            {
                clientgrid.Focus();
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);
                glAmort = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lamort"]);
                saveAmort.Enabled = glApproved && !glIssued && !glAmort ? true : false;
                saveAmort.BackColor = glApproved && !glIssued && !glAmort ? Color.LawnGreen : Color.Gainsboro;
            }
        }

        private void insertAmort()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = " insert into amortabl(due_date,npayment,nprinpmnt,nintpmnt,nbegbal,nendbal,ncumint,loanid,norder,nbalance,amort_date)";
                cglquery += " values ";
                cglquery += " (@ldue_date,@lnpayment,@lnprinpmnt,@lnintpmnt,@lnbegbal,@lnendbal,@lncumint,@lloanid,@lnorder,@lnbalance,convert(date,getdate()))";

                string amortquery = " update loan_det set lamort=1 where loan_id =@lloanid";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                SqlDataAdapter updCommand = new SqlDataAdapter();

                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                updCommand.UpdateCommand = new SqlCommand(amortquery, nConnHandle2);

                updCommand.UpdateCommand.Parameters.Add("@lloanid", SqlDbType.Int).Value = gnLoanID;
                nConnHandle2.Open();
                foreach (DataRow drow in amortview.Rows)
                {
                    insCommand.InsertCommand.Parameters.Add("@ldue_date", SqlDbType.DateTime).Value     = drow["duedate"];
                    insCommand.InsertCommand.Parameters.Add("@lnpayment", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["npayment"]);
                    insCommand.InsertCommand.Parameters.Add("@lnprinpmnt", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["nprinpay"]);
                    insCommand.InsertCommand.Parameters.Add("@lnintpmnt", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["nintpay"]);
                    insCommand.InsertCommand.Parameters.Add("@lnbegbal", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["begbal"]);
                    insCommand.InsertCommand.Parameters.Add("@lnendbal", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["namount"]);
                    insCommand.InsertCommand.Parameters.Add("@lncumint", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["cumInt"]);
                    insCommand.InsertCommand.Parameters.Add("@lloanid", SqlDbType.Int).Value            = gnLoanID; ;
                   insCommand.InsertCommand.Parameters.Add("@lnorder", SqlDbType.Int).Value            = drow["dperiod"];
                    insCommand.InsertCommand.Parameters.Add("@lnbalance", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["npayment"]);

                    insCommand.InsertCommand.ExecuteNonQuery();
                    insCommand.InsertCommand.Parameters.Clear();
                }
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void clientgrid_Enter(object sender, EventArgs e)
        {
            if (clientview.Rows.Count > 0)
            {
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                loanAmortization(gnLoanID);
                glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);
                glAmort = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lamort"]);
                saveAmort.Enabled = glApproved && !glIssued && !glAmort ? true : false;
                saveAmort.BackColor = glApproved && !glIssued && !glAmort ? Color.LawnGreen : Color.Gainsboro;
            }

        }

        /*
 --exec tsp_getLoans4Amort_Done 30
--exec tsp_getLoans4Amort_pend 30

             */
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            getclientList(1);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button1.BackColor = Color.Gainsboro;
            button1.ForeColor = Color.Black;
            getclientList(2);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button1.BackColor = Color.Green;
            button1.ForeColor = Color.White;
            getclientList(3);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton4.Checked)
            {
                textBox3.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
                textBox3.Text = "";
            }
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                textBox3.Text = textBox3.Text.ToString().PadLeft(6, '0');
                getIndclientList(textBox3.Text);
            }
        }

        private void getIndclientList(string tcCustcode)
        {
//            exec tsp_getLoans4Amort_One 30,'000038'
            string dsql = "exec tsp_getLoans4Amort_One  " + ncompid+",'"+tcCustcode+"'";
            clientview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    gnLoantype = Convert.ToInt16(clientview.Rows[0]["prd_id"]);
//                    MessageBox.Show("this is the scooe" + gnLoantype);
                    clientgrid.AutoGenerateColumns = false; 
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "loan_id";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "loanamt";
                    clientgrid.Columns[3].DataPropertyName = "loan_interest";
                    clientgrid.Columns[4].DataPropertyName = "loandur";
                    clientgrid.Columns[5].DataPropertyName = "totinterest";
                    clientgrid.Columns[6].DataPropertyName = "totpay";
                    ndConnHandle.Close();
                    dobackground();
                    //         updateAmort();
                    for (int i = 0; i < 1; i++)
                    {
                        clientview.Rows.Add();
                    }
                    clientgrid.Focus();
                }else { MessageBox.Show("nothing is found or this member"); }
            }
        }//end of getclientlist

        private void button1_Click(object sender, EventArgs e)
        {
//            bool glAmortDone = Convert.ToBoolean(clientgrid.CurrentRow.Cells["lamort"].Value);
            gnLoanID= Convert.ToInt32(clientgrid.CurrentRow.Cells[0].Value);
                     //   MessageBox.Show("Amortization will be generated for loan ID = "+gnLoanID);
            insertAmort();
            MessageBox.Show("Amortization Schedule Successfully Completed");
            clientview.Clear();
            amortview.Clear();
            getclientList(1);
            if (clientview.Rows.Count > 0)
            {
                clientgrid.Focus();
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);
                glAmort = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lamort"]);
                saveAmort.Enabled = glApproved && !glIssued && !glAmort ? true : false;
                saveAmort.BackColor = glApproved && !glIssued && !glAmort ? Color.LawnGreen : Color.Gainsboro;
            }

//            checkamort(lnLoanID);
        }
    }
}
