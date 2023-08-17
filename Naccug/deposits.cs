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
using System.IO;
using System.Drawing.Printing;
using CrystalDecisions.CrystalReports.Engine;
using RestSharp;
using System.Net;

namespace WinTcare
{
    public partial class deposits : Form
    {
        string gcTranCode = string.Empty;
        string gcContraAcct = string.Empty;
        string gcBalanceAcct = string.Empty;
        string gcControlAcct = string.Empty; //gcIntAcct
        string gcIntAcct = string.Empty;
        string gcReceiptNo = string.Empty;
        string gcAccountNumber = string.Empty;
        string gcWorkSt = globalvar.gcWorkStation.ToString().Trim();
        string gcWinUsr = globalvar.gcWinUser.ToString().Trim();
        string cs = globalvar.cos;

        int ncompid = globalvar.gnCompid;
        string cloca = globalvar.cLocalCaption;
        int gnLoanID = 0;
        int gnSaveProduct = 0;
        int gnIntScope = 0;
        int gnLoanProduct = 0;
        double gnLoanAmt = 0.00;
        double gnLoanInterest = 0.00;
        double gnLoanRepay = 0.00;
        double gnPerPay = 0.00;
        int gnLoanDur = 0;
        double gnTotalInterest = 0.00;
        DateTime gdToday = globalvar.gdSysDate;


        DateTime gdStartDate ;
        DataTable branchView = new DataTable();
        DataTable acctview = new DataTable();
        DataTable actview = new DataTable();
        DataTable intview = new DataTable();
        DataTable lastview = new DataTable();
        DataTable loanview = new DataTable();
        DataTable amortview = new DataTable();
        DataTable pictview = new DataTable();
        DateTime ldToday = DateTime.Today;
        DataTable clearview = new DataTable();
        DataTable feeView = new DataTable();

        updateGlmast gls = new updateGlmast();
//        updateDaily Balance dbal = new updateDailyBalance();
        updateCuTranhist tls1 = new updateCuTranhist();
        updateClient_Code updcl = new updateClient_Code();

        AuditTrail au = new AuditTrail();
        updateJournal upj = new updateJournal();

        DepositDataSet dds = new DepositDataSet();

        string gcCustCode = string.Empty;
        string gcFeeAccount = string.Empty;
        bool glCashPay = false;
        bool glCheqPay = false;
        bool glMobiPay = false;
        bool glBalanceOk = false;
        bool glControlAccountOk = false;
        decimal gnPayAmt = 0.00m;
        decimal gnTotalAccruedInterest = 0.00m;
        decimal gnInterestBalance = 0.00m;
        DateTime gdLoanStartDate = new DateTime();

        //************************these variables will be used as parameters  
        string tcContra = string.Empty;
        string tcDesc = string.Empty;
        string tcAcctNumb = string.Empty; 
        string tcCustcode = string.Empty;
        string tcContcode = string.Empty;
        string gcUserid = globalvar.gcUserid;
        decimal tnTranAmt = 0.00m;
        decimal tnInterestAmt = 0.00m;
        decimal tnContAmt = 0.00m;
        string tcVoucher = string.Empty;
        decimal unitprice = 0.00m;
        string tcChqno = string.Empty;
        int npaytype = 0; 
        decimal lnWaiveAmt = 0.00m;
        string tcTranCode = string.Empty;
        int lnServID = 0; 
        bool llPaid = true;
        int tnqty = 1;
        bool llCashpay = true;
        bool glLoanProcess = false;
        int visno = 1;
        bool isproduct = false;
        int srvid = 1;
        bool lFreeBee = false;
        string auditDesc = string.Empty;
        decimal tnPayDiff = 0.00m;
        decimal tnPrinAmt = 0.00m;
        decimal tnPrinCont = 0.00m;
        decimal tnAccruedPost = 0.00m;
        decimal tnAccruedCont = 0.00m;
        int gnLoanStatus = 0;



        //*********************parameters 

        public EventHandler RefreshNeeded;
        public deposits(string dtrancode)
        {
            InitializeComponent();
            gcTranCode = dtrancode;
        }

  
        private void deposits_Load(object sender, EventArgs e)
        {

           // MessageBox.Show("This is the connection string " + cs);
            this.Text = cloca + 
                (gcTranCode =="01" ? "<< Member Deposit >>" : 
                (gcTranCode == "02" ? "<< Member Withdrawal >>" :
                (gcTranCode == "03" ? "<< Loan Offset >>" :
                (gcTranCode == "07" ? "<< Loan Repayment >>"    :
                (gcTranCode == "09" ? "<< Loan Payout/close >>" :
                (gcTranCode == "22" ? "<< Loan Charge Off >>" : ""))))));

            textBox1.Text = 
                (gcTranCode == "01" ? "Deposit" : 
                (gcTranCode == "02" ? "Withdrawal" :
                (gcTranCode == "03" ? "Loan Offset" :
                (gcTranCode == "07" ? "Loan Repayment" :
                (gcTranCode == "09" ? "Loan Payout/close" :
                (gcTranCode == "22" ? "Loan Charge Off" : ""))))));

            if(gcTranCode=="01") //deposit, when a user has deposit limit, then user can proceed, otherwise user is blocked
            {
                maskedTextBox1.Enabled = globalvar.gnCreditLimit == 0.00m ? false : true;
            }

            if (gcTranCode == "02" || gcTranCode=="07") //withdrawal, loan repayment , when a user has debit limit, then user can proceed, otherwise user is blocked
            {
                maskedTextBox1.Enabled = globalvar.gnDebitLimit == 0.00m ? false : true;
            }

            //            maskedTextBox1.Enabled = globalvar.gnDebitLimit==0.00m && globalvar.gnCreditLimit==0.00m

            //            textBox7.Text = genbill.genvoucher(cs, globalvar.gdSysDate);
            string djv = CuVoucher.genJv(cs, globalvar.gdSysDate);
            textBox7.Text = "<" + djv.Substring(0, 2) + "-" + djv.Substring(2, 2) + "-" + djv.Substring(4, 2) + ">" + djv.Substring(6, 4);// CuVoucher.genJv(cs, globalvar.gdSysDate);

            gcReceiptNo = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
           // gcReceiptNo = ldToday.Year.ToString().Trim().PadLeft(2,'0')+ldToday.Month.ToString().Trim().PadLeft(2,'0')+GetClient_Code.clientCode_int(cs, "rec_no").ToString().Trim().PadLeft(4,'0');
            textBox15.Text = gcReceiptNo;
            label19.Visible = label20.Visible = textBox3.Visible = panel6.Visible = comboBox2.Visible = gcTranCode == "07" ? true : false;

            getbanks();
            gcContraAcct = globalvar.gcCashAccont.ToString().Trim();
            if(gcContraAcct.ToString().Trim().Length !=11)
            {
                MessageBox.Show("The contract account format is invalid, you will not be able to do Cash Transactions ");
//                this.clos
            }

            get5Lasttrans(globalvar.gcUserid);
            textBox4.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccontName : "");
            textBox2.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccont : "");
            gcContraAcct = globalvar.gcCashAccont;
            textBox5.Text = textBox1.Text;
            
            loanGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            loanGrid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            loanGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            loanGrid.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            loanGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            loanGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;


            amortGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            amortGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            amortGrid.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            loanGrid.Visible = (gcTranCode == "03" || gcTranCode == "07" ? true : false);
            label25.Visible = (gcTranCode == "03" || gcTranCode == "07" ? true : false);
            textBox17.Visible = (gcTranCode == "03" || gcTranCode == "07" ? true : false);
            label12.Visible = (gcTranCode == "03" || gcTranCode == "07" ? true : false);
            textBox9.Visible = (gcTranCode == "03" || gcTranCode == "07" ? true : false);
            makeAmort();
            getFee();
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

