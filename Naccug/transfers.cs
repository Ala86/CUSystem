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
using RestSharp;
using System.Net;
using System.Transactions;

namespace WinTcare
{
    public partial class transfers : Form
    {
        DataTable postview = new DataTable();
        DataTable contview = new DataTable();
        DataTable feeView = new DataTable();
        int ncompid = globalvar.gnCompid;
        string cs = globalvar.cos;
        string cloca = globalvar.cLocalCaption;
        decimal gnPayAmt = 0.00m;
        decimal gnPostBookbal = 0.00m;
        decimal gnTotalAccruedInterest = 0.00m;
        decimal gnInterestBalance = 0.00m;

        int gnIntScope = 0;
        int trntype = 0;
        int tnPrd_ID = 0;
        int tnPrd_IDs = 0;

        string gcFeeAccount = string.Empty;
        string gcPostAcct = string.Empty;
        string gcContAcct = string.Empty;

        string gcPostControlAcct = string.Empty;
        string gcContControlAcct = string.Empty;  //gcContInterestAcct
        string gcContInterestAcct = string.Empty;

        updateClient_Code updcl2 = new updateClient_Code();

        public transfers(int transtype)
        {
            InitializeComponent();
            trntype = transtype;
        }

        private void transfers_Load(object sender, EventArgs e)
        {
            this.Text = cloca + (trntype ==1 ?  " << Account Transfers  >>" : "<< Member Transfer >>");
            textBox1.Text = trntype == 1 ? "Account Transfer" : " Member Transfer ";
            PostAcct();
            loanGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            loanGrid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            loanGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            loanGrid.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            loanGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            loanGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            getFee();
  //          loanGrid.Visible = (gcTranCode == "03" || gcTranCode == "07" ? true : false);
        }


