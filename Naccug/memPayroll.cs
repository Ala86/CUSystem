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
using RestSharp;
using System.Net;
//using Microsoft.VisualBasic.ex
//using Microsoft.Extensions.Configuration;

namespace WinTcare
{
    public partial class memPayroll : Form
    {
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
//        DataTable csvDataView = new DataTable();
        DataTable loanView = new DataTable();
        DataTable member2process = new DataTable();
        DataView cliDataView = new DataView();
        DataTable payview = new DataTable();
        DataView payDataView = new DataView();

        //   DataTable payview = new DataTable();


        //        DataTable 
        decimal gnTotalAccruedInterest = 0.00m;
        decimal gnInterestBalance = 0.00m;
        decimal gnPrincipalArrears = 0.00m;

        decimal gnContBalance = 0.00m;
        decimal gnDistributed = 0.00m;
        decimal gnMemberContribution = 0.00m;
        decimal gnDistributedBalance = 0.00m;
        decimal gnValueB4 = 0.00m;
        decimal gnValueAft = 0.00m;

        decimal invalue = 0.00m;
        decimal outvalue = 0.00m;
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


        bool glProcessed = false;
        bool glMemberAmended = false;
        bool glMemberExist = false;
        bool glAutomaticProcessing = true;

        public memPayroll()
        {
            InitializeComponent();
        }

        private void memPayroll_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + " << Member Payroll Management >> " + (glProcessed ? " Processed Members " : " Unprocessed Members");
            acctGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            acctGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            acctGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            acctGrid.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            acctGrid.Columns["Column1"].SortMode = DataGridViewColumnSortMode.NotSortable;
            acctGrid.Columns["Column1"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            getBatchList();
            setheaders(true);
            clientgrid.Focus();
            if (clientview != null && clientview.Rows.Count > 0)
            {
                gcMembCode = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                gcEmplID = Convert.ToString(clientview.Rows[clientgrid.CurrentCell.RowIndex]["cstaffno"]);
            }
            payview.Columns.Add("Payroll", typeof(String));
            payview.Columns.Add("fname", typeof(String));
            payview.Columns.Add("lname", typeof(String));
            payview.Columns.Add("cont", typeof(decimal));
            payview.Columns.Add("actExs", typeof(bool));
            payview.Columns.Add("colAmd", typeof(bool));
        }


