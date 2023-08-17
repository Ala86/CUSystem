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
    public partial class InterestCalculation : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string gcLocalCaption = globalvar.cLocalCaption;
        DateTime gdTodayDate = globalvar.gdSysDate;
        string gcUserID = globalvar.gcUserid;
        int gnBranch = globalvar.gnBranchid;
        DataTable minbalview = new DataTable();
        DataTable intcalview = new DataTable();
        DataTable intappview = new DataTable();
        DataTable clientview = new DataTable();
        decimal gnInterestRate = 0.00m;
        string gcSavingsControl = string.Empty;
        string gcSavingsExpense = string.Empty;
        int gnSaveProduct = 0;
        bool glProductDeduction = false;
        bool gldestype = false;
        string gcProductDesc = string.Empty;
        public InterestCalculation()
        {
            InitializeComponent();
        }

        private void InterestCalculation_Load(object sender, EventArgs e)
        {
            this.Text = gcLocalCaption + "<< Interest Calculation >>";
            getclientList();
            clientgrid.Columns["intRate"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["intRate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            clientgrid.Columns["dmainCat"].SortMode = DataGridViewColumnSortMode.NotSortable;
            clientgrid.Columns["dmainCat"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

        }


        private void Calc_Loan_EMI(double Principal_Amount, double Term_Months, double Intrest_Rate)
        {
            Intrest_Rate = Intrest_Rate / 1200;
            double Years = Term_Months / 12;
            double Payable_Amount = Principal_Amount * Intrest_Rate / (1 - (Math.Pow(1 / (1 + Intrest_Rate), Term_Months)));
            double Total_Amount = Payable_Amount * Term_Months;
            double Total_Interest = Total_Amount - Principal_Amount;
            double Yearly_Interest = Total_Interest / Years;
            double Interest_PA = Yearly_Interest / Principal_Amount * 100;
            double Interest_PM = (Yearly_Interest / Principal_Amount * 100) / 12;


            //                labelmonthlyemi.Text = Payable_Amount.ToString("N3");
            //              labeltotamountwithinterset.Text = Total_Amount.ToString("N3");
            //            labelinterestpa.Text = Interest_PA.ToString("N3");
            //          labelinterestpm.Text = Interest_PM.ToString("N3");
            //        labeltotinterest.Text = Total_Interest.ToString("N3");
            //      labelyearlyintersest.Text = Yearly_Interest.ToString("N3");
        }

        private void getclientList()
        {
            string dsql = "exec tsp_GetProducts  " + ncompid;
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
                    clientgrid.Columns[0].DataPropertyName = "selprd";
                    clientgrid.Columns[1].DataPropertyName = "maincat";
                    clientgrid.Columns[2].DataPropertyName = "prd_name";
                    clientgrid.Columns[3].DataPropertyName = "int_rate";
                    clientgrid.Columns[4].DataPropertyName = "int_sco";
                    clientgrid.Columns[5].DataPropertyName = "int_method";
                    clientgrid.Columns[6].DataPropertyName = "prd_mand";
                    clientgrid.Columns[7].DataPropertyName = "prd_scope";
                    //clientgrid.Columns[7].DataPropertyName = "loanstart_date";
                    //clientgrid.Columns[8].DataPropertyName = "totinterest";
                    //clientgrid.Columns[9].DataPropertyName = "loan_interest";
                    //clientgrid.Columns[10].DataPropertyName = "loanacct";


                    //int loanDur = Convert.ToInt32(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loandur"]);

                    //gnLoanAmt = Convert.ToDouble(clientgrid.CurrentRow.Cells["loanAmt"].Value);
                    //gnPerPay = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["payamt"]);
                    //gdStartDate = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loanstart_date"]);
                    //double nTotalInterest = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["totinterest"]);
                    //double gnLoanInterest = Convert.ToDouble(clientview.Rows[clientgrid.CurrentCell.RowIndex]["loan_interest"]);


                    ndConnHandle.Close();
                    //for (int i = 0; i < 1; i++)
                    //{
                    //    clientview.Rows.Add();
                    //}
                    clientgrid.Focus();
                }
            }
        }//end of getclientlist


        private void Calc_Amortization(double loanAmt, double Term_Months, double interestRate, double Installment_Number, double monthValue, double yearValue)
        {
            double interestRateForMonth = interestRate / 12; // (Monthly Rate of Interest in %)
            double interestRateForMonthFraction = interestRateForMonth / 100; // (Monthly Interest Rate expressed as a fraction)
            double emi = calculateEMI(loanAmt, interestRate, Term_Months);


            var loanOustanding = loanAmt;
            double totalPayment = 0;
            double totalInterestPortion = 0;
            double totalPrincipal = 0;
            string installmentDate = string.Empty;
            double interestPortion = 0, principal = 0;


            List<CLS_AMORTIZATION> listamort = new List<CLS_AMORTIZATION>();
            double month = 0, year = 0;


            if (Installment_Number > Term_Months || Installment_Number == 0)
            {
                //The Installment must be less than or equal to the Tenure
            }
            else
            {
                for (int i = 1; i <= Term_Months; i++)
                {
                    CLS_AMORTIZATION obj = new CLS_AMORTIZATION();


                    if (monthValue != 0)
                    {
                        month = monthValue + i - 1;
                    }
                    else
                    {
                        month = month + 1;
                    }


                    if (month > 12)
                    {
                        year = yearValue + 1;
                        yearValue = year;
                        monthValue = 0;
                        month = monthValue + 1;
                    }
                    else
                    {
                        year = yearValue;
                    }


                    if (month < 10)
                    {
                        installmentDate = "0" + month + "/" + year;
                    }
                    else
                    {
                        installmentDate = month + "/" + year;
                    }


                    if (loanOustanding == loanAmt)
                    {
                        loanOustanding = loanAmt;


                        obj.INSTALLMENTNO = i.ToString();
                        obj.INSTALLMENTDATE = installmentDate;
                        obj.OPENINGBALANCE = loanOustanding.ToString();
                        obj.EMI = emi.ToString();


                        totalPayment = totalPayment + emi;
                        interestPortion = loanOustanding * interestRateForMonthFraction;
                        interestPortion = roundDecimals(interestPortion, 0);
                    }
                    else
                    {
                        obj.INSTALLMENTNO = i.ToString();
                        obj.INSTALLMENTDATE = installmentDate;
                        obj.OPENINGBALANCE = loanOustanding.ToString();
                        obj.EMI = emi.ToString();


                        totalPayment = totalPayment + emi;
                        interestPortion = loanOustanding * interestRateForMonthFraction;
                        interestPortion = roundDecimals(interestPortion, 0);
                    }


                    loanOustanding = loanOustanding + interestPortion - emi;
                    loanOustanding = roundDecimals(loanOustanding, 0);


                    obj.LOANOUTSTANDING = loanOustanding.ToString();
                    obj.INTEREST = interestPortion.ToString();


                    totalInterestPortion = totalInterestPortion + interestPortion;
                    principal = roundDecimals(emi - interestPortion, 0);


                    obj.PRINCIPAL = principal.ToString();


                    totalPrincipal = totalPrincipal + principal;


                    listamort.Add(obj);
                }


                //        dataGridView1.DataSource = listamort;
            }
        }


        private double calculateEMI(double loanAmt, double interestRate, double tenure)
        {
            if (interestRate != 0)
            {
                double interestRateForMonth = interestRate / 12; // (Monthly Rate of Interest in %)
                double interestRateForMonthFraction = interestRateForMonth / 100; // (Monthly Interest Rate expressed as a fraction)
                double emi = 1 / Math.Pow((1 + interestRateForMonthFraction), tenure);
                double emiPerLakh = (loanAmt * interestRateForMonthFraction) / (1 - emi); // (EMI per lakh borrowed)
                emiPerLakh = roundDecimals(emiPerLakh, 0);
                return emiPerLakh;
            }
            else
            {
                double emi = loanAmt / tenure;
                double emiPerLakh = roundDecimals(emi, 0);
                return emiPerLakh;
            }
        }


        private double roundDecimals(double original_number, int decimals)
        {
            double result1 = original_number * Math.Pow(10, decimals);
            double result2 = Math.Round(result1);
            double result3 = result2 / Math.Pow(10, decimals);


            return (result3);
        }


        public class CLS_AMORTIZATION
        {
            public string INSTALLMENTNO { get; set; }
            public string INSTALLMENTDATE { get; set; }
            public string OPENINGBALANCE { get; set; }
            public string EMI { get; set; }
            public string LOANOUTSTANDING { get; set; }
            public string INTEREST { get; set; }
            public string PRINCIPAL { get; set; }
        }


        private void eMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  groupBoxcalcemi.Visible = true;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }


        private void buttonCalculateemi_Click(object sender, EventArgs e)
        {
            try
            {
                //    double Loan_Amt = Convert.ToDouble(textBoxloanamount.Text), Tenture = Convert.ToDouble(textBoxtenture.Text), Interest = Convert.ToDouble(textBoxInterestRate.Text);
                //     Calc_Loan_EMI(Loan_Amt, Tenture, Interest);
                //   Calc_Amortization(Loan_Amt, Tenture, Interest, 1, DateTime.Now.Month, DateTime.Now.Year);
                //    groupBoxLoandetails.Visible = true;
                //  groupBoxrepaydetails.Visible = true;
            }
            catch (Exception ex)
            {
                //     groupBoxLoandetails.Visible = false;
                //   groupBoxrepaydetails.Visible = false;
                MessageBox.Show("Sorry! there is a error: " + ex.Message);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resetMinBal();
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                int startMonth = dateTimePicker1.Value.Month;
                int endMonth = dateTimePicker2.Value.Month;
                int numMonths = endMonth - startMonth;
                decimal tnMinBal = 0.00m;
                string dsql2 = "select cacctnumb from glmast  where intcode=0 and acode in ('250','251')";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, nConnHandle2);
                da2.Fill(minbalview);
                if (minbalview != null && minbalview.Rows.Count > 0)
                {
                    int dcounter = 0;
                    int j = 0;
                    int linecount = minbalview.Rows.Count;
                    for (int i = 0; i < minbalview.Rows.Count; i++)
                    {
                        j = i + 1;
                        progressBar1.Value = j * progressBar1.Maximum / linecount;
                        string dacct = minbalview.Rows[i]["cacctnumb"].ToString();
                        for (int z = 0; z <= numMonths; z++)
                        {
                            tnMinBal = tnMinBal + calcminBal(dacct, startMonth+z);
                            updateMinBal(dacct, tnMinBal);
                        }
//                        MessageBox.Show("Minimum balance for " + dacct + " = " + tnMinBal);
                        updateMinBal(dacct, tnMinBal);
                        dcounter++;
                    }
                    checkBox4.Checked = true;
                    MessageBox.Show("Minimum Balance Calculation Successful");
                    progressBar1.Value = 0;
                }
            }
        }


        private void resetMinBal()
        {
            string dsql = "update glmast set nminbal=0.00 where acode in ('250','251') ";
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(dsql, nConnHandle2);
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();//   insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private decimal calcminBal(string tcAct,int dmonth)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string minbalsql = " select nminbal = case when min(nnewbal) is not null then min(nnewbal) else 0.00 end from tranhist where cacctnumb = @dact AND MONTH(dpostdate) = @dmth";
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(minbalsql, nConnHandle2);

                selCommand.SelectCommand.Parameters.Add("@dact", SqlDbType.VarChar).Value = tcAct;
                selCommand.SelectCommand.Parameters.Add("@dmth", SqlDbType.Int).Value = dmonth;
                DataTable balview = new DataTable();
                nConnHandle2.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                nConnHandle2.Close();
                selCommand.Fill(balview);
                if (balview != null && balview.Rows.Count > 0)
                {
                    decimal dminbal = Convert.ToDecimal(balview.Rows[0]["nminbal"]);
//                    MessageBox.Show("The minimum balance for month " + dmonth + ", account " + tcAct + " = " + dminbal);
                    return dminbal;
                }
                else { MessageBox.Show("something is wrong"); return 0.00m; }
            }
        }


        private void updateMinBal(string tcAcct, decimal tnMinBal)
        {
            string dsql = "update glmast set nminbal = @dminbal where cacctnumb=@dAcct";
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(dsql, nConnHandle2);
                updCommand.UpdateCommand.Parameters.Add("@dAcct", SqlDbType.VarChar).Value = tcAcct;
                updCommand.UpdateCommand.Parameters.Add("@dminbal", SqlDbType.Decimal).Value = tnMinBal;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();//   insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            intcalview.Clear();
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string dsql20 = "select cacctnumb from glmast where intcode=0 and acode in ('250','251','270','271')";
                SqlDataAdapter da20 = new SqlDataAdapter(dsql20, nConnHandle2);
                da20.Fill(intcalview);
                if (intcalview != null && intcalview.Rows.Count > 0)
                {
                    int dcounter = 0;
                    int j = 0;
                    int linecount = intcalview.Rows.Count;
                    for (int i = 0; i < intcalview.Rows.Count; i++)
                    {
                        j = i + 1;
                        progressBar1.Value = j * progressBar1.Maximum / linecount;
                        string dacct = intcalview.Rows[i]["cacctnumb"].ToString();
                        getIntrate();
                        updAccruedInterest(dacct, gnInterestRate);
                        dcounter++;
                    }
                    checkBox5.Checked = true;
                    MessageBox.Show("Interest Calculation Successful");
                    progressBar1.Value = 0;
                }
                else
                {
                    MessageBox.Show("We have not found any Savings Accounts");
                }
            }
        }

        private void getIntrate()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string dsql21 = "select int_rate, sexp_acc from prodtype where prd_id=5";
                SqlDataAdapter da21 = new SqlDataAdapter(dsql21, nConnHandle2);
                DataTable rateview = new DataTable();
                da21.Fill(rateview);
                if (rateview != null && rateview.Rows.Count > 0)
                {
                    gnInterestRate = Convert.ToDecimal(rateview.Rows[0]["int_inc"]);
                    gcSavingsControl = rateview.Rows[0]["int_acc"].ToString();
                    gcSavingsExpense = rateview.Rows[0]["sexp_acc"].ToString();
                    textBox4.Text = gcSavingsExpense;
                }
                else { MessageBox.Show("nothing of interest found"); }
            }
        }

        private void updAccruedInterest(string tcAcct, decimal tnIntestRate)
        {
            string dsql = "update glmast set AccruedInterest = (nminbal * @tnIntRate)/1200 where cacctnumb=@dAcct";
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(dsql, nConnHandle2);
                updCommand.UpdateCommand.Parameters.Add("@dAcct", SqlDbType.VarChar).Value = tcAcct;
                updCommand.UpdateCommand.Parameters.Add("@tnIntRate", SqlDbType.Decimal).Value = tnIntestRate;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            intcalview.Clear();
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                if(glProductDeduction)          //This code has been moved to Periodic Dues Calculation/Application
                {
                    string dsouProd = clientview.Rows[clientgrid.CurrentCell.RowIndex]["souproduct"].ToString().Trim();
                    gldestype = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["destype"]);
                    string cdesacct = clientview.Rows[clientgrid.CurrentCell.RowIndex]["desacct"].ToString();
                    int nfreq = Convert.ToInt16(clientview.Rows[clientgrid.CurrentCell.RowIndex]["freq"]);
                    bool ldedtype = Convert.ToBoolean(clientview.Rows[clientgrid.CurrentCell.RowIndex]["dedtype"]);
                    decimal ndedamt = Convert.ToDecimal(clientview.Rows[clientgrid.CurrentCell.RowIndex]["dedamt"]);

                    string dsql201 = "exec tsp_GetDeductAccts " + ncompid + ",'" + dsouProd + "'," + "'" + cdesacct + "'"; 
                    SqlDataAdapter da201 = new SqlDataAdapter(dsql201, nConnHandle2);
                    DataTable deductview = new DataTable();
                    da201.Fill(deductview);
                    if (deductview != null && deductview.Rows.Count > 0)
                    {
                        int dcounter = 0;
                        int j = 0;
                        int linecount = deductview.Rows.Count;
                        for (int i = 0; i < deductview.Rows.Count; i++)
                            {
                            j = i + 1;
                            progressBar1.Value = j * progressBar1.Maximum / linecount;
                            string dSourAcct = deductview.Rows[i]["sourAcct"].ToString();
                            string dDestAcct = gldestype ? deductview.Rows[i]["destAcct"].ToString() : cdesacct;
                            postAccounts(dSourAcct, ndedamt,dDestAcct);
                            dcounter++;
                        }
                        MessageBox.Show("Deduction Application Successful");
                        progressBar1.Value = 0;
                    }
                }
                else
                {
                    getIntrate();
                    if (gcSavingsExpense != "")
                    {
                        updateJournal upj = new updateJournal();
                        string dsql20 = "select cacctnumb,AccruedInterest from glmast where AccruedInterest >0.00 ";
                        SqlDataAdapter da20 = new SqlDataAdapter(dsql20, nConnHandle2);
                        da20.Fill(intcalview);
                        if (intcalview != null && intcalview.Rows.Count > 0)
                        {
                            int dcounter = 0;
                            int j = 0;
                            decimal tnTotalAccrued = 0.00m;
                            int linecount = intcalview.Rows.Count;
                            for (int i = 0; i < intcalview.Rows.Count; i++)
                            {
                                j = i + 1;
                                progressBar1.Value = j * progressBar1.Maximum / linecount;
                                string dacct = intcalview.Rows[i]["cacctnumb"].ToString();
                                decimal dAccruuedAmt = Convert.ToDecimal(intcalview.Rows[i]["AccruedInterest"]);
                                postAccounts(dacct, dAccruuedAmt, gcSavingsExpense);
                                tnTotalAccrued = tnTotalAccrued + dAccruuedAmt;
                                dcounter++;
                            }
                            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                            string tcDesc = glProductDeduction ? gcProductDesc : "Savings Interest Application";
                            string tcTranCode = glProductDeduction ? (gldestype ? "24" : "14") : "04";

                            upj.updJournal(cs, gcSavingsControl, tcDesc, Math.Abs(tnTotalAccrued), tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                            upj.updJournal(cs, gcSavingsExpense, tcDesc, -Math.Abs(tnTotalAccrued), tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);  

                            checkBox6.Checked = true;
                            MessageBox.Show("Interest Application Successful");
                            resetMinAccrBal();
                            progressBar1.Value = 0;
                        }
                        else
                        {
                            MessageBox.Show("Interest not accrued or already applied");
                        }

                    }
                    else { MessageBox.Show("Savings Expense Account not found , inform IT DEPT immediately "); }
                }
            }
        }

        private void postAccounts(string tcAcctNumb, decimal lnTranAmt, string ccontra)
        {
            string tcContra = ccontra;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = glProductDeduction ? gcProductDesc : "Savings Interest Application";
            int tncompid = globalvar.gnCompid;
            string tcCustcode = tcAcctNumb.Substring(3, 6);
            decimal tnTranAmt = glProductDeduction ? -Math.Abs(lnTranAmt) : Math.Abs(lnTranAmt);
            decimal tnContAmt = glProductDeduction ? Math.Abs(lnTranAmt) : -Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = "000001";
            int npaytype = 1;
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = glProductDeduction ? (gldestype ? "24" : "14")  : "04";
            int lnServID = 5;   //this will be parameterized later gnSaveProduct;
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


            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
                                                                                   //            dbal.updDayBal(cs, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account
                                                                                                                                                                           //            updateJournal(gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode);
            string auditDesc = glProductDeduction ? gcProductDesc+" Applied -> " : "Savings Interest Applied -> " + tcCustcode;
            AuditTrail au = new AuditTrail();
            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            //gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
            //decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
            //tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            //srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                        //update tranhist account 396 1756
            updateClient_Code updcl = new updateClient_Code();
            updcl.updClient(cs, "nvoucherno");
        }

        private void resetMinAccrBal()
        {
            string dsql = "update glmast set nminbal=0.00,AccruedInterest=0.00 where acode in ('250','251') ";
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                updCommand1.UpdateCommand = new SqlCommand(dsql, nConnHandle2);
                nConnHandle2.Open();
                updCommand1.UpdateCommand.ExecuteNonQuery();//   insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }


        private void button18_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	If Messagebox('Are you sure you want to run Interest Calculation?' ,260,'Interest Calculation Run')=6
		Select interest_rates
		Locate
		If !lintcal
			Store 0.00 To gnWithInt,gnDeptInt,gnAccrInt,debitinterest,creditinterest
			gcf1=Alltrim(Str(.spinner1.Value))
			gnRecCount=0
			gnIntRate=Lint_rate*.01
			gcf2=Padl(Alltrim(Str(.spinner3.Value)),2,'0')
			gcf3=.spinner1.Value
			gcf4=.spinner3.Value
			.label4.Caption='Interest Amount'
			Set Exact Off
			Select loanview
			Set Filter To !Empty(nbookbal)
			Locate
			Count To .text2.Value
			Locate
			gnRecCount=0
			Do While !Eof()
				gnRecCount=gnRecCount+1
				.text1.Value=gnRecCount
				gcAcctNumb=cacctnumb
				.text3.Value=(nbookbal*gnIntRate)/12
				.text4.Value=cacctnumb
				gnAccrInt=-Abs((nbookbal*gnIntRate)/12)
				Select glmast
				If Seek(gcAcctNumb)
					Replace nmtdprofit With 0.00
					Replace nmtdprofit With gnAccrInt
				Endif
				gnAccrInt=0.00
				Select loanview
				Skip
			Enddo
			=Requery('acc_interest1')
			Select interest_rates
			Replace lintcal With .T.
		Else
			=Messagebox('Interest already calculated for selected period',0,'Interest. Cal. Update')
		Endif
	Endif
	Thisform.refdata
Endwith

             */
        }

        private void button17_Click(object sender, EventArgs e)
        {
            /*
             Release gncount
Public gncount
With Thisform
	If Messagebox('Are you sure you want to run Interest Application ?' ,260,'Minimum Balance Run')=6
		Thisform.refdata
		Select interest_rates
		If !lintapp
			.label4.Caption='Interest Amount'
			Release gcmudarib1,gcacctnumb,gnamount,gcprofdis,gcholinv3,gcacrinv3,gctrandesc,gnnewbal,gnmamount,gcacrminv3,gcAcode,;
				gcDescrip,gnHsTranCnt,gnHsTranCnt1,gnOldBal1,gnOldBal2
			Public gcmudarib1,gcacctnumb,gnamount,gcprofdis,gcholinv3,gcacrinv3,gctrandesc,gnnewbal,gnmamount,gcacrminv3,gcAcode,;
				gcDescrip,gnHsTranCnt,gnHsTranCnt1,gnOldBal1,gnOldBal2
			Store '' To gcmudarib1,gcacctnumb,gcprofdis,gcholinv3,gcacrinv3,gctrandesc,gcacrminv3,gcAcode,gcDescrip
			Store 0.00 To gnamount,gnnewbal,gnmamount,gnOldBal1,gnOldBal2
			Store 0 To gnHsTranCnt,gnHsTranCnt1
			gcvouch=genevouc('1')
			=OPENDBF('GLMAST','CACCTNUMB')

*************will be removed later when we know the correct contra account************************
			gcContra=gcLoanControl
			=Seek(gcContra)
			gnOldBal2=nbookbal
			gnHsTranCnt1=Thisform.getcount(gcContra)
***************************************************************************************************
			Select acc_interest1
			Release gncount
			Public gncount
			gncount=0
			.label4.Caption='Accrued Interest'
			.text2.Value=Reccount()
			Sum(nmtdprofit) To gnmamount
			If gnmamount<>0.00		&&There are accrued amounts
				Go Top
				Do While !Eof()
					Store .F. To glTransOk, glTransOk1,glTransSucc
					gncount=gncount+1
					Store cAcctNumb To gcacctnumb
					Store nbookbal To gnOldBal1
					Store ntrancnt To gnTranHstCnt
					Store nmtdprofit To gnamount
					gnHsTranCnt=Thisform.getcount(gcacctnumb)
					.text1.Value=gncount
					.text4.Value=gcacctnumb
					.text3.Value=Abs(gnamount)
					If gnamount<>0.00		&&There is an accrued amount for this customer account
						gnContAmt=-gnamount
						gnNewGmd=gnamount
						Store .F. To glUpdated,glUpdated1					&&initialise account update variable
						gcTranCode='91'
						Thisform.UPDGL(gcacctnumb,gnamount,gnNewGmd,.T.,0)   									&&upddate Posting A/C in GL
						If glUpdated
							Thisform.UPDGL(gcContra,-gnamount,-gnNewGmd,.F.,0)  									&&upddate Contra  A/C in GL
							If glUpdated1
								gctrandesc='Calculated Debit Interest  '+gcContra
								Thisform.UPDTRANS(gcacctnumb,gnamount,gnOldBal1, gcTranCode,gcContra,gnNewGmd,gnHsTranCnt,1)	&&Update Trans file posting A/C
								gctrandesc='Calculated Credit Interest  '+gcacctnumb
								Thisform.UPDTRANS(gcContra,-gnamount,gnOldBal2,Iif(gnamount>0,'98','99'),gcacctnumb,-gnNewGmd,gnHsTranCnt1,2) 	&&Update Trans file Contra A/C
								Thisform.trancountupdate()
							Else
								=Messagebox('Contra Account Balance was not updated,transaction rejected'+Chr(13)+;
									'Please check book balance of posting account',0,'Update check')
							Endif
						Else
							=Messagebox('Posting Account Balance was not updated,transaction rejected'+Chr(13)+;
								'Please repost',0,'Update check')
						Endif
					Endif
					Thisform.updinterest (gcacctnumb)
					Select acc_interest1
					Skip
				Enddo
				=Messagebox('Interest Application Successful',0,'Interest Application')
			Endif
			Select interest_rates
			Replace lintapp With .T.
		Else
			=Messagebox('Interest already applied for selected year',0,'Min. Bal. Update')
		Endif
		.Refresh
	Endif
Endwith


             */
        }

        private void button8_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	Select sysdate
		If Messagebox('Are you sure you want to run Annual Subscriptions',260,'Annual Subs')=6
			Release gcmudarib1,gcacctnumb,gnamount,gcprofdis,gcholinv3,gcacrinv3,gctrandesc,gnnewbal,gnmamount,gcacrminv3,gcAcode,;
				gcDescrip,gnHsTranCnt,gnHsTranCnt1,gnOldBal1,gnOldBal2,gnCount
			Public gcmudarib1,gcacctnumb,gnamount,gcprofdis,gcholinv3,gcacrinv3,gctrandesc,gnnewbal,gnmamount,gcacrminv3,gcAcode,;
				gcDescrip,gnHsTranCnt,gnHsTranCnt1,gnOldBal1,gnOldBal2,gnCount
			Store '' To gcmudarib1,gcacctnumb,gcprofdis,gcholinv3,gcacrinv3,gctrandesc,gcacrminv3,gcAcode,gcDescrip
			Store 0.00 To gnamount,gnnewbal,gnmamount,gnOldBal1,gnOldBal2
			Store 0 To gnHsTranCnt,gnHsTranCnt1,gnCount
			Store .F. To glTransOk, glTransOk1,glTransSucc
			gcvouch='Subs001'
			*WAIT WINDOW gnAnnsubs
			gcContra=gcSubsControl
			*WAIT WINDOW gcSubsControl
			Select savglmast
			Set Filter To Left(cacctnumb,4)=gcSaveFilt AND nbookbal>=gnAnnSubs
			LOCATE
			*brow
			*BROWSE FIELDS cacctnumb,nbookbal
			Do While !Eof()
				Store cacctnumb To gcacctnumb
				Store nbookbal To gnOldBal1
				Store ntrancnt To gnTranHstCnt
				gnHsTranCnt=Thisform.getcount(gcacctnumb)
				gnAnnSubs=-ABS(gnAnnSubs)
				gnContAmt=-gnAnnSubs
				gnNewGmd=gnAnnSubs
				gnCount=gnCount+1
				.text8.Value=gnCount
				Store .F. To glUpdated,glUpdated1					&&initialise account update variable
				gcTranCode='80'
				Thisform.UPDGL(gcacctnumb,gnAnnSubs,gnNewGmd,.T.,0)   									&&upddate Posting A/C in GL
				If glUpdated
					Thisform.UPDGL(gcSubsControl,-gnAnnSubs,-gnNewGmd,.F.,0)  									&&upddate Contra  A/C in GL
					If glUpdated1
						gctrandesc='Annual Subscription Debit  '+gcSubsControl
						Thisform.UPDTRANS(gcacctnumb,gnAnnSubs,gnOldBal1, gcTranCode,gcSubsControl,gnNewGmd,gnHsTranCnt,1)	&&Update Trans file posting A/C
						gctrandesc='Annual Subscription Credit '+gcacctnumb
						Thisform.UPDTRANS(gcSubsControl,-gnAnnSubs,gnOldBal2,Iif(gnAnnSubs>0,'98','99'),gcacctnumb,-gnNewGmd,gnHsTranCnt1,2) 	&&Update Trans file Contra A/C
						Thisform.trancountupdate()
					Else
						=Messagebox('Contra Account Balance was not updated,transaction rejected'+Chr(13)+;
							'Please check book balance of posting account',0,'Update check')
					Endif
				Else
					=Messagebox('Posting Account Balance was not updated,transaction rejected'+Chr(13)+;
						'Please repost',0,'Update check')
				Endif
				Select savglmast				&&acc_interest1
				Set Filter To Left(cacctnumb,4)=gcSaveFilt AND nbookbal>=gnAnnSubs
				Skip
			ENDDO
			 =Messagebox('Subscription Application Successful',0,'Interest Application')
			* thisform.updbookbal
			Select sysdate
			Replace subsdone With .T.
		ENDIF
		=REQUERY('annsubs')
	*Endif