        private void PostAcct()
        {
            postview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                    string dsqlb = " exec tsp_MemberAccounts " + ncompid;
                    SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                    dab.Fill(postview);

                if (postview.Rows.Count > 0)
                {
                    comboBox1.DataSource = postview.DefaultView;
                    comboBox1.DisplayMember = "acctname";
                    comboBox1.ValueMember = "cacctnumb";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        private void ContAcct(string tcacct,string  tcClientCode)
        {
            contview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                if (trntype == 1)   //Account transfer
                {
                    string dsqlb1 = " exec tsp_MemberContAccounts " + ncompid + ",'" + tcacct+"','"+ tcClientCode + "'";
                    SqlDataAdapter dab1 = new SqlDataAdapter(dsqlb1, ndConnHandle);
                    dab1.Fill(contview);
                }
                else               //Loan offset
                {
                    string dsqlb1 = "exec tsp_MemberContAccounts_all " + ncompid + ",'" + tcClientCode + "'";// + tcClientCode + "'"; 
                    SqlDataAdapter dab1 = new SqlDataAdapter(dsqlb1, ndConnHandle);
                    dab1.Fill(contview);
                }
                if (contview.Rows.Count > 0)
                {
                    
                    comboBox2.DataSource = contview.DefaultView;
                    comboBox2.DisplayMember = "acctname";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                   
                }
                else { MessageBox.Show("no contra accounts found "); }
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
            if (textBox2.Text != "" && textBox5.Text != "" && textBox6.Text != "" && maskedTextBox1.Text != "")
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
        private void button3_Click(object sender, EventArgs e)
        {
            acctEnquiry ae = new acctEnquiry(cs,ncompid,cloca);
            ae.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                textBox2.Text = comboBox1.SelectedValue.ToString();
                string act = comboBox1.SelectedValue.ToString();
                gcPostAcct = act;
                string tcCustcode = act.Substring(3, 6);
                gnPostBookbal = Convert.ToDecimal(postview.Rows[Convert.ToInt32(comboBox1.SelectedIndex)]["nbookbal"]);
                textBox3.Text = gnPostBookbal.ToString("N2");
                textBox8.Text = postview.Rows[comboBox1.SelectedIndex]["ctel"].ToString();
                maskedTextBox1.Enabled = gnPostBookbal > 0.00m ? true : false;
                 tnPrd_IDs = Convert.ToInt16(postview.Rows[Convert.ToInt32(comboBox1.SelectedIndex)]["prd_id"]);
                gcPostControlAcct = getProductControl.productControl(cs, tnPrd_IDs);
             //   MessageBox.Show("customer code " + tcCustcode);
                ContAcct(act, tcCustcode);//  act);
            }
        }

        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            gnPayAmt = Convert.ToDecimal(maskedTextBox1.Text);
            if(gnPayAmt <= gnPostBookbal)
            {
                string cpayment = maskedTextBox1.Text;
                if (cpayment.Replace(",", "").Replace(".", "").Trim() != "")         //masked text box was not empty, meaning values were keyed in
                {
                    if (cpayment.Substring(cpayment.Length - 3, 1) != ".")  //if no decimal are entered, we will conca.. .00 to the endi 
                    {
                        cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim() + ".00";
                        maskedTextBox1.Text = cpayment.PadLeft(12, ' ');
                    }
                    else
                    {
                        cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim();
                    }
                    string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');     // .Trim(); 
                    string cwords = daltext.inwords(fintext);
                    maskedTextBox1.Text = Convert.ToDecimal(fintext).ToString("N2");
                    textBox4.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim().ToUpper()).Replace("UNIT", globalvar.gcCurrUnit.Trim().ToUpper());        // + globalvar.gcCurrName;//  daltext.inwords(fintext);
                }
            }
            else
            {
                maskedTextBox1.Text = "";
                maskedTextBox1.Focus();
            }
            AllClear2Go();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                textBox6.Text = comboBox2.SelectedValue.ToString();
                gcContAcct = comboBox2.SelectedValue.ToString();
                string lccode = comboBox2.SelectedValue.ToString().Trim().Substring(0, 3); 
                textBox7.Text = Math.Abs(Convert.ToDecimal(contview.Rows[Convert.ToInt32(comboBox2.SelectedIndex)]["nbookbal"])).ToString("N2");

                tnPrd_ID = Convert.ToInt16(contview.Rows[Convert.ToInt32(comboBox2.SelectedIndex)]["prd_id"]);
                gcContControlAcct = getProductControl.productControl(cs, tnPrd_ID);

                gcContInterestAcct = getProductControl.interestControl(cs, tnPrd_ID);

            //    MessageBox.Show("This the interest Account" + gcContInterestAcct);

                AllClear2Go();
                if (lccode=="130" || lccode == "131")
                {
                    getAccruedInterest(gcContAcct);
                    //gcCustCode = custcode;
           //         acctdetails(gcCustCode, rown1);

                }
                /*
                 * 
                                     getAccruedInterest(dAcctNumb);
                        gcCustCode = custcode;
                        acctdetails(gcCustCode, rown1);
    
             */
            }
        }