        // SMS Alert
        private void sms(string dAcctNumb, decimal lnBookBal, decimal dContAmt, string  tcCtel)
        {
            //  MessageBox.Show("You are inside Ok 1");
            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, dAcctNumb);      //  0.00m;
            string tcDesc = "Monthly Payroll";
            decimal balance = lnBookBal;
            //  MessageBox.Show("This is the Balabce " + balance);
            string messages = "" + tcDesc.Trim() + ": GMD" + dContAmt + " to your account at " + globalvar.gcComname + " on the " + dTranDate.Text.Trim() + ". Your new Balance is GMD" + balance + "";
            string PhoneNumber = tcCtel;
            var client = new RestClient("https://alchemytelco.com:9443/api?action=sendmessage&originator=NACCUG&username=naccug&password=n@ccu9&recipient=PhoneNumber&messagetype=SMS:TEXT&messagedata=messages");
            client.Timeout = -1;
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddParameter("messagedata", messages);
            requestPost.AddParameter("recipient", PhoneNumber);
            IRestResponse response = client.Execute(requestPost);
            string rawResponse = response.Content.Trim();
            int nStatusCode = (int)response.StatusCode;
            //   MessageBox.Show("This is STatus Code " + nStatusCode);
            string lcStatusCode = response.StatusDescription.ToString().Trim();
            //  string lcStatusDesc = errorMessages(nStatusCode.ToString().Trim().PadLeft(2, '0'));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //  postFee();
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
            string dsql = string.Empty;
            string dsql0 = string.Empty;
            try
            {
                
               dsql0 = "exec tsp_getMemberAccounts_default " + ncompid + ",'" + tcpayroll + "'";

                membacctview1.Clear();

                if(glAutomaticProcessing)
                {
                    dsql = "exec tsp_getMemberAccounts " +ncompid + ",'" + tcpayroll + "'";
                }
                else
                {
                    // check the condition If glprocess else
                   // MessageBox.Show("This exist");
                    dsql = "exec tsp_getPayTemplate  " +ncompid + ",'" + tcpayroll + "'";
                }

                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    //SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                    //DataTable defaview = new DataTable();
                    //da0.Fill(defaview);
                    //if (defaview.Rows.Count > 0)
                    //{
                        SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                        da.Fill(membacctview1);
                    if (membacctview1 != null && membacctview1.Rows.Count > 0)
                    {
                        int numberofAccounts = membacctview1.Rows.Count;
                        decimal gnDefaultContribution = 0.00m;
                        acctGrid.AutoGenerateColumns = false;
                        acctGrid.DataSource = membacctview1.DefaultView;
                        acctGrid.Columns[0].DataPropertyName = "cacctnumb";
                        acctGrid.Columns[1].DataPropertyName = "acctname";
                        acctGrid.Columns[2].DataPropertyName = "acctype";
                        acctGrid.Columns[3].DataPropertyName = "nbookbal";
                        acctGrid.Columns[4].DataPropertyName = "prinPay";
                        ndConnHandle.Close();
                        for (int i = 0; i < numberofAccounts; i++)
                        {
                            string dstatus = membacctview1.Rows[i]["acctype"].ToString().Trim().ToUpper();
                            dstatus = dstatus.Replace('<', ' ').Replace('>', ' ').Trim();
                            bool tlDefinedAccounts = dstatus != "UNDEFINED" ? true : false; // membacctview1.Rows[i]["cacctnumb"].ToString().Trim();
                            if (tlDefinedAccounts)
                            {
                                if (!glAutomaticProcessing)              //manual processing 
                                {
                                    decimal lnBookBal = Math.Abs(Convert.ToDecimal(membacctview1.Rows[i]["ncont"]));
                                    acctGrid.Rows[i].Cells["ncont"].Value = lnBookBal;// gnDefaultContribution;
                                }
                                else                                  //automatic processing 
                                {
                                    string tcAcct = membacctview1.Rows[i]["cacctnumb"].ToString().Trim();
                                    string tcAcctype = tcAcct.Substring(0, 3);
                                    decimal lnContBalance = 0.00m;
                                    if (glMemberAmended)
                                    {
                                        acctGrid.Rows[i].Cells["ncont"].Value = getAmendments(tcpayroll, tcAcct);
                                    }

                                    if (tcAcctype == "130" || tcAcctype == "131")
                                    {
                                        getAccruedInterest(tcAcct, i, true);
                                        gnPrincipalArrears = getPrincipalArrears(tcAcct, i, ncompid, cs);
                                        decimal lnBookBal = Math.Abs(Convert.ToDecimal(membacctview1.Rows[i]["nbookbal"]));
                                        decimal lnPrinPay = Convert.ToDecimal(membacctview1.Rows[i]["prinPay"]);
                                        //  decimal lnTotal2Pay = Math.Abs(lnPrinPay) + Math.Abs(gnPrincipalArrears) + Math.Abs(gnTotalAccruedInterest);
                                        // decimal lnTotal2Pay = Math.Abs(lnPrinPay) + Math.Abs(gnPrincipalArrears);
                                        decimal lnTotal2Pay = Math.Abs(lnPrinPay) ;
                                        // decimal ala = acctGrid.Rows[i].Cells["ncont"].Value;
                                        //  MessageBox.Show("This is it" + gnT);
                                        if (tnTotalContribution > lnTotal2Pay)                  //total payment is more than total liability
                                        {
                                            if (!glMemberAmended)
                                            {
                                                if (Math.Abs(lnPrinPay) > Math.Abs(lnBookBal)) //principal payment is more than book balance, settle book balance and send balance to savings 
                                                {
                                                 //   acctGrid.Rows[i].Cells["ncont"].Value = lnBookBal;
                                                    acctGrid.Rows[i].Cells["ncont"].Value = lnBookBal;   // tnTotalContribution;
                                                    tnTotalContribution = tnTotalContribution - lnBookBal;
                                                }
                                                else                                          //principal pay is equal to book balance 
                                                {
                                                    acctGrid.Rows[i].Cells["ncont"].Value = lnTotal2Pay;// tnTotalContribution;
                                                    tnTotalContribution = tnTotalContribution - lnTotal2Pay;// 0.00m;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (tnTotalContribution == lnTotal2Pay)
                                            {
                                                if (!glMemberAmended)
                                                {
                                                    acctGrid.Rows[i].Cells["ncont"].Value = lnTotal2Pay;
                                                    tnTotalContribution = 0.00m;
                                                }
                                            }
                                            else
                                            {
                                                if (tnTotalContribution > gnTotalAccruedInterest)           //member payment is more than Total Interest (current interest + accrued interest) butless than total liability
                                                {
                                                    if (!glMemberAmended)
                                                    {
                                                        if (Math.Abs(tnTotalContribution) >= Math.Abs(lnBookBal)) //member payment is more than book balance, settle book balance and send balance to savings 
                                                        {
                                                            acctGrid.Rows[i].Cells["ncont"].Value = lnBookBal + gnTotalAccruedInterest;
                                                            tnTotalContribution = tnTotalContribution - (lnBookBal + gnTotalAccruedInterest);
                                                        }
                                                        else                                                     //member payment is less than book bal, just pay into principal 
                                                        {
                                                            acctGrid.Rows[i].Cells["ncont"].Value = tnTotalContribution;
                                                            tnTotalContribution = 0.00m;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (tnTotalContribution == gnTotalAccruedInterest)     //member payment is equal to Total Interest (current interest + accrued interest) 
                                                    {
                                                        if (!glMemberAmended)
                                                        {
                                                            acctGrid.Rows[i].Cells["ncont"].Value = gnTotalAccruedInterest;
                                                            tnTotalContribution = 0.00m;
                                                        }
                                                    }
                                                    else                                    //member payment is less than total Interest (current interest + accrued interest)  
                                                    {
                                                        if (!glMemberAmended)
                                                        {
                                                            acctGrid.Rows[i].Cells["ncont"].Value = tnTotalContribution;
                                                            tnTotalContribution = 0.00m;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!glMemberAmended)
                                        {
                                            acctGrid.Rows[i].Cells["ncont"].Value = membacctview1.Rows[i]["saveflag"].ToString().Trim() == "S" ? tnTotalContribution : 0.00m;
                                            tnTotalContribution = 0.00m;
                                        }
                                    }
                                }
                            } //tldefinedaccounts
                            else
                            {
                                //acctGrid.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                //acctGrid.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            }
                        }
                    }
                    else
                    {
                        SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndConnHandle);
                        DataTable defaview = new DataTable();
                        da0.Fill(defaview);
                        if (defaview.Rows.Count > 0)
                        {
                            acctGrid.AutoGenerateColumns = false;
                            acctGrid.DataSource = defaview.DefaultView;
                            acctGrid.Columns[0].DataPropertyName = "cacctnumb";
                            acctGrid.Columns[1].DataPropertyName = "acctname";
                            acctGrid.Columns[2].DataPropertyName = "acctype";
                            acctGrid.Columns[3].DataPropertyName = "nbookbal";
                            acctGrid.Columns[4].DataPropertyName = "prinPay";

                            ndConnHandle.Close();
                            foreach(DataGridViewRow dgr in acctGrid.Rows )
                            {
                                decimal lnBookBal = 0.00m;
                               dgr.Cells["ncont"].Value = lnBookBal;
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error is : " + ex.Message);
            }
        }

//        private

        private void updateMemberPayrollBkp(string tcpayroll, decimal tnTotalContribution,bool tlMemberAmended)
        {
           
            try
            {
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    if (radioButton7.Checked)                 //manual processing
                    {
                       

                     //   MessageBox.Show("You are in the Manual process");
                        string dsql = "exec tsp_getMemberManualAccounts " + ncompid + ",'" + tcpayroll + "'";
                        ndConnHandle.Open();
                        SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                        DataTable membacctview2 = new DataTable();
                        da.Fill(membacctview2);
                        if (membacctview2.Rows.Count > 0)
                        {
                            int numberofAccounts = membacctview2.Rows.Count;
                            int tnProductID = 0;
                            string tcProdControl = string.Empty;
                            decimal tnContAmt = 0.00m;
                            decimal lnBookBal = 0.00m;
                            decimal lnPrinPay = 0.00m;
                            decimal lnTotal2Pay = 0.00m;
                            string tcAcct = string.Empty;
                            string tcAcctype = string.Empty;
                            decimal lnContBalance = 0.00m;
                            for (int i = 0; i < numberofAccounts; i++)
                            {
                                tcAcct = membacctview2.Rows[i]["cacctnumb"].ToString().Trim();
                                lnBookBal = Math.Abs(Convert.ToDecimal(membacctview2.Rows[i]["nbookbal"]));
                                lnContBalance = Convert.ToDecimal(membacctview2.Rows[i]["nbookbal"]);
                                tcAcctype = tcAcct.Substring(0, 3);
                                tnProductID = Convert.ToInt16(membacctview2.Rows[i]["prd_id"]);
                                tcProdControl = getProductControl.productControl(cs, tnProductID);
                                string dstatus = membacctview2.Rows[i]["acctype"].ToString().Trim().ToUpper();
                                dstatus = dstatus.Replace('<', ' ').Replace('>', ' ').Trim();
                                bool tlDefinedAccounts = dstatus != "UNDEFINED" ? true : false; // membacctview1.Rows[i]["cacctnumb"].ToString().Trim();
                                tnContAmt = Convert.ToDecimal(membacctview2.Rows[i]["ncont"]);
                                gnTotalAccruedInterest = 0.00m;
                                gnTotalAccruedInterest = Convert.ToDecimal(membacctview2.Rows[i]["accrInterest"]);
                                lnPrinPay = Convert.ToDecimal(membacctview2.Rows[i]["prinPay"]);
                                lnTotal2Pay = Math.Abs(lnPrinPay);

                               // getAccruedInterest(tcAcct, i, false);
                              //   MessageBox.Show("this is Insert " + gnTotalAccruedInterest);

                                string cpatquery = "insert into mpayroll_bkp (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest,prinpay,nbookbal) ";
                                cpatquery += "values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt,@tprinpay,@tnbookbal)";
                                SqlDataAdapter tempCommand = new SqlDataAdapter();
                                tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle);

                                tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnTempBatchID;
                                tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = gcEmplID;
                                tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = tcAcct;
                                tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = tnContAmt;
                                tempCommand.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = Math.Abs(gnTotalAccruedInterest);
                                tempCommand.InsertCommand.Parameters.Add("@tprinpay", SqlDbType.Decimal).Value = lnPrinPay;
                                tempCommand.InsertCommand.Parameters.Add("@ccontra", SqlDbType.VarChar).Value = gcContra;
                                tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProdControl;
                                tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = tnProductID;
                                tempCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnContBalance;
                            //    MessageBox.Show("this is tnContAmt" + tnContAmt);
                                if (tnContAmt > 0.00m)
                                {
                             //       MessageBox.Show("this is tnContAmt"+ tnContAmt);
                                    tempCommand.InsertCommand.ExecuteNonQuery();
                                }
                                tempCommand.InsertCommand.Parameters.Clear();
                                ///////************************************************************************************
                            }
                        }
                    }
                    else
                    {
                        string dsql = "exec tsp_getMemberAccounts  " + ncompid + ",'" + tcpayroll + "'";
                        ndConnHandle.Open();
                        SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                        DataTable membacctview2 = new DataTable();
                        da.Fill(membacctview2);
                        if (membacctview2.Rows.Count > 0) // membacctview2 != null && membacctview2.Rows.Count > 0)
                        {
                            int numberofAccounts = membacctview2.Rows.Count;
                            int tnProductID = 0;
                            string tcProdControl = string.Empty;
                            decimal tnContAmt = 0.00m;
                            decimal lnBookBal = 0.00m;
                            decimal lnPrinPay = 0.00m;
                            decimal lnTotal2Pay = 0.00m;
                            string tcAcct = string.Empty;
                            string tcAcctype = string.Empty;
                            decimal lnContBalance = 0.00m;

                            for (int i = 0; i < numberofAccounts; i++)
                            {
                                tcAcct = membacctview2.Rows[i]["cacctnumb"].ToString().Trim();
                                lnBookBal = Math.Abs(Convert.ToDecimal(membacctview2.Rows[i]["nbookbal"]));
                                lnContBalance = Convert.ToDecimal(membacctview2.Rows[i]["nbookbal"]);
                                tcAcctype = tcAcct.Substring(0, 3);
                                tnProductID = Convert.ToInt16(membacctview2.Rows[i]["prd_id"]);
                                tcProdControl = getProductControl.productControl(cs, tnProductID);
                                string dstatus = membacctview2.Rows[i]["acctype"].ToString().Trim().ToUpper();
                                dstatus = dstatus.Replace('<', ' ').Replace('>', ' ').Trim();
                                bool tlDefinedAccounts = dstatus != "UNDEFINED" ? true : false; // membacctview1.Rows[i]["cacctnumb"].ToString().Trim();
                               // decimal lnPrinPay1 = Convert.ToDecimal(membacctview2.Rows[i]["prinPay"]);
                                
                                if (tlDefinedAccounts)
                                {
                                    if (tlMemberAmended)// glMemberAmended)
                                    {
                                        tnContAmt = getAmendments(tcpayroll, tcAcct);
                                        //  gnTotalAccruedInterest = 0.00m;
                                    }
                                    else
                                    {
                                        if (tcAcctype == "130" || tcAcctype == "131")
                                        {

                                            getAccruedInterest(tcAcct, i, false);

                                            gnPrincipalArrears = getPrincipalArrears(tcAcct, i, ncompid, cs);
                                            lnPrinPay = Convert.ToDecimal(membacctview2.Rows[i]["prinPay"]);
                                            //  lnTotal2Pay = Math.Abs(lnPrinPay) + Math.Abs(gnPrincipalArrears) + Math.Abs(gnTotalAccruedInterest);
                                           // lnTotal2Pay = Math.Abs(lnPrinPay) + Math.Abs(gnPrincipalArrears);
                                            lnTotal2Pay = Math.Abs(lnPrinPay);
                                            if (tnTotalContribution > lnTotal2Pay)                  //total payment is more than total liability
                                            {
                                                if (!glMemberAmended)
                                                {
                                                    if (Math.Abs(lnTotal2Pay) > Math.Abs(lnBookBal)) //principal payment is more than book balance, settle book balance and send balance to savings 
                                                    {
                                                        tnContAmt = lnBookBal;// acctGrid.Rows[i].Cells["ncont"].Value = lnBookBal;                  // tnTotalContribution;
                                                        tnTotalContribution = tnTotalContribution - lnBookBal;
                                                    }
                                                    else                                          //principal pay is equal to book balance 
                                                    {
                                                        tnContAmt = lnTotal2Pay;//                                            acctGrid.Rows[i].Cells["ncont"].Value = lnTotal2Pay;// tnTotalContribution;
                                                        tnTotalContribution = tnTotalContribution - lnTotal2Pay;// 0.00m;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (tnTotalContribution == lnTotal2Pay)
                                                {
                                                    //            //pay interest amount
                                                    //            //pay principal amount (check book bal)
                                                    if (!glMemberAmended)
                                                    {
                                                        tnContAmt = lnTotal2Pay;//                                            acctGrid.Rows[i].Cells["ncont"].Value = lnTotal2Pay;
                                                        tnTotalContribution = 0.00m;
                                                    }
                                                }
                                                else
                                                {
                                                    if (tnTotalContribution > gnTotalAccruedInterest)           //member payment is more than Total Interest (current interest + accrued interest) butless than total liability
                                                    {
                                                        //pay interest 
                                                        //pay balance into principal (Check book bal)
                                                        if (!glMemberAmended)
                                                        {
                                                            if (Math.Abs(tnTotalContribution) >= Math.Abs(lnBookBal)) //member payment is more than book balance, settle book balance and send balance to savings 
                                                            {
                                                                tnContAmt = lnBookBal + gnTotalAccruedInterest;//     acctGrid.Rows[i].Cells["ncont"].Value = lnBookBal + gnTotalAccruedInterest;
                                                                tnTotalContribution = tnTotalContribution - (lnBookBal + gnTotalAccruedInterest);
                                                            }
                                                            else                                        //just pay into the principal
                                                            {
                                                                tnContAmt = tnTotalContribution;   //acctGrid.Rows[i].Cells["ncont"].Value = tnTotalContribution;
                                                                tnTotalContribution = 0.00m;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (tnTotalContribution == gnTotalAccruedInterest)     //member payment is equal to Total Interest (current interest + accrued interest) 
                                                        {
                                                            //pay interest
                                                            if (!glMemberAmended)
                                                            {
                                                                tnContAmt = gnTotalAccruedInterest;//acctGrid.Rows[i].Cells["ncont"].Value = gnTotalAccruedInterest;
                                                                tnTotalContribution = 0.00m;
                                                            }
                                                        }
                                                        else                                    //member payment is less than total Interest (current interest + accrued interest)  
                                                        {
                                                            if (!glMemberAmended)
                                                            {
                                                                tnContAmt = tnTotalContribution;// acctGrid.Rows[i].Cells["ncont"].Value = tnTotalContribution;
                                                                tnTotalContribution = 0.00m;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            ///*********************we will post to mpayroll_bkp
                                            string cpatquery = "insert into mpayroll_bkp (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest,prinpay,nbookbal) ";
                                            cpatquery += "values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt,@tprinpay,@tnbookbal)";
                                            SqlDataAdapter tempCommand = new SqlDataAdapter();
                                            tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle);

                                            tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnTempBatchID;
                                            tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = gcEmplID;
                                            tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = tcAcct;
                                            tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = tnContAmt ;
                                            tempCommand.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = Math.Abs(gnTotalAccruedInterest);
                                            tempCommand.InsertCommand.Parameters.Add("@tprinpay", SqlDbType.Decimal).Value = lnPrinPay;
                                            tempCommand.InsertCommand.Parameters.Add("@ccontra", SqlDbType.VarChar).Value = gcContra;
                                            tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProdControl;
                                            tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = tnProductID;
                                            tempCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnContBalance;
                                            if (tnContAmt > 0.00m)
                                            {
                                                tempCommand.InsertCommand.ExecuteNonQuery();
                                            }
                                            tempCommand.InsertCommand.Parameters.Clear();
                                            ///////************************************************************************************

                                        }
                                        else                            //savings and shares
                                        {
                                            lnPrinPay = 0.00m;
                                            if (!glMemberAmended)
                                            {
                                                tnContAmt = membacctview2.Rows[i]["saveflag"].ToString().Trim() == "S" ? tnTotalContribution : 0.00m;
                                                tnTotalContribution = 0.00m;
                                            }

                                            ///*********************we will post to mpayroll_bkp
                                            string cpatquery = "insert into mpayroll_bkp (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest,prinpay,nbookbal) ";
                                            cpatquery += "values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt,@tprinpay,@tnbookbal)";
                                            SqlDataAdapter tempCommand = new SqlDataAdapter();
                                            tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle);

                                            tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnTempBatchID;
                                            tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = gcEmplID;
                                            tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = tcAcct;
                                            tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = tnContAmt;
                                            tempCommand.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = 0.00;
                                            tempCommand.InsertCommand.Parameters.Add("@tprinpay", SqlDbType.Decimal).Value = lnPrinPay;
                                            tempCommand.InsertCommand.Parameters.Add("@ccontra", SqlDbType.VarChar).Value = gcContra;
                                            tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProdControl;
                                            tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = tnProductID;
                                            tempCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnContBalance;
                                            if (tnContAmt > 0.00m)
                                            {
                                                tempCommand.InsertCommand.ExecuteNonQuery();
                                            }
                                            tempCommand.InsertCommand.Parameters.Clear();
                                            ///////************************************************************************************

                                        }
                                    }
                                    /////*********************we will post to mpayroll_bkp
                                    //string cpatquery = "insert into mpayroll_bkp (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest,prinpay,nbookbal) ";
                                    //cpatquery += "values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt,@tprinpay,@tnbookbal)";
                                    //SqlDataAdapter tempCommand = new SqlDataAdapter();
                                    //tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle);

                                    //tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnTempBatchID;
                                    //tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = gcEmplID;
                                    //tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = tcAcct;
                                    //tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = tnContAmt - Math.Abs(gnTotalAccruedInterest);
                                    //tempCommand.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = Math.Abs(gnTotalAccruedInterest);
                                    //tempCommand.InsertCommand.Parameters.Add("@tprinpay", SqlDbType.Decimal).Value = lnPrinPay;
                                    //tempCommand.InsertCommand.Parameters.Add("@ccontra", SqlDbType.VarChar).Value = gcContra;
                                    //tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProdControl;
                                    //tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = tnProductID;
                                    //tempCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnContBalance;
                                    //if (tnContAmt > 0.00m)
                                    //{
                                    //    tempCommand.InsertCommand.ExecuteNonQuery();
                                    //}
                                    //tempCommand.InsertCommand.Parameters.Clear();
                                    /////////************************************************************************************

                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Something is not right, inform IT DEPT immediately ");
                        }
                        ndConnHandle.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error is : " + ex.Message);
            }
        }

        private void getAccruedInterest(string tcAcctNumb, int rownumber, bool tlRowDisp)
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
                   // MessageBox.Show("This is checking the interest");
                    DateTime lastInterestDate = Convert.ToDateTime(dprodview.Rows[0]["lintdate"]);
                    decimal nAccruedInterest = Convert.ToDecimal(dprodview.Rows[0]["accruedinterest"]); //accrued interest from glmast 
                    decimal nloanBalance = Convert.ToDecimal(dprodview.Rows[0]["nbookbal"]);
                    decimal nloanInterest = Convert.ToDecimal(dprodview.Rows[0]["intrate"]) / (100 * 365m);
                    int numberofdays = (DateTime.Now - lastInterestDate).Days;
                    string cloanType = dprodview.Rows[0]["loantype"].ToString().Trim();
                    decimal calculatedInterest = nloanBalance * nloanInterest * numberofdays; //calculated interest
                    gnTotalAccruedInterest =Math.Abs(calculatedInterest) + Math.Abs(nAccruedInterest);                 //total accrued interest =  calculated interest + balance of accrued interest

                    if(tlRowDisp)
                    {
                        acctGrid.Rows[rownumber].Cells["naccInt"].Value = Math.Abs(gnTotalAccruedInterest).ToString("N2");
                    }
                }
            }
        }

        private static decimal getPrincipalArrears(string tcAcctNumb, int rownumber, int ncompid, string cs)
        {
            decimal nPrincipalArrears = 0.00m;
            string prinsql = "exec tsp_LoanPrincipalPay " + ncompid + " ,'" + tcAcctNumb + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                try
                {
                    ndConnHandle.Open();
                    SqlDataAdapter daprin = new SqlDataAdapter(prinsql, ndConnHandle);
                    DataTable dprinview = new DataTable();
                    daprin.Fill(dprinview);
                    if (dprinview.Rows.Count > 0)
                    {
                        nPrincipalArrears = nPrincipalArrears + Convert.ToDecimal(dprinview.Rows[0]["nprinpmnt"]);
                        return nPrincipalArrears;
                    }
                    else
                    {
                        nPrincipalArrears = 0.00M;
                        return nPrincipalArrears;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error is : " + ex.Message);
                    return 0.00m;
                }
            }
        }

        private void updateInterestDate(string tcAcctNumb)
        {
            string cglquery = "update glmast set lintdate = @dTransactionDate where cacctnumb = @acctNumb";
            using (SqlConnection nconnHandle = new SqlConnection(cs))
            {
                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, nconnHandle);
                updCommand.UpdateCommand.Parameters.Add("@dTransactionDate", SqlDbType.DateTime).Value = Convert.ToDateTime(gdTransactionDate);
                updCommand.UpdateCommand.Parameters.Add("@acctNumb", SqlDbType.VarChar).Value = tcAcctNumb;
                nconnHandle.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                nconnHandle.Close();
            }
        }

        private decimal getAmendments(string tcpayroll, string tcAcctNumb)
        {
            decimal contamt = 0.00m;
            decimal accrinte = 0.00m;
            decimal nprinpal = 0.00m;
            string cglquery = "select ncont,accrInterest, prinpay from mpayamd_bkp where payroll_id = @tpayrollid and cacctnumb = @tcAcctNumb and  compid = @tcompid";
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
                    accrinte = Convert.ToDecimal(mycontview.Rows[0]["accrInterest"]);
                    nprinpal = Convert.ToDecimal(mycontview.Rows[0]["prinpay"]);
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
                }
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
                } else { MessageBox.Show("oooooops, cannot find "); }
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
            insertBkpPayroll();
          //  deletePayAmd(gnTempBatchID);
            membacctview.Clear();
            member2process.Clear();
            clientgrid.ClearSelection();
         
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            gnDistributed = 0.00m;
            clientview.Clear();
         
          //  getclientList();
         //   MessageBox.Show("This is the end 1");
            clientgrid.Focus();
          //  MessageBox.Show("This is the end");
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
            clientgrid.DataSource = null;// .Rows.Clear();
            progressBar1.Value = 0;
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
          //  MessageBox.Show("This is the end");
        }


        private void insertBkpPayroll()
        {
            int linecount = clientgrid.Rows.Count;
            int j = 1;
            foreach (DataGridViewRow clrow in clientgrid.Rows)
            {
                int dinx = clrow.Index;
                string memcode = Convert.ToString(clrow.Cells[0].Value);
                string dacct = Convert.ToString(clrow.Cells["actExist"].Value);
                bool tlExist = !Convert.IsDBNull(clrow.Cells["actExist"].Value) ? Convert.ToBoolean(clrow.Cells["actExist"].Value) : false;
                if (tlExist)
                {
                    progressBar1.Value = j * progressBar1.Maximum / linecount;
                    gcEmplID = Convert.ToString(clrow.Cells["col0"].Value) != "" ? Convert.ToString(clrow.Cells["col0"].Value) : "";
                    bool tlAmended =  Convert.ToString(clrow.Cells["colAmend"].Value) != "" ? Convert.ToBoolean(clrow.Cells["colAmend"].Value) : false;
                    decimal ntotalCont = 0.00m;
                    gnPrdID = 0;
                    ntotalCont = Convert.ToString(clrow.Cells["col3"].Value) != "" ?  Convert.ToDecimal(clrow.Cells["col3"].Value) :  0.00m;
                    member2process.Clear();
                    if(gcEmplID != "")
                    {
                        //MessageBox.Show("gcEmplID " + gcEmplID);
                        //MessageBox.Show("ntotalCont " + ntotalCont);
                        //MessageBox.Show("tlAmended " + tlAmended);
                        updateMemberPayrollBkp(gcEmplID, ntotalCont, tlAmended);
                        updateProcessFlag(gcEmplID);
                    }
                    else
                    {
                        MessageBox.Show("The payroll number is empty or invalid and the row number is "+clrow.Index);
                    }
                }
                j++;
            }
            MessageBox.Show("Payroll insert successful");
        }// end of insert payroll


        private void deleteBkpPayroll(int tnBatchNo)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {

              //  MessageBox.Show("This is the batch no2 " + tnBatchNo);
                ndConnHandle3.Open();
                string cpatquery = "delete from mpayroll_bkp where batch_id = @nBatchID";
             //   string cpatquery1 = "delete from mpayamd_bkp where batch_id = @nBatchID";
                string cpatquery2 = "delete from mpayroll where batch_id = @nBatchID";

                SqlDataAdapter delCommand = new SqlDataAdapter();
                SqlDataAdapter delCommand1 = new SqlDataAdapter();
                SqlDataAdapter delCommand2 = new SqlDataAdapter();

                delCommand.DeleteCommand = new SqlCommand(cpatquery, ndConnHandle3);
                delCommand.DeleteCommand.Parameters.Add("@nBatchID", SqlDbType.Int).Value = tnBatchNo;
                delCommand.DeleteCommand.ExecuteNonQuery();

               // delCommand1.DeleteCommand = new SqlCommand(cpatquery1, ndConnHandle3);
               // delCommand1.DeleteCommand.Parameters.Add("@nBatchID", SqlDbType.Int).Value = tnBatchNo;
               // delCommand1.DeleteCommand.ExecuteNonQuery();

                delCommand2.DeleteCommand = new SqlCommand(cpatquery2, ndConnHandle3);
                delCommand2.DeleteCommand.Parameters.Add("@nBatchID", SqlDbType.Int).Value = tnBatchNo;
                delCommand2.DeleteCommand.ExecuteNonQuery();

                ndConnHandle3.Close();
            }
        }

        private void deleteBkpPayrollOne(string tcPayrollNo)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                ndConnHandle3.Open();
                string cpatquery1 = "delete from mpayamd_bkp where payroll_id  = @tcPay";

                SqlDataAdapter delCommand1 = new SqlDataAdapter();

                delCommand1.DeleteCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                delCommand1.DeleteCommand.Parameters.Add("@tcPay", SqlDbType.VarChar).Value = tcPayrollNo;
                delCommand1.DeleteCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }

        //private void deleteFrmPayroll(int tnBatchNo, string tcActNo)
        //{
        //    using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
        //    {
        //        string cpatquery = "delete from mpayroll_bkp where batch_id = @nBatchID and acctnumb = @acctnumber";
        //        SqlDataAdapter delCommand = new SqlDataAdapter();
        //        delCommand.DeleteCommand = new SqlCommand(cpatquery, ndConnHandle3);
        //        delCommand.DeleteCommand.Parameters.Add("@nBatchID", SqlDbType.Int).Value = tnBatchNo;
        //        delCommand.DeleteCommand.Parameters.Add("@acctnumber", SqlDbType.VarChar).Value = tcActNo;
        //        ndConnHandle3.Open();
        //        delCommand.DeleteCommand.ExecuteNonQuery();
        //        ndConnHandle3.Close();
        //    }
        //}

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
                                getAccruedInterest(tcAcct, i,true);
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
                        }
                    }
                    else
                    {
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("There is a problem with member accounts for Payroll number " + tcpayroll + " , inform IT DEPT immediately \n " + ex);
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
                textBox1.Text = gnContBalance.ToString("N2");
            }
            else
            {
                acctGrid.ReadOnly = true;
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            gnTempBatchID = Convert.ToInt16(comboBox1.SelectedIndex) == -1 ? 0 : Convert.ToInt16(comboBox1.SelectedValue);
            if (gnTempBatchID > 0)
            {
                if (MessageBox.Show("Do you want to confirm and post Batch No. " + gnTempBatchID, "Batch confirmation and Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    gdTransactionDate = Convert.ToDateTime(dTranDate.Value);
                    confirmButton.Enabled = false;
                    confirmButton.BackColor = Color.Gainsboro;
                    insertMainPayroll();
                    postSummaryAccounts();
                    clientview.Clear();
                    Processedclientview.Clear();
                    MessageBox.Show("Payroll Successfully Confirmed and Posted");
                }
            }
        }


        private void insertMainPayroll()
        {
            string dsql = "exec tsp_getProcessed2Post_one  " + ncompid + "," + comboBox1.SelectedValue;

            string cpatquery = "insert into mpayroll (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest,prinpay,nbookbal,lbatprocessed,lbatposted,dpostdate,ctel) ";
            cpatquery += "values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt,@tprinpay,@tnbookbal,1,1,getdate(),@ctel)";

            string cpatquery1 = "insert into mpayroll_temp (batch_ID,empl_no,acctnumb,ncont,ccontra,prod_control,prd_id,accrInterest,prinpay,nbookbal,lbatprocessed,lbatposted,dpostdate,ctel) ";
            cpatquery1 += "values (@lbatch_ID,@lempl_no,@lacctnumb,@lncont,@ccontra,@prdControl,@prd_id,@accrInt,@tprinpay,@tnbookbal,1,1,getdate(),@ctel)";

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
                    SqlDataAdapter paytemplate = new SqlDataAdapter();
                    SqlDataAdapter delpay = new SqlDataAdapter();

                    tempCommand.InsertCommand = new SqlCommand(cpatquery, ndConnHandle);
                    paytemplate.InsertCommand = new SqlCommand(cpatquery1, ndConnHandle);

                    int linecount = Processedclientview.Rows.Count;
                    decimal lnBookBal = 0.00m;
                    decimal lnPrinPay = 0.00m;
                    int gnBatchID = Convert.ToInt16(Processedclientview.Rows[0]["batch_ID"]);
                    string tcBatchContra = Processedclientview.Rows[0]["ccontra"].ToString();
                    string tcProductControl = Processedclientview.Rows[0]["prod_control"].ToString();
                    string tcCtel = Processedclientview.Rows[0]["ctel"].ToString();
                    for (int j = 0; j < Processedclientview.Rows.Count; j++)
                    {
                        progressBar1.Value = j * progressBar1.Maximum / linecount;
                        dAcctNumb = Processedclientview.Rows[j]["acctnumb"].ToString();
                        dContAmt = Convert.ToDecimal(Processedclientview.Rows[j]["ncont"]);
                        dnAccrInt = Convert.ToDecimal(Processedclientview.Rows[j]["accrInterest"]);
                        nprdid = Convert.ToInt16(Processedclientview.Rows[j]["prd_id"]);
                        lnBookBal = Convert.ToDecimal(Processedclientview.Rows[j]["nbookbal"]);
                        lnPrinPay = Convert.ToDecimal(Processedclientview.Rows[j]["prinpay"]);
                        string tcPayrollID = Processedclientview.Rows[j]["payroll_id"].ToString().Trim();

                        paytemplate.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnBatchID;
                        paytemplate.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = tcPayrollID;
                        paytemplate.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = dAcctNumb;
                        paytemplate.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = dContAmt;
                        paytemplate.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = dnAccrInt;
                        paytemplate.InsertCommand.Parameters.Add("@cContra", SqlDbType.VarChar).Value = tcBatchContra;
                        paytemplate.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProductControl;
                        paytemplate.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = nprdid;
                        paytemplate.InsertCommand.Parameters.Add("@tprinpay", SqlDbType.Decimal).Value = lnPrinPay;
                        paytemplate.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnBookBal;

                        tempCommand.InsertCommand.Parameters.Add("@lbatch_ID", SqlDbType.Int).Value = gnBatchID;
                        tempCommand.InsertCommand.Parameters.Add("@lempl_no", SqlDbType.VarChar).Value = tcPayrollID;
                        tempCommand.InsertCommand.Parameters.Add("@lacctnumb", SqlDbType.Char).Value = dAcctNumb;
                        tempCommand.InsertCommand.Parameters.Add("@lncont", SqlDbType.Decimal).Value = dContAmt;
                        tempCommand.InsertCommand.Parameters.Add("@accrInt", SqlDbType.Decimal).Value = dnAccrInt; 
                        tempCommand.InsertCommand.Parameters.Add("@cContra", SqlDbType.VarChar).Value = tcBatchContra;
                        tempCommand.InsertCommand.Parameters.Add("@prdControl", SqlDbType.VarChar).Value = tcProductControl;
                        tempCommand.InsertCommand.Parameters.Add("@prd_id", SqlDbType.Int).Value = nprdid;
                        tempCommand.InsertCommand.Parameters.Add("@tprinpay", SqlDbType.Decimal).Value = lnPrinPay;
                        tempCommand.InsertCommand.Parameters.Add("@tnbookbal", SqlDbType.Decimal).Value = lnBookBal;  //tcCtel
                        tempCommand.InsertCommand.Parameters.Add("@ctel", SqlDbType.VarChar).Value = tcCtel;  //tcCtel

                        if (dContAmt > 0.00m)
                        {
                            tempCommand.InsertCommand.ExecuteNonQuery();
                            postMemberAccounts(dAcctNumb, dContAmt, dnAccrInt, lnBookBal, lnPrinPay, tcBatchContra, nprdid);
                            if(radioButton7.Checked )                   //will insert into the template table for future use
                            {
                                paytemplate.InsertCommand.ExecuteNonQuery();
                                paytemplate.InsertCommand.Parameters.Clear();
                            }
                        }
                     //   sms(dAcctNumb, lnBookBal, dContAmt, tcCtel);
                        tempCommand.InsertCommand.Parameters.Clear();
                    }
                    ndConnHandle.Close();
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

        private void postMemberAccounts(string tcAcctNumb, decimal tnMemberPay, decimal tnAccruedInterest, decimal tnBookBal, decimal tnPrinPay, string tccontra, int prid)
        {
            string dacode = tcAcctNumb.Substring(0, 3).ToString().Trim();  //we need to identify whether savings, shares or loans
            string tcTranType = (dacode == "250" || dacode == "251" || dacode == "270" || dacode == "271" ? "01" : "07");
            string tcTranCode = string.Empty;

             // gcControlAcct = getProductControl.productControl(cs, tnPrdid);
            //  gcIntAcct = getProductControl.interestControl(cs, tnPrdid);

            updateGlmast gls = new updateGlmast();
          //  updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            string auditDesc = string.Empty;
            decimal tnTotalContribution = tnMemberPay;
            decimal lnTotal2Pay = Math.Abs(tnPrinPay);
            //decimal lnTotal2Pay = Math.Abs(tnPrinPay) + Math.Abs(tnAccruedInterest);
            //   decimal ala = tnTotalContribution - lnTotal2Pay;

            if (tcTranType == "07")            //loan payment 
            {
                if (tnTotalContribution >= lnTotal2Pay)                  //total payment is more than or equal to total liability (principal payment + accrued interest)
                {
                   
                    tcTranCode = "13";
                    postIntoAccounts(1, false, tcAcctNumb, tccontra, tnAccruedInterest, prid, tcTranCode);
                    decimal principalAmt = Math.Abs(tnMemberPay) - Math.Abs(tnAccruedInterest); 
                    //pay principal amount  
                    tcTranCode = "08";
                    postIntoAccounts(1, true, tcAcctNumb, tccontra, principalAmt, prid, tcTranCode);
                }
                else                                          //total payment is less than total liability(principal payment + accrued interest) 
                {
                    if (tnTotalContribution > tnAccruedInterest)           //member payment is more than Total Interest (current interest + accrued interest) but less than total liability
                    {
                        //pay interest 

                        tcTranCode = "13";
                        postIntoAccounts(1, false, tcAcctNumb, tccontra, tnAccruedInterest, prid, tcTranCode);
                        decimal tnContributionBalance = Math.Abs(tnMemberPay) - Math.Abs(tnAccruedInterest);
                        //pay balance into principal payment
                        tcTranCode = "08";
                        postIntoAccounts(1, true, tcAcctNumb, tccontra, tnContributionBalance, prid, tcTranCode);
                       //     tnAccruedInterest + " and principal of " + tnContributionBalance);
                    }
                    else                                                //member payment is equal to or less than the  total interest
                    {
                        if (tnTotalContribution == tnAccruedInterest) //member payment is equal to total interest 
                        {
                            //pay interest 
                            tcTranCode = "13";
                            postIntoAccountsINT(1, false, tcAcctNumb, tccontra, tnAccruedInterest, prid, tcTranCode);
//                                tnAccruedInterest);
                        }
                        else                                          //member payment is less than total interest, pay part of interest and update accrued interest field in glmast.dbf
                        {
                            //pay part interest 
                            tcTranCode = "13";
                            postIntoAccountsINT(1, false, tcAcctNumb, tccontra, tnTotalContribution, prid, tcTranCode);
                            decimal lnAccrBal = Math.Abs(tnAccruedInterest) - Math.Abs(tnTotalContribution);
                            //update accrued interest in glmast.dbf
                            updateAccruedInterest(tcAcctNumb, lnAccrBal);
                         //       tnTotalContribution + " and update accrued interest in glmast with " + (Math.Abs(tnAccruedInterest) - Math.Abs(tnTotalContribution)));
                        }

                    }
                }
            }
            else                        //savings or shares 
            {
                tcTranCode = "03";
                postIntoAccounts(2, true, tcAcctNumb, tccontra, tnTotalContribution, prid, tcTranCode);
            }
        }

        private void updateAccruedInterest(string tcAct,decimal tnAccruedInterest)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery1 = "update glmast set AccruedInterest = @tnAccruedBalance where cacctnumb  = @tcAcctNum";
                ndConnHandle3.Open();
                SqlDataAdapter tempCommand1 = new SqlDataAdapter();
                tempCommand1.UpdateCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                tempCommand1.UpdateCommand.Parameters.Add("@tcAcctNum", SqlDbType.VarChar).Value = tcAct;
                tempCommand1.UpdateCommand.Parameters.Add("@tnAccruedBalance", SqlDbType.Decimal).Value = tnAccruedInterest;
                tempCommand1.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
            }
        }

        private void postIntoAccounts(int tnType, bool tlPrinInt, string tcAcct, string tcContra, decimal tnPostAmt, int tnprid,  string tcTranCode)
        {
            string tcUserid = globalvar.gcUserid;
            string tcDesc = "Payroll Processing " + tcAcct;
            //string dacode = tcAcctNumb.Substring(0, 3).ToString().Trim();// tcDacode;// tcAcctNumb.Substring(0, 3);  //we need to identify whether savings, shares or loans
            string tcCustcode = tcAcct.Substring(3, 6);
            decimal tnTranAmt = Math.Abs(tnPostAmt);
            decimal tnContraAmt = Math.Abs(tnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(tnPostAmt);
            decimal lnWaiveAmt = 0.00m;
            string tcChqno = "";
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
          //  int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            int lnServID = tnprid;
            bool lFreeBee = false;
            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            string auditDesc = string.Empty;

            switch (tnType)
            {
                case 1:
                    //loan repayment posting 
               //if(tnPostAmt > )
                    if (tlPrinInt) //post principal amount 
                    {
                        //we will post the principal amount as  
                        //                        decimal tnPrinAmt = Math.Abs(tnPostAmt);
                      //  decimal prinpam = tnPostAmt - Math.Abs(gnTotalAccruedInterest);
                        decimal prinpam = tnPostAmt;
                        gls.updGlmast(cs, tcAcct, tnPostAmt);                              //update glmast posting account - credit
                        decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcct);       
                        tcCustcode = tcAcct.Substring(3, 6);
                        tls1.updCuTranhist(cs, tcAcct, prinpam, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                        srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                                                                                                                                                                                  //we will be updating the audit trail. 
                        auditDesc = "Loan Principal Repayment " + tcAcct; 
                        au.upd_audit_trail(cs, auditDesc, 0.00m, prinpam, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                      //  gls.updGlmast(cs, tcContra, tnContraAmt);                                //update glmast contra account - debit
                        decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);         
                        tcCustcode = tcContra.Substring(3, 6);
                       // tls1.updCuTranhist(cs, tcContra, tnContraAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                       // srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
                      //  upj.updJournal(cs, tcContra, tcDesc, tnContraAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                      //  upj.updJournal(cs, gcControlAcct, tcDesc, prinpam, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                    }
                    else          //post interest amount 
                    {
                        //************************************************************************************************************************************************************
                        //we will post the interest amount as per the income account defined in the product definition 
                        if (tnPostAmt > 0)
                        {
                            tcTranCode = "17";
                            tcDesc = "Payroll Processing - Interest Charged ";
                            decimal tnAccruedPost = -Math.Abs(tnPostAmt); 
                            gls.updGlmast(cs, tcAcct, tnAccruedPost);                              //update accrued interest or interest charged - debit
                            decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, tcAcct);
                            tcCustcode = tcAcct.Substring(3, 6);
                            tls1.updCuTranhist(cs, tcAcct, tnAccruedPost, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal0, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                            srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                            auditDesc = "Accrued Interest Charged " + tcAcct; //Audit Trail
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnAccruedPost, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            tcTranCode = "13";
                            tcDesc = "Payroll Processing - Interest Paid ";
                            decimal tnAccruedCont = Math.Abs(tnAccruedPost);
                            gls.updGlmast(cs, tcAcct, tnAccruedCont);                              ////update accrued interest paid or interest paid - Credit
                            decimal tnPNewBal2 = CheckLastBalance.lastbalance(cs, tcAcct);
                            tcCustcode = tcAcct.Substring(3, 6);
                            tls1.updCuTranhist(cs, tcAcct, tnAccruedCont, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal2, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                            srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                            auditDesc = "Accrued Interest Paid " + tcAcct;   //Audit Trail
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnAccruedCont, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            updateInterestDate(tcAcct);  //update the last interest calculation date

                           // gls.updGlmast(cs, tcContra, tnAccruedPost);                                //update contra account - Debit 
                            decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, tcContra);
                            tcCustcode = tcContra.Substring(3, 6);
                          //  tls1.updCuTranhist(cs, tcContra, tnAccruedPost, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal2, tcTranCode, lnServID, llPaid, tcAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                          //  srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
                          //  upj.updJournal(cs, tcContra, tcDesc, tnAccruedPost, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                          //  upj.updJournal(cs, gcControlAcct, tcDesc, tnAccruedPost, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                            updateClient_Code updcl = new updateClient_Code();
                            updcl.updClient(cs, "nvoucherno");
                            updcl.updClient(cs, "stackcount");
                        }
                    }
                    break;
                case 2:                                                //savings and shares payment posting 
                    //we will post the principal amount as  
                    gls.updGlmast(cs, tcAcct, tnPostAmt);                              //update glmast posting account
                    decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, tcAcct);
                    tcCustcode = tcAcct.Substring(3, 6);
                    tls1.updCuTranhist(cs, tcAcct, tnPostAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal1, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                    srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                                                                                                                                                                              //we will be updating the audit trail. 
                    auditDesc = "Payroll Savings " + tcAcct;
                    au.upd_audit_trail(cs, auditDesc, 0.00m, tnPostAmt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                   // gls.updGlmast(cs, tcContra, tnContraAmt);                                //update glmast contra account
                    decimal tnCNewBal1 = CheckLastBalance.lastbalance(cs, tcContra);
                    tcCustcode = tcContra.Substring(3, 6);
                   // tls1.updCuTranhist(cs, tcContra, tnContraAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal1, tcTranCode, lnServID, llPaid, tcAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                   // srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
                   // upj.updJournal(cs, tcContra, tcDesc, tnContraAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                   // upj.updJournal(cs, gcControlAcct, tcDesc, tnPostAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                    break;
            }
        }

        /// Pay only interest 

        private void postIntoAccountsINT(int tnType, bool tlPrinInt, string tcAcct, string tcContra, decimal tnPostAmt, int tnprid, string tcTranCode)
        {
            string tcUserid = globalvar.gcUserid;
            string tcDesc = "Payroll Processing " + tcAcct;
            //string dacode = tcAcctNumb.Substring(0, 3).ToString().Trim();// tcDacode;// tcAcctNumb.Substring(0, 3);  //we need to identify whether savings, shares or loans
            string tcCustcode = tcAcct.Substring(3, 6);
            decimal tnTranAmt = Math.Abs(tnPostAmt);
            decimal tnContraAmt = Math.Abs(tnTranAmt);
            string tcVoucher = CuVoucher.genCuVoucher(cs, globalvar.gdSysDate);
            decimal unitprice = Math.Abs(tnPostAmt);
            decimal lnWaiveAmt = 0.00m;
            string tcChqno = "";
            bool llPaid = false;
            int tnqty = 1;
            string tcReceipt = "";
            bool llCashpay = true;
            //  int visno = 1;
            bool isproduct = false;
            int srvid = 1;
            int lnServID = tnprid;
            bool lFreeBee = false;
            updateGlmast gls = new updateGlmast();
            updateDailyBalance dbal = new updateDailyBalance();
            updateCuTranhist tls1 = new updateCuTranhist();
            AuditTrail au = new AuditTrail();
            updateJournal upj = new updateJournal();
            string auditDesc = string.Empty;

            //switch (tnType)
            //{
            //    case 1:
            //        //loan repayment posting 
            //        //if(tnPostAmt > )
            //        if (tlPrinInt) //post principal amount 
            //        {
            //            //we will post the principal amount as  
            //            //                        decimal tnPrinAmt = Math.Abs(tnPostAmt);
            //            //  decimal prinpam = tnPostAmt - Math.Abs(gnTotalAccruedInterest);
            //            decimal prinpam = tnPostAmt;
            //            gls.updGlmast(cs, tcAcct, tnPostAmt);                              //update glmast posting account - credit
            //            decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcct);
            //            tcCustcode = tcAcct.Substring(3, 6);
            //            tls1.updCuTranhist(cs, tcAcct, prinpam, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
            //            srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
            //                                                                                                                                                                      //we will be updating the audit trail. 
            //            auditDesc = "Loan Principal Repayment " + tcAcct;
            //            au.upd_audit_trail(cs, auditDesc, 0.00m, prinpam, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            //            //  gls.updGlmast(cs, tcContra, tnContraAmt);                                //update glmast contra account - debit
            //            decimal tnCNewBal = CheckLastBalance.lastbalance(cs, tcContra);
            //            tcCustcode = tcContra.Substring(3, 6);
            //            // tls1.updCuTranhist(cs, tcContra, tnContraAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
            //            // srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
            //            //  upj.updJournal(cs, tcContra, tcDesc, tnContraAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
            //            //  upj.updJournal(cs, gcControlAcct, tcDesc, prinpam, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
            //        }
            //        else          //post interest amount 
            //        {
                        //************************************************************************************************************************************************************
                        //we will post the interest amount as per the income account defined in the product definition 
                        //if (tnPostAmt > 0)
                        //{
                            tcTranCode = "17";
                            tcDesc = "Payroll Processing - Interest Charged ";
                            decimal tnAccruedPost = -Math.Abs(tnPostAmt);
                            gls.updGlmast(cs, tcAcct, tnAccruedPost);                              //update accrued interest or interest charged - debit
                            decimal tnPNewBal0 = CheckLastBalance.lastbalance(cs, tcAcct);
                            tcCustcode = tcAcct.Substring(3, 6);
                            tls1.updCuTranhist(cs, tcAcct, tnAccruedPost, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal0, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                            srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                            auditDesc = "Accrued Interest Charged " + tcAcct; //Audit Trail
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnAccruedPost, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            tcTranCode = "13";
                            tcDesc = "Payroll Processing - Interest Paid ";
                            decimal tnAccruedCont = Math.Abs(tnPostAmt);
                            gls.updGlmast(cs, tcAcct, tnAccruedCont);                              ////update accrued interest paid or interest paid - Credit
                            decimal tnPNewBal2 = CheckLastBalance.lastbalance(cs, tcAcct);
                            tcCustcode = tcAcct.Substring(3, 6);
                            tls1.updCuTranhist(cs, tcAcct, tnAccruedCont, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal2, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                            srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
                            auditDesc = "Accrued Interest Paid " + tcAcct;   //Audit Trail
                            au.upd_audit_trail(cs, auditDesc, 0.00m, tnAccruedCont, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                            updateInterestDate(tcAcct);  //update the last interest calculation date

                            // gls.updGlmast(cs, tcContra, tnAccruedPost);                                //update contra account - Debit 
                            decimal tnCNewBal2 = CheckLastBalance.lastbalance(cs, tcContra);
                            tcCustcode = tcContra.Substring(3, 6);
                            //  tls1.updCuTranhist(cs, tcContra, tnAccruedPost, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal2, tcTranCode, lnServID, llPaid, tcAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                            //  srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
                            //  upj.updJournal(cs, tcContra, tcDesc, tnAccruedPost, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                            //  upj.updJournal(cs, gcControlAcct, tcDesc, tnAccruedPost, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                            updateClient_Code updcl = new updateClient_Code();
                            updcl.updClient(cs, "nvoucherno");
                            updcl.updClient(cs, "stackcount");
                        //}
            //        }
            //        break;
            //    case 2:                                                //savings and shares payment posting 
            //        //we will post the principal amount as  
            //        gls.updGlmast(cs, tcAcct, tnPostAmt);                              //update glmast posting account
            //        decimal tnPNewBal1 = CheckLastBalance.lastbalance(cs, tcAcct);
            //        tcCustcode = tcAcct.Substring(3, 6);
            //        tls1.updCuTranhist(cs, tcAcct, tnPostAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnPNewBal1, tcTranCode, lnServID, llPaid, tcContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
            //        srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                          //update tranhist posting account
            //                                                                                                                                                                  //we will be updating the audit trail. 
            //        auditDesc = "Payroll Savings " + tcAcct;
            //        au.upd_audit_trail(cs, auditDesc, 0.00m, tnPostAmt, globalvar.gcUserid, "C", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

            //        // gls.updGlmast(cs, tcContra, tnContraAmt);                                //update glmast contra account
            //        decimal tnCNewBal1 = CheckLastBalance.lastbalance(cs, tcContra);
            //        tcCustcode = tcContra.Substring(3, 6);
            //        // tls1.updCuTranhist(cs, tcContra, tnContraAmt, tcDesc, tcVoucher, tcChqno, tcUserid, tnCNewBal1, tcTranCode, lnServID, llPaid, tcAcct, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
            //        // srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, gdTransactionDate, gdTransactionDate);                        //update tranhist account 
            //        // upj.updJournal(cs, tcContra, tcDesc, tnContraAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
            //        // upj.updJournal(cs, gcControlAcct, tcDesc, tnPostAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
            //        break;
            //}
        }

        private void postSummaryAccounts()
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string tcAcctNumb = "";
                decimal tnTranAmt = 0.00m;
                string tcDesc = "";
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
               // int visno = 1;
                bool isproduct = false;
                int srvid = 1;
                string tcCustcode = "";
                bool lFreeBee = false;
                string auditDesc = "";

                string cpatquery = "select sum(ncont) as nconTotal,prod_control,prd_id from mpayroll where batch_id = @nBatchid group by prd_id, prod_control order by prd_id";
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
                    updateJournal upj = new updateJournal();
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
                        tcTranCode = (dacode == "250" || dacode == "251" || dacode == "270" || dacode == "271" ? "01" : "07");
                        llPaid = false;
                        tnqty = 1;
                        tcReceipt = "";
                        llCashpay = true;
                      //  visno = 1;
                        isproduct = false;
                        srvid = 1;
                        lFreeBee = false;

                        gls.updGlmast(cs, tcAcctNumb, tnTranAmt);                              //update glmast posting account
                        decimal tnPNewBal = CheckLastBalance.lastbalance(cs, tcAcctNumb);      //  0.00m;
                       // dbal.updDayBal(cs, dTranDate.Value,tcAcctNumb, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
                        tls1.updCuTranhist(cs, tcAcctNumb, tnTranAmt, tcDesc, tcVoucher, tcChqno, globalvar.gcUserid, tnPNewBal, tcTranCode, lnServID, llPaid, gcBatchContra, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                        srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                          //update tranhist posting account
                        auditDesc = "Payroll Process Completed -> " + tcAcctNumb;
                        au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                      //  gls.updGlmast(cs, gcBatchContra, tnContAmt);                                //update glmast contra account
                      //  dbal.updDayBal(cs, dTranDate.Value, gcBatchContra, tnContAmt, globalvar.gnBranchid, globalvar.gnCompid);
                        decimal tnCNewBal = CheckLastBalance.lastbalance(cs, gcBatchContra);        // 0.00m;
                       // tls1.updCuTranhist(cs, gcBatchContra, tnContAmt, tcDesc, tcVoucher, tcChqno, globalvar.gcUserid, tnCNewBal, tcTranCode, lnServID, llPaid, tcAcctNumb, lnWaiveAmt, tnqty, unitprice, tcReceipt, llCashpay, gnTempBatchID, isproduct,
                       // srvid, "", "", lFreeBee, tcCustcode, ncompid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gdSysDate, globalvar.gdSysDate);                        //update tranhist account 396 1756
                        auditDesc = "Payroll Process Completed -> " + gcBatchContra;
                        au.upd_audit_trail(cs, auditDesc, 0.00m, tnTranAmt, globalvar.gcUserid, "U", "", "", "", "", globalvar.gcWorkStation, globalvar.gcWinUser);

                        upj.updJournal(cs, tcAcctNumb, tcDesc,  tnContAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                        upj.updJournal(cs, gcBatchContra, tcDesc, tnTranAmt, tcVoucher, tcTranCode, gdTransactionDate, globalvar.gcUserid, globalvar.gnCompid);
                       
                        updateClient_Code updcl = new updateClient_Code();
                        updcl.updClient(cs, "nvoucherno");
                    }
                }
            }
        }


        private void acctGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
           // MessageBox.Show("This is in1");
            if (acctGrid.Columns[e.ColumnIndex].Name == "ncont" && acctGrid.CurrentRow.Cells["actnumb"].Value.ToString() != "")
            {
             //   MessageBox.Show("This is in 2");
                decimal lnTotalPaid = 0.00m;
                decimal lnTotalCont = Convert.ToDecimal(textBox1.Text);
                decimal lnBalance = 0.00m;

                decimal ntotalCont = !Convert.IsDBNull(clientgrid.CurrentRow.Cells["col3"].Value) ? Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value) : 0.00M;
                gnMemberContribution = ntotalCont;
                gnDistributedBalance = ntotalCont;


                clientgrid.CurrentRow.Cells["colAmend"].Value = true;
                string lcAcctNumb = acctGrid.CurrentRow.Cells["actnumb"].Value.ToString().Trim();
                decimal acctCont = Convert.ToDecimal(acctGrid.CurrentRow.Cells["ncont"].Value);
                decimal accrint = Convert.ToDecimal(acctGrid.CurrentRow.Cells["naccInt"].Value);   //naPrinPay
               // decimal nprinpal1 = Convert.ToDecimal(acctGrid.CurrentRow.Cells["naPrinPay"].Value);
                  decimal nprinpal = Convert.ToDecimal(acctGrid.CurrentRow.Cells["Column1"].Value);
                if (glAutomaticProcessing)                    //automatic processing 
                {
                    if (gnDistributedBalance >= acctCont)
                    {
                        gnMemberContribution = gnMemberContribution - acctCont;
                        acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2");
                        gnDistributed = gnDistributed + acctCont;
                        gnDistributedBalance = gnDistributedBalance - acctCont;
                        if (acctCont > 0.00m)
                        {
                            updateMemAmendBkp(lcAcctNumb, acctCont, accrint, nprinpal);
                        }
                    }
                    else
                    {
                        acctCont = gnMemberContribution;
                        gnDistributed = gnDistributed + acctCont;
                        acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2");
                        gnMemberContribution = 0.00m;
                        updateMemAmendBkp(lcAcctNumb, acctCont, accrint, nprinpal);
                    }
                }
                else                                      //manual processing 
                {
                    if (gnDistributedBalance > 0.00m)
                    {
                       // MessageBox.Show("This is in 0");
                        if (acctCont > 0.00m)
                        {
                          //  MessageBox.Show("This is in 1");
                            if (gnDistributedBalance >= acctCont)
                            {
                              
                                acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2");
                              
                                getAccruedInterest(lcAcctNumb, Convert.ToInt16(acctGrid.CurrentRow.Index), true);

                                //gnPrincipalArrears = getPrincipalArrears(tcAcct, i, ncompid, cs);
                                //decimal lnBookBal = Math.Abs(Convert.ToDecimal(membacctview1.Rows[i]["nbookbal"]));
                                //decimal lnPrinPay = Convert.ToDecimal(membacctview1.Rows[i]["prinPay"]);
                                //decimal lnTotal2Pay = Math.Abs(lnPrinPay) + Math.Abs(gnPrincipalArrears) + Math.Abs(gnTotalAccruedInterest);
                               // nprinpal = acctCont - Math.Abs(gnTotalAccruedInterest);
                               // MessageBox.Show("This is in 1");
                              //  acctGrid.CurrentRow.Cells["Column1"].Value = nprinpal.ToString("N2");
                               // acctGrid.CurrentRow.Cells["naccInt"].Value = gnTotalAccruedInterest.ToString("N2");
                              
                                updateMemAmendBkp(lcAcctNumb, acctCont, gnTotalAccruedInterest, nprinpal);
                               // MessageBox.Show("This is the 2");
                                gnDistributed = gnDistributed + acctCont;
                                //gnDistributedBalance = gnDistributedBalance - acctCont;
                                gnDistributedBalance = acctCont - nprinpal;
                            }
                            else
                            {
                              //  MessageBox.Show("This is in 1");
                                acctGrid.CurrentRow.Cells["ncont"].Value = gnDistributedBalance;
                                //                              updateMemAmendBkp(lcAcctNumb, gnDistributedBalance, accrint, nprinpal);
                                //                            gnDistributedBalance = 0.00m;// gnDistributedBalance - acctCont;
                            }
                        }
                    }
                    //else
                    //{
                    //}
                }

                textBox5.Text = Convert.ToDecimal(gnDistributedBalance).ToString("N2");
            }
            AllClear2Go();


            //          if (acctGrid.Columns[e.ColumnIndex].Name == "ncont" && acctGrid.CurrentRow.Cells["actnumb"].Value.ToString() != "")
            //          {
            //              decimal lnTotalPaid = 0.00m;
            //              decimal lnTotalCont = Convert.ToDecimal(textBox1.Text);
            //              decimal lnBalance = 0.00m;

            //              clientgrid.CurrentRow.Cells["colAmend"].Value = true;
            //              string lcAcctNumb = acctGrid.CurrentRow.Cells["actnumb"].Value.ToString().Trim();
            //              decimal acctCont = Convert.ToDecimal(acctGrid.CurrentRow.Cells["ncont"].Value);
            //              decimal accrint = Convert.ToDecimal(acctGrid.CurrentRow.Cells["naccInt"].Value);
            //              decimal nprinpal = Convert.ToDecimal(acctGrid.CurrentRow.Cells["Column1"].Value);
            //              if (glAutomaticProcessing)
            //              {
            //                  if (gnDistributedBalance >= acctCont)               
            //                  {
            //                      gnMemberContribution = gnMemberContribution - acctCont;
            //                      acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2");
            //                      gnDistributed = gnDistributed + acctCont;
            //                      gnDistributedBalance = gnDistributedBalance - acctCont;
            //                      if (acctCont > 0.00m)
            //                      {
            //                          updateMemAmendBkp(lcAcctNumb, acctCont, accrint, nprinpal);
            //                      }
            //                  }
            //                  else
            //                  {
            //                      acctCont = gnMemberContribution;
            //                      gnDistributed = gnDistributed + acctCont;
            //                      acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2"); 
            //                      gnMemberContribution = 0.00m;
            //                      updateMemAmendBkp(lcAcctNumb, acctCont, accrint, nprinpal);
            //                  }
            //              }
            //              else
            //              {
            //                  if (gnDistributedBalance > 0.00m)
            //                  {
            //                      if (acctCont > 0.00m)
            //                      {
            //                          if (acctCont <= gnDistributedBalance)
            //                          {
            //                              acctGrid.CurrentRow.Cells["ncont"].Value = acctCont.ToString("N2");
            //                              updateMemAmendBkp(lcAcctNumb, acctCont, accrint, nprinpal);
            //                              gnDistributed = gnDistributed + acctCont;
            //                              gnDistributedBalance = gnDistributedBalance - acctCont;
            //                          }
            //                          else
            //                          {
            //                              acctGrid.CurrentRow.Cells["ncont"].Value = gnDistributedBalance;
            ////                              updateMemAmendBkp(lcAcctNumb, gnDistributedBalance, accrint, nprinpal);
            //  //                            gnDistributedBalance = 0.00m;// gnDistributedBalance - acctCont;
            //                          }
            //                      }
            //                  }
            //                  else
            //                  {
            //                  }
            //              }
            //              //foreach (DataGridViewRow dgr in acctGrid.Rows)
            //              //{
            //              //    lnTotalPaid = lnTotalPaid + Convert.ToDecimal(dgr.Cells["ncont"].Value);
            //              //}
            //              //lnBalance = (lnTotalCont - lnTotalPaid) > 0.00m ? (lnTotalCont - lnTotalPaid) : 0.00m;
            //              textBox5.Text = Convert.ToDecimal(gnDistributedBalance).ToString("N2");
            //          }
            //          AllClear2Go();
        }

        private void updateMemAmendBkp(string tcAcct,decimal tnCont,decimal tcacctint,decimal tcnprinpal)
        {
            using (SqlConnection ndcon = new SqlConnection(cs))
            {
              //  MessageBox.Show("You are inside Manual");
                string dsql0 = "select 1 from mpayamd_bkp where cacctnumb = @AcctNumb";
                string dsql1 = "update mpayamd_bkp set ncont = @tnCont where payroll_id = @payid and cacctnumb = @AcctNumb";
                string dsql2 = "insert mpayamd_bkp (payroll_id,cacctnumb,ncont,compid,accrInterest,prinpay, batch_id) values (@payid,@AcctNumb,@tnCont,@tncompid,@accrInterest,@prinpay,@batch_id)";
                string dsql3 = "update glmast set payroll_id = @payid where ccustcode = substring(@AcctNumb,4,6)";
                string mpayID = Convert.ToString(clientgrid.CurrentRow.Cells["col0"].Value);
                int mBatID = Convert.ToInt16(comboBox1.SelectedValue);
              //  string ala = Subst
                ndcon.Open();

                //**********Check whether account is already in mpayamd_bkp 
                SqlDataAdapter mpSelect = new SqlDataAdapter();
                mpSelect.SelectCommand = new SqlCommand(dsql0, ndcon);
                mpSelect.SelectCommand.Parameters.Add("@AcctNumb", SqlDbType.Char).Value = tcAcct;
              //  mpSelect.SelectCommand.Parameters.Add("@batch_id", SqlDbType.Char).Value = mBatID;
                mpSelect.SelectCommand.ExecuteNonQuery();
                DataTable mpView0 = new DataTable();
                mpSelect.Fill(mpView0);
                if (mpView0.Rows.Count > 0)    //account exists, will update 
                {
                    SqlDataAdapter mpUpdate = new SqlDataAdapter();
                    mpUpdate.UpdateCommand = new SqlCommand(dsql1, ndcon);
                    mpUpdate.UpdateCommand.Parameters.Add("@payid", SqlDbType.Char).Value = mpayID;
                    mpUpdate.UpdateCommand.Parameters.Add("@AcctNumb", SqlDbType.Char).Value = tcAcct;
                    mpUpdate.UpdateCommand.Parameters.Add("@tnCont", SqlDbType.Decimal).Value = tnCont;
                    mpUpdate.UpdateCommand.Parameters.Add("@accrInterest", SqlDbType.Decimal).Value = tcacctint;
                    mpUpdate.UpdateCommand.Parameters.Add("@prinpay", SqlDbType.Decimal).Value = tcnprinpal;
                    mpUpdate.UpdateCommand.Parameters.Add("@batch_id", SqlDbType.Char).Value = mBatID;
                    mpUpdate.UpdateCommand.ExecuteNonQuery();
                }
                else                         //account does not exist, will insert
                {
                    SqlDataAdapter mpInsert = new SqlDataAdapter();
                    mpInsert.InsertCommand = new SqlCommand(dsql2, ndcon);
                    mpInsert.InsertCommand.Parameters.Add("@payid", SqlDbType.Char).Value = mpayID;
                    mpInsert.InsertCommand.Parameters.Add("@AcctNumb", SqlDbType.Char).Value = tcAcct;
                    mpInsert.InsertCommand.Parameters.Add("@tnCont", SqlDbType.Decimal).Value = tnCont;
                    mpInsert.InsertCommand.Parameters.Add("@tncompid", SqlDbType.Decimal).Value = ncompid;
                    mpInsert.InsertCommand.Parameters.Add("@accrInterest", SqlDbType.Decimal).Value = tcacctint;
                    mpInsert.InsertCommand.Parameters.Add("@prinpay", SqlDbType.Decimal).Value = tcnprinpal;
                    mpInsert.InsertCommand.Parameters.Add("@batch_id", SqlDbType.Char).Value = mBatID;
                    mpInsert.InsertCommand.ExecuteNonQuery();


                    SqlDataAdapter glUpdate = new SqlDataAdapter();
                    glUpdate.UpdateCommand = new SqlCommand(dsql3, ndcon);
                    glUpdate.UpdateCommand.Parameters.Add("@payid", SqlDbType.Char).Value = mpayID;
                    glUpdate.UpdateCommand.Parameters.Add("@AcctNumb", SqlDbType.Char).Value = tcAcct;
                    glUpdate.UpdateCommand.ExecuteNonQuery();
                }
            }

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
            clientview.Clear();
            payview.Clear();
            radioButton3.Enabled = radioButton4.Enabled = radioButton5.Enabled = false;
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

                payview.Columns.Remove("Payroll");
                payview.Columns.Remove("fname");
                payview.Columns.Remove("lname");
                payview.Columns.Remove("cont");
                payview.Columns.Remove("actExs");
                payview.Columns.Remove("colAmd");

                payview.Columns.Add("Payroll", typeof(String));
                payview.Columns.Add("fname", typeof(String));
                payview.Columns.Add("lname", typeof(String));
                payview.Columns.Add("cont", typeof(decimal));
                payview.Columns.Add("actExs", typeof(bool));
                payview.Columns.Add("colAmd", typeof(bool));

                //******************* New procedure starts here
                DataTable csvDataView = new DataTable();
                clientgrid.DataSource = null;
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
//                        clientgrid.Rows.Clear();
                        clientgrid.Rows.Add(csvDataView.Rows.Count);
                        int i = 0;
                        int rowcount = csvDataView.Rows.Count;
                        textBox3.Text = rowcount.ToString("N0");
                        string cPayrollNumber = string.Empty;           // csvDataView.Rows[j][0].ToString().Trim();
                        string cFirstName = string.Empty;               // csvDataView.Rows[j][1].ToString().Trim();
                        string cLastName = string.Empty;                // csvDataView.Rows[j][2].ToString().Trim();
                        string nContribution = string.Empty;            //Convert.ToDecimal(csvDataView.Rows[j][3]).ToString("N2");//.Trim();
                        bool tlAcctExists = false;                      // checkAccountExists(ncompid, cPayrollNumber, cs);
                        bool tlAmended = false;
                        for (int j = 0; j < rowcount; j++)
                        {
                            switch (columncount)
                            {

                                case 2:                                                             //2 column spreadsheet
                                    string dsearched = csvDataView.Rows[j][0].ToString().Trim();
                                    int dslen = dsearched.Trim().Length;
                                    int dstartindex0 = dsearched.IndexOf(' ');
                                    int dstartindex1 = dsearched.IndexOf(' ', dsearched.IndexOf(' ') + 1);
                                    cPayrollNumber = dsearched.Substring(0, dstartindex0).Trim();
                                    cLastName = dsearched.Substring(dstartindex0, dstartindex1 - dstartindex0).Trim();
                                    cFirstName = dsearched.Substring(dstartindex1, dslen - dstartindex1).Trim();
                                    nContribution = Convert.ToDecimal(csvDataView.Rows[j][1]).ToString("N2");
                                    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                                    break;

                                case 3:                                                             //3 column spreadsheet;
                                    cPayrollNumber = csvDataView.Rows[j][0].ToString().Trim();
                                    string dsearched1 = csvDataView.Rows[j][1].ToString().Trim();
                                    int dslen1 = dsearched1.Trim().Length;
                                    int dstartindex01 = dsearched1.IndexOf(' ');
                                    cLastName = dsearched1.Substring(0, dstartindex01).Trim();
                                    cFirstName = dsearched1.Substring(dstartindex01, dslen1 - dstartindex01).Trim();
                                    nContribution = Convert.ToDecimal(csvDataView.Rows[j][2]).ToString("N2");
                                    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                                    break;

                                case 4:                                                                 //4 column spreadsheet
                                    cPayrollNumber = csvDataView.Rows[j][0].ToString().Trim();
                                    cFirstName = csvDataView.Rows[j][1].ToString().Trim();
                                    cLastName = csvDataView.Rows[j][2].ToString().Trim();
                                    nContribution = Convert.ToDecimal(csvDataView.Rows[j][3]).ToString("N2");//.Trim();
                                    tlAcctExists = checkAccountExists(ncompid, cPayrollNumber, cs);
                                    break;

                            }
                            payview.Rows.Add(cPayrollNumber, cFirstName, cLastName, nContribution, tlAcctExists, tlAmended);
                            // updatePAYROLLID(cFirstName, cLastName, cPayrollNumber);
                            totContribution = totContribution + Convert.ToDecimal(nContribution);
                            i++;
                        }

                        clientgrid.AutoGenerateColumns = false;
                        clientgrid.DataSource = payview.DefaultView;
                        clientgrid.Columns[0].DataPropertyName = Convert.ToString(payview.Columns[0]);
                        clientgrid.Columns[1].DataPropertyName = Convert.ToString(payview.Columns[1]);
                        clientgrid.Columns[2].DataPropertyName = Convert.ToString(payview.Columns[2]);
                        clientgrid.Columns[3].DataPropertyName = Convert.ToString(payview.Columns[3]);
                        clientgrid.Columns[4].DataPropertyName = Convert.ToString(payview.Columns[4]);
                        clientgrid.Columns[5].DataPropertyName = Convert.ToString(payview.Columns[5]);

                        foreach(DataGridViewRow dgr in clientgrid.Rows )
                        {
                            dgr.DefaultCellStyle.BackColor = Convert.ToBoolean(dgr.Cells[4].Value) ? Color.Green : Color.Red;   //clientgrid.Rows[i].DefaultCellStyle.BackColor = tlAcctExists ? Color.Green : Color.Red;
                            dgr.DefaultCellStyle.ForeColor = Color.White;
                        }
                        string gcPayroll_ID = clientgrid.Rows[0].Cells["col0"].Value.ToString();
                       // deleteBkpPayrollOne(gcPayroll_ID);
                        ntotalCont = Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value);
                        gnMemberContribution = ntotalCont;
                        gnDistributedBalance = ntotalCont;
                        textBox1.Text = ntotalCont.ToString("N2");
                        getaccounts(gcPayroll_ID, ntotalCont);
                        radioButton3.Enabled = radioButton4.Enabled = radioButton5.Enabled = true;
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

        /*
          try
    {
        SqlParameter errNumber = new SqlParameter("@errNumber", 0);
        SqlParameter errLine = new SqlParameter("@errLine", 0);
        SqlParameter errMessage = new SqlParameter("@errMessage", "");

        Command.ExecuteNonQuery();

        int SqlError = (int)(errNumber.Value);
        int SqlLine = (int)(errNumber.Value);
        string SqlMessage = (string)errMessage.Value;

        if (SqlError == 0 ) { Success = true; }
        else {
            Success = false;
            // whatever else you want to do with the error data
        }
    }
             */

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
                    MessageBox.Show("Error is : " + ex.Message);
                    return false;   
                }
            }
        }

        //private void dobackground()
        //{
        //    for (int i = 0; i < clientview.Rows.Count; i++)
        //    {
        //        if (clientview.Rows[i]["lamort"] != null && clientview.Rows[i]["lamort"].ToString() != "")
        //        {
        //            //glAmort = Convert.ToBoolean(clientview.Rows[i]["lamort"]);
        //            //clientgrid.Rows[i].DefaultCellStyle.BackColor = glAmort ? Color.Green : Color.Yellow;
        //            //clientgrid.Rows[i].DefaultCellStyle.ForeColor = glAmort ? Color.White : Color.Black;
        //        }
        //        else
        //        {
        //            clientgrid.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
        //            clientgrid.Rows[i].DefaultCellStyle.BackColor = Color.White;
        //        }
        //    }
        //}

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
                            }

                            if (dColCount > 3)
                            {
                                csvData.Columns[3].DataType = typeof(decimal);      // contribution
                            }

                            if (dColCount >= 2 && dColCount <= 4)
                            {
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


        private static DataTable csvSorted(DataTable tcsvData)
        {
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
//            csvDataView.Clear();
            if (clientgrid.Rows.Count > 0)
            {
                clientgrid.Rows.Clear();
                textBox2.Text = "";
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            //          textBox2.Text = "";
  //          csvDataView.Clear();
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
                        ntotalCont = !Convert.IsDBNull(clientgrid.CurrentRow.Cells["col3"].Value) ? Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value): 0.00M;
                        gnMemberContribution = ntotalCont;
                        gnDistributedBalance = ntotalCont;
                        textBox1.Text = ntotalCont.ToString("N2");
                        textBox5.Text = ntotalCont.ToString("N2");
                        getaccounts(gcPayroll_ID, ntotalCont);
                    }
                }
                else
                {
                    string gcPayroll_ID = clientgrid.CurrentRow.Cells["col0"].Value.ToString();
                    gcMembCode = Convert.ToString(Processedclientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"]);
                    gcEmplID = Convert.ToString(Processedclientview.Rows[clientgrid.CurrentCell.RowIndex]["empl_no"]);
                    decimal ntotalCont = !Convert.IsDBNull(clientgrid.CurrentRow.Cells["col3"].Value) ? Convert.ToDecimal(clientgrid.CurrentRow.Cells["col3"].Value) : 0.00M;
                    getaccounts(gcPayroll_ID, ntotalCont);
//                    getProcessedAccounts(gcEmplID);
                }

                for (int i=0; i < membacctview1.Rows.Count; i++)
                {
                    string dstatus = membacctview1.Rows[i]["acctype"].ToString().Trim().ToUpper();
                    dstatus = dstatus.Replace('<', ' ').Replace('>', ' ').Trim();
                    bool tlDefinedAccounts = dstatus != "UNDEFINED" ? true : false; // membacctview1.Rows[i]["cacctnumb"].ToString().Trim();
                    if(!tlDefinedAccounts)
                    {
                        acctGrid.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        acctGrid.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    }
                }

            }
        }

        private void clientgrid_Sorted(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            payDataView = payview.DefaultView;
            payDataView.RowFilter = "actExs = 0";
            clientgrid.DataSource = payDataView.ToTable();
            foreach (DataGridViewRow dgr in clientgrid.Rows)
            {
                dgr.DefaultCellStyle.BackColor = Color.Red;   
                dgr.DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            payDataView = payview.DefaultView;
            payDataView.RowFilter = "actExs = 1";
            clientgrid.DataSource = payDataView.ToTable();
            foreach (DataGridViewRow dgr in clientgrid.Rows)
            {
                dgr.DefaultCellStyle.BackColor = Color.Green;
                dgr.DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            payDataView = payview.DefaultView;
            payDataView.RowFilter = "actExs = 1";
            clientgrid.DataSource = payDataView.ToTable();
            foreach (DataGridViewRow dgr in clientgrid.Rows)
            {
                dgr.DefaultCellStyle.BackColor = Color.Green;
                dgr.DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            payDataView = payview.DefaultView;
            payDataView.RowFilter = "actExs = 0";
            clientgrid.DataSource = payDataView.ToTable();
            foreach (DataGridViewRow dgr in clientgrid.Rows)
            {
                dgr.DefaultCellStyle.BackColor = Color.Red;
                dgr.DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            payDataView = payview.DefaultView;
            payDataView.RowFilter = "actExs = 0 or actExs = 1";
            clientgrid.DataSource = payDataView.ToTable();

            foreach (DataGridViewRow dgr in clientgrid.Rows)
            {
                dgr.DefaultCellStyle.BackColor = Convert.ToBoolean(dgr.Cells[4].Value) ? Color.Green : Color.Red;   //clientgrid.Rows[i].DefaultCellStyle.BackColor = tlAcctExists ? Color.Green : Color.Red;
                dgr.DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void acctGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            glAutomaticProcessing = false;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            glAutomaticProcessing = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

  

        private void acctGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            int? rowIdx = e?.RowIndex;
            int? colIdx = e?.ColumnIndex;
            if (rowIdx.HasValue && colIdx.HasValue)
            {
                var dgv = (DataGridView)sender;
                var cell = dgv?.Rows?[rowIdx.Value]?.Cells?[colIdx.Value]?.Value;
                if (dgv.Columns[colIdx.Value].Name == "ncont")
                {
                    if (!string.IsNullOrEmpty(cell?.ToString()))
                    {
                        decimal lnNewValue = Convert.ToDecimal(acctGrid.CurrentRow.Cells["ncont"].Value);
                      //  MessageBox.Show("This is the balance " + lnNewValue);
                        if (gnDistributedBalance <= 0.00m )//&& gnValueB4 > 0.00m )
                        {
                            if (gnValueB4 > 0.00m)
                            {
                                if (MessageBox.Show("Balance is zero, previouis balance is non zero, Do you want to redistribute ", "Redistribution",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                {
                                    int lnCurrIndex = acctGrid.CurrentRow.Index;
                                    string lcPayroll = Convert.ToString(clientgrid.CurrentRow.Cells["col0"].Value).Trim();
                                    acctGrid.CurrentRow.Cells["ncont"].Value = lnNewValue.ToString("N2");
                                    gnDistributedBalance = gnMemberContribution - lnNewValue;

                                    string lcAcctNumb = acctGrid.CurrentRow.Cells["actnumb"].Value.ToString().Trim();
                                    decimal accrint = Convert.ToDecimal(acctGrid.CurrentRow.Cells["naccInt"].Value);
                                    decimal nprinpal = Convert.ToDecimal(acctGrid.CurrentRow.Cells["Column1"].Value);
                                    decimal lnInitValue = 0.00m;

                                    foreach (DataGridViewRow dgr1 in acctGrid.Rows)
                                    {
                                        if (dgr1.Index != lnCurrIndex)
                                        {
                                            dgr1.Cells["ncont"].Value = lnInitValue.ToString("N2");
                                        }
                                    }
                                //    deleteBkpPayrollOne(lcPayroll);
                                    updateMemAmendBkp(lcAcctNumb, lnNewValue, accrint, nprinpal);
                                    textBox5.Text = gnDistributedBalance.ToString("N2");
                                }
                                else
                                {
                                    acctGrid.CurrentRow.Cells["ncont"].Value = gnValueB4.ToString("N2");
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }

        private void acctGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            gnValueB4 = Convert.ToString(acctGrid.CurrentRow.Cells["ncont"].Value)!= "" ? Convert.ToDecimal(acctGrid.CurrentRow.Cells["ncont"].Value) : 0.00m;
        }

        private void clientgrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton10_Click(object sender, EventArgs e)
        {
            button12.Enabled = true;
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            button12.Enabled = true;
        }

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            acctGrid.Enabled = true;
            acctGrid.Focus();
        }

        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            acctGrid.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
