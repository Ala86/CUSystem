using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;
using System.Data.SqlClient;

namespace WinTcare
{
    public partial class loanApproval : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable clientview = new DataTable();
        DataTable loanview = new DataTable();
        DataTable amortview = new DataTable();
        DataTable chkamortview = new DataTable();

        DateTime gdStartDate;
        string gcMembCode = string.Empty;
        bool glTopUp = false;
        bool glResched = false;
        string gcMemberName = string.Empty;
        int gnLoanID = 0;
        int gnLoanType = 0;
        double gnLoanAmt = 0.00;
        string gcMemberType = "";
        double gnPerPay = 0.00;
        public loanApproval()
        {
            InitializeComponent();
        }

        private void loanApproval_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Loan Approval  >>";
            textBox5.Text = globalvar.gdSysDate.ToLongDateString();
            makeAmort();
            getclientList();
            if (clientview.Rows.Count > 0)
            {

                clientgrid.Columns["loanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
                clientgrid.Columns["loanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                clientgrid.Columns["appldate"].SortMode = DataGridViewColumnSortMode.NotSortable;
                clientgrid.Columns["appldate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                gnLoanAmt = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["principal_amt"]);
                gcMemberName = clientview.Rows[clientgrid.CurrentCell.RowIndex]["membername"].ToString().Trim();
                gcMemberType = clientview.Rows[clientgrid.CurrentCell.RowIndex]["cust_type"].ToString().Trim();
                string lcLoanAcct = clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"] != null ? clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString() : "";
                string memtype = (gcMemberType == "C" ? "Conventional member" : (gcMemberType == "S" ? "Sharia member " : "Both"));
                textBox7.Text = Convert.ToInt16(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_status"]) == 1 ? getNewAcct.newAcctNumber(cs, gcMembCode, "130") : clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString();
                getLoanDetails(gcMembCode, gnLoanID);
                checktopup(gnLoanID);
                AllClear2Go();
            }

        }


        private void makeAmort()
        {
            amortview.Columns.Add("dperiod");
            amortview.Columns.Add("begbal");
            amortview.Columns.Add("npayment");
            amortview.Columns.Add("nprinpay");
            amortview.Columns.Add("nintpay");
            amortview.Columns.Add("namount");
            amortview.Columns.Add("duedate");
            amortview.Columns.Add("cumInt");
        }
        private void getclientList()
        {
            string dsql = "exec tsp_getLoans4Approval  " + ncompid;
            clientview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "principal_amt";
                    clientgrid.Columns[3].DataPropertyName = "loan_appl_date";
                    clientgrid.Columns[4].DataPropertyName = "loan_id";

                    clientgrid.Columns[5].DataPropertyName = "lduration_num";
                    clientgrid.Columns[6].DataPropertyName = "repayment_amt";
                    clientgrid.Columns[7].DataPropertyName = "loanstart_date";
                    clientgrid.Columns[8].DataPropertyName = "totinterest";
                    clientgrid.Columns[9].DataPropertyName = "loan_interest";
                    clientgrid.Columns[10].DataPropertyName = "loanacct";
                    clientgrid.Columns[11].DataPropertyName = "loan_status";
                    clientgrid.Columns[12].DataPropertyName = "prd_id";

                    ndConnHandle.Close();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                    clientgrid.Focus();
                }
            }
        }//end of getclientlist

        private void getLoanDetails(string memcode, int tnloanid)
        {
            loanview.Clear();
            string dsql = "exec tsp_getLoanDetails  " + ncompid+",'"+memcode+"',"+tnloanid;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(loanview);
                if (loanview!=null && loanview.Rows.Count > 0)
                {
                    //MessageBox.Show("tesing");

                    textBox1.Text = loanview.Rows[0]["loantype"].ToString();
                    gnLoanType= Convert.ToInt16(loanview.Rows[0]["prd_id"]);
                    textBox20.Text = Convert.ToDecimal(loanview.Rows[0]["principal_amt"]).ToString("N2");
                    textBox12.Text = loanview.Rows[0]["lduration_num"].ToString();
                    textBox2.Text = loanview.Rows[0]["lduration_num"].ToString();
                    textBox18.Text =Convert.ToDecimal(loanview.Rows[0]["repayment_amt"]).ToString("N2");
                    textBox6.Text = loanview.Rows[0]["econsec"].ToString();
                  //  MessageBox.Show("tesing1");
                    textBox21.Text = Convert.ToDecimal(loanview.Rows[0]["total_interest"]).ToString("N2");
                    textBox13.Text = (Convert.ToDouble(textBox21.Text) + gnLoanAmt).ToString("N2");
                    textBox10.Text = Convert.ToInt32(loanview.Rows[0]["graceperiod"]).ToString();
                    textBox4.Text =  Convert.ToInt32(loanview.Rows[0]["savebal"]).ToString();
                    int nofpay = Convert.ToInt32(loanview.Rows[0]["nofpayperyear"]);
                    textBox16.Text = (nofpay == 365 ? "Daily" :   (nofpay == 52 ? "Weekly" :  (nofpay == 26 ? "Fortnight" :
                    (nofpay == 12 ? "Monthly " : (nofpay == 4 ? "Quarterly " :  (nofpay == 2 ? "Half-yearly " :  (nofpay == 1 ? "Yearly " : "Undefined frequency")))))));
                }
            }
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
                    //clientview.Clear();
                    //amortview.Clear();
                    //getclientList(1);
                    //if (clientview.Rows.Count > 0)
                    //{
                    //    clientgrid.Focus();
                    //    gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
                    //    loanAmortization(loanid);
                    //    glApproved = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lapproved"]);
                    //    glIssued = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lissued"]);
                    //    glAmort = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["lamort"]);
                    //    dobackground();
                    //    saveAmort.Enabled = glApproved && !glIssued && !glAmort ? true : false;
                    //    saveAmort.BackColor = glApproved && !glIssued && !glAmort ? Color.LawnGreen : Color.Gainsboro;
                    //}
                }
            }
        }

        private void loanAmortization(int loanid)
        {

            amortview.Clear();
            int nyrpay = 12;            //this is the number of payments per year
            int loanDur = Convert.ToInt16(clientgrid.CurrentRow.Cells["loandur"].Value);
            gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["loanAmt"].Value);
            gnPerPay = Convert.ToDouble(clientgrid.CurrentRow.Cells["rpayment"].Value);
            gdStartDate = Convert.ToDateTime(clientgrid.CurrentRow.Cells["stdate"].Value);
            double nTotalInterest = Convert.ToDouble(clientgrid.CurrentRow.Cells["totint"].Value);
            double gnLoanInterest = Convert.ToDouble(clientgrid.CurrentRow.Cells["intrate"].Value);
            double gnOrigLoan = Math.Round(gnLoanAmt, 2);
            double gnCumuInt = 0.00;
            double totalprin = 0.00;
            double totalint = 0.00;
            double nintpay = 0.00;
            double nprinpay = 0.00;
            double nfactor = gnLoanAmt / loanDur;
            double nPrincipal = gnLoanAmt;// - nfactor;
            double AnnInt = Convert.ToDouble(gnLoanInterest); // clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);
            double nPeriodicInt = Math.Pow(1 + (AnnInt / 100), 1.0 / 12) - 1;

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




        private void checktopup(int loanid)
        {
            loanview.Clear();
            string dsql = "exec  tsp_checkLoansTopUp  " + ncompid + "," + loanid;

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable topview = new DataTable();
                da.Fill(topview);
                if (topview != null && topview.Rows.Count > 0)
                {
                    glTopUp = Convert.ToInt16(topview.Rows[0]["loan_status"])==3 ? true: false;
                    glResched = Convert.ToInt16(topview.Rows[0]["loan_status"]) == 4 ? true : false;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (gcMembCode != "" && textBox20.Text.ToString().Trim() != "")  

            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion

        private void textBox20_Validated(object sender, EventArgs e)
        {
            if( Convert.ToString(textBox20.Text) !="" && Convert.ToDouble(textBox20.Text)> gnLoanAmt)
            {
                MessageBox.Show("Cannot be more than applied Amount");
                textBox20.Text = gnLoanAmt.ToString("N2");
            }
            else
            {
                textBox20.Text = (textBox20.Text.ToString() != "" ? Convert.ToDecimal(textBox20.Text).ToString("N2") : 0.00m.ToString());
            }
            AllClear2Go();
        }

        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
//            clientgrid.EndEdit();
//            if (clientview.Rows.Count > 0)
//            {
//                gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);
//                int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
//                gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value);
//                gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["loanAmt"].Value);
//                gcMemberName = clientgrid.CurrentRow.Cells["tcMembName"].Value.ToString().Trim();
//                string lcLoanAcct = clientgrid.CurrentRow.Cells["loanAcct"].Value.ToString().Trim()=="" ? getNewAcct.newAcctNumber(cs, gcMembCode, "130"):  clientgrid.CurrentRow.Cells["loanAcct"].Value.ToString().Trim();// clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"] != null ? clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString() : "";
//                textBox7.Text = lcLoanAcct;// Convert.ToInt16(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_status"]) == 1 ? getNewAcct.newAcctNumber(cs, gcMembCode, "130") : clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString();
//                getLoanDetails(gcMembCode,gnLoanID);
//                checktopup(gnLoanID);
////                textBox7.Text = getNewAcct.newAcctNumber(cs, gcMembCode, "130");
//            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int lnLoanId = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value);
             updateloan();
            if (!glTopUp && !glResched ) //This is a new loan
            {

                string lcAcctNumb1 = textBox7.Text.ToString().Trim();// "130" + gcMembCode; //&&conventional loans
                string lcAcctNumb2 = "131" + lcAcctNumb1.Substring(3, 8);// gcMembCode; //&&sharia facilities
                //  gnLoanType = Convert.ToInt32(clientgrid.CurrentRow.Cells["nloantype"].Value);
               // gnLoanType = Convert.ToInt16(loanview.Rows[0]["prd_id"]);
                string lcCustCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);
                switch (gcMemberType)
                {
                    case "C":
                      //  MessageBox.Show("This is loan approval");
                        createAccount(lcAcctNumb1,lcCustCode ); //conventional loan account
                        break;
                    case "S":
                        createAccount(lcAcctNumb2,lcCustCode); //sharia loan account
                        break;
                    case "B":
                        createAccount(lcAcctNumb1,lcCustCode); //conventional loan account
                        createAccount(lcAcctNumb2,lcCustCode); //sharia loan account
                        break;
                }
            }
            else                          //this is a topup loan so a new accountnumber is not created
            {
                string lcAcctNumb1 = textBox7.Text.ToString().Trim(); // "130" + gcMembCode; //&&conventional loans
            }
            checkamort(lnLoanId);
//            insertAmort();
            initvariables();
        }

        private void updateloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
             if (MessageBox.Show("Do you want to approve the loan", "Loan Approval",  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==DialogResult.Yes)
                {
                string cglquery = "update loan_det set lapproved = 1, lissued=0,loan_appr_date = convert(date,getdate()),principal_amt=@loanamt, capprovedby =@approvedby,lduration_num=@duration,loanacct = @lloanacct where loan_id=@loanid";
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

                    insCommand.UpdateCommand.Parameters.Add("@loanamt", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox20.Text);
                    insCommand.UpdateCommand.Parameters.Add("@approvedby", SqlDbType.VarChar).Value = globalvar.gcUserid;
                    insCommand.UpdateCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnLoanID; 
                    insCommand.UpdateCommand.Parameters.Add("@duration", SqlDbType.Int).Value = Convert.ToInt32(textBox2.Text);
                    insCommand.UpdateCommand.Parameters.Add("@lloanacct", SqlDbType.VarChar).Value = textBox7.Text;

                    nConnHandle2.Open();
                    insCommand.UpdateCommand.ExecuteNonQuery();
                    nConnHandle2.Close();
                  //  MessageBox.Show("This is loan approval 1");
                    string auditDesc = "Loan Approval  ->" + gcMembCode;
                    AuditTrail au = new AuditTrail();
                    au.upd_audit_trail(cs, auditDesc, 0.00m, Convert.ToDecimal(textBox20.Text), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                }
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
                    insCommand.InsertCommand.Parameters.Add("@ldue_date", SqlDbType.DateTime).Value = drow["duedate"];
                    insCommand.InsertCommand.Parameters.Add("@lnpayment", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["npayment"]);
                    insCommand.InsertCommand.Parameters.Add("@lnprinpmnt", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["nprinpay"]);
                    insCommand.InsertCommand.Parameters.Add("@lnintpmnt", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["nintpay"]);
                    insCommand.InsertCommand.Parameters.Add("@lnbegbal", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["begbal"]);
                    insCommand.InsertCommand.Parameters.Add("@lnendbal", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["namount"]);
                    insCommand.InsertCommand.Parameters.Add("@lncumint", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["cumInt"]);
                    insCommand.InsertCommand.Parameters.Add("@lloanid", SqlDbType.Int).Value = gnLoanID; ;
                    insCommand.InsertCommand.Parameters.Add("@lnorder", SqlDbType.Int).Value = drow["dperiod"];
                    insCommand.InsertCommand.Parameters.Add("@lnbalance", SqlDbType.Decimal).Value = Convert.ToDecimal(drow["npayment"]);

                    insCommand.InsertCommand.ExecuteNonQuery();
                    insCommand.InsertCommand.Parameters.Clear();
                }
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        //private void updateloanBal(decimal nbookbal,string cacctnumb)  //this is for top up loan as the reschedulted loans do not affect the book balance, only the amortization
        //{
        //    using (SqlConnection nConnHandle2 = new SqlConnection(cs))
        //    {
        //        string cglquery = "update glmast set nbookbal = @lnBookBal,ncleabal=0.00,nunclbal=0.00 where cacctnumb = @lcCacctnumb";
        //        SqlDataAdapter insCommand = new SqlDataAdapter();
        //        insCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

        //        insCommand.UpdateCommand.Parameters.Add("@lnBookBal", SqlDbType.Decimal).Value = nbookbal;
        //        insCommand.UpdateCommand.Parameters.Add("@lcCacctnumb", SqlDbType.VarChar).Value = cacctnumb;

        //        nConnHandle2.Open();
        //        insCommand.UpdateCommand.ExecuteNonQuery();
        //        nConnHandle2.Close();
        //    }
        //}

       private void createAccount(string tcAcctNumb,string tcMembCode)
        {
         //   MessageBox.Show("This is the product Type" + gnLoanType);
            string acode = tcAcctNumb.Substring(0, 3);
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "Insert Into glmast (cacctnumb,cacctname,acode,dopedate,compid,ccustcode,cuserid,prd_id)";
                cglquery += " values (@tcAcctNumb,@lcAcctName,@lcAcode,convert(date,getdate()),@ncompid,@lcCustCode,@lcuserid,@prd_id)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value =tcAcctNumb;
                insCommand.InsertCommand.Parameters.Add("@lcAcctName", SqlDbType.VarChar).Value = gcMemberName+" "+tcAcctNumb.Substring(9,2);
                insCommand.InsertCommand.Parameters.Add("@lcAcode", SqlDbType.VarChar).Value = acode;
                insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.VarChar).Value = ncompid;
                insCommand.InsertCommand.Parameters.Add("@lcCustCode", SqlDbType.VarChar).Value = tcMembCode;
                insCommand.InsertCommand.Parameters.Add("@lcUserid", SqlDbType.VarChar).Value = globalvar.gcUserid;
                insCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = gnLoanType;

                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ApproveAllLoans();// this.Close();
        }
        private void initvariables()
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = textBox10.Text = textBox12.Text = textBox13.Text =
            textBox16.Text = textBox18.Text = textBox20.Text = textBox21.Text = "";
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            getclientList();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to Reject Loan", "Loan Rejection ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                loanReject lrej = new loanReject(gcMembCode, textBox1.Text.Trim(), gnLoanAmt, gnLoanID);
                lrej.ShowDialog();
                textBox13.Text = "";
                textBox16.Text = "";
                textBox21.Text = "";
                textBox6.Text = "";
                textBox12.Text = "";
                textBox18.Text = "";
                getclientList();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            loanAmort lamt = new loanAmort();
            lamt.ShowDialog();
        }


        private void ApproveAllLoans()
        {
            if (MessageBox.Show("Do you want to approve all loans", "Loan Approval", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                using (SqlConnection nConnHandle2 = new SqlConnection(cs))
                {
                    nConnHandle2.Open();
                    string cglquery = "update loan_det set lapproved = 1, lissued=0,loan_appr_date = convert(date,getdate()),principal_amt=@loanamt, capprovedby =@approvedby,lduration_num=@duration,loanacct = @lloanacct where loan_id=@loanid";
                    SqlDataAdapter insCommand = new SqlDataAdapter();
                    insCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

                    foreach (DataGridViewRow dgr in clientgrid.Rows)
                    {
                        if (Convert.ToString(dgr.Cells[0]).Trim() != "" && Convert.ToString(dgr.Cells["loanid"].Value).Trim() !="")
                        {
                            int lnLoanId = Convert.ToInt32(dgr.Cells["loanid"].Value);
                            string lcCustCode = Convert.ToString(dgr.Cells["dcuscode"].Value).Trim();
                            string lcNewAcct = Convert.ToInt16(dgr.Cells["loanStatus"].Value) == 1 ? getNewAcct.newAcctNumber(cs, lcCustCode, "130") :
                               Convert.ToString(dgr.Cells["loanAcct"].Value).Trim();
                            string lcMemberType = clientview.Rows[dgr.Index]["cust_type"].ToString().Trim();
                            string lcCustcode = Convert.ToString(dgr.Cells["dcuscode"].Value).Trim();
                            gcMemberName = Convert.ToString(dgr.Cells["tcMembName"].Value).Trim();// clientview.Rows[clientgrid.CurrentCell.RowIndex]["membername"].ToString().Trim();
                          //  gnLoanType = Convert.ToInt32(dgr.Cells["nloantype"].Value);

                            insCommand.UpdateCommand.Parameters.Add("@loanamt", SqlDbType.Decimal).Value = Convert.ToDecimal(dgr.Cells["loanAmt"].Value);
                            insCommand.UpdateCommand.Parameters.Add("@approvedby", SqlDbType.VarChar).Value = globalvar.gcUserid;
                            insCommand.UpdateCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = lnLoanId;
                            insCommand.UpdateCommand.Parameters.Add("@duration", SqlDbType.Int).Value = Convert.ToInt32(dgr.Cells["loandur"].Value);
                            insCommand.UpdateCommand.Parameters.Add("@lloanacct", SqlDbType.VarChar).Value = Convert.ToString(dgr.Cells["loanAcct"].Value).Trim();

                            insCommand.UpdateCommand.ExecuteNonQuery();
                            insCommand.UpdateCommand.Parameters.Clear();

                            string auditDesc = "Loan Approval  ->" + gcMembCode;
                            AuditTrail au = new AuditTrail();
                            au.upd_audit_trail(cs, auditDesc, 0.00m, Convert.ToDecimal(textBox20.Text), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            checktopup(lnLoanId);

                            if (!glTopUp && !glResched) //This is a new loan
                            {
                                string lcAcctNumb1 = lcNewAcct.Trim();
                                string lcAcctNumb2 = "131" + lcAcctNumb1.Substring(3, 8);
                                switch (lcMemberType)
                                {
                                    case "C":
                                        createAccount(lcAcctNumb1,lcCustCode); //conventional loan account
                                        break;
                                    case "S":
                                        createAccount(lcAcctNumb2,lcCustCode); //sharia loan account
                                        break;
                                    case "B":
                                        createAccount(lcAcctNumb1,lcCustCode); //conventional loan account
                                        createAccount(lcAcctNumb2,lcCustCode); //sharia loan account
                                        break;
                                }

                            }
                            else                          //this is a topup loan so a new accountnumber is not created
                            {
                                string lcAcctNumb1 = lcNewAcct;// textBox7.Text.ToString().Trim(); // "130" + gcMembCode; //&&conventional loans
                            }
                        }
                    }
                    nConnHandle2.Close();
                    MessageBox.Show("All loans have been approved successfully ");
                }
            }
            initvariables();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clientgrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            clientgrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void clientgrid_Click(object sender, EventArgs e)
        {
//            clientgrid.EndEdit();
            if (clientview.Rows.Count > 0 && Convert.ToString(clientgrid.CurrentRow.Cells[0].Value).Trim() != "")
            {
                gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);
                int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
                 gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value);
                gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                gcMemberName = clientgrid.CurrentRow.Cells["tcMembName"].Value.ToString().Trim();
                string lcLoanAcct = clientgrid.CurrentRow.Cells["loanAcct"].Value.ToString().Trim() == "" ? getNewAcct.newAcctNumber(cs, gcMembCode, "130") : clientgrid.CurrentRow.Cells["loanAcct"].Value.ToString().Trim();// clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"] != null ? clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString() : "";
                textBox7.Text = lcLoanAcct;// Convert.ToInt16(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_status"]) == 1 ? getNewAcct.newAcctNumber(cs, gcMembCode, "130") : clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString();
                getLoanDetails(gcMembCode, gnLoanID);
                checktopup(gnLoanID);
            }

        }
    }
}
