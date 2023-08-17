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
    public partial class PeriodicDues : Form
    {
        DataTable duesView = new DataTable();
        int ncompid = globalvar.gnCompid;
        string cs = globalvar.cos;
        int gnDuesSelected = 0;
        int gnProductID = 0;
        string gcContraAccount = string.Empty;
        string gcControlAcct = string.Empty;
        public PeriodicDues()
        {
            InitializeComponent();
        }

        private void PeriodicDues_Load(object sender, EventArgs e)
        {
                this.Text = globalvar.cLocalCaption + " << Periodic Subscription Processing ";
            dProcessDate.MaxDate = DateTime.Now;
            getPeriodicDues();
            duesGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            duesGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }


        private void getDues()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                duesView.Clear();
                ndConnHandle.Open();
                string dsql = "exec tsp_GetAnnualDues " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(duesView);
                if (duesView.Rows.Count > 0)
                {
                    duesGrid.AutoGenerateColumns = false;
                    duesGrid.DataSource = duesView.DefaultView;
                    duesGrid.Columns[1].DataPropertyName = "anndues";
                    duesGrid.Columns[2].DataPropertyName = "feefreq";
                    duesGrid.Columns[3].DataPropertyName = "profreq";
                    duesGrid.Columns[4].DataPropertyName = "annual_dues";
                    ndConnHandle.Close();
                }
            }
        }

        private void getPeriodicDues()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql = "exec tsp_GetPeriodicDues " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(duesView);
                if (duesView.Rows.Count > 0)
                {
                    duesGrid.AutoGenerateColumns = false;
                    duesGrid.DataSource = duesView.DefaultView;
                    duesGrid.Columns[1].DataPropertyName = "anndues";
                    duesGrid.Columns[2].DataPropertyName = "feefreq";
                    duesGrid.Columns[3].DataPropertyName = "profreq";
                    duesGrid.Columns[4].DataPropertyName = "annual_dues";
                    ndConnHandle.Close();
                }
            }
        }



        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                page01ok();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                page01ok();
            }
        }

        private void page01ok() //staff basic details 
        {


            if (gnDuesSelected > 0)
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
        private void button12_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void duesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (duesGrid.Focused)
            {
                //if (duesGrid.Columns[e.ColumnIndex].Name == "selPrd")
                //      {
                //      if (Convert.ToBoolean(duesGrid.CurrentCell.Value))
                //      {
                //          gnDuesSelected++;
                //      }
                //      else
                //      {
                //          gnDuesSelected--;
                //      }
                //      page01ok();
                //  }
            }
        }

        private void duesGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            duesGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int linecount = duesGrid.Rows.Count;
            int j = 1;
            foreach (DataGridViewRow clrow in duesGrid.Rows)
            {
                bool tlSelected = Convert.ToBoolean(clrow.Cells["selPrd"].Value);
                if (tlSelected)
                {
                    string tcAcctCode = duesView.Rows[clrow.Index]["souproduct"].ToString().Trim();
                    gnProductID = Convert.ToInt16(duesView.Rows[clrow.Index]["prd_id"]);
                   // MessageBox.Show("This is the product" + gnProductID);
                    decimal tnDedAmouknt = Convert.ToDecimal(duesView.Rows[clrow.Index]["annual_dues"]);
                    string tcDueName = duesView.Rows[clrow.Index]["anndues"].ToString().Trim();
                    bool tlmem2mem = Convert.ToBoolean(duesView.Rows[clrow.Index]["destype"]);
                    gcContraAccount = duesView.Rows[clrow.Index]["desacct"].ToString().Trim();
                    string memContra = tlmem2mem ? duesView.Rows[clrow.Index]["desacct"].ToString().Trim() : "";
                    progressBar1.Value = 0;
                    gcControlAcct = getProductControl.productControl(cs, gnProductID);
               //     MessageBox.Show("This is the product " + gcControlAcct);
//                    getProductControl(gnProductID, out gcControlAcct);
                    if (checkBox2.Checked)
                    {
                        getAccountsSMS(tcAcctCode, tnDedAmouknt, tcDueName, gcContraAccount, tlmem2mem, memContra);
                    }
                    else
                    {
                        getAccounts(tcAcctCode, tnDedAmouknt, tcDueName, gcContraAccount, tlmem2mem, memContra);
                    }
                  
                }
            }
            initvariables();
        }

        private void initvariables()
        {
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            dProcessDate.MaxDate = DateTime.Now;
            progressBar1.Value = 0;
            duesView.Clear();
        }

        private void getAccounts(string tcAct, decimal tnAmt, string tcAnnDues, string tcContraAccount, bool mem2mem, string memact)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                updateJournal upj = new updateJournal();
                string dsql = "exec tsp_GetMemAccts @tncompid,@tsouAcct,@tdesAcct";//"select cacctnumb from glmast where intcode=0 and acode = '" + tcAct + "'"; 
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(dsql, ndConnHandle);

                selCommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                selCommand.SelectCommand.Parameters.Add("@tsouAcct", SqlDbType.VarChar).Value = tcAct;
                selCommand.SelectCommand.Parameters.Add("@tdesAcct", SqlDbType.VarChar).Value = memact;
                DataTable acctview = new DataTable();
                ndConnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                selCommand.Fill(acctview);
                if (acctview.Rows.Count > 0)
                {
                    int linecount = acctview.Rows.Count;
                    int j = 1;
                    decimal nTotal = 0.00m;
                    for (int k = 0; k < linecount; k++)
                    {
                        string dcustcode = acctview.Rows[k]["ccustcode"].ToString().Trim();
                        string dSouAcct = acctview.Rows[k]["souacct"].ToString().Trim();
                        string dDesAcct = acctview.Rows[k]["desacct"].ToString().Trim();
                        if (dSouAcct != "")
                        {
                            postMemberAccounts(dSouAcct, tnAmt, tcAnnDues, true);
                        }


                        if (mem2mem && dDesAcct != "")
                        {
                            postMemberAccounts(dDesAcct, tnAmt, tcAnnDues, false);
                        }
                        nTotal = nTotal + tnAmt;
                        progressBar1.Value = j * progressBar1.Maximum / linecount;
                        j++;
                    }

                    ///posting to gl:  product control account of source  - debit, defined income account - credit     and transaction sent for verification sent for verification
                    //postGlAccounts(tcContraAccount, nTotal, tcAnnDues, false);  //for member to GL deductions
                    decimal tnPostAmt = -Math.Abs(nTotal);
                    decimal tnContAmt = Math.Abs(nTotal);
                    string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);

                    //updateJournal(gcControlAcct, tcAnnDues, tnPostAmt, tcVoucher, "19", dProcessDate.Value);
                    upj.updJournal(cs, gcControlAcct,tcAnnDues,tnPostAmt, tcVoucher,"19",dProcessDate.Value, globalvar.gcUserid, globalvar.gnCompid);

                    //updateJournal(tcContraAccount, tcAnnDues, tnContAmt, tcVoucher, "18", dProcessDate.Value);
                    upj.updJournal(cs, tcContraAccount, tcAnnDues,tnContAmt, tcVoucher,"18",dProcessDate.Value, globalvar.gcUserid, globalvar.gnCompid);



                    MessageBox.Show(tcAnnDues + " successfully processed  ");
                }
            }
        }

        // For SMS
        private void getAccountsSMS(string tcAct, decimal tnAmt, string tcAnnDues, string tcContraAccount, bool mem2mem, string memact)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                updateJournal upj = new updateJournal();
                string dsql = "exec tsp_GetMemAcctsSMS @tncompid,@tsouAcct,@tdesAcct";//"select cacctnumb from glmast where intcode=0 and acode = '" + tcAct + "'"; 
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(dsql, ndConnHandle);

                selCommand.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                selCommand.SelectCommand.Parameters.Add("@tsouAcct", SqlDbType.VarChar).Value = tcAct;
                selCommand.SelectCommand.Parameters.Add("@tdesAcct", SqlDbType.VarChar).Value = memact;
                DataTable acctview = new DataTable();
                ndConnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                selCommand.Fill(acctview);
                if (acctview.Rows.Count > 0)
                {
                    int linecount = acctview.Rows.Count;
                    int j = 1;
                    decimal nTotal = 0.00m;
                    for (int k = 0; k < linecount; k++)
                    {
                        string dcustcode = acctview.Rows[k]["ccustcode"].ToString().Trim();
                        string dSouAcct = acctview.Rows[k]["souacct"].ToString().Trim();
                        string dDesAcct = acctview.Rows[k]["desacct"].ToString().Trim();
                        if (dSouAcct != "")
                        {
                            postMemberAccounts(dSouAcct, tnAmt, tcAnnDues, true);
                        }


                        if (mem2mem && dDesAcct != "")
                        {
                            postMemberAccounts(dDesAcct, tnAmt, tcAnnDues, false);
                        }
                        nTotal = nTotal + tnAmt;
                        progressBar1.Value = j * progressBar1.Maximum / linecount;
                        j++;
                    }

                    ///posting to gl:  product control account of source  - debit, defined income account - credit     and transaction sent for verification sent for verification
                    decimal tnPostAmt = -Math.Abs(nTotal);
                    decimal tnContAmt = Math.Abs(nTotal);
                    string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);

                    //updateJournal(gcControlAcct, tcAnnDues, tnPostAmt, tcVoucher, "19", dProcessDate.Value);
                    upj.updJournal(cs, gcControlAcct, tcAnnDues, tnPostAmt, tcVoucher, "19", dProcessDate.Value, globalvar.gcUserid, globalvar.gnCompid);

                    //updateJournal(tcContraAccount, tcAnnDues, tnContAmt, tcVoucher, "18", dProcessDate.Value);
                    upj.updJournal(cs, tcContraAccount, tcAnnDues, tnContAmt, tcVoucher, "18", dProcessDate.Value, globalvar.gcUserid, globalvar.gnCompid);



                    MessageBox.Show(tcAnnDues + " successfully processed  ");
                }
            }
        }
        private void postMemberAccounts(string tcAcctNumb, decimal lnTranAmt, string tcNarrative, bool tlDebitCredit)//, decimal lnAccrInt, string tccontra, int prid)
        {
            string tcUserid = globalvar.gcUserid;
            string tcDesc = tcNarrative + " " + tcAcctNumb;// "Periodic Dayroll Processing " + tcAcctNumb;
            string dacode = tcAcctNumb.Substring(0, 3);  //we need to identify whether savings, shares or loans
            string tcCustcode = tcAcctNumb.Substring(3, 6);
            decimal tnTranAmt = tlDebitCredit ? -Math.Abs(lnTranAmt) : Math.Abs(lnTranAmt);
            decimal tnContAmt = -tnTranAmt;
            string tcTranCode = tlDebitCredit ? "19" : "18";//  (dacode == "250" || dacode == "251" || dacode == "270" || dacode == "271" ? "01" : "07");
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            decimal lnWaiveAmt = 0.00m;
            string tcChqno = "";
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 1;  //for service centres/departments, etc
            int lnServID = gnProductID; //for services/products - this is not in use for T-care Medical
            bool lFreeBee = false;

            updateGlmast gls = new updateGlmast();
           // updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();

            //************************************************************************************************************************************************************
            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update accrued interest or interest charged - debit
            decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, tcAcctNumb);
            tcCustcode = tcAcctNumb.Substring(3, 6);
            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal0, tcTranCode, lnServID, llPaid, globalvar.gcIntSuspense, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
            srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, dProcessDate.Value, dProcessDate.Value);                          //update tranhist posting account
            string auditDesc = tcDesc; //Audit Trail
            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
        }

        //private void updateJournal(string actnumb, string desc, decimal tamt, string tcvou, string trancode, DateTime dtransDate)
        //{
        //    using (SqlConnection nConnHandle2 = new SqlConnection(cs))
        //    {
        //        int jourtype = (trancode == "01" ? 2 :  //Deposits
        //        (trancode == "02" ? 3 : //Withdrawals
        //        (trancode == "07" ? 4 : //Loan Repayment
        //        (trancode == "09" ? 5 : //"<< Loan Payout/close >>" :
        //        (trancode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));
        //        string cglquery = "Insert Into journal (cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,jour_TYPE)";
        //        cglquery += " VALUES  (@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@jtype)";
        //        nConnHandle2.Open();
        //        SqlDataAdapter insCommand = new SqlDataAdapter();
        //        insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
        //        insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
        //        insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
        //        insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dtransDate;
        //        insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = desc;
        //        insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = actnumb;
        //        insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
        //        insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = tamt;
        //        insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = tcvou;
        //        insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
        //        insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
        //        insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
        //        insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = trancode;
        //        insCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;

        //        insCommand.InsertCommand.ExecuteNonQuery();
        //        insCommand.InsertCommand.Parameters.Clear();
        //        nConnHandle2.Close();
        //        //                updateClientCode();
        //    }
        //}

        private void duesGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (duesGrid.Focused)
            {
                if (duesGrid.Columns[e.ColumnIndex].Name == "selPrd")
                {
                    if (Convert.ToBoolean(duesGrid.CurrentCell.Value))
                    {
                        ptype.Text = Convert.ToBoolean(duesView.Rows[Convert.ToInt16(duesGrid.CurrentRow.Index)]["destype"]) ? "Member to Member" : "Member to GL";

                        //                        MessageBox.Show("cell is selected in cellvaluechanged");
                        gnDuesSelected++;
                    }
                    else
                    {
                        gnDuesSelected--;
                        //            MessageBox.Show("cell is deselected in cellvaluechanged");
                    }
                    page01ok();
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {

        }

        //private string getProductControl(int lprid, out string dacct)
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
    }
}