        private void getAccruedInterest(string tcAcctNumb)
        {
            string dasql = "exec tsp_MemberLoanAccounts_A " + ncompid + " ,'" + tcAcctNumb + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    gnIntScope = Convert.ToInt16(dprodview.Rows[0]["int_scope"]);
                    DateTime dtrandate = Convert.ToDateTime(dTranDate.Value);

                    DateTime lastInterestDate = Convert.ToDateTime(dprodview.Rows[0]["lintdate"]);
                    decimal nloanBalance = Convert.ToDecimal(dprodview.Rows[0]["nbookbal"]);
                    if (gnIntScope == 3)
                    {
                        decimal nAccruedInterest = Convert.ToDecimal(dprodview.Rows[0]["accruedinterest"]); //accrued interest from glmast 
                        decimal ndur = Convert.ToDecimal(dprodview.Rows[0]["ndur"]);
                        decimal nodays = ndur * 30.4375m;
                        decimal pv = Convert.ToDecimal(dprodview.Rows[0]["loanamt"]);
                        decimal rate = Convert.ToDecimal(dprodview.Rows[0]["loanint"]);
                        decimal intrate = (rate / pv);
                        int numberofdays = (dtrandate - lastInterestDate).Days;
                        decimal ala = intrate * (numberofdays / nodays);
                        decimal ala1 = pv * ala;
                        string cloanType = dprodview.Rows[0]["loantype"].ToString().Trim();
                        decimal accruedInterest = ala1;
                        gnTotalAccruedInterest = accruedInterest + nAccruedInterest;
                        loanGrid.AutoGenerateColumns = false;
                        loanGrid.DataSource = dprodview.DefaultView;
                        loanGrid.Columns[0].DataPropertyName = "loantype";
                        loanGrid.Rows[0].Cells[0].Value = Math.Abs(nloanBalance).ToString("N2");
                        loanGrid.Rows[0].Cells[1].Value = Math.Abs(accruedInterest).ToString("N2");
                        loanGrid.Rows[0].Cells[2].Value = Math.Abs(nAccruedInterest).ToString("N2");
                        //loanGrid.Rows[0].Cells[1].Value = Math.Abs(nloanBalance).ToString("N2");
                        //loanGrid.Rows[0].Cells[2].Value = Math.Abs(accruedInterest).ToString("N2");
                    }
                    else
                    {
                        decimal nAccruedInterest = Convert.ToDecimal(dprodview.Rows[0]["accruedinterest"]); //accrued interest from glmast 
                        decimal nloanInterest = Convert.ToDecimal(dprodview.Rows[0]["intrate"]) / (100 * 365m);
                        int numberofdays = (dtrandate - lastInterestDate).Days;
                        string cloanType = dprodview.Rows[0]["loantype"].ToString().Trim();
                        decimal calculatedInterest = nloanBalance * nloanInterest * numberofdays;  //calculated interest
                        gnTotalAccruedInterest = calculatedInterest + nAccruedInterest;                 //total accrued interest =  calculated interest + balance of accrued interest
                        textBox9.Text = Math.Abs(gnTotalAccruedInterest).ToString("N2");
                        loanGrid.AutoGenerateColumns = false;
                        loanGrid.DataSource = dprodview.DefaultView;
                        loanGrid.Columns[0].DataPropertyName = "loantype";
                        loanGrid.Rows[0].Cells[0].Value = Math.Abs(nloanBalance).ToString("N2");
                        loanGrid.Rows[0].Cells[1].Value = Math.Abs(calculatedInterest).ToString("N2");
                        loanGrid.Rows[0].Cells[2].Value = Math.Abs(nAccruedInterest).ToString("N2");
                    }

                }
                else { MessageBox.Show("No loan details found for selected loan account, inform IT Dept immediately"); }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
         //   using (TransactionScope scope = new TransactionScope())
         //   {
                decimal tnTranAmt = Convert.ToDecimal(maskedTextBox1.Text);
                postAccounts(gcPostAcct, tnTranAmt, gcContAcct);

