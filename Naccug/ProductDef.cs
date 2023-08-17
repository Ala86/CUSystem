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
    public partial class ProductDef : Form

    {
        /*
                 string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        DataTable acctview = new DataTable();
        DataTable transview = new DataTable();
        DataTable runbalview = new DataTable();
        DataTable fixbalview = new DataTable();
        DataTable bkbalview = new DataTable();
        decimal gnRunBal = 0.00m;
        decimal gnBookBal = 0.00m;
        public fixrbal(string tcCos, int tnCompid, string tcLoca, DateTime tdSysDate, string tcUserID, int tnBranchid)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }


             */

        DataTable ProductView = new DataTable();

        DataTable intview = new DataTable();
        DataTable nintview = new DataTable();
        DataTable apview = new DataTable();
        DataTable arview = new DataTable();
        DataTable expview = new DataTable();
        DataTable badincview = new DataTable();
        DataTable badexpview = new DataTable();
        DataTable prodaccview = new DataTable();

        DataTable sproview = new DataTable();
        DataTable sexpview = new DataTable();
        DataTable sarview = new DataTable();
        DataTable sapview = new DataTable();
        DataTable productView = new DataTable();

        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        bool glnewProduct = false;
        string dloca = string.Empty;

        public ProductDef(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void ProductDef_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Product Definition >>";
            getdetails();
            getAccounts();
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
            comboBox29.DataSource = mones;
            comboBox29.SelectedIndex = -1;
        }

        private void proFrequency()
        {
            string[] mones = new string[7];
            mones[0] = "Daily";
            mones[1] = "Weekly";
            mones[2] = "Fortnight";
            mones[3] = "Monthly";
            mones[4] = "Quarterly";
            mones[5] = "Half-Yearly";
            mones[6] = "Yearly";
            comboBox28.DataSource = mones;
            comboBox28.SelectedIndex = -1;
        }
        private void getdetails()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting account type                
                string dsql1 = "select acode,adescrip from acc_type ";
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1 != null)
                {
                    comboBox1.DataSource = ds1.Tables[0];
                    comboBox1.DisplayMember = "adescrip";
                    comboBox1.ValueMember = "acode";
                    comboBox1.SelectedIndex = -1;

                    comboBox27.DataSource = ds1.Tables[0];
                    comboBox27.DisplayMember = "adescrip";
                    comboBox27.ValueMember = "acode";
                    comboBox27.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }


                //************Getting product type                
                string dsql2 = "select prd_id,prd_name from prodtype ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    comboBox17.DataSource = ds2.DefaultView;
                    comboBox17.DisplayMember = "prd_name";
                    comboBox17.ValueMember = "prd_id";
                    comboBox17.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find product types, inform IT Dept immediately"); }
            }
        }

        private void getAccounts()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting accounts                
                string dsql0 = "exec tsp_AssetAccounts " + ncompid;
                string dsql1 = "exec tsp_LiabilityAccounts " + ncompid;
                string dsql2 = "exec tsp_IncomeAccounts " + ncompid;
                string dsql3 = "exec tsp_ExpenseAccounts " + ncompid;

                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle1);
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                SqlDataAdapter da3 = new SqlDataAdapter(dsql3, ndConnHandle1);

                da0.Fill(arview);
                da0.Fill(prodaccview);

                da1.Fill(apview);

                da2.Fill(intview);
                da2.Fill(nintview);
                da2.Fill(badincview);

                da3.Fill(expview);
                da3.Fill(badexpview);


                da0.Fill(sarview);

                da1.Fill(sapview);
                da1.Fill(sproview);

                da3.Fill(sexpview);



                if (intview != null)
                {
                    comboBox2.DataSource = intview.DefaultView;
                    comboBox2.DisplayMember = "cacctname";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (nintview != null)
                {
                    comboBox3.DataSource = nintview.DefaultView;
                    comboBox3.DisplayMember = "cacctname";
                    comboBox3.ValueMember = "cacctnumb";
                    comboBox3.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (expview != null)
                {
                    comboBox4.DataSource = expview.DefaultView;
                    comboBox4.DisplayMember = "cacctname";
                    comboBox4.ValueMember = "cacctnumb";
                    comboBox4.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (arview != null)
                {
                    comboBox5.DataSource = arview.DefaultView;
                    comboBox5.DisplayMember = "cacctname";
                    comboBox5.ValueMember = "cacctnumb";
                    comboBox5.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (apview != null)
                {
                    comboBox6.DataSource = apview.DefaultView;
                    comboBox6.DisplayMember = "cacctname";
                    comboBox6.ValueMember = "cacctnumb";
                    comboBox6.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (badincview != null)
                {
                    comboBox7.DataSource = badincview.DefaultView;
                    comboBox7.DisplayMember = "cacctname";
                    comboBox7.ValueMember = "cacctnumb";
                    comboBox7.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (badexpview != null)
                {
                    comboBox8.DataSource = badexpview.DefaultView;
                    comboBox8.DisplayMember = "cacctname";
                    comboBox8.ValueMember = "cacctnumb";
                    comboBox8.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (prodaccview != null)
                {
                    comboBox9.DataSource = prodaccview.DefaultView;
                    comboBox9.DisplayMember = "cacctname";
                    comboBox9.ValueMember = "cacctnumb";
                    comboBox9.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (sproview != null)
                {
                    comboBox16.DataSource = sproview.DefaultView;
                    comboBox16.DisplayMember = "cacctname";
                    comboBox16.ValueMember = "cacctnumb";
                    comboBox16.SelectedIndex = -1;

                    comboBox26.DataSource = sproview.DefaultView;
                    comboBox26.DisplayMember = "cacctname";
                    comboBox26.ValueMember = "cacctnumb";
                    comboBox26.SelectedIndex = -1;

                    comboBox18.DataSource = sproview.DefaultView;
                    comboBox18.DisplayMember = "cacctname";
                    comboBox18.ValueMember = "cacctnumb";
                    comboBox18.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (sexpview != null)
                {
                    comboBox21.DataSource = sexpview.DefaultView;
                    comboBox21.DisplayMember = "cacctname";
                    comboBox21.ValueMember = "cacctnumb";
                    comboBox21.SelectedIndex = -1;

                    comboBox24.DataSource = sexpview.DefaultView;
                    comboBox24.DisplayMember = "cacctname";
                    comboBox24.ValueMember = "cacctnumb";
                    comboBox24.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (sarview != null)
                {
                    comboBox20.DataSource = sarview.DefaultView;
                    comboBox20.DisplayMember = "cacctname";
                    comboBox20.ValueMember = "cacctnumb";
                    comboBox20.SelectedIndex = -1;

                    comboBox23.DataSource = sarview.DefaultView;
                    comboBox23.DisplayMember = "cacctname";
                    comboBox23.ValueMember = "cacctnumb";
                    comboBox23.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

                if (sapview != null)
                {
                    comboBox19.DataSource = sapview.DefaultView;
                    comboBox19.DisplayMember = "cacctname";
                    comboBox19.ValueMember = "cacctnumb";
                    comboBox19.SelectedIndex = -1;

                    comboBox22.DataSource = sapview.DefaultView;
                    comboBox22.DisplayMember = "cacctname";
                    comboBox22.ValueMember = "cacctnumb";
                    comboBox22.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }

            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
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
            bool tlDeduct = checkBox1.Checked ? (textBox8.Text !="" && comboBox28.SelectedIndex > -1 && comboBox29.SelectedIndex > -1 && comboBox27.SelectedIndex > -1 && comboBox30.SelectedIndex > -1 ? true : false) : true;
            if (Convert.ToInt32(comboBox1.SelectedValue) > 0 && txtProdName.Text.Trim() != string.Empty && tlDeduct)

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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (glnewProduct)
            {
                insertproduct();
            }
            else
            {
                updateproduct();
            }
            initvariables();
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
        }

        private void insertproduct()
        {
            //  MessageBox.Show("inside product step 0");
            string cprodName = txtProdName.Text.ToString().Trim();
            decimal nminAmt = (textBox14.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox14.Text) : 0.00m);
            decimal nmaxAmt = (textBox15.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox15.Text) : 0.00m);
            int nminDur = (textBox16.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox16.Text) : 0);
            int nmaxDur = (textBox17.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox17.Text) : 0);
            int nminMem = (textBox18.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox18.Text) : 0);
            int nmaxMem = (textBox19.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox19.Text) : 0);
            decimal nintRate = (textBox22.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox22.Text) : 0.00m);
            decimal nminSav = (textBox13.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox13.Text) : 0.00m);
            int nageFrom = (textBox20.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox20.Text) : 0);
            int nageTo = (textBox21.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox21.Text) : 0);
            decimal ncommAmt = (textBox10.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox10.Text) : 0.00m);
            decimal nledFees = (textBox9.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox9.Text) : 0.00m);

            //      MessageBox.Show("inside product step 1");
            //            bool lisLed = Convert.ToBoolean(checkBox1.Checked);
            int nprdScope = (radioButton3.Checked ? 1 : (radioButton4.Checked ? 2 : (radioButton5.Checked ? 3 : 4)));
            int nintCalc = (radioButton13.Checked ? 1 : (radioButton14.Checked ? 2 : (radioButton15.Checked ? 3 : (radioButton16.Checked ? 4 : (radioButton8.Checked ? 5 : (radioButton7.Checked ? 6 : 7))))));
            int ndayCount = (radioButton20.Checked ? 1 : (radioButton21.Checked ? 2 : (radioButton22.Checked ? 3 : (radioButton23.Checked ? 4 : 5))));
            int ndayRoll = (radioButton25.Checked ? 1 : (radioButton26.Checked ? 2 : (radioButton27.Checked ? 3 : 4)));
            int lintScope = (radioButton33.Checked ? 1 : (radioButton32.Checked ? 2 : 3));
            bool lcommType = (radioButton10.Checked ? true : false);
            bool ltracking = (radioButton29.Checked ? true : false);
            bool lLedgerType = (radioButton12.Checked ? true : false);

            //     MessageBox.Show("inside product step 2");
            string lintAcct = Convert.ToString(comboBox2.SelectedValue);
            string lnintAcct = Convert.ToString(comboBox3.SelectedValue);
            string lexpAcct = Convert.ToString(comboBox4.SelectedValue);
            string larAcct = Convert.ToString(comboBox5.SelectedValue);
            string lapAcct = Convert.ToString(comboBox6.SelectedValue);
            string lbadebinc = Convert.ToString(comboBox7.SelectedValue);
            string lbadebexp = Convert.ToString(comboBox8.SelectedValue);
            string lprodAcc = Convert.ToString(comboBox9.SelectedValue);
            //   MessageBox.Show("inside product step 3");

            string sproAcc = Convert.ToString(comboBox16.SelectedValue);
            string sexpAcc = Convert.ToString(comboBox21.SelectedValue);
            string sarAcc = Convert.ToString(comboBox20.SelectedValue);
            string sapAcc = Convert.ToString(comboBox19.SelectedValue);
            bool lgender = radioButton42.Checked ? false : true;

            bool tlDeduct = checkBox1.Checked;
            bool tlDestype = radioButton47.Checked ? true : false;
            int tnsouProd = Convert.ToInt16(comboBox27.SelectedValue);
            string tcdesAcct = comboBox30.SelectedIndex > -1 ? comboBox30.SelectedValue.ToString() : "";
            int tnFreq = Convert.ToInt16(comboBox29.SelectedIndex);
            int tnProFreq = Convert.ToInt16(comboBox28.SelectedIndex);

            bool tldedType = radioButton45.Checked ? true : false;
            decimal tndetAmt = textBox8.Text.ToString() != "" ? Convert.ToDecimal(textBox8.Text) : 0.00m;
            //            decimal tndetAmt = !Convert.IsDBNull(textBox8.Text.ToString()) ? Convert.ToDecimal(textBox8.Text) : 0.00m;
            //            decimal tndetAmt = Convert.ToDecimal(textBox8.Text);
            DateTime tdfromdate = dateTimePicker1.Value;
            DateTime tdtodate = dateTimePicker2.Value;

            //  MessageBox.Show("inside product step 4");



            //            string ltranCode = Convert.ToString(comboBox12.SelectedValue);
            bool lprodMan = (radioButton19.Checked ? true : false);

            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery = "Insert Into prodtype (prd_name,prod_scope,min_amt,max_amt,min_dur,max_dur,min_mem,max_mem,int_rate,int_scope,";
                cquery += "min_save,age_from,age_to,comm_type,comm_amt,tracking,int_calc,day_count,day_roll,compid,ledger_fees,ledger_type,maincat,";
                cquery += "int_inc,nint_inc,exp_acc,acc_pay,acc_rec,bad_deb_inc,bad_deb_exp,prod_control,man_product, sprod_control,sexp_acc,sar_acc,sap_acc,gender,";
                cquery += "deduct,destype,souproduct,desacct,freq,dedtype,dedamt,fromdate,todate,profreq)";
                cquery += "values (@lprd_name,@lprod_scope,@lmin_amt,@lmax_amt,@lmin_dur,@lmax_dur,@lmin_mem,@lmax_mem,@lint_rate,@lint_scope,";
                cquery += "@lmin_save,@lage_from,@lage_to,@lcomm_type,@lcomm_amt,@ltracking,@lint_calc,@lday_count,@lday_roll,@lcompid,@lledger_fees,@lledger_type,@lmaincat,";
                cquery += "@lint_inc,@lnint_inc, @lexp_acc, @lacc_pay, @lacc_rec, @lbad_deb_inc, @lbad_deb_exp, @lprod_control,@manproduct,@lsprod_control, @lsexp_acc, @lsar_acc, @lsap_acc,@dgender,";
                cquery += "@lDeduct,@lDestype,@nsouProd,@cdesAcct,@nFreq,@ldedType,@ndedAmt,@dfromdate,@dtodate,@tnProFreq)";
                SqlDataAdapter myCommand = new SqlDataAdapter();
                myCommand.InsertCommand = new SqlCommand(cquery, ndConnHandle1);
                myCommand.InsertCommand.Parameters.Add("@lprd_name", SqlDbType.VarChar).Value = cprodName;
                //                myCommand.InsertCommand.Parameters.Add("@lamt_as", SqlDbType.Bit).Value = lamtType;
                myCommand.InsertCommand.Parameters.Add("@lprod_scope", SqlDbType.Int).Value = nprdScope;
                myCommand.InsertCommand.Parameters.Add("@lmin_amt", SqlDbType.Decimal).Value = nminAmt;
                myCommand.InsertCommand.Parameters.Add("@lmax_amt", SqlDbType.Decimal).Value = nmaxAmt;
                myCommand.InsertCommand.Parameters.Add("@lmin_dur", SqlDbType.Int).Value = nminDur;
                myCommand.InsertCommand.Parameters.Add("@lmax_dur", SqlDbType.Int).Value = nmaxDur;
                myCommand.InsertCommand.Parameters.Add("@lmin_mem", SqlDbType.Int).Value = nminMem;
                myCommand.InsertCommand.Parameters.Add("@lmax_mem", SqlDbType.Int).Value = nmaxMem;
                myCommand.InsertCommand.Parameters.Add("@lint_rate", SqlDbType.Decimal).Value = nintRate;
                //                myCommand.InsertCommand.Parameters.Add("@lint_thres", SqlDbType.Decimal).Value = nintThre;
                myCommand.InsertCommand.Parameters.Add("@lint_scope", SqlDbType.Int).Value = lintScope;
                myCommand.InsertCommand.Parameters.Add("@lmin_save", SqlDbType.Decimal).Value = nminSav;
                myCommand.InsertCommand.Parameters.Add("@lage_from", SqlDbType.Int).Value = nageFrom;
                myCommand.InsertCommand.Parameters.Add("@lage_to", SqlDbType.Int).Value = nageTo;
                myCommand.InsertCommand.Parameters.Add("@lcomm_type", SqlDbType.Bit).Value = lcommType;
                myCommand.InsertCommand.Parameters.Add("@lcomm_amt", SqlDbType.Decimal).Value = ncommAmt;
                myCommand.InsertCommand.Parameters.Add("@ltracking", SqlDbType.Bit).Value = ltracking;
                //                myCommand.InsertCommand.Parameters.Add("@lisledger", SqlDbType.Bit).Value = lisLed; 
                myCommand.InsertCommand.Parameters.Add("@lint_calc", SqlDbType.Int).Value = nintCalc;
                myCommand.InsertCommand.Parameters.Add("@lday_count", SqlDbType.Int).Value = ndayCount;
                myCommand.InsertCommand.Parameters.Add("@lday_roll", SqlDbType.Int).Value = ndayRoll;
                myCommand.InsertCommand.Parameters.Add("@lcompid", SqlDbType.Int).Value = ncompid;
                myCommand.InsertCommand.Parameters.Add("@lledger_type", SqlDbType.Bit).Value = lLedgerType;
                myCommand.InsertCommand.Parameters.Add("@lledger_fees", SqlDbType.Decimal).Value = nledFees;
                myCommand.InsertCommand.Parameters.Add("@lmaincat", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);
                myCommand.InsertCommand.Parameters.Add("@lint_inc", SqlDbType.Char).Value = lintAcct;
                myCommand.InsertCommand.Parameters.Add("@lnint_inc", SqlDbType.Char).Value = lnintAcct;
                myCommand.InsertCommand.Parameters.Add("@lexp_acc", SqlDbType.Char).Value = lexpAcct;
                myCommand.InsertCommand.Parameters.Add("@lacc_pay", SqlDbType.Char).Value = larAcct;
                myCommand.InsertCommand.Parameters.Add("@lacc_rec", SqlDbType.Char).Value = lapAcct;
                myCommand.InsertCommand.Parameters.Add("@lbad_deb_inc", SqlDbType.Char).Value = lbadebinc;
                myCommand.InsertCommand.Parameters.Add("@lbad_deb_exp", SqlDbType.Char).Value = lbadebexp;
                myCommand.InsertCommand.Parameters.Add("@lprod_control", SqlDbType.Char).Value = lprodAcc;
                //                myCommand.InsertCommand.Parameters.Add("@ltrancode", SqlDbType.Char).Value = ltranCode;
                myCommand.InsertCommand.Parameters.Add("@manproduct", SqlDbType.Bit).Value = lprodMan;

                myCommand.InsertCommand.Parameters.Add("@lsprod_control", SqlDbType.Char).Value = sproAcc;
                myCommand.InsertCommand.Parameters.Add("@lsexp_acc", SqlDbType.Char).Value = sexpAcc;
                myCommand.InsertCommand.Parameters.Add("@lsar_acc", SqlDbType.Char).Value = sarAcc;
                myCommand.InsertCommand.Parameters.Add("@lsap_acc", SqlDbType.Char).Value = sapAcc;
                myCommand.InsertCommand.Parameters.Add("@dgender", SqlDbType.Bit).Value = lgender;
                //                @,@,@,@,@,@,@

                myCommand.InsertCommand.Parameters.Add("@lDeduct", SqlDbType.Bit).Value = tlDeduct;
                myCommand.InsertCommand.Parameters.Add("@lDestype", SqlDbType.Bit).Value = tlDestype;
                myCommand.InsertCommand.Parameters.Add("@nsouProd", SqlDbType.Int).Value = tnsouProd;
                myCommand.InsertCommand.Parameters.Add("@cdesAcct", SqlDbType.Char).Value = tcdesAcct;
                myCommand.InsertCommand.Parameters.Add("@nFreq", SqlDbType.Int).Value = tnFreq;
                myCommand.InsertCommand.Parameters.Add("@tnProFreq", SqlDbType.Int).Value = tnProFreq;
                myCommand.InsertCommand.Parameters.Add("@ldedType", SqlDbType.Bit).Value = tldedType;
                myCommand.InsertCommand.Parameters.Add("@ndedAmt", SqlDbType.Decimal).Value = tndetAmt;

                myCommand.InsertCommand.Parameters.Add("@dfromdate", SqlDbType.DateTime).Value = tdfromdate;
                myCommand.InsertCommand.Parameters.Add("@dtodate", SqlDbType.DateTime).Value = tdtodate; ;

                ndConnHandle1.Open();
                myCommand.InsertCommand.ExecuteNonQuery();  //Insert new record                                                            //                myCommand1.InsertCommand.ExecuteNonQuery();  //Insert new record
                ndConnHandle1.Close();
            }
        }

        private void initvariables()
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = comboBox5.SelectedIndex = comboBox6.SelectedIndex = comboBox7.SelectedIndex = comboBox8.SelectedIndex =
            comboBox9.SelectedIndex = comboBox16.SelectedIndex = comboBox21.SelectedIndex = comboBox20.SelectedIndex = comboBox19.SelectedIndex = comboBox28.SelectedIndex = comboBox29.SelectedIndex = comboBox27.SelectedIndex = comboBox30.SelectedIndex =  -1;
            glnewProduct = comboBox1.Enabled = txtProdName.Enabled = comboBox17.Visible = DedActGrp.Enabled = radioButton47.Checked = radioButton46.Checked = radioButton45.Checked = radioButton44.Checked =  false;
            txtProdName.Visible = true;
            txtProdName.Text = textBox9.Text = textBox10.Text = textBox14.Text = textBox15.Text = textBox8.Text= textBox16.Text = textBox17.Text = textBox18.Text = textBox19.Text = textBox20.Text = textBox21.Text = textBox22.Text = textBox13.Text = "";
            getdetails();
        }

        private void updateproduct()
        {
            string cprodName = txtProdName.Text.ToString().Trim();
            decimal nminAmt = (textBox14.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox14.Text) : 0.00m);
            decimal nmaxAmt = (textBox15.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox15.Text) : 0.00m);
            int nminDur = (textBox16.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox16.Text) : 0);
            int nmaxDur = (textBox17.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox17.Text) : 0);
            int nminMem = (textBox18.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox18.Text) : 0);
            int nmaxMem = (textBox19.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox19.Text) : 0);
            decimal nintRate = (textBox22.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox22.Text) : 0.00m);
            decimal nminSav = (textBox13.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox13.Text) : 0.00m);
            int nageFrom = (textBox20.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox20.Text) : 0);
            int nageTo = (textBox21.Text.ToString().Trim() != "" ? Convert.ToInt32(textBox21.Text) : 0);
            decimal ncommAmt = (textBox10.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox10.Text) : 0.00m);
            decimal nledFees = (textBox9.Text.ToString().Trim() != "" ? Convert.ToDecimal(textBox9.Text) : 0.00m);

            int nprdScope = (radioButton3.Checked ? 1 : (radioButton4.Checked ? 2 : (radioButton5.Checked ? 3 : 4)));
            int nintCalc = (radioButton13.Checked ? 1 : (radioButton14.Checked ? 2 : (radioButton15.Checked ? 3 : (radioButton16.Checked ? 4 : (radioButton8.Checked ? 5 : (radioButton7.Checked ? 6 : 7))))));
            //            int nintCalc = (radioButton13.Checked ? 1 : (radioButton14.Checked ? 2 : (radioButton15.Checked ? 3 : 4)));
            int ndayCount = (radioButton20.Checked ? 1 : (radioButton21.Checked ? 2 : (radioButton22.Checked ? 3 : (radioButton23.Checked ? 4 : 5))));
            int ndayRoll = (radioButton25.Checked ? 1 : (radioButton26.Checked ? 2 : (radioButton27.Checked ? 3 : 4)));
            int lintScope = (radioButton33.Checked ? 1 : (radioButton32.Checked ? 2 : 3));
            bool lcommType = (radioButton10.Checked ? true : false);
            bool ltracking = (radioButton29.Checked ? true : false);
            bool lLedgerType = (radioButton12.Checked ? true : false);

            string lintAcct = Convert.ToString(comboBox2.SelectedValue);
            string lnintAcct = Convert.ToString(comboBox3.SelectedValue);
            string lexpAcct = Convert.ToString(comboBox4.SelectedValue);
            string larAcct = Convert.ToString(comboBox5.SelectedValue);
            string lapAcct = Convert.ToString(comboBox6.SelectedValue);
            string lbadebinc = Convert.ToString(comboBox7.SelectedValue);
            string lbadebexp = Convert.ToString(comboBox8.SelectedValue);
          //  string lprodAcc = Convert.ToString(comboBox9.SelectedValue);
            string lprodAcc = comboBox9.SelectedIndex > -1 ? comboBox9.SelectedValue.ToString() : comboBox26.SelectedValue.ToString();


            string sproAcc = Convert.ToString(comboBox16.SelectedValue);
            string sexpAcc = Convert.ToString(comboBox21.SelectedValue);
            string sarAcc = Convert.ToString(comboBox20.SelectedValue);
            string sapAcc = Convert.ToString(comboBox19.SelectedValue);
            int lnprd_id = Convert.ToInt16(comboBox17.SelectedValue);
            bool lgender = radioButton42.Checked ? false : true;

            bool tlDeduct = checkBox1.Checked;
            bool tlDestype = radioButton47.Checked ? true : false;
            int tnsouProd = Convert.ToInt16(comboBox27.SelectedValue);
            string tcdesAcct = comboBox30.SelectedIndex > -1 ? comboBox30.SelectedValue.ToString() : "";
            int tnFreq = Convert.ToInt16(comboBox29.SelectedIndex);
            int tnProFreq = Convert.ToInt16(comboBox28.SelectedIndex);

            bool tldedType = radioButton45.Checked ? true : false;
            decimal tndetAmt = textBox8.Text.ToString() != "" ? Convert.ToDecimal(textBox8.Text) : 0.00m;

            DateTime tdfromdate = dateTimePicker1.Value;
            DateTime tdtodate = dateTimePicker2.Value;


            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery = "update prodtype set prd_name=@lprd_name,prod_scope=@lprod_scope,min_amt=@lmin_amt,max_amt=@lmax_amt,min_dur=@lmin_dur,max_dur=@lmax_dur,min_mem=@lmin_mem,max_mem=@lmax_mem,int_rate=@lint_rate,";
                cquery += "int_scope=@lint_scope,min_save=@lmin_save,age_from=@lage_from,age_to=@lage_to,comm_type=@lcomm_type,comm_amt=@lcomm_amt,tracking=@ltracking,int_calc=@lint_calc,day_count=@lday_count,";
                cquery += "day_roll =@lday_roll,ledger_fees=@lledger_fees,ledger_type=@lledger_type,maincat=@lmaincat,int_inc=@lint_inc,nint_inc=@lnint_inc,exp_acc=@lexp_acc,acc_pay=@lacc_pay,acc_rec=@lacc_rec,bad_deb_inc=@lbad_deb_inc,";
                cquery += "bad_deb_exp =@lbad_deb_exp,prod_control=@lprod_control,sprod_control= @lsprod_control,sexp_acc=@lsexp_acc,sar_acc= @lsar_acc,sap_acc=@lsap_acc,gender = @dgender, ";
                cquery += "deduct=@lDeduct,destype=@lDestype,souproduct=@nsouProd,desacct=@cdesAcct,freq=@nFreq,dedtype=@ldedType,dedamt=@ndedAmt,fromdate = @dfromdate, todate = @dtodate,profreq=@tnProFreq  where prd_id=@lprd_id";



                SqlDataAdapter myCommand1 = new SqlDataAdapter();
                myCommand1.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);
                myCommand1.UpdateCommand.Parameters.Add("@lprd_name", SqlDbType.VarChar).Value = cprodName;
                myCommand1.UpdateCommand.Parameters.Add("@lprod_scope", SqlDbType.Int).Value = nprdScope;
                myCommand1.UpdateCommand.Parameters.Add("@lmin_amt", SqlDbType.Decimal).Value = nminAmt;
                myCommand1.UpdateCommand.Parameters.Add("@lmax_amt", SqlDbType.Decimal).Value = nmaxAmt;
                myCommand1.UpdateCommand.Parameters.Add("@lmin_dur", SqlDbType.Int).Value = nminDur;
                myCommand1.UpdateCommand.Parameters.Add("@lmax_dur", SqlDbType.Int).Value = nmaxDur;
                myCommand1.UpdateCommand.Parameters.Add("@lmin_mem", SqlDbType.Int).Value = nminMem;
                myCommand1.UpdateCommand.Parameters.Add("@lmax_mem", SqlDbType.Int).Value = nmaxMem;

                myCommand1.UpdateCommand.Parameters.Add("@lint_rate", SqlDbType.Decimal).Value = nintRate;
                myCommand1.UpdateCommand.Parameters.Add("@lint_scope", SqlDbType.Int).Value = lintScope;
                myCommand1.UpdateCommand.Parameters.Add("@lmin_save", SqlDbType.Decimal).Value = nminSav;
                myCommand1.UpdateCommand.Parameters.Add("@lage_from", SqlDbType.Int).Value = nageFrom;
                myCommand1.UpdateCommand.Parameters.Add("@lage_to", SqlDbType.Int).Value = nageTo;
                myCommand1.UpdateCommand.Parameters.Add("@lcomm_type", SqlDbType.Bit).Value = lcommType;
                myCommand1.UpdateCommand.Parameters.Add("@lcomm_amt", SqlDbType.Decimal).Value = ncommAmt;
                myCommand1.UpdateCommand.Parameters.Add("@ltracking", SqlDbType.Bit).Value = ltracking;

                myCommand1.UpdateCommand.Parameters.Add("@lint_calc", SqlDbType.Int).Value = nintCalc;
                myCommand1.UpdateCommand.Parameters.Add("@lday_count", SqlDbType.Int).Value = ndayCount;
                myCommand1.UpdateCommand.Parameters.Add("@lday_roll", SqlDbType.Int).Value = ndayRoll;
                myCommand1.UpdateCommand.Parameters.Add("@lledger_type", SqlDbType.Bit).Value = lLedgerType;
                myCommand1.UpdateCommand.Parameters.Add("@lledger_fees", SqlDbType.Decimal).Value = nledFees;
                myCommand1.UpdateCommand.Parameters.Add("@lmaincat", SqlDbType.Int).Value = Convert.ToInt32(comboBox1.SelectedValue);

                myCommand1.UpdateCommand.Parameters.Add("@lint_inc", SqlDbType.Char).Value = lintAcct;
                myCommand1.UpdateCommand.Parameters.Add("@lnint_inc", SqlDbType.Char).Value = lnintAcct;
                myCommand1.UpdateCommand.Parameters.Add("@lexp_acc", SqlDbType.Char).Value = lexpAcct;
                myCommand1.UpdateCommand.Parameters.Add("@lacc_pay", SqlDbType.Char).Value = lapAcct;
                myCommand1.UpdateCommand.Parameters.Add("@lacc_rec", SqlDbType.Char).Value = larAcct;
                myCommand1.UpdateCommand.Parameters.Add("@lbad_deb_inc", SqlDbType.Char).Value = lbadebinc;
                myCommand1.UpdateCommand.Parameters.Add("@lbad_deb_exp", SqlDbType.Char).Value = lbadebexp;
                myCommand1.UpdateCommand.Parameters.Add("@lprod_control", SqlDbType.Char).Value = lprodAcc;
                myCommand1.UpdateCommand.Parameters.Add("@lprd_id", SqlDbType.Int).Value = lnprd_id;

                myCommand1.UpdateCommand.Parameters.Add("@lsprod_control", SqlDbType.Char).Value = sproAcc;
                myCommand1.UpdateCommand.Parameters.Add("@lsexp_acc", SqlDbType.Char).Value = sexpAcc;
                myCommand1.UpdateCommand.Parameters.Add("@lsar_acc", SqlDbType.Char).Value = sarAcc;
                myCommand1.UpdateCommand.Parameters.Add("@lsap_acc", SqlDbType.Char).Value = sapAcc;
                myCommand1.UpdateCommand.Parameters.Add("@dgender", SqlDbType.Bit).Value = lgender;

                myCommand1.UpdateCommand.Parameters.Add("@lDeduct", SqlDbType.Bit).Value = tlDeduct;
                myCommand1.UpdateCommand.Parameters.Add("@lDestype", SqlDbType.Bit).Value = tlDestype;
                myCommand1.UpdateCommand.Parameters.Add("@nsouProd", SqlDbType.Int).Value = tnsouProd;
                myCommand1.UpdateCommand.Parameters.Add("@cdesAcct", SqlDbType.Char).Value = tcdesAcct;
                myCommand1.UpdateCommand.Parameters.Add("@nFreq", SqlDbType.Int).Value = tnFreq;
                myCommand1.UpdateCommand.Parameters.Add("@tnProFreq", SqlDbType.Int).Value = tnProFreq;
                
                myCommand1.UpdateCommand.Parameters.Add("@ldedType", SqlDbType.Bit).Value = tldedType;
                myCommand1.UpdateCommand.Parameters.Add("@ndedAmt", SqlDbType.Decimal).Value = tndetAmt;

                myCommand1.UpdateCommand.Parameters.Add("@dfromdate", SqlDbType.DateTime).Value = tdfromdate;
                myCommand1.UpdateCommand.Parameters.Add("@dtodate", SqlDbType.DateTime).Value = tdtodate; ;

                ndConnHandle1.Open();
                myCommand1.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }


        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void newButton_Click(object sender, EventArgs e)
        {
            glnewProduct = true;
            txtProdName.Visible = true;
            comboBox17.Visible = false;
            comboBox1.Enabled = true;
            txtProdName.Enabled = true;
            comboBox1.Focus();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtProdName.Visible = false;
            comboBox17.Visible = true;
            comboBox17.Enabled = true;
            comboBox1.Enabled = false;
            comboBox17.Focus();
            glnewProduct = false;
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void txtProdName_Leave(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                string cCatSwitch = comboBox1.SelectedValue.ToString();
                if (cCatSwitch == "130" || cCatSwitch == "131")  //conventional and sharia loans
                {
                    intPanel.Enabled = loansPanel.Enabled = true;
                    CurActGrp.Enabled = DedActGrp.Enabled = savingsPanel.Enabled = false;
                }
                else if (cCatSwitch == "250" || cCatSwitch == "251" || cCatSwitch == "270" || cCatSwitch == "271") //conventional and sharia share accounts
                {
                    savingsPanel.Enabled = true;
                    intPanel.Enabled = loansPanel.Enabled = CurActGrp.Enabled = DedActGrp.Enabled = false;
                }
                else //other deductions
                {
                    intPanel.Enabled = loansPanel.Enabled = false;
                    CurActGrp.Enabled = DedActGrp.Enabled = false;
                    savingsPanel.Enabled = false;
                }


                if (Convert.ToString(comboBox1.SelectedValue) == "130")
                {
                    textBox16.Enabled = textBox17.Enabled = textBox13.Enabled = radioButton29.Enabled = radioButton280.Enabled = intPanel.Enabled = loansPanel.Enabled = panel7.Enabled = panel8.Enabled = true;

                    radioButton29.Enabled = radioButton280.Enabled = radioButton33.Enabled = comboBox2.Enabled = comboBox3.Enabled =
                    comboBox4.Enabled = comboBox5.Enabled = comboBox6.Enabled = comboBox7.Enabled = comboBox8.Enabled = comboBox9.Enabled = comboBox10.Enabled = comboBox11.Enabled = true;

                    radioButton32.Enabled = radioButton7.Enabled = radioButton8.Enabled = comboBox14.Enabled = comboBox15.Enabled = comboBox16.Enabled = comboBox19.Enabled = comboBox20.Enabled =
                        savingsPanel.Enabled = DedActGrp.Enabled = CurActGrp.Enabled = comboBox21.Enabled = radioButton8.Enabled = radioButton7.Enabled = radioButton41.Enabled = checkBox1.Checked = false;
                }
                else
                {
                    radioButton29.Enabled = radioButton280.Enabled = textBox16.Enabled = textBox17.Enabled = textBox13.Enabled = radioButton29.Enabled = radioButton280.Enabled = intPanel.Enabled = loansPanel.Enabled = panel7.Enabled = panel8.Enabled = false;

                    radioButton33.Enabled = radioButton32.Enabled = radioButton7.Enabled =
                    radioButton8.Enabled = comboBox14.Enabled = comboBox15.Enabled = comboBox16.Enabled = comboBox19.Enabled = comboBox20.Enabled = comboBox21.Enabled = radioButton8.Enabled =
                    radioButton7.Enabled = radioButton41.Enabled = checkBox1.Enabled = true;

                    comboBox2.Enabled = comboBox3.Enabled = comboBox4.Enabled = comboBox5.Enabled = comboBox6.Enabled = comboBox7.Enabled = comboBox8.Enabled = comboBox9.Enabled = intPanel.Enabled = loansPanel.Enabled =
                    comboBox10.Enabled = comboBox11.Enabled = checkBox1.Checked =   false;

                }
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox18.Enabled = textBox19.Enabled = true;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox18.Enabled = textBox19.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox18.Enabled = textBox19.Enabled = false;
        }

        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {
            textBox18.Enabled = textBox19.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox18.Enabled = textBox19.Enabled = false;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            label16.Text = "Percentage ";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            label16.Text = "Value ";
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            label15.Text = "Percentage ";
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            label15.Text = "Value ";
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            label22.Text = "Percentage ";
        }

        private void radioButton39_CheckedChanged(object sender, EventArgs e)
        {
            label22.Text = "Value ";
        }

        private void comboBox17_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox17.Focused)
            {
                int tnProductID = Convert.ToInt16(comboBox17.SelectedValue);
                getProductDetails(tnProductID);
                AllClear2Go();
            }
        }

        private void getProductDetails(int tnPrdID)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                productView.Clear();
                string dsql01 = "select * from prodtype where prd_id = " + tnPrdID;
                SqlDataAdapter da01 = new SqlDataAdapter(dsql01, ndConnHandle1);
                da01.Fill(productView);
                if (productView != null && productView.Rows.Count > 0)
                {
                    string dswitch = string.Empty;
                    dswitch = Convert.ToString(productView.Rows[0]["maincat"]);

                    //switch (dswitch)
                    if (dswitch == "130" || dswitch == "131") //Conventional and Sharia Loans
                    {
                        textBox16.Enabled = textBox17.Enabled = textBox13.Enabled = radioButton29.Enabled = radioButton280.Enabled = intPanel.Enabled = loansPanel.Enabled = panel8.Enabled =
                        radioButton29.Enabled = radioButton280.Enabled = radioButton33.Enabled = comboBox2.Enabled = comboBox3.Enabled =
                        comboBox4.Enabled = comboBox5.Enabled = comboBox6.Enabled = comboBox7.Enabled = comboBox8.Enabled = comboBox9.Enabled = comboBox10.Enabled = comboBox11.Enabled = true;
                        radioButton32.Enabled = radioButton7.Enabled = radioButton8.Enabled = comboBox14.Enabled = comboBox15.Enabled = comboBox16.Enabled = comboBox19.Enabled = comboBox20.Enabled =
                        comboBox21.Enabled = radioButton8.Enabled = radioButton7.Enabled = radioButton41.Enabled = CurActGrp.Enabled = DedActGrp.Enabled = false;
                        comboBox18.Enabled = comboBox22.Enabled = comboBox23.Enabled = comboBox24.Enabled = false;
                    }
                    else if (dswitch == "250" || dswitch == "251") //Convention and Sharia Savings
                    {
                        radioButton29.Enabled = radioButton280.Enabled = textBox16.Enabled = textBox17.Enabled = textBox13.Enabled = radioButton29.Enabled = radioButton280.Enabled = intPanel.Enabled = loansPanel.Enabled = panel7.Enabled = panel8.Enabled = false;
                        radioButton33.Enabled = radioButton32.Enabled = radioButton7.Enabled =
                        radioButton8.Enabled = comboBox14.Enabled = comboBox15.Enabled = comboBox16.Enabled = comboBox19.Enabled = comboBox20.Enabled = comboBox21.Enabled = radioButton8.Enabled =
                        radioButton7.Enabled = radioButton41.Enabled = savingsPanel.Enabled = true;
                        comboBox2.Enabled = comboBox3.Enabled = comboBox4.Enabled = comboBox5.Enabled = comboBox6.Enabled = comboBox7.Enabled = comboBox8.Enabled = comboBox9.Enabled =
                        comboBox10.Enabled = comboBox11.Enabled = false;
                        comboBox18.Enabled = comboBox22.Enabled = comboBox23.Enabled = comboBox24.Enabled = CurActGrp.Enabled = DedActGrp.Enabled = false;
                    }
                    else if (dswitch == "270" || dswitch == "271") //Conventional and Sharia Shares
                    {
                        radioButton29.Enabled = radioButton280.Enabled = textBox16.Enabled = textBox17.Enabled = textBox13.Enabled = radioButton29.Enabled = radioButton280.Enabled = intPanel.Enabled = loansPanel.Enabled = panel8.Enabled = false;
                        radioButton33.Enabled = radioButton32.Enabled = radioButton7.Enabled =
                        radioButton8.Enabled = comboBox14.Enabled = comboBox15.Enabled = comboBox16.Enabled = comboBox19.Enabled = comboBox20.Enabled = comboBox21.Enabled = radioButton8.Enabled =
                        radioButton7.Enabled = radioButton41.Enabled = savingsPanel.Enabled = true;
                        comboBox2.Enabled = comboBox3.Enabled = comboBox4.Enabled = comboBox5.Enabled = comboBox6.Enabled = comboBox7.Enabled = comboBox8.Enabled = comboBox9.Enabled =
                        comboBox10.Enabled = comboBox11.Enabled = false;
                        comboBox18.Enabled = comboBox22.Enabled = comboBox23.Enabled = comboBox24.Enabled = CurActGrp.Enabled = DedActGrp.Enabled = false;
                    }
                    else   //all other products
                    {
                        radioButton29.Enabled = radioButton280.Enabled = textBox16.Enabled = textBox17.Enabled = textBox13.Enabled = radioButton29.Enabled = radioButton280.Enabled = intPanel.Enabled = loansPanel.Enabled = panel8.Enabled = false;
                        radioButton33.Enabled = radioButton32.Enabled = radioButton7.Enabled =
                        radioButton8.Enabled = comboBox14.Enabled = comboBox15.Enabled = comboBox16.Enabled = comboBox19.Enabled = comboBox20.Enabled = comboBox21.Enabled = radioButton8.Enabled =
                        radioButton7.Enabled = radioButton41.Enabled = savingsPanel.Enabled = CurActGrp.Enabled = DedActGrp.Enabled = false;
                        comboBox2.Enabled = comboBox3.Enabled = comboBox4.Enabled = comboBox5.Enabled = comboBox6.Enabled = comboBox7.Enabled = comboBox8.Enabled = comboBox9.Enabled =
                        comboBox10.Enabled = comboBox11.Enabled = false;
                        comboBox18.Enabled = comboBox22.Enabled = comboBox23.Enabled = comboBox24.Enabled = true;
                    }


                    txtProdName.Text = productView.Rows[0]["prd_name"].ToString();
                    textBox9.Text = productView.Rows[0]["ledger_fees"].ToString();
                    textBox13.Text = productView.Rows[0]["min_save"].ToString();
                    textBox14.Text = Convert.ToDecimal(productView.Rows[0]["min_amt"]).ToString("N2");
                    textBox15.Text = Convert.ToDecimal(productView.Rows[0]["max_amt"]).ToString("N2");
                    textBox16.Text = productView.Rows[0]["min_dur"].ToString();
                    textBox17.Text = productView.Rows[0]["max_dur"].ToString();
                    textBox18.Text = productView.Rows[0]["min_mem"].ToString();
                    textBox19.Text = productView.Rows[0]["max_mem"].ToString();
                    textBox22.Text = productView.Rows[0]["int_rate"].ToString();
                    textBox20.Text = productView.Rows[0]["age_from"].ToString();
                    textBox21.Text = productView.Rows[0]["age_to"].ToString();
                    textBox10.Text = productView.Rows[0]["comm_amt"].ToString();

                    radioButton10.Checked = Convert.ToBoolean(productView.Rows[0]["comm_type"]);
                    radioButton9.Checked = !Convert.ToBoolean(productView.Rows[0]["comm_type"]);
                    radioButton29.Checked = Convert.ToBoolean(productView.Rows[0]["tracking"]);
                    radioButton280.Checked = !Convert.ToBoolean(productView.Rows[0]["tracking"]);
                    radioButton33.Checked = Convert.ToInt16(productView.Rows[0]["int_scope"]) == 1 ? true : false;
                    radioButton32.Checked = Convert.ToInt16(productView.Rows[0]["int_scope"]) == 2 ? true : false;
                    radioButton17.Checked = Convert.ToInt16(productView.Rows[0]["int_scope"]) == 3 ? true : false;
                    radioButton13.Checked = Convert.ToBoolean(productView.Rows[0]["ledger_type"]);
                    radioButton11.Checked = !Convert.ToBoolean(productView.Rows[0]["ledger_type"]);
                    radioButton42.Checked = !Convert.IsDBNull(productView.Rows[0]["gender"]) ? !Convert.ToBoolean(productView.Rows[0]["gender"]) : false;
                    radioButton2.Checked = !Convert.IsDBNull(productView.Rows[0]["gender"]) ? Convert.ToBoolean(productView.Rows[0]["gender"]) : false;


                    comboBox1.SelectedValue = productView.Rows[0]["maincat"];

                    comboBox2.SelectedValue = productView.Rows[0]["int_inc"].ToString();

                    comboBox3.SelectedValue = productView.Rows[0]["nint_inc"].ToString();
                    comboBox4.SelectedValue = productView.Rows[0]["exp_acc"].ToString();
                    comboBox5.SelectedValue = productView.Rows[0]["acc_rec"].ToString();
                    comboBox6.SelectedValue = productView.Rows[0]["acc_pay"].ToString();
                    comboBox7.SelectedValue = productView.Rows[0]["bad_deb_inc"].ToString();
                    comboBox8.SelectedValue = productView.Rows[0]["bad_deb_exp"].ToString();
                    comboBox9.SelectedValue = productView.Rows[0]["prod_control"].ToString();
                    comboBox16.SelectedValue = productView.Rows[0]["sprod_control"].ToString();
                    comboBox21.SelectedValue = productView.Rows[0]["sexp_acc"].ToString();
                    comboBox20.SelectedValue = productView.Rows[0]["sar_acc"].ToString();
                    comboBox19.SelectedValue = productView.Rows[0]["sap_acc"].ToString();
                    comboBox26.SelectedValue = productView.Rows[0]["sprod_control"].ToString();
                }
                else { MessageBox.Show("Could not find Product Details, inform IT Dept immediately"); }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Focused)
            {
                DedActGrp.Enabled = checkBox1.Checked ? true : false;
                intPanel.Enabled = CurActGrp.Enabled = loansPanel.Enabled = savingsPanel.Enabled = comboBox27.Enabled = false;
                radioButton8.Enabled = radioButton7.Enabled = radioButton41.Enabled = radioButton40.Enabled = radioButton39.Enabled = radioButton12.Enabled = radioButton11.Enabled =
                    radioButton10.Enabled = radioButton9.Enabled = radioButton22.Enabled = radioButton13.Enabled = radioButton29.Enabled = radioButton280.Enabled =
                    textBox10.Enabled = textBox9.Enabled = textBox7.Enabled = checkBox1.Checked ? false : true;
                comboBox27.SelectedValue = comboBox1.SelectedValue;
                numofpayments();
                proFrequency();
                AllClear2Go();
            } 
        }

        private void getDestAccounts(string tcacctype)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting account type                
                string dsql1a = "select acode,adescrip from acc_type where acode <> "+"'"+tcacctype+"'";
                SqlDataAdapter da1a = new SqlDataAdapter(dsql1a, ndConnHandle1);
                DataTable ds1a = new DataTable();
                da1a.Fill(ds1a);
                if (ds1a != null && ds1a.Rows.Count>0)
                {
                    comboBox30.DataSource = ds1a.DefaultView; 
                    comboBox30.DisplayMember = "adescrip";
                    comboBox30.ValueMember = "acode";
                    comboBox30.SelectedIndex = -1;
                }
