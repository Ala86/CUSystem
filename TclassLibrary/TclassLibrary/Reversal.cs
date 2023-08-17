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

namespace TclassLibrary
{
    public partial class Reversal : Form
    {
        DataTable reverseview = new DataTable();
        DataTable transview = new DataTable();
        DateTime gdToday = new DateTime();

        int gnRowCount = 0;
        int gnSelCount = 0;
        string gcRevType = string.Empty;
        string gcStack = string.Empty;
        string revFrom = string.Empty;
        string gcPostControlAcct = string.Empty;
        string gcContControlAcct = string.Empty;
        bool glAccountIsGl = false;

        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        string UserID = string.Empty;
        int nBranchid = 0;
        int nCurrCode = 0;

        public Reversal(string tcCos, int tnCompid, string tcLoca,DateTime tdSysDate,string tcUserID, int tnBranchid,int tnCurrCode,string tcRevFrom)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            gdToday = tdSysDate;
            UserID = tcUserID;
            nBranchid = tnBranchid;
            nCurrCode = tnCurrCode;
            revFrom = tcRevFrom == "T" ? "patients" : "cusreg";
        }

        private void Reversal_Load(object sender, EventArgs e)
        {
            this.Text = dloca + " << Transaction Reversal / Adjustment >>";
            transGrid.Columns["ddebit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            transGrid.Columns["ddebit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            transGrid.Columns["dcredit"].SortMode = DataGridViewColumnSortMode.NotSortable;
            transGrid.Columns["dcredit"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            if (textBox1.Text.ToString()!= "" && gnRowCount>0 && gnSelCount>0)
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

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             With Thisform
	This.Enabled = .F.
	If Messagebox("Please confirm reversal of transaction",4+32+256,"Reversal Confirmation")=6
		Select jourrecs
		Locate
		gnPat_ID=pat_id
		lnPrice=gnTranAmt
		gdPostDate=dpostDate
		gdValudate=dvaluedate
		gcReveVouch=cvoucherno
		lnServID=serv_id

***************************************** update for patient account - posting account ***********************************
		lnPostAmt=lnPrice
		.updgl(gcAcctNumb,lnPostAmt)
		gnPNewBal=.chklastbalance(gcAcctNumb)
		.updtrans(gcAcctNumb,lnPostAmt,gcTranNarr,gdSysDate,gdSysDate,gcReveVouch,'000001',gnPNewBal,'90',lnServID,gnPat_ID,gnCompID,gcContra,0.00,'')

***************************************** update for patient control account - contra account *****************************
		lnContAmt=-lnPrice
		gnDIncome=lnContAmt
		gnDAcct_rec=lnContAmt
		lnPatID=''
		.updgl(gcContra,lnContAmt)
		gncNewBal=Thisform.chklastbalance(gcContra)
		.updtrans(gcContra,lnContAmt,gcTranNarr,gdSysDate,gdSysDate,gcReveVouch,'000001',gncNewBal,'90',lnServID,gnPat_ID,gnCompID,gcAcctNumb,0.00,'')
 		.updreverseflag
	Endif
	gcVoucherNo=''
	.text18.Value=''
	.text1.Value=0.00
	Thisform.definegrid
 	.Refresh
Endwith
             */
        }



        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            transview.Clear();
            textBox2.Text = textBox1.Text =  "";
            label1.Text = radioButton3.Checked ? "Member No" : "Voucher No";
            label5.Text = radioButton3.Checked ? "Member Name" : "Voucher Name";
            textBox1.Visible = radioButton3.Checked ? true : false;
            maskedTextBox1.Visible = radioButton3.Checked ? false : true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            for(int i=0;i<transview.Rows.Count;i++)
            {
                if (transGrid.Rows[i].Cells["seltrans"].Value != null && transGrid.Rows[i].Cells["seltrans"].Value.ToString()!="")  
                {
                    if (Convert.ToBoolean(transGrid.Rows[i].Cells["seltrans"].Value))
                        {
                            decimal tnDebit = Convert.ToDecimal(transview.Rows[i]["ndebit"]);
                            decimal tnCredit = Convert.ToDecimal(transview.Rows[i]["ncredit"]);
                            DateTime dtranDate = Convert.ToDateTime(transview.Rows[i]["dtrandate"]);
                            DateTime dvaueDate = Convert.ToDateTime(transview.Rows[i]["dvaluedate"]);
                            gcStack = transview.Rows[i]["cstack"].ToString();

                            string tcAcctNumb = transview.Rows[i]["cacctnumb"].ToString().Trim();
                            bool postIsGl = checkInternalStatus(tcAcctNumb, cs);

                            string tcContra = transview.Rows[i]["ccontra"].ToString().Trim();
                            bool contIsGl  = checkInternalStatus(tcContra, cs);
                         
                            string tcDesc = radioButton1.Checked ?  "Transaction Reversal <<" : " Transaction Adjustment <<" + transview.Rows[i]["ctrandesc"].ToString().Trim()+" >> ";
                            int tncompid = ncompid;
                            string tcUserid = UserID;
                            string tcTranCode = radioButton1.Checked ? "16" : "21";
                            string auditDesc = radioButton1.Checked ? " Transaction Reversal " : " Transaction Adjustment ";
                            decimal tnTranAmt = tnDebit > 0.00m ? Math.Abs(tnDebit) : -Math.Abs(tnCredit);
                            decimal tnContAmt = -tnTranAmt;
                            string tcVoucher = CuVoucher.genCuVoucher(cs, gdToday);
                            decimal unitprice = Math.Abs(tnTranAmt);
                            string tcChqno = "000001";
                            decimal lnWaiveAmt = 0.00m;
                            int lnServID = 0;
                            bool llPaid = true;
                            int tnqty = 1;
                            string tcReceipt = "";
                            bool llCashpay = true;
                            int visno = 1;
                            bool isproduct = false;
                            int srvid = 1;
                            bool lFreeBee = false;
                            string tcCustcode = string.Empty;

                        updateGlmast gls = new updateGlmast();
                       // updateDailyBalance dbal = new updateDailyBalance();
                        updateCuTranhist tls1 = new updateCuTranhist();
                        AuditTrail au = new AuditTrail();
                        updateJournal upj = new updateJournal();


                        if (postIsGl)  //posting account is a GL account, such as in journal
                        {
                            upj.updJournal(cs, tcAcctNumb, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dateTimePicker1.Value, UserID,ncompid);
                            auditDesc = auditDesc + " " + tcAcctNumb;
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                        }
                        else
                        {
                            int tnPostPrd = getProductId(tcAcctNumb, cs);
                            gcPostControlAcct = getProductControl.productControl(cs, tnPostPrd);
                            gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
                            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                            tcCustcode = tcAcctNumb.Substring(3, 6);
                            tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                            srvid, "", "", lFreeBee, tcCustcode, tncompid, nBranchid, nCurrCode, dateTimePicker1.Value, dateTimePicker1.Value);                          //update tranhist posting account
                            auditDesc = auditDesc + " " + tcAcctNumb;
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            upj.updJournal(cs, gcPostControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode, dateTimePicker1.Value, UserID,ncompid);
                            auditDesc = auditDesc + " " + gcPostControlAcct;
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                        }

                        if(contIsGl)    //contra account is  a GL account, such as in journal
                        {
                            upj.updJournal(cs, tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode, dateTimePicker1.Value, UserID,ncompid);
                            auditDesc = auditDesc + " " + tcContra;
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                        }
                        else
                        {
                            int tnContPrd = getProductId(tcContra, cs);
                            gcContControlAcct = getProductControl.productControl(cs, tnContPrd);
                          //  gls.updGlmast(cs, tcContra, tnContAmt);                                //update glmast contra account
                            decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);        // 0.00m;
                            tcCustcode = tcContra.Substring(3, 6);
                          //  tls1.updCuTranhist(cs, tcContra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                          //  srvid, "", "", lFreeBee, tcCustcode, tncompid, nBranchid, nCurrCode, dtranDate, dvaueDate);                        //update tranhist account 
                            auditDesc = auditDesc + " " + tcContra;
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnContAmt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            upj.updJournal(cs, gcContControlAcct, tcDesc, tnContAmt, tcVoucher, tcTranCode, dateTimePicker1.Value, UserID,ncompid);
                            auditDesc = auditDesc + " " + gcContControlAcct;
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnContAmt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                        }
                        updateReverseFlag(gcStack);

                        string dProBy = radioButton3.Checked ? "M" : "V";
                        string cCode = textBox1.Text.Trim().PadLeft(6, '0');
                        getTrans(dProBy, cCode);
                        gnSelCount = 0;
                        AllClear2Go();
                    }
                }
            }
        }

        private static int getProductId(string tcAcctNumb,string cs1)
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

        private static bool  checkInternalStatus(string tcAcctNumb, string cs1 )
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs1))
            {
                string cquery = "select intcode,prd_id from glmast where cacctnumb = @lcAcctNumb and intcode = 1 ";

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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void updateReverseFlag(string tcStack)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string cquery = "update tranhist set reverseflag =1 where cstack = @tcDStack";

                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle);
                cuscommand.UpdateCommand.Parameters.Add("@tcDStack", SqlDbType.VarChar).Value = tcStack; 
                ndConnHandle.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle.Close();
                MessageBox.Show("Transaction(s) "+(radioButton1.Checked ? "Reversed " : "Adjusted ")+" successfully");
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Text = radioButton1.Checked ? "Reverse by " : "Adjust by ";
            radioButton3.Checked = true;
            radioButton4.Checked = false;
        }

        private void button29_Click(object sender, EventArgs e)
        {
             FindClient fc = new FindClient(cs,ncompid, dloca, 1,revFrom);
            fc.ShowDialog();
            string retval = fc.returnValue;
            string dProBy = radioButton3.Checked ? "M" : "V";
            getMember(retval);
            getTrans(dProBy, retval);
            textBox1.Text = retval;
            AllClear2Go();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            string dProBy = radioButton3.Checked ? "M" : "V";
            string cCode = textBox1.Text.Trim().PadLeft(6, '0');
            textBox1.Text = cCode;
            getMember(cCode);
            getTrans( dProBy, cCode);
            AllClear2Go();
        }

        private void getMember(string tcMembCode)
        {
            string dsql = "select ltrim(RTRIM(ccustfname))+' '+ltrim(rtrim(ccustmname))+' '+ltrim(rtrim(ccustlname)) as membername from cusreg where ccustcode = " + "'" + tcMembCode + "'"; 
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable memview = new DataTable();
                da.Fill(memview);
                if (memview.Rows.Count > 0)
                {
                    textBox2.Text = memview.Rows[0]["membername"].ToString().Trim();
                }
            }
        }
        private void getTrans(string ttype, string tcSearchKey)
        { 
            transview.Clear();
            decimal totrev = 0.00m;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                if (radioButton1.Checked) //transaction reversal
                {
                    string dsql = (ttype == "M" ? "exec tsp_ReversalByMember " : "exec tsp_ReversalByVoucher   ") + ncompid + ",'" + tcSearchKey + "'";
                    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    da.Fill(transview);
                }
                else //transaction adjustment
                {
                    string dsql = (ttype == "M" ? "exec tsp_AdjustmentByMember " : "exec tsp_AdjustmentByVoucher   ") + ncompid + ",'" + tcSearchKey + "'";
                    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    da.Fill(transview);
                }
                if (transview.Rows.Count > 0)
                {
                    gnRowCount = transview.Rows.Count;
                    transGrid.AutoGenerateColumns = false;
                    transGrid.DataSource = transview.DefaultView;
                    transGrid.Columns[0].DataPropertyName = "reverseflag";
                    transGrid.Columns[1].DataPropertyName = "dpostdate";
                    transGrid.Columns[2].DataPropertyName = "ndebit";
                    transGrid.Columns[3].DataPropertyName = "ncredit";
                    transGrid.Columns[4].DataPropertyName = "ctrandesc";
                    ndConnHandle.Close();
                    for(int k =0; k<transview.Rows.Count;k++)
                    {
                        totrev = totrev + Convert.ToDecimal(transview.Rows[k]["ncredit"]);
                    }
                    textBox10.Text = totrev.ToString("N2");

                    //for (int i = 0; i < 10; i++)
                    //{
                    //    transview.Rows.Add();
                    //}
                    transGrid.Focus();
                }
                else
                {
                    MessageBox.Show((radioButton3.Checked ? "Member not found or no transactions to reverse" : "Voucher not found or no transactions to reverse" ));
                    textBox1.Text = "";
                }
            }
        }//end of getclientlist

        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            string dProBy = radioButton3.Checked ? "M" : "V";
            string cCode = maskedTextBox1.Text.Trim(); 
            maskedTextBox1.Text = cCode;
            getTrans(dProBy, cCode);
            AllClear2Go();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            acctEnquiry dac = new acctEnquiry(cs, ncompid,dloca);
            dac.ShowDialog();
        }


        private void transGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (transGrid.Columns[e.ColumnIndex].Name == "seltrans")
            {
                if (Convert.ToBoolean(transview.Rows[e.RowIndex]["reverseflag"]))
                {
                    gnSelCount++;
                }
                else
                {
                    gnSelCount--;
                }
//                MessageBox.Show("The selected count is " + gnSelCount);
            }
            AllClear2Go();
        }

        private void transGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            transGrid.CommitEdit(DataGridViewDataErrorContexts.Commit); 
        }
    }
}
