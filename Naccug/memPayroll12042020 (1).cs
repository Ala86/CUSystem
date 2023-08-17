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
using System.IO;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
//using Microsoft.VisualBasic.ex
//using Microsoft.Extensions.Configuration;

namespace WinTcare
{
    public partial class memPayroll : Form
    {
        // DateTime gntrandate = Convert.ToDateTime(dTranDate.Value);
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable clientview = new DataTable();
        DataTable membacctview = new DataTable();
        DataTable Processedclientview = new DataTable();
        DataTable Processedmembacctview = new DataTable();
        DataTable batview = new DataTable();
        DataTable assetview = new DataTable();
        DataTable MembAcctView = new DataTable();
        DataTable membacctview1 = new DataTable();
        DataTable csvDataView = new DataTable();
        DataTable loanView = new DataTable();
        DataTable member2process = new DataTable();
        decimal  gnAccruedInterest = 0.00m;
        int gnUploadExceptions = 0;
        DateTime gdTransactionDate = new DateTime();

        string gcContra = "";
        string gcBatchContra = "";
        string gcControlAcct = string.Empty;

        int gnTempBatchID = 0;
        int gnMemberRecordsCount = 0;
        static int gnPrdID = 0;
        int gnRecordsBalance = 0;
        string gcMembCode = string.Empty;
        string gcEmplID = string.Empty;
        decimal gnContBalance = 0.00m;
        decimal gnDistributed = 0.00m;
        decimal gnMemberContribution = 0.00m;
        decimal invalue = 0.00m;
        decimal outvalue = 0.00m;
        bool glProcessed = false;
        bool glMemberAmended = false;
        bool glMemberExist = false;

              public memPayroll()
        {
            InitializeComponent();
        }

        private void memPayroll_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Member Payroll Management >> " + (glProcessed ? " Processed Members " : " Unprocessed Members");
            acctGrid.Columns["actbala"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["actbala"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            acctGrid.Columns["ncont"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["ncont"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            getBatchList();
            setheaders(true);
            clientgrid.Focus();
            if (clientview != null && clientview.Rows.Count > 0)
            {
                //                gnTempBatchID = Convert.ToInt16(clientview.Rows[clientgrid.CurrentCell.RowIndex]["batch_id"]);
                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gcEmplID = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["cstaffno"]);
                //                string gcPayroll_ID = clientgrid.Rows[0].Cells["col0"].Value.ToString();
                //      getaccounts(gcMembCode);
            }
        }

        private void getclientList()
        {
            string dsql = "exec tsp_getMembersPayroll  " + ncompid;
            clientview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview != null && clientview.Rows.Count > 0)
                {
                    gnRecordsBalance = clientview.Rows.Count;
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "membername";
                    clientgrid.Columns[2].DataPropertyName = "membertype";
                    clientgrid.Columns[3].DataPropertyName = "cacctnumb";
                    clientgrid.Columns[4].DataPropertyName = "datejoin";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 20; i++)
                    {
                        clientview.Rows.Add();
                    }
                }
                AllClear2Go();
            }
        }//end of getclientlist

        private void getBatchList()
        {
            string dsqlb = "select bat_name,bat_id from batch_main order by bat_name  ";
            batview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dab = new SqlDataAdapter(dsqlb, ndConnHandle);
                dab.Fill(batview);
                if (batview.Rows.Count > 0)
                {
                    comboBox1.DataSource = batview.DefaultView;
                    comboBox1.DisplayMember = "bat_name";
                    comboBox1.ValueMember = "bat_id";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }//end of getBatchlist


        private void getaccounts(string tcpayroll, decimal tnTotalContribution)
        {
            glMemberAmended = Convert.ToBoolean(clientgrid.CurrentRow.Cells["colAmend"].Value) ? true : false;
            try
            {
                membacctview1.Clear();
                string dsql = "exec tsp_getMemberAccounts  " + ncompid + ",'" + tcpayroll + "'";
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    da.Fill(membacctview1);
                    if (membacctview1 != null && membacctview1.Rows.Count > 0)
                    {
                        int numberofAccounts = membacctview1.Rows.Count;
                        acctGrid.AutoGenerateColumns = false;
                        acctGrid.DataSource = membacctview1.DefaultView;
                        acctGrid.Columns[0].DataPropertyName = "cacctnumb";
                        acctGrid.Columns[1].DataPropertyName = "acctname";
                        acctGrid.Columns[2].DataPropertyName = "acctype";
                        acctGrid.Columns[3].DataPropertyName = "nbookbal";
                        acctGrid.Columns[4].DataPropertyName = "loanRepay";
                        ndConnHandle.Close();
                        for (int i = 0; i < numberofAccounts; i++)
                        {
                            string tcAcct = membacctview1.Rows[i]["cacctnumb"].ToString().Trim();
                            string tcAcctype = tcAcct.Substring(0, 3);
                            if (glMemberAmended)
                            {
                                acctGrid.Rows[i].Cells["ncont"].Value = getAmendments(tcpayroll, tcAcct);
                            }

                            if (tcAcctype == "130" || tcAcctype == "131")
                            {
                                decimal nLoanRepay = Convert.ToDecimal(membacctview1.Rows[i]["loanRepay"]);
                                getAccruedInterest(tcAcct,i);
                                if (nLoanRepay >= tnTotalContribution)
                                {
                                    if (!glMemberAmended)
                                    {
                                        acctGrid.Rows[i].Cells["ncont"].Value = tnTotalContribution;
                                        tnTotalContribution = 0.00m;
                                    }
                                }
                                else
                                {
                                    tnTotalContribution = tnTotalContribution - Convert.ToDecimal(membacctview1.Rows[i]["loanRepay"]);
                                    if (!glMemberAmended)
                                    {
                                        acctGrid.Rows[i].Cells["ncont"].Value = Convert.ToDecimal(membacctview1.Rows[i]["loanRepay"]);
                                    }
                                }
                            }
                            else
                            {
                                if (!glMemberAmended)
                                {
                                    acctGrid.Rows[i].Cells["ncont"].Value = membacctview1.Rows[i]["saveflag"].ToString().Trim() == "S" ? tnTotalContribution : 0.00m;
                                }
                            }
                        }
                    }
                    else
                    {
                            MessageBox.Show("Something is not right, inform IT DEPT immediately ");
                    }
                }
            }
            catch (SqlException ex)
            {
                //                ex.Errors[0];
            }
        }