//                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }


            }
        }

        private void getDIncomeAccounts()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                //************Getting account type                
                string dincsql = "exec tsp_IncomeAccounts " + ncompid;
                SqlDataAdapter dainc = new SqlDataAdapter(dincsql, ndConnHandle1);
                DataTable dtinc = new DataTable();
                dainc.Fill(dtinc);
                if (dtinc != null && dtinc.Rows.Count > 0)
                {
                    comboBox30.DataSource = dtinc.DefaultView;
                    comboBox30.DisplayMember = "cacctname";
                    comboBox30.ValueMember = "cacctnumb";
                    comboBox30.SelectedIndex = -1;
                }
                //                else { MessageBox.Show("Could not find main accounts, inform IT Dept immediately"); }


            }
        }
        


        private void radioButton47_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton47.Focused)
            {
                if (radioButton47.Checked)
                {
                    string dacctype = comboBox1.SelectedValue.ToString().Trim();
                    getDestAccounts(dacctype);
                }
                AllClear2Go();
            }
        }

        private void radioButton46_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton46.Focused)
            {
                if (radioButton46.Checked)
                {
                    getDIncomeAccounts();
                }
                AllClear2Go();
            }

        }

        private void textBox8_Validated(object sender, EventArgs e)
        {
                textBox8.Text = textBox8.Text != "" ? Convert.ToDecimal(textBox8.Text).ToString("N2") : "";
            AllClear2Go();
        }

        private void comboBox30_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox30.Focused)
            {
                AllClear2Go();
            }
        }

        private void comboBox29_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox29.Focused )
            {
                AllClear2Go();
            }
        }

        private void radioButton45_CheckedChanged(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void radioButton44_CheckedChanged(object sender, EventArgs e)
        {
            AllClear2Go();
        }

        private void comboBox28_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox28.Focused)
            {
                AllClear2Go();
            }
        }

        private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox26_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}