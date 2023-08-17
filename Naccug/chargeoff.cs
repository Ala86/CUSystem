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
    public partial class chargeoff : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable clientview = new DataTable();
        DataTable loanview = new DataTable();
        DataTable membSavingsView = new DataTable();
        DataTable membSharesView = new DataTable();
        DataTable LoanAccountsView = new DataTable();

        updateClient_Code updcl = new updateClient_Code();
        string gcBadDebtExp = string.Empty;
        string gcSavingsControlAcct = string.Empty;
        string gcSharesControlAcct = string.Empty;
        string gcLoansControlAcct = string.Empty;
        string gcMemberLoanAcct = string.Empty;
        int gnLoanProduct = 0;
        decimal gnTotalAccruedInterest = 0.00m;
        int gnIntScope = 0;

        string gcMembCode = string.Empty;
        int gnLoanID = 0;
        decimal gnLoanAmt = 0.00m;
        decimal gnLoanBal = 0.00m;
        int gnChargeType = 0;
        int gnNewLoanID = 0;
        DateTime gdTransactionDate = new DateTime();


        public chargeoff(int chargetype)
        {
            InitializeComponent();
            gnChargeType = chargetype;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chargeoff_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + (gnChargeType == 1 ? " << Loan Charge Off  >>" : (gnChargeType == 2 ? " << Loan Activate  >>" : " << Loan Write off  >>"));
            saveButton.Text = (gnChargeType == 1 ? "Confirm Charge Off" : (gnChargeType == 2 ? "Confirm Activate" : "Confirm Write off"));
           // MessageBox.Show("This is the error First");
            getclientList();
           // MessageBox.Show("This is the error!");
            if (clientview.Rows.Count > 0)
            {
                clientgrid.Columns["loanAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
                clientgrid.Columns["loanAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                clientgrid.Columns["appldate"].SortMode = DataGridViewColumnSortMode.NotSortable;
                clientgrid.Columns["appldate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gnLoanID = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_id"]);
            }
            getchargeoff(globalvar.gnLoanChargeOff);
          //  MessageBox.Show("This is the error 1");
            AllClear2Go();
        }


        private void getclientList()
        {
            clientview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                switch (gnChargeType)
                {
                    case 1:     //loan charge off
                        string dsql0 = "exec tsp_getLoans4ChargeOff  " + ncompid ;  //," + globalvar.gnChargeoffloor;
                        SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                        da0.Fill(clientview);
                        break;
                        
                    case 2: //loan activate 
                        string dsql1 = "exec tsp_getLoansChargeOff  " + ncompid;
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                        da1.Fill(clientview);
                        break;

                    case 3: //loan write off 
                        string dsql2 = "exec tsp_getLoans4WriteOff  " + ncompid + "" ;
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                        da2.Fill(clientview);
                        break;
                }
                if (clientview.Rows.Count > 0)
                {
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "loantype";
                    clientgrid.Columns[3].DataPropertyName = "principal_amt";
                    clientgrid.Columns[4].DataPropertyName = "issued_date";
                    clientgrid.Columns[5].DataPropertyName = "memberacct";
                    ndConnHandle.Close();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                    firstclient();
                    clientgrid.Focus();
                }
                else { MessageBox.Show("No " + (gnChargeType == 1 ? "Active Loans" : (gnChargeType == 2 ? "Charge Off Loans" : " Write off Loans"))+" found to process"); }
            }
        }//end of getclientlist

        private void firstclient()
        {
            if (clientview.Rows.Count > 0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                gcMembCode = drow["ccustcode"].ToString();
                gcMemberLoanAcct = drow["memberacct"].ToString();
                gnLoanID = Convert.ToInt32(drow["loan_id"]);
                gnLoanProduct = Convert.ToInt32(drow["loan_type_id"]);
                gnLoanAmt = Convert.ToInt32(drow["principal_amt"]);  //gnLoanBal
              //  gnLoanBal = Convert.ToInt32(drow["loanbal"]);  //gnLoanBal
                getLoanDetails(gcMembCode,gnLoanID, gcMemberLoanAcct);
                getLoanAccounts(gnLoanID);

                //                MessageBox.Show("AFTER GETTING loan accounts ");

                if (gnChargeType==3)  //write off loan
                {
                    label2.Visible = true;
                    textBox1.Visible = true;
                    getmemberSavings(gcMembCode);
                    getmemberShares(gcMembCode);
                    //  textBox1.Text = (Convert.ToDecimal(textBox13.Text) - (Convert.ToDecimal(textBox18.Text) + Convert.ToDecimal(textBox12.Text))).ToString("N2");
                 //   textBox1.Text = gnLoanBal.ToString("N2") - (Convert.ToDecimal(textBox18.Text) + Convert.ToDecimal(textBox12.Text))).ToString("N2");
                }
                AllClear2Go();
            }
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (clientview.Rows.Count > 0)
            {
              //  MessageBox.Show("This is the place");
                gcMembCode = Convert.ToString(clientview.Rows[e.RowIndex]["ccustcode"]);
                gcMemberLoanAcct = Convert.ToString(clientview.Rows[e.RowIndex]["memberacct"]);
                gnLoanID = Convert.ToInt32(clientview.Rows[e.RowIndex]["loan_id"]);
                gnLoanAmt = Convert.ToInt32(clientview.Rows[e.RowIndex]["principal_amt"]);
              //  MessageBox.Show("This is the place");
                getLoanDetails(gcMembCode,gnLoanID, gcMemberLoanAcct);
              //  MessageBox.Show("This is the place 1");
                getLoanAccounts(gnLoanID);
                if (gnChargeType == 3)  //write off loan
                {
                    label2.Visible = true;
                    textBox1.Visible = true;
                    getmemberSavings(gcMembCode);
                    getmemberShares(gcMembCode);

                    // textBox1.Text = (Math.Abs(Convert.ToInt32(textBox2.Text)) + Math.Abs(Convert.ToInt32(textBox3.Text)).ToString());
                    //  textBox1.Text = (Convert.ToInt32(textBox2.Text) + Convert.ToInt32(textBox3.Text)).ToString();
                   // textBox1 = "0";
                   // textBox1.Text = Convert.ToString(Convert.ToInt32(textBox2.Text) + Convert.ToInt32(textBox3.Text));
                }
                    AllClear2Go();
            }
        }

        private void getLoanDetails(string memcode,int tnloanid, string accountnum)
        {
            string dsql = "exec tsp_getLoanDetails1  " + ncompid + ",'" + memcode + "'," + tnloanid;
            loanview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(loanview);
                if (loanview.Rows.Count > 0)
                {
                    getAccruedInterest(accountnum);
                    textBox16.Text = Convert.ToInt32(loanview.Rows[0]["principal_amt"]).ToString("N2");
                    textBox21.Text = Convert.ToInt32(loanview.Rows[0]["total_interest"]).ToString("N2");
                    textBox13.Text = (Convert.ToInt32(loanview.Rows[0]["principal_amt"]) + Convert.ToInt32(loanview.Rows[0]["total_interest"])).ToString("N2");
                    textBox2.Text = Math.Abs(Convert.ToInt32(loanview.Rows[0]["loanbal"])).ToString("N2") ;
                    textBox3.Text = Math.Abs(gnTotalAccruedInterest).ToString("N2");
                  //  MessageBox.Show("This is the Input 0 "+ Math.Round(Math.Abs(gnTotalAccruedInterest)));
                    textBox4.Text = (Math.Abs(Convert.ToInt32(loanview.Rows[0]["loanbal"])) + Math.Round(Math.Abs(gnTotalAccruedInterest))).ToString();
                   // MessageBox.Show("This is the Input 1 " + textBox4.Text);

                  //  MessageBox.Show("This is the Input 2 " + Math.Abs(Convert.ToInt32(loanview.Rows[0]["ala"])));
                    //textBox1.Text = (Math.Abs(Convert.ToInt32(loanview.Rows[0]["ala"])) - (Math.Abs(Convert.ToInt32(loanview.Rows[0]["loanbal"])) + gnTotalAccruedInterest)).ToString("N2");
                    textBox1.Text = (Math.Round(Math.Abs(Convert.ToDecimal(textBox4.Text))) - Math.Abs(Convert.ToInt32(loanview.Rows[0]["ala"]))).ToString("N2");
                   // MessageBox.Show("This is the Input 2 " + textBox1.Text);
                    // decimal baks = 0.00m
                    //  textBox3.Text = (Convert.ToInt32(loanview.Rows[0]["sharebal"]).ToString("N2") + Convert.ToInt32(loanview.Rows[0]["savebal"]).ToString("N2"));
                    //   MessageBox.Show("This is the Input 1 " + textBox3.Text);
                    //  decimal ala = Convert.ToDecimal(textBox2.Text);
                    //  MessageBox.Show("This is the Input 1 ");
                    string ala1 = textBox3.Text;
                 //   MessageBox.Show("This is the Input 2 ");
                  //  decimal ala2 = ala - ala1;
                  //  textBox1.Text = ala2.ToString("N2");
                //    MessageBox.Show("This is the Input 3 ");
                } //else { MessageBox.Show("No loan details found"); }
            }
        }


        // Interest Calculation

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
                    DateTime dtrandate = Convert.ToDateTime(dateTimePicker1.Value);

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
                        //loanGrid.AutoGenerateColumns = false;
                        //loanGrid.DataSource = dprodview.DefaultView;
                        //loanGrid.Columns[0].DataPropertyName = "loantype";
                        //loanGrid.Rows[0].Cells[0].Value = Math.Abs(nloanBalance).ToString("N2");
                        //loanGrid.Rows[0].Cells[1].Value = Math.Abs(accruedInterest).ToString("N2");
                        //loanGrid.Rows[0].Cells[2].Value = Math.Abs(nAccruedInterest).ToString("N2");
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
                        textBox18.Text = Math.Abs(gnTotalAccruedInterest).ToString("N2");
                        //loanGrid.AutoGenerateColumns = false;
                        //loanGrid.DataSource = dprodview.DefaultView;
                        //loanGrid.Columns[0].DataPropertyName = "loantype";
                        //loanGrid.Rows[0].Cells[0].Value = Math.Abs(nloanBalance).ToString("N2");
                        //loanGrid.Rows[0].Cells[1].Value = Math.Abs(calculatedInterest).ToString("N2");
                        //loanGrid.Rows[0].Cells[2].Value = Math.Abs(nAccruedInterest).ToString("N2");
                    }

                }
                else { MessageBox.Show("No loan details found for selected loan account, inform IT Dept immediately"); }
            }
        }

        private void getLoanAccounts(int tnLoanid)
        {
            string dsql = "exec tsp_getLoansProdAccounts  " + ncompid +","+ tnLoanid;
            LoanAccountsView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(LoanAccountsView);
                if (LoanAccountsView.Rows.Count > 0)
                {
                    gcBadDebtExp = LoanAccountsView.Rows[0]["bad_deb_exp"].ToString();
                    int tnPrdid = Convert.ToInt16(LoanAccountsView.Rows[0]["prd_id"]);
                    gcLoansControlAcct = getProductControl.productControl(cs, tnPrdid);
//                    gcLoansControlAcct = LoanAccountsView.Rows[0]["prod_control"].ToString();
//                    MessageBox.Show("bad debt, prod control " + gcBadDebtExp + "," + gcLoansControlAcct);
                }
            }
        }
        private void getmemberSavings(string memcode)
        {
            decimal dmemsavings = 0.00m;
            string dsql = "select cacctnumb, cacctname, nbookbal,prd_id  from glmast where intcode = 0 and acode in ('250','251') AND nbookbal>0.00 and  ccustcode = " + "'" + memcode + "'"; 
            membSavingsView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(membSavingsView);
                if (membSavingsView.Rows.Count > 0)
                {
                    for (int i = 0; i < membSavingsView.Rows.Count;i++)
                    {
                        dmemsavings += Convert.ToDecimal(membSavingsView.Rows[i]["nbookbal"]);
                    }
                    textBox18.Text = dmemsavings.ToString("N2");
                    label21.Text = "Member Savings Balance";
                }
            }
        }

        private void getmemberShares(string memcode)
        {
            decimal dmembshares = 0.00m;
            string dsql = "select cacctnumb, cacctname, nbookbal,prd_id  from glmast where intcode = 0 and acode in ('270','271') AND nbookbal > 0.00 and ccustcode = " + "'" + memcode + "'";
            membSharesView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(membSharesView);
                if (membSharesView.Rows.Count > 0)
                {
                    for (int i = 0; i < membSharesView.Rows.Count; i++)
                    {
                        dmembshares += Convert.ToDecimal(membSharesView.Rows[i]["nbookbal"]);
                    }
                    textBox12.Text = dmembshares.ToString("N2");
                    label1.Text = "Member Shares Balance";
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
            if (textBox16.Text.ToString() != "")
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

        private void getchargeoff(int nchargeoff)
        {
            string dsqloan = "select ltrim(rtrim(prd_name)) as productname from prodtype where prd_id = " + nchargeoff;
            DataTable loanchargeview = new DataTable();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dsqloan, ndConnHandle);
                daloan.Fill(loanchargeview);
                if (loanchargeview.Rows.Count > 0)
                {
                    if(gnChargeType!=3)
                    {
                        textBox1.Text = loanchargeview.Rows[0]["productname"].ToString();
                    }
                }
            }
        }

        private void insertloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                gnNewLoanID = GetClient_Code.clientCode_int(cs, "loan_id");
                decimal gnLoanAmt = Convert.ToDecimal(loanview.Rows[0]["PRINCIPAL_AMT"]);


                string cglquery = "Insert Into LOAN_DET (loan_id, ccustcode,NET_SAVINGS,LOAN_TYPE_ID,LOAN_INTEREST,PRINCIPAL_AMT,LDURATION_NUM,LOANSTART_DATE,MATURITY_DATE,";
                cglquery += "REPAYMENT_AMT,NOFPAYMENTS,TOTAL_INTEREST,cuserid,BRANCH_ID,loan_appl_date,leconsec,lloanpurpos,memsourcefunds,guasourcefunds,nofpayperyear,graceperiod,graceperiodinterest,compid,topup,resched,loan_status,loan_appr_date,";
                cglquery += "lissued,issued_date,lgranteed,lapproved,lamort,old_loan_id)";
                cglquery += " values (@loanid,@lccustcode,@lNET_SAVINGS,@lLOAN_TYPE_ID,@lLOAN_INTEREST,@lPRINCIPAL_AMT,@lLDURATION_NUM,@lLOANSTART_DATE,@lMATURITY_DATE,";
                cglquery += "@lREPAYMENT_AMT,@lNOFPAYMENTS,@lTOTAL_INTEREST,@lcuserid,@lBRANCH_ID,convert(date,getdate()),@nleconsec, @nlloanpurpos, @nmemsourcefunds, @nguasourcefunds,@lnofpayperyear,@lgraceperiod,@lgraceperiodinterest,@ncompid,";
                cglquery += "@ltopup,@lresched,@lloan_status,@lloan_appr_date,1,convert(date,getdate()),1,1,1,@loldloan)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnNewLoanID;
                insCommand.InsertCommand.Parameters.Add("@lccustcode", SqlDbType.VarChar).Value = loanview.Rows[0]["ccustcode"].ToString();
                insCommand.InsertCommand.Parameters.Add("@lNET_SAVINGS", SqlDbType.Decimal).Value = Convert.ToDecimal(loanview.Rows[0]["net_savings"]);
                insCommand.InsertCommand.Parameters.Add("@lLOAN_TYPE_ID", SqlDbType.Int).Value = globalvar.gnLoanChargeOff;
                insCommand.InsertCommand.Parameters.Add("@lLOAN_INTEREST", SqlDbType.Decimal).Value = Convert.ToDecimal(loanview.Rows[0]["LOAN_INTEREST"]);
                insCommand.InsertCommand.Parameters.Add("@lPRINCIPAL_AMT", SqlDbType.Decimal).Value = gnLoanAmt;
                insCommand.InsertCommand.Parameters.Add("@lLDURATION_NUM", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["LDURATION_NUM"]);
                insCommand.InsertCommand.Parameters.Add("@lLOANSTART_DATE", SqlDbType.DateTime).Value = Convert.ToDateTime(loanview.Rows[0]["LOANSTART_DATE"]);
                insCommand.InsertCommand.Parameters.Add("@lMATURITY_DATE", SqlDbType.DateTime).Value = Convert.ToDateTime(loanview.Rows[0]["MATURITY_DATE"]);
                insCommand.InsertCommand.Parameters.Add("@lREPAYMENT_AMT", SqlDbType.Decimal).Value = Convert.ToDecimal(loanview.Rows[0]["REPAYMENT_AMT"]);
                insCommand.InsertCommand.Parameters.Add("@lnofpayperyear", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["nofpayperyear"]);
                insCommand.InsertCommand.Parameters.Add("@lNOFPAYMENTS", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["NOFPAYMENTS"]);
                insCommand.InsertCommand.Parameters.Add("@lTOTAL_INTEREST", SqlDbType.Decimal).Value = Convert.ToInt16(loanview.Rows[0]["TOTAL_INTEREST"]);
                insCommand.InsertCommand.Parameters.Add("@lcuserid", SqlDbType.Char).Value = globalvar.gcUserid;
                insCommand.InsertCommand.Parameters.Add("@lBRANCH_ID", SqlDbType.Int).Value = globalvar.gnBranchid;
                insCommand.InsertCommand.Parameters.Add("@nleconsec", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["leconsec"]);
                insCommand.InsertCommand.Parameters.Add("@nlloanpurpos", SqlDbType.Int).Value = loanview.Rows[0]["lloanpurpos"] != null ? Convert.ToInt16(loanview.Rows[0]["lloanpurpos"]) : 0;
                insCommand.InsertCommand.Parameters.Add("@nmemsourcefunds", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["memsourcefunds"]) > 0 ? Convert.ToInt16(loanview.Rows[0]["memsourcefunds"]) : 0;
                insCommand.InsertCommand.Parameters.Add("@nguasourcefunds", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["guasourcefunds"]) > 0 ? Convert.ToInt16(loanview.Rows[0]["guasourcefunds"]) : 0;
                insCommand.InsertCommand.Parameters.Add("@lgraceperiod", SqlDbType.Int).Value = Convert.ToInt16(loanview.Rows[0]["graceperiod"]);
                insCommand.InsertCommand.Parameters.Add("@lgraceperiodinterest", SqlDbType.Decimal).Value = Convert.ToDecimal(loanview.Rows[0]["graceperiodinterest"]);
                insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = globalvar.gnCompid;
                insCommand.InsertCommand.Parameters.Add("@ltopup", SqlDbType.Bit).Value = Convert.ToBoolean(loanview.Rows[0]["topup"]);
                insCommand.InsertCommand.Parameters.Add("@lresched", SqlDbType.Bit).Value = Convert.ToBoolean(loanview.Rows[0]["resched"]);
                insCommand.InsertCommand.Parameters.Add("@lloan_status", SqlDbType.Int).Value = 3;
                insCommand.InsertCommand.Parameters.Add("@lloan_appr_date", SqlDbType.DateTime).Value = DateTime.Today;
                insCommand.InsertCommand.Parameters.Add("@loldloan", SqlDbType.Int).Value = gnLoanID;

                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();

                string auditDesc = "Loan Charge Off  -> " + gcMembCode;
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, gnLoanAmt, globalvar.gcUserid, "C", "", gcMembCode, "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            }
        }

        // Interest date update
        private void updateInterestDate(string tcAcctNumb)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                //  MessageBox.Show("you are indise the code" + gdTransactionDate);
                nConnHandle2.Open();
                string cupdatequry1 = "update glmast set lintdate = @dTransactionDate where cacctnumb = @acctNumb";
                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                updCommand1.UpdateCommand = new SqlCommand(cupdatequry1, nConnHandle2);
                updCommand1.UpdateCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = dateTimePicker1.Value;// Convert.ToDateTime(gdTransactionDate);
                updCommand1.UpdateCommand.Parameters.Add("@acctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                updCommand1.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void updOldloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "update LOAN_DET set loan_status = 5,chargeoff_date = convert(date,getdate()) where loan_id = @LoanID ";
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

                updCommand.UpdateCommand.Parameters.Add("@LoanID", SqlDbType.Int).Value = gnLoanID;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
            gdTransactionDate = Convert.ToDateTime(dateTimePicker1.Value);
     
        }

        private void activateLoan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "update LOAN_DET set loan_status=1,activate_date = convert(date,getdate()) where loan_id = @LoanID ";
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

                updCommand.UpdateCommand.Parameters.Add("@LoanID", SqlDbType.Int).Value = gnLoanID;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void initvariables()
        {
            saveButton.BackColor = Color.Gainsboro;
            saveButton.Enabled = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
             switch (gnChargeType)
            {
                case 1: //loan charge off
                    updOldloan();
                    break;

                case 2: //loan activate 
                    activateLoan();
                    updateInterestDate(gcMemberLoanAcct);
                    break;
                case 3: //loan write off 
                    if (membSavingsView.Rows.Count>0)  //savings balance exists 
                    {
                        for(int p =0; p<membSavingsView.Rows.Count;p++)
                        {
                            string lcSavAcct = membSavingsView.Rows[p]["cacctnumb"].ToString(); 
                            decimal lnSavBal = Convert.ToDecimal(membSavingsView.Rows[p]["nbookbal"]);
                            int lnSPrd_id = Convert.ToInt16(membSavingsView.Rows[p]["prd_id"]);
                            gcSavingsControlAcct = getProductControl.productControl(cs, lnSPrd_id); //get the product control account for the product
                            postAccounts(lcSavAcct, gcMemberLoanAcct,lnSavBal,true);
                            postAccounts(gcSavingsControlAcct, string.Empty,-Math.Abs(lnSavBal), false);  //update the product control account for savings 
                            postAccounts(gcLoansControlAcct,string.Empty, Math.Abs(lnSavBal), false);  //update the product control account for loans 
                        }
                    } else { }

                    if (membSharesView.Rows.Count > 0)  //share balance exists 
                    {
                        for (int p = 0; p < membSharesView.Rows.Count; p++)
                        {
                            string lcShaAcct = membSharesView.Rows[p]["cacctnumb"].ToString();
                            decimal lnShaBal = Convert.ToDecimal(membSharesView.Rows[p]["nbookbal"]);
                            int lnhPrd_id = Convert.ToInt16(membSharesView.Rows[p]["prd_id"]);
                            gcSharesControlAcct = getProductControl.productControl(cs, lnhPrd_id);
                            postAccounts(lcShaAcct, gcMemberLoanAcct,lnShaBal,true);
                            postAccounts(gcSharesControlAcct,string.Empty,-Math.Abs(lnShaBal), false); //update the product control account for shares
                            postAccounts(gcLoansControlAcct,string.Empty,Math.Abs(lnShaBal), false); //update the product control account for loans
                        }
                    }

                    if (Convert.ToDecimal(textBox1.Text)>0.00m) //balance to be written off exists
                    {
                        decimal lnWriteBal = Convert.ToDecimal(textBox1.Text);
                        postAccounts(gcBadDebtExp, gcLoansControlAcct, -Math.Abs(lnWriteBal),false);  //update the loan product control account
                        postAccounts(gcLoansControlAcct, string.Empty, Math.Abs(lnWriteBal), false); //update the product control account for loans

                    }
                    updateLoan2WriteOff();
                    break;

            }
            // updateLoan2Bad();
            updcl.updClient(cs, "nvoucherno");
            getclientList();
            initvariables();
            AllClear2Go();
        }

        private void postAccounts(string tcPostAcct, string tcContAcct,decimal tnAmt,bool tlMemberOrGl)
        {
            string tcContra = tcContAcct;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = "Loan Write off";
            int tncompid = globalvar.gnCompid;
            string tcCustcode = gcMembCode;
            decimal tnTranAmt = tlMemberOrGl ?  -Math.Abs(tnAmt) : tnAmt;
            decimal tnContAmt = tlMemberOrGl ?  Math.Abs(tnAmt) : tnAmt;
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(tnAmt);
            string tcChqno ="000001";
            int npaytype = 1;
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "06";
            int lnServID = gnLoanProduct;
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
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            

            if (tlMemberOrGl )   //posting into member accounts 
            {
                gls.updGlmast(cs, tcPostAcct, tnTranAmt);                              //update glmast posting account
                dbal.updDayBal(cs, globalvar.gdSysDate, tcPostAcct, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcPostAcct);      //  0.00m;
                tls1.updCuTranhist(cs, tcPostAcct, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account
                string auditDesc = "Loan Write off Completed -> " + tcPostAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
                dbal.updDayBal(cs, globalvar.gdSysDate, tcContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
                string auditDesc1 = "Loan Write off Completed -> " + tcContAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcPostAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                        //update tranhist account 396 1756
              //  updcl.updClient(cs, "nvoucherno");
            }
            else                //sending gl transacctions into journal.dbf to be verified (verification)
            {
                string auditDesc = "Loan Write off Completed -> " + tcPostAcct;
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                upj.updJournal(cs, tcPostAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode,globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
            //    updcl.updClient(cs, "nvoucherno");
            }
       
        }


        private void updateLoan2WriteOff()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "update LOAN_DET set loan_status = 6, writeoff_date = convert(date,getdate()) where loan_id = @LoanID ";
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

                updCommand.UpdateCommand.Parameters.Add("@LoanID", SqlDbType.Int).Value = gnLoanID;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }
    }
}