Endwith

             */
        }

        /*
                             int tnPrdid = Convert.ToInt16(LoanAccountsView.Rows[0]["prd_id"]);
                    gcControlAcct = getProductControl.productControl(cs, tnPrdid);

             */
        /*
                 string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string gcLocalCaption = globalvar.cLocalCaption;
        DateTime gdTodayDate = globalvar.gdSysDate;
        string gcUserID = globalvar.gcUserid;
        int gnBranch = globalvar.gnBranchid;

             */
        private void button4_Click(object sender, EventArgs e)
        {
            fixrbal fx = new fixrbal(cs, ncompid, gcLocalCaption, gdTodayDate, gcUserID, gnBranch);
            fx.ShowDialog();
        }


        private void clientgrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (clientgrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                clientgrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            else { return; }
        }

        private static int getProductId(string tcAcctNumb, string cs1)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs1))
            {
                string cquery = "select prd_id from glmast where cacctnumb = @lcAcctNumb ";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.SelectCommand = new SqlCommand(cquery, ndConnHandle);
                cuscommand.SelectCommand.Parameters.Add("@lcAcctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                DataTable statview = new DataTable();
                ndConnHandle.Open();
                cuscommand.SelectCommand.ExecuteNonQuery();
                cuscommand.Fill(statview);
                ndConnHandle.Close();
                if (statview.Rows.Count > 0)
                {
                    return Convert.ToInt16(statview.Rows[0]["prd_id"]);
                }
                else
                {
                    return 0;
                }
            }
        }
        private void clientgrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (clientgrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                bool dres1 = Convert.ToBoolean(clientgrid.Rows[e.RowIndex].Cells["selPrd"].Value);
                if (dres1 == true)
                {
                    int dRowIndex = Convert.ToInt16(clientgrid.Rows[e.RowIndex].Cells["selPrd"].RowIndex);
                    gcProductDesc = clientview.Rows[e.RowIndex]["prd_name"].ToString(); 
                    glProductDeduction = !Convert.IsDBNull(clientview.Rows[e.RowIndex]["deduct"]) ? Convert.ToBoolean(clientview.Rows[e.RowIndex]["deduct"]) : false;
                    string dProdType = clientview.Rows[dRowIndex]["acode"].ToString();
                    button1.Enabled = button2.Enabled = button3.Enabled = (dProdType == "250" || dProdType == "251" || dProdType == "270" || dProdType == "271") && !glProductDeduction ? true : false;
                    button1.BackColor = button2.BackColor = button3.BackColor = (dProdType == "250" || dProdType == "251" || dProdType == "270" || dProdType == "271") && !glProductDeduction ? Color.LawnGreen : Color.Gainsboro;
                    button3.Text = "Interest Application";


                    for (int j = 0; j < clientgrid.Rows.Count; j++)
                    {
                        if(j!=dRowIndex)
                        {
                            clientgrid.Rows[j].Cells["selPrd"].Value = false;
                        }
                    }
                    if(glProductDeduction) //This is a deduction 
                    {
                        button3.Enabled = true;
                        button3.BackColor = Color.LawnGreen;
                        button3.Text = "Deduction Application";
                    }
                }
            }
        }

        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (clientgrid.CurrentCell is DataGridViewCheckBoxCell)
            //{
            //    bool dres1 = Convert.ToBoolean(clientgrid.Rows[e.RowIndex].Cells["selPrd"].Value);
            //    if (dres1 == true)
            //    {
            //        int dRowIndex = Convert.ToInt16(clientgrid.Rows[e.RowIndex].Cells["selPrd"].RowIndex);
            //        string dProdType = clientview.Rows[dRowIndex]["acode"].ToString();
            //        MessageBox.Show("The product code in cell click is " + dProdType);
            //        for (int j = 0; j < clientgrid.Rows.Count; j++)
            //        {
            //            if (j != dRowIndex)
            //            {
            //                clientgrid.Rows[j].Cells["selPrd"].Value = false;
            //            }
            //        }
            //    }
            //}
        }
    }
}
