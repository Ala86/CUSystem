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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Drawing.Printing;
using TclassLibrary;

namespace WinTcare
{
    public partial class ReprintForm : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int gnVisno = 0;
        string cloca = globalvar.cLocalCaption;
        string gcAcctNumb = string.Empty;
        string gcCustCode = string.Empty;
        string gcReceiptNo = string.Empty;
        String gcClientID = String.Empty;
        int gnMainRow = 0;
        int gnSubRow = 0;
        bool glClientFound = false;
        bool glPreviewPrint = false;
        bool glNewClient = false;
        DataTable transview = new DataTable();

        DepositDataSet dds = new DepositDataSet();



        public ReprintForm()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReprintForm_Load(object sender, EventArgs e)
        {
            this.Text = cloca + " << Reprint >> ";
            copyright.Text = globalvar.gcCopyRight;
            TransGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            TransGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                string lcClientCode = textBox1.Text.Trim().PadLeft(6, '0');
                textBox1.Text = lcClientCode;
                checkClient(lcClientCode);
                if (glClientFound == true)
                {
                        getReceipts(lcClientCode);
                }
            }
        }
    
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                //            AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                //         AllClear2Go();
            }
        }
        private void getReceipts(string tcCustCode)
        {
            string dsql1 = "exec tsp_getReceiptsByMember @tncompid, @tcMemberID,@tDate1,@tDate2";
            transview.Clear();
            DateTime ldFrom = Convert.ToDateTime(fromDate.Value.ToShortDateString());
            DateTime ldTo = Convert.ToDateTime(toDate.Value.ToShortDateString());

            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                    SqlDataAdapter da1 = new SqlDataAdapter();
                    da1.SelectCommand = new SqlCommand(dsql1, ndConnHandle1);
                    da1.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                    da1.SelectCommand.Parameters.Add("@tcMemberID", SqlDbType.Char).Value = tcCustCode;
                    da1.SelectCommand.Parameters.Add("@tDate1", SqlDbType.DateTime).Value = ldFrom;
                    da1.SelectCommand.Parameters.Add("@tDate2", SqlDbType.DateTime).Value = ldTo;
                    da1.SelectCommand.ExecuteNonQuery();
                    da1.Fill(transview);

                if (transview.Rows.Count > 0)
                {
                        TransGrid.AutoGenerateColumns = false;
                        TransGrid.DataSource = transview.DefaultView;
                        TransGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        TransGrid.Columns[0].DataPropertyName = "receiptno";
                        TransGrid.Columns[1].DataPropertyName = "dpostdate";
                        TransGrid.Columns[2].DataPropertyName = "ntranamnt";
                        TransGrid.Columns[3].DataPropertyName = "sel";
                        ndConnHandle1.Close();
                        TransGrid.Rows[0].Selected = true;
                }
                else
                {
                    MessageBox.Show("No receipts found for selected condition ") ;
                }
            }
        }

        //private void printreceipt(string tcCode, string tcReceiptNumber,int tnOrder)
        //{
        //    using (SqlConnection dcon = new SqlConnection(cs))
        //    {
        //        dcon.Open();
        //        dds.CashReceiptDataTable.Clear();
        //        string lcReportFolder = getStartupFolder.gcStartUpDirectory;
        //        string lcReportFile = tnOrder ==1 ?  "CashReceipt.rpt" : "CashPowerReceipt.rpt";
        //        DateTime ldReceiptDate = DateTime.Today;
        //        string cpatquery = "Exec tsp_ClientReceipt @tnCompid,@tcCustCode,@tcReceiptNo";
        //        string cpatquery1 = "Exec tsp_CashPowerReceipt @tcToken";

        //        SqlDataAdapter glinsCommand = new SqlDataAdapter();
        //        SqlDataAdapter cashSel = new SqlDataAdapter();

        //        glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
        //        glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
        //        glinsCommand.SelectCommand.Parameters.Add("@tcCustCode", SqlDbType.Char).Value = tcCode;
        //        glinsCommand.SelectCommand.Parameters.Add("@tcReceiptNo", SqlDbType.Char).Value = tcReceiptNumber;


        //        cashSel.SelectCommand = new SqlCommand(cpatquery1, dcon);
        //        cashSel.SelectCommand.Parameters.Add("@tcToken", SqlDbType.Char).Value = tcReceiptNumber;

        //        int rcount = 0;
        //      //  int ccount = 0;

        //        if(radioButton1.Checked )
        //        {
        //            glinsCommand.SelectCommand.ExecuteNonQuery();
        //            glinsCommand.Fill(dds.CashReceiptDataTable);
        //             rcount = dds.CashReceiptDataTable.Rows.Count;
        //        }
        //        else
        //        {
        //        }

        //        if (rcount > 0)
        //        {
        //            ReportDocument CashReceiptDoc = new ReportDocument();
        //            bool dRepExists = getReportFile.dReportfile(lcReportFolder, lcReportFile);
        //            if (dRepExists)
        //            {
        //                if(radioButton1.Checked )                     //deposit Receipt
        //                {
        //                    string lcCashier = globalvar.gcUserName;
        //                    string lcTranDesc = string.Empty;
        //                    decimal tnTranAmt = 0.00m;
        //                    decimal tnWaiveAmt = 0.00m;
        //                    decimal tnBillTotal = 0.00m;
        //                    string lcReceiptNo = dds.CashReceiptDataTable.Rows[0]["autorecit"].ToString().Trim();
        //                    string lcFullname = dds.CashReceiptDataTable.Rows[0]["cfullname"].ToString().Trim();
        //                    string lcCustCode = dds.CashReceiptDataTable.Rows[0]["ccustcode"].ToString().Trim();
        //                    CashReceiptDoc.Load(lcReportFile);
        //                    CashReceiptDoc.SetDataSource(dds);
        //                    CashReceiptDoc.Refresh();
        //                    CashReceiptDoc.SetParameterValue(0, "something here");
        //                    CashReceiptDoc.SetParameterValue("ReceiptNumber", lcReceiptNo);
        //                    CashReceiptDoc.SetParameterValue("ClientName", lcFullname);
        //                    CashReceiptDoc.SetParameterValue("ClientCode", lcCustCode);
        //                    CashReceiptDoc.SetParameterValue("CashierCode", lcCashier);
        //                    CashReceiptDoc.SetParameterValue("nWaiveAmt", tnWaiveAmt);
        //                    CashReceiptDoc.SetParameterValue("receiptDate", ldReceiptDate);
        //                    try
        //                    {
        //                        foreach (DataRow dr in dds.CashReceiptDataTable.Rows)
        //                        {
        //                            tnTranAmt = tnTranAmt + Convert.ToDecimal(dr["ntranamnt"]);
        //                        }
        //                        tnTranAmt = tnTranAmt - tnWaiveAmt;
        //                        CashReceiptDoc.SetParameterValue("nTranTotal", tnTranAmt);
        //                        tnBillTotal = tnTranAmt + tnWaiveAmt;
        //                        CashReceiptDoc.SetParameterValue("nBillTotal", tnBillTotal);
        //                        cashviewer.ReportSource = CashReceiptDoc;
        //                        cashviewer.Zoom(1);
        //                        cashviewer.Refresh();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(ex.ToString());
        //                    }
        //                    PrinterSettings getprinterName = new PrinterSettings();
        //                    CashReceiptDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
        //                    CashReceiptDoc.PrintToPrinter(1, false, 0, 1);
        //                }
        //                else                                              //cash power receipt 
        //                {
      
        //                    //PrinterSettings getprinterName = new PrinterSettings();
        //                    //CashReceiptDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
        //                    //CashReceiptDoc.PrintToPrinter(1, false, 0, 1);
        //                }

        //            }
        //            else { MessageBox.Show("Report file " + lcReportFile + " could not be found , inform IT DEPT "); }
        //        }
        //        else { MessageBox.Show("No receipt details found for client ID = " + tcCode + " and  receipt number = " + tcReceiptNumber); }
        //        dcon.Close();
        //    }
        //} //end of printreceipt


        private void printreceipt(string tcCode, string tcReceiptNumber, int tnRecOrder)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                dds.CashReceiptDataTable.Clear();
                string lcReportFolder = getStartupFolder.gcStartUpDirectory;
                string lcReportFile = "CashReceipt.rpt";
                DateTime ldReceiptDate = DateTime.Today;
                int lnRecType = tnRecOrder > 1 ? 2 : 1;
                string cpatquery = "Exec tsp_ClientReceipt @tnCompid,@tcCustCode,@tcReceiptNo,@tnrecType";

                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                SqlDataAdapter cashSel = new SqlDataAdapter();

                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@tcCustCode", SqlDbType.Char).Value = tcCode;
                glinsCommand.SelectCommand.Parameters.Add("@tcReceiptNo", SqlDbType.Char).Value = tcReceiptNumber;
                glinsCommand.SelectCommand.Parameters.Add("@tnrecType", SqlDbType.Int).Value = lnRecType;

                int rcount = 0;
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(dds.CashReceiptDataTable);
                rcount = dds.CashReceiptDataTable.Rows.Count;


                if (rcount > 0)
                {
                    ReportDocument CashReceiptDoc = new ReportDocument();
                    bool dRepExists = getReportFile.dReportfile(lcReportFolder, lcReportFile);
                    if (dRepExists)
                    {
                        string lcCashier = globalvar.gcUserName;
                        string lcTranDesc = string.Empty;
                        decimal tnTranAmt = 0.00m;
                        decimal tnWaiveAmt = 0.00m;
                        decimal tnBillTotal = 0.00m;
                        string lcReceiptNo = dds.CashReceiptDataTable.Rows[0]["autorecit"].ToString().Trim();
                        string lcFullname = dds.CashReceiptDataTable.Rows[0]["clientname"].ToString().Trim();
                        string lcCustCode = dds.CashReceiptDataTable.Rows[0]["ccustcode"].ToString().Trim();
                        CashReceiptDoc.Load(lcReportFile);
                        CashReceiptDoc.SetDataSource(dds);
                        CashReceiptDoc.Refresh();
                        CashReceiptDoc.SetParameterValue(0, "something here");
                        CashReceiptDoc.SetParameterValue("ReceiptNumber", lcReceiptNo);
                        CashReceiptDoc.SetParameterValue("ClientName", lcFullname);
                        CashReceiptDoc.SetParameterValue("ClientCode", lcCustCode);
                        CashReceiptDoc.SetParameterValue("CashierCode", lcCashier);
                        CashReceiptDoc.SetParameterValue("nWaiveAmt", tnWaiveAmt);
                        CashReceiptDoc.SetParameterValue("receiptDate", ldReceiptDate);
                        try
                        {
                            foreach (DataRow dr in dds.CashReceiptDataTable.Rows)
                            {
                                tnTranAmt = tnTranAmt + Convert.ToDecimal(dr["ntranamnt"]);
                            }
                            tnTranAmt = tnTranAmt - tnWaiveAmt;
                            CashReceiptDoc.SetParameterValue("nTranTotal", tnTranAmt);
                            tnBillTotal = tnTranAmt + tnWaiveAmt;
                            CashReceiptDoc.SetParameterValue("nBillTotal", tnBillTotal);
                            cashviewer.ReportSource = CashReceiptDoc;
                            cashviewer.Zoom(1);
                            cashviewer.Refresh();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        PrinterSettings getprinterName = new PrinterSettings();
                        CashReceiptDoc.PrintOptions.PrinterName = getprinterName.PrinterName;
                        CashReceiptDoc.PrintToPrinter(1, false, 0, 1);

                    }
                    else { MessageBox.Show("Report file " + lcReportFile + " could not be found , inform IT DEPT "); }
                }
                else { MessageBox.Show("No receipt details found for client ID = " + tcCode + " and  receipt number = " + tcReceiptNumber); }
                dcon.Close();
            }
        } //end of printreceipt


        private void checkClient(string tcCode)
        {
            /*exec tsp_New_Client_One ?gnCompid,?lcCardNumber*/
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_New_Client_One  " + ncompid + ",'" + tcCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable vtable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(vtable);
                if (vtable.Rows.Count > 0)
                {
                    glClientFound = true;
//                    gcAcctNumb = "260"+ vtable.Rows[0]["ccustcode"].ToString().Trim()+"01";
                }
                else
                { glClientFound = false;}
            }
        }

        private void TransGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            TransGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void TransGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (TransGrid.Focused)
            {
                try
                {
                    if (TransGrid.Columns[e.ColumnIndex].Name == "rSelect")
                    {
                        gnMainRow = e.RowIndex;
                        string lcSel = Convert.ToString(TransGrid.CurrentRow.Cells[e.ColumnIndex].Value).Trim();
                        bool recsel = lcSel != "" ? Convert.ToBoolean(TransGrid.CurrentRow.Cells[e.ColumnIndex].Value) : false; ;
                        gcCustCode = textBox1.Text.ToString();
                        decimal lnRecAmount = Convert.ToDecimal(TransGrid.CurrentRow.Cells["dreceipt1"].Value); 
                        gcReceiptNo = TransGrid.CurrentRow.Cells["dreceipt"].Value.ToString().Trim(); 
                        if (recsel)
                        {
                            int lnRecType = lnRecAmount > 0.00m ? 1 : 2;
                                printreceipt(gcCustCode, gcReceiptNo,lnRecType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //}
            }
        }

        private bool checkVisitNumber(string tcClientCode)
        {
            string dsql = "Exec tsp_GetVisit_New  " + ncompid + "," + "'" + tcClientCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dav = new SqlDataAdapter(dsql, ndConnHandle);
                DataTable visview1 = new DataTable();
                dav.Fill(visview1);
                if (visview1.Rows.Count > 0)
                {
                    glNewClient = true;
                    return true;
                }
                else
                {
                    glNewClient = false;
                    return false;
                }
            }
        }
        //private void SaveButton_Click(object sender, EventArgs e)
        //{
        //    printreceipt(gcCustCode, gcReceiptNo);
        //}

        private void TransGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            gcReceiptNo = TransGrid.CurrentRow.Cells["dreceipt"].Value.ToString();
        }

        private void TransGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (TransGrid.Focused)
            {
                if (TransGrid.Columns[e.ColumnIndex].Name == "rSelect")
                {
                    SaveButton.Enabled = false;
                    SaveButton.BackColor = Color.Gainsboro;
                    int lnRecCount = TransGrid.Rows.Count;
                    if (gcReceiptNo != "")
                    {
                        for (int i = 0; i < lnRecCount; i++)
                        {
                            if (Convert.ToString(TransGrid.Rows[i].Cells["dreceipt"].Value).Trim() != gcReceiptNo)
                            {
                                TransGrid.Rows[i].Cells["rSelect"].Value = false;
                            }
                        }
                    }
                }
            }
        }

        /*
         //Create ping object
Ping netMon = new netMon();

private void Load()
{
    //Wire events (in constructor or InitializeComponent)
    netMon.PingError += new PingErrorEventHandler(netMon_PingError);
    netMon.PingStarted += new PingStartedEventHandler(netMon_PingStarted);
    netMon.PingResponse += new PingResponseEventHandler(netMon_PingResponse);
    netMon.PingCompleted += new PingCompletedEventHandler(netMon_PingCompleted);
}

private void Ping(string hostname)
{
    //Start ping
    IAsyncResult result = netMon.BeginPingHost(
                     new AsyncCallback(EndPing), hostname, 4);
}

private void EndPing(IAsyncResult result)
{
    netMon.EndPingHost(result);
}

private void netMon_PingStarted(object sender, PingStartedEventArgs e)
{
    //Process ping started
}

private void netMon_PingResponse(object sender, PingResponseEventArgs e)
{
    //Process ping response
}

private void netMon_PingCompleted(object sender, PingCompletedEventArgs e)
{
    //Process ping completed
}

private void netMon_PingError(object sender, PingErrorEventArgs e)
{
    //Process ping error
}
         */


        /*
         IPEndPoint ipep = new IPEndPoint(Ipaddress.Parse("IP TO CHECK"), YOUR_PORT_INTEGER);
Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Connect(ipep);
         */

    }
}
