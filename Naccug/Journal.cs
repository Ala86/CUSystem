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
    public partial class Journal : Form
    {
        string cs = globalvar.cos;
        DataTable acctview = new DataTable();
        DataTable jourview = new DataTable();
        DataTable bnkview = new DataTable();
        int ncompid = globalvar.gnCompid;
        int nrow;
        //    string gcDrCr = "";
        bool glBankAcct = false;
        bool glAcctSelected = false;
        decimal gnTotaldebit = 0.00m;
        decimal gnTotalCredit = 0.00m;
        decimal gnBookBal = 0.00m;
        decimal gnNewBal = 0.00m;
        updateClient_Code ucc = new updateClient_Code();
        DateTime gdToday = globalvar.gdSysDate;
        public EventHandler RefreshNeeded;

        public Journal()
        {
            InitializeComponent();
        }

        private void Journal_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Journal Entries >>";

            int djv0 = 0;
            string djv = string.Empty;
            djv0 = GetClient_Code.clientCode_int(cs, "jv_no");//  ucc. updateClient_Code

            if(djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "jv_no");
                djv0 = 1;
            }
           djv = djv0.ToString().Trim().PadLeft(4, '0');
            textBox2.Text = gdToday.Year.ToString().Trim().Substring(0,2) + gdToday.Month.ToString().Trim() +gdToday.Day.ToString().Trim()+ "-" + djv.Trim();

            textBox3.Text = globalvar.gdSysDate.ToLongDateString();

            getInternalAccounts();
            jourgrid.Columns["ddebit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            jourgrid.Columns["ddebit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            jourgrid.Columns["dcredit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            jourgrid.Columns["dcredit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            bnkGrid.Columns["bnkAmt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            bnkGrid.Columns["bnkAmt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            nrow = Convert.ToInt16(jourgrid.CurrentRow);// .CurrentRow();
            textBox1.Text = globalvar.gcUserName;
        }


        private void getInternalAccounts()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                acctview.Clear();
                ndConnHandle1.Open();
                string dsql1 = "exec tsp_InternalAccounts  " + ncompid;
                SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle1);
                da1.Fill(acctview);
                if (acctview != null && acctview.Rows.Count > 0)
                {
                    var bcol = new DataGridViewComboBoxColumn();
                    bcol.Name = "Account";
                    bcol.DataPropertyName = "cacctname";
                    bcol.HeaderText = "Account";
                    bcol.Name = "actSelect";
                    bcol.DataSource = acctview;
                    bcol.DisplayMember = "displayName";// "Account"+" "+"cacctname";
                    bcol.ValueMember = "cacctnumb";
                    bcol.Width = 200;
                    bcol.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
                    jourview.Rows.Add();
                    this.jourgrid.Columns.Insert(2, bcol);
                    jourgrid.Rows[0].Cells["ddebit"].Value = 0.00m;
                    jourgrid.Rows[0].Cells["dcredit"].Value = 0.00m;
                    //       MessageBox.Show("internal accounts sorted");
                }
                else { MessageBox.Show("internal accounts not found"); }
            }
        }


        /*       protected override void OnKeyDown(KeyEventArgs e)
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
               }*/

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (gnTotalCredit == gnTotaldebit && gnTotaldebit > 0.00m)

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


        private void getBnkStatus(string actnumb)
        {
            bnkview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                string dsql11 = "exec tsp_bnk_accounts_one " + ncompid + ",'" + actnumb + "'";
                SqlDataAdapter da11 = new SqlDataAdapter(dsql11, ndConnHandle1);
                DataTable oneacc = new DataTable();
                da11.Fill(oneacc);
                da11.Fill(bnkview);
                if (oneacc != null && oneacc.Rows.Count > 0)
                {
                    //                    MessageBox.Show("we are about to show the details");
                    textBox4.Text = oneacc.Rows[0]["cacctnumb"].ToString();
                    textBox7.Text = oneacc.Rows[0]["cacctname"].ToString();
                    bool llbank = Convert.ToBoolean(oneacc.Rows[0]["lbank"]);
                    glBankAcct = llbank;
                    if (llbank)
                    {
                        bnkGrid.Enabled = true;
                        bnkGrid.AutoGenerateColumns = false;
                        bnkGrid.DataSource = bnkview.DefaultView;
                        bnkGrid.Columns[0].DataPropertyName = "bnk_name";
                        bnkGrid.Columns[1].DataPropertyName = "bnkbr_name";
                        bnkGrid.Columns[2].DataPropertyName = "chqno";
                        bnkGrid.Columns[3].DataPropertyName = "chqdate";
                        bnkGrid.Columns[4].DataPropertyName = "nbookbal";
                        bnkview.Rows[0]["chqdate"] = DateTime.Now.ToShortDateString();
                    }
                    else { bnkGrid.Enabled = false; }
                } 
            }
        }

        private void getBookBalance(string actnumb)
        {
            bnkview.Clear();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                string dsql11 = "select nbookbal from glmast where cacctnumb = '" + actnumb + "'";
                SqlDataAdapter da11 = new SqlDataAdapter(dsql11, ndConnHandle1);
                DataTable oneacc = new DataTable();
                da11.Fill(oneacc);
                if (oneacc.Rows.Count > 0)
                {
//                    jourgrid.CurrentRow.Cells["dBookBal"].Value = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    jourgrid.CurrentRow.Cells["dBookBal"].Value = oneacc.Rows[0]["nbookbal"].ToString();
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void jourGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            jourgrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void jourGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (jourgrid.Columns[e.ColumnIndex].Name == "dtrans")
            {
                jourgrid.Rows[e.RowIndex].Cells["ddate"].Value = DateTime.Now.ToShortDateString();
                jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value = 0.00m;
                jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value = 0.00m;
            }

            //debit transactions
            if (jourgrid.Columns[e.ColumnIndex].Name == "ddebit" && jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value != null && Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value) > 0.00m)
            {
                if (jourgrid.Rows[e.RowIndex].Cells["actSelect"].Value != null && jourgrid.Rows[e.RowIndex].Cells["actSelect"].Value.ToString() != "")
                {
                    decimal debitAmt = Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value);
                    jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value = debitAmt.ToString("N2");
                    //string actnegbal = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    //MessageBox.Show("account balance for debits is " + actnegbal);
                    //jourgrid.Rows[e.RowIndex].Cells["dBookBal"].Value = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value = (Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value) > 0.00m ? 0.00m : Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value)).ToString("N3");
                    sumtotal();
                }
                else
                {
                    MessageBox.Show("Please select an account to continue");
                    jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value = 0.00m;
                }
            }

            //credit transactions
            if (jourgrid.Columns[e.ColumnIndex].Name == "dcredit" && jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value != null && Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value) > 0.00m)
            {
                if (jourgrid.Rows[e.RowIndex].Cells["actSelect"].Value != null && jourgrid.Rows[e.RowIndex].Cells["actSelect"].Value.ToString() != "")
                {
                    decimal creditAmt = Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value);
                    jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value = creditAmt.ToString("N2");
                    //string actcrebal = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    //MessageBox.Show("account balance for credits is " + actcrebal);
                    //jourgrid.Rows[e.RowIndex].Cells["dBookBal"].Value = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value = (Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value) > 0.00m ? 0.00m : Convert.ToDecimal(jourgrid.Rows[e.RowIndex].Cells["ddebit"].Value)).ToString("N2");
                    sumtotal();
                    if (glBankAcct)
                    {
                        bnkview.Rows[0]["nbookbal"] = creditAmt.ToString("N2");
                    }
                }
                else
                {
                    MessageBox.Show("Please select an account to continue");
                    jourgrid.Rows[e.RowIndex].Cells["dcredit"].Value = 0.00m;
                }
            }
        }


        private void sumtotal()
        {
            gnTotaldebit = 0.00m;
            gnTotalCredit = 0.00m;
            for (int i = 0; i < jourgrid.Rows.Count; i++)
            {
                gnTotaldebit = gnTotaldebit + Convert.ToDecimal(jourgrid.Rows[i].Cells["ddebit"].Value);
                gnTotalCredit = gnTotalCredit + Convert.ToDecimal(jourgrid.Rows[i].Cells["dcredit"].Value);
            }
            textBox10.Text = gnTotaldebit.ToString("N2");
            textBox11.Text = gnTotalCredit.ToString("N2");
            AllClear2Go();
        }

        private void jourGrid_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            if (jourgrid.CurrentCell is DataGridViewComboBoxCell)
            {
                jourgrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            else { return; }
        }

        private void jourgrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (jourgrid.CurrentCell is DataGridViewComboBoxCell)
            {
                if (jourgrid.CurrentCell.Value != null)
                {
                    string actsel = jourgrid.CurrentRow.Cells["actSelect"].Value.ToString();
                    textBox4.Text = actsel;
                    textBox7.Text = Convert.ToString(jourgrid.CurrentRow.Cells[2].FormattedValue);
                    getBookBalance(actsel);
                    //string actbal = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    //MessageBox.Show("account balance is " + actbal);
                    //jourgrid.CurrentRow.Cells["dBookBal"].Value = acctview.Rows[e.RowIndex]["nbookbal"].ToString();
                    glAcctSelected = true;
                    getBnkStatus(actsel);
                }
            }
            else { return; }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string lcJVno = textBox2.Text.Replace("-","");
                string lTrancode = "08";
                string cglquery = "Insert Into journal (nbookbal,nnewbal,cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,ccurrcode,branchid)";
                cglquery += " VALUES  (@tnbookbal,@tnnewbal,@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@lnCurrCode,@lnBranchid)";
                nConnHandle2.Open();
                foreach (DataGridViewRow dr in jourgrid.Rows)
                {
                    if (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m || Convert.ToDecimal(dr.Cells["dcredit"].Value) > 0.00m)
                    {
                        decimal jouramt = (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                        decimal lnBookBal = Convert.ToDecimal(dr.Cells["dBookBal"].Value);
                        decimal lnNewBal = lnBookBal + jouramt;
                        SqlDataAdapter insCommand = new SqlDataAdapter();
                        insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                        insCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnBookBal;
                        insCommand.InsertCommand.Parameters.Add("@tnnewbal", SqlDbType.Decimal).Value = lnNewBal;
                        insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
                        insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
                        insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dr.Cells["ddate"].Value;
                        insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = dr.Cells["dtrans"].Value.ToString();
                        insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = dr.Cells["actSelect"].Value.ToString();
                        insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                        insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = jouramt;
                        insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = lcJVno;// textBox2.Text;
                        insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                        insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                        insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
                        insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = lTrancode;
                        insCommand.InsertCommand.Parameters.Add("@lnCurrCode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                        insCommand.InsertCommand.Parameters.Add("@lnBranchid", SqlDbType.Int).Value = globalvar.gnBranchid;

                        insCommand.InsertCommand.ExecuteNonQuery();
                        insCommand.InsertCommand.Parameters.Clear();
                        string auditDesc = "Jouranal Entry " + dr.Cells["actSelect"].Value.ToString();
                        AuditTrail au = new AuditTrail();
                        au.upd_audit_trail(cs, auditDesc, 0.00m, jouramt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                      //  if (globalvar.gcCashAccont != null && globalvar.gcCashAccont != string.Empty && lcJVno != string.Empty && lcJVno != null)
                      //  {
                      ////      decimal jouramt = (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                      //      MessageBox.Show("This is the posting" + globalvar.gcCashAccont);
                      //      MessageBox.Show("This is the posting" + jouramt);
                      //      updateTemporaryCashBalances(globalvar.gcCashAccont, jouramt);
                      //      cashAccount(globalvar.gcCashAccont);
                      //  }

                    }
                  
                }
           
                ucc.updClient(cs, "jv_no");
                nConnHandle2.Close();

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

                string cashap = "update Daytrans set nbookbal = nbookbal+ @nAmount where cacctnumb = @cAcct";
                SqlDataAdapter cashUpd = new SqlDataAdapter();
                cashUpd.UpdateCommand = new SqlCommand(cashap, ncon);
                cashUpd.UpdateCommand.Parameters.Add("@nAmount", SqlDbType.Decimal).Value = tnTransAmount;
                cashUpd.UpdateCommand.Parameters.Add("@cAcct", SqlDbType.VarChar).Value = actnumb;
                ncon.Open();
                cashUpd.UpdateCommand.ExecuteNonQuery();
            }
        }
        private void initvariables()
        {
            gnTotalCredit = 0.00m;
            gnTotaldebit = 0.00m;
            textBox10.Text = gnTotaldebit .ToString();
            textBox11.Text = gnTotalCredit.ToString();
            int djv0 = 0;
            string djv = string.Empty;

            djv0 = GetClient_Code.clientCode_int(cs, "jv_no");
            if (djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "jv_no");
                djv0 = 1;
            }
            djv = djv0.ToString().Trim().PadLeft(4, '0');
            textBox2.Text = gdToday.Year.ToString().Trim().Substring(0, 2) + gdToday.Month.ToString().Trim() + gdToday.Day.ToString().Trim() + "-" + djv.Trim();

            jourview.Clear();
            bnkview.Clear();
//            getInternalAccounts();
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            foreach(DataGridViewRow drv in jourgrid.Rows )
            {
                drv.Cells["dtrans"].Value = "";
                drv.Cells["ddebit"].Value = 0.00m;
                drv.Cells["dcredit"].Value = 0.00m;
                drv.Cells["actSelect"].Value = "";
            }
            for(int j=1;j<jourgrid.Rows.Count;j++)
            {
          //      jourgrid.Rows.RemoveAt(j);
            }
        //    MessageBox.Show("done in initvariables");
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
