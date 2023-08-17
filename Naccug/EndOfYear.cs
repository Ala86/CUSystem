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
    public partial class EndOfYear : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int nbranch = globalvar.gnBranchid;
        DataTable transview = new DataTable();
        DateTime gdToday = globalvar.gdSysDate;
        updateClient_Code ucc = new updateClient_Code();
        updateJournal upj = new updateJournal();
        public EndOfYear()
        {
            InitializeComponent();
        }

        private void EndOfYear_Load(object sender, EventArgs e)
        {
            transGrid.Columns["baldate"].DefaultCellStyle.Format = "dd/MMM/yyyy";
            transGrid.Columns["debitclose"].SortMode = DataGridViewColumnSortMode.NotSortable;
            transGrid.Columns["debitclose"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            transGrid.Columns["creditclose"].SortMode = DataGridViewColumnSortMode.NotSortable;
            transGrid.Columns["creditclose"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            getmyAccts();

            int djv0 = 0;
            string djv = string.Empty;
            djv0 = GetClient_Code.clientCode_int(cs, "jv_no");//  ucc. updateClient_Code

            if (djv0 > 10000)
            {
                resetClient_Code rcc = new resetClient_Code();
                rcc.setClient(cs, "jv_no");
                djv0 = 1;
            }
            djv = djv0.ToString().Trim().PadLeft(4, '0');

            textBox2.Text = gdToday.Year.ToString().Trim().Substring(0, 2) + gdToday.Month.ToString().Trim() + gdToday.Day.ToString().Trim() + "-" + djv.Trim();


        }

        private void getrdata()
        {
            string dsql = string.Empty;
            transview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
               dsql = "exec tsp_EndOfYear";
                transview.Clear();
                decimal totalDebit = 0.00m;
                decimal totalCredit = 0.00m;
                int lnAccountCounter = 0;
            SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(transview);
                if (transview.Rows.Count > 0)
                {
                    transGrid.AutoGenerateColumns = false;
                    transGrid.DataSource = transview.DefaultView;
                    transGrid.Columns[0].DataPropertyName = "baldate";
                    transGrid.Columns[1].DataPropertyName = "cacctnumb";
                    transGrid.Columns[2].DataPropertyName = "cacctname";
                    transGrid.Columns[3].DataPropertyName = "debitClose";
                    transGrid.Columns[4].DataPropertyName = "creditClose";

                    ndConnHandle.Close();
                    transGrid.Focus();
                    for (int j = 0; j < transview.Rows.Count; j++)
                    {
                        totalDebit = totalDebit + Convert.ToDecimal(transview.Rows[j]["debitclose"]);
                        totalCredit = totalCredit + Convert.ToDecimal(transview.Rows[j]["creditclose"]);
                            lnAccountCounter++;
                    }
                    textBox1.Text = totalDebit.ToString("N2");
                    textBox2.Text = totalCredit.ToString("N2");
                    decimal ala = totalCredit - totalDebit;
                    textBox3.Text = ala.ToString("N2"); 
                }
            }
        }

        private void getmyAccts()
        {
            string cglquery = "select cacctname,glmast.cacctnumb from glmast where glmast.intcode = 1 and glmast.acode in ('320')";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(cglquery, nconnHandle);
                //selCommand.SelectCommand.Parameters.Add("@lcTcode", SqlDbType.VarChar).Value = tcode;
                //selCommand.SelectCommand.Parameters.Add("@lcAcode", SqlDbType.VarChar).Value = acode;
                DataTable myacctview = new DataTable();
                nconnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                nconnHandle.Close();

                selCommand.Fill(myacctview);
                if (myacctview.Rows.Count > 0)
                {
                    comboBox2.DataSource = myacctview.DefaultView;
                    comboBox2.DisplayMember = "cacctname";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }

        private void debitaccount()
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                if (MessageBox.Show("Are you ready to run End of Year procedures", "Transaction Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string tcDesc = "End of Year ";
                    string lcJVno = textBox2.Text.Replace("-", "");
                    string lTrancode = "08";
                    string cglquery = "Insert Into journal (nbookbal,nnewbal,cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,ccurrcode,branchid)";
                    cglquery += " VALUES  (@tnbookbal,@tnnewbal,@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@lnCurrCode,@lnBranchid)";
                    dconn.Open();
                    foreach (DataGridViewRow dr in transGrid.Rows)
                    {
                        decimal ala = -Math.Abs(Convert.ToDecimal(dr.Cells["debitclose"].Value));
                        if (Convert.ToDecimal(transview.Rows[0]["code"]) == 1 || Convert.ToDecimal(transview.Rows[0]["code"]) == 2 || Convert.ToDecimal(transview.Rows[0]["code"]) == 3) //|| Convert.ToDecimal(dr.Cells["dcredit"].Value) > 0.00m
                        {
                            MessageBox.Show("This is the debit Amount " + ala);
                          //  Convert.ToDecimal(transview.Rows[0]["code"]) == 1 || Convert.ToDecimal(transview.Rows[0]["code"]) == 2 || Convert.ToDecimal(transview.Rows[0]["code"]) == 3
                            decimal dbjouramt = -Math.Abs(Convert.ToDecimal(dr.Cells["debitclose"].Value)); //(Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                            decimal lnBookBal = -Math.Abs(Convert.ToDecimal(dr.Cells["debitclose"].Value));
                            decimal lnNewBal = lnBookBal + Math.Abs(dbjouramt);
                            SqlDataAdapter insCommand = new SqlDataAdapter();

                            insCommand.InsertCommand = new SqlCommand(cglquery, dconn);
                            insCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnBookBal;
                            insCommand.InsertCommand.Parameters.Add("@tnnewbal", SqlDbType.Decimal).Value = lnNewBal;
                            insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
                            insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
                            insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dateTimePicker1.Value;//dr.Cells["ddate"].Value;
                            insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = tcDesc;
                            insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = dr.Cells["cacctnumb"].Value.ToString();
                            insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                            insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = Math.Abs(dbjouramt); // Convert.ToDecimal(dr.Cells["debitclose"].Value);
                            insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);// textBox2.Text;
                            insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                            insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                            insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
                            insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = lTrancode;
                            insCommand.InsertCommand.Parameters.Add("@lnCurrCode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                            insCommand.InsertCommand.Parameters.Add("@lnBranchid", SqlDbType.Int).Value = globalvar.gnBranchid;

                            insCommand.InsertCommand.ExecuteNonQuery();
                            insCommand.InsertCommand.Parameters.Clear();
                            string auditDesc = "End Of Year " + dr.Cells["cacctnumb"].Value.ToString();
                            AuditTrail au = new AuditTrail();
                            au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(dbjouramt), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            upj.updJournal(cs, Convert.ToString(comboBox2.SelectedValue), tcDesc, -Math.Abs(dbjouramt), genbill.genvoucher(cs, globalvar.gdSysDate), lTrancode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                            //upj.updJournal(cs, gcIntAcct, tcDesc, Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                            //updcl.updClient(cs, "nvoucherno");
                            //updcl.updClient(cs, "stackcount");

                        }


                    }
                    ucc.updClient(cs, "jv_no");
                    dconn.Close();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection dconn = new SqlConnection(cs))
            {
                if (Convert.ToInt16(comboBox2.SelectedIndex) > -1)
                {
                    if (MessageBox.Show("Are you ready to run End of Year procedures", "Transaction Update Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        string tcDesc = "End of Year ";
                        string lcJVno = textBox2.Text.Replace("-", "");
                        string lTrancode = "08";
                        string cglquery = "Insert Into journal (nbookbal,nnewbal,cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,ccurrcode,branchid)";
                        cglquery += " VALUES  (@tnbookbal,@tnnewbal,@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@lnCurrCode,@lnBranchid)";
                        dconn.Open();
                        foreach (DataGridViewRow dr in transGrid.Rows)
                        {
                            decimal ala = -Math.Abs(Convert.ToDecimal(dr.Cells["debitclose"].Value));
                        //    MessageBox.Show("This is the debit Balances " + ala);
                            if (ala < 0.00m) //|| Convert.ToDecimal(dr.Cells["dcredit"].Value) > 0.00m
                            {
                                decimal dbjouramt = -Math.Abs(Convert.ToDecimal(dr.Cells["debitclose"].Value)); //(Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                                decimal lnBookBal = -Math.Abs(Convert.ToDecimal(dr.Cells["debitclose"].Value));
                                decimal lnNewBal = lnBookBal + Math.Abs(dbjouramt);
                                SqlDataAdapter insCommand = new SqlDataAdapter();

                                insCommand.InsertCommand = new SqlCommand(cglquery, dconn);
                                insCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnBookBal;
                                insCommand.InsertCommand.Parameters.Add("@tnnewbal", SqlDbType.Decimal).Value = lnNewBal;
                                insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
                                insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
                                insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dateTimePicker1.Value;//dr.Cells["ddate"].Value;
                                insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = tcDesc;
                                insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = dr.Cells["cacctnumb"].Value.ToString();
                                insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                                insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = Math.Abs(dbjouramt); // Convert.ToDecimal(dr.Cells["debitclose"].Value);
                                insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);// textBox2.Text;
                                insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                                insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                                insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
                                insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = lTrancode;
                                insCommand.InsertCommand.Parameters.Add("@lnCurrCode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                                insCommand.InsertCommand.Parameters.Add("@lnBranchid", SqlDbType.Int).Value = globalvar.gnBranchid;

                                insCommand.InsertCommand.ExecuteNonQuery();
                                insCommand.InsertCommand.Parameters.Clear();
                                string auditDesc = "End Of Year " + dr.Cells["cacctnumb"].Value.ToString();
                                AuditTrail au = new AuditTrail();
                                au.upd_audit_trail(cs, auditDesc, 0.00m, Math.Abs(dbjouramt), globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                                upj.updJournal(cs, Convert.ToString(comboBox2.SelectedValue), tcDesc, -Math.Abs(dbjouramt), genbill.genvoucher(cs, globalvar.gdSysDate), lTrancode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                                //upj.updJournal(cs, gcIntAcct, tcDesc, Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                                //updcl.updClient(cs, "nvoucherno");
                                //updcl.updClient(cs, "stackcount");

                            } else
                            {
                                //if (Convert.ToDecimal(dr.Cells["creditclose"].Value) > 0.00m) //|| Convert.ToDecimal(dr.Cells["dcredit"].Value) > 0.00m
                                //{
                                    decimal crjouramt = Convert.ToDecimal(dr.Cells["creditclose"].Value); // (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                                    decimal lnBookBal = Convert.ToDecimal(dr.Cells["creditclose"].Value);
                                    decimal lnNewBal = lnBookBal + -crjouramt;
                                    SqlDataAdapter insCommand = new SqlDataAdapter();

                                    insCommand.InsertCommand = new SqlCommand(cglquery, dconn);
                                    insCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnBookBal;
                                    insCommand.InsertCommand.Parameters.Add("@tnnewbal", SqlDbType.Decimal).Value = lnNewBal;
                                    insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
                                    insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
                                    insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dateTimePicker1.Value;//dr.Cells["ddate"].Value;
                                    insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = tcDesc;
                                    insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = dr.Cells["cacctnumb"].Value.ToString();
                                    insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                                    insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = -crjouramt; // Convert.ToDecimal(dr.Cells["creditclose"].Value);
                                    insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);// textBox2.Text;
                                    insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                                    insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                                    insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
                                    insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = lTrancode;
                                    insCommand.InsertCommand.Parameters.Add("@lnCurrCode", SqlDbType.Int).Value = globalvar.gnCurrCode;
                                    insCommand.InsertCommand.Parameters.Add("@lnBranchid", SqlDbType.Int).Value = globalvar.gnBranchid;

                                    insCommand.InsertCommand.ExecuteNonQuery();
                                    insCommand.InsertCommand.Parameters.Clear();
                                    string auditDesc = "End Of Year " + dr.Cells["cacctnumb"].Value.ToString();
                                    AuditTrail au = new AuditTrail();
                                    au.upd_audit_trail(cs, auditDesc, 0.00m, -crjouramt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                                    upj.updJournal(cs, Convert.ToString(comboBox2.SelectedValue), tcDesc, crjouramt, genbill.genvoucher(cs, globalvar.gdSysDate), lTrancode, globalvar.gdSysDate, globalvar.gcUserid, globalvar.gnCompid);
                                    //upj.updJournal(cs, gcIntAcct, tcDesc, Math.Abs(gnTotalAccruedInterest), tcVoucher, tcTranCode, dTranDate.Value, globalvar.gcUserid, globalvar.gnCompid);
                                    //updcl.updClient(cs, "nvoucherno");
                                    //updcl.updClient(cs, "stackcount");

                               // }
                            }

                        }
                        ucc.updClient(cs, "jv_no");                      
                        transview.Clear();
                        dconn.Close();
                    }
               } else {
                    MessageBox.Show("Kindly select the contra account ");
              }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getrdata();
        }
    }
}