                //   if (checkBox2.Checked && textBox22.Text != "" && textBox23.Text != "")
                if (checkBox2.Checked && textBox22.Text != "" && textBox23.Text != "")
                {
                    postFee();
                    //   sms();
                }
                else
                {
                    MessageBox.Show("SMS or the fee is not selected ");
                }
             //   scope.Complete();
         //   }
            initvariables();
        }

        // Fee
      
        // Transfer

        private void sms()
        {
            //  MessageBox.Show("You are inside Ok 1");
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, gcPostAcct);      //  0.00m;
                                                                                   // string tcDesc = "Loan Disbursement";
            decimal balance = tnPNewBal - Convert.ToDecimal(maskedTextBox1.Text);


          //  double balance = Convert.ToDouble(textBox3.Text) - Convert.ToDouble(maskedTextBox1.Text);
          //  MessageBox.Show("This is the Balabce " + balance);
            string messages = "" + textBox5.Text.Trim() + ": GMD" + maskedTextBox1.Text.Trim() + " " + textBox5.Text.Trim() + " from your account at " + globalvar.gcComname + " on the " + dTranDate.Text.Trim() + ". Your new Balance is GMD" + balance + "";
            string PhoneNumber = textBox8.Text.Trim();
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

        }

        private void postAccounts(string tcAcctNumb, decimal lnTranAmt, string ccontra)
        {
            string tcContra = ccontra;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = textBox5.Text.Trim(); 
            int tncompid = globalvar.gnCompid;
            string contratype = ccontra.Substring(0, 3);
            string contratype1 = tcAcctNumb.Substring(0, 3);
            string tcPostCustcode = tcAcctNumb.Substring(3, 6);
            string tcContCustcode = ccontra.Substring(3, 6);
         //   decimal tnTranAmt = -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));
            decimal tnContAmt = Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = "000001";
            string auditDesc = string.Empty;
            //            int npaytype =1;
            string tcCustcode = string.Empty;
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "12";
            int lnServID = 0;
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            bool lFreeBee = false;
            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            updateJournal upj = new updateJournal();
            AuditTrail au = new AuditTrail();
            updateClient_Code updcl = new updateClient_Code();

            if (contratype == "130" || contratype == "131")
            {
               tcTranCode = "07";
                decimal tnPayDiff = Math.Abs(Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest)); //difference between payment and total accrued interst
                decimal tnPrinAmt = tnPayDiff > 0.00m ? Math.Abs(Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest)) :// Math.Abs(Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest)) : //payment is more than total account interest, something goes to principal payment  
                tnPayDiff = 0.00m ;                                                                             //payment is equal to or less than total accrued interest, nothing goes to principal payment
                decimal tnPrinCont = -Math.Abs(tnPrinAmt);

                decimal tnAccruedPost = tnPayDiff >= 0.00m ?  -Math.Abs(gnTotalAccruedInterest) : //Total Accrued Interest is covered and Accrued Interest Balance = 0.00m 
                                        -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));        //payment is less than total accrued interest, payment will service part of the Total Accrued Interest, Accrued Interest Balance > 0.00m
                decimal tnAccruedCont = Math.Abs(gnTotalAccruedInterest);

                gnInterestBalance = tnPayDiff < 0.00m ? (gnTotalAccruedInterest - Math.Abs(Convert.ToDecimal(maskedTextBox1.Text))) : 0.00m; //Accrued Interest Balance will be used to update in glmast

                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, ccontra);      //  0.00m;
                gls.updGlmast(cs, ccontra, tnPrinAmt);                              //update glmast posting account
                tcCustcode = ccontra.Substring(3, 6);                                                                     //            dbal.updDayBal(cs, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                decimal tnPNewBa = CheckLastBalance.lastbalance(cs, ccontra);      //  0.00m;
              //  MessageBox.Show("This is the balance " + tnPNewBal);
              //  MessageBox.Show("This is the balance " + tnPrinAmt);

               
                if (Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)) > Math.Abs(gnTotalAccruedInterest))
                {
                  //  MessageBox.Show("This is for principal and interest payment ");
                    tls1.updCuTranhist(cs, ccontra, tnPrinAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal + Math.Abs(tnPrinAmt), tcTranCode, tnPrd_ID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
               srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account

                    if (tnPrinAmt>0.00m)
                {
                    if (tnPNewBal == 0.00m)
                    {
                        UpdateLoanClosed(ccontra);
                    }
                 upj.updJournal(cs, gcContControlAcct, tcDesc, tnPrinAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                }
                    //else
                    //{
                    //    UPdInterestAmtOnly();
                    //}


                    if (tnAccruedCont > 0)
                    {
                     //   MessageBox.Show("This is only intest payment ");
                        tcTranCode = "17";
                        tcDesc = "Transfer Interest Charged ";
                        gls.updGlmast(cs, gcContAcct, tnAccruedPost);                              //update accrued interest or interest charged - debit for receiving account
                        decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, gcContAcct);
                        tcCustcode = gcContAcct.Substring(3, 6);
                        tls1.updCuTranhist(cs, gcContAcct, tnAccruedPost, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal0, tcTranCode, tnPrd_ID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                        srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                        auditDesc = "Transfer Interest Charged " + gcContAcct; //Audit Trail
                        au.upd_audit_trail(cs, auditDesc, 0.00m, tnAccruedPost, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                        tcTranCode = "05";
                        tcDesc = "Transfer Interest Paid ";
                        gls.updGlmast(cs, gcContAcct, Math.Abs(gnTotalAccruedInterest));                              //update accrued interest paid  - credit
                        decimal tnPNewBal12 = CheckLastBalance.lastbalance(cs, gcContAcct);
                        tcCustcode = gcContAcct.Substring(3, 6);
                        tls1.updCuTranhist(cs, gcContAcct, Math.Abs(gnTotalAccruedInterest), tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal12, tcTranCode, tnPrd_ID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                        srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                                                                                                                                                                               //we will be updating the audit trail. 
                        auditDesc = "Transfer Interest Paid " + gcContAcct; //Audit Trail
                        au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(gnTotalAccruedInterest), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                        updateInterestDate(gcContAcct);  //update the last interest calculation date and accrued interest balance

                        // upj.updJournal(cs, gcContAcct, tcDesc, -Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                        upj.updJournal(cs, gcContInterestAcct, tcDesc, Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);

                        updcl2.updClient(cs, "nvoucherno");
                        updcl2.updClient(cs, "stackcount");

                        // savings(tcAcctNumb, lnTranAmt, ccontra);
                    }
                }
                else
                {
                    // Interest > repayment Amount

                //    MessageBox.Show("Amount less than the intest charged"); 
                    tcTranCode = "17";
                    tcDesc = "Transfer Interest Charged ";
                    gls.updGlmast(cs, gcContAcct, -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)));                              //update accrued interest or interest charged - debit for receiving account
                    decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, gcContAcct);
                    tcCustcode = gcContAcct.Substring(3, 6);
                    tls1.updCuTranhist(cs, gcContAcct, -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)), tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal0, tcTranCode, tnPrd_ID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                    srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                    auditDesc = "Transfer Interest Charged " + gcContAcct; //Audit Trail
                    au.upd_audit_trail(cs, auditDesc, 0.00m, -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                    tcTranCode = "05";
                    tcDesc = "Transfer Interest Paid ";
                    gls.updGlmast(cs, gcContAcct, Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)));                              //update accrued interest paid  - credit
                    decimal tnPNewBal12 = CheckLastBalance.lastbalance(cs, gcContAcct);
                    tcCustcode = gcContAcct.Substring(3, 6);
                    tls1.updCuTranhist(cs, gcContAcct, Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)), tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal12, tcTranCode, tnPrd_ID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                    srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                                                                                                                                                                           //we will be updating the audit trail. 
                    auditDesc = "Transfer Interest Paid " + gcContAcct; //Audit Trail
                    au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                    updateInterestDate(gcContAcct);  //update the last interest calculation date and accrued interest balance

                    // upj.updJournal(cs, gcContAcct, tcDesc, -Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                    upj.updJournal(cs, gcContInterestAcct, tcDesc, Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);

                    updcl2.updClient(cs, "nvoucherno");
                    updcl2.updClient(cs, "stackcount");

                }

                tcDesc = textBox5.Text;
                decimal tnTranAmt = Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));
                gls.updGlmast(cs, tcAcctNumb, -tnTranAmt);
                tcPostCustcode = tcAcctNumb.Substring(3, 6); //update glmast posting account
                decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                tls1.updCuTranhist(cs, tcAcctNumb, -tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal1, tcTranCode, tnPrd_IDs, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcPostCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account

                upj.updJournal(cs, gcPostControlAcct, tcDesc, -tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);  //send control account for posting account to verification

                if (contratype == "250" || contratype == "251" || contratype == "271" || contratype == "270")
                {
                    gls.updGlmast(cs, tcContra, tnContAmt);
                    tcContCustcode = tcContra.Substring(3, 6);
                    decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                    tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, tnPrd_ID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                    srvid, "", "", lFreeBee, tcContCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, globalvar.gdSysDate);                        //update tranhist account 396 1756

                    upj.updJournal(cs, gcContControlAcct, tcDesc, tnContAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid); //send control account for receiving account to verification

                    updcl.updClient(cs, "nvoucherno");
                }

               // savings(tcAcctNumb, lnTranAmt, ccontra);
            }
            else // if (contratype1 == "250" || contratype1 == "270")
            {
                decimal tnTranAmt = Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));
                gls.updGlmast(cs, tcAcctNumb, -tnTranAmt);
                tcPostCustcode = tcAcctNumb.Substring(3, 6); //update glmast posting account
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
               // MessageBox.Show("This is the second Savings " + tnPNewBal);
                tls1.updCuTranhist(cs, tcAcctNumb, -tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, tnPrd_IDs, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcPostCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

                upj.updJournal(cs, gcPostControlAcct, tcDesc, -tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);  //send control account for posting account to verification


                gls.updGlmast(cs, tcContra, tnContAmt);
                tcContCustcode = tcContra.Substring(3, 6);
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, tnPrd_ID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcContCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, globalvar.gdSysDate);                        //update tranhist account 396 1756

                upj.updJournal(cs, gcContControlAcct, tcDesc, tnContAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid); //send control account for receiving account to verification

                updcl.updClient(cs, "nvoucherno");
            }
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
                    comboBox5.DataSource = feeView.DefaultView;
                    comboBox5.DisplayMember = "feeName";
                    comboBox5.ValueMember = "FeeID";
                    comboBox5.SelectedIndex = -1;

                    //  textBox22.Text = feeView.Rows[0]["feeAmount"].ToString();
                    // gcFeeAccount = feeView.Rows[0]["feeIncome"].ToString();
                }
                else { MessageBox.Show("No active Fee found"); }
            }
        }


        private void postFee()
        {
            // string tcContra = globalvar.gcIntSuspense;
            string tcContra = gcContAcct.Substring(4, 6);
           // ccontra.Substring(0, 3);
        //    MessageBox.Show("this is tcContra " + tcContra);
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcDesc = string.Empty;
            string tcCustcode = gcPostAcct.Substring(4, 6);
        //    MessageBox.Show("this is tcContra " + tcCustcode);
            decimal tnTranAmt = -Math.Abs(Convert.ToDecimal(textBox22.Text));
            decimal tnContAmt = Math.Abs(Convert.ToDecimal(textBox22.Text));
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
            gls.updGlmast(cs, gcPostAcct, tnTranAmt);                                                                       //update member savings account glmast 
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, gcPostAcct);
            tls1.updCuTranhist(cs, gcPostAcct, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcCustcode, lnWaiveAmt, tnqty, tnTranAmt, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account

            //savings product control account and send for verification - debit 
            auditDesc = "Fee Paid-> " + gcPostControlAcct;
            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
            //    upj.updJournal(cs, gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            upj.updJournal(cs, gcPostControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, tncompid);
            updcl.updClient(cs, "nvoucherno");

            // joining fee defined in company details  - credit
            //                MessageBox.Show("The joining fee account = " + gcJoinFeeAcct);
            auditDesc = "Fee Paid-> " + gcFeeAccount;
            au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(tnTranAmt), globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
            upj.updJournal(cs, gcFeeAccount, tcDesc, Math.Abs(tnTranAmt), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, tncompid);
            updcl.updClient(cs, "nvoucherno");
        }
        //////////

        void savings(string tcAcctNumb, decimal lnTranAmt, string ccontra)
        {
         //   MessageBox.Show("This is the account number " + tcAcctNumb + lnTranAmt);
            string tcContra = ccontra;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = textBox5.Text.Trim();
            int tncompid = globalvar.gnCompid;
            string contratype = ccontra.Substring(0, 3);
            string contratype1 = tcAcctNumb.Substring(0, 3);
            string tcPostCustcode = tcAcctNumb.Substring(3, 6);
            string tcContCustcode = ccontra.Substring(3, 6);
            decimal tnTranAmt = -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));
            decimal tnContAmt = Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = "000001";
            string auditDesc = string.Empty;
            //            int npaytype =1;
            string tcCustcode = string.Empty;
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "12";
            int lnServID = 0;
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            bool lFreeBee = false;

            updateGlmast gls = new updateGlmast();
            // updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            updateJournal upj = new updateJournal();
            AuditTrail au = new AuditTrail();
            updateClient_Code updcl = new updateClient_Code();

         //   MessageBox.Show("This is the account number " + tcAcctNumb + tnTranAmt);
            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);
            tcPostCustcode = tcAcctNumb.Substring(3, 6); //update glmast posting account
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcPostCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

            upj.updJournal(cs, gcPostControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);  //send control account for posting account to verification

            if (contratype == "251" || contratype == "271")
            {
                gls.updGlmast(cs, tcContra, tnContAmt);
                tcContCustcode = tcContra.Substring(3, 6);
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcContCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, globalvar.gdSysDate);                        //update tranhist account 396 1756

                upj.updJournal(cs, gcContControlAcct, tcDesc, tnContAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid); //send control account for receiving account to verification

                updcl.updClient(cs, "nvoucherno");
            }
        }
        private void updateInterestDate(string tcAcctNumb)
        {
            string cglquery = "update glmast set lintdate = @dTransactionDate,accruedinterest = @taccruedBal where cacctnumb = @acctNumb";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nconnHandle);
                updCommand.UpdateCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dTranDate.Value);
                updCommand.UpdateCommand.Parameters.Add("@taccruedBal", SqlDbType.Decimal).Value = gnInterestBalance;
                updCommand.UpdateCommand.Parameters.Add("@acctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                nconnHandle.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nconnHandle.Close();
            }
        }

        private void updateAmort()
        {
            //for (int m = 0; m < amortview.Rows.Count; m++)
            //{
            //    if (Convert.ToBoolean(amortview.Rows[m]["lpaid"]))
            //    {
            //        int gnOrder = Convert.ToInt32(amortview.Rows[m]["norder"]);
            //        amortOrder(gnOrder);
            //    }
            //}
        }
        private void UpdateLoanClosed(string tcAcctNumb)
        {
            string cglquery = "Update glmast set caccstat = 'C' where cacctnumb = @acctNumb";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nconnHandle);
                //  updCommand.UpdateCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(gdTransactionDate);
                updCommand.UpdateCommand.Parameters.Add("@acctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                nconnHandle.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nconnHandle.Close();
            }
        }

        private void initvariables()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            maskedTextBox1.Text = "";
            textBox5.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox22.Text = "";
            textBox23.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            postview.Clear();
            contview.Clear();
            pictureBox1.ResetText();
            //pictureBox2.ResetText();
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            PostAcct();
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox5.Focused)
            {
                DataRow dr = feeView.Rows[comboBox5.SelectedIndex];
                //   textBox22.Text = feeView.Rows[comboBox5.SelectedIndex]["feeamount"].ToString();
                textBox22.Text = Convert.ToDecimal(feeView.Rows[comboBox5.SelectedIndex]["feeamount"]).ToString("N2");
                gcFeeAccount = feeView.Rows[comboBox5.SelectedIndex]["feeIncome"].ToString();
                textBox23.Text = gcFeeAccount;
            }
        }
    }
}
