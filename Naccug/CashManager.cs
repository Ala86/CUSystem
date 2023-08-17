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
using System.Transactions;

namespace WinTcare
{
    public partial class CashManager : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string clocation = globalvar.cLocalCaption;
        updateJournal upj = new updateJournal();
        updateClient_Code updcl = new updateClient_Code();

        DataTable cashview = new DataTable();
        DataTable manview = new DataTable();
        DataTable brchView = new DataTable();

        DataView cashDataView = new DataView();
        DateTime gdToday = globalvar.gdSysDate;

        string duser = globalvar.gcUserid;
        string gcPostAcct = string.Empty;
        string gcContAcct = string.Empty;
        decimal gnTranAmt = 0.00m;
        decimal gnCurrBal = 0.00m;
        bool glTillBal = false;
        bool glBalOk = false;
        public EventHandler RefreshNeeded;
        int djv0 = 0;
        string djv = string.Empty;
        

        public CashManager()
        {
            InitializeComponent();
        }


        private void CashManager_Load(object sender, EventArgs e)
        {
            this.Text = clocation + "<< Member Deposit >>";
            getBranch();
            getMainCash(globalvar.gnBranchid);
            getCashiers(globalvar.gnBranchid,"99999999999");
            CheckAccountVerification();
            cashGrid.Columns["tillamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            cashGrid.Columns["tillamt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            cashGrid.Columns["endbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            cashGrid.Columns["endbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            textBox4.Text = gnTranAmt.ToString("N2");
            textBox5.Text = gnTranAmt.ToString("N2");

            djv0 = GetClient_Code.clientCode_int(cs, "nvoucherno");//  ucc. updateClient_Code

            if (djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "nvoucherno");
                djv0 = 1;
            }
            djv = djv0.ToString().Trim().PadLeft(4, '0');
            textBox1.Text = gdToday.Year.ToString().Trim().Substring(0, 2) + gdToday.Month.ToString().Trim() + gdToday.Day.ToString().Trim() + "-" + djv.Trim();
        }

        private void getBranch()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                brchView.Clear();
                ndConnHandle1.Open();
                string dsql1r0 = " exec tsp_GetBranch " + ncompid;
                SqlDataAdapter da1r0 = new SqlDataAdapter(dsql1r0, ndConnHandle1);
                da1r0.Fill(brchView);
                if (brchView != null && brchView.Rows.Count > 0)
                {
                    comboBox1.DataSource = brchView.DefaultView;
                    comboBox1.DisplayMember = "br_name";
                    comboBox1.ValueMember = "branchid";
                    comboBox1.SelectedValue = globalvar.gnBranchid;
                    ndConnHandle1.Close();
                }
            }

        }

        private void CheckAccountVerification()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                string dsqlver = " exec tsp_CheckVerification " + ncompid;
                SqlDataAdapter daver = new SqlDataAdapter(dsqlver, ndConnHandle1);
                DataTable verview = new DataTable();
                daver.Fill(verview);
                if (verview.Rows.Count > 0)
                {
                    int lnOutStand = Convert.ToInt32(verview.Rows[0]["acctCount"]);
                    if(lnOutStand > 0)
                    {
                        MessageBox.Show(lnOutStand + " Outstanding Items require verification, Pls verify and try again");
                        cashGrid.ReadOnly = true;
                    }
                }
                else
                {
                    cashGrid.ReadOnly = false;
                }
            }

        }


        private void getMainCash(int tnBranch)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                manview.Clear();
                ndConnHandle1.Open();
                string dsql1r1 = " exec tsp_getVaultAccounts " + ncompid+","+tnBranch ;
                SqlDataAdapter da1r1 = new SqlDataAdapter(dsql1r1, ndConnHandle1);
                da1r1.Fill(manview);
                if (manview != null && manview.Rows.Count>0)
                {
                    comboBox2.DataSource = manview.DefaultView;
                    comboBox2.DisplayMember = "cacctname";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                    ndConnHandle1.Close();
                }
            }

        }



        private void getCashiers(int tnBranchid,string tcExAcct)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                cashview.Clear();
                ndConnHandle1.Open();
                //************Getting accounts  exec tsp_getBranchCashiers 30,16              
                decimal curbal = 0.00m;
                string casSql = " exec tsp_getBranchCashiers " + ncompid + "," + tnBranchid + ",'" + tcExAcct + "'";
                SqlDataAdapter dalCas = new SqlDataAdapter(casSql,ndConnHandle1);
                //                string casSql = " exec tsp_getBranchCashiers @tcompid,@tbranchid, @tact ";
                //                dalCas.SelectCommand = new SqlCommand(casSql, ndConnHandle1);
                //                dalCas.SelectCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;
                //                dalCas.SelectCommand.Parameters.Add("@tbranchid", SqlDbType.Int).Value = tnBranchid;
                //                dalCas.SelectCommand.Parameters.Add("@tact", SqlDbType.Int).Value = tcExAcct;
                ////                dalCas.SelectCommand.Parameters.Add("@tuser", SqlDbType.Int).Value = globalvar.gcUserid;
                //                dalCas.SelectCommand.ExecuteNonQuery();
                dalCas.Fill(cashview);
                if (cashview != null && cashview.Rows.Count>0)
                {
                    cashGrid.AutoGenerateColumns = false;
                    cashGrid.DataSource = cashview.DefaultView;

                    cashGrid.Columns[0].DataPropertyName = "username";
                    cashGrid.Columns[1].DataPropertyName = "cacctnumb";
                    cashGrid.Columns[2].DataPropertyName = "cacctname";
                    cashGrid.Columns[3].DataPropertyName = "nbookbal";
                    for(int j=0;j<cashview.Rows.Count;j++)
                    {
                       decimal bkbal = Convert.ToDecimal(cashview.Rows[j]["nbookbal"]);
                       curbal = curbal + bkbal; 
                    }
                    textBox6.Text = curbal.ToString("N2");
                }
                ndConnHandle1.Close();
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
            if (textBox2.Text.Trim() != "" && glTillBal && glBalOk) 
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            acctEnquiry ac = new acctEnquiry(cs,ncompid,clocation);
            ac.ShowDialog();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
           // using (TransactionScope scope = new TransactionScope())
          //  {
                gcContAcct = textBox2.Text;
                for (int i = 0; i < cashview.Rows.Count; i++)
                {
                    gcPostAcct = cashview.Rows[i]["cacctnumb"].ToString();
                    gnTranAmt = Convert.ToDecimal(cashGrid.Rows[i].Cells["tillamt"].Value);
                    gnCurrBal = Convert.ToDecimal(cashGrid.Rows[i].Cells["curbal"].Value);
                    string trNarr = "Till Transfer for " + gcPostAcct;
                //if(gnTranAmt>0.00m)
                //{
           //     MessageBox.Show("This is the amount " + gnTranAmt);
                if (gnTranAmt != 0.00m)
                {
                //    MessageBox.Show("This is the amount " + gnTranAmt);
                    postAccounts(gcPostAcct, gnTranAmt, gcContAcct, trNarr);
                }
                    //                }
                    //                else
                    //                {
                    ////                    MessageBox.Show("Nothing to do for account number " + gcPostAcct);
                    //                }

                }
               // scope.Complete();
        //    }
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
        private void postAccounts(string tcAcctNumb, decimal lnTranAmt, string ccontra,string tctrnarr)
        {
            string tcContra = ccontra;
            string tcUserid = globalvar.gcUserid;
            string tcDesc = tctrnarr;
            int tncompid = globalvar.gnCompid;
            string tcPostCustcode = tcAcctNumb.Substring(3, 6);
            string tcContCustcode = ccontra.Substring(3, 6);

            decimal tnTranAmt = lnTranAmt;// gnCurrBal >= 0.00m ? -Math.Abs(lnTranAmt) : Math.Abs(lnTranAmt);
            decimal tnContAmt = -tnTranAmt;// Math.Abs(lnTranAmt);
            string tcVoucher = textBox1.Text.Replace("-", "");

            decimal unitprice = Math.Abs(lnTranAmt);
            string tcChqno = "000001";
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
           // updateGlmast gls = new updateGlmast();
           // updateDailyBalance dbal = new updateDailyBalance();
           // updateCuTranhist tls1 = new updateCuTranhist();

            //gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
            //dbal.updDayBal(cs,globalvar.gdSysDate, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
            //decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
            //tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            //srvid, "", "", lFreeBee, tcPostCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode,globalvar.gdSysDate,globalvar.gdSysDate);                          //update tranhist posting account


            //gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
            //dbal.updDayBal(cs, globalvar.gdSysDate, tcContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
            //decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
            //tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            //srvid, "", "", lFreeBee, tcContCustcode, tncompid, globalvar.gnBranchid, globalvar.gnCurrCode,globalvar.gdSysDate,globalvar.gdSysDate);                        //update tranhist account 396 1756

            upj.updJournal(cs, tcAcctNumb, tcDesc, tnTranAmt, tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
            upj.updJournal(cs, tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
            updcl.updClient(cs, "nvoucherno");
            updcl.updClient(cs, "stackcount");                                                                                                                                                            //updateClient_Code updcl = new updateClient_Code();                                                                                                                                                                        //updcl.updClient(cs, "nvoucherno");

            if (globalvar.gcCashAccont != null && globalvar.gcCashAccont != string.Empty)
            {
                updateTemporaryCashBalances(globalvar.gcCashAccont, -Math.Abs(tnContAmt));
                cashAccount(globalvar.gcCashAccont);
            }
        }


        private void initvariables()
        {
            cashview.Clear();
            glTillBal = glBalOk =  false;

            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            getCashiers(globalvar.gnBranchid,"99999999999");
            comboBox2.SelectedIndex = -1;
            textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text =  "";
            djv0 = GetClient_Code.clientCode_int(cs, "nvoucherno");
            if (djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "nvoucherno");
                djv0 = 1;
            }
            djv = djv0.ToString().Trim().PadLeft(4, '0');
            textBox1.Text = gdToday.Year.ToString().Trim().Substring(0, 2) + gdToday.Month.ToString().Trim() + gdToday.Day.ToString().Trim() + "-" + djv.Trim();
        }

        private void cashGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            int lnOption = radioButton1.Checked ? 1 :  2;
            if (cashGrid.Columns[e.ColumnIndex].Name == "tillamt" && cashGrid.CurrentRow.Cells["cname"].Value.ToString() != "")
            {
                decimal endBal = 0.00m;
                decimal tillAmount = 0.00m;
                decimal currAmount = -Math.Abs(Convert.ToDecimal(cashGrid.CurrentRow.Cells["curbal"].Value));
                if (currAmount <= 0.00m)
                {
                    switch (lnOption)
                    {
                        case 1: //allocation
                            tillAmount = -Math.Abs(Convert.ToDecimal(cashGrid.CurrentRow.Cells["tillamt"].Value));
                            cashGrid.CurrentRow.Cells["tillamt"].Value = tillAmount.ToString("N2");
                                endBal = currAmount -Math.Abs(tillAmount);
                                cashGrid.CurrentRow.Cells["endbal"].Value = endBal.ToString("N2");
                            doTotals();
                            glTillBal = true;
                            break;
                        case 2: //retirement 
                         //   MessageBox.Show("Cannot retire more than Till Balance 1");
                            tillAmount = Math.Abs(Convert.ToDecimal(cashGrid.CurrentRow.Cells["tillamt"].Value));
                            if (tillAmount > Math.Abs(currAmount))
                            {
                                MessageBox.Show("Cannot retire more than Till Balance");
                                tillAmount = 0.00m;
                                cashGrid.CurrentRow.Cells["tillamt"].Value = tillAmount.ToString("N2");
                                endBal = currAmount;
                                cashGrid.CurrentRow.Cells["endbal"].Value = endBal.ToString("N2");
                                //                                tillAmount = 0.00m;
                                doTotals();
                                glTillBal = false;
                                break;
                            }
                            else
                            {
                                cashGrid.CurrentRow.Cells["tillamt"].Value = tillAmount.ToString("N2");
                                endBal = currAmount + tillAmount;
                                cashGrid.CurrentRow.Cells["endbal"].Value = endBal.ToString("N2");
                                doTotals();
                                glTillBal = true;
                                break;
                            }
                    }
                }
                else { MessageBox.Show("Cash account cannot be in credit"); }
            }
            else
            {
                cashGrid.CurrentRow.Cells["tillamt"].Value = 0.00m.ToString("N2");
            }
            AllClear2Go();
        }

        private void doTotals()
        {
            decimal lnTotalCurBal = 0.00m;
            decimal lnTotalTilBal = 0.00m;
            decimal lnTotalEndBal = 0.00m;
            for(int j=0;j<cashGrid.Rows.Count;j++)
            {
                lnTotalCurBal = lnTotalCurBal + Convert.ToDecimal(cashGrid.Rows[j].Cells["curbal"].Value);
                lnTotalTilBal = lnTotalTilBal + Convert.ToDecimal(cashGrid.Rows[j].Cells["tillamt"].Value);
            }
            lnTotalEndBal = lnTotalCurBal + lnTotalTilBal;
            textBox4.Text = lnTotalTilBal.ToString("N2");
            textBox5.Text = lnTotalEndBal.ToString("N2");
            glBalOk = Math.Abs(lnTotalTilBal) > 0.00m ? true : false;

            //glTillBal = (endBal == currAmount + tillAmount) ? true : false;
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                getMainCash(Convert.ToInt16(comboBox1.SelectedValue));
                //string tcAcct = manview.Rows.Count > 0 ? manview.Rows[0]["cacctnumb"].ToString() : "";
                //textBox2.Text = tcAcct;
                //textBox3.Text = manview.Rows.Count>0 ? Convert.ToDecimal(manview.Rows[0]["nbookbal"]).ToString("N2") : 0.00m.ToString();
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox2.Focused )
            {
                string tcAcct = comboBox2.SelectedValue.ToString().Trim();
                textBox2.Text = tcAcct;
                getCashiers(Convert.ToInt16(comboBox1.SelectedValue), tcAcct);
                textBox2.Text = manview.Rows[comboBox2.SelectedIndex]["cacctnumb"].ToString();
                textBox3.Text = Convert.ToDecimal(manview.Rows[comboBox2.SelectedIndex]["nbookbal"]).ToString("N2");

            }
            /*
            string outputInfo = "";
            string outputInfo = "";
            string[] keyWords = textBox9.Text.Split(' ');

            foreach (string word in keyWords)
            {
                if (outputInfo.Length == 0)
                {
                    glLabSearch = false;
                    outputInfo = "(servce_name LIKE '%" + word + "%')";
                }
                else
                {
                    glLabSearch = true;
                    outputInfo += " AND (servce_name LIKE '%" + word + "%')";
                }
            }
            labDataView = labview.DefaultView;
            labDataView.RowFilter = outputInfo;
            labGrid.DataSource = labDataView.ToTable();
             */
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;

            for(int i=0; i<cashGrid.Rows.Count;i++)
            {
                cashGrid.Rows[i].Cells["tillamt"].Value = "";
                cashGrid.Rows[i].Cells["endbal"].Value = "";
            }

            textBox4.Text = textBox5.Text = "";
            glTillBal = false;
            AllClear2Go();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = true;

            for (int i = 0; i < cashGrid.Rows.Count; i++)
            {
             //   MessageBox.Show("This is the Input 1");
                cashGrid.Rows[i].Cells["tillamt"].Value = "";
                cashGrid.Rows[i].Cells["endbal"].Value = "";
              //  MessageBox.Show("This is the Input 2");
            }
            textBox4.Text = textBox5.Text = "";
            glTillBal = false;
            AllClear2Go();
        }
    }
}