        private void getAccruedInterest(string tcAcctNumb,int rownumber)
        {
            string dasql = "exec tsp_MemberLoanAccounts_A " + ncompid + " ,'" + tcAcctNumb + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    DateTime lastInterestDate = Convert.ToDateTime(dprodview.Rows[0]["lintdate"]);
                    decimal nloanBalance = Convert.ToDecimal(dprodview.Rows[0]["nbookbal"]);
                    decimal nloanInterest = Convert.ToDecimal(dprodview.Rows[0]["intrate"]) / (100 * 365m);
                    int numberofdays = (DateTime.Now - lastInterestDate).Days;
                    string cloanType = dprodview.Rows[0]["loantype"].ToString().Trim();
                    decimal accruedInterest = nloanBalance * nloanInterest * numberofdays;
                    gnAccruedInterest = accruedInterest;
                    acctGrid.Rows[rownumber].Cells["naccInt"].Value = Math.Abs(accruedInterest).ToString("N2");
                }
           //     else { MessageBox.Show("No loan details found for selected loan account, inform IT Dept immediately"); }
            }
        }
        private void updateInterestDate(string tcAcctNumb)
        {
            string cglquery = "update glmast set lintdate = @dTransactionDate where cacctnumb = @acctNumb";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nconnHandle);
                updCommand.UpdateCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(dTranDate.Value);
                updCommand.UpdateCommand.Parameters.Add("@acctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                nconnHandle.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nconnHandle.Close();
            }
        }

        private decimal getAmendments(string tcpayroll,string tcAcctNumb)
        {
            decimal contamt = 0.00m;
            string cglquery = "select payroll_id,cacctnumb,ncont,compid from mpayamd_bkp where payroll_id = @tpayrollid and cacctnumb = @tcAcctNumb and  compid = @tcompid";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(cglquery, nconnHandle);
                selCommand.SelectCommand.Parameters.Add("@tpayrollid", SqlDbType.VarChar).Value = tcpayroll;
                selCommand.SelectCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                selCommand.SelectCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;
                DataTable mycontview = new DataTable();
                nconnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                nconnHandle.Close();

                selCommand.Fill(mycontview);
                if (mycontview.Rows.Count > 0)
                {
                    contamt = Convert.ToDecimal(mycontview.Rows[0]["ncont"]);
                    return contamt;
                }
                else
                {
                    return contamt;
                }
            }
        }


        private void getAmendedDistribution(string tcpayroll)
        {
            member2process.Clear();
            decimal contamt = 0.00m;
            string cglquery = "select payroll_id,cacctnumb,ncont,compid from mpayamd_bkp where payroll_id = @tpayrollid and compid = @tcompid";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(cglquery, nconnHandle);
                selCommand.SelectCommand.Parameters.Add("@tpayrollid", SqlDbType.VarChar).Value = tcpayroll;
                selCommand.SelectCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;
                nconnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                nconnHandle.Close();
                selCommand.Fill(member2process);
                if (member2process.Rows.Count > 0)
                {
                    int numberofAccounts = member2process.Rows.Count;
                    gnMemberRecordsCount = numberofAccounts;
                    for (int i = 0; i < numberofAccounts; i++)
                    {
                        string tcAcct = member2process.Rows[i]["cacctnumb"].ToString().Trim();
                        decimal tnCont = Convert.ToDecimal(member2process.Rows[i]["ncont"]);
                        string tcAcctype = tcAcct.Substring(0, 3);
                        acctGrid.Rows[i].Cells["actnumb"].Value = tcAcct;
                        acctGrid.Rows[i].Cells["ncont"].Value = tnCont;
                    }
                }
            }
        }


        private void insAmendments(string tcpayroll, string tcAcctNumb, decimal tncont)
        {
            string cglquery = "insert into mpayamd_bkp (payroll_id,cacctnumb,ncont,compid) values (@tpayrollid,@tcAcctNumb,@tncont,@tcompid)";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nconnHandle);
                insCommand.InsertCommand.Parameters.Add("@tpayrollid", SqlDbType.VarChar).Value = tcpayroll;
                insCommand.InsertCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                insCommand.InsertCommand.Parameters.Add("@tncont", SqlDbType.Decimal).Value = tncont;
                insCommand.InsertCommand.Parameters.Add("@tcompid", SqlDbType.Int).Value = ncompid;
                nconnHandle.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nconnHandle.Close();
            }
        }

        private void getmyAccts(string tcode, string acode)
        {
            string cglquery = "select cacctname,glmast.cacctnumb from glmast,cusreg where cusreg.ccustcode = @lcTcode and acode=@lcAcode and cusreg.ccustcode = glmast.ccustcode";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter selCommand = new SqlDataAdapter();
                selCommand.SelectCommand = new SqlCommand(cglquery, nconnHandle);

                selCommand.SelectCommand.Parameters.Add("@lcTcode", SqlDbType.VarChar).Value = tcode;
                selCommand.SelectCommand.Parameters.Add("@lcAcode", SqlDbType.VarChar).Value = acode;
                DataTable myacctview = new DataTable();
                nconnHandle.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                nconnHandle.Close();

                selCommand.Fill(myacctview);
                if (myacctview.Rows.Count > 0)
                {
                    comboBox2.DataSource = myacctview.DefaultView;
                    comboBox2.DisplayMember = "cacctnumb";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                }
            }
        }

        private void getProcessedClientList()
        {
            string dsql = "exec tsp_getmembersDone  " + ncompid;
            Processedclientview.Clear();        // clientview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(Processedclientview);
                if (Processedclientview.Rows.Count > 0)
                {
                    int pram = Processedclientview.Rows.Count;
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = Processedclientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "fname";
                    clientgrid.Columns[2].DataPropertyName = "mname";
                    clientgrid.Columns[3].DataPropertyName = "lname";
                    clientgrid.Columns[4].DataPropertyName = "cstaffno";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 20; i++)
                    {
                        Processedclientview.Rows.Add();
                    }
                }
            }
        }

        private void getProcessedMembers(int tnProcessType)
        {

            Processedclientview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                if (tnProcessType == 1)  //we will show and process all batches
                {
                    string dsql = "exec tsp_getProcessedMembers  " + ncompid;
                    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    da.Fill(Processedclientview);
                }
                else                   //we will show and process a single batch
                {
                    string dsql = "exec tsp_getProcessedMembers_one  " + ncompid + "," + comboBox1.SelectedValue;
                    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    da.Fill(Processedclientview);
                }
                ndConnHandle.Open();

                if (Processedclientview.Rows.Count > 0)
                {
                    int pram = Processedclientview.Rows.Count;
                    decimal totcont = 0.00m;
                    textBox3.Text = pram.ToString("N0");
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = Processedclientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "empl_no";
                    clientgrid.Columns[1].DataPropertyName = "fname";
                    clientgrid.Columns[2].DataPropertyName = "lname";
                    clientgrid.Columns[3].DataPropertyName = "ncont";
                    for (int k = 0; k < pram; k++)
                    {
                        totcont = totcont + Convert.ToDecimal(Processedclientview.Rows[k]["ncont"]);
                        clientgrid.Rows[k].DefaultCellStyle.BackColor = Color.Green;
                        clientgrid.Rows[k].DefaultCellStyle.ForeColor = Color.White;
                    }
                    textBox4.Text = totcont.ToString("N2");
                    ndConnHandle.Close();
                    clientgrid.Focus();
                }// else { MessageBox.Show("There are no processed member details"); }
            }
        }

        private void getProcessedAccounts(string tcpayroll)
        {
            string dsql = "exec tsp_getProcessedMemberAccounts  " + "'" + tcpayroll + "'";
            Processedmembacctview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(Processedmembacctview);
                if (Processedmembacctview.Rows.Count > 0)
                {
                    acctGrid.AutoGenerateColumns = false;
                    decimal totcont = 0.00m;
                    acctGrid.DataSource = Processedmembacctview.DefaultView;
                    acctGrid.Columns[0].DataPropertyName = "acctnumb";
                    acctGrid.Columns[1].DataPropertyName = "acctname";
                    acctGrid.Columns[2].DataPropertyName = "acctype";
                    acctGrid.Columns[3].DataPropertyName = "nbookbal";
                    acctGrid.Columns[4].DataPropertyName = "loanRepay";
                    acctGrid.Columns[5].DataPropertyName = "ncont";

                    ndConnHandle.Close();

                    for (int k = 0; k < Processedmembacctview.Rows.Count; k++)
                    {
                        totcont = totcont + Convert.ToDecimal(Processedmembacctview.Rows[k]["ncont"]);
                    }
                    textBox1.Text = totcont.ToString("N2"); 
                    gnContBalance = totcont; 
                }else { MessageBox.Show("oooooops, cannot find "); }
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
            if (textBox3.Text != "" && textBox4.Text != "" && comboBox1.SelectedIndex > -1 && comboBox2.SelectedIndex > -1)
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

        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clientgrid.EndEdit();
            //if (glProcessed == false)
            //{
            //    string gcPayroll_ID = clientgrid.CurrentRow.Cells["col0"].Value.ToString();
            //    decimal ntotalCont = 0.00m;
            //    gnDistributed = 0.00m;
            //    ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
            //    gnMemberContribution = ntotalCont;
            //    textBox1.Text = ntotalCont.ToString("N2");
            //    getaccounts(gcPayroll_ID, ntotalCont);
            //}
            //else
            //{
            //    gcMembCode = Convert.ToString(Processedclientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
            //    gcEmplID = Convert.ToString(Processedclientview.Rows[clientgrid.CurrentCell.RowIndex]["empl_no"]);
            //    getProcessedAccounts(gcEmplID);
            //}
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BatchSetup pays = new BatchSetup();
            pays.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            memberAttach ma = new WinTcare.memberAttach();
            ma.ShowDialog();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            gnTempBatchID = Convert.ToInt16(comboBox1.SelectedValue);
            gcContra = comboBox2.SelectedValue.ToString();
            deleteBkpPayroll(gnTempBatchID);
            insertPayroll();
            deletePayAmd(gnTempBatchID);
            membacctview.Clear();
            member2process.Clear();
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            gnDistributed = 0.00m;
            clientview.Clear();
            getclientList();
            clientgrid.Focus();
            if (clientview.Rows.Count > 0)
            {
                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gcEmplID = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["cstaffno"]);
                decimal ntotalCont = 0.00m;
                ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                textBox1.Text = ntotalCont.ToString("N2");
                getaccounts(gcEmplID, ntotalCont);
            }
            textBox2.Text = textBox3.Text = textBox4.Text = "";
            radioButton8.Checked = true;
            radioButton9.Checked = false;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            clientgrid.Rows.Clear();
            progressBar1.Value = 0;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
        }


        private void insertPayroll()
        {
            int linecount = clientgrid.Rows.Count;
            int j = 1;
            foreach (DataGridViewRow clrow in clientgrid.Rows)
            {
                bool tlExist = Convert.ToBoolean(clrow.Cells["actExist"].Value);
                if (tlExist)
                {
                    progressBar1.Value = j * progressBar1.Maximum / linecount;
                    gcEmplID = clrow.Cells["col0"].Value.ToString();
                    bool tlAmended = Convert.ToBoolean(clrow.Cells["colAmend"].Value);
                    decimal ntotalCont = 0.00m;
                    gnPrdID = 0;
                    ntotalCont = Convert.ToDecimal(clrow.Cells["col3"].Value);
                    member2process.Clear();
                    if (tlAmended)                                       //we will get the details from mpayamd_bkp
                    {
                        getAmendedDistribution(gcEmplID);
                    }
                    else                                                //we will do the automatic distribution of the contribution 
                    {
                        getAutoDistribution(gcEmplID, ntotalCont);
                    }

                    if (gnMemberRecordsCount>0) //member2process.Rows.Count > 0)
                    {
                        using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                        {
                            string cpatquery = "insert into mpayroll_bkp (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest) values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt)";
                            string tcAcctNumb = "";
                            string tcProdControl = "";
                            decimal tnContAmt = 0.00m;
                            decimal tnAccruedInterest = 0.00m;
                            ndConnHandle3.Open();
                            SqlDataAdapter tempCommand = new SqlDataAdapter();
                            tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle3);

                            foreach (DataGridViewRow dr in acctGrid.Rows)
                            {
                                bool lready2go = !Convert.IsDBNull(dr.Cells["ncont"].Value) ? (Convert.ToDecimal(dr.Cells["ncont"].Value) > 0.00m ? true : false) : false;
                                if(lready2go)                               
                                {
                                    tcAcctNumb        = !Convert.IsDBNull(dr.Cells["actnumb"].Value) ? dr.Cells["actnumb"].Value.ToString().Trim() : "";
                                    if (tcAcctNumb != "")
                                    {
                                        tcProdControl = getProductControl(tcAcctNumb);
                                    }
                                    tnContAmt = dr.Cells["ncont"].Value != null ? Convert.ToDecimal(dr.Cells["ncont"].Value) : 0.00m;
                                    tnAccruedInterest = dr.Cells["naccInt"].Value != null ? Convert.ToDecimal(dr.Cells["naccInt"].Value) : 0.00m;

//                                    MessageBox.Show("Accrued Interest for account " + tcAcctNumb + " is " + tnAccruedInterest+" and the contribution is "+tnContAmt);

                                    tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnTempBatchID;
                                    tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = gcEmplID;
                                    tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = tcAcctNumb;
                                    tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = tnContAmt;
                                    tempCommand.InsertCommand.Parameters.Add("@ccontra", SqlDbType.VarChar).Value = gcContra;
                                    tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProdControl;
                                    tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = gnPrdID;
                                    tempCommand.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = tnAccruedInterest;

                                    tempCommand.InsertCommand.ExecuteNonQuery();
                                    tempCommand.InsertCommand.Parameters.Clear();
                                } 
                            }
                            ndConnHandle3.Close();
                        }
                    }
                    updateProcessFlag(gcEmplID);
                }
                j++;
            }
            MessageBox.Show("Payroll insert successful");
        }// end of insert payroll

        private static string getProductControl(string tcAcct)
        {
            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string acode = tcAcct.Substring(0, 3);
                string prdctrl = string.Empty;
                string cpatquery = "select prod_control,prd_id from prodtype where prod_type = @prdtype";
                SqlDataAdapter selCommand = new SqlDataAdapter();
                DataTable prodview = new DataTable();
                selCommand.SelectCommand = new SqlCommand(cpatquery, ndConnHandle3);
                selCommand.SelectCommand.Parameters.Add("@prdtype", SqlDbType.Int).Value = acode;
                ndConnHandle3.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
                selCommand.Fill(prodview);
                if (prodview.Rows.Count > 0)
                {
                    prdctrl = prodview.Rows[0]["prod_control"].ToString();
                    gnPrdID = Convert.ToInt16(prodview.Rows[0]["prd_id"]);
                    return prdctrl;
                }
                else
                {
                    return "";
                }
            }
        }

        private void deleteBkpPayroll(int tnBatchNo)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "delete from mpayroll_bkp where batch_id = @nBatchID";
                SqlDataAdapter delCommand = new SqlDataAdapter();
                delCommand.DeleteCommand = new SqlCommand(cpatquery, ndConnHandle3);
                delCommand.DeleteCommand.Parameters.Add("@nBatchID", SqlDbType.Int).Value = tnBatchNo;
                ndConnHandle3.Open();
                delCommand.DeleteCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }

        private void deleteFrmPayroll(int tnBatchNo, string tcActNo)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "delete from mpayroll where batch_id = @nBatchID and acctnumb = @acctnumber";
                SqlDataAdapter delCommand = new SqlDataAdapter();
                delCommand.DeleteCommand = new SqlCommand(cpatquery, ndConnHandle3);
                delCommand.DeleteCommand.Parameters.Add("@nBatchID", SqlDbType.Int).Value = tnBatchNo;
                delCommand.DeleteCommand.Parameters.Add("@acctnumber", SqlDbType.VarChar).Value = tcActNo;
                ndConnHandle3.Open();
                delCommand.DeleteCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }

        private void deletePayAmd(int tnBatchNo)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery1 = "delete from mpayamd_bkp";
                SqlDataAdapter delCommand1 = new SqlDataAdapter();
                delCommand1.DeleteCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                ndConnHandle3.Open();
                delCommand1.DeleteCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }
        private void getAutoDistribution(string tcpayroll, decimal tnTotalContribution)
        {
            try
            {
                gnMemberRecordsCount = 0;
                member2process.Clear();
                string dsql = "exec tsp_getMemberAccounts  " + ncompid + ",'" + tcpayroll + "'";
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                    da.Fill(member2process);
                    ndConnHandle.Close();
                    if (member2process != null && member2process.Rows.Count > 0)
                    {
                        acctGrid.DataSource = member2process.DefaultView;
                        int numberofAccounts = member2process.Rows.Count;
                        gnMemberRecordsCount = numberofAccounts;
                        textBox1.Text = tnTotalContribution.ToString("N2");
                        for (int i = 0; i < gnMemberRecordsCount; i++)
                        {
                            string tcAcct = member2process.Rows[i]["cacctnumb"].ToString().Trim();
                            string tcActname = member2process.Rows[i]["acctname"].ToString().Trim();
                            string tcActype = member2process.Rows[i]["acctype"].ToString().Trim();
                            decimal tnBal = Convert.ToDecimal(member2process.Rows[i]["nbookbal"]);
                            string tcAcctype = tcAcct.Substring(0, 3);
                            if (tcAcctype == "130" || tcAcctype == "131")
                            {
                                decimal nLoanRepay = Convert.ToDecimal(member2process.Rows[i]["loanRepay"]);
                                getAccruedInterest(tcAcct, i);
                                if (nLoanRepay >= tnTotalContribution)
                                {
                                    acctGrid.Rows[i].Cells["actnumb"].Value = tcAcct;
                                    acctGrid.Rows[i].Cells["actname"].Value = tcActname;
                                    acctGrid.Rows[i].Cells["acttype"].Value = tcActype;
                                    acctGrid.Rows[i].Cells["actbala"].Value = tnBal;
                                    acctGrid.Rows[i].Cells["ncont"].Value = tnTotalContribution;
                                    tnTotalContribution = 0.00m;
                                }
                                else
                                {
                                    tnTotalContribution = tnTotalContribution - Convert.ToDecimal(member2process.Rows[i]["loanRepay"]);
                                    acctGrid.Rows[i].Cells["actnumb"].Value = tcAcct;
                                    acctGrid.Rows[i].Cells["actname"].Value = tcActname;
                                    acctGrid.Rows[i].Cells["acttype"].Value = tcActype;
                                    acctGrid.Rows[i].Cells["actbala"].Value = tnBal;
                                    acctGrid.Rows[i].Cells["ncont"].Value = Convert.ToDecimal(member2process.Rows[i]["loanRepay"]);
                                }
                            }
                            else
                            {
                                acctGrid.Rows[i].Cells["actnumb"].Value = tcAcct;
                                acctGrid.Rows[i].Cells["actname"].Value = tcActname;
                                acctGrid.Rows[i].Cells["acttype"].Value = tcActype;
                                acctGrid.Rows[i].Cells["actbala"].Value = tnBal;
                                acctGrid.Rows[i].Cells["ncont"].Value = member2process.Rows[i]["saveflag"].ToString().Trim() == "S" ? tnTotalContribution : 0.00m;
                            }
  //                          MessageBox.Show("We are going for the next account for " + tcpayroll+" and the count is "+i);
                        }
    //                    MessageBox.Show("We are done with member with payroll " + tcpayroll);
                    }
                    else
                    {
                        //    MessageBox.Show("This is the else of the if membacctview1 ");
                    }
                }
            }
            catch (SqlException ex)
            {
                //                ex.Errors[0];
            }
        }



        private void updateProcessFlag(string tcPayRollID)
        {
            using (SqlConnection ndConnHandle31 = new SqlConnection(cs))
            {
                string cpatquery = "update cusreg set lpayrollprocessed = 1 where payroll_id = @cPayrollID";
                ndConnHandle31.Open();
                SqlDataAdapter temp1Command = new SqlDataAdapter();
                temp1Command.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle31);
                temp1Command.UpdateCommand.Parameters.Add("@cPayrollID", SqlDbType.VarChar).Value = tcPayRollID;
                temp1Command.UpdateCommand.ExecuteNonQuery();
                temp1Command.UpdateCommand.Parameters.Clear();
                ndConnHandle31.Close();
            }
        }


        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && Convert.ToDecimal(textBox1.Text) > 0.00m)
            {
                acctGrid.ReadOnly = false;
                gnContBalance = Convert.ToDecimal(textBox1.Text);
                textBox1.Text = gnContBalance.ToString("N2");// + gnContBalance);
            }
            else
            {
                acctGrid.ReadOnly = true;
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            gnTempBatchID = Convert.ToInt16(comboBox1.SelectedValue);
            if (MessageBox.Show("Do you want to confirm and post Batch No. " + gnTempBatchID, "Batch confirmation and Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                gdTransactionDate = Convert.ToDateTime(dTranDate.Value);
                gnTempBatchID = Convert.ToInt16(comboBox1.SelectedValue);
                confirmButton.Enabled = false;
                confirmButton.BackColor = Color.Gainsboro;
                insertMainPayroll();
                postSummaryAccounts();
                clientgrid.Rows.Clear();
                MessageBox.Show("Successfully Confirmed and Posted");
            }
        }


        private void insertMainPayroll()
        {
            string dsql = "exec tsp_getProcessed2Post_one  " + ncompid + "," + comboBox1.SelectedValue;
            string cpatquery = "insert into mpayroll (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id) values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id)";
            string dAcctNumb = "";
            decimal dContAmt = 0.00m;
            decimal dnAccrInt = 0.00m;
            int nprdid = 0;
            string gcContra = globalvar.gcIntSuspense;
            Processedclientview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(Processedclientview);
                if (Processedclientview.Rows.Count > 0)
                {
                    gcBatchContra = Processedclientview.Rows[0]["ccontra"].ToString();

                    SqlDataAdapter tempCommand = new SqlDataAdapter();
                    SqlDataAdapter delpay = new SqlDataAdapter();

                    tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle);
                    int linecount = Processedclientview.Rows.Count;
                    for (int j = 0; j < Processedclientview.Rows.Count; j++)
                    {
                        progressBar1.Value = j * progressBar1.Maximum / linecount;
                        dAcctNumb = Processedclientview.Rows[j]["acctnumb"].ToString();
                        dContAmt = Convert.ToDecimal(Processedclientview.Rows[j]["ncont"]);
                        dnAccrInt = Convert.ToDecimal(Processedclientview.Rows[j]["accrInterest"]);
                        nprdid = Convert.ToInt16(Processedclientview.Rows[j]["prd_id"]);

                        tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = Convert.ToInt16(Processedclientview.Rows[j]["batch_ID"]);
                        tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = Processedclientview.Rows[j]["payroll_id"].ToString();
                        tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = dAcctNumb;
                        tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = dContAmt;
                        tempCommand.InsertCommand.Parameters.Add("@cContra", SqlDbType.VarChar).Value = Processedclientview.Rows[j]["ccontra"].ToString();
                        tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = Processedclientview.Rows[j]["prod_control"].ToString();
                        tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = nprdid;
                        tempCommand.InsertCommand.ExecuteNonQuery();
                        tempCommand.InsertCommand.Parameters.Clear();
                        postMemberAccounts(dAcctNumb, dContAmt,dnAccrInt, gcContra, nprdid);
                        deleteFrmPayroll(gnTempBatchID, dAcctNumb);
                    }
                    ndConnHandle.Close();
                    MessageBox.Show("Main payrol file update successful");
                }
                else { MessageBox.Show("There is nothing to process"); }
            }
        }// end of insertmainpayroll


        private void acctGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (acctGrid.CurrentCell.ColumnIndex == 3)
            {
                acctGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void postMemberAccounts(string tcAcctNumb, decimal lnTranAmt,decimal lnAccrInt, string tccontra, int prid)
        {
            string tcUserid = globalvar.gcUserid;
            string tcDesc = "Payroll Processing " + tcAcctNumb;
            string tcDescInt = "Payroll Interest Paid " + tcAcctNumb;
            string tcDescIntAcc = "Payroll Interest Charged" + tcAcctNumb;
            string dacode = tcAcctNumb.Substring(0, 3);  //we need to identify whether savings, shares or loans
            string tcCustcode = tcAcctNumb.Substring(3, 6);
            decimal tnTranAmt = Math.Abs(lnTranAmt);
            //decimal tnTranAmtAcc = -Math.Abs(lnTranAmt);
            decimal tnContAmt = Math.Abs(lnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(lnTranAmt);
            int npaytype = 1;
            decimal lnWaiveAmt = 0.00m;
            string tcChqno = "";
            string tcTranCode = (dacode == "250" || dacode == "251" || dacode == "270" || dacode == "271" ? "03" : "08");
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            int lnServID = prid;
            bool lFreeBee = false;
        //    DateTime dTranDate = DateTime.Now;

            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            if (tcTranCode == "08")
            {
                getloancontrol(prid, out gcControlAcct);

                decimal tnPrinAmt = Math.Abs(lnTranAmt-lnAccrInt);
                decimal tnPrinCont = -tnPrinAmt;
        //        tcTranCode = "07";
                //we will post the principal amount as  
                gls.updGlmast(cs, tcAcctNumb, tnPrinAmt);                              //update glmast posting account
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tls1.updCuTranhist(cs, tcAcctNumb, tnPrinAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tccontra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                //we will be updating the audit trail. 
                string auditDesc = textBox1.Text.Trim() + " " + tcAcctNumb;// "Loan Disbursement Completed";
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnPrinAmt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                gls.updGlmast(cs, tccontra, tnPrinCont);                                //update glmast contra account
                                                                                        //                updateJournal(tcContra, tcDesc, tnPrinCont, tcVoucher, tcTranCode);
                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tccontra);        // 0.00m;
                tcCustcode = tccontra.Substring(3, 6);
                tls1.updCuTranhist(cs, tccontra, tnPrinCont, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
                //updateAmort();  //we need to update the amortabl so that it does not become part of the outstanding payments

                updateJournal(tccontra, tcDesc, tnPrinCont, tcVoucher, tcTranCode, gdTransactionDate);
                updateJournal(gcControlAcct, tcDesc, tnPrinAmt, tcVoucher, tcTranCode, gdTransactionDate);
                //     updateJournal(tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode);
                //******************************************************************************************************************************************************************
                //we will post the Accrued interest amount as per the income account defined in the product definition 
                tcTranCode = "17";
                decimal tnAccIntCont = -Math.Abs(lnAccrInt);// 0.00m;
                gls.updGlmast(cs, tcAcctNumb, tnAccIntCont);                              //update glmast posting account
                decimal tnPNewBal3 = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tls1.updCuTranhist(cs, tcAcctNumb, tnAccIntCont, tcDescIntAcc, tcVoucher, tcChqno, tcUserid, tnPNewBal3, tcTranCode, lnServID, llPaid, tccontra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account

                //we will be updating the audit trail. 
                string auditDesc2 = tcDescIntAcc + tcAcctNumb;// "Loan Disbursement Completed";
                AuditTrail a21 = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnAccIntCont, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                
                //************************************************************************************************************************************************************
                //we will post the interest amount as per the income account defined in the product definition 
                tcTranCode = "13";
                decimal tnAccruedCont = Math.Abs(lnAccrInt);// 0.00m;
                gls.updGlmast(cs, tcAcctNumb, lnAccrInt);                              //update glmast posting account
                decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                tcCustcode = tcAcctNumb.Substring(3, 6);
                tls1.updCuTranhist(cs, tcAcctNumb, lnAccrInt, tcDescInt, tcVoucher, tcChqno, tcUserid, tnPNewBal1, tcTranCode, lnServID, llPaid, tccontra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account


                 //we will be updating the audit trail. 
                string auditDesc1 = textBox1.Text.Trim() + " " + tcAcctNumb;
                AuditTrail au1 = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, lnAccrInt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);
                updateInterestDate(tcAcctNumb);  //update the last interest calculation date

                gls.updGlmast(cs, tccontra, tnAccruedCont);                                //update glmast contra account
                                                                                           //                updateJournal(tcContra, tcDesc, tnAccruedCont, tcVoucher, tcTranCode);
                decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, tccontra);        // 0.00m;
                tcCustcode = tccontra.Substring(3, 6);
                tls1.updCuTranhist(cs, tccontra, tnAccruedCont, tcDescInt, tcVoucher, tcChqno, tcUserid, tnCNewBal2, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
//                updateAmort();
                updateJournal(tccontra, tcDesc, tnAccruedCont, tcVoucher, tcTranCode, gdTransactionDate);
                updateJournal(gcControlAcct, tcDesc, tnAccruedCont, tcVoucher, tcTranCode, gdTransactionDate);
                //     updateJournal(tcContra, tcDesc, tnContAmt, tcVoucher, tcTranCode);
                updateClient_Code updcl = new updateClient_Code();
                updcl.updClient(cs, "nvoucherno");
                updcl.updClient(cs, "stackcount");
            }
            else
            {
                //we will handle corporate and group payrolls at a later stage
                gcControlAcct = (tcAcctNumb.Substring(0, 3) == "250" || tcAcctNumb.Substring(0, 3) == "251") ? globalvar.gcSaveCtrl_Ind : globalvar.gcShareCtrl_Ind; 





                //            MessageBox.Show("we will post acct, amt, contra " + tcAcctNumb + "," + lnTranAmt + "," + tccontra);
                gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
                                                                                       //   dbal.updDayBal(cs, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tccontra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

                //   updateJournal(gcControlAcct, tcDesc, tnTranAmt, tcVoucher, tcTranCode);
                string auditDesc = "Payroll Process Completed -> " + gcMembCode;
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                gls.updGlmast(cs, tccontra, tnContAmt);                                //update glmast contra account

                decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tccontra);        // 0.00m;
                tls1.updCuTranhist(cs, tccontra, tnContAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                        //update tranhist account 396 1756

                updateJournal(tccontra, tcDesc, tnContAmt, tcVoucher, tcTranCode, gdTransactionDate);
                updateJournal(gcControlAcct, tcDesc, tnContAmt, tcVoucher, tcTranCode, gdTransactionDate);
                updateClient_Code updcl = new updateClient_Code();
                updcl.updClient(cs, "nvoucherno");
                updcl.updClient(cs, "stackcount");
            }
        }
        private void updateJournal(string actnumb, string desc, decimal tamt, string tcvou, string trancode,DateTime dtransDate)
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                int jourtype = (trancode == "03" ? 2 :  //Deposits
                (trancode == "02" ? 3 : //Withdrawals
                (trancode == "08" ? 4 : //Loan Repayment
                (trancode == "09" ? 5 : //"<< Loan Payout/close >>" :
                (trancode == "22" ? 6 : 99)))));// "<< Loan Charge Off >>" : "")))));
                string cglquery = "Insert Into journal (cvoucherno,cuserid,dtrandate,ctrandesc,cacctnumb,dpostdate,cstack,ntranamnt,jvno,bank,cchqno,jcstack,compid,ctrancode,jour_TYPE)";
                cglquery += " VALUES  (@llcVoucherNo,@luserid,@lgdtrans_date,@lgcDesc,@lcActNumb,convert(date,getdate()),@llcStack,@llnTranAmt,@llcjvno,@llcBank,@lgcChqno,@llcStack,@lgnCompID,@lTrancode,@jtype)";
                nConnHandle2.Open();
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@llcVoucherNo", SqlDbType.VarChar).Value = genbill.genvoucher(cs, globalvar.gdSysDate);
                insCommand.InsertCommand.Parameters.Add("@luserid", SqlDbType.Char).Value = globalvar.gcUserid;
                insCommand.InsertCommand.Parameters.Add("@lgdtrans_date", SqlDbType.DateTime).Value = dtransDate;
                insCommand.InsertCommand.Parameters.Add("@lgcDesc", SqlDbType.VarChar).Value = desc;
                insCommand.InsertCommand.Parameters.Add("@lcActNumb", SqlDbType.VarChar).Value = actnumb;
                insCommand.InsertCommand.Parameters.Add("@llcStack", SqlDbType.VarChar).Value = genStack.getstack(cs);
                insCommand.InsertCommand.Parameters.Add("@llnTranAmt", SqlDbType.Decimal).Value = tamt;
                insCommand.InsertCommand.Parameters.Add("@llcjvno", SqlDbType.VarChar).Value = tcvou;
                insCommand.InsertCommand.Parameters.Add("@llcBank", SqlDbType.Int).Value = 1;
                insCommand.InsertCommand.Parameters.Add("@lgcChqno", SqlDbType.VarChar).Value = "000001";
                insCommand.InsertCommand.Parameters.Add("@lgnCompID", SqlDbType.Int).Value = globalvar.gnCompid;
                insCommand.InsertCommand.Parameters.Add("@lTrancode", SqlDbType.Char).Value = trancode;
                insCommand.InsertCommand.Parameters.Add("@jtype", SqlDbType.Int).Value = jourtype;

                insCommand.InsertCommand.ExecuteNonQuery();
                insCommand.InsertCommand.Parameters.Clear();
                nConnHandle2.Close();
//                updateClientCode();
            }
        }
        private void postSummaryAccounts()
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string tcAcctNumb = "";
                decimal tnTranAmt = 0.00m;
                string tcDesc = "";
                string tcDescInt = "";
                int lnServID = 0;
                decimal tnContAmt = 0.00m;
                string tcVoucher = "";
                decimal unitprice = 0.00m;
                string dacode = "";
                decimal lnWaiveAmt = 0.00m;
                string tcChqno = "";
                string tcTranCode = "";
                bool llPaid = false;
                int tnqty = 1;
                string tcReceipt = "";
                bool llCashpay = true;
                int visno = 1;
                bool isproduct = false;
                int srvid = 1;
                string tcCustcode = "";
                bool lFreeBee = false;
                string auditDesc = "";

                string cpatquery = "select sum(ncont) as nconTotal,prod_control,prd_id from mpayroll_bkp where batch_id = @nBatchid group by prd_id, prod_control order by prd_id";
                SqlDataAdapter selCommand = new SqlDataAdapter();
                DataTable prodSummview = new DataTable();
                selCommand.SelectCommand = new SqlCommand(cpatquery, ndConnHandle3);
                selCommand.SelectCommand.Parameters.Add("@nBatchid", SqlDbType.Int).Value = gnTempBatchID;
                ndConnHandle3.Open();
                selCommand.SelectCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
                selCommand.Fill(prodSummview);
                if (prodSummview.Rows.Count > 0)
                {
                    updateGlmast gls = new updateGlmast();
                    updateDailyBalance dbal = new updateDailyBalance();
                    updateCuTranhist tls1 = new updateCuTranhist();
                    AuditTrail au = new AuditTrail();

                    for (int j = 0; j < prodSummview.Rows.Count; j++)
                    {
                        tcAcctNumb = prodSummview.Rows[j]["prod_control"].ToString();
                        tnTranAmt = -Math.Abs(Convert.ToDecimal(prodSummview.Rows[j]["nconTotal"]));
                        lnServID = Convert.ToInt16(prodSummview.Rows[j]["prd_id"]);
                        tcDesc = "Payroll Batch Summary " + tcAcctNumb;
                        tnContAmt = Math.Abs(tnTranAmt);
                        tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
                        unitprice = Math.Abs(tnTranAmt);
                        dacode = tcAcctNumb.Substring(0, 3);
                        lnWaiveAmt = 0.00m;
                        tcCustcode = tcAcctNumb.Substring(3, 6);
                        tcChqno = "";
                        tcTranCode = (dacode == "250" || dacode == "251" || dacode == "270" || dacode == "271" ? "03" : "08");
                        llPaid = false;
                        tnqty = 1;
                        tcReceipt = "";
                        llCashpay = true;
                        visno = 1;
                        isproduct = false;
                        srvid = 1;
                        lFreeBee = false;



                        gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
                                                                                               //   dbal.updDayBal(cs, tcAcctNumb, tnTranAmt, globalvar.gnBranchid, globalvar.gnCompid);
                        decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                        dbal.updDayBal(cs, tcAcctNumb, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
                        tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, globalvar.gcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, gcBatchContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                        srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account

                        auditDesc = "Payroll Process Completed -> " + tcAcctNumb;
                        au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);


                        gls.updGlmast(cs, gcBatchContra, tnContAmt);                                //update glmast contra account
                        dbal.updDayBal(cs, gcBatchContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
                        decimal tnCNewBal = CheckLastBalance.lastbalance(cs, gcBatchContra);        // 0.00m;
                        tls1.updCuTranhist(cs, gcBatchContra, tnContAmt, tcDesc, tcVoucher, tcChqno, globalvar.gcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, visno, isproduct,
                        srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                        //update tranhist account 396 1756

                        auditDesc = "Payroll Process Completed -> " + gcBatchContra;
                        au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                        updateClient_Code updcl = new updateClient_Code();
                        updcl.updClient(cs, "nvoucherno");

                    }
                }
            }
        }


        private void acctGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (acctGrid.Columns[e.ColumnIndex].Name == "ncont" && acctGrid.CurrentRow.Cells["actnumb"].Value.ToString() != "")
            {
                decimal acctCont = Convert.ToDecimal(acctGrid.CurrentRow.Cells["ncont"].Value);
                if (gnMemberContribution >= acctCont)
                {
                    gnMemberContribution = gnMemberContribution - acctCont;
                    acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2"); 
                    gnDistributed = gnDistributed + acctCont;
                }
                else
                {
                    acctCont = gnMemberContribution;
                    gnDistributed = gnDistributed + acctCont;
                    acctGrid.CurrentRow.Cells["ncont"].Value = gnMemberContribution.ToString("N2");
                    gnMemberContribution = 0.00m;
                }
            }
            AllClear2Go();
        }


        private void ProcessOK()
        {
            if (clientgrid.Rows.Count > 0)
            {
                confirmButton.Enabled = true;
                confirmButton.BackColor = Color.LawnGreen;
            }
            else
            {
                confirmButton.Enabled = false;
                confirmButton.BackColor = Color.Gainsboro;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            glProcessed = (button10.Text == "View Processed Members") ? true : false;
            panel1.Visible = glProcessed ? true : false;
            if (glProcessed)
            {
                radioButton8.Enabled = radioButton9.Enabled = button12.Enabled = button1.Enabled = comboBox2.Enabled = button5.Enabled = comboBox1.Enabled = false;
                clientgrid.DataSource = null;
                this.Text = globalvar.cLocalCaption + " << Member Payroll Management >>  Processed Members";
                button10.Text = "View UnProcessed Members";
                clientview.Clear();
                membacctview.Clear();
                Processedclientview.Clear();
                Processedmembacctview.Clear();
                textBox1.Text = "";
                setheaders(true);
                getProcessedMembers(1);  //processed members details are displayed
                clientgrid.Focus();
                if (clientview.Rows.Count > 0)
                {
                    gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                    gcEmplID = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["cstaffno"]);
                    decimal ntotalCont = 0.00m;
                    ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                    textBox1.Text = ntotalCont.ToString("N2");
                    getaccounts(gcEmplID, ntotalCont);
                }
                confirmButton.Enabled = false;
                confirmButton.BackColor = Color.Gainsboro;
            }
            else
            {
                radioButton8.Enabled = radioButton9.Enabled = button12.Enabled = button1.Enabled = comboBox2.Enabled = button5.Enabled = true;
                clientgrid.DataSource = null;
                this.Text = globalvar.cLocalCaption + " << Member Payroll Management >>  unProcessed Members";
                acctGrid.ReadOnly = false;
                gnDistributed = 0.00m;
                button10.Text = "View Processed Members";
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog getfil = new OpenFileDialog();
            getfil.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse CSV Files",

                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xls",
                Filter = "Data Files (*.csv)|*.csv",
                FilterIndex = 1,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.SafeFileName;
                string csvFile = openFileDialog1.FileName;
                decimal ntotalCont = 0.00m;

                //******************* New procedure starts here
                csvDataView.Clear();
                setheaders(true);
                csvDataView = GetDataTabletFromCSVFile(csvFile, 1);
                int columncount = csvDataView.Columns.Count;
                int drowcount = csvDataView.Rows.Count;

                decimal totContribution = 0.00m;
                if (columncount > 0 && columncount <= 4)
                {
                    if (csvDataView != null)
                    {
                        clientgrid.Rows.Clear();
                        clientgrid.Rows.Add(csvDataView.Rows.Count);
                        int i = 0;
                        int rowcount = csvDataView.Rows.Count;
                        textBox3.Text = rowcount.ToString("N0");
                        string cPayrollNumber = string.Empty;           // csvDataView.Rows[j][0].ToString().Trim();
                        string cFirstName = string.Empty;               // csvDataView.Rows[j][1].ToString().Trim();
                        string cLastName = string.Empty;                // csvDataView.Rows[j][2].ToString().Trim();
                        string cContribution = string.Empty;            //Convert.ToDecimal(csvDataView.Rows[j][3]).ToString("N2");//.Trim();
                        bool tlAcctExists = false;                      // checkAccountExists(ncompid, cPayrollNumber, cs);
                        for (int j = 0; j < rowcount; j++)
                        {
                            //if (columncount > 2)  //4 column excel file
                            //{
                            //    cPayrollNumber = csvDataView.Rows[j][0].ToString().Trim();
                            //    cFirstName = csvDataView.Rows[j][1].ToString().Trim();
                            //    cLastName = csvDataView.Rows[j][2].ToString().Trim();
                            //    cContribution = Convert.ToDecimal(csvDataView.Rows[j][3]).ToString("N2");//.Trim();
                            //    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                            //}
                            //else  //2 column excel file
                            //{
                            //    string dsearched = csvDataView.Rows[j][0].ToString().Trim();
                            //    int dslen = dsearched.Trim().Length;
                            //    int dstartindex0 = dsearched.IndexOf(' ');
                            //    int dstartindex1 = dsearched.IndexOf(' ', dsearched.IndexOf(' ') + 1);
                            //    cPayrollNumber = dsearched.Substring(0, dstartindex0).Trim();
                            //    cLastName = dsearched.Substring(dstartindex0, dstartindex1 - dstartindex0).Trim();
                            //    cFirstName = dsearched.Substring(dstartindex1, dslen - dstartindex1).Trim();
                            //    cContribution = Convert.ToDecimal(csvDataView.Rows[j][1]).ToString("N2");
                            //    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                            //}
//                            MessageBox.Show("The Column count is " + columncount);
                            switch(columncount)
                            {
                                case 2:                                                             //2 column spreadsheet
                                    string dsearched = csvDataView.Rows[j][0].ToString().Trim();
                                    int dslen = dsearched.Trim().Length;
                                    int dstartindex0 = dsearched.IndexOf(' ');
                                    int dstartindex1 = dsearched.IndexOf(' ', dsearched.IndexOf(' ') + 1);
                                    cPayrollNumber = dsearched.Substring(0, dstartindex0).Trim();
                                    cLastName = dsearched.Substring(dstartindex0, dstartindex1 - dstartindex0).Trim();
                                    cFirstName = dsearched.Substring(dstartindex1, dslen - dstartindex1).Trim();
                                    cContribution = Convert.ToDecimal(csvDataView.Rows[j][1]).ToString("N2");
                                    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                                    break;

                                case 3:                                                             //3 column spreadsheet;
                                    //MessageBox.Show("3 column spreadsheet");
                                    cPayrollNumber = csvDataView.Rows[j][0].ToString().Trim();
                                    string dsearched1 = csvDataView.Rows[j][1].ToString().Trim();
                                    int dslen1 = dsearched1.Trim().Length;
                                    int dstartindex01 = dsearched1.IndexOf(' ');
                                    cLastName = dsearched1.Substring(0, dstartindex01).Trim();
                                    cFirstName = dsearched1.Substring(dstartindex01, dslen1 - dstartindex01).Trim();
                                    cContribution = Convert.ToDecimal(csvDataView.Rows[j][2]).ToString("N2");
//                                    MessageBox.Show("3 column spreadsheet before exists and checking for  "+cPayrollNumber);
                                    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                                    break;
                                case 4:                                                                 //4 column spreadsheet
                                    cPayrollNumber = csvDataView.Rows[j][0].ToString().Trim();
                                    cFirstName = csvDataView.Rows[j][1].ToString().Trim();
                                    cLastName = csvDataView.Rows[j][2].ToString().Trim();
                                    cContribution = Convert.ToDecimal(csvDataView.Rows[j][3]).ToString("N2");//.Trim();
                                    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                                    break;
                        }

                            clientgrid.Rows[i].Cells["col0"].Value = cPayrollNumber;
                            clientgrid.Rows[i].Cells["col1"].Value = cFirstName;
                            clientgrid.Rows[i].Cells["col2"].Value = cLastName;
                            clientgrid.Rows[i].Cells["col3"].Value = cContribution;
                            clientgrid.Rows[i].Cells["colAmend"].Value = false;
                            clientgrid.Rows[i].Cells["actExist"].Value = tlAcctExists;
                            clientgrid.Rows[i].DefaultCellStyle.BackColor = tlAcctExists ? Color.Green : Color.Red;
                            clientgrid.Rows[i].DefaultCellStyle.ForeColor = Color.White;// tlAcctExists ? Color.White : Color.White;
                            updatePAYROLLID(cFirstName, cLastName, cPayrollNumber);  //Update payroll number if it does not exist in cusreg
                            totContribution = totContribution + Convert.ToDecimal(cContribution);
                            i++;
                        }

                        csvDataView.Rows.Add();
                        string gcPayroll_ID = clientgrid.Rows[0].Cells["col0"].Value.ToString();
                        ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                        gnMemberContribution = ntotalCont;
                        textBox1.Text = ntotalCont.ToString("N2");
//                        MessageBox.Show("before get accounts for payroll number "+gcPayroll_ID+" and total contribution "+ntotalCont);
                        getaccounts(gcPayroll_ID, ntotalCont);
                    }
                }
                else { MessageBox.Show("The uploaded file format is incorrect, inform IT DEPT immediately "); }

                textBox4.Text = totContribution.ToString("N2");

            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void updatePAYROLLID(string tcFname, string tcLname, string tcPayroll)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "select ccustcode from cusreg  where ccustfname = @fname and ccustlname = @lname";
                string cpatquery1 = "update cusreg set payroll_id = @tcPayroll where ccustcode = @custcode";
                ndConnHandle3.Open();
                SqlDataAdapter tempCommand = new SqlDataAdapter();
                tempCommand.SelectCommand = new SqlCommand(cpatquery, ndConnHandle3);
                DataTable clview = new DataTable();
                tempCommand.SelectCommand.Parameters.Add("@fname", SqlDbType.VarChar).Value = tcFname;
                tempCommand.SelectCommand.Parameters.Add("@lname", SqlDbType.VarChar).Value = tcLname;
                tempCommand.SelectCommand.ExecuteNonQuery();
                tempCommand.Fill(clview);
                if (clview.Rows.Count > 0)
                {
                    if (clview.Rows.Count == 1)
                    {
                        string custcode = clview.Rows[0]["ccustcode"].ToString().Trim();
                        SqlDataAdapter tempCommand1 = new SqlDataAdapter();
                        tempCommand1.UpdateCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                        tempCommand1.UpdateCommand.Parameters.Add("@tcPayroll", SqlDbType.VarChar).Value = tcPayroll;
                        tempCommand1.UpdateCommand.Parameters.Add("@custcode", SqlDbType.VarChar).Value = custcode;
                        tempCommand1.UpdateCommand.ExecuteNonQuery();
                    }
                }

                ndConnHandle3.Close();
            }
        }

            private static bool checkAccountExists(int tncompid, string tpayroll, string tcs)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(tcs))
            {
                try
                {
                    ndConnHandle.Open();
                    string actdsql = "exec tsp_getMemberAccounts  @ncompid,@tcPayroll ";
                    SqlDataAdapter tempCommand = new SqlDataAdapter();
                    tempCommand.SelectCommand = new SqlCommand(actdsql, ndConnHandle);
                    DataTable acctview = new DataTable();
                    tempCommand.SelectCommand.Parameters.Add("@ncompid", SqlDbType.Int).Value = tncompid;
                    tempCommand.SelectCommand.Parameters.Add("@tcPayroll", SqlDbType.VarChar).Value = tpayroll;
                    tempCommand.SelectCommand.ExecuteNonQuery();
                    tempCommand.Fill(acctview);
                    ndConnHandle.Close();
                    if (acctview != null && acctview.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch(Exception ex)
                {
//                    MessageBox.Show("There is a problem with Payroll ID = "+tpayroll+ "\nCopy the Message Below and extend to IT DEPT \n"+ex.Message); we will come back to this 
                    return false;   
                }
            }
        }

        private void dobackground()
        {
            for (int i = 0; i < clientview.Rows.Count; i++)
            {
                if (clientview.Rows[i]["lamort"] != null && clientview.Rows[i]["lamort"].ToString() != "")
                {
                    //glAmort = Convert.ToBoolean(clientview.Rows[i]["lamort"]);
                    //clientgrid.Rows[i].DefaultCellStyle.BackColor = glAmort ? Color.Green : Color.Yellow;
                    //clientgrid.Rows[i].DefaultCellStyle.ForeColor = glAmort ? Color.White : Color.Black;
                }
                else
                {
                    clientgrid.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    clientgrid.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void setheaders(bool headtype)
        {
            if (headtype)
            {
                clientgrid.Columns[0].Width = 100;
                clientgrid.Columns[1].Width = 280;
                clientgrid.Columns[2].Width = 280;
                clientgrid.Columns[3].Width = 200;

                clientgrid.Columns[0].HeaderText = "Payroll No.";
                clientgrid.Columns[0].DefaultCellStyle.Format = "C";
                clientgrid.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
                clientgrid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                clientgrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                clientgrid.Columns[1].HeaderText = "First Name";
                clientgrid.Columns[1].DefaultCellStyle.Format = "C";
                clientgrid.Columns[1].SortMode = DataGridViewColumnSortMode.Automatic;
                clientgrid.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                clientgrid.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                clientgrid.Columns[2].HeaderText = "Last Name";
                clientgrid.Columns[2].DefaultCellStyle.Format = "N2";
                clientgrid.Columns[2].SortMode = DataGridViewColumnSortMode.Automatic;
                clientgrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                clientgrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                clientgrid.Columns[3].HeaderText = "Contribution";
                clientgrid.Columns[3].DefaultCellStyle.Format = "N2";
                clientgrid.Columns[3].SortMode = DataGridViewColumnSortMode.Automatic;
                clientgrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                clientgrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else
            {

            }

        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path, int temptype)
        {
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    DataTable csvData = new DataTable();
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();

                    if (csvData.Columns.Count == 0)
                    {
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }
                        int dColCount = csvData.Columns.Count;
                        if (temptype == 1)                                          //column based template
                        {
                            csvData.Columns[0].DataType = typeof(string);       // payroll number
                            csvData.Columns[1].DataType = typeof(string);       // first name 
                            if (dColCount > 2)
                            {
                                csvData.Columns[2].DataType = typeof(string);       // last name;
                                if(dColCount >3)
                                {
                                    csvData.Columns[3].DataType = typeof(decimal);      // contribution
                                }
                            }

                            if (dColCount >= 2 && dColCount <= 4)
                            {
                                //csvData.Columns[0].DataType = typeof(string);       // payroll number
                                //csvData.Columns[1].DataType = typeof(string);       // first name 
                                //csvData.Columns[2].DataType = typeof(string);       // last name;
                                //csvData.Columns[3].DataType = typeof(decimal);      // contribution
                                while (!csvReader.EndOfData)
                                {
                                    string[] fieldData = csvReader.ReadFields();
                                    for (int i = 0; i < fieldData.Length; i++)
                                    {
                                        if (fieldData[i] == "")
                                        {
                                            fieldData[i] = null;
                                        }
                                    }
                                    csvData.Rows.Add(fieldData);
                                }
                                //                                MessageBox.Show("Everything went well and the row count is " + csvData.Rows.Count);
                                //                                csvData = csvSorted(csvData);
                                return csvData;
                            }
                            else
                            {
                                return csvData;
                            }
                        }
                        else
                        {
                            MessageBox.Show("row base4d");
                            return csvData;
                        }
                        //else            //row based template
                        //{
                        //    MessageBox.Show("Does it have anything to do with this");
                        //    if (dColCount > 3)
                        //    {
                        //        return csvData;
                        //    }
                        //    else
                        //    { 
                        //        csvData.Columns[0].DataType = typeof(string);
                        //        csvData.Columns[1].DataType = typeof(string);
                        //        csvData.Columns[2].DataType = typeof(decimal);
                        //        //                          MessageBox.Show("We are just about the check the csv file");
                        //        while (!csvReader.EndOfData)
                        //        {
                        //            string[] fieldData1 = csvReader.ReadFields();
                        //            for (int i = 0; i < fieldData1.Length; i++)
                        //            {
                        //                if (fieldData1[i] == "")
                        //                {
                        //                    fieldData1[i] = null;
                        //                }
                        //            }
                        //            csvData.Rows.Add(fieldData1);
                        //        }
                        //        int datarows = csvData.Rows.Count;// - 1;
                        //        csvData = csvSorted(csvData);
                        //        //                            MessageBox.Show("Number of rows sorted in csvSorted is " + csvData.Rows.Count);
                        //        return csvData;
                        //    }
                        //}
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is a problem with the file from xls, inform IT DEPT immediately \n " + ex);
                return null;
            }
        }

        private void refreshMembers()
        {
            //******************* New procedure starts here
            textBox2.Text = openFileDialog1.SafeFileName;
            string csvFile = openFileDialog1.FileName;
            decimal ntotalCont = 0.00m;
            bool glAcctExist = false;

            //******************* New procedure starts here
            csvDataView.Clear();
            setheaders(true);
            csvDataView = GetDataTabletFromCSVFile(csvFile, 1);
            csvDataView = GetDataTabletFromCSVFile(csvFile, 1);
            int columncount = csvDataView.Columns.Count;
            decimal totContribution = 0.00m;
            if (columncount > 0 && columncount <= 4)
            {
                if (csvDataView != null)
                {
                    clientgrid.Rows.Clear();
                    clientgrid.Rows.Add(csvDataView.Rows.Count);
                    int i = 0;
                    int rowcount = csvDataView.Rows.Count;
                    textBox3.Text = rowcount.ToString("N0");

                    for (int j = 0; j < rowcount; j++)
                    {
                        string cPayrollNumber = csvDataView.Rows[j][0].ToString().Trim();
                        string cFirstName = csvDataView.Rows[j][1].ToString().Trim();
                        string cLastName = csvDataView.Rows[j][2].ToString().Trim();
                        string cContribution = Convert.ToDecimal(csvDataView.Rows[j][3]).ToString("N2");//.Trim();
                        bool tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);

                        clientgrid.Rows[i].Cells["col0"].Value = cPayrollNumber;
                        clientgrid.Rows[i].Cells["col1"].Value = cFirstName;
                        clientgrid.Rows[i].Cells["col2"].Value = cLastName;
                        clientgrid.Rows[i].Cells["col3"].Value = cContribution;
                        clientgrid.Rows[i].Cells["actExist"].Value = tlAcctExists;
                        clientgrid.Rows[i].DefaultCellStyle.BackColor = tlAcctExists ? Color.Green : Color.Red;
                        clientgrid.Rows[i].DefaultCellStyle.ForeColor = Color.White;// tlAcctExists ? Color.White : Color.White;
                        updatePAYROLLID(cFirstName, cLastName, cPayrollNumber);  //Update payroll number if it does not exist in cusreg
                        totContribution = totContribution + Convert.ToDecimal(cContribution);
                        i++;
                    }
                    csvDataView.Rows.Add();
                    string gcPayroll_ID = clientgrid.Rows[0].Cells["col0"].Value.ToString();
                    ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                    textBox1.Text = ntotalCont.ToString("N2");
                    getaccounts(gcPayroll_ID, ntotalCont);
                }
            }
        }

        private static DataTable csvSorted(DataTable tcsvData)
        {
       //     MessageBox.Show(" we have created the data table");
            int totrows = 0;
            int numrows = tcsvData.Rows.Count;

            var dt = new DataTable("sortedtable");

            DataTable csvSortedData = new DataTable();
            csvSortedData.Columns.Add("Payroll_id", typeof(string));
            csvSortedData.Columns.Add("First_name", typeof(string));
            csvSortedData.Columns.Add("Last_name", typeof(string));
            csvSortedData.Columns.Add("Amount", typeof(decimal));

            dt.Columns.Add("Payroll_id", typeof(string));
            dt.Columns.Add("Memb_name", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));

            for (int i = 0; i < numrows; i++)
            {
                int namecount = 0;
                decimal nTotCont = 0.00m;
                string empid = tcsvData.Rows[i][0].ToString();
                string memname0 = tcsvData.Rows[i][1].ToString();
                for (int j = 0; j < numrows; j++)
                {
                    string memname1 = tcsvData.Rows[j][1].ToString();
                    if (memname0 == memname1)
                    {
                        namecount++;
                        nTotCont = nTotCont + Convert.ToDecimal(tcsvData.Rows[j][2]);
                    }
                }
                //                MessageBox.Show("Name " + memname0 +" empl id "+empid+ " has " + namecount + " appearances and the total contribution is "+nTotCont);

                DataRow dr = dt.NewRow();

                dr[0] = empid;
                dr[1] = memname0;
                dr[2] = nTotCont;

                //                dr[3] = "coldata4";
                dt.Rows.Add(dr);
                totrows++;
                for (int k = 0; k < namecount - 1; k++)
                {
                    i++;
                }
            }
            return dt;// csvSortedData;// 0;// namecount;
        }


        public void InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData)
        {
            using (SqlConnection dbConnection = new SqlConnection(cs))
            {
                dbConnection.Open();
                using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
                {
                    s.DestinationTableName = "PAYROLL_DET";
                    foreach (var column in csvFileData.Columns)
                        s.ColumnMappings.Add(column.ToString(), column.ToString());
                    s.WriteToServer(csvFileData);
                }
            }
        }


        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            csvDataView.Clear();
            if (clientgrid.Rows.Count > 0)
            {
                clientgrid.Rows.Clear();
                textBox2.Text = "";
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            //          textBox2.Text = "";
            csvDataView.Clear();
            if (clientgrid.Rows.Count > 0)
            {
                clientgrid.Rows.Clear();
                textBox2.Text = "";
            }
            //            clientgrid.Rows.Clear();
        }



        private void getAssetAccounts()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string dsql0 = "exec tsp_AssetAccounts " + ncompid;
                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle1);
                da0.Fill(assetview);
                if (assetview != null)
                {
                    comboBox2.DataSource = assetview.DefaultView;
                    comboBox2.DisplayMember = "cacctname";
                    comboBox2.ValueMember = "cacctnumb";
                    comboBox2.SelectedIndex = -1;
                }
                else { MessageBox.Show("Could not find Asset Accounts, inform IT Dept immediately"); }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Focused)
            {
                if (!radioButton1.Checked)  //unprocessed member details
                {
                    getAssetAccounts();
                    AllClear2Go();
                }
                else                      //processed member details
                {
                    int batid = Convert.ToInt16(comboBox1.SelectedValue);
                    clientgrid.DataSource = null;
                    radioButton8.Enabled = radioButton9.Enabled = button12.Enabled = button1.Enabled = comboBox2.Enabled = button5.Enabled = false;
                    clientgrid.DataSource = null;
                    this.Text = globalvar.cLocalCaption + " << Member Payroll Management >>  Unprocessed Members";
//                    glProcessed = false;
                    button10.Text = "UnProcessed Members";
                    clientview.Clear();
                    membacctview.Clear();
                    Processedclientview.Clear();
                    Processedmembacctview.Clear();
                    textBox1.Text = "";
                    setheaders(false);
                    getProcessedMembers(2);   
                    clientgrid.Focus();
                    if (clientview.Rows.Count > 0)
                    {
                        gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                        gcEmplID = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["cstaffno"]);
                        decimal ntotalCont = 0.00m;
                        ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                        textBox1.Text = ntotalCont.ToString("N2");
                        getaccounts(gcEmplID, ntotalCont);
                    }
                    ProcessOK();
                }
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.Focused)
            {
                AllClear2Go();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            clientgrid.DataSource = null;
            radioButton8.Enabled = radioButton9.Enabled = button12.Enabled = button1.Enabled = comboBox2.Enabled = button5.Enabled = comboBox1.Enabled = false;
            this.Text = globalvar.cLocalCaption + " << Member Payroll Management >>  Unprocessed Members";
//            glProcessed = false;
            button10.Text = "UnProcessed Members";
            clientview.Clear();
            membacctview.Clear();
            Processedclientview.Clear();
            Processedmembacctview.Clear();
            textBox1.Text = "";
            setheaders(false);
            getProcessedMembers(1);  //processed members details are displayed
            clientgrid.Focus();
            if (clientview.Rows.Count > 0)
            {
                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gcEmplID = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["cstaffno"]);
                decimal ntotalCont = 0.00m;
                ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                textBox1.Text = ntotalCont.ToString("N2");
                getaccounts(gcEmplID, ntotalCont);
            }
            //            ProcessOK();
            confirmButton.Enabled = false;
            confirmButton.BackColor = Color.Gainsboro;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            clientgrid.DataSource = null;
            ProcessOK();
        }

        private void clientgrid_Click(object sender, EventArgs e)
        {
            int clientcount = clientgrid.Rows.Count;
            if(clientcount >0)
            {
                if (!glProcessed)
                {
                    bool glExist = Convert.ToBoolean(clientgrid.CurrentRow.Cells["actExist"].Value);
                    if (!glExist)
                    {
                        string tcPayroll = clientgrid.CurrentRow.Cells["col0"].Value.ToString().Trim();
                        string tcfname = clientgrid.CurrentRow.Cells["col1"].Value.ToString().Trim();
                        string tclname = clientgrid.CurrentRow.Cells["col2"].Value.ToString().Trim();
                        updPayID upd = new updPayID(cs, ncompid, tcfname, tclname, tcPayroll);
                        upd.ShowDialog();
                    }
                    else
                    {
                        string gcPayroll_ID = clientgrid.CurrentRow.Cells["col0"].Value.ToString();
                        decimal ntotalCont = 0.00m;
                        gnDistributed = 0.00m;
                        ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                        gnMemberContribution = ntotalCont;
                        textBox1.Text = ntotalCont.ToString("N2");
                        getaccounts(gcPayroll_ID, ntotalCont);
                    }
                }
                else
                {
                    gcMembCode = Convert.ToString(Processedclientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                    gcEmplID = Convert.ToString(Processedclientview.Rows[clientgrid.CurrentCell.RowIndex]["empl_no"]);
                    getProcessedAccounts(gcEmplID);
                }
            }//else { MessageBox.Show("The client count is zero"); }
        }

        private void acctGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (acctGrid.Columns[e.ColumnIndex].Name == "ncont")
            {
                int? rowIdx = e?.RowIndex;
                int? colIdx = e?.ColumnIndex;
                if (rowIdx.HasValue && colIdx.HasValue)
                {
                    var dgv = (DataGridView)sender;
                    var cell = dgv?.Rows?[rowIdx.Value]?.Cells?[colIdx.Value]?.Value;
                    if (!string.IsNullOrEmpty(cell?.ToString()))
                    {
                        clientgrid.CurrentRow.Cells["colAmend"].Value = true;
                        string tcMemberPayroll = clientgrid.CurrentRow.Cells["col0"].Value.ToString().Trim();
                        string tcMemberAccount = acctGrid.CurrentRow.Cells["actnumb"].Value.ToString().Trim();
                        decimal tnMemberCont   = Convert.ToDecimal(acctGrid.CurrentRow.Cells["ncont"].Value);
                        insAmendments(tcMemberPayroll, tcMemberAccount, tnMemberCont);
                    };
                };
            }
        }

        private void acctGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (acctGrid.Columns[e.ColumnIndex].Name == "ncont")
            {
          //      MessageBox.Show("The status of amended is " + glMemberAmended);
            }
        }

        private void clientgrid_Sorted(object sender, EventArgs e)
        {
        //   MessageBox.Show("sorted even ");
        }


        private string getloancontrol(int lprid, out string dacct)
        {
            string dasql = "select  prod_control,prd_name from  prodtype where prd_id = " + lprid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daloan = new SqlDataAdapter(dasql, ndConnHandle);
                DataTable dprodview = new DataTable();
                daloan.Fill(dprodview);
                if (dprodview.Rows.Count > 0)
                {
                    dacct = dprodview.Rows[0]["prod_control"].ToString();// Convert.ToInt32(loanview.Rows[0]["loan_id"]);
                    return dacct;
                }
                else
                {
                    MessageBox.Show("Product Control not defined, inform IT Dept immediately");
                    dacct = string.Empty;
                    return dacct;
                }
            }
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
