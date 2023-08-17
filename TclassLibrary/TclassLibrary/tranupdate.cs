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
using System.Transactions;


namespace TclassLibrary
{
    public partial class tranupdate : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        int gnBranchid = 0;
        int gnCurrCode = 0;
        int gnAccountsok = 0;
        int gnTempRowCount = 0;
        int gnrecCount = 0;
        string clocation = string.Empty;
        string gcWkStation = string.Empty;
        string gcWinUser = string.Empty;
        bool glAllAccountsOk = false;
        bool glDebitCreditOk = false;

        DataTable transview = new DataTable();
        DataTable updateView = new DataTable();
        DataTable tempVoucher = new DataTable();

        int gnSelectedCount = 0;
        string gcUser = string.Empty;
        string gcJournalVoucher = string.Empty;

        public tranupdate(string tcCos, int tncompid, string tcLoca,string duserid,int tnbra,int tnCurrCode,string tcWkStation, string tcWinuser)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tncompid;
            clocation = tcLoca;
            gcUser = duserid  ;
            gnBranchid = tnbra;
            gnCurrCode = tnCurrCode;
            gcWkStation = tcWkStation;
            gcWinUser = "Ala";

            //cs = tcCos;
            //ncompid = tncompid;
            //clocation = tcloca;
        }

        public tranupdate()
        {
        }

        private void tranupdate_Load(object sender, EventArgs e)
        {
            this.Text = clocation + "<< Transaction Update >>";
            transGrid.Columns["ddebit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            transGrid.Columns["ddebit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            transGrid.Columns["dcredit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            transGrid.Columns["dcredit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            transGrid.Columns["postdate"].DefaultCellStyle.Format = "dd/MMM/yyyy";
            transGrid.Columns["trandate"].DefaultCellStyle.Format = "dd/MMM/yyyy";
            tempVoucher.Columns.Add("jvno", typeof(String));
            getrdata(0);
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (gnSelectedCount>0 && glAllAccountsOk && glDebitCreditOk)
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
            if (button3.Focused)
            {
                if (button3.Text == "Select All")
                {
                    CheckAccountsAll();
                    CheckBalancesAll();
                    for(int k=0;k<transGrid.Rows.Count;k++)
                    {
                        if(Convert.ToBoolean(transGrid.Rows[k].Cells["dupt"].Value))
                        {
                            gnSelectedCount++;
                        }
                    }
                    button3.Text = "deselect All";
                }
                else
                {
                    button3.Text = "Select All";
                    for (int k = 0; k < transview.Rows.Count; k++)
                    {
                        if (transGrid.Rows[k].Cells["actnumb"].Value.ToString() != "")
                        {
                            transGrid.Rows[k].Cells["dupt"].Value = false;
                            transGrid.Rows[k].DefaultCellStyle.BackColor = Color.White;
                            transGrid.Rows[k].DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                    glAllAccountsOk = false;
                }
            }
            glAllAccountsOk = glDebitCreditOk = gnSelectedCount > 0 ? true : false;

            AllClear2Go();
        }

        private void CheckAccountsAll()
        {
            int recNumb = transGrid.Rows.Count;
            for (int i = 0; i < recNumb; i++)
            {
                string djvnumb = transGrid.Rows[i].Cells["djvno"].Value.ToString().Trim();
                foreach (DataGridViewRow drow in transGrid.Rows)
                {
                    string lcAcctNumb = Convert.ToString(drow.Cells["actnumb"].Value);
                    if (drow.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                    {
                        glAllAccountsOk = CheckAccountExistence(cs, lcAcctNumb);
                        if (!glAllAccountsOk)
                        {
                            drow.Cells["dupt"].Value = false;
                            drow.DefaultCellStyle.BackColor = Color.Red;
                            drow.DefaultCellStyle.ForeColor = Color.Yellow;
                            temporaryFlagUpdate(djvnumb, false);
                        }
                        else
                        {
                            drow.Cells["dupt"].Value = true;
                        }
                    }
                }
            }
        }



        private void CheckBalancesAll()
        {
            int recNumb = transGrid.Rows.Count;
            decimal lnDebit = 0.00m;
            decimal lnCredit = 0.00m;
            for (int i = 0; i < recNumb; i++)
            {
                lnDebit = 0.00m;
                lnCredit = 0.00m;
                string djvnumb = transGrid.Rows[i].Cells["djvno"].Value.ToString().Trim();
                foreach (DataGridViewRow drow in transGrid.Rows)
                {
                    if (drow.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                    {
                        lnDebit = lnDebit + Convert.ToDecimal(drow.Cells["ddebit"].Value);
                        lnCredit = lnCredit + Convert.ToDecimal(drow.Cells["dcredit"].Value);
                    }
                }
                glDebitCreditOk = Math.Abs(lnCredit) == Math.Abs(lnDebit);
                if (!glDebitCreditOk)
                {
                    foreach (DataGridViewRow drow1 in transGrid.Rows)
                    {
                        if (drow1.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                        {
                            drow1.Cells["dupt"].Value = false;
                            drow1.DefaultCellStyle.BackColor = Color.Red;
                            drow1.DefaultCellStyle.ForeColor = Color.Yellow;
                            temporaryFlagUpdate(djvnumb, false);
                        }
                    }
                }
                else
                {
                    int lnActNotOk = 0;
                    foreach (DataGridViewRow drow1 in transGrid.Rows)
                    {
                        if (drow1.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                        {
                            string lcAcctNumb = Convert.ToString(drow1.Cells["actnumb"].Value);
                            glAllAccountsOk = CheckAccountExistence(cs, lcAcctNumb);
                            if (!glAllAccountsOk)
                            {
                                lnActNotOk++;
                                break;
                            }
                        }
                    }
                    if (lnActNotOk > 0)
                    {
                        foreach (DataGridViewRow drow2 in transGrid.Rows)
                        {
                            if (drow2.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                            {
                                drow2.Cells["dupt"].Value = false;
                                drow2.DefaultCellStyle.BackColor = Color.Red;
                                drow2.DefaultCellStyle.ForeColor = Color.Yellow;
                                temporaryFlagUpdate(djvnumb, false);
                            }
                        }
                    }
                }
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            if (MessageBox.Show("Are you sure you want to confirm transactions", "Transaction Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
           //   using (TransactionScope scope = new TransactionScope())
              //  {
                 //   updateGlmast gls = new updateGlmast();
                  //  updateDailyBalance dbal = new updateDailyBalance();

                  //  updateTranhist tls = new updateTranhist();
                  //  updateCuTranhist tls1 = new updateCuTranhist();

               //     AuditTrail au = new AuditTrail();

                    foreach (DataRow dr in tempVoucher.Rows)
                    {
                    //foreach (DataGridViewRow dr in transGrid.Rows)
                    //{
                    //    if (dr.Cells["actnumb"].Value != null && dr.Cells["actnumb"].Value.ToString() != "")
                    //    {
                    //        if (Convert.ToBoolean(dr.Cells["dupt"].Value) == true)
                    //        {
                               // {

                           string lcVoucherNo = Convert.ToString(dr["jvno"]);
                    //     string lcVoucherNo = dr.Cells["djvno"].Value.ToString();
                    //DateTime dTrandate = Convert.ToDateTime(dr.Cells["trandate"].Value);
                    //          string tcAcctNumb = dr.Cells["actnumb"].Value.ToString();
                    //string tcCustcode = tcAcctNumb.Substring(3, 6);
                    //   decimal gnPostAmt = (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                    //string tcVoucher = (dr.Cells["djvno"].Value.ToString() != null ? dr.Cells["djvno"].Value.ToString() : "");
                    //string tcChqno = dr.Cells["dchq"].Value.ToString();
                    //string tcUserid = dr.Cells["duser"].Value.ToString();
                    //string djstack = dr.Cells["jstack"].Value.ToString();

                    //gls.updGlmast(cs, tcAcctNumb, gnPostAmt);
                    //DateTime ldToday = DateTime.Today;
                    //MessageBox.Show("This is texting 1");
                        insertVerification(lcVoucherNo);

                                    //int lnDays = (ldToday - dTrandate).Days + 1;

                                    //if (lnDays > 0)
                                    //{
                                    //    dbal.updDayBal(cs, dTrandate, tcAcctNumb, gnPostAmt, gnBranchid, ncompid);
                                    //    updDayRunningBal(dTrandate, tcAcctNumb, gnPostAmt, lnDays);
                                    //}
                                    //else
                                    //{
                                    //    dbal.updDayBal(cs, dTrandate, tcAcctNumb, gnPostAmt, gnBranchid, ncompid);
                                    //}

                              //  }
                        //    }
                        //}
                        //else
                        //{
                        //    break;
                        //}
                  //  }


                }
                    initvariables();
                    getrdata(0);
                 //   scope.Complete();
              //  }
              }
         
            }
        

        private void insertVerification(string tcVoucher)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {

            //    SqlDataAdapter cuscommand = new SqlDataAdapter();
            //    cuscommand.InsertCommand = new SqlCommand(cquery1, ndConnHandle1);
                string cpatquery = "Exec Tsp_InsertVerification_new @compid,@tcVoucher,@tcUserID, @lbranchid,@gcWkStation,@gcWinUser";
                SqlDataAdapter payCommand = new SqlDataAdapter();
                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@Compid", SqlDbType.Int).Value = ncompid;
                payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = gcUser;
                payCommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = gnBranchid;
                payCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.VarChar).Value = gcWkStation;
                payCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.VarChar).Value = gcWinUser;

                dcon.Open();
                payCommand.InsertCommand.ExecuteNonQuery();  //Insert new record

                // Updateing and Inserting Daybal

                     updateGlmast gls = new updateGlmast();
                updateDailyBalance dbal = new updateDailyBalance();

                //   updateTranhist tls = new updateTranhist();
                //   updateCuTranhist tls1 = new updateCuTranhist();

                foreach (DataGridViewRow dr in transGrid.Rows)
                {
                    if (dr.Cells["actnumb"].Value != null && dr.Cells["actnumb"].Value.ToString() != "")
                    {
                        if (Convert.ToBoolean(dr.Cells["dupt"].Value) == true)
                        {
                            {
                                string lcServ_name = dr.Cells["dtrans"].Value.ToString();
                                DateTime dTrandate = Convert.ToDateTime(dr.Cells["trandate"].Value);
                                string tcAcctNumb = dr.Cells["actnumb"].Value.ToString();
                                string tcCustcode = tcAcctNumb.Substring(3, 6);
                                decimal gnPostAmt = (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                                string tcUserid = dr.Cells["duser"].Value.ToString();
                                string djstack = dr.Cells["jstack"].Value.ToString();
                                DateTime dTranDate = Convert.ToDateTime(dr.Cells["trandate"].Value);

                            //    gls.updGlmast(cs, tcAcctNumb, gnPostAmt);
                              //  MessageBox.Show("This is texting 2");
                                // DateTime ldToday = DateTime.Today;

                                //int lnDays = (ldToday - dTrandate).Days + 1;
                                //if (lnDays > 0)
                                //{
                                //    dbal.updDayBal(cs, dTrandate, tcAcctNumb, gnPostAmt, gnBranchid, ncompid);
                                //    updDayRunningBal(dTrandate, tcAcctNumb, gnPostAmt, lnDays);
                                //}
                                //else
                                //{
                                //    dbal.updDayBal(cs, dTrandate, tcAcctNumb, gnPostAmt, gnBranchid, ncompid);
                                //}

                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                dcon.Close();

            }
        }

        private void updateVerification()
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                SqlDataAdapter payCommand = new SqlDataAdapter();

                //               @tnCompid int, @tcAcctNumb char(11), @tcContra char(11), @tcControlAcct char(11), @tnTranAmt numeric(19, 2), @tcDesc varchar(200), @dtrandate datetime, @tcTranCode varchar(2), @dvaludate datetime,
                //@tcVoucher varchar(14), @tcReceipt varchar(14), @tcChqno varchar(20), @tcUserID varchar(20), @tnPNewBal numeric(19, 2), @tnCNewBal numeric(19, 2), @lcStack varchar(20), @lVerified bit, @lcCustcode char(6),
                //@lcContcode char(6), @lbranchid int, @lcurrcode int, @lnLoanProcess bit,@llcjvno varchar(20), @llcBank int, @jtype int, @gcWkStation varchar(50), @gcWinUser varchar(50), @serv_id numeric(19, 2)


                string cpatquery = "Exec Tsp_InsertVerification @tcVoucher,@tcUserID, @lbranchid,@gcWkStation,@gcWinUser ";

                payCommand.InsertCommand = new SqlCommand(cpatquery, dcon);
                payCommand.InsertCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                //payCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.Char).Value = tcAcctNumb;
                //payCommand.InsertCommand.Parameters.Add("@tcContra", SqlDbType.Char).Value = gcContraAcct;
                //payCommand.InsertCommand.Parameters.Add("@tcControlAcct", SqlDbType.Char).Value = gcControlAcct;
                //payCommand.InsertCommand.Parameters.Add("@tnTranAmt", SqlDbType.Decimal).Value = tnTranAmt;
                //payCommand.InsertCommand.Parameters.Add("@tcDesc", SqlDbType.VarChar).Value = tcDesc.Trim();
                //payCommand.InsertCommand.Parameters.Add("@dtrandate", SqlDbType.DateTime).Value = dTranDate.Value;
                //payCommand.InsertCommand.Parameters.Add("@tcTranCode", SqlDbType.Char).Value = tcTranCode;
                //payCommand.InsertCommand.Parameters.Add("@dvaludate", SqlDbType.DateTime).Value = dTranDate.Value;
                //payCommand.InsertCommand.Parameters.Add("@tcVoucher", SqlDbType.Char).Value = tcVoucher;
                //payCommand.InsertCommand.Parameters.Add("@tcReceipt", SqlDbType.Char).Value = gcReceiptNo;
                //payCommand.InsertCommand.Parameters.Add("@tcChqno", SqlDbType.Char).Value = tcChqno;
                //payCommand.InsertCommand.Parameters.Add("@tcUserid", SqlDbType.Char).Value = gcUserid;
                //payCommand.InsertCommand.Parameters.Add("@tnPNewBal", SqlDbType.Decimal).Value = tnPNewBal;
                //payCommand.InsertCommand.Parameters.Add("@tnCNewBal", SqlDbType.Decimal).Value = tnCNewBal;
                //payCommand.InsertCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = lcStack;
                //payCommand.InsertCommand.Parameters.Add("@lVerified", SqlDbType.Bit).Value = true;
                //payCommand.InsertCommand.Parameters.Add("@lcCustcode", SqlDbType.Char).Value = tcCustcode;
                //payCommand.InsertCommand.Parameters.Add("@lcContcode", SqlDbType.Char).Value = tcContcode;
                //payCommand.InsertCommand.Parameters.Add("@lbranchid", SqlDbType.Int).Value = globalvar.gnBranchid;
                //payCommand.InsertCommand.Parameters.Add("@lcurrcode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                //payCommand.InsertCommand.Parameters.Add("@lnLoanProcess", SqlDbType.Bit).Value = glLoanProcess;
                //payCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                //payCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                //payCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;
                //payCommand.InsertCommand.Parameters.Add("@gcWkStation", SqlDbType.VarChar).Value = globalvar.gcWorkStation.Trim();
                //payCommand.InsertCommand.Parameters.Add("@gcWinUser", SqlDbType.VarChar).Value = globalvar.gcWinUser.Trim();
                payCommand.InsertCommand.ExecuteNonQuery();
                dcon.Close();

            }
        }

        private static bool CheckAccountExistence(string cs, string tcAcct)
        {
            using (SqlConnection ndcon = new SqlConnection(cs))
            {
                ndcon.Open();
                string sqlqueryint = "select 1 from glmast where intcode=1 and cacctnumb = " + "'" + tcAcct + "'";
                SqlDataAdapter acctCheck = new SqlDataAdapter(sqlqueryint, ndcon);
                DataTable acctview = new DataTable();
                acctCheck.Fill(acctview);
                if (acctview.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void updDayRunningBal(DateTime ldTransDate, string dacct, decimal ntranAmt,int tnDays)
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                dconn.Open();

                decimal lnUpdAmtPos = (ntranAmt > 0.00m ? ntranAmt : 0.00m);
                decimal lnUpdAmtNeg = (ntranAmt < 0.00m ? ntranAmt : 0.00m);
                decimal lnUpdAmt = (lnUpdAmtPos > 0.00m ? lnUpdAmtPos : lnUpdAmtNeg);

                string sqlquerybal2 = "select baldate,nclosbal from daybal where cacctnumb = " + "'" + dacct + "' and convert(date,baldate)<='" + ldTransDate + "' order by baldate desc";  //checking to see if there are previouis records for client
                SqlDataAdapter dab2 = new SqlDataAdapter(sqlquerybal2, dconn);
                DataTable preview2 = new DataTable();
                dab2.Fill(preview2);
                if (preview2.Rows.Count > 0)
                {
                    decimal lnLastClosingBal = Convert.ToDecimal(preview2.Rows[0]["nclosbal"]);         //last closing balance
                    DateTime ldLastBalanceDate = Convert.ToDateTime(preview2.Rows[0]["baldate"]);       //last balance date
                    DateTime ldRunningDate = Convert.ToDateTime(ldLastBalanceDate.AddDays(1).ToShortDateString());
                    decimal lnOpeningBal = lnLastClosingBal;
                    decimal lnClosingBal = lnOpeningBal;// + ntranAmt;

                    updateView.Clear();
                    for (int i = 0; i < tnDays; i++)
                    {
                        string sqlquerybal1 = "select cacctnumb,ndebitsum,ncreditsum, nclosbal from daybal where cacctnumb = @cacctnumb and convert(date,baldate)= @ldBalDate0 order by dayid desc"; //check if there is a transaction for this date
                        SqlDataAdapter selUpd = new SqlDataAdapter();
                        selUpd.SelectCommand = new SqlCommand(sqlquerybal1, dconn);
                        selUpd.SelectCommand.Parameters.Add("@cacctnumb", SqlDbType.VarChar).Value = dacct;
                        selUpd.SelectCommand.Parameters.Add("@ldBalDate0", SqlDbType.DateTime).Value = ldRunningDate;
                        selUpd.SelectCommand.ExecuteNonQuery();
                        selUpd.Fill(updateView);
                        if (updateView.Rows.Count > 0)                                   //transaction exist for this day
                        {
                            decimal lnNClosBal = Convert.ToDecimal(updateView.Rows[0]["nclosbal"]);
                            decimal lnDebitSum = Convert.ToDecimal(updateView.Rows[0]["ndebitsum"]);
                            decimal lnCreditum = Convert.ToDecimal(updateView.Rows[0]["ncreditsum"]);
                            decimal lnOtherClosBal = lnNClosBal + ntranAmt; 

                            lnClosingBal = lnClosingBal + Math.Abs(lnCreditum) - Math.Abs(lnDebitSum);
                            string runquery = "Update daybal Set nopenbal=@tnOpenbal,nclosbal=@tnClosBal Where cacctnumb = @cacctnumb  and convert(DATE, baldate) = convert(date, @ldBalDate)";
                            SqlDataAdapter updapp = new SqlDataAdapter();
                            updapp.UpdateCommand = new SqlCommand(runquery, dconn);
                            updapp.UpdateCommand.Parameters.Add("@tnOpenbal", SqlDbType.Decimal).Value  = lnOpeningBal;
                            updapp.UpdateCommand.Parameters.Add("@tnClosBal", SqlDbType.Decimal).Value  = lnClosingBal;
                            updapp.UpdateCommand.Parameters.Add("@cacctnumb", SqlDbType.VarChar).Value  = dacct;
                            updapp.UpdateCommand.Parameters.Add("@ldBalDate", SqlDbType.DateTime).Value = ldRunningDate;
                            updapp.UpdateCommand.ExecuteNonQuery();
                            updapp.UpdateCommand.Parameters.Clear();
                            lnOpeningBal = lnClosingBal;
                        }
                        //else
                        //{
                        //    MessageBox.Show("No transactions for date = " + ldRunningDate);
                        //}
                        ldRunningDate = ldRunningDate.AddDays(1);
                        updateView.Clear();
                    }
                }
            }
        }


        //private static decimal getStartingBalance(string tcAcct)
        //{

        //    using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
        //    {
        //        string sqlrunbal = "select nnewbal from tranhist where cacctnumb = " + "'" + tcAcct + "'" + " order by itemid  "; //This process is for internal accounts only
        //        SqlDataAdapter runbal = new SqlDataAdapter(sqlrunbal, ndConnHandle3);
        //        DataTable runview = new DataTable();
        //        runbal.Fill(runview);
        //        if (runview.Rows.Count > 0)
        //        {
        //            decimal lnInitBal = Convert.ToDecimal(runview.Rows[0]["nnewbal"]);
        //            return lnInitBal;
        //        }
        //        else
        //        {
        //            return 0.00m;
        //        }
        //    }
        //}


        private void initvariables()
        {
            transview.Clear();
            tempVoucher.Clear();
            gnSelectedCount = 0;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
        }
        private void updateSubgrp()
        {
        }


        private void updateJournal(string dstack)
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                dconn.Open();
                string sqlquery = " update journal set sverified=1 where jcstack=@tcJstack";
                SqlDataAdapter dapp = new SqlDataAdapter();
                dapp.UpdateCommand = new SqlCommand(sqlquery, dconn);
                dapp.UpdateCommand.Parameters.Add("@tcJstack", SqlDbType.VarChar).Value = dstack;
                dapp.UpdateCommand.ExecuteNonQuery();
                dconn.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int dopt = radioButton11.Checked ? 0 : radioButton6.Checked ? 1 : radioButton7.Checked ? 2 : radioButton8.Checked ? 3 : radioButton9.Checked ? 4 : 5;
            getrdata(dopt);
        }

        private void getrdata(int opt)
        {
            string dsql = string.Empty;
            transview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                switch (opt)
                {
                    case 0:
                        ;
                        string dsql0 = "select sverified,tempupd,cuserid,cacctnumb,dpostdate,dtrandate,ndebit =case when ntranamnt< 0.00 then abs(ntranamnt) else 0.00 end,";
                        dsql0 += "ncredit =case when ntranamnt > 0.00 then abs(ntranamnt) else 0.00 end,ltrim(rtrim(ctrandesc)) as ctrandesc, jvno,cvoucherno, Cchqno,jcstack  ";
                        dsql0 += " from journal where convert(date,dtrandate)<>'' and ltrim(rtrim(jvno))<>'' and  sverified=0 order by jvno";
                        dsql = dsql0;
                        break;
                    case 1:
                        ;
                        string dsql1 = "select sverified,tempupd,cuserid,cacctnumb,dpostdate,dtrandate,ndebit =case when ntranamnt< 0.00 then abs(ntranamnt) else 0.00 end,";
                        dsql1 += "ncredit =case when ntranamnt > 0.00 then abs(ntranamnt) else 0.00 end,ltrim(rtrim(ctrandesc)) as ctrandesc, jvno,cvoucherno, Cchqno,jcstack  ";
                        dsql1 += " from journal where convert(date,dtrandate)<>'' and ltrim(rtrim(jvno))<>'' and  sverified=0 and jour_type=1 order by jvno";
                        dsql = dsql1; 
                        break;
                    case 2:
                        ;
                        string dsql2 = "select sverified,tempupd,cuserid,cacctnumb,dpostdate,dtrandate,ndebit =case when ntranamnt< 0.00 then abs(ntranamnt) else 0.00 end,";
                        dsql2 += "ncredit =case when ntranamnt > 0.00 then abs(ntranamnt) else 0.00 end,ltrim(rtrim(ctrandesc)) as ctrandesc, jvno,cvoucherno, Cchqno,jcstack  ";
                        dsql2 += " from journal where convert(date,dtrandate)<>'' and ltrim(rtrim(jvno))<>'' and  sverified=0 and jour_type=2 order by jnvo";
                        dsql = dsql2;
                        break;
                    case 3:
                        ;
                        string dsql3 = "select sverified,tempupd,cuserid,cacctnumb,dpostdate,dtrandate,ndebit =case when ntranamnt< 0.00 then abs(ntranamnt) else 0.00 end,";
                        dsql3 += "ncredit =case when ntranamnt > 0.00 then abs(ntranamnt) else 0.00 end,ltrim(rtrim(ctrandesc)) as ctrandesc, jvno,cvoucherno, Cchqno,jcstack  ";
                        dsql3 += " from journal where convert(date,dtrandate)<>'' and ltrim(rtrim(jvno))<>'' and  sverified=0  and jour_type=3  order by nvno";
                        dsql = dsql3;
                        break;
                    case 4:
                        ;
                        string dsql4 = "select sverified,tempupd,cuserid,cacctnumb,dpostdate,dtrandate,ndebit =case when ntranamnt< 0.00 then abs(ntranamnt) else 0.00 end,";
                        dsql4 += "ncredit =case when ntranamnt > 0.00 then abs(ntranamnt) else 0.00 end,ltrim(rtrim(ctrandesc)) as ctrandesc, jvno,cvoucherno, Cchqno,jcstack  ";
                        dsql4 += " from journal where convert(date,dtrandate)<>'' and ltrim(rtrim(jvno))<>'' and  sverified=0  and jour_type=4  order by jvno";
                        dsql = dsql4;
                        break;
                }
                decimal totalDebit = 0.00m;
                decimal totalCredit = 0.00m;
                int lnAccountCounter = 0;
                string lcJournalVoucher = string.Empty;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(transview);
                if (transview.Rows.Count > 0)
                {
                    transGrid.AutoGenerateColumns = false;
                    transGrid.DataSource = transview.DefaultView;
                    transGrid.Columns[0].DataPropertyName = "sverified";
                    transGrid.Columns[1].DataPropertyName = "cuserid";
                    transGrid.Columns[2].DataPropertyName = "cacctnumb";
                    transGrid.Columns[3].DataPropertyName = "dpostdate";      
                    transGrid.Columns[4].DataPropertyName = "dtrandate";
                    transGrid.Columns[5].DataPropertyName = "ndebit";
                    transGrid.Columns[6].DataPropertyName = "ncredit";
                    transGrid.Columns[7].DataPropertyName = "ctrandesc";
                    transGrid.Columns[8].DataPropertyName = "jvno";
                    transGrid.Columns[9].DataPropertyName = "Cchqno";
                    transGrid.Columns[10].DataPropertyName = "cvoucherno";
                    transGrid.Columns[11].DataPropertyName = "jcstack";
                    transGrid.Columns[12].DataPropertyName = "tempupd";
                    ndConnHandle.Close();
                    transGrid.Focus();
                    for (int j = 0; j < transview.Rows.Count; j++)
                    {
                      totalDebit = totalDebit+Convert.ToDecimal(transview.Rows[j]["ndebit"]);
                      totalCredit = totalCredit + Convert.ToDecimal(transview.Rows[j]["ncredit"]);
                        if (Convert.ToBoolean(transview.Rows[j]["tempupd"]))
                        {
                            lcJournalVoucher = Convert.ToString(transview.Rows[j]["jvno"]);
                            transview.Rows[j]["sverified"] = 1;
                            temporaryFlagUpdate(lcJournalVoucher, true);
                            lnAccountCounter++;
                        }
                    }
                    maskedTextBox1.Text = totalDebit.ToString("N2");
                    maskedTextBox2.Text = totalCredit.ToString("N2");
                    gnSelectedCount = lnAccountCounter;
                    glAllAccountsOk = true;
                    glDebitCreditOk = Math.Abs(totalDebit) == Math.Abs(totalCredit) ? true : false;
                    AllClear2Go();
                }
            }
        }//end of getclientlist

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton4.Checked)
            {
                transDate.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                transDate.Enabled = true;
            }
        }

        private void transGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (transGrid.Focused)
            {
                if (transGrid.CurrentCell is DataGridViewCheckBoxCell)
                {
                    decimal lnDebit = 0.00m;
                    decimal lnCredit = 0.00m;
                    gnSelectedCount = 0;
                    bool dres1 = Convert.ToBoolean(transGrid.Rows[e.RowIndex].Cells["dupt"].Value);
                    if (dres1 == true)
                    {
                        string djvnumb = transGrid.Rows[e.RowIndex].Cells["djvno"].Value.ToString().Trim();
                        foreach (DataGridViewRow drow in transGrid.Rows)
                        {
                            if (drow.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                            {
                                drow.Cells["dupt"].Value = true;
                                string lcAcctNumb = Convert.ToString(drow.Cells["actnumb"].Value);
                                lnDebit = lnDebit + Convert.ToDecimal(drow.Cells["ddebit"].Value);
                                lnCredit = lnCredit + Convert.ToDecimal(drow.Cells["dcredit"].Value);
                                glAllAccountsOk = CheckAccountExistence(cs, lcAcctNumb);
                                if (!glAllAccountsOk)
                                {
                                  //  MessageBox.Show("This is the error " + drow.Cells["dupt"].Value);
                                  //  drow.Cells["dupt"].Value = "";
                                    drow.Cells["dupt"].Value = false;
                                    drow.DefaultCellStyle.BackColor = Color.Red;
                                    drow.DefaultCellStyle.ForeColor = Color.Yellow;
                                    temporaryFlagUpdate(djvnumb, false);
                                    break;
                                }
                            }
                        }
                        temporaryFlagUpdate(djvnumb, true);

                        glDebitCreditOk = Math.Abs(lnDebit) == Math.Abs(lnCredit) ? true : false;
                        if (!glDebitCreditOk)
                        {
                            foreach (DataGridViewRow drow1 in transGrid.Rows)
                            {
                                if (drow1.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                                {
                                    //   MessageBox.Show("This is the " + drow1.Cells["djvno"].Value.ToString().Trim());
                                 
                                    drow1.DefaultCellStyle.BackColor = Color.Red;
                                    drow1.DefaultCellStyle.ForeColor = Color.Yellow;
                                    temporaryFlagUpdate(djvnumb, false);
                                }
                            }
                        }
                    }
                    else
                    {
                        string djvnumb = transGrid.Rows[e.RowIndex].Cells["djvno"].Value.ToString().Trim();
                        foreach (DataGridViewRow drow in transGrid.Rows)
                        {
                            if (drow.Cells["djvno"].Value.ToString().Trim() == djvnumb)
                            {
                                drow.Cells["dupt"].Value = false;
                            }
                        }
                        temporaryFlagUpdate(djvnumb, false);
                    }

                    for (int j = 0; j < transGrid.Rows.Count; j++)
                    {
                        if (transGrid.Rows[j].Cells["actnumb"].Value != null && transGrid.Rows[j].Cells["actnumb"].Value.ToString() != "")// & transGrid.Rows[j].Cells["dupt"].Value != null)
                        {
                            if (Convert.ToBoolean(transGrid.Rows[j].Cells["dupt"].Value) == true)
                            {
                                gnSelectedCount ++;
                                break;
                            }
                        }
                        else
                        {
                            gnSelectedCount = 0;
                            break;
                        }
                    }
                }
                else { return; }
                AllClear2Go();
            }
        }


        private void temporaryFlagUpdate(string tcVoucherNo, bool tlUpdate)
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                dconn.Open();
                string lcUPdQuery = string.Empty;
                if (tlUpdate)
                {
                    lcUPdQuery = "update journal set tempupd = 1 where ltrim(rtrim(jvno)) =@tcvoucher";
                    SqlDataAdapter tmpUpd = new SqlDataAdapter();
                    tmpUpd.UpdateCommand = new SqlCommand(lcUPdQuery, dconn);
                    tmpUpd.UpdateCommand.Parameters.Add("@tcvoucher", SqlDbType.VarChar).Value = tcVoucherNo;
                    tmpUpd.UpdateCommand.ExecuteNonQuery();
                    //if (tlFirstTime)
                    //{
                        bool exists = tempVoucher.AsEnumerable().Where(c => c.Field<string>("jvno").Equals(tcVoucherNo)).Count() > 0;
                        if (!exists)
                        {
                            DataRow dr = tempVoucher.NewRow();
                            dr["jvno"] = tcVoucherNo;
                            tempVoucher.Rows.Add(dr);
                            gnTempRowCount = tempVoucher.Rows.Count;
                        }
                    //}
                    //else
                    //{
                    //    bool exists = tempVoucher.AsEnumerable().Where(c => c.Field<string>("jvno").Equals(tcVoucherNo)).Count() > 0;
                    //    if (!exists)
                    //    {
                    //        DataRow dr = tempVoucher.NewRow();
                    //        dr["jvno"] = tcVoucherNo;
                    //        tempVoucher.Rows.Add(dr);
                    //        gnTempRowCount = tempVoucher.Rows.Count;
                    //    }
                    //}
                }
                else
                {
                    lcUPdQuery = "update journal set tempupd = 0 where ltrim(rtrim(jvno)) =@tcvoucher";
                    SqlDataAdapter tmpUpd = new SqlDataAdapter();
                    tmpUpd.UpdateCommand = new SqlCommand(lcUPdQuery, dconn);
                    tmpUpd.UpdateCommand.Parameters.Add("@tcvoucher", SqlDbType.VarChar).Value = tcVoucherNo;
                    tmpUpd.UpdateCommand.ExecuteNonQuery();

                    bool exists = tempVoucher.AsEnumerable().Where(c => c.Field<string>("jvno").Equals(tcVoucherNo)).Count() > 0;
                    if (exists)
                    {
                        DataRow[] matches = tempVoucher.Select("jvno='" + tcVoucherNo + "'");
                        foreach (DataRow row in matches)
                        {
                            tempVoucher.Rows.Remove(row);
                        }
                    }
                    gnTempRowCount = tempVoucher.Rows.Count;
                }

                //if (gnTempRowCount > 0)
                //{
                //    MessageBox.Show("Temporary row count is " + gnTempRowCount + " and first rows is " + tempVoucher.Rows[0]["jvno"]);
                //}
                //else { MessageBox.Show("Row count is Zero"); }
            }
        }


        private void transGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void transGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (transGrid.CurrentCell is DataGridViewCheckBoxCell)
            {
                transGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            else { return; }

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            getrdata(0);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            getrdata(1);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            getrdata(2);
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            getrdata(3);
        }

        private void transGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
 
        }
    }
}
