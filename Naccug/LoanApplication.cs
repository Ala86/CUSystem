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
using System.Globalization;

namespace WinTcare
{
    public partial class LoanApplication : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string dloca = globalvar.cLocalCaption;

        int gnNewLoanID = 0;
        int gnOldLoanID = 0;
        decimal gnPrevLoanBal = 0.00m;
        decimal gnPrincipal = 0.00m;
        double gnTotalAmt = 0.00;
        double gnMaxAmt = 0.00;
        int gnPrdId = 0;
        int gnIntScope = 0;
        int gnLoanTopup = 0;
        int gnLoanResched = 4;
        DataTable prevLoanView = new DataTable();

        bool glTopup    = false;
        bool glResched  = false;
        double annRat;
        int nper;
        int nyrpay;
        double pv;  //This will return the periodic payment

        public LoanApplication()
        {
            InitializeComponent();
        }


        private void LoanApplication_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Loan Application >>";
            gnNewLoanID = GetClient_Code.clientCode_int(cs, "loan_id");
            textBox2.Text = gnNewLoanID.ToString();
            textBox18.Text = 0.00m.ToString();
            getdetails();
            membcode.Focus();
            numofpayments();
        }

        private void numofpayments()
        {
            string[] mones = new string[7];
            mones[0] = "Daily";
            mones[1] = "Weekly";
            mones[2] = "Fortnight";
            mones[3] = "Monthly";
            mones[4] = "Quarterly";
            mones[5] = "Half-Yearly";
            mones[6] = "Yearly";
            comboBox6.DataSource = mones;
            comboBox6.SelectedIndex = -1;
        }

        private void getdetails()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {

                //************Getting product type     
               
                string dsql2 = "select prd_id,prd_name from prodtype ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    comboBox1.DataSource = ds2.DefaultView;
                    comboBox1.DisplayMember = "prd_name";
                    comboBox1.ValueMember = "prd_id";
                    comboBox1.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find product types, inform IT Dept immediately"); }

                //************Getting purpose of loan
                string dsql3 = "select res_id,res_name from loanReason ";
                SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle1);
                DataTable ds3 = new DataTable();
                da3.Fill(ds3);
                if (ds3 != null)
                {
                    comboBox3.DataSource = ds3.DefaultView;
                    comboBox3.DisplayMember = "res_name";
                    comboBox3.ValueMember = "res_id";
                    comboBox3.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Loan reasons types, inform IT Dept immediately"); }

                //************Getting economic sector
                string dsql6 = "select sec_id,sec_name from sector ";
                SqlDataAdapter da6 = new SqlDataAdapter(dsql6, ndConnHandle1);
                DataTable ds6 = new DataTable();
                da6.Fill(ds6);
                if (ds6 != null)
                {
                    comboBox2.DataSource = ds6.DefaultView;
                    comboBox2.DisplayMember = "sec_name";
                    comboBox2.ValueMember = "sec_id";
                    comboBox2.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find economic sector types, inform IT Dept immediately"); }

                //************Getting source of finds
                string dsql4 = "select sou_id,sou_name from sourcefunds ";
                SqlDataAdapter da4 = new SqlDataAdapter(dsql4, ndConnHandle1);
                DataTable ds4 = new DataTable();
                DataTable ds5 = new DataTable();
                da4.Fill(ds4);
                if (ds4 != null)
                {
                    comboBox4.DataSource = ds4.DefaultView;
                    comboBox4.DisplayMember = "sou_name";
                    comboBox4.ValueMember = "sou_id";
                    comboBox4.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find source of funds types, inform IT Dept immediately"); }

                da4.Fill(ds5);
                if (ds5 != null)
                {
                    comboBox5.DataSource = ds5.DefaultView;
                    comboBox5.DisplayMember = "sou_name";
                    comboBox5.ValueMember = "sou_id";
                    comboBox5.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find source of funds types, inform IT Dept immediately"); }
            //    MessageBox.Show("this is me 2");

            }
        }

        private void getprodtype()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
               // MessageBox.Show("this is it" + gnIntScope);
                //************Getting product type                
                string dsql2 = "select prd_id,prd_name from prodtype ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    comboBox1.DataSource = ds2.DefaultView;
                    comboBox1.DisplayMember = "prd_name";
                    comboBox1.ValueMember = "prd_id";
                    comboBox1.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find product types, inform IT Dept immediately"); }
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
            bool lnPrincipalOk = radioButton3.Checked ? true : gnPrincipal > 0.00m; 
            if (membcode.Text.ToString().Trim() != "" && lnPrincipalOk && gnTotalAmt > 0.00) 

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

        private void button29_Click(object sender, EventArgs e)
        {
            FindClient fc = new FindClient(cs,ncompid,dloca, 1,"Cusreg");
            fc.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void membcode_Validated(object sender, EventArgs e)
        {
            if (membcode.Text.ToString().Trim() != null && membcode.Text.ToString().Trim() != "")
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    string custcode = Convert.ToString(membcode.Text).Trim();
                    string tcCode = custcode.PadLeft(6, '0');// Convert.ToString(membcode.Text).Trim().PadLeft(6, '0');
                    membcode.Text = tcCode;
                    ndConnHandle.Open();
                    string dsql2 = "select ccustfname,ccustmname,ccustlname,ccustname, nbookbal from cusreg, glmast ";
                    dsql2 += "where cusreg.ccustcode = glmast.ccustcode and glmast.acode in ('250','251') and cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    DataTable ds2 = new DataTable();
                    da2.Fill(ds2);
                    if (ds2 != null && ds2.Rows.Count > 0)
                    {
                        textBox1.Text = ds2.Rows[0]["ccustname"].ToString().Trim() + ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString() + ' ' + ds2.Rows[0]["ccustlname"].ToString();
                        textBox4.Text = ds2.Rows[0]["nbookbal"].ToString().Trim();
                        Checkloans(tcCode);
                        if (radioButton2.Checked)
                        {
                            getloans(tcCode);
                        } 
                    }
                    else
                    {
                        MessageBox.Show("Member ID not found");
                        membcode.Text = "";
                        membcode.Focus();
                    }
                }
                AllClear2Go();
            } 
        }

        private void Checkloans(string memcode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql2lc = "exec tsp_getLoans4TopUp " + ncompid + ",'" + memcode + "'";
                SqlDataAdapter da2lc = new SqlDataAdapter(dsql2lc, ndConnHandle);
                DataTable prevLoanViewc = new DataTable();
                da2lc.Fill(prevLoanViewc);
                if(prevLoanViewc!=null && prevLoanViewc.Rows.Count>0)
                {
                    radioButton2.Enabled = prevLoanViewc != null && prevLoanViewc.Rows.Count > 0 ? true : false;
                    radioButton3.Enabled = prevLoanViewc != null && prevLoanViewc.Rows.Count > 0 ? true : false;
                }
            }
        }

        private void getloans(string memcode)
        {
            prevLoanView.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql2l = "exec tsp_getLoans4TopUp " + ncompid + ",'" + memcode + "'";
                SqlDataAdapter da2l = new SqlDataAdapter(dsql2l, ndConnHandle);
                da2l.Fill(prevLoanView);
                if (prevLoanView != null && prevLoanView.Rows.Count > 0)
                {
                   gnIntScope = Convert.ToInt32(prevLoanView.Rows[0]["intscope"]);
                  //  MessageBox.Show("this is the topup"+gnIntScope);
                    comboBox8.DataSource = prevLoanView.DefaultView;
                    comboBox8.DisplayMember = "loantype";
                    comboBox8.ValueMember = "loan_id";
                    comboBox8.SelectedIndex = -1;
                } else { MessageBox.Show("No active loans found"); }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          if(comboBox1.Focused )
            {
            //   MessageBox.Show("Selected index change");
                if(radioButton1.Checked)
                {
                    int selval = Convert.ToInt32(comboBox1.SelectedValue);
                    using (SqlConnection ndConnHandle = new SqlConnection(cs))
                    {
                        ndConnHandle.Open();
                        string dsql21 = "select max_amt,int_rate,int_scope,int_calc from prodtype where prd_id= " + selval;
                        SqlDataAdapter da21 = new SqlDataAdapter(dsql21, ndConnHandle);
                        DataTable prodview = new DataTable();
                        da21.Fill(prodview);
                        if (prodview != null && gnIntScope == 3)
                        {
                            string intmethod = (Convert.ToInt32(prodview.Rows[0]["int_calc"]) == 1 ? "Profit Calculation" :
                                (Convert.ToInt32(prodview.Rows[0]["int_calc"]) == 2 ? "Compound Interest" : (Convert.ToInt32(prodview.Rows[0]["int_calc"]) == 3 ?
                                "Simple/flat rate" : "Reducing Balance")));
                            string maxamt = prodview.Rows[0]["max_amt"].ToString().Trim();
                            textBox6.Text = intmethod;
                            textBox20.Text = Convert.ToDouble(maxamt).ToString("###,###,###.00");
                           // textBox7.Text = prodview.Rows[0]["int_rate"].ToString().Trim();
                        }
                        else
                        {
                            string intmethod = (Convert.ToInt32(prodview.Rows[0]["int_calc"]) == 1 ? "Reducing/Declining Balance" :
                                (Convert.ToInt32(prodview.Rows[0]["int_calc"]) == 2 ? "Compound Interest" : (Convert.ToInt32(prodview.Rows[0]["int_calc"]) == 3 ?
                               "Simple/flat rate" : "Reducing Balance")));
                            string maxamt = prodview.Rows[0]["max_amt"].ToString().Trim();
                            textBox6.Text = intmethod;
                            textBox20.Text = Convert.ToDouble(maxamt).ToString("###,###,###.00");
                            textBox7.Text = prodview.Rows[0]["int_rate"].ToString().Trim();
                        }
                    }
                } 
            }
        }


        private void dcalcInterest()
        {
            try
            {
             //   MessageBox.Show("This is the First Step 11");

                if (textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && gnIntScope == 3 && textBox5.Text != "")
                {
              //      MessageBox.Show("This is the First Step 11");
                    DateTime start_date = DateTime.Parse(txtStartDate.Text);                                //start date of loan
                    DateTime end_date = DateTime.Parse(txtEndDate.Text);
                    double dRate = Convert.ToDouble(textBox25.Text);       //interest rate per annum
                    double pv = (radioButton1.Checked ? double.Parse(textBox5.Text) : double.Parse(textBox3.Text));              //Principal
                    int dDur = Convert.ToInt32(textBox9.Text);            //duration - currently in months
                    double dpaymt = 0.00;
                    double intPay = 0.00;
                    double totPay = 0.00;
                    //  loanCalculation lc = new loanCalculation();
                    int ppyr = Convert.ToInt32(textBox8.Text);           // number of payment per year

                    double rate = (double)dRate;           //(double)annRate / 100 / 12;
                    double denom = Math.Pow((1 + rate), nper) - 1;
                    double pmtn = (rate + (rate / denom)) * pv;
                    double paymt1 = (pv + dRate);


                    if (dRate > 0.00)
                    {
                        dpaymt = paymt1 / dDur;  // Fixed Periodic Payment
                        intPay = rate;
                        totPay = dpaymt * dDur;
                        // Paymt * dDur;
                    }
                    else
                    {
                        dpaymt = pv / dDur;
                        intPay = 0.0;
                        totPay = dpaymt * dDur;

                    }
                    textBox18.Text = dpaymt.ToString("N2");     // Paymt.ToString("N2");
                    textBox13.Text = totPay.ToString("N2");
                    gnTotalAmt = totPay;
                    textBox14.Text = pv.ToString("N2");
                    textBox21.Text = intPay.ToString("N2");         // (totPay - pv).ToString("N2");
                    textBox12.Text = textBox9.Text;
                    textBox24.Text = Math.Abs(dpaymt).ToString("N2");
                }
                else
                {
                //    MessageBox.Show("This is the First Step 1");
                    DateTime start_date = DateTime.Parse(txtStartDate.Text);                                //start date of loan
                    DateTime end_date = DateTime.Parse(txtEndDate.Text);                                    //end date of loan
                                                                                                            //double dRate = Convert.ToDouble(textBox7.Text);       //interest rate per annum
                    double dRate = gnIntScope == 3 ? Convert.ToDouble(textBox25.Text) : Convert.ToDouble(textBox7.Text);       //interest rate per annum
                    double pv = (radioButton1.Checked ? double.Parse(textBox5.Text) : double.Parse(textBox3.Text));              //Principal
                    int dDur = Convert.ToInt32(textBox9.Text);            //duration - currently in months
                    double dpaymt = 0.00;
                    double intPay = 0.00;
                    double totPay = 0.00;
                    loanCalculation lc = new loanCalculation();
                    int ppyr = Convert.ToInt32(textBox8.Text);           // number of payment per year
                    double newrate = dRate / 100 / ppyr;
               //     MessageBox.Show("This is the First Step ");
                    if (dRate > 0.00)
                    {
                    //    MessageBox.Show("This is the second Step ");
                        dpaymt = Math.Abs(loanCalculation.pmt(newrate, dDur, pv, 0.00, 0));  // Fixed Periodic Payment
                        intPay = loanCalculation.intrest(dRate, dDur, ppyr, pv);
                        totPay = dpaymt * dDur;              // Paymt * dDur;
                    }
                    else
                    {
                   //     MessageBox.Show("This is the Third Step ");
                        dpaymt = pv / dDur;
                        intPay = 0.0;
                        totPay = dpaymt * dDur;

                    }
                    textBox18.Text = dpaymt.ToString("N2");     // Paymt.ToString("N2");
                    textBox13.Text = totPay.ToString("N2");
                    gnTotalAmt = totPay;
                    textBox14.Text = pv.ToString("N2");
                    textBox21.Text = intPay.ToString("N2");         // (totPay - pv).ToString("N2");
                    textBox12.Text = textBox9.Text;
                    textBox24.Text = Math.Abs(dpaymt).ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
   
        private void textBox5_Validated(object sender, EventArgs e)
        {
            if(textBox5.Text != null && textBox20.Text !=null)
            {
                if(textBox5.Text!="")
                {
                    gnPrincipal = Convert.ToDecimal(textBox5.Text);

                }
                if(radioButton1.Checked)                //new loan
                {
                    if (Convert.ToDecimal(textBox20.Text) > 0.00m && (Convert.ToDecimal(textBox5.Text) > Convert.ToDecimal(textBox20.Text)))
                    {
                        MessageBox.Show("Principle cannot be more than loan Limit");
                        textBox5.Text = Convert.ToDecimal(textBox20.Text).ToString("N2");
                    }
                    else { textBox5.Text = Convert.ToDecimal(textBox5.Text).ToString("N2"); }
                    AllClear2Go();
                }else 
                    if(radioButton2.Checked || radioButton3.Checked)            //top up loan
                        {
                            label4.Text = "New Principal";
                            label4.BackColor = Color.Yellow;
                            textBox5.Text = Convert.ToDecimal(textBox5.Text).ToString("N2");
                            decimal nprev = gnPrevLoanBal + Convert.ToDecimal(textBox5.Text);
                            if(Convert.ToDouble(nprev) > gnMaxAmt)
                            {
                                MessageBox.Show("New Principal "+nprev+" cannot be more than maximum amount "+gnMaxAmt);
                                textBox5.Text = Convert.ToDecimal(gnMaxAmt - Convert.ToDouble(gnPrevLoanBal)).ToString("N2");
                                nprev = gnPrevLoanBal + Convert.ToDecimal(textBox5.Text);
                                textBox3.Text = nprev.ToString("N2");
                            }
                            else
                            {
                                textBox3.Text = (Convert.ToDecimal(textBox3.Text)+Convert.ToDecimal(textBox5.Text)).ToString("N2"); //; ; Convert.ToDecimal(nprev).ToString("N2"); 
                            }

                            textBox13.Text = "";
                            gnTotalAmt = 0.00;
                            textBox12.Text = "";
                            textBox14.Text = "";
                            textBox18.Text = "";
                            textBox19.Text = "";
                            textBox17.Text = "";
                            textBox23.Text = "";
                            textBox16.Text = "";
                            textBox21.Text = "";
                            textBox9.Text = "";
                           // calcInterest();
                            AllClear2Go();
                        }
            }
        }

        private void txtStartDate_Validated(object sender, EventArgs e)
        {
            txtFirstPayDate = txtStartDate;
            textBox17.Text = Convert.ToDateTime(txtFirstPayDate.Text).ToLongDateString();
          //  calcInterest();
        }

        private void textBox8_Validated(object sender, EventArgs e)
        {
            if(textBox8.Text!="")
            {
                textBox16.Text = (Convert.ToInt32(textBox8.Text) == 365 ? "Daily" :
                   (Convert.ToInt32(textBox8.Text) == 52 ? "Weekly" :
                   (Convert.ToInt32(textBox8.Text) == 26 ? "Fortnight" :
                   (Convert.ToInt32(textBox8.Text) == 12 ? "Monthly " :
                   (Convert.ToInt32(textBox8.Text) == 4 ? "Quarterly " :
                   (Convert.ToInt32(textBox8.Text) == 2 ? "Half-yearly " :
                   (Convert.ToInt32(textBox8.Text) == 1 ? "Yearly " : "Undefined frequency")))))));
          //      calcInterest();
            }
        }

        private void textBox9_Validated(object sender, EventArgs e)
        {
            if(textBox8.Text!="")
            {
                int ddays = (Convert.ToInt32(textBox8.Text) == 365 ? 1 :
               (Convert.ToInt32(textBox8.Text) == 52 ? 7 :
               (Convert.ToInt32(textBox8.Text) == 26 ? 14 :
               (Convert.ToInt32(textBox8.Text) == 12 ? 30 :
               (Convert.ToInt32(textBox8.Text) == 4 ? 90 :
               (Convert.ToInt32(textBox8.Text) == 2 ? 180 :
               (Convert.ToInt32(textBox8.Text) == 1 ? 365 : 0))))))) * Convert.ToInt32(textBox9.Text);
                //    MessageBox.Show("The number of days are " + ddays);

                textBox17.Text = Convert.ToDateTime(txtStartDate.Text).ToLongDateString();
                int dmonth = Convert.ToInt32(textBox9.Text);
                txtEndDate.Text = Convert.ToDateTime(txtStartDate.Text).AddMonths(dmonth).ToLongDateString();
                textBox23.Text = Convert.ToDateTime(txtEndDate.Text).ToLongDateString();
              //  MessageBox.Show("The number of days are 1 " + ddays);
                dcalcInterest();
             //   MessageBox.Show("The number of days are 2 " + ddays);
                AllClear2Go();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
             glTopup   = radioButton2.Checked ;
             glResched = radioButton3.Checked;
            if(radioButton1.Checked )
            {
                insertloan();
            }

            if (radioButton2.Checked)  //top up loan. We need to close old loan product, maintain old loan account, update bookbal, close old amortization. New amortization will be created at loan approval.
            {
                UpdateOldLoan(gnOldLoanID);
            }

            if (radioButton3.Checked)  //reschedule loan. We need to close old loan product, maintain old loan account, update bookbal, close old amortization. New amortization will be created at loan approval.
            {
                UpdateOldLoan(gnOldLoanID);
            }

            initvariables();
            updateClient_Code updc = new updateClient_Code();
            updc.updClient(cs, "loan_id");
            gnNewLoanID = GetClient_Code.clientCode_int(cs, "loan_id");
            textBox2.Text = gnNewLoanID.ToString();
            membcode.Focus();
        }

        private void insertloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                decimal dloanAmt = (radioButton1.Checked ? Convert.ToDecimal(textBox5.Text) : Convert.ToDecimal(textBox3.Text));
                decimal dPrinPay = Math.Round(Convert.ToDecimal(textBox5.Text) / Convert.ToDecimal(textBox9.Text), 2);
                string cglquery = "Insert Into LOAN_DET (loan_id, ccustcode,NET_SAVINGS,LOAN_TYPE_ID,LOAN_INTEREST,PRINCIPAL_AMT,LDURATION_NUM,LOANSTART_DATE,MATURITY_DATE,";
                cglquery += "REPAYMENT_AMT,NOFPAYMENTS,TOTAL_INTEREST,cuserid,BRANCH_ID,loan_appl_date,leconsec,lloanpurpos,memsourcefunds,guasourcefunds,nofpayperyear,graceperiod,graceperiodinterest,compid,topup,resched,nprinpmnt)";
                cglquery += " values (@loanid,@lccustcode,@lNET_SAVINGS,@lLOAN_TYPE_ID,@lLOAN_INTEREST,@lPRINCIPAL_AMT,@lLDURATION_NUM,@lLOANSTART_DATE,@lMATURITY_DATE,";
                cglquery += "@lREPAYMENT_AMT,@lNOFPAYMENTS,@lTOTAL_INTEREST,@lcuserid,@lBRANCH_ID,convert(date,getdate()),@nleconsec, @nlloanpurpos, @nmemsourcefunds, @nguasourcefunds,@lnofpayperyear,@lgraceperiod,@lgraceperiodinterest,@ncompid,@ltopup,@lresched,@tnprinpmnt)";

                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnNewLoanID;
                insCommand.InsertCommand.Parameters.Add("@lccustcode", SqlDbType.VarChar).Value = membcode.Text.ToString();
                insCommand.InsertCommand.Parameters.Add("@lNET_SAVINGS", SqlDbType.Decimal).Value = (textBox4.Text != "" ? Convert.ToDecimal(textBox4.Text) : 0.00m);
                insCommand.InsertCommand.Parameters.Add("@lLOAN_TYPE_ID", SqlDbType.Int).Value = gnPrdId;// Convert.ToInt16(comboBox1.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@lLOAN_INTEREST", SqlDbType.Decimal).Value = (gnIntScope ==3 ? 0.00m : Convert.ToDecimal(textBox7.Text));
                insCommand.InsertCommand.Parameters.Add("@lPRINCIPAL_AMT", SqlDbType.Decimal).Value = (radioButton1.Checked ? Convert.ToDecimal(textBox5.Text) : Convert.ToDecimal(textBox3.Text));
                insCommand.InsertCommand.Parameters.Add("@lLDURATION_NUM", SqlDbType.Int).Value = Convert.ToInt16(textBox9.Text);
                insCommand.InsertCommand.Parameters.Add("@lLOANSTART_DATE", SqlDbType.DateTime).Value = Convert.ToDateTime(txtStartDate.Text);
                insCommand.InsertCommand.Parameters.Add("@lMATURITY_DATE", SqlDbType.DateTime).Value = Convert.ToDateTime(txtEndDate.Text);
                insCommand.InsertCommand.Parameters.Add("@lREPAYMENT_AMT", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox18.Text);
                insCommand.InsertCommand.Parameters.Add("@lnofpayperyear", SqlDbType.Int).Value = Convert.ToInt32(textBox8.Text);
                insCommand.InsertCommand.Parameters.Add("@lNOFPAYMENTS", SqlDbType.Int).Value = Convert.ToInt32(textBox9.Text);
                insCommand.InsertCommand.Parameters.Add("@lTOTAL_INTEREST", SqlDbType.Decimal).Value = (textBox21.Text != "" ? Convert.ToDecimal(textBox21.Text) : 0.00m);
                insCommand.InsertCommand.Parameters.Add("@lcuserid", SqlDbType.Char).Value = globalvar.gcUserid;
                insCommand.InsertCommand.Parameters.Add("@lBRANCH_ID", SqlDbType.Int).Value = globalvar.gnBranchid;
                insCommand.InsertCommand.Parameters.Add("@nleconsec", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@nlloanpurpos", SqlDbType.Int).Value = Convert.ToInt32(comboBox3.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@nmemsourcefunds", SqlDbType.Int).Value = Convert.ToInt32(comboBox4.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@nguasourcefunds", SqlDbType.Int).Value = Convert.ToInt32(comboBox5.SelectedValue);
                insCommand.InsertCommand.Parameters.Add("@lgraceperiod", SqlDbType.Int).Value = (textBox10.Text != "" ? Convert.ToInt32(textBox10.Text) : 0);
                insCommand.InsertCommand.Parameters.Add("@lgraceperiodinterest", SqlDbType.Decimal).Value = (textBox22.Text != "" ? Convert.ToDecimal(textBox22.Text) : 0);
                insCommand.InsertCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = globalvar.gnCompid;
                insCommand.InsertCommand.Parameters.Add("@ltopup", SqlDbType.Bit).Value = glTopup;
                insCommand.InsertCommand.Parameters.Add("@lresched", SqlDbType.Bit).Value = glResched;
                insCommand.InsertCommand.Parameters.Add("@tnprinpmnt", SqlDbType.Decimal).Value = dPrinPay;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();

                //we will be updating the audit trail. 
                string auditDesc = "Loan Application  -> " + membcode.Text.ToString();
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, dloanAmt, globalvar.gcUserid, "C", "", membcode.Text.ToString(), "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            }
        }

        private void UpdateOldLoan(int loanid)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                int tnLoanStat = glTopup ? 3 : 4;
                decimal dPrinPay = radioButton3.Checked ? 
                                    Math.Round(Convert.ToDecimal(textBox3.Text) / Convert.ToDecimal(textBox9.Text), 2) : 
                                    Math.Round(Convert.ToDecimal(textBox5.Text) / Convert.ToDecimal(textBox9.Text), 2);

                string cglquery = "update loan_det set lapproved=0, lgranteed =0,lissued =0, loan_status = @loanstat, LOAN_INTEREST=@lLOAN_INTEREST, PRINCIPAL_AMT=@lPRINCIPAL_AMT,LDURATION_NUM=@lLDURATION_NUM,LOANSTART_DATE=@lLOANSTART_DATE,MATURITY_DATE=@lMATURITY_DATE,REPAYMENT_AMT=@lREPAYMENT_AMT,";
                cglquery += "NOFPAYMENTS =@lNOFPAYMENTS,TOTAL_INTEREST=@lTOTAL_INTEREST,cuserid=@lcuserid,BRANCH_ID=@lBRANCH_ID,loan_appl_date=convert(date,getdate()),leconsec=@lleconsec,lloanpurpos=@llloanpurpos,memsourcefunds=@lmemsourcefunds,guasourcefunds=@lguasourcefunds,";
                cglquery += "nofpayperyear =@lnofpayperyear,graceperiod=@lgraceperiod,graceperiodinterest=@lgraceperiodinterest,totalbalance=@tltotalbalance,nprinpmnt = @tnprinpmnt where loan_id=@loanid";
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand .UpdateCommand = new SqlCommand(cglquery, ndConnHandle);
                updCommand.UpdateCommand.Parameters.Add("@loanid", SqlDbType.Int).Value = gnOldLoanID;
                updCommand.UpdateCommand.Parameters.Add("@loanstat", SqlDbType.Int).Value = tnLoanStat;
                updCommand.UpdateCommand.Parameters.Add("@lLOAN_INTEREST", SqlDbType.Decimal).Value = (gnIntScope ==3 ? Convert.ToDecimal(textBox25.Text) : Convert.ToDecimal(textBox7.Text));
                updCommand.UpdateCommand.Parameters.Add("@lPRINCIPAL_AMT", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox3.Text);
                updCommand.UpdateCommand.Parameters.Add("@lLDURATION_NUM", SqlDbType.Int).Value = Convert.ToInt16(textBox9.Text);
                updCommand.UpdateCommand.Parameters.Add("@lLOANSTART_DATE", SqlDbType.DateTime).Value = Convert.ToDateTime(txtStartDate.Text);
                updCommand.UpdateCommand.Parameters.Add("@lMATURITY_DATE", SqlDbType.DateTime).Value = Convert.ToDateTime(txtEndDate.Text);
                updCommand.UpdateCommand.Parameters.Add("@lREPAYMENT_AMT", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox18.Text);
                updCommand.UpdateCommand.Parameters.Add("@lnofpayperyear", SqlDbType.Int).Value = Convert.ToInt32(textBox8.Text);
                updCommand.UpdateCommand.Parameters.Add("@lNOFPAYMENTS", SqlDbType.Int).Value = Convert.ToInt32(textBox9.Text);
                updCommand.UpdateCommand.Parameters.Add("@lTOTAL_INTEREST", SqlDbType.Decimal).Value = (textBox21.Text != "" ? Convert.ToDecimal(textBox21.Text) : 0.00m);
                updCommand.UpdateCommand.Parameters.Add("@lcuserid", SqlDbType.Char).Value = globalvar.gcUserid;

                updCommand.UpdateCommand.Parameters.Add("@lBRANCH_ID", SqlDbType.Int).Value = globalvar.gnBranchid;
                updCommand.UpdateCommand.Parameters.Add("@lleconsec", SqlDbType.Int).Value = Convert.ToInt32(comboBox2.SelectedValue);
                updCommand.UpdateCommand.Parameters.Add("@llloanpurpos", SqlDbType.Int).Value = Convert.ToInt32(comboBox3.SelectedValue);
                updCommand.UpdateCommand.Parameters.Add("@lmemsourcefunds", SqlDbType.Int).Value = Convert.ToInt32(comboBox4.SelectedValue);
                updCommand.UpdateCommand.Parameters.Add("@lguasourcefunds", SqlDbType.Int).Value = Convert.ToInt32(comboBox5.SelectedValue);
                updCommand.UpdateCommand.Parameters.Add("@lgraceperiod", SqlDbType.Int).Value = (textBox10.Text != "" ? Convert.ToInt32(textBox10.Text) : 0);
                updCommand.UpdateCommand.Parameters.Add("@lgraceperiodinterest", SqlDbType.Decimal).Value = (textBox22.Text != "" ? Convert.ToDecimal(textBox22.Text) : 0);
                updCommand.UpdateCommand.Parameters.Add("@tltotalbalance", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox13.Text);
                // MessageBox.Show("You are updating1");
                updCommand.UpdateCommand.Parameters.Add("@tnprinpmnt", SqlDbType.Decimal).Value = dPrinPay;
                //updCommand.UpdateCommand.Parameters.Add("@ltopup", SqlDbType.Bit).Value = glTopup;
                //updCommand.UpdateCommand.Parameters.Add("@lresched", SqlDbType.Bit).Value = glResched;

                ndConnHandle.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
            }
        }

        private void updateTopsched()
        {
            //bool ltopup = radioButton2.Checked ? true : false;
            //using (SqlConnection ndConnHandle = new SqlConnection(cs))
            //{
            //    string cglquery = "insert into topsched (old_loan_id,new_loan_id,topup ,compid) values (@lold_loan_id,@lnew_loan_id,@ltopup ,@lcompid)";
            //    SqlDataAdapter insCommand = new SqlDataAdapter();
            //    insCommand.InsertCommand = new SqlCommand(cglquery, ndConnHandle);
            //    insCommand.InsertCommand.Parameters.Add("@lold_loan_id", SqlDbType.Int).Value = gnOldLoanID;
            //    insCommand.InsertCommand.Parameters.Add("@lnew_loan_id", SqlDbType.Int).Value = gnNewLoanID;
            //    insCommand.InsertCommand.Parameters.Add("@ltopup", SqlDbType.Bit).Value = ltopup;
            //    insCommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid;
            //    ndConnHandle.Open();
            //    insCommand.InsertCommand.ExecuteNonQuery();
            //    ndConnHandle.Close();
            //}
        }


        private void initvariables()
        {
            membcode.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox25.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox22.Text = "";

            Label13.Visible = false;
            label14.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            Label17.Visible = false;

            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox19.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            textBox23.Text = "";
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            label4.Text = "Current Loan Balance";
            label4.BackColor = Color.Gainsboro;
            comboBox6.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;

        }

        private void refreshVar()
        {
            textBox2.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox25.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox22.Text = "";

            textBox12.Text = "";
            textBox13.Text = "";
            textBox14.Text = "";
            textBox15.Text = "";
            textBox16.Text = "";
            textBox17.Text = "";
            textBox18.Text = "";
            textBox19.Text = "";
            textBox20.Text = "";
            textBox21.Text = "";
            textBox23.Text = "";
            label4.Text = "Current Loan Balance";
            label4.BackColor = Color.Gainsboro;
            comboBox6.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
        }


        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

            if (comboBox1.Focused)
            {
      
                textBox5.Text = "";
                int selval = Convert.ToInt32(comboBox1.SelectedValue);
                gnPrdId = Convert.ToInt32(comboBox1.SelectedValue);
                label8.Text = radioButton2.Checked ? "Top up Amount" : "Principal Amount";
                textBox5.Enabled = radioButton3.Checked ? false : true;
                if (radioButton1.Checked)
                {
                    using (SqlConnection ndConnHandle = new SqlConnection(cs))
                    {

                        ndConnHandle.Open();
                        string dsql21 = "select int_rate,max_amt,int_scope, int_calc from prodtype where prd_id= " + selval;
                        SqlDataAdapter da21 = new SqlDataAdapter(dsql21, ndConnHandle);
                        DataTable ds21 = new DataTable();
                        da21.Fill(ds21);
                        if (ds21 != null)
                        {
                            gnIntScope = Convert.ToInt32(ds21.Rows[0]["int_scope"]);
                          //  MessageBox.Show("this is me ALA 2"+ gnIntScope);
                            if (gnIntScope == 3)
                            {
                               // MessageBox.Show("this is me ALA 2" + gnIntScope);
                                string cmethod = (Convert.ToInt32(ds21.Rows[0]["int_calc"]) == 1 ? "Profit Calculation" :
                                   (Convert.ToInt32(ds21.Rows[0]["int_calc"]) == 2 ? "Compound Interest" :
                                   (Convert.ToInt32(ds21.Rows[0]["int_calc"]) == 3 ? "Simple/flat Interest" : "Reducing Balance")));
                                textBox7.Text = ds21.Rows[0]["int_rate"].ToString().Trim();
                                textBox20.Text = Convert.ToDecimal(ds21.Rows[0]["max_amt"]).ToString("N2");
                                textBox6.Text = cmethod;
                                textBox25.Visible = true;
                                label30.Visible = true;
                                textBox7.Visible = false;
                                label28.Visible = false;
                            }
                            else
                            {
                                string cmethod = (Convert.ToInt32(ds21.Rows[0]["int_calc"]) == 1 ? "Reducing Balance Method" :
                                  (Convert.ToInt32(ds21.Rows[0]["int_calc"]) == 2 ? "Compound Interest" :
                                  (Convert.ToInt32(ds21.Rows[0]["int_calc"]) == 3 ? "Simple/flat Interest" : "Reducing Balance")));
                                textBox7.Text = ds21.Rows[0]["int_rate"].ToString().Trim();
                                textBox20.Text = Convert.ToDecimal(ds21.Rows[0]["max_amt"]).ToString("N2");
                                textBox6.Text = cmethod;
                                textBox7.Visible = true;
                                label28.Visible = true;
                                textBox25.Visible = false;
                                label30.Visible = false;

                            }
                        }
                    }
                }
                else
                {
                    int lnyrpay = Convert.ToInt32(prevLoanView.Rows[0]["nyrpay"]);
                    int intmeth = Convert.ToInt32(prevLoanView.Rows[0]["intmethod"]);
                    label6.Text = "Select Loan ";
                    textBox3.Text = Convert.ToDecimal(prevLoanView.Rows[0]["loanBal"]).ToString("N2");
                    gnPrevLoanBal = Convert.ToDecimal(prevLoanView.Rows[0]["loanBal"]);
                    textBox5.Text = radioButton3.Checked ? gnPrevLoanBal.ToString("N2") : "";
                    textBox6.Text = (intmeth == 1 ? "Reducing/Declining Balance" : (intmeth == 2 ? "Compound Interest" : (intmeth == 3 ? "Simple/flat Interest" : "Reducing Balance")));
                    textBox7.Text = Convert.ToDecimal(prevLoanView.Rows[0]["intrate"]).ToString("N2");
                    //  textBox25.Text = Convert.ToDecimal(prevLoanView.Rows[0]["intrate"]).ToString("N2");
                    textBox9.Text = prevLoanView.Rows[0]["ldur"].ToString();
                    textBox18.Text = Convert.ToDecimal(prevLoanView.Rows[0]["paymt"]).ToString("N2");
                    textBox17.Text = Convert.ToDateTime(prevLoanView.Rows[0]["startdate"]).ToLongDateString();
                    textBox23.Text = Convert.ToDateTime(prevLoanView.Rows[0]["matdate"]).ToLongDateString();
                    textBox12.Text = prevLoanView.Rows[0]["ldur"].ToString();
                    textBox14.Text = Convert.ToDecimal(prevLoanView.Rows[0]["loanamt"]).ToString("N2");
                    textBox20.Text = Convert.ToDecimal(prevLoanView.Rows[0]["maxamt"]).ToString("N2");
                    gnMaxAmt = Convert.ToDouble(prevLoanView.Rows[0]["maxamt"]);
                    textBox21.Text = Convert.ToDecimal(prevLoanView.Rows[0]["totint"]).ToString("N2");
                    textBox13.Text = (Convert.ToDecimal(prevLoanView.Rows[0]["loanamt"]) + Convert.ToDecimal(prevLoanView.Rows[0]["totint"])).ToString("N2");
                    gnTotalAmt = Convert.ToDouble((Convert.ToDecimal(prevLoanView.Rows[0]["loanamt"]) + Convert.ToDecimal(prevLoanView.Rows[0]["totint"])));
                    textBox16.Text = (lnyrpay == 1 ? "Yearly" : (lnyrpay == 2 ? "Half-yearly" : (lnyrpay == 4 ? "Quarterly" : (lnyrpay == 12 ? "Monthly" :
                    (lnyrpay == 26 ? "Fortnightly" : (lnyrpay == 52 ? "Weekly" : (lnyrpay == 365 ? "Daily" : "Undefined")))))));
                    gnOldLoanID = Convert.ToInt32(prevLoanView.Rows[0]["loan_id"]);

                }
                AllClear2Go();
            }
        }
        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void comboBox6_Validated(object sender, EventArgs e)
        {
//            MessageBox.Show("The seleced items is " + comboBox6.SelectedValue.ToString());
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
         /*   if(comboBox6.Focused)
            {
                int sindex = Convert.ToInt32(comboBox6.SelectedIndex);
//                MessageBox.Show("You have selected index " + sindex);
                textBox16.Text = (sindex==0 ? "Daily" :
                   (sindex == 1 ? "Weekly" :
                   (sindex == 2 ? "Fortnightly" :
                   (sindex == 3 ? "Monthly " :
                   (sindex == 4 ? "Quarterly " :
                   (sindex == 5 ? "Half-yearly " :
                   (sindex == 6 ? "Yearly " : "Undefined frequency")))))));

                textBox8.Text = (sindex == 0 ? 365 :
                                   (sindex == 1 ? 52 :
                                   (sindex == 2 ? 26 :
                                   (sindex == 3 ? 12  :
                                   (sindex == 4 ? 4 :
                                   (sindex == 5 ? 2 :
                                   (sindex == 6 ? 1 : 0))))))).ToString();
                calcInterest();
            }*/
        }

        private void comboBox6_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox6.Focused)
            {
                int sindex = Convert.ToInt32(comboBox6.SelectedIndex);
                textBox16.Text = (sindex == 0 ? "Daily" :
                   (sindex == 1 ? "Weekly" :
                   (sindex == 2 ? "Fortnightly" :
                   (sindex == 3 ? "Monthly " :
                   (sindex == 4 ? "Quarterly " :
                   (sindex == 5 ? "Half-yearly " :
                   (sindex == 6 ? "Yearly " : "Undefined frequency")))))));

                textBox8.Text = (sindex == 0 ? 365 :
                                   (sindex == 1 ? 52 :
                                   (sindex == 2 ? 26 :
                                   (sindex == 3 ? 12 :
                                   (sindex == 4 ? 4 :
                                   (sindex == 5 ? 2 :
                                   (sindex == 6 ? 1 : 0))))))).ToString();
                if(radioButton3.Checked)
                {
                    textBox13.Text = "";
                    if (textBox5.Text != "")
                    {
                        gnPrincipal = Convert.ToDecimal(textBox5.Text);
                    }
                    gnTotalAmt = 0.00;
                    textBox12.Text = "";
                    textBox14.Text = "";
                    textBox18.Text = "";
                    textBox19.Text = "";
                    textBox17.Text = "";
                    textBox23.Text = "";
              //      textBox16.Text = "";
                    textBox21.Text = "";
                    textBox9.Text = "";
                }
        //        calcInterest();
                AllClear2Go();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            loanAmort lam = new WinTcare.loanAmort();
            lam.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Visible = false;
            comboBox1.Visible = true;
            refreshVar();
            label6.Text = "Loan Product";
            getprodtype();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Visible = true;
            comboBox1.Visible = false;
            refreshVar();
            label6.Text = "Select Loan";
            string lcmemcode = membcode.Text.Trim();
            if (lcmemcode != "")
            {
                getloans(lcmemcode);
            } 
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Visible = true;
            comboBox1.Visible = false;
            refreshVar();
            label6.Text = "Select Loan";
            string lcmemcode = membcode.Text.Trim();
            if (lcmemcode != "")
            {
                getloans(lcmemcode);
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox2.Focused )
            {
                textBox19.Text = comboBox2.Text;
            }
        }

        private void comboBox8_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox8.Focused )
            {
                DataRow dr = prevLoanView.Rows[comboBox8.SelectedIndex];
                gnIntScope = Convert.ToInt16(prevLoanView.Rows[comboBox8.SelectedIndex]["intscope"]);
                textBox5.Enabled = radioButton3.Checked ? false : true;
                if (gnIntScope == 3)
                {
                    gnOldLoanID = Convert.ToInt16(comboBox8.SelectedValue);
                    gnPrdId = Convert.ToInt16(prevLoanView.Rows[comboBox8.SelectedIndex]["prd_id"]);
                    textBox3.Text = Convert.ToDecimal(prevLoanView.Rows[comboBox8.SelectedIndex]["loanBal"]).ToString("N2");
                    textBox7.Text = Convert.ToDecimal(prevLoanView.Rows[comboBox8.SelectedIndex]["intrate"]).ToString("N2");
                    gnMaxAmt = Convert.ToDouble(prevLoanView.Rows[comboBox8.SelectedIndex]["maxamt"]);
                    textBox20.Text = gnMaxAmt.ToString("N2");
                    textBox14.Text = Convert.ToDecimal(dr["loanamt"]).ToString("N2");
                    textBox12.Text = Convert.ToInt16(dr["ldur"]).ToString();
                    int lnYr = Convert.ToInt16(dr["nyrpay"]);
                    textBox8.Text = lnYr.ToString().Trim();
                    int lnYrFactor = lnYr == 365 ? 0 :
                                        lnYr == 52 ? 1 :
                                        lnYr == 26 ? 2 :
                                        lnYr == 12 ? 3 :
                                        lnYr == 4 ? 4 :
                                        lnYr == 2 ? 5 : 6;
                    comboBox6.SelectedIndex = lnYrFactor;
                    int dmethod = Convert.ToInt32(prevLoanView.Rows[comboBox8.SelectedIndex]["intmethod"]);
                    string intmethod = (dmethod == 1 ? "Reducing/Declining Balance" : (dmethod == 2 ? "Compound Interest" : (dmethod == 3 ? "Simple/flat rate" : "Reducing Balance")));
                    textBox6.Text = intmethod;
                    textBox25.Visible = true;
                    label30.Visible = true;
                    textBox7.Visible = false;
                    label28.Visible = false;

                }
                else
                {
                    gnOldLoanID = Convert.ToInt16(comboBox8.SelectedValue);
                    gnPrdId = Convert.ToInt16(prevLoanView.Rows[comboBox8.SelectedIndex]["prd_id"]);
                    textBox3.Text = Convert.ToDecimal(prevLoanView.Rows[comboBox8.SelectedIndex]["loanBal"]).ToString("N2");
                    textBox7.Text = Convert.ToDecimal(prevLoanView.Rows[comboBox8.SelectedIndex]["intrate"]).ToString("N2");
                    gnMaxAmt = Convert.ToDouble(prevLoanView.Rows[comboBox8.SelectedIndex]["maxamt"]);
                    textBox20.Text = gnMaxAmt.ToString("N2");
                    textBox14.Text = Convert.ToDecimal(dr["loanamt"]).ToString("N2");
                    textBox12.Text = Convert.ToInt16(dr["ldur"]).ToString();
                    int lnYr = Convert.ToInt16(dr["nyrpay"]);
                    textBox8.Text = lnYr.ToString().Trim();
                    int lnYrFactor = lnYr == 365 ? 0 :
                                        lnYr == 52 ? 1 :
                                        lnYr == 26 ? 2 :
                                        lnYr == 12 ? 3 :
                                        lnYr == 4 ? 4 :
                                        lnYr == 2 ? 5 : 6;
                    comboBox6.SelectedIndex = lnYrFactor; 
                    textBox18.Text = Convert.ToDecimal(dr["paymt"]).ToString();
                    int dmethod = Convert.ToInt32(prevLoanView.Rows[comboBox8.SelectedIndex]["intmethod"]);
                    string intmethod = (dmethod == 1 ? "Reducing/Declining Balance" : (dmethod == 2 ? "Compound Interest" : (dmethod == 3 ? "Simple/flat rate" : "Reducing Balance")));
                    textBox6.Text = intmethod;
                    textBox25.Visible = false;
                    label30.Visible = false;
                    textBox7.Visible = true;
                    label28.Visible = true;

                }
            }
          
        }

        private void membcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_Validated(object sender, EventArgs e)
        {
      
        }
    }
}