        private void get5Lasttrans(string duser)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************Getting accounts                
                string dsql1r = " exec tsp_Last4Trans " + ncompid + ",'" + duser + "'"; 
                SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                da1r.Fill(lastview);
                if (lastview != null)
                {
                    //acctGrid.AutoGenerateColumns = false;
                    //acctGrid.DataSource = lastview.DefaultView;
                    //acctGrid.Columns[0].DataPropertyName = "dtrandate";
                    //acctGrid.Columns[1].DataPropertyName = "cacctnumb";
                    //acctGrid.Columns[2].DataPropertyName = "ctrandesc";
                    //acctGrid.Columns[3].DataPropertyName = "ndebit";
                    //acctGrid.Columns[4].DataPropertyName = "ncredit";
                    ndConnHandle1.Close();
                }
 //               else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }
            }
           
        }

        private void getMemberAccounts(string memtype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting accounts   ("The memtype is " + memtype);
                switch (memtype)
                {
                    case "01":      //Member deposit for savings and shares accounts
                        string dsql1 = "exec  tsp_MemberDepositAccounts " + ncompid;
                        SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                        da1.Fill(intview);
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        else { MessageBox.Show("no depositable account"); }
                        break;
                    case "02":      // member withdrawal  tsp_MemberWithdrawalAccounts
                                    //                        string dsql2 = "exec tsp_MemberWithdrawalAccounts " + ncompid; 
                        string dsql2 = "exec tsp_MemberDepositAccounts " + ncompid; // this is because when members close their accounts, meaning leavingthe credit union, they will have to withdraw their funds
                        SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                        da2.Fill(intview);
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "07":      // Loan repayment
                                    // exec tsp_getLoansDisbursed 30,'000011'
                        string dsql31 = "exec tsp_MemberLoanAccounts " + ncompid; ;
                        SqlDataAdapter da31 = new SqlDataAdapter(dsql31, ndConnHandle1);
                        //                        DataTable disview = new DataTable();
                        da31.Fill(intview);
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "09":      // Loan payout/close 
                        string dsql31p = "exec tsp_MemberLoanAccounts " + ncompid; ;
                        SqlDataAdapter da31p = new SqlDataAdapter(dsql31p, ndConnHandle1);
                        //                        DataTable disviewp = new DataTable();
                        da31p.Fill(intview);
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "22":      // Loan charge off , deposits only
                        string dsql31o = "exec tsp_MemberChargeOffLoanAccounts " + ncompid; ;
                        SqlDataAdapter da31o = new SqlDataAdapter(dsql31o, ndConnHandle1);
                        //                        DataTable disviewo = new DataTable();
                        da31o.Fill(intview);
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                }
            }
        }

        private void getAccounts(string memtype,string searchKey,string searchtype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                intview.Clear();
                //************Getting accounts     
                switch (memtype)
                {
                    case "01":      //Member deposit for savings and shares accounts

                        if (searchtype == "C")
                        {
                            string dsql1 = "exec  tsp_MemberDepositAccounts_c " + ncompid + ", '" + searchKey + "'";
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                            da1.Fill(intview);
                        }
                        else
                        {
                            string dsql1 = "exec  tsp_MemberDepositAccounts_p " + ncompid + ", '" + searchKey + "'";
                            SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                            da1.Fill(intview);
                        }
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;

                            //textBox6.Text = searchtype == "P" ? intview.Rows[0]["ccustcode"].ToString().Trim() : searchKey;
                            //textBox8.Text = searchtype == "C" ? intview.Rows[0]["payroll_id"].ToString().Trim() : searchKey;
                        }
                        else
                        { MessageBox.Show("no depositable account"); }
                        break;
                    case "02":      // member withdrawal  
                        if(searchtype == "C")
                        {
                            string dsql2 = "exec  tsp_MemberDepositAccounts_c " + ncompid + ", '" + searchKey + "'";
                            SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                            da2.Fill(intview);

                        }
                        else
                        {
                            string dsql2 = "exec tsp_MemberDepositAccounts_p " + ncompid + ", '" + searchKey + "'"; //search by member payroll
                            SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                            da2.Fill(intview);

                        }

                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "03":      // Loan offset
                        if (searchtype == "C")
                        {
                            string dsql3 = "exec  tsp_MemberLoanAccounts_c " + ncompid + ", '" + searchKey + "'";
                            SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle1);
                            da3.Fill(intview);
                        }
                        else
                        {
                            string dsql31 = "exec tsp_MemberLoanAccounts_p " + ncompid + ", '" + searchKey + "'"; //search by member payroll
                            SqlDataAdapter da31 = new SqlDataAdapter(dsql31, ndConnHandle1);
                            da31.Fill(intview);
                        }
                        if ( intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "cacctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "07":      // Loan repayment
                        if (searchtype == "C")
                        {
                            string dsql3 = "exec tsp_MemberLoanAccounts_c " + ncompid + ", '" + searchKey + "'";
                            SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle1);
                            da3.Fill(intview);
                        }
                        else
                        {
                            string dsql31 = "exec tsp_MemberLoanAccounts_p " + ncompid + ", '" + searchKey + "'"; //search by member payroll
                            SqlDataAdapter da31 = new SqlDataAdapter(dsql31, ndConnHandle1);
                            da31.Fill(intview);
                        }
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "cacctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "09":      // Loan payout/close 
                        if (searchtype == "C")
                        {
                            string dsql31p = "exec tsp_MemberLoanAccounts_c" + ncompid + ",'" + searchKey + "'";
                            SqlDataAdapter da31p = new SqlDataAdapter(dsql31p, ndConnHandle1);
                            da31p.Fill(intview);
                        }
                        else
                        {
                            string dsql31p = "exec tsp_MemberLoanAccounts_p " + ncompid + ",'" + searchKey + "'";
                            SqlDataAdapter da31p = new SqlDataAdapter(dsql31p, ndConnHandle1);
                            da31p.Fill(intview);
                        }
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                    case "22":      // Loan charge off  
                        if (searchtype == "C")
                        {
                            string dsql31o = "exec tsp_MemberChargeOffLoanAccounts_c " + ncompid + ",'" + searchKey + "'";
                            SqlDataAdapter da31o = new SqlDataAdapter(dsql31o, ndConnHandle1);
                            da31o.Fill(intview);
                        }
                        else
                        {
                            string dsql31o = "exec tsp_MemberChargeOffLoanAccounts_p " + ncompid + ",'" + searchKey + "'";
                            SqlDataAdapter da31o = new SqlDataAdapter(dsql31o, ndConnHandle1);
                            da31o.Fill(intview);
                        }
                        if (intview != null)
                        {
                            comboBox4.DataSource = intview.DefaultView;
                            comboBox4.DisplayMember = "acctname";
                            comboBox4.ValueMember = "cacctnumb";
                            comboBox4.SelectedIndex = -1;
                        }
                        break;
                }
            }
        }
        // Close member 
        private void ClosedMember(string searchKey, string searchtype)
        {

            actview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************Getting accounts   
                string tcCustCode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
                textBox6.Text = tcCustCode;
                if (searchtype == "C")
                {
                    string dsql1r = "Select lactive from cusreg where compid =  " + ncompid + " and ccustcode = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(actview);
                }
                else
                {
                    string dsql1r = "Select lactive from cusreg where compid =  " + ncompid + " and payroll_id = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(actview);
                }
                if (actview != null)
                {

                    for (int m = 0; m < actview.Rows.Count; m++)
                    {
                        if (Convert.ToBoolean(actview.Rows[m]["lactive"]))
                        {
                          //  MessageBox.Show("This is the 1" + tcCustCode);

                           clearTrans();
                           getPictures(tcCustCode, "C");
                           getAccounts(gcTranCode, tcCustCode, "C");
                        }
                        else
                        {
                            MessageBox.Show("This member is closed");
                            textBox6.Text = "";
                            comboBox4.SelectedValue = -1;
                        };
                    }


                }


                else
                { MessageBox.Show("This Member those not exist!"); };
            }

        }

        // Inaccttive member 
        private void InactivedMember(string searchKey, string searchtype)
        {

            actview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************Getting accounts   
                string tcCustCode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
                textBox6.Text = tcCustCode;
                if (searchtype == "C")
                {
                    string dsql1r = "Select lactive from cusreg where compid =  " + ncompid + " and ccustcode = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(actview);
                }
                else
                {
                    string dsql1r = "Select lactive from cusreg where compid =  " + ncompid + " and payroll_id = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(actview);
                }
                if (actview != null)
                {

                    for (int m = 0; m < actview.Rows.Count; m++)
                    {
                        if (Convert.ToBoolean(actview.Rows[m]["lactive"]))
                        {
                            //  MessageBox.Show("This is the 1" + tcCustCode);

                            clearTrans();
                            getPictures(tcCustCode, "I");
                            getAccounts(gcTranCode, tcCustCode, "I");
                        }
                        else
                        {
                            MessageBox.Show("This member is Inactive");
                            textBox6.Text = "";
                            comboBox4.SelectedValue = -1;
                        };
                    }


                }


                else
                { MessageBox.Show("This Member those not exist!"); };
            }

        }
        private void OutstandingLoans(string tcact)
        {
//            string dsqll = "exec tsp_MemberLoanAccounts_One  " + ncompid + ",'" + custcode + "'";
            string dsqll = "exec tsp_MemberLoanDetails_One " + ncompid + ",'" + tcact + "'";
            loanview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dal = new SqlDataAdapter(dsqll, ndConnHandle);
                dal.Fill(loanview);
                if (loanview.Rows.Count > 0)
                {
                    gnLoanID = Convert.ToInt32(loanview.Rows[0]["loan_id"]);
                    gnLoanProduct = Convert.ToInt32(loanview.Rows[0]["prd_id"]);
                    gnLoanAmt = Convert.ToDouble(loanview.Rows[0]["loanamt"]);
                    gnLoanInterest = Convert.ToDouble(loanview.Rows[0]["loanint"]);
                    gnLoanRepay = Convert.ToDouble(loanview.Rows[0]["repay"]);
                    gnLoanDur = Convert.ToInt32(loanview.Rows[0]["ndur"]);
                    gdLoanStartDate = Convert.ToDateTime(loanview.Rows[0]["startdate"]);
                    ndConnHandle.Close();
                    checkamort(gnLoanID);
                }
              //  else { MessageBox.Show("No Outstanding Loans for Account number "+tcact); }
            }
        }

        private void OutstandingAmort(int loanid)
        {
            string dsqll = "exec tsp_LoanAmort_One  " +ncompid+","+loanid;
            amortview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dal = new SqlDataAdapter(dsqll, ndConnHandle);
                dal.Fill(amortview);
                if (amortview.Rows.Count > 0)
                {
                    amortGrid.AutoGenerateColumns = false;
                    amortGrid.DataSource = amortview.DefaultView;
                    amortGrid.Columns[0].DataPropertyName = "npayment";
                    amortGrid.Columns[1].DataPropertyName = "loanamt";
                    amortGrid.Columns[2].DataPropertyName = "loanint";
                    amortGrid.Columns[3].DataPropertyName = "nbegbal";
                    amortGrid.Columns[4].DataPropertyName = "nendbal";
                    amortGrid.Columns[5].DataPropertyName = "nbalance";
                    amortGrid.Columns[6].DataPropertyName = "norder";
                    ndConnHandle.Close();
                }
                else
                {
                    MessageBox.Show("No amortization found for this loan " + loanid);
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
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
            glCashPay = false;
            glCheqPay = false;
            glMobiPay = false;
            glCashPay = (radioButton1.Checked && gcContraAcct.ToString().Trim().Length == 11  ? true : false);
            glCheqPay = (radioButton2.Checked && Convert.ToInt16(comboBox1.SelectedIndex ) > -1 &&  Convert.ToInt16(comboBox3.SelectedIndex) > -1 && textBox14.Text != "" ? true : false);
            glMobiPay = (radioButton3.Checked && textBox10.Text != "" & textBox16.Text != "" ? true : false);

            glBalanceOk = (gnPayAmt > 0.00m ? true : (Convert.ToString(textBox3.Text) != "" && Convert.ToInt32(comboBox2.SelectedValue)> 0 ? true : false));

            if ((glCashPay || glCheqPay || glMobiPay) && glBalanceOk  && textBox1.Text!= "" && textBox2.Text != "" && textBox5.Text != "" && 
                Convert.ToInt16(comboBox4.SelectedIndex) > -1 && maskedTextBox1.Text != "" && glControlAccountOk)
            {
//                MessageBox.Show("cash, chq, mob, gcContraAcct, contra length " + glCashPay + "," + glCheqPay + "," + glMobiPay+","+gcContraAcct+","+gcContraAcct.ToString().Trim().Length);
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
        private void getbanks()
        {
            //************Getting banks
            string dsql12 = "select bnk_name,bnk_id from bank order by bnk_name ";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
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
        }

        // Fee
        //private void getFee()
        //{
        //    //************Getting banks
        //    string dsql12 = "select feeID,feename,feeAmount,feeIncome,feeAccured,compid from Fee order by feename ";
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        SqlDataAdapter da12 = new SqlDataAdapter(dsql12, ndConnHandle);
        //        DataTable ds12 = new DataTable();
        //        da12.Fill(ds12);
        //        if (ds12 != null)
        //        {
        //            comboBox5.DataSource = ds12.DefaultView;
        //            comboBox5.DisplayMember = "feename";
        //            comboBox5.ValueMember = "feeID";
        //            comboBox5.SelectedIndex = -1;
        //        }
        //    }
        //}
        //   xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboBox1.Visible = true;
                comboBox3.Visible = true;
                comboBox1.Enabled = false;
                comboBox3.Enabled = false;
                chkdate.Enabled = false;
                textBox10.Enabled = false;
                textBox10.Visible = false;
                mobTransDate.Enabled = false;
                textBox5.Visible = true;
                textBox14.Enabled = false;
                label17.Text = "Bank Name";
                label13.Text = "Bank Account";
                textBox4.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccontName : "");
                textBox2.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccont : "");
            }
            AllClear2Go();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox1.Visible = true;
                comboBox3.Visible = true;
                comboBox1.Enabled = true;
                comboBox3.Enabled = true;
                textBox14.Enabled = true;
                chkdate.Enabled = true;
                textBox10.Enabled = false;
                textBox10.Visible = false;
                mobTransDate.Enabled = false;
                textBox5.Visible = true;
                label17.Text = "Bank Name";
                label13.Text = "Bank Account";
                comboBox1.Focus();
            }
            AllClear2Go();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox10.Visible = true;
                textBox10.Enabled = true;
                mobTransDate.Enabled = true;
                textBox5.Visible = true;
                comboBox1.Visible = false;
                comboBox3.Visible = false;
                chkdate.Enabled = false;
                textBox14.Enabled = false;
                textBox16.Visible = true;
                textBox16.Enabled = true;
                label17.Text = "Transaction ID";
                label15.Text = "Transaction Date";
                label13.Text = "Transaction Time";
                textBox1.Focus();
            }
            AllClear2Go();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                int bnkid = Convert.ToInt32(comboBox1.SelectedValue);
                getBnkAccts(bnkid);
            }
            AllClear2Go();
        }

        //private string  getloancontrol(int lprid, out string dacct)
        //{
        //    string dasql = "select  prod_control,prd_name from  prodtype where prd_id = " + lprid;
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        ndConnHandle.Open();
        //        SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
        //        DataTable dprodview = new DataTable();
        //        daloan.Fill(dprodview);
        //        if (dprodview.Rows.Count > 0)
        //        {
        //            dacct = dprodview.Rows[0]["prod_control"].ToString();// Convert.ToInt32(loanview.Rows[0]["loan_id"]);
        //            return dacct;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Product Control not defined, inform IT Dept immediately");
        //            dacct = string.Empty;
        //            return dacct;
        //        }
        //    }
        //}

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(comboBox4.Focused)
            //{
            //    MessageBox.Show("inside the combo4 selected index changed ");
            //    int rown1 = Convert.ToInt32(comboBox4.SelectedIndex);
            //    string custcode = intview.Rows[rown1]["ccustcode"].ToString().Trim();
            //    if(gcTranCode !="07")
            //    {
            //        gnSaveProduct = Convert.ToInt16(intview.Rows[rown1]["save_prod"]);
            //    }
            //    gcCustCode = custcode;
            //    acctdetails(gcCustCode, rown1);
            //} 
        }

        private void acctdetails(string dcode,int rown)
        {
            gcCustCode = dcode;// custcode;
            int tnPrdid = Convert.ToInt16(intview.Rows[rown]["prd_id"]);

            gnLoanProduct = tnPrdid;
          //  MessageBox.Show("This is the Product ALA " + gnLoanProduct);
            glControlAccountOk = false;
            gcControlAcct = getProductControl.productControl(cs, tnPrdid);
            if(gcControlAcct.ToString().Trim().Length ==11)
            {
                glControlAccountOk = true;
            }
            else
            {
                MessageBox.Show("Product Control Account is invalid, please verify and try again ");
                glControlAccountOk = false;
            }
            gcIntAcct = getProductControl.interestControl(cs, tnPrdid);
            textBox101.Text = Convert.ToDecimal(intview.Rows[rown]["nbookbal"]).ToString("N2");
            textBox11.Text = Convert.ToDecimal(intview.Rows[rown]["nunclbal"]).ToString("N2");
            textBox13.Text = Convert.ToDecimal(intview.Rows[rown]["ncleabal"]).ToString("N2");
            if (gcTranCode == "03" || gcTranCode == "07")
            {
                OutstandingLoans(gcAccountNumber);
            }
            AllClear2Go();
        }

        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            bool glConContinue = false;
            if (maskedTextBox1.Text != "")
            {
                if (gcTranCode == "03" || gcTranCode == "07")  //Loan repayment, so we need to make sure that  payment <= (loan balance + total accrued interest), otherwise the loan account will go into credit
                {
                    if (Math.Round(Math.Abs(Convert.ToDecimal(maskedTextBox1.Text))) > Math.Round((Math.Abs(Convert.ToDecimal(textBox101.Text)))+ Math.Round(Math.Abs(gnTotalAccruedInterest))))
                    {
                        maskedTextBox1.Text = "";
                        maskedTextBox1.Focus();
                        glConContinue = false;
                    }
                    else
                    {
                        glConContinue = true;
                    }
                }
                else
                {
                    glConContinue = true;
                }

                if (glConContinue)
                {
                    int limitype = gcTranCode == "01" ? 1 : 2;
                    gnPayAmt = Convert.ToDecimal(maskedTextBox1.Text);
                    maskedTextBox1.Text = gnPayAmt.ToString("N2");
                    switch (limitype)
                    {
                        case 1:  //deposit 
                            if (gnPayAmt > globalvar.gnCreditLimit) 
                            {
                                appRoval dapr = new appRoval(cs, ncompid, limitype, gnPayAmt);
                                dapr.ShowDialog();
                                bool dres = dapr.ReturnValue;
                                if (dres)
                                {
                                    string cpayment = maskedTextBox1.Text;
                                    if (cpayment.Replace(",", "").Replace(".", "").Trim() != "")
                                    {
                                        if (cpayment.Substring(cpayment.Length - 3, 1) != ".")
                                        {
                                            cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim() + ".00";
                                            maskedTextBox1.Text = cpayment.PadLeft(12, ' ');
                                        }
                                        else
                                        {
                                            cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim();
                                        }
                                        string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');
                                        string cwords = daltext.inwords(fintext);
                                        string currname = globalvar.gcCurrName.Trim().ToUpper().ToString();
                                        string currunit = globalvar.gcCurrUnit.Trim().ToUpper().ToString();
                                        maskedTextBox1.Text = Convert.ToDecimal(fintext).ToString("N2");
                                        textBox12.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim().ToUpper()).Replace("UNIT", globalvar.gcCurrUnit.Trim().ToUpper());
                                        checkamort();
                                        AllClear2Go();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Authorisation failure, pls try again");
                                    maskedTextBox1.Text = "";
                                    gnPayAmt = 0.00m;
                                    maskedTextBox1.Focus();
                                }
                            }
                            else
                            {
                                string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');
                                string cwords = daltext.inwords(fintext);
                                string currname = globalvar.gcCurrName.Trim().ToUpper().ToString();
                                string currunit = globalvar.gcCurrUnit.Trim().ToUpper().ToString();
                                maskedTextBox1.Text = Convert.ToDecimal(fintext).ToString("N2");
                                textBox12.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim().ToUpper()).Replace("UNIT", globalvar.gcCurrUnit.Trim().ToUpper());
                                checkamort();
                                AllClear2Go();
                            }
                            break;
                        case 2:
                            if (gnPayAmt > globalvar.gnDebitLimit)
                            {
                                appRoval dapr = new appRoval(cs, ncompid, limitype, gnPayAmt);
                                dapr.ShowDialog();
                                bool dres = dapr.ReturnValue;
                                if (dres)
                                {
                                    string cpayment = maskedTextBox1.Text;
                                    if (cpayment.Replace(",", "").Replace(".", "").Trim() != "")
                                    {
                                        if (cpayment.Substring(cpayment.Length - 3, 1) != ".")
                                        {
                                            cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim() + ".00";
                                            maskedTextBox1.Text = cpayment.PadLeft(12, ' ');
                                        }
                                        else
                                        {
                                            cpayment = maskedTextBox1.Text.Replace(",", "").Replace(".", "").Trim();
                                        }
                                        string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');
                                        string cwords = daltext.inwords(fintext);
                                        string currname = globalvar.gcCurrName.Trim().ToUpper().ToString();
                                        string currunit = globalvar.gcCurrUnit.Trim().ToUpper().ToString();
                                        maskedTextBox1.Text = Convert.ToDecimal(fintext).ToString("N2");
                                        textBox12.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim().ToUpper()).Replace("UNIT", globalvar.gcCurrUnit.Trim().ToUpper());
                                        checkamort();
                                        AllClear2Go();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Authorisation failure, pls try again");
                                    maskedTextBox1.Text = "";
                                    gnPayAmt = 0.00m;
                                    maskedTextBox1.Focus();
                                }
                            }
                            else
                            {
                                string fintext = maskedTextBox1.Text.Replace(",", "").PadLeft(12, ' ');
                                string cwords = daltext.inwords(fintext);
                                string currname = globalvar.gcCurrName.Trim().ToUpper().ToString();
                                string currunit = globalvar.gcCurrUnit.Trim().ToUpper().ToString();
                                maskedTextBox1.Text = Convert.ToDecimal(fintext).ToString("N2");
                                textBox12.Text = cwords.Replace("MAIN", globalvar.gcCurrName.Trim().ToUpper()).Replace("UNIT", globalvar.gcCurrUnit.Trim().ToUpper());
                                checkamort();
                                AllClear2Go();
                            }
                            break;
                    }
                }
            }
        }


        private void checkamort()
        {
            decimal lnPayAmt = gnPayAmt;
            decimal lnLinePay = 0.00m;
            for (int k = 0; k < amortview.Rows.Count; k++)
            {
                decimal npay = Convert.ToDecimal(amortview.Rows[k]["loanamt"]) + Convert.ToDecimal(amortview.Rows[k]["loanint"]);
                if (Math.Abs(lnPayAmt) > 0.00m)
                {
                    if (lnPayAmt >= npay)
                    {
                        lnLinePay = npay;
                        amortGrid.Rows[k].DefaultCellStyle.BackColor = Color.Green;
                        amortGrid.Rows[k].DefaultCellStyle.ForeColor = Color.White;
                        amortGrid.Rows[k].Cells[0].Value = lnLinePay;
                        amortGrid.Rows[k].Cells["lpaid"].Value = true;
                        lnPayAmt = lnPayAmt - npay;
                        glBalanceOk = lnPayAmt == 0.00m ? true : false;
                        textBox3.Text = lnPayAmt.ToString("N2");
                        panel6.Enabled = lnPayAmt > 0.00m ? true : false;
                    }
                    else
                    {
                        amortGrid.Rows[k].DefaultCellStyle.BackColor = Color.Red;
                        amortGrid.Rows[k].DefaultCellStyle.ForeColor = Color.Yellow;
                        lnLinePay = lnPayAmt;
                        amortGrid.Rows[k].Cells[0].Value = lnLinePay;
                        amortGrid.Rows[k].Cells["lpaid"].Value = false;
                        lnPayAmt = 0.00m;
                    }
                }
            }
        }

        private void textBox5_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox2_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox3_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox14_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void textBox16_Validated(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            saveButton.ForeColor = Color.Black;
            SaveTran();

           
          //    if (checkBox2.Checked && textBox22.Text != "" && textBox23.Text != "")
                if (textBox22.Text != "" && textBox23.Text != "")
                {
                postFee();
                ////      sms();
            }
            //  else
            //  {
            //      MessageBox.Show("SMS or the fee is not selected ");
            //  }
            if (checkBox1.Checked)
            {
                printreceipt(gcCustCode, gcReceiptNo, 1);
                Reprint re = new Reprint(gcCustCode, gcReceiptNo, 1);
                re.Show();
            }
           initvariables();
        }

        private void SaveTran()
        {
            tcContra = (radioButton1.Checked ? globalvar.gcCashAccont : (radioButton2.Checked ? comboBox3.SelectedValue.ToString() : globalvar.gcMobileCashAccont));
            gcContraAcct = textBox2.Text.ToString().Trim();
            tcDesc = textBox5.Text.Trim();
            tcAcctNumb = comboBox4.SelectedValue.ToString();
            tcCustcode = "";
            tnTranAmt = gcTranCode == "01" ? Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)) :
                             (gcTranCode == "02" ? -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)) :
                             ((gcTranCode == "03" || gcTranCode == "07") ? Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)) : Math.Abs(Convert.ToDecimal(maskedTextBox1.Text))));
            tnInterestAmt = 0.00m;
            tnContAmt = -tnTranAmt;

            int djv0 = GetClient_Code.clientCode_int(cs, "nvoucherno");//  ucc. updateClient_Code
            string djv = string.Empty;
            if (djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "nvoucherno");
                djv = "0001";
            }
            djv = djv0.ToString().Trim().PadLeft(4, '0');
            tcVoucher = gdToday.Year.ToString().Trim().Substring(0, 2) + gdToday.Month.ToString().Trim() + gdToday.Day.ToString().Trim() + djv.Trim();

            unitprice = Math.Abs(tnTranAmt);
            tcChqno = (radioButton1.Checked ? "000001" : (radioButton2.Checked ? textBox14.Text : "999999"));
            npaytype = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
            lnWaiveAmt = 0.00m;
            tcTranCode = gcTranCode;
           // MessageBox.Show("This is the Product " + gnLoanProduct);
            lnServID = gcTranCode == "07" ? gnLoanProduct : gnSaveProduct;
          //  MessageBox.Show("This is the Product " + lnServID);
            llPaid = true;
            tnqty = 1;

            gcReceiptNo = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
          //  gcReceiptNo = ldToday.Year.ToString().Trim().PadLeft(2, '0') + ldToday.Month.ToString().Trim().PadLeft(2, '0') + GetClient_Code.clientCode_int(cs, "rec_no").ToString().Trim().PadLeft(4, '0');
           // MessageBox.Show("This is receipt" + gcReceiptNo);
            textBox15.Text = gcReceiptNo;
            llCashpay = (radioButton2.Checked ? false : true);
            visno = 1;
            isproduct = false;
            srvid = 1;
            lFreeBee = false;
            auditDesc = string.Empty;

            if (gcTranCode == "03" || gcTranCode == "07")
            {
                glLoanProcess = true;
                tcTranCode = gcTranCode == "03" ? "28" : "07";
                tnPayDiff = Math.Abs(Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest)); //different between payment and total accrued interst
                tnPrinAmt = tnPayDiff > 0.00m ? Math.Abs(Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest)) : //payment is more than total account interest, something goes to principal payment  
                tnPayDiff = 0.00m;                                                                             //payment is equal to or less than total accrued interest, nothing goes to principal payment
                tnPrinCont = -Math.Abs(tnPrinAmt);
                tnAccruedPost = tnPayDiff >= 0.00m ? -Math.Abs(gnTotalAccruedInterest) :                       //Total Accrued Interest is covered and Accrued Interest Balance = 0.00m 
                                        -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));                      //payment is less than total accrued interest, payment will service part of the Total Accrued Interest, Accrued Interest Balance > 0.00m
                tnAccruedCont = Math.Abs(gnTotalAccruedInterest);
                decimal tnPayDifff = (Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest));
                gnInterestBalance = tnPayDifff < 0.00m ? (Convert.ToDecimal(maskedTextBox1.Text) - Math.Abs(gnTotalAccruedInterest)) : 0.00m; //Accrued Interest Balance will be used to update in glmast

                // gnInterestBalance = tnPayDiff < 0.00m ? (gnTotalAccruedInterest - Math.Abs(Convert.ToDecimal(maskedTextBox1.Text))) : 0.00m; //Accrued Interest Balance will be used to update in glmast
                if (Math.Abs(Convert.ToDecimal(maskedTextBox1.Text)) > Math.Abs(gnTotalAccruedInterest))
                {
                    if (tnPrinAmt > 0.00m)
                    {
                        principalPayment();
                    }

                    if (tnAccruedCont > 0)    //interest amount payment as per product definition
                    {
                        UPdInterestAmt();
                    }
                }
                else
                {
                    UPdInterestAmtOnly();
                }


            }
            else
            {
                updGlmastDetails();
            }

            if (textBox3.Text != "") //The balance is sent to either the member's selected savings account or loan account
            {
                if (Convert.ToInt32(comboBox2.SelectedValue) > 0)
                {
                    postAccounts(comboBox2.SelectedValue.ToString(), Convert.ToDecimal(textBox3.Text), textBox2.Text);
                }

            }

            if (gcTranCode == "02")
            {
              // MessageBox.Show("This is Withdrawal" + -Math.Abs(tnContAmt));
                if (textBox2.Text.Trim() != null && textBox2.Text.Trim() != string.Empty)
                {
                    updateTemporaryCashBalances(textBox2.Text.Trim(), -Math.Abs(tnContAmt));
                    cashAccount(textBox2.Text.Trim());
                }

            }
            else
            {
                if (textBox2.Text.Trim() != null && textBox2.Text.Trim() != string.Empty)
                {
                //   MessageBox.Show("This is deposit" + Math.Abs(tnTranAmt));
                updateTemporaryCashBalances(textBox2.Text.Trim(), Math.Abs(tnTranAmt));
                cashAccount(textBox2.Text.Trim());
                //if (globalvar.gcCashAccont != null && globalvar.gcCashAccont != string.Empty)
                //{
                //    cashAccount(globalvar.gcCashAccont);
                }
            }
        }
        private void principalPayment()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                string lcStack = GetClient_Code.clientCode_int(cs, "stackcount").ToString().Trim().PadLeft(15, '0');
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, gcControlAcct);
                decimal tnPNewBal12 = CheckLastBalance.lastbalance(cs, gcContraAcct);
                // MessageBox.Show("This is Account Balance" + tnPNewBal1 + gcControlAcct);
                // MessageBox.Show("This is Account Balance" + tnCNewBal + tcContra);
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tcContcode = tcContra.Substring(3, 6);
                int jourtype =  (tcTranCode == "01" ? 2 :  //Deposits
                                (tcTranCode == "02" ? 3 : //Withdrawals
                                (tcTranCode == "07" ? 4 : //Loan Repayment
                                (tcTranCode == "09" ? 5 : //"<< Loan Payout/close >>" :
                                (tcTranCode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));

                SqlDataAdapter payCommand = new SqlDataAdapter();

                string cpatquery = "Exec Tsp_InsertPrincipalPayment ";
                cpatquery       += "@tnCompid, @tcAcctNumb, @tcContra, @tcControlAcct, @tnTranAmt, @tcDesc, @dtrandate, @tcTranCode, @dvaludate,";
                cpatquery       += "@tcVoucher, @tcReceipt, @tcChqno, @tcUserID, @tnPNewBal, @tnCNewBal, @lcStack, @lVerified, @lcCustcode,";
                cpatquery       += "@lcContcode, @lbranchid, @lcurrcode, @lnLoanProcess, @llcjvno, @llcBank, @jtype,@gcWkStation,@gcWinUser,@serv_id,";
                cpatquery       += "@tnPNewBal1, @tnPNewBal12 ";

                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                payCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb; 
                payCommand.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = gcContraAcct;
                payCommand.InsertCommand.Parameters.Add("@tcControlAcct", SqlDbType.Char).Value = gcControlAcct;
                payCommand.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnPrinAmt;
                payCommand.InsertCommand.Parameters.Add("@tnContraAmt", SqlDbType.Decimal).Value = -tnPrinAmt;
                payCommand.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.VarChar).Value = tcDesc.Trim();
                payCommand.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                payCommand.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                payCommand.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = gcReceiptNo;
                payCommand.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = gcUserid;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal", SqlDbType.Decimal).Value = tnPNewBal + tnPrinAmt;
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal", SqlDbType.Decimal).Value = tnCNewBal + -tnPrinAmt;
                payCommand.InsertCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = lcStack;
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
                payCommand.InsertCommand.Parameters.Add("@serv_id", SqlDbType.Decimal).Value = lnServID;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal1", SqlDbType.Decimal).Value = tnPNewBal1 + tnPrinAmt;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal12", SqlDbType.Decimal).Value = tnPNewBal1 + -tnPrinAmt;
                payCommand.InsertCommand.ExecuteNonQuery();
                dcon.Close();
            }
            updateAmort();  // we need to update the amortabl so that it does not become part of the outstanding payments
        }

        private void UPdInterestAmt()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string tcPostTranCode = "17";
                string tcContDesc = "Teller Interest";
                //gls.updGlmast(cs, tcAcctNumb, tnAccruedPost);                              //update accrued interest or interest charged - debit
                decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, tcAcctNumb);
                //tcCustcode = tcAcctNumb.Substring(3, 6);
                //tls1.updCuTranhist(cs, tcAcctNumb, -tnAccruedPost, tcDesc, tcVoucher, tcChqno, gcUserid, tnPNewBal0, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                //auditDesc = "Teller Interest Charged " + tcAcctNumb; //Audit Trail

                //au.upd_audit_trail(cs, auditDesc, 0.00m, -tnAccruedPost, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                string tcContTranCode = "05";
                string tcPostDesc = "Teller Interest ";
                //gls.updGlmast(cs, tcAcctNumb, Math.Abs(gnTotalAccruedInterest));                              //update accrued interest paid  - credit
                decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, tcAcctNumb);
                //tcCustcode = tcAcctNumb.Substring(3, 6);
                //tls1.updCuTranhist(cs, tcAcctNumb, Math.Abs(gnTotalAccruedInterest), tcDesc, tcVoucher, tcChqno, gcUserid, tnPNewBal1, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                //                                                                                                                                                      //we will be updating the audit trail. 
                //auditDesc = "Teller Interest Paid " + tcAcctNumb; //Audit Trail
                //au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(gnTotalAccruedInterest), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                //updateInterestDate(tcAcctNumb);  //update the last interest calculation date and accrued interest balance


                //gls.updGlmast(cs, tcContra, tnAccruedPost);                                //update glmast contra account - debit
                //decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                //tcCustcode = tcContra.Substring(3, 6);
                //tls1.updCuTranhist(cs, tcContra, tnAccruedPost, tcDesc, tcVoucher, tcChqno, gcUserid, tnCNewBal2, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                        //update tranhist account 
                //updateAmort();

                //upj.updJournal(cs, tcContra, tcDesc, -Math.Abs(tnAccruedPost), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                //upj.updJournal(cs, gcIntAcct, tcDesc, Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                //updcl.updClient(cs, "nvoucherno");
                //updcl.updClient(cs, "stackcount");

                dcon.Open();
                string lcStack = GetClient_Code.clientCode_int(cs, "stackcount").ToString().Trim().PadLeft(15, '0');
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);
                decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, gcContraAcct); // 0.00m;
                decimal tnCNewBal1 = CheckLastBalance.lastbalance(cs, gcIntAcct);        // 0.00m;gcIntAcct
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tcContcode = tcContra.Substring(3, 6);
                int jourtype = (tcTranCode == "01" ? 2 :  //Deposits
                                (tcTranCode == "02" ? 3 : //Withdrawals
                                (tcTranCode == "07" ? 4 : //Loan Repayment
                                (tcTranCode == "09" ? 5 : //"<< Loan Payout/close >>" :
                                (tcTranCode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));

                SqlDataAdapter payCommand = new SqlDataAdapter();

                
                 string cpatquery = "Exec Tsp_UPdInterestAmt ";
                cpatquery += "@tnCompid, @tcAcctNumb, @tcContra, @tcControlAcct, @tnTranAmt,@tnContAmt, @tcPostDesc,@tcContDesc, @dtrandate, @tcPostTranCode,@tcContTranCode, @dvaludate,";
                cpatquery += "@tcVoucher, @tcReceipt, @tcChqno, @tcUserID, @tnPNewBal, @tnCNewBal, @lcStack, @lVerified, @lcCustcode,";
                cpatquery += "@lcContcode, @lbranchid, @lcurrcode, @lnLoanProcess, @llcjvno, @llcBank, @jtype ,@dTransactionDate,@taccruedBal,@gcWkStation,@gcWinUser,@serv_id, @tnPNewBal1,@tnCNewBal2  "; //, 

                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                payCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                payCommand.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = gcContraAcct;
                payCommand.InsertCommand.Parameters.Add("@tcControlAcct", SqlDbType.Char).Value = gcIntAcct;
                payCommand.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = -Math.Abs(tnAccruedPost);
                payCommand.InsertCommand.Parameters.Add("@tnContAmt", SqlDbType.Decimal).Value = Math.Abs(gnTotalAccruedInterest);
                payCommand.InsertCommand.Parameters.Add("@tcPostDesc", SqlDbType.VarChar).Value = tcPostDesc;
                payCommand.InsertCommand.Parameters.Add("@tcContDesc", SqlDbType.VarChar).Value = tcContDesc;
                payCommand.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcPostTranCode", SqlDbType.Char).Value = tcPostTranCode;
                payCommand.InsertCommand.Parameters.Add("@tcContTranCode", SqlDbType.Char).Value = tcContTranCode;
                payCommand.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                payCommand.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = gcReceiptNo;
                payCommand.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = gcUserid;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal", SqlDbType.Decimal).Value = tnPNewBal0 ;
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal", SqlDbType.Decimal).Value = tnPNewBal1 ;
                payCommand.InsertCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = lcStack;
                payCommand.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                payCommand.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = tcCustcode;
                payCommand.InsertCommand.Parameters.Add("@lcContcode", SqlDbType.Char).Value = tcCustcode;
                payCommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = globalvar.gnBranchid;
                payCommand.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                payCommand.InsertCommand.Parameters.Add("@lnLoanProcess", SqlDbType.Bit).Value = glLoanProcess;
                payCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                payCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                payCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;
                payCommand.InsertCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@taccruedBal", SqlDbType.Decimal).Value = gnInterestBalance;
                payCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.VarChar).Value = gcWorkSt;
                payCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.VarChar).Value = gcWinUsr;
                payCommand.InsertCommand.Parameters.Add("@serv_id", SqlDbType.Decimal).Value = lnServID;
                
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal1", SqlDbType.Decimal).Value = tnCNewBal1 + Math.Abs(gnTotalAccruedInterest);
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal2", SqlDbType.Decimal).Value = tnCNewBal2 + Math.Abs(gnTotalAccruedInterest);

                payCommand.InsertCommand.ExecuteNonQuery();
                dcon.Close();
            }
            updateAmort();  //we need to update the amortabl so that it does not become part of the outstanding payments
        }

        // If the amount is less than the total interest 

        private void UPdInterestAmtOnly()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                string tcPostTranCode = "17";
                string tcContDesc = "Teller Interest ";
                //gls.updGlmast(cs, tcAcctNumb, tnAccruedPost);                              //update accrued interest or interest charged - debit
                decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, tcAcctNumb);
                //tcCustcode = tcAcctNumb.Substring(3, 6);
                //tls1.updCuTranhist(cs, tcAcctNumb, -tnAccruedPost, tcDesc, tcVoucher, tcChqno, gcUserid, tnPNewBal0, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                //auditDesc = "Teller Interest Charged " + tcAcctNumb; //Audit Trail

                //au.upd_audit_trail(cs, auditDesc, 0.00m, -tnAccruedPost, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                string tcContTranCode = "05";
                string tcPostDesc = "Teller Interest ";
                //gls.updGlmast(cs, tcAcctNumb, Math.Abs(gnTotalAccruedInterest));                              //update accrued interest paid  - credit
                decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, tcAcctNumb);
             //   MessageBox.Show("this is the balance Amount" + tnPNewBal1);
                //tcCustcode = tcAcctNumb.Substring(3, 6);
                //tls1.updCuTranhist(cs, tcAcctNumb, Math.Abs(gnTotalAccruedInterest), tcDesc, tcVoucher, tcChqno, gcUserid, tnPNewBal1, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account
                //                                                                                                                                                      //we will be updating the audit trail. 
                //auditDesc = "Teller Interest Paid " + tcAcctNumb; //Audit Trail
                //au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(gnTotalAccruedInterest), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                //updateInterestDate(tcAcctNumb);  //update the last interest calculation date and accrued interest balance


                //gls.updGlmast(cs, tcContra, tnAccruedPost);                                //update glmast contra account - debit
                decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
              //  MessageBox.Show("this is the balance Contra " + tnCNewBal2);
                //tcCustcode = tcContra.Substring(3, 6);
                //tls1.updCuTranhist(cs, tcContra, tnAccruedPost, tcDesc, tcVoucher, tcChqno, gcUserid, tnCNewBal2, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
                //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                        //update tranhist account 
                //updateAmort();

                //upj.updJournal(cs, tcContra, tcDesc, -Math.Abs(tnAccruedPost), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                //upj.updJournal(cs, gcIntAcct, tcDesc, Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                //updcl.updClient(cs, "nvoucherno");
                //updcl.updClient(cs, "stackcount");

                dcon.Open();
                string lcStack = GetClient_Code.clientCode_int(cs, "stackcount").ToString().Trim().PadLeft(15, '0');
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                decimal tnCNewBal1 = CheckLastBalance.lastbalance(cs, gcIntAcct);        // 0.00m; 
                decimal tnCNewBal3 = CheckLastBalance.lastbalance(cs, gcContraAcct);
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tcContcode = tcContra.Substring(3, 6);
                int jourtype = (tcTranCode == "01" ? 2 :  //Deposits
                                (tcTranCode == "02" ? 3 : //Withdrawals
                                (tcTranCode == "07" ? 4 : //Loan Repayment
                                (tcTranCode == "09" ? 5 : //"<< Loan Payout/close >>" :
                                (tcTranCode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));

                SqlDataAdapter payCommand = new SqlDataAdapter();


                string cpatquery = "Exec Tsp_UPdInterestAmt ";
                cpatquery += "@tnCompid, @tcAcctNumb, @tcContra, @tcControlAcct, @tnTranAmt,@tnContAmt, @tcPostDesc,@tcContDesc, @dtrandate, @tcPostTranCode,@tcContTranCode, @dvaludate,";
                cpatquery += "@tcVoucher, @tcReceipt, @tcChqno, @tcUserID, @tnPNewBal, @tnCNewBal, @lcStack, @lVerified, @lcCustcode,";
                cpatquery += "@lcContcode, @lbranchid, @lcurrcode, @lnLoanProcess, @llcjvno, @llcBank, @jtype ,@dTransactionDate,@taccruedBal,@gcWkStation,@gcWinUser,@serv_id, @tnPNewBal1,@tnCNewBal3 "; //, 

                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                payCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                payCommand.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = gcContraAcct;
                payCommand.InsertCommand.Parameters.Add("@tcControlAcct", SqlDbType.Char).Value = gcIntAcct;
                payCommand.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = -Math.Abs(Convert.ToDecimal(maskedTextBox1.Text));
                payCommand.InsertCommand.Parameters.Add("@tnContAmt", SqlDbType.Decimal).Value = Convert.ToDecimal(maskedTextBox1.Text);
                payCommand.InsertCommand.Parameters.Add("@tcPostDesc", SqlDbType.VarChar).Value = tcPostDesc;
                payCommand.InsertCommand.Parameters.Add("@tcContDesc", SqlDbType.VarChar).Value = tcContDesc;
                payCommand.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcPostTranCode", SqlDbType.Char).Value = tcPostTranCode;
                payCommand.InsertCommand.Parameters.Add("@tcContTranCode", SqlDbType.Char).Value = tcContTranCode;
                payCommand.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                payCommand.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = gcReceiptNo;
                payCommand.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = gcUserid;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal", SqlDbType.Decimal).Value = tnPNewBal1 ;
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal", SqlDbType.Decimal).Value = tnPNewBal1;//tnCNewBal2;
                payCommand.InsertCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = lcStack;
                payCommand.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                payCommand.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = tcCustcode;
                payCommand.InsertCommand.Parameters.Add("@lcContcode", SqlDbType.Char).Value = tcCustcode;
                payCommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = globalvar.gnBranchid;
                payCommand.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                payCommand.InsertCommand.Parameters.Add("@lnLoanProcess", SqlDbType.Bit).Value = glLoanProcess;
                payCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                payCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                payCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;
                payCommand.InsertCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@taccruedBal", SqlDbType.Decimal).Value = gnInterestBalance;

                payCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.VarChar).Value = gcWorkSt;
                payCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.VarChar).Value = gcWinUsr;
                payCommand.InsertCommand.Parameters.Add("@serv_id", SqlDbType.Decimal).Value = lnServID;

                payCommand.InsertCommand.Parameters.Add("@tnPNewBal1", SqlDbType.Decimal).Value = tnCNewBal1 + Convert.ToDecimal(maskedTextBox1.Text);
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal3", SqlDbType.Decimal).Value = tnCNewBal2 + Convert.ToDecimal(maskedTextBox1.Text);

                payCommand.InsertCommand.ExecuteNonQuery();
                dcon.Close();
            }
            updateAmort();  //we need to update the amortabl so that it does not become part of the outstanding payments
            printreceipt(gcCustCode, gcReceiptNo, 1);
        }

        private void updGlmastDetails()
        {
            //gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;

            tcCustcode = tcAcctNumb.Substring(3, 6);
            string lcPostDesc = tcDesc;
            //tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, gcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
            //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                          //update tranhist posting account

            //auditDesc = textBox1.Text.Trim() + " " + tcAcctNumb;// "Loan Disbursement Completed";
            //au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


            //gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
            //upj.updJournal(cs, tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            //decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
            //tcCustcode = tcContra.Substring(3, 6);
            //tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, gcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, gcReceiptNo, llCashpay, visno, isproduct,
            //srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dTranDate.Value, dTranDate.Value);                        //update tranhist account 
            //                                                                                                                                                    //        updateAmort();  //we need to update the amortabl so that it does not become part of the outstanding payments

            //upj.updJournal(cs, gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            //upj.updJournal(cs, tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
            //updcl.updClient(cs, "nvoucherno");
            //updcl.updClient(cs, "stackcount");

            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                string lcStack = GetClient_Code.clientCode_int(cs, "stackcount").ToString().Trim().PadLeft(15, '0');
             //   decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, gcControlAcct);
                decimal tnPNewBal12 = CheckLastBalance.lastbalance(cs, gcContraAcct);
                //  MessageBox.Show("This is Account Balance" + tnPNewBal1 + gcControlAcct);
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tcContcode = tcContra.Substring(3, 6);
                int jourtype = (tcTranCode == "01" ? 2 :  //Deposits
                                (tcTranCode == "02" ? 3 : //Withdrawals
                                (tcTranCode == "07" ? 4 : //Loan Repayment
                                (tcTranCode == "09" ? 5 : //"<< Loan Payout/close >>" :
                                (tcTranCode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));
        
                SqlDataAdapter payCommand = new SqlDataAdapter();

                string cpatquery = "Exec Tsp_UpdateGlMaster ";
                cpatquery += "@tnCompid, @tcAcctNumb, @tcContra, @tcControlAcct, @tnPostAmt, @tnContAmt, @tcTransDesc, @dtrandate, @tcTranCode,@tcTranCode, @dvaludate,";
                cpatquery += "@tcVoucher, @tcReceipt, @tcChqno, @tcUserID, @tnPNewBal, @tnCNewBal, @lcStack, @lVerified, @lcCustcode,";
                cpatquery += "@lcContcode, @lbranchid, @lcurrcode, @lnLoanProcess, @llcjvno, @llcBank, @jtype,  @dtrandate,@gcWkStation,@gcWinUser,@serv_id, @tnPNewBal1, @tnPNewBal12";  //, 

                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                payCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                payCommand.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = gcContraAcct;
                payCommand.InsertCommand.Parameters.Add("@tcControlAcct", SqlDbType.Char).Value = gcControlAcct;
                payCommand.InsertCommand.Parameters.Add("@tnPostAmt", SqlDbType.Decimal).Value = tnTranAmt;
                payCommand.InsertCommand.Parameters.Add("@tnContAmt", SqlDbType.Decimal).Value = tnContAmt;
                // payCommand.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.Char).Value = "dep";// lcPostDesc;
                payCommand.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                payCommand.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dTranDate.Value;
                payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.VarChar).Value = tcVoucher;
                payCommand.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.VarChar).Value =gcReceiptNo;
                payCommand.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.VarChar).Value = tcChqno;
                payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.VarChar).Value =  gcUserid;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal", SqlDbType.Decimal).Value = tnPNewBal + tnTranAmt;
                payCommand.InsertCommand.Parameters.Add("@tnCNewBal", SqlDbType.Decimal).Value = tnCNewBal + tnContAmt;
                payCommand.InsertCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = lcStack;
                payCommand.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                payCommand.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = tcCustcode;
                payCommand.InsertCommand.Parameters.Add("@lcContcode", SqlDbType.Char).Value = tcContcode;
                payCommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = globalvar.gnBranchid;
                payCommand.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                payCommand.InsertCommand.Parameters.Add("@lnLoanProcess", SqlDbType.Bit).Value = glLoanProcess;
                payCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                payCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                payCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;
                payCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.VarChar).Value =  globalvar.gcWorkStation;
                payCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.VarChar).Value =  globalvar.gcWinUser;
                payCommand.InsertCommand.Parameters.Add("@tcTransDesc", SqlDbType.VarChar).Value = lcPostDesc;
                payCommand.InsertCommand.Parameters.Add("@serv_id", SqlDbType.Decimal).Value = lnServID;
                payCommand.InsertCommand.Parameters.Add("@tnPNewBal1", SqlDbType.Decimal).Value = tnPNewBal1 + tnTranAmt;
                 payCommand.InsertCommand.Parameters.Add("@tnPNewBal12", SqlDbType.Decimal).Value = tnPNewBal12 + tnContAmt;
                payCommand.InsertCommand.ExecuteNonQuery();
                dcon.Close();
            }
        }

        // Fee payment
        private void postFee()
        {
           // string tcContra = globalvar.gcIntSuspense;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcDesc = string.Empty;
            string tcCustcode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
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
            //if (tcAcode == "")// ************* for joining fees which will be deducted from the member's savings account and credited to an income account
            //{
            //Member's savings account - debit 
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


        private void printreceipt(string tcCode, string tcReceiptNumber, int tnOrder)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();

                dds.CashReceiptDataTable.Clear();
                string lcReportFolder = getStartupFolder.gcStartUpDirectory;
                string lcReportFile = "CashReceipt.rpt";
                DateTime ldReceiptDate = DateTime.Today;
                int lnRecType = gcTranCode == "02" ? 2 : 1;

               // MessageBox.Show("This is the Compound ID" + ncompid);
               // MessageBox.Show("This is the CCUSCT" + tcCode);
               // MessageBox.Show("This is the Receipt" + tcReceiptNumber);
               // MessageBox.Show("This is the Receipt" + lnRecType);

                string cpatquery = "Exec tsp_ClientReceipt @tnCompid,@tcCustCode,@tcReceiptNo,@tnrecType";
              
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

        private void postAccounts(string tcAcctNumb, decimal lnTranAmt, string ccontra)
        {
            string tcContra = ccontra;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = "Loan Disbursement";
            int tncompid = globalvar.gnCompid;
            string tcCustcode = gcCustCode;
            decimal tnTranAmt = Math.Abs(lnTranAmt);
            decimal tnContAmt = -Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = (radioButton1.Checked ? "000001" : (radioButton2.Checked ? textBox14.Text : textBox16.Text));
            int npaytype = (radioButton1.Checked ? 1 : (radioButton2.Checked ? 2 : 3));
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = radioButton9.Checked ? "01" : "07";    //we will use savings transaction code and loans transaction code depending for the balance
            int lnServID = gnLoanProduct;
            bool llPaid = true;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            bool lFreeBee = false;

            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
            //dbal.updDayBal(cs, dTranDate.Value, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode,dTranDate.Value,dTranDate.Value);                          //update tranhist posting account


            gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
          //  dbal.updDayBal(cs, dTranDate.Value, tcContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
            decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
            globalvar.gcCashAccontBal = tnCNewBal;
            tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode,dTranDate.Value,dTranDate.Value);                        //update tranhist account 396 1756

            updcl.updClient(cs, "nvoucherno");

        }

        private void updateAmort()
        {
            decimal lnPayAmt = 0.00m;
            for(int m=0;m<amortGrid.Rows.Count;m++)
            {
                decimal lnPayment = Convert.ToDecimal(amortGrid.Rows[m].Cells["dpayment"].Value);
                if (lnPayment > 0.00m)
                {
                    if (Convert.ToBoolean(amortGrid.Rows[m].Cells["lpaid"].Value))
                    {
                        lnPayAmt = Convert.ToDecimal(amortGrid.Rows[m].Cells["dpayment"].Value);
                        int gnOrder = Convert.ToInt32(amortGrid.Rows[m].Cells["dorder"].Value);
                        amortOrder(gnOrder, lnPayAmt, true);
                    }
                    else
                    {
                        lnPayAmt = Convert.ToDecimal(amortGrid.Rows[m].Cells["dpayment"].Value);
                        int gnOrder = Convert.ToInt32(amortGrid.Rows[m].Cells["dorder"].Value);
                        if (lnPayAmt > 0.00m)
                        {
                            amortOrder(gnOrder, lnPayAmt, false);
                        }
                    }
                }
            }
        }

        private void amortOrder(int dorder,decimal tnPayAmt,bool tlFull)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = string.Empty; 
                if(tlFull)
                {
                    cglquery = "update amortabl set lpaid=1,nbalance=0.00 where loanid=@loanid and norder=@dorder";
                }
                else
                {
                    cglquery = "update amortabl set nbalance=nbalance-@tnPay where loanid=@loanid and norder=@dorder";
                }
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);

                updCommand.UpdateCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnLoanID;
                updCommand.UpdateCommand.Parameters.Add("@tnPay", SqlDbType.Decimal).Value = tnPayAmt;
                updCommand.UpdateCommand.Parameters.Add("@dorder", SqlDbType.Int).Value = dorder;
 
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void initvariables()
        {
            maskedTextBox1.Text = "";
            textBox5.Text = textBox1.Text;
            textBox22.Text = "";
            textBox23.Text = "";
            textBox14.Text = "";
            textBox11.Text = "";
            textBox13.Text = "";
            textBox101.Text = "";
            textBox10.Text = "";
            textBox16.Text = "";
            textBox12.Text = "";
            textBox3.Text = "";
            textBox21.Text = "";
            loanview.Clear();
            amortview.Clear();
            radioButton9.Checked = false;
            radioButton8.Checked = false;
            comboBox2.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            radioButton1.Checked = true;
            radioButton1.Checked = false;
            radioButton1.Checked = false;
            textBox4.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccontName : "");
            textBox2.Text = (globalvar.gcCashAccont != "" ? globalvar.gcCashAccont : "");
            updcl.updClient(cs, "rec_no");
            gcReceiptNo = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
           // gcReceiptNo = ldToday.Year.ToString().Trim().PadLeft(2, '0') + ldToday.Month.ToString().Trim().PadLeft(2, '0') + GetClient_Code.clientCode_int(cs, "rec_no").ToString().Trim().PadLeft(4, '0');
            textBox15.Text = gcReceiptNo;

            updcl.updClient(cs, "stackcount");
            updcl.updClient(cs, "nvoucherno");
            updcl.updClient(cs, "jv_no");

            int djv0 = GetClient_Code.clientCode_int(cs, "nvoucherno");//  ucc. updateClient_Code
            string djv = string.Empty;
            if (djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "nvoucherno");
                djv0 = 1;
            }
            djv = djv0.ToString().Trim().PadLeft(4, '0');
            tcVoucher = gdToday.Year.ToString().Trim().Substring(0, 2) + gdToday.Month.ToString().Trim() + gdToday.Day.ToString().Trim() + djv.Trim();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TclassLibrary.acctEnquiry dac = new TclassLibrary.acctEnquiry(cs, ncompid, cloca);
            dac.ShowDialog();
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox3.Focused)
            {
                textBox2.Text = comboBox3.SelectedValue.ToString();
                textBox4.Text = comboBox3.Text.ToString();
            }
        }

        private void loanGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            OutstandingAmort(gnLoanID);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton9.Checked)
            {
                getmyAccts(gcCustCode, "250");
            }
            comboBox2.Enabled = radioButton9.Checked ? true : false;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
            {
                getmyAccts(gcCustCode, "130");
            }
            comboBox2.Enabled = radioButton8.Checked ? true : false;
        }

        private void getmyAccts(string tcode, string acode)
        {
            string cglquery = "select cacctname,glmast.cacctnumb from glmast,cusreg where cusreg.ccustcode = @lcTcode and acode=@lcAcode and cusreg.ccustcode = glmast.ccustcode";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(cglquery, nconnHandle);

                selCommand.SelectCommand.Parameters.Add("@lcTcode", SqlDbType.VarChar).Value = tcode;
                selCommand.SelectCommand.Parameters.Add("@lcAcode", SqlDbType.VarChar).Value = acode;
                DataTable myacctview = new DataTable();
                nconnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                nconnHandle.Close();
                selCommand.Fill(myacctview);
                if (myacctview.Rows.Count > 0)
                {
                    comboBox2.DataSource = myacctview.DefaultView;
                    comboBox2.DisplayMember = "cacctnumb";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                } 
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox2.Focused )
            {
                gcBalanceAcct = comboBox2.SelectedValue.ToString();
                AllClear2Go();
            }
        }

        private void loanGrid_Enter(object sender, EventArgs e)
        {
            OutstandingAmort(gnLoanID);
        }

        private void textBox6_Validated(object sender, EventArgs e)
        {
            loanview.Clear();
            if (textBox6.Text.ToString().Trim()!="")
            {
                textBox8.Text = textBox101.Text = "";
                string tcCustCode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
                textBox6.Text = tcCustCode;
                MemberMsg();
               // ClosedMember(tcCustCode, "C");
            }
        }

        private void textBox8_Validated(object sender, EventArgs e)
        {
            if (textBox8.Text.ToString().Trim() != "")
            {
                loanview.Clear();
                textBox6.Text = textBox101.Text = "";
                string tcPayCode = textBox8.Text.ToString().Trim();
                textBox8.Text = tcPayCode;
                ClosedMember1(tcPayCode, "P");
                //getPictures(tcPayCode, "P");
                //getAccounts(gcTranCode, tcPayCode, "P");
            }
        }
        private void ClosedMember1(string searchKey, string searchtype)
        {

            actview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************Getting accounts   
                textBox6.Text = textBox101.Text = "";
                string tcPayCode = textBox8.Text.ToString().Trim();
                textBox8.Text = tcPayCode;
                if (searchtype == "C")
                {
                    string dsql1r = "Select lactive from cusreg where compid =  " + ncompid + " and ccustcode = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(actview);
                }
                else
                {
                    string dsql1r = "Select lactive from cusreg where compid =  " + ncompid + " and payroll_id = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(actview);
                }
                if (actview != null)
                {

                    for (int m = 0; m < actview.Rows.Count; m++)
                    {
                        if (Convert.ToBoolean(actview.Rows[m]["lactive"]))
                        {

                            getPictures(tcPayCode, "P");
                            getAccounts(gcTranCode, tcPayCode, "P");
                        }
                        else
                        {
                            MessageBox.Show("This member is closed");
                            textBox8.Text = "";
                            comboBox4.SelectedValue = -1;
                        };
                    }


                }


                else
                {
                    MessageBox.Show("This Member those not exist!");
                };
            }

        }
        private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox4.Focused)
            {

                int rown1 = Convert.ToInt32(comboBox4.SelectedIndex);
                if (rown1 > -1)
                {
                    string AccountStat = intview.Rows[rown1]["caccstat"].ToString().Trim();

                  

                    if (AccountStat == "I" || AccountStat == "F" || AccountStat == "D") {
                        MessageBox.Show("This account is restricted, please contact the General Manager!");
                    }
                else { 

                        string custcode = intview.Rows[rown1]["ccustcode"].ToString().Trim();
                        string dAcctNumb = intview.Rows[rown1]["cacctnumb"].ToString().Trim();
                        gcAccountNumber = intview.Rows[rown1]["cacctnumb"].ToString().Trim();
                        textBox18.Text = gcAccountNumber;
                        maskedTextBox2.Text = gcAccountNumber;

                        if (gcTranCode != "03" && gcTranCode != "07")
                        {
                            gnSaveProduct = !Convert.IsDBNull(intview.Rows[Convert.ToInt16(comboBox4.SelectedIndex)]["prd_id"]) ? Convert.ToInt16(intview.Rows[rown1]["prd_id"]) : 0;
                            if (gnSaveProduct == 0)
                            {
                                MessageBox.Show("Product type not defined, inform IT Dept");
                                textBox6.Text = textBox8.Text = "";
                                intview.Clear();
                            }
                            else
                            {
                                gcCustCode = custcode;
                                acctdetails(gcCustCode, rown1);
                            }
                        }
                        else
                        {
                            gcCustCode = custcode;
                            acctdetails(gcCustCode, rown1);
                            getAccruedInterest(dAcctNumb);

                        }
                    }
                }
            }
        }


        private void getAccruedInterest(string tcAcctNumb)
        {
            string dasql = "exec tsp_MemberLoanAccounts_A " +ncompid+" ,'"+ tcAcctNumb+"'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    gnLoanDur = Convert.ToInt16(dprodview.Rows[0]["ndur"]);
                    gnLoanAmt = Convert.ToDouble(dprodview.Rows[0]["loanAmt"]);
                    gnPerPay = Convert.ToDouble(dprodview.Rows[0]["repay"]);
                    gdStartDate = Convert.ToDateTime(dprodview.Rows[0]["lintdate"]);
                    gnTotalInterest = Convert.ToDouble(dprodview.Rows[0]["loanint"]);
                    gnLoanInterest = Convert.ToDouble(dprodview.Rows[0]["intrate"]);

                    gnIntScope = Convert.ToInt16(dprodview.Rows[0]["int_scope"]);
                    DateTime dtrandate = Convert.ToDateTime(dTranDate.Value);
                    gnLoanStatus = Convert.ToInt16(dprodview.Rows[0]["LOAN_STATUS"]);

                    DateTime lastInterestDate = Convert.ToDateTime(dprodview.Rows[0]["lintdate"]);
                    decimal nloanBalance = Convert.ToDecimal(dprodview.Rows[0]["nbookbal"]);
                    textBox17.Text = Convert.ToDecimal(dprodview.Rows[0]["repay"]).ToString("N2");
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
                     //   MessageBox.Show("This is status " + gnLoanStatus);
                        if (gnLoanStatus == 5)
                        {
                          //  MessageBox.Show("This is a chargeOff loan");
                            decimal nAccruedInterest = Convert.ToDecimal(dprodview.Rows[0]["accruedinterest"]); //accrued interest from glmast 
                            decimal nloanInterest = Convert.ToDecimal(dprodview.Rows[0]["intrate"]) / (100 * 365m);
                            int numberofdays = (dtrandate - lastInterestDate).Days;
                            string cloanType = dprodview.Rows[0]["loantype"].ToString().Trim();
                            decimal calculatedInterest = nloanBalance * nloanInterest * numberofdays;  //calculated interest
                            decimal chargeoff = 0.00m;
                            gnTotalAccruedInterest = chargeoff + nAccruedInterest;                 //total accrued interest =  calculated interest + balance of accrued interest
                            textBox9.Text = gnTotalAccruedInterest.ToString("N2");
                            loanGrid.AutoGenerateColumns = false;
                            loanGrid.DataSource = dprodview.DefaultView;
                            loanGrid.Columns[0].DataPropertyName = "loantype";
                            loanGrid.Rows[0].Cells[0].Value = Math.Abs(nloanBalance).ToString("N2");
                            loanGrid.Rows[0].Cells[1].Value = Math.Abs(chargeoff).ToString("N2");
                            loanGrid.Rows[0].Cells[2].Value = Math.Abs(nAccruedInterest).ToString("N2");
                        }
                        else
                        {
                            decimal nAccruedInterest = Convert.ToDecimal(dprodview.Rows[0]["accruedinterest"]); //accrued interest from glmast 
                            decimal nloanInterest = Convert.ToDecimal(dprodview.Rows[0]["intrate"]) / (100 * 365m);
                            int numberofdays = (dtrandate - lastInterestDate).Days;
                            string cloanType = dprodview.Rows[0]["loantype"].ToString().Trim();
                            decimal calculatedInterest = nloanBalance * nloanInterest * numberofdays;  //calculated interest
                            gnTotalAccruedInterest = calculatedInterest + nAccruedInterest;                 //total accrued interest =  calculated interest + balance of accrued interest
                            textBox9.Text = gnTotalAccruedInterest.ToString("N2");
                            loanGrid.AutoGenerateColumns = false;
                            loanGrid.DataSource = dprodview.DefaultView;
                            loanGrid.Columns[0].DataPropertyName = "loantype";
                            loanGrid.Rows[0].Cells[0].Value = Math.Abs(nloanBalance).ToString("N2");
                            loanGrid.Rows[0].Cells[1].Value = Math.Abs(calculatedInterest).ToString("N2");
                            loanGrid.Rows[0].Cells[2].Value = Math.Abs(nAccruedInterest).ToString("N2");
                        }
                    }

                }
                else { MessageBox.Show("No loan details found for selected loan account "+tcAcctNumb+", inform IT Dept immediately"); }
            }
        }

        private void getPictures(string searchKey, string searchtype)
        {
           
            pictview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************Getting accounts     
                if (searchtype == "C")
                {
                    string dsql1r = "Select mempict,memsign from cusreg where compid =  " + ncompid + " and ccustcode = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(pictview);
                }
                else
                {
                    string dsql1r = "Select mempict,memsign from cusreg where compid =  " + ncompid + " and payroll_id = '" + searchKey + "'";
                    SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                    da1r.Fill(pictview);
                }
                if (pictview != null)
                {
                    if (!Convert.IsDBNull(pictview.Rows[0]["mempict"]))
                    {
                        byte[] myimg = (byte[])pictview.Rows[0]["mempict"];
                        if (myimg != null)
                        {
                            MemoryStream ms = new MemoryStream(myimg);
                            ms.Position = 0;
                            Bitmap bm = new Bitmap(ms);
                            pictBox.Image = bm;
                        }
                    }
                    if (!Convert.IsDBNull(pictview.Rows[0]["memsign"]))
                    {
                        byte[] mysig = (byte[])pictview.Rows[0]["memsign"];
                        if (mysig != null)
                        {
                            MemoryStream al = new MemoryStream(mysig);
                            al.Position = 0;
                            Bitmap bms = new Bitmap(al);
                            signBox.Image = bms;
                        }
                    }
                }
            }
        }


        private void clearTrans()
        {
             string tcCustCode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
           // MessageBox.Show("This is the Ccustcode ALA" + tcCustCode);

            clearview.Clear();
            using (SqlConnection ndConnHandle2 = new SqlConnection(cs))
            {
                ndConnHandle2.Open();
              //  if (searchtype == "C")
               // {
                
                    string dsqll = "exec tsp_clearTransaction " + 30 + ",'" + tcCustCode + "'";
                    SqlDataAdapter dal = new SqlDataAdapter(dsqll, ndConnHandle2);
                    dal.Fill(clearview);
                    if (clearview.Rows.Count > 0)
                    {
                    textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2");  //Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2");
                    if (Convert.ToDecimal(clearview.Rows[0]["totgran"]) > 0.00m && gcTranCode == "02")
                    {
                        if (MessageBox.Show("This member is guranting an amount of " + textBox20.Text, "Proceed ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {             
                         getPictures(tcCustCode, "C");
                         getAccounts(gcTranCode, tcCustCode, "C");

                        if (Convert.ToString(clearview.Rows[0]["acode"]) == "250")
                        {
                            // ala = Convert.ToDouble(loanview.Rows[0]["loanamt"]);//Convert.ToDouble(clearview.Rows[0]["nbookbal"]);
                            textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2");  //Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2");
                        }
                        else if (Convert.ToString(clearview.Rows[0]["acode"]) == "130")
                        {
                            textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2"); 
                        }

                        }
                        //else
                        //{

                        //    //getPictures(tcCustCode, "C");
                        //    //getAccounts(gcTranCode, tcCustCode, "C");

                        //    if (Convert.ToString(clearview.Rows[0]["acode"]) == "250")
                        //    {
                        //        // ala = Convert.ToDouble(loanview.Rows[0]["loanamt"]);//Convert.ToDouble(clearview.Rows[0]["nbookbal"]);
                        //        textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2");  //Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2");
                        //    }
                        //    else if (Convert.ToString(clearview.Rows[0]["acode"]) == "130")
                        //    {
                        //        textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2");
                        //    }

                        //}
                        {

                        }
                    }

                    getPictures(tcCustCode, "C");
                    getAccounts(gcTranCode, tcCustCode, "C");

                    if (Convert.ToString(clearview.Rows[0]["acode"]) == "250")
                    {
                        // ala = Convert.ToDouble(loanview.Rows[0]["loanamt"]);//Convert.ToDouble(clearview.Rows[0]["nbookbal"]);
                        textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2");  //Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2");
                    }
                    else if (Convert.ToString(clearview.Rows[0]["acode"]) == "130")
                    {
                        textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["totgran"]).ToString("N2");
                    }
                }
                   // else { MessageBox.Show("No Outstanding Loans for Account number " + tcCustCode); }

                        // 
             
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ReprintForm rpf = new ReprintForm();
            rpf.ShowDialog();
        }

        //private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        //{
        //    if (comboBox4.Focused)
        //    {
        //        //                int rown1 = Convert.ToInt32(comboBox4.SelectedIndex)>-1 ? Convert.ToInt32(comboBox4.SelectedIndex) : 0;
        //        int rown1 = Convert.ToInt32(comboBox4.SelectedIndex);
        //        MessageBox.Show("step 1 and rown1 is " + rown1);
        //        if(rown1 > -1)
        //        {
        //            string custcode = intview.Rows[rown1]["ccustcode"].ToString().Trim();
        //            string dAcctNumb = intview.Rows[rown1]["cacctnumb"].ToString().Trim();
        //            MessageBox.Show("step 2");

        //            if (gcTranCode != "07")
        //            {
        //                MessageBox.Show("before save product");
        //                gnSaveProduct = Convert.ToInt16(intview.Rows[rown1]["save_prod"]);
        //            }
        //            else
        //            {
        //                MessageBox.Show("before acrued interest");
        //                getAccruedInterest(dAcctNumb);
        //            }
        //            gcCustCode = custcode;
        //            MessageBox.Show("before acct details");
        //            acctdetails(gcCustCode, rown1);
        //        }
        //    }
        //}



        /*
                                 byte[] myimg = (byte[])memberview.Rows[0]["memPict"];
                        if (myimg != null)
                        {
                            MemoryStream ms = new MemoryStream(myimg);
                            ms.Position = 0;
                            Bitmap bm = new Bitmap(ms);
                            pictureBox3.Image = bm;
         */

        private void checkamort(int loanid)
        {
            string dsql = "select 1 from amortabl where loanid = " + loanid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daa = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable chkamortview = new DataTable();
                daa.Fill(chkamortview);
                if (chkamortview != null && chkamortview.Rows.Count == 0)
                {
                    loanAmortization(loanid);
                    insertAmort();
                }
                else
                {
                    OutstandingAmort(loanid);
                }
            }
        }

        private void loanAmortization(int loanid)
        {

            amortview.Clear();
            int nyrpay = 12;            //this is the number of payments per year
            double gnOrigLoan = Math.Round(gnLoanAmt, 2);
            double gnCumuInt = 0.00;
            double totalprin = 0.00;
            double totalint = 0.00;
            double nintpay = 0.00;
            double nprinpay = 0.00;
            double nfactor = gnLoanAmt / gnLoanDur;
            double nPrincipal = gnLoanAmt;// - nfactor;
            double AnnInt = Convert.ToDouble(gnLoanInterest); // clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);
            double nPeriodicInt = Math.Pow(1 + (AnnInt / 100), 1.0 / 12) - 1;

            for (int j = 1; j <= gnLoanDur; j++)
            {
                double newrate = AnnInt / 100 / nyrpay;
                if (gnLoanInterest > 0.00)
                {
                    gnPerPay = loanCalculation.pmt(newrate, gnLoanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
                    nintpay = loanCalculation.ipmt(newrate, j, gnLoanDur, gnLoanAmt, 0.00, 0); // Interest portion of the Fixed periodic payment
                    nprinpay = loanCalculation.ppmt(newrate, j, gnLoanDur, gnLoanAmt, 0.00, 0);   // Principal payment of the periodic payment 
                }
                else
                {
                    gnPerPay = gnLoanAmt / gnLoanDur;// loanCalculation.pmt(newrate, loanDur, gnLoanAmt, 0.00, 0);  // Fixed Periodic Payment
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

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Check whether the Loan amount is > than the Savings
        //private void clearTrans(string searchKey, string searchtype)
        //{
        //    string tcCustCode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
        //    MessageBox.Show("This is the Ccustcode" + searchKey);
        //    string dsqll = "exec tsp_clearTransaction " + 30 + ",'" + searchKey + "'";
        //    clearview.Clear();
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        ndConnHandle.Open();
        //        SqlDataAdapter dal = new SqlDataAdapter(dsqll, ndConnHandle);
        //        dal.Fill(loanview);
        //        if (clearview.Rows.Count > 0)
        //        {
        //            getPictures(tcCustCode, "C");
        //            getAccounts(gcTranCode, tcCustCode, "C");


        //            if (Convert.ToString(clearview.Rows[0]["acode"]) == "250")
        //            {

        //                // ala = Convert.ToDouble(loanview.Rows[0]["loanamt"]);//Convert.ToDouble(clearview.Rows[0]["nbookbal"]);
        //                textBox19.Text = Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2"); ;
        //            }
        //            else if (Convert.ToString(clearview.Rows[0]["acode"]) == "130")
        //            {
        //                textBox20.Text = Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2"); ;
        //            }
        //         }
        //        else { MessageBox.Show("No Outstanding Loans for Account number " + searchKey); }
        //    }
        //}
        private void MemberMsg()
        {
            string tcCustCode = textBox6.Text.ToString().Trim().PadLeft(6, '0');
            // MessageBox.Show("This is the Ccustcode ALA" + tcCustCode);

            clearview.Clear();
            using (SqlConnection ndConnHandle2 = new SqlConnection(cs))
            {
                ndConnHandle2.Open();
                //  if (searchtype == "C")
                // {

                string dsqll = "exec tsp_getMembersMsg " + 30 + ",'" + tcCustCode + "'";
                SqlDataAdapter dal = new SqlDataAdapter(dsqll, ndConnHandle2);
                dal.Fill(clearview);
                if (clearview.Rows.Count > 0)
                {
                    textBox21.Text = clearview.Rows[0]["ctel"].ToString();
                    textBox19.Text = clearview.Rows[0]["MemberMsg"].ToString();  //Convert.ToDecimal(clearview.Rows[0]["nbookbal"]).ToString("N2");
                    if (clearview != null && textBox19.Text != "")
                    {
                        if (MessageBox.Show(" " + textBox19.Text, "Proceed ", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {

                            ClosedMember(tcCustCode, "C");
                         //   InactivedMember(tcCustCode, "I");

                        }

                    }
                    ClosedMember(tcCustCode, "C");
                //    InactivedMember(tcCustCode, "I");
                }
                else {
                    MessageBox.Show("No record found!");
                     }

                // 

            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void sms()
        {
            //  MessageBox.Show("You are inside Ok 1");
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
           // string tcDesc = "Loan Disbursement";
            decimal balance = tnPNewBal;
           // double balance = Convert.ToDouble(textBox101.Text) + Convert.ToDouble(maskedTextBox1.Text);
           // MessageBox.Show("This is the Balabce 1" + balance);
            string messages = "" + textBox5.Text.Trim() + ": GMD" + maskedTextBox1.Text.Trim() + " " + textBox5.Text.Trim() + " into your account at "+ globalvar.gcComname + " on the " + dTranDate.Text.Trim()+". Your new Balance is GMD"+ balance + "";
            string PhoneNumber = textBox21.Text.Trim();
            var client = new RestClient("https://alchemytelco.com:9443/api?action=sendmessage&originator=NACCUG&username=naccug&password=n@ccu9&recipient=PhoneNumber&messagetype=SMS:TEXT&messagedata=messages");
            client.Timeout = -1;
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddParameter("messagedata", messages);
            requestPost.AddParameter("recipient", PhoneNumber);
            IRestResponse response = client.Execute(requestPost);
            string rawResponse = response.Content.Trim();
            int nStatusCode = (int)response.StatusCode;
         //  MessageBox.Show("This is STatus Code 2 ALA " + nStatusCode);
            string lcStatusCode = response.StatusDescription.ToString().Trim();
          //  MessageBox.Show("This is STatus Code 3 " + nStatusCode);
            //   string lcStatusDesc = errorMessages(nStatusCode.ToString().Trim().PadLeft(2, '0'));
            if (response.StatusCode == HttpStatusCode.OK)
            {
              //  MessageBox.Show("This is STatus Code 4 " + nStatusCode);
                postFee();
            }

        }

        private static string errorMessages(string tcStatusCode)
        {
            tcStatusCode = tcStatusCode.PadLeft(2, '0');
            string retValue = string.Empty;
            retValue =
                        tcStatusCode == "0" ? "Approved or completed successfully " :
                        tcStatusCode == "2" ? " Not honor" : "Undefined ";
            return retValue;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            //int x = comboBox5.SelectedIndex;
            //string j = comboBox5.SelectedValue.ToString();
            //var y = comboBox5.Items[x];

            //textBox1.DataBindings.Clear();
            //textBox2.DataBindings.Clear();
            //textBox3.DataBindings.Clear();
            //textBox4.DataBindings.Clear();

            //textBox1.DataBindings.Add("Text", comboBox1.SelectedItem, "gcServername");
            //globalvar.gcServername = textBox1.Text;
            //textBox2.DataBindings.Add("Text", comboBox1.SelectedItem, "gcDatabaseName");
            //globalvar.gcDatabaseName = textBox2.Text;
            //textBox3.DataBindings.Add("Text", comboBox1.SelectedItem, "gcsusername");
            //globalvar.gcsusername = textBox3.Text;
            //textBox4.DataBindings.Add("Text", comboBox1.SelectedItem, "gcspassword");
            //globalvar.gcspassword = textBox4.Text;
            //if (comboBox5.Focused)
            //{
            //    textBox22.Text = "";
            //    DataRow dr = feeView.Rows[comboBox5.SelectedIndex];
            //    //   textBox22.Text = feeView.Rows[comboBox5.SelectedIndex]["feeamount"].ToString();
            //    textBox22.Text = Convert.ToDecimal(feeView.Rows[comboBox5.SelectedIndex]["feeamount"]).ToString("N2");
            //    textBox23.Text = feeView.Rows[comboBox5.SelectedIndex]["feeIncome"].ToString();
            //    // textBox23.Text = gcFeeAccount;
            //}
        }

        private void comboBox5_SelectedValueChanged_1(object sender, EventArgs e)
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

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }



        //private void sms()
        //{
        //    // Find your Account Sid and Token at twilio.com / console
        //    //  DANGER!This is insecure.See http://twil.io/secure
        //    const string accountSid = "AC25bde79e64a93e1844baddb3a5a77608";
        //    const string authToken = "49b2e50d4eca4870a2f44dd38ec7eb9b";

        //    TwilioClient.Init(accountSid, authToken);
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        //    var message = MessageResource.Create(
        //        body: "Please find your new PIN number '" + TextBox2.Text + "'",
        //        from: new Twilio.Types.PhoneNumber("+12513129241"),
        //        to: new Twilio.Types.PhoneNumber("+2202790316")
        //    );

        //    Console.WriteLine(message.Sid);

        //}
    }
}

