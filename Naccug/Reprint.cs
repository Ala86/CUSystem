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
    public partial class Reprint : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int gnVisno = 0;
        string cloca = globalvar.cLocalCaption;
        string gcAcctNumb = string.Empty;
        string gcCustCode = string.Empty;
        string gcReceiptNo = string.Empty;
        String gcClientID = String.Empty;
      //  string 
        int gnMainRow = 0;
        int gnSubRow = 0;
        bool glClientFound = false;
        bool glPreviewPrint = false;
        bool glNewClient = false;

        DataTable transview = new DataTable();

        DepositDataSet dds = new DepositDataSet();

        public Reprint(string tcCode, string tcReceiptNumber, int tnOrder)
        {
            InitializeComponent();
            gcCustCode = tcCode;
            gcReceiptNo = tcReceiptNumber;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            printreceipt(gcCustCode, gcReceiptNo, 1);
        }
        private void printreceipt(string tcCode, string tcReceiptNumber, int tnRecOrder)
        {
            using (SqlConnection dcon = new SqlConnection(cs))
            {
                dcon.Open();
                dds.CashReceiptDataTable.Clear();
                string lcReportFolder = getStartupFolder.gcStartUpDirectory;
                string lcReportFile = "CashReceipt.rpt";
                DateTime ldReceiptDate = DateTime.Today;
                int lnRecType = tnRecOrder = 1;
                // int lnRecType = tnRecOrder > 1 ? 2 : 1;

                string cpatquery = "Exec tsp_ClientReceipt 30,@tcCustCode,@tcReceiptNo,@tnrecType";

                SqlDataAdapter glinsCommand = new SqlDataAdapter();
                SqlDataAdapter cashSel = new SqlDataAdapter();
                glinsCommand.SelectCommand = new SqlCommand(cpatquery, dcon);
                glinsCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                glinsCommand.SelectCommand.Parameters.Add("@tcCustCode", SqlDbType.Char).Value = gcCustCode;
                glinsCommand.SelectCommand.Parameters.Add("@tcReceiptNo", SqlDbType.Char).Value = gcReceiptNo;
                glinsCommand.SelectCommand.Parameters.Add("@tnrecType", SqlDbType.Int).Value = lnRecType;

                int rcount = 0;
                glinsCommand.SelectCommand.ExecuteNonQuery();
                glinsCommand.Fill(dds.CashReceiptDataTable);
                rcount = dds.CashReceiptDataTable.Rows.Count;

              //  MessageBox.Show("This is the receipt No." + tcCode);
              //  MessageBox.Show("This is the receipt No." + tcReceiptNumber);

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
                           // cashviewer.ReportSource = CashReceiptDoc;
                         //   cashviewer.Zoom(1);
                          //  cashviewer.Refresh();
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

    }
}
