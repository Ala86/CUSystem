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
using System.Threading;

namespace WinTcare
{
    public partial class batchProcess : Form
    {
        DataTable clientview = new DataTable();
        DataTable batview = new DataTable();
        DataTable batdetview = new DataTable();
        DataTable bnkview = new DataTable();
        DataTable batoneview = new DataTable();
        string gcControlAcct = string.Empty;
        string cs = globalvar.cos;
        string gcMembCode = "";
        int ncompid = globalvar.gnCompid;
//        private BackgroundWorker _BackgroundWorker;
  //      private Random _Random;

        public batchProcess()
        {
            InitializeComponent();
 
        }

    private void batchProcess_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Batch Processing >> " ;
            getBatchList();
            BnkAccts();
        }

        private void getBatchList()
        {
            string dsqlb = " exec Tsp_Batch4Processing "+ncompid;
            batview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                dab.Fill(batview);
                if (batview.Rows.Count > 0)
                {
                    comboBox2.DataSource = batview.DefaultView;
                    comboBox2.DisplayMember = "bat_name";
                    comboBox2.ValueMember = "bat_id";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }//end of getBatchlist

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
            if (textBox3.Text != "" )
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

        private void BnkAccts()
        {
            string dsql1 = "exec tsp_getBankAccounts  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(bnkview);
                if (bnkview.Rows.Count > 0)
                {
                    comboBox1.DataSource = bnkview.DefaultView;
                    comboBox1.DisplayMember = "cacctname";
                    comboBox1.ValueMember = "cacctnumb";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }
        private void getdetails(int batid)
        {
            string dsqlb = " exec Tsp_BatchMembers4Processing_one "+ncompid+"," + batid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                dab.Fill(batoneview);
                if (batoneview.Rows.Count > 0)
                {
                    textBox4.Text = batoneview.Rows.Count.ToString();
                    textBox3.Text = batoneview.Rows[0]["bat_cont"].ToString();
                    textBox1.Text = batoneview.Rows[0]["cont_name"].ToString();
//                    textBox5.Text = batoneview.Rows[0]["cont_bal"].ToString();
                    AllClear2Go();
                }else { MessageBox.Show("Batch already processed"); }
            }

        }

    
    private void postTransactions(string actnumb,decimal ntranamt)
        {
            Thread.Sleep(500);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string acct = "";
            decimal ncontAmt = 0.00m;
            decimal nSaveCont_Ind = 0.00m;
            decimal nSaveCont_Cor = 0.00m;
            decimal nSaveCont_Grp = 0.00m;
            decimal nSaveCont_Stf = 0.00m;
            decimal nShareCont_Ind = 0.00m;
            decimal nShareCont_Cor = 0.00m;
            decimal nShareCont_Grp = 0.00m;
            decimal nShareCont_Stf = 0.00m;
            string gcContra = textBox3.Text;

            int j = 0;
            int linecount = batoneview.Rows.Count;
            for (int i=0;i<batoneview.Rows.Count;i++)
            {
                j = i + 1;
                progressBar1.Value = j * progressBar1.Maximum / linecount;

                int presentage = (j * 100) / linecount;
                LabelPresentage.Text = presentage.ToString() + " %"; //show precentage in lable
                textBox12.Text = presentage.ToString(); 
                acct = batoneview.Rows[i]["acctnumb"].ToString();
                ncontAmt = Convert.ToDecimal(batoneview.Rows[i]["ncont"]);

                //control amounts 
                nSaveCont_Ind = nSaveCont_Ind + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"])  == 1 ? ncontAmt : 0.00m);
                nSaveCont_Cor = nSaveCont_Cor + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"])  == 2 ? ncontAmt : 0.00m);
                nSaveCont_Grp = nSaveCont_Grp + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"])  == 3 ? ncontAmt : 0.00m);
                nSaveCont_Stf = nSaveCont_Stf + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"])  == 4 ? ncontAmt : 0.00m);
                nShareCont_Ind = nSaveCont_Ind + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"]) == 1 ? ncontAmt : 0.00m);
                nShareCont_Cor = nSaveCont_Cor + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"]) == 2 ? ncontAmt : 0.00m);
                nShareCont_Grp = nSaveCont_Grp + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"]) == 3 ? ncontAmt : 0.00m);
                nShareCont_Stf = nSaveCont_Stf + (acct.Substring(0, 3) == "250" && Convert.ToInt32(batoneview.Rows[i]["mem_type"]) == 4 ? ncontAmt : 0.00m);
                gcMembCode = batoneview.Rows[i]["ccustcode"].ToString();
                Thread.Sleep(500);
                postAccounts(acct, ncontAmt, globalvar.gcIntSuspense);
                updateMpayroll(gcMembCode,acct);
                Application.DoEvents();
            }

            if (nSaveCont_Ind>0.00m)
            {
                postAccounts(globalvar.gcSaveCtrl_Ind, nSaveCont_Ind, gcContra);   //Savings individual
            }

            if (nSaveCont_Cor > 0.00m)
            {
                postAccounts(globalvar.gcSaveCtrl_Cor, nSaveCont_Cor, gcContra);   //Savings corporate
            }

            if (nSaveCont_Grp > 0.00m)
            {
                postAccounts(globalvar.gcSaveCtrl_Grp, nSaveCont_Grp, gcContra);   //Savings Group
            }

            if (nSaveCont_Stf > 0.00m)
            {
                postAccounts(globalvar.gcSaveCtrl_Stf, nSaveCont_Stf, gcContra);   //Savings staff
            }

            if (nShareCont_Ind > 0.00m)
            {
                postAccounts(globalvar.gcShareCtrl_Ind, nShareCont_Ind, gcContra);  //Shares individual
            }

            if (nShareCont_Cor > 0.00m)
            {
                postAccounts(globalvar.gcShareCtrl_Cor, nShareCont_Cor, gcContra);  //Shares Corporate
            }

            if (nShareCont_Grp > 0.00m)
            {
                postAccounts(globalvar.gcShareCtrl_Grp, nShareCont_Grp, gcContra);  //Shares Group
            }

            if (nShareCont_Stf > 0.00m)
            {
                postAccounts(globalvar.gcShareCtrl_Stf, nShareCont_Stf, gcContra);  //Shares Staff
            }

            initvariables();
            MessageBox.Show("Batch Process Completed");
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void updateMpayroll(string custcode,string actnumb)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "update mpayroll set lbatprocessed=1 where ccustcode=@lcustcode and acctnumb=@lacctnumb";
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nConnHandle2);
                updCommand.UpdateCommand.Parameters.Add("@lcustcode", SqlDbType.VarChar).Value = custcode;
                updCommand.UpdateCommand.Parameters.Add("@lacctnumb", SqlDbType.VarChar).Value = actnumb;
                nConnHandle2.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }

        }
        private void postAccounts(string tcAcctNumb, decimal lnTranAmt, string ccontra)
        {
            string tcContra = ccontra;
            DateTime dTrandate = bRunDate.Value;
            string tcUserid = globalvar.gcUserid;
            int tncompid = globalvar.gnCompid;
            string tcCustcode = gcMembCode;
            string tcDesc = "BatchProcessing "+tcCustcode;
            decimal tnTranAmt = Math.Abs(lnTranAmt);// tcAcctNumb.Substring(0,3)=="250" || tcAcctNumb.Substring(0, 3) == "270" ? Math.Abs(lnTranAmt) : -Math.Abs(lnTranAmt);
            decimal tnContAmt = -Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = "000001" ;
            int npaytype = Convert.ToInt32(comboBox1.SelectedValue)>0 ?2 :1;   //npaytype=1 cash; npaytype=2 cheque    npaytype=3 mobile which is not yet implemented
            decimal lnWaiveAmt = 0.00m;
            string tcTranCode = "06";
            int lnServID = 1;// gnLoanProduct; we will revisit this. Product could be saved in mpayroll when member payroll is being set up  
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
            dbal.updDayBal(cs, bRunDate.Value, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode,bRunDate.Value,bRunDate.Value);                          //update tranhist posting account


            gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
            dbal.updDayBal(cs, bRunDate.Value, tcContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
            decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
            tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode,bRunDate.Value,bRunDate.Value);                        //update tranhist account 396 1756
            updateClient_Code updcl = new updateClient_Code();
            updcl.updClient(cs, "nvoucherno");
        }

        private void initvariables()
        {
            textBox3.Text = "";
            textBox1.Text = "";
            textBox5.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            batview.Clear();
            clientview.Clear();
            batdetview.Clear();
            bnkview.Clear();
            batoneview.Clear();
            textBox12.Text = "";
            progressBar1.Value = 0;
            comboBox2.Enabled = false;
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                comboBox2.Enabled = false;
//                exec Tsp_BatchMembers4Processing_all 30
 
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label31.Text = "Select Batch";
            getBatchList();
            comboBox2.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label31.Text = "Select Product";
            comboBox2.Enabled = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                textBox3.Text = batview.Rows[0]["bat_cont"].ToString();
                textBox1.Text = batview.Rows[0]["cont_name"].ToString();
                textBox2.Text = batview.Rows[0]["bat_total"].ToString();
                textBox5.Text = batview.Rows[0]["bat_total"].ToString();
                getdetails(Convert.ToInt16(comboBox2.SelectedValue));
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                textBox3.Text = comboBox1.SelectedValue.ToString();
                textBox1.Text = comboBox1.Text.ToString();
//                textBox5.Text = Convert.ToDecimal(bnkview.Rows[comboBox1.SelectedIndex]["nbookbal"]).ToString("N2"); 
            }
        }
    }
}
