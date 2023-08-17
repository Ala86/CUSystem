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
using System.IO;
using System.Drawing.Printing;
using CrystalDecisions.CrystalReports.Engine;
using RestSharp;
using System.Net;

namespace WinTcare
{
    public partial class LoanDisburse : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable clientview = new DataTable();
        DataTable branchView = new DataTable();
        DataTable loanview = new DataTable();
        DataTable acctview = new DataTable();
        DataTable chkamortview = new DataTable();
        DataTable amortview = new DataTable();
        string gcControlAcct = string.Empty;
        DepositDataSet dds = new DepositDataSet();
        DateTime gdTransactionDate = new DateTime();
        DateTime ldToday = DateTime.Today;
        DataTable feeView = new DataTable();
        string gcReceiptNo = string.Empty;
        //        int gnLoanTyp = 0;

        string gcMembCode = string.Empty;
        string gcFeeAccount = string.Empty;
        string gcAcctNumb = string.Empty;
        string ccontra = globalvar.gcCashAccont;
        string gcDjv = string.Empty;
        int gnLoanID = 0;
        int gnLoanProduct = 0;
        int gnLoanSchedule = 0;
        int gnLoanStatus = 0;
        int gnloanDur = 0;
        double  gnLoanAmt = 0.00;
        double  gnPerPay = 0.00;
        DateTime gdStartDate = new  DateTime();
        decimal gnTotInterest = 0.00m;
        decimal gnLoanInterest = 0.00m;

        bool glCashOk = false;
        bool glCheqOk = false;
        bool glMobiOk = false;
        bool glTopUp = false;
        bool glResched = false;

        string tcContra = string.Empty;
        string tcDesc = string.Empty;
        string tcAcctNumb = string.Empty;
        string tcCustcode = string.Empty;
        string tcContcode = string.Empty;
        string tcTranCode = string.Empty;
        string gcUserid = globalvar.gcUserid;
        decimal tnContAmt = 0.00m;
        string tcVoucher = string.Empty;
        string tcChqno = string.Empty;
        bool glLoanProcess = false;

        public EventHandler RefreshNeeded;
        public LoanDisburse()
        {
            InitializeComponent();
        }

        private void LoanDisburse_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Loan Disbursement  >>";
            //textBox5.Text = globalvar.gdSysDate.ToLongDateString();
            makeAmortView();
            getclientList();
            if (clientview.Rows.Count > 0)
            {
                clientgrid.Columns["loanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
                clientgrid.Columns["loanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                clientgrid.Columns["appldate"].SortMode = DataGridViewColumnSortMode.NotSortable;
                clientgrid.Columns["appldate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                gcMembCode = Convert.ToString(clientview.Rows[0]["ccustcode"]);
                gnLoanID = Convert.ToInt32(clientview.Rows[0]["loan_id"]);
                gnLoanAmt = Convert.ToDouble(clientview.Rows[0]["principal_amt"]);
                getLoanDetails(gcMembCode,gnLoanID);
            }
            getFee();
            radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = clientview.Rows.Count > 0 ? true: false;
            //            textBox3.Text = 
            textBox4.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccont : "");
            textBox8.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccontName : "");
            string djv = CuVoucher.genJv(cs, globalvar.gdSysDate);
            gcDjv = "<" + djv.Substring(0, 2) + "-" + djv.Substring(2, 2) + "-" + djv.Substring(4, 2) + ">" + djv.Substring(6, 4);
             
        }

        private void makeAmortView()
        {
            amortview.Columns.Add("duedate");
            amortview.Columns.Add("npayment");
            amortview.Columns.Add("nprinpay");
            amortview.Columns.Add("nintpay");
            amortview.Columns.Add("begbal");

            amortview.Columns.Add("namount");
            amortview.Columns.Add("cumInt");
            amortview.Columns.Add("dperiod");

            //adrow["duedate"] = gdStartDate.AddMonths(j).ToShortDateString();
            //adrow["npayment"] = Math.Abs(gnPerPay).ToString("N2");
            //adrow["nprinpay"] = Math.Abs(nprinpay).ToString("N2");
            //adrow["nintpay"] = Math.Abs(nintpay).ToString("N2");
            //adrow["begbal"] = gnOrigLoan.ToString("N2");
            //adrow["namount"] = nPrincipal.ToString("N2");
            //adrow["cumInt"] = gnCumuInt.ToString("N2");
            //adrow["dperiod"] = j;


        }
        private void getFee()
        {
            feeView.Clear();
            // textBox22.Text = "";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                // MessageBox.Show("this is the Fee 0");
                ndConnHandle.Open();
                string dsql2l = "exec SPFeeCharges";
                SqlDataAdapter da2l = new SqlDataAdapter(dsql2l, ndConnHandle);
                da2l.Fill(feeView);
                if (feeView != null && feeView.Rows.Count > 0)
                {
                    //   MessageBox.Show("this is the Fee");

                    //  MessageBox.Show("this is the topup"+gnIntScope);
                    comboBox2.DataSource = feeView.DefaultView;
                    comboBox2.DisplayMember = "feeName";
                    comboBox2.ValueMember = "FeeID";
                    comboBox2.SelectedIndex = -1;

                    //  textBox22.Text = feeView.Rows[0]["feeAmount"].ToString();
                    // gcFeeAccount = feeView.Rows[0]["feeIncome"].ToString();
                }
                else { MessageBox.Show("No active Fee found"); }
            }
        }
        private void getclientList()
        {
            string dsql = "exec tsp_getLoans4Issue  " + ncompid;
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

                    textBox17.Text = clientview.Rows[0]["ctel"].ToString();
                    gnLoanStatus = Convert.ToInt16(clientview.Rows[0]["loan_status"].ToString());
                    ndConnHandle.Close();
//                    updateAmort();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                    clientgrid.Focus();
                }
//                else { MessageBox.Show("No Pending Disbursement"); }

                //************Getting banks
                string dsql12 = "select bnk_name,bnk_id from bank order by bnk_name ";
                SqlDataAdapter da12 = new SqlDataAdapter(dsql12, ndConnHandle);
                DataTable ds12 = new DataTable();
                da12.Fill(ds12);
                if (ds12 != null)
                {
                    comboBox1.DataSource = ds12.DefaultView;
                    comboBox1.DisplayMember = "bnk_name";
                    comboBox1.ValueMember = "bnk_id";
                    comboBox1.SelectedIndex = -1;

                }
            }
            }//end of getclientlist


        private void updateAmort(int loanid)
        {
//            for (int j = 0; j < clientview.Rows.Count; j++)
  //          {
    //            if (Convert.ToInt32(clientview.Rows[j]["loan_id"]) > 0)
      //          {
//                    gnLoanID = Convert.ToInt32(clientview.Rows[j]["loan_id"]);
                    checkamort(loanid);
//                    gnLoanID = 0;
        //        }
          //  }
        }


        private void getLoanDetails(string memcode,int tnloanid)
        {
            string dsql = "exec tsp_getLoanDetails  " + ncompid + ",'" + memcode + "',"+tnloanid;
            loanview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {

                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(loanview);
                if (loanview.Rows.Count > 0)
                {
                    gnLoanSchedule = Convert.ToInt32(loanview.Rows[0]["Loan_Status"]);
                    gnloanDur = Convert.ToInt32(loanview.Rows[0]["lduration_num"]);
                    gnLoanProduct = Convert.ToInt32(loanview.Rows[0]["prd_id"]);
                    gdStartDate = Convert.ToDateTime(loanview.Rows[0]["loanstart_date"]);
                    gnPerPay = Convert.ToDouble(loanview.Rows[0]["repayment_amt"]);
                    gnTotInterest = Convert.ToDecimal(loanview.Rows[0]["total_interest"]);
                    textBox12.Text = loanview.Rows[0]["lduration_num"].ToString();
                    textBox18.Text = Convert.ToDecimal(loanview.Rows[0]["repayment_amt"]).ToString("N2");
                    textBox6.Text = loanview.Rows[0]["econsec"].ToString();
                    textBox21.Text = Convert.ToDecimal(loanview.Rows[0]["total_interest"]).ToString("N2");
                    textBox13.Text = (Convert.ToDouble(textBox21.Text) + gnLoanAmt).ToString("N2");
                    if (gnLoanSchedule == 4)
                    {
                        textBox2.Text = 0.00.ToString("N2");
                    }else
                    {
                        textBox2.Text = gnLoanAmt.ToString("N2");
                    }
                    gcAcctNumb = loanview.Rows[0]["loanacct"].ToString();
                    textBox3.Text = loanview.Rows[0]["loanacct"].ToString();
                    textBox7.Text = loanview.Rows[0]["loanAcctName"].ToString();
                    int nofpay = Convert.ToInt32(loanview.Rows[0]["nofpayperyear"]);
                    textBox16.Text = (nofpay == 365 ? "Daily" : (nofpay == 52 ? "Weekly" : (nofpay == 26 ? "Fortnight" :
                    (nofpay == 12 ? "Monthly " : (nofpay == 4 ? "Quarterly " : (nofpay == 2 ? "Half-yearly " : (nofpay == 1 ? "Yearly " : "Undefined frequency")))))));
                    checktopup(tnloanid);
                    AllClear2Go();
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
                    MessageBox.Show("no amortization was done for this client, we will create");
                    loanAmortization();
                    insertAmort();
                }
            }
        }

        private void loanAmortization()
        {
            amortview.Clear();
            int nyrpay = 12;            //this is the number of payments per year
//            int loanDur = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loandur"]);
//            gnLoanAmt = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanamt"]);
//            gnPerPay = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["payamt"]);
  //          gdStartDate = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanstart_date"]);

            double nTotalInterest = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["totinterest"]);
            double gnOrigLoan = Math.Round(Convert.ToDouble(gnLoanAmt), 2);
//            double dloanAmt = Convert.ToDouble(gnLoanAmt);
            double gnCumuInt = 0.00;
            double totalprin = 0.00;
            double totalint = 0.00;
            double nintpay = 0.00;
            double nprinpay = 0.00;
            double nfactor = Convert.ToDouble(gnLoanAmt) / Convert.ToDouble(gnloanDur); 
            double nPrincipal = Convert.ToDouble(gnLoanAmt);// - nfactor;
            double AnnInt = Convert.ToDouble(gnLoanInterest);// Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);
            double nPeriodicInt = Math.Pow(1 + (AnnInt / 100), 1.0 / 12) - 1;

            for (int j = 1; j <= gnloanDur; j++)
            {
                double newrate = AnnInt / 100 / nyrpay;

                gnPerPay = loanCalculation.pmt(newrate,gnloanDur, gnLoanAmt, 0.00, 0); // (newrate, gnloanDur, 0.00, 0.00, 0);  // Fixed Periodic Payment
                nintpay = loanCalculation.ipmt(newrate, j, gnloanDur, gnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                nprinpay = loanCalculation.ppmt(newrate, j, gnloanDur, gnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                gnOrigLoan = Math.Round(gnOrigLoan, 2);
                gnCumuInt = Math.Round((gnCumuInt + Math.Abs(nintpay)), 2);
                nPrincipal = Math.Abs(nPrincipal) - Math.Abs(nprinpay);
                totalprin = Math.Abs(totalprin) + Math.Abs(nprinpay);
                totalint = Math.Abs(totalint) + Math.Abs(nintpay);
                DataRow adrow = amortview.NewRow();
              //  MessageBox.Show("before adding rows to amortization view with j = "+j);
                adrow["duedate"] = gdStartDate.AddMonths(j).ToShortDateString();
                adrow["npayment"] = Math.Abs(gnPerPay).ToString("N2");
                adrow["nprinpay"] = Math.Abs(nprinpay).ToString("N2");

              ////  MessageBox.Show("step 0");
                adrow["nintpay"] = Math.Abs(nintpay).ToString("N2");
                adrow["begbal"] = gnOrigLoan.ToString("N2");

               /// MessageBox.Show("step 1");
                adrow["namount"] = nPrincipal.ToString("N2");
                adrow["cumInt"] = gnCumuInt.ToString("N2");
                adrow["dperiod"] = j;
               // MessageBox.Show("step 2");

                amortview.Rows.Add(adrow);
                gnOrigLoan = Math.Round(Math.Abs(gnOrigLoan), 2) - Math.Round(Math.Abs(nprinpay), 2);
            }
//            textBox10.Text = Math.Abs(Convert.ToDouble(Math.Round(gnloanDur * gnPerPay, 2))).ToString("N2");
  //          textBox9.Text = Math.Abs(Math.Round(totalprin, 2)).ToString("N2");
    //        textBox11.Text = Math.Abs(Math.Round(totalint, 2)).ToString("N2");
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
                    insCommand.InsertCommand.Parameters.Add("@lnpayment", SqlDbType.Decimal).Value = drow["npayment"];
                    insCommand.InsertCommand.Parameters.Add("@lnprinpmnt", SqlDbType.Decimal).Value = drow["nprinpay"];
                    insCommand.InsertCommand.Parameters.Add("@lnintpmnt", SqlDbType.Decimal).Value = drow["nintpay"];
                    insCommand.InsertCommand.Parameters.Add("@lnbegbal", SqlDbType.Decimal).Value = drow["begbal"];
                    insCommand.InsertCommand.Parameters.Add("@lnendbal", SqlDbType.Decimal).Value = drow["namount"];
                    insCommand.InsertCommand.Parameters.Add("@lncumint", SqlDbType.Decimal).Value = drow["cumInt"];
                    insCommand.InsertCommand.Parameters.Add("@lloanid", SqlDbType.Int).Value = gnLoanID; ;
                    insCommand.InsertCommand.Parameters.Add("@lnorder", SqlDbType.Int).Value = drow["dperiod"];
                    insCommand.InsertCommand.Parameters.Add("@lnbalance", SqlDbType.Decimal).Value = drow["npayment"];

                    insCommand.InsertCommand.ExecuteNonQuery();
                    insCommand.InsertCommand.Parameters.Clear();
                }
//                MessageBox.Show("Amortization successful for " + gnLoanID);
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
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
            //Convert.ToInt32(comboBox2.SelectedValue) > 0 &&
            glCashOk = (radioButton1.Checked && Convert.ToDecimal(textBox2.Text) >= 0 ? true : false);

           glCheqOk = radioButton2.Checked && Convert.ToInt32(comboBox1.SelectedValue) > 0 && chkdate.Value.ToString() != "" && textBox10.Text.ToString().Trim()!="" && 
               comboBox3.SelectedIndex>-1 ? true : false;// && Convert.ToInt16(comboBox3.SelectedValue.ToString()) > 0 ? true : false); 
                //&&             chkdate.Text != "" && textBox10.Text != "" ? true : false);

        //    glCheqOk = true;

            glMobiOk = (radioButton3.Checked && textBox1.Text != "" && textBox5.Text != "");

            if (gcMembCode != ""  && (glCashOk || glCheqOk || glMobiOk)) 

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

        private void getBnkAccts(int bankid)
        {
            string dsql1 = "exec tsp_bnk_accounts  " + bankid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(acctview);
                if (acctview.Rows.Count > 0)
                {
                    comboBox3.DataSource = acctview.DefaultView;
                    comboBox3.DisplayMember = "cacctname";
                    comboBox3.ValueMember = "bnkacct";
                    comboBox3.SelectedIndex = -1;
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            saveButton.ForeColor = Color.Black;
            MessageBox.Show("Biometric authentication will be required for this process");
            gdTransactionDate = Convert.ToDateTime(dTranDate.Value);
            int doption = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
            bool llPaid = true;
            decimal tranamt = !glTopUp ? Convert.ToDecimal(textBox2.Text) : Convert.ToDecimal(textBox9.Text);


            gcControlAcct = getProductControl.productControl(cs, gnLoanProduct);


            //MessageBox.Show("Biometric authentication will be required for this process");
            //int doption = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
            // decimal tranamt = !glTopUp ? Convert.ToDecimal(textBox2.Text) : Convert.ToDecimal(textBox9.Text);
            // getloancontrol(gnLoanProduct, out gcControlAcct);
            switch (doption)
            {
                case 1:             //cash payment, posting account = member account, contra account is cashier cash account
                    postAccounts(gcAcctNumb, tranamt,globalvar.gcCashAccont);
                    break;

                case 2:             //cheque payment, posting account = member account, contract accounti is bank account
                    ccontra = Convert.ToString(textBox4.Text.Trim());
                   // MessageBox.Show("This is the contra account" + ccontra);
                    postAccounts(gcAcctNumb, tranamt, ccontra);
                    break;
                case 3:             //Mobile wallet payment, posting account = member account, contract account is mobile wallet control account
                    MessageBox.Show("loan account is " + gcAcctNumb + " but we are waiting for the mobile wallet GL account " );
                    break;
            }
           
            updateloan();

            if (textBox4.Text.Trim() != null && textBox4.Text.Trim() != string.Empty)
            {
                updateTemporaryCashBalances(textBox4.Text.Trim(), -tranamt);
                cashAccount(textBox4.Text.Trim());
            }
         //   if (checkBox2.Checked && textBox14.Text != "" && textBox15.Text != "")
           if (textBox14.Text != "" && textBox15.Text != "")
            {
                postFee();
                //   sms();
            }

            initvariables();         
        }

        private void cashAccount(string actnumb)
        {
            using (SqlConnection nConnHandle = new SqlConnection(cs))
            {
                string cashap = "select cacctname,nbookbal from Daytrans where cacctnumb =" + "'" + actnumb + "'";
                SqlDataAdapter da21 = new SqlDataAdapter(cashap, nConnHandle);
                nConnHandle.Open();
                DataTable ds1 = new DataTable();
                da21.Fill(ds1);
                if (ds1 != null & ds1.Rows.Count > 0)
                {
                    globalvar.gcCashAccontName = ds1.Rows[0]["cacctname"].ToString();
                    globalvar.gcCashAccontBal = Convert.ToDecimal(ds1.Rows[0]["nbookbal"]);
                    globalvar.gnCashierBalance = Convert.ToDecimal(ds1.Rows[0]["nbookbal"]);
                    globalvar.gcToolbutton = ds1.Rows[0]["nbookbal"].ToString();
                    RefreshNeeded?.Invoke(this, new EventArgs());
                }
            }
        }

        private void updateTemporaryCashBalances(string actnumb, decimal tnTransAmount)
        {
            using (SqlConnection ncon = new SqlConnection(cs))
            {

                string cashap = "update Daytrans set nbookbal = @nAmount where cacctnumb = @cAcct";
                SqlDataAdapter cashUpd = new SqlDataAdapter();
                cashUpd.UpdateCommand = new SqlCommand(cashap, ncon);
                cashUpd.UpdateCommand.Parameters.Add("@nAmount", SqlDbType.Decimal).Value = tnTransAmount + globalvar.gcCashAccontBal;
                cashUpd.UpdateCommand.Parameters.Add("@cAcct", SqlDbType.VarChar).Value = actnumb;
                ncon.Open();
                cashUpd.UpdateCommand.ExecuteNonQuery();
            }
        }
        private void printreceipt(string tcCode, string tcReceiptNumber, int tnOrder)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                dds.CashReceiptDataTable.Clear();
                string lcReportFolder = getStartupFolder.gcStartUpDirectory;
                string tcTranCode = "06";
                string lcReportFile = "CashReceipt.rpt";
                DateTime ldReceiptDate = DateTime.Today;
                string cpatquery = "Exec tsp_ClientReceipt @tnCompid,@tcCustCode,@tcReceiptNo,@tnrecType";
                int lnRecType = tcTranCode == "06" ? 2 : 1;

                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                SqlDataAdapter cashSel = new SqlDataAdapter();

                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@tcCustCode", SqlDbType.Char).Value = tcCode;
                glinsCommand.SelectCommand.Parameters.Add("@tcReceiptNo", SqlDbType.Char).Value = tcReceiptNumber;
                glinsCommand.SelectCommand.Parameters.Add("@tnrecType", SqlDbType.Int).Value = lnRecType;


                int rcount = 0;
                //  int ccount = 0;
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(dds.CashReceiptDataTable);
                rcount = dds.CashReceiptDataTable.Rows.Count;


                if (rcount > 0)
                {
                    
                    ReportDocument CashReceiptDoc = new ReportDocument();
                    bool dRepExists = getReportFile.dReportfile(lcReportFolder, lcReportFile);
                    if (dRepExists)
                    {
                        string lcCashier = globalvar.gcUserName;
                        string lcTranDesc = string.Empty;
                        decimal tnTranAmt = 0.00m;
                        decimal tnWaiveAmt = 0.00m;
                        decimal tnBillTotal = 0.00m;
                        string lcReceiptNo = dds.CashReceiptDataTable.Rows[0]["autorecit"].ToString().Trim();
                        string lcFullname = dds.CashReceiptDataTable.Rows[0]["clientname"].ToString().Trim();
                        string lcCustCode = dds.CashReceiptDataTable.Rows[0]["ccustcode"].ToString().Trim();
                        CashReceiptDoc.Load(lcReportFile);
                        CashReceiptDoc.SetDataSource(dds);
                        CashReceiptDoc.Refresh();
                        CashReceiptDoc.SetParameterValue(0, "something here");
                        CashReceiptDoc.SetParameterValue("ReceiptNumber", lcReceiptNo);
                        CashReceiptDoc.SetParameterValue("ClientName", lcFullname);
                        CashReceiptDoc.SetParameterValue("ClientCode", lcCustCode);
                        CashReceiptDoc.SetParameterValue("CashierCode", lcCashier);
                        CashReceiptDoc.SetParameterValue("nWaiveAmt", tnWaiveAmt);
                        CashReceiptDoc.SetParameterValue("receiptDate", ldReceiptDate);
                        try
                        {
                            foreach (DataRow dr in dds.CashReceiptDataTable.Rows)
                            {
                                tnTranAmt = tnTranAmt + Convert.ToDecimal(dr["ntranamnt"]);
                            }
                            tnTranAmt = tnTranAmt - tnWaiveAmt;
                            CashReceiptDoc.SetParameterValue("nTranTotal", tnTranAmt);
                            tnBillTotal = tnTranAmt + tnWaiveAmt;
                            CashReceiptDoc.SetParameterValue("nBillTotal", tnBillTotal);
                          //  cashviewer.ReportSource = CashReceiptDoc;
                           // cashviewer.Zoom(1);
                           // cashviewer.Refresh();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        PrinterSettings getprinterName = new PrinterSettings();
                        CashReceiptDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                        CashReceiptDoc.PrintToPrinter(1, false, 0, 1);

                    }
                    else { MessageBox.Show("Report file " + lcReportFile + " could not be found , inform IT DEPT "); }
                }
                else { MessageBox.Show("No receipt details found for client ID = " + tcCode + " and  receipt number = " + tcReceiptNumber); }
                dcon.Close();
            }
        } //end of printreceipt

        private void postAccounts(string tcAcctNumb, decimal lnTranAmt, string ccontra)
        {
            string tcContra = ccontra;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = "Loan Disbursement";
            int tncompid = globalvar.gnCompid;
            string tcCustcode = gcMembCode;
            decimal tnTranAmt = -Math.Abs(lnTranAmt);
            decimal tnContAmt = Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = (radioButton1.Checked ? "000001" : (radioButton2.Checked ? textBox10.Text : textBox1.Text));
            int npaytype = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "06";
            int lnServID = gnLoanProduct;
            bool llPaid = true;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            bool lFreeBee = false;

            gcReceiptNo = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
          //  gcReceiptNo = ldToday.Year.ToString().Trim().PadLeft(2, '0') + ldToday.Month.ToString().Trim().PadLeft(2, '0') + GetClient_Code.clientCode_int(cs, "rec_no").ToString().Trim().PadLeft(4, '0');
            //    textBox15.Text = gcReceiptNo;
            string lcStack = GetClient_Code.clientCode_int(cs, "stackcount").ToString().Trim().PadLeft(15, '0');
            updateGlmast gls = new updateGlmast();
            //  updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            updateJournal upj = new updateJournal();

            //gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
            //                                                                       //            dbal.updDayBal(cs, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
            //decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            //tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
            //srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
            //                                                                                                                                                       // updateJournal(gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode);
            //upj.updJournal(cs, gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            //string auditDesc = "Loan Disbursement Completed -> " + gcMembCode;
            //AuditTrail au = new AuditTrail();
            //au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


            ////     gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
            ////            dbal.updDayBal(cs, tcContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
            ////  updateJournal(tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode);
            //upj.updJournal(cs, tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            ////    tnContAmt
            //decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
            //                                                                       // tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            //                                                                       // srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                        //update tranhist account 396 1756

           // principalPayment(tcAcctNumb, globalvar.gcCashAccont, tnTranAmt,tcDesc, lnServID, tcVoucher, gcReceiptNo, lcStack); //tcContra
            principalPayment(tcAcctNumb, tcContra, tnTranAmt, tcDesc, lnServID, tcVoucher, gcReceiptNo, lcStack); //tcContra
          //  MessageBox.Show("This is Loan Status 1 " + gnLoanStatus);
            if (gnLoanStatus != 3)
            {
           // MessageBox.Show("This is Loan Status 2 " + gnLoanStatus);
                updateInterestDate(tcAcctNumb);
            }
            updateClient_Code updcl = new updateClient_Code();
            updcl.updClient(cs, "nvoucherno");

            if (checkBox1.Checked)
            {
                printreceipt(tcCustcode, gcReceiptNo, 1);
                Reprint re = new Reprint(tcCustcode, gcReceiptNo, 1);
                re.ShowDialog();
            }
            else
            {
                //  textBox17.Text = "";

            };
        }

        private void principalPayment(string tcAcct, string tcCont, decimal tnTran,string tcdes, int lnServ, string tcVouch, string gcReceipt, string Stack)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
              decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcct);      //  0.00m;
              //  decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcCont);        // 0.00m;
                decimal tnCNewBal1 = CheckLastBalance.lastbalance(cs, tcCont);        // 0.00m;
                decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, gcControlAcct);        // 0.00m;
                                                                                      // gcReceiptNo = ldToday.Year.ToString().Trim().PadLeft(2, '0') + ldToday.Month.ToString().Trim().PadLeft(2, '0') + GetClient_Code.clientCode_int(cs, "rec_no").ToString().Trim().PadLeft(4, '0');
                tcCustcode = tcAcct.Substring(3, 6);
                tcContcode = tcCont.Substring(3, 6);
                tcTranCode = "06";
                int jourtype = 8;
                //(tcTranCode == "01" ? 2 :  //Deposits
                //            (tcTranCode == "02" ? 3 : //Withdrawals
                //            (tcTranCode == "07" ? 4 : //Loan Repayment
                //            (tcTranCode == "09" ? 5 : //"<< Loan Payout/close >>" :
                //            (tcTranCode == "22" ? 6 :
                //            (tcTranCode == "26" ? 7 : 99))))));// "<< Loan Charge Off >>" : "")))));

                SqlDataAdapter payCommand = new SqlDataAdapter();

                string cpatquery = "Exec Tsp_InsertLoanIssued ";
                cpatquery += "@tnCompid, @tcAcctNumb, @tcContra, @tcControlAcct, @tnTranAmt, @tcDesc, @dtrandate, @tcTranCode, @dvaludate,";
                cpatquery += "@tcVoucher, @tcReceipt, @tcChqno, @tcUserID, @tnPNewBal, @tnCNewBal, @lcStack, @lVerified, @lcCustcode,";
                cpatquery += "@lcContcode, @lbranchid, @lcurrcode, @lnLoanProcess, @llcjvno, @llcBank, @jtype,@gcWkStation,@gcWinUser,@serv_id, @tnPNewBal1,@tnCNewBal1 ";  //

                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                payCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcct;
                payCommand.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = tcCont;
                payCommand.InsertCommand.Parameters.Add("@tcControlAcct", SqlDbType.Char).Value = gcControlAcct;
                payCommand.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnTran;
                payCommand.InsertCommand.Parameters.Add("@tnContraAmt", SqlDbType.Decimal).Value = -tnTran;
                payCommand.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.VarChar).Value = tcdes.Trim();
                payCommand.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                payCommand.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVouch;
                payCommand.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = gcReceipt;
                payCommand.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = gcUserid;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal", SqlDbType.Decimal).Value = tnPNewBal + tnTran;
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal", SqlDbType.Decimal).Value = tnCNewBal + tnContAmt;
                payCommand.InsertCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = Stack;
                payCommand.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                payCommand.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = tcCustcode;
                payCommand.InsertCommand.Parameters.Add("@lcContcode", SqlDbType.Char).Value = tcContcode;
                payCommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = globalvar.gnBranchid;
                payCommand.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                payCommand.InsertCommand.Parameters.Add("@lnLoanProcess", SqlDbType.Bit).Value = glLoanProcess;
                payCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                payCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                payCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;
                payCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.VarChar).Value = globalvar.gcWorkStation.Trim();
                payCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.VarChar).Value = globalvar.gcWinUser.Trim();
                payCommand.InsertCommand.Parameters.Add("@serv_id", SqlDbType.Decimal).Value = lnServ;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal1", SqlDbType.Decimal).Value = tnCNewBal2 + tnTran;
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal1", SqlDbType.Decimal).Value = tnCNewBal + -tnTran;
                payCommand.InsertCommand.ExecuteNonQuery();
                dcon.Close();

            }
          //  updateAmort();  //we need to update the amortabl so that it does not become part of the outstanding payments
        }

        private void updateInterestDate(string tcAcctNumb)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                //  MessageBox.Show("you are indise the code" + gdTransactionDate);
                nConnHandle2.Open();
                string cupdatequry1 = "update glmast set lintdate = @dTransactionDate where cacctnumb = @acctNumb";
                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                updCommand1.UpdateCommand = new SqlCommand(cupdatequry1, nConnHandle2);
                updCommand1.UpdateCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(gdTransactionDate);
                updCommand1.UpdateCommand.Parameters.Add("@acctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                updCommand1.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                int bnkid = Convert.ToInt32(comboBox1.SelectedValue);
                getBnkAccts(bnkid);
                AllClear2Go();
            }
        }

        private void updateJournal(string actnumb, string desc, decimal tamt, string tcvou, string trancode)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string tcTranCode = "06";
                string cglquery = "Insert Into journal (cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,jour_TYPE)";
                cglquery += " VALUES  (@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@jtype)";
                nConnHandle2.Open();
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
                insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
                insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = globalvar.gdSysDate;
                insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = desc;
                insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = actnumb;
                insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = tamt;
                insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = gcDjv;
                insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
                insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = tcTranCode;
                insCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = 7;

                insCommand.InsertCommand.ExecuteNonQuery();
                insCommand.InsertCommand.Parameters.Clear();
                nConnHandle2.Close();
                updateClientCode();
            }
        }

        private void updateClientCode()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                nConnHandle2.Open();
                string cupdatequery1 = "update client_code set  stackcount=stackcount+1";
                string cupdatequery2 = "update client_code set  nvoucherno=nvoucherno+1";

                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                SqlDataAdapter updCommand2 = new SqlDataAdapter();

                updCommand1.UpdateCommand = new SqlCommand(cupdatequery1, nConnHandle2);
                updCommand2.UpdateCommand = new SqlCommand(cupdatequery2, nConnHandle2);

                updCommand1.UpdateCommand.ExecuteNonQuery();
                updCommand2.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }
        private void updateloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "update loan_det set lissued = 1  where loan_id=@loanid";
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnLoanID;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked)
            {
//                MessageBox.Show("The buttonm is checked");
                comboBox1.Visible = true ;
//                comboBox2.Visible = true ;
                comboBox3.Visible = true ;
                comboBox1.Enabled = true;
  //              comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                chkdate.Enabled = true;
                textBox10.Enabled = true;
                textBox1.Visible = false;
                mobTransDate.Enabled = false;
//                mobTransDate.Visible = false;
                textBox5.Visible = false;
                label15.Text = "Bank Name";
//                label6.Text = "Bank Branch";
                label3.Text = "Bank Account";
                comboBox1.Focus();
                AllClear2Go();
            }
            else
            {
                comboBox1.Enabled = false;
//                comboBox2.Enabled = false ;
                comboBox3.Enabled = false ;
                chkdate.Enabled = false ;
                textBox10.Enabled = false ;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1.Visible  = true;
                mobTransDate.Enabled = true; 
                textBox5.Visible  = true;
                comboBox1.Visible = false;
//                comboBox2.Visible = false;
                comboBox3.Visible = false;
                chkdate.Enabled = false;
                textBox10.Enabled = false;
                label15.Text = "Transaction ID";
                label6.Text = "Transaction Date";
                label3.Text = "Transaction Time";
                textBox1.Focus();
                AllClear2Go();
            }
            else
            {
                mobTransDate.Enabled = false;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                comboBox1.Visible = true;
                comboBox3.Visible = true;
                comboBox1.Enabled = false;
                comboBox3.Enabled = false;
                comboBox1.SelectedIndex = comboBox3.SelectedIndex = -1;

                chkdate.Enabled = false;
                textBox10.Enabled = false;
                textBox1.Visible = false;
                mobTransDate.Enabled = false;
                textBox5.Visible = false;
                label15.Text = "Bank Name";
                label3.Text = "Bank Account";
                AllClear2Go();
            } 
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(Focused)
            //{
            //    textBox4.Text = comboBox3.SelectedValue.ToString();
            //    textBox8.Text = comboBox3.Text.ToString();
            //    AllClear2Go();
            //}
        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox4_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox3_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private string getloancontrol(int lprid, out string dacct)
        {
            string dasql = "select  prod_control,prd_name from  prodtype where prd_id = " + lprid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["prod_control"].ToString();// Convert.ToInt32(loanview.Rows[0]["loan_id"]);
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }
        private void initvariables()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox6.Text = "";
            textBox10.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            textBox16.Text = "";
            textBox18.Text = "";
            textBox21.Text = "";
            textBox9.Text = "";
            textBox3.Text = textBox4.Text = textBox7.Text = textBox8.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            clientview.Clear();
            loanview.Clear();
            getclientList();
            string djv = CuVoucher.genJv(cs, globalvar.gdSysDate);
            gcDjv = "<" + djv.Substring(0, 2) + "-" + djv.Substring(2, 2) + "-" + djv.Substring(4, 2) + ">" + djv.Substring(6, 4);
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

                  gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["ccustcode"].Value);
                  int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
                  gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loan_id"].Value);
                 gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["principal_amt"].Value);
                 getLoanDetails(gcMembCode, gnLoanID);
                 checktopup(gnLoanID);

                // gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["dcuscode"].Value);
                // int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
                // gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loanid"].Value);
                //   gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                //   gcMemberName = clientgrid.CurrentRow.Cells["tcMembName"].Value.ToString().Trim();
                // string lcLoanAcct = clientgrid.CurrentRow.Cells["loanAcct"].Value.ToString().Trim() == "" ? getNewAcct.newAcctNumber(cs, gcMembCode, "130") : clientgrid.CurrentRow.Cells["loanAcct"].Value.ToString().Trim();// clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"] != null ? clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString() : "";
                //textBox7.Text = lcLoanAcct;// Convert.ToInt16(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_status"]) == 1 ? getNewAcct.newAcctNumber(cs, gcMembCode, "130") : clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanacct"].ToString();
               // getLoanDetails(gcMembCode, gnLoanID);
               // checktopup(gnLoanID);
            }

        }
        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          //  if(clientview.Rows.Count>0)
          //  {
              //  gcMembCode = Convert.ToString(clientgrid.CurrentRow.Cells["ccustcode"].Value);
              //  int drowindex = Convert.ToInt16(clientgrid.CurrentCell.RowIndex);
              //  gnLoanID = Convert.ToInt32(clientgrid.CurrentRow.Cells["loan_id"].Value);
               // gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["principal_amt"].Value);
               // getLoanDetails(gcMembCode, gnLoanID);
               // checktopup(gnLoanID);


                //gcMembCode = Convert.ToString(clientview.Rows[e.RowIndex]["ccustcode"]);
                //gnLoanID = Convert.ToInt32(clientview.Rows[e.RowIndex]["loan_id"]);
                //gnLoanAmt = Convert.ToInt32(clientview.Rows[e.RowIndex]["principal_amt"]);
              //  getLoanDetails(gcMembCode,gnLoanID);
               // checktopup(gnLoanID);
          //  }
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

                    //loanview.Clear();
                    //string dsql = "exec  tsp_checkLoansTopUp  " + ncompid + "," + loanid;

                    //using (SqlConnection ndConnHandle = new SqlConnection(cs))
                    //{
                    //    ndConnHandle.Open();
                    //    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    //    DataTable topview = new DataTable();
                    //    da.Fill(topview);
                    //    if (topview != null && topview.Rows.Count > 0)
                    //    {
                    //        glTopUp = Convert.ToInt16(topview.Rows[0]["loan_status"]) == 3 ? true : false;
                    //        glResched = Convert.ToInt16(topview.Rows[0]["loan_status"]) == 4 ? true : false;
                    //    }
                    //}

                    glTopUp = Convert.ToInt16(topview.Rows[0]["loan_status"]) == 3 ? true : false;
                    glResched = Convert.ToInt16(topview.Rows[0]["loan_status"]) == 4 ? true : false;
                    decimal nNewAmt = Math.Abs(Convert.ToDecimal(topview.Rows[0]["principal_amt"]));
                    decimal nOldAmt = Math.Abs(Convert.ToDecimal(topview.Rows[0]["loanbalance"]));

                    textBox9.Text = nNewAmt > nOldAmt ? (nNewAmt - nOldAmt).ToString("N2") : "";// 0.00 (Math.Abs(Convert.ToDecimal(topview.Rows[0]["principal_amt"])) - Math.Abs(Convert.ToDecimal(topview.Rows[0]["loanbalance"]))).ToString("N2");
                }
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox3.Focused)
            {
                textBox4.Text = comboBox3.SelectedValue.ToString();
                textBox8.Text = comboBox3.Text.ToString();
                AllClear2Go();
            }
        }
        // SMS 
        private void sms()
        {
            //  MessageBox.Show("You are inside Ok 1");
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            string tcDesc = "Loan Disbursement";
            decimal balance = tnPNewBal + Convert.ToDecimal(textBox2.Text);
            //  MessageBox.Show("This is the Balabce " + balance);
            string messages = "" + tcDesc.Trim() + ": GMD" + textBox2.Text.Trim() + " " + textBox5.Text.Trim() + " to your account at " + globalvar.gcComname + " on the " + dTranDate.Text.Trim() + ". Your new Balance is GMD" + balance + "";
            string PhoneNumber = textBox17.Text.Trim();
            var client = new RestClient("https://alchemytelco.com:9443/api?action=sendmessage&originator=NACCUG&username=naccug&password=n@ccu9&recipient=PhoneNumber&messagetype=SMS:TEXT&messagedata=messages");
            client.Timeout = -1;
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddParameter("messagedata", messages);
            requestPost.AddParameter("recipient", PhoneNumber);
            IRestResponse response = client.Execute(requestPost);
            string rawResponse = response.Content.Trim();
            int nStatusCode = (int)response.StatusCode;
            //   MessageBox.Show("This is STatus Code " + nStatusCode);
            string lcStatusCode = response.StatusDescription.ToString().Trim();
            //  string lcStatusDesc = errorMessages(nStatusCode.ToString().Trim().PadLeft(2, '0'));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                postFee();
            }
              // 

        }

        //
        // Fee payment
        private void postFee()
        {
            // string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcDesc = string.Empty;
            string tcCustcode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
            decimal tnTranAmt = -Math.Abs(Convert.ToDecimal(textBox14.Text));
            decimal tnContAmt = Math.Abs(Convert.ToDecimal(textBox14.Text));
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            //decimal unitprice = Math.Abs(nTranAmt);
            string tcChqno = "000001";
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "15";
            int lnServID = 0;
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 5;
            bool lFreeBee = false;
            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            updateClient_Code updcl = new updateClient_Code();

            //************************************The posting should follow the processes detailed below
             tcDesc = "Fee Paid";
            string auditDesc = string.Empty;
            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                                                                       //update member savings account glmast 
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);
            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, tnTranAmt, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account

            //savings product control account and send for verification - debit 
            auditDesc = "Fee Paid-> " + gcControlAcct;
            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
            //    upj.updJournal(cs, gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            upj.updJournal(cs, gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, tncompid);
            updcl.updClient(cs, "nvoucherno");

            // joining fee defined in company details  - credit
            //                MessageBox.Show("The joining fee account = " + gcJoinFeeAcct);
            auditDesc = "Fee Paid-> " + gcFeeAccount;
            au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(tnTranAmt), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
            upj.updJournal(cs, gcFeeAccount, tcDesc, Math.Abs(tnTranAmt), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, tncompid);
            updcl.updClient(cs, "nvoucherno");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                DataRow dr = feeView.Rows[comboBox2.SelectedIndex];
                //   textBox22.Text = feeView.Rows[comboBox5.SelectedIndex]["feeamount"].ToString();
                textBox14.Text = Convert.ToDecimal(feeView.Rows[comboBox2.SelectedIndex]["feeamount"]).ToString("N2");
                gcFeeAccount = feeView.Rows[comboBox2.SelectedIndex]["feeIncome"].ToString();
                textBox15.Text = gcFeeAccount;
            }
        }

        private void label113_Click(object sender, EventArgs e)
        {

        }
    }
}
