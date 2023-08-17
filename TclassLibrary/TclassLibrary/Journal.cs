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
//using TclassLibrary;

namespace TclassLibrary
{
    public partial class Journal : Form
    {
        /*
                 DateTime gdToday = new DateTime();

        int gnRowCount = 0;
        int gnSelCount = 0;
        string gcRevType = string.Empty;

        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;
        string UserID = string.Empty;
        int nBranchid = 0;
        int nCurrCode = 0;

        public Reversal(string tcCos, int tnCompid, string tcLoca,DateTime tdSysDate,string tcUserID, int tnBranchid,int tnCurrCode)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
            gdToday = tdSysDate;
            UserID = tcUserID;
            nBranchid = tnBranchid;
            nCurrCode = tnCurrCode;
        }
             */
        string cs = string.Empty;
        int ncompid = 0;
        string cLocalCaption = string.Empty;
        DateTime gdToday = new DateTime();
        string UserID = string.Empty;
        int nBranchid = 0;
        int nCurrCode = 0;
        DataTable acctview = new DataTable();
        DataTable jourview = new DataTable();
        DataTable bnkview = new DataTable();
        int nrow;
        bool glBankAcct = false;
        decimal gnTotaldebit = 0.00m;
        decimal gnTotalCredit = 0.00m;

        public Journal(string tccos, int tncompid, string tcLoca, DateTime tdSysDate, string tcUserID, int tnBranchid, int tnCurrCode)
        {
            InitializeComponent();
            cs = tccos;
            ncompid = tncompid;
            cLocalCaption = tcLoca;
            gdToday = tdSysDate;
            UserID = tcUserID;
            nBranchid = tnBranchid;
            nCurrCode = tnCurrCode;
        }

        private void Journal_Load(object sender, EventArgs e)
        {
            this.Text = cLocalCaption + "<< Journal Entries >>";
            string djv = CuVoucher.genJv(cs, gdToday); 
            textBox3.Text = gdToday.ToLongDateString(); 
            maskedTextBox1.Text = djv;
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
                else { MessageBox.Show("internal accounts not fuond"); }
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
                    getBnkStatus(actsel);
                }
            }
            else { return; }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (SqlConnection nConnHandle2 = new SqlConnection(cs))
                {
                    string lTrancode = "08";
                    string jvno = maskedTextBox1.Text;// textBox2.Text;
                    string cglquery = "Insert Into journal (cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode)";
                    cglquery += " VALUES  (@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode)";
                    nConnHandle2.Open();
                    foreach (DataGridViewRow dr in jourgrid.Rows)
                    {
                        if (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m || Convert.ToDecimal(dr.Cells["dcredit"].Value) > 0.00m)
                        {
                            decimal jouramt = (Convert.ToDecimal(dr.Cells["ddebit"].Value) > 0.00m ? -Math.Abs(Convert.ToDecimal(dr.Cells["ddebit"].Value)) : Math.Abs(Convert.ToDecimal(dr.Cells["dcredit"].Value)));
                            SqlDataAdapter insCommand = new SqlDataAdapter();
                            insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                            insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, gdToday);
                            insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = UserID;
                            insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dr.Cells["ddate"].Value;
                            insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = dr.Cells["dtrans"].Value.ToString();
                            insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = dr.Cells["actSelect"].Value.ToString();
                            insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                            insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = jouramt;
                            insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = jvno;
                            insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                            insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                            insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = ncompid;
                            insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = lTrancode;

                            insCommand.InsertCommand.ExecuteNonQuery();
                            insCommand.InsertCommand.Parameters.Clear();
                            updateClientCode();

                            //we will be updating the audit trail. 
                            string auditDesc = "Jouranal Entry " + dr.Cells["actSelect"].Value.ToString();
                            AuditTrail au = new AuditTrail();
                            au.upd_audit_trail(cs, auditDesc, 0.00m, jouramt, UserID, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                        }
                    }
                    scope.Complete();
                    nConnHandle2.Close();
                }

                initvariables();
                updatejv();

               
            }
//            updateClientCode();
        }

        private void initvariables()
        {
            gnTotalCredit = 0.00m;
            gnTotaldebit = 0.00m;
            textBox10.Text = gnTotaldebit .ToString();
            textBox11.Text = gnTotalCredit.ToString();
            jourview.Clear();
            bnkview.Clear();
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

        private void updateClientCode()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                nConnHandle2.Open();
                string cupdatequery1 = "update client_code set  stackcount=stackcount+1";
                string cupdatequery2 = "update client_code set  nvoucherno=nvoucherno+1";

                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                SqlDataAdapter updCommand2 = new SqlDataAdapter();

                updCommand1.UpdateCommand = new SqlCommand(cupdatequery1, nConnHandle2);
                updCommand2.UpdateCommand = new SqlCommand(cupdatequery2, nConnHandle2);

                updCommand1.UpdateCommand.ExecuteNonQuery();
                updCommand2.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }
        private void updatejv()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                nConnHandle2.Open();
                string cupdatequry1 = "update client_code set jv_no=jv_no+1";
                SqlDataAdapter updCommand1 = new SqlDataAdapter();
                updCommand1.UpdateCommand = new SqlCommand(cupdatequry1, nConnHandle2);
                updCommand1.UpdateCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
