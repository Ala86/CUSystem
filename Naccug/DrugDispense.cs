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
using System.Data.SqlClient;

namespace WinTcare
{
    public partial class DrugDispense : Form
    {
        DataTable clientview = new DataTable();
        DataTable drugview = new DataTable();
        bool glCanDischarge = false;
        bool glCoverClient = false;
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int gnVisno = 0;
        string gcCustCode = "";
        string tcUserid = globalvar.gcUserid;
        string gcDrugName = "";
        string gcProdCode = "";
        decimal gnPostAmt = 0.00m;
        decimal gnContAmt = 0.00m;
        int gnQty = 0;
        string gcUnitMeas = "";
        decimal gnUnitPrice = 0.00m;
        string gcFormula = "";
        string gcCoverAcct = "";
        string gcIncAcct = "";
        string gcExpAcct = "";
        string gcAccPay = "";
        string gcAccRec = "";
        string gcInvAcc = ""; 


        public DrugDispense()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DrugDispense_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Drug Dispense >>";
            getclientList();
//            firstclient();
        }

        private void getclientList()
        {
            clientview.Clear();
            drugview.Clear();
//            MessageBox.Show("inside getclientlist ");
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string dsql = "exec tsp_OutstandingDrugs  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    //                    gnVisno = Convert.ToInt16(ds.Rows["visno"]);
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "fname";
                    clientgrid.Columns[1].DataPropertyName = "mname";
                    clientgrid.Columns[2].DataPropertyName = "lname";
                    clientgrid.Columns[3].DataPropertyName = "visdate";      // "age";
                    clientgrid.Columns[4].DataPropertyName = "vistime";
                    clientgrid.Columns[5].DataPropertyName = "visno";
                    clientgrid.Columns[6].DataPropertyName = "ccustcode";
                    for (int i = 0; i < 10; i++)
                    {
                        //                        clientgrid.Rows[i].Cells["vistime"].Value = clientview.Rows[i]["vistime"].ToString().Substring(0, 8);
                        clientview.Rows.Add();
                    }
                    ndConnHandle.Close();
                    firstclient();
                    clientgrid.Focus();
                }//else { MessageBox.Show("All drugs dispensed"); }
            }
        }


        private void firstclient()
        {
            if (clientview.Rows.Count > 0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                gnVisno = Convert.ToInt16(drow["visno"]);
                gcCustCode = drow["ccustcode"].ToString();
                textBox2.Text = gcCustCode;
                textBox3.Text = Convert.ToDateTime(drow["consdate"]).ToLongDateString(); 
                textBox4.Text = drow["dr_fullname"].ToString();
                textBox8.Text = (Convert.ToBoolean(drow["gender"]) ? "Male" : "Female");
                DateTime ddob = Convert.ToDateTime(drow["ddatebirth"]);
                textBox5.Text = (DateTime.Now.Year - ddob.Year).ToString();
                textBox6.Text = (((DateTime.Now - ddob).Days % 364) / 30).ToString();
                textBox7.Text = (((DateTime.Now - ddob).Days % 364) % 30).ToString();
                getdetails(gcCustCode);
                getdiag(gcCustCode);
                getdinsurance(gcCustCode);        //only for cover clients
            }
        }//firstclient

        private void getdinsurance(string tcCode)
        {
//            MessageBox.Show("we are about to get the insurance account");
            string dinsuance = "exec tsp_GetDinsurance  " + ncompid + "," + "'" + tcCode + "'"+","+ gnVisno;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                DataTable insview = new DataTable();
                SqlDataAdapter insdata = new SqlDataAdapter(dinsuance, ndConnHandle);
                insdata.Fill(insview);
                if (insview.Rows.Count > 0)
                {
                    gcCoverAcct = insview.Rows[0]["cacctnumb"].ToString();
                }
            }
        }
        private void getdetails(string tcCode)
        {
                string dsql1 = "exec tsp_OutstandingDrugsDetailed  " + ncompid + "," + "'" + tcCode + "'";
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    SqlDataAdapter da1 = new SqlDataAdapter(dsql1, ndConnHandle);
                    da1.Fill(drugview);
                    if (drugview.Rows.Count > 0)
                    {
                        drugGrid.AutoGenerateColumns = false;
                        drugGrid.DataSource = drugview.DefaultView;
                        drugGrid.Columns[0].DataPropertyName = "dpostdate";
                        drugGrid.Columns[1].DataPropertyName = "prod_name";
                        drugGrid.Columns[2].DataPropertyName = "unitmeas";
                        drugGrid.Columns[3].DataPropertyName = "perday";
                        drugGrid.Columns[4].DataPropertyName = "cformula";      // "age";
                        drugGrid.Columns[5].DataPropertyName = "quantity";
                        drugGrid.Columns[6].DataPropertyName = "unitPrice";
                        drugGrid.Columns[7].DataPropertyName = "damount";
                        drugGrid.Columns[8].DataPropertyName = "dosage";

                        drugGrid.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                        drugGrid.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        drugGrid.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        drugGrid.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        drugGrid.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        drugGrid.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        drugGrid.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        drugGrid.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        drugGrid.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                        drugGrid.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        drugGrid.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                        drugGrid.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                        decimal totalbill = 0.00m;
                        for (int j = 0;j < drugview.Rows.Count;j++)
                        {
                            totalbill += Convert.ToDecimal(drugview.Rows[j]["damount"]);
                        }
                        textBox9.Text = totalbill.ToString();
                        for (int i = 0; i < 5; i++)
                        {
                            drugview.Rows.Add();
                        }
                        ndConnHandle.Close();
                        SaveButton.Enabled = true;
                        SaveButton.BackColor = Color.LawnGreen;
                    }else
                    {
                        SaveButton.Enabled = false;
                        SaveButton.BackColor = Color.Gainsboro;
                    }
            }
        }//end of getdetails 

        private void getdiag(string tcCode)
        {
            string dsql2 = "exec tsp_FinalDiagnosis_One  " + ncompid + "," + "'" + tcCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable diagview = new DataTable();
                da1.Fill(diagview);
                if (diagview.Rows.Count > 0)
                {
                    provDiag.Text = diagview.Rows[0]["prov_dianosis"].ToString();
                    finaDiag.Text = diagview.Rows[0]["fina_dianosis"].ToString();
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "update drug_dispense set dispensed = 1, disp_date = convert(date, getdate()), disp_time = convert(time, getdate()) ";
                cpatquery += " where ccustcode=@gcCustCode and visno=@nVisNo and prod_code=@tcProdCode";
                ndConnHandle3.Open();
                if (drugview.Rows.Count > 0)
                {
                    for (int i = 0; i < drugview.Rows.Count; i++)
                    {
                        if(drugview.Rows[i]["prod_code"].ToString()!="")
                        {
                            gnQty = Convert.ToInt32(drugview.Rows[i]["quantity"]);
                            gcUnitMeas = drugview.Rows[i]["unitmeas"].ToString();
                            gnUnitPrice = Convert.ToDecimal(drugview.Rows[i]["unitprice"]);
                            gcFormula = drugview.Rows[i]["cformula"].ToString();
                            gcDrugName = drugview.Rows[i]["prod_name"].ToString();
                            gcProdCode = drugview.Rows[i]["prod_code"].ToString();
                            gnPostAmt = -Math.Abs(Convert.ToInt32(drugview.Rows[i]["quantity"]) * Convert.ToDecimal(drugview.Rows[i]["unitprice"]));
                            gnContAmt = Math.Abs(gnPostAmt);   //        gnContAmt = Abs(gnPostAmt)
         //                   MessageBox.Show("We are working with " +gcProdCode );
                            if (glCoverClient)  //for cover clients because they had not been billed
                            {
                                updateaccounts(gcProdCode);
                            }
                            SqlDataAdapter drugupdcommand = new SqlDataAdapter();
                            drugupdcommand.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                            drugupdcommand.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = gcCustCode;
                            drugupdcommand.UpdateCommand.Parameters.Add("@nVisNo", SqlDbType.Int).Value = gnVisno;
                            drugupdcommand.UpdateCommand.Parameters.Add("@tcProdCode", SqlDbType.Char).Value = drugview.Rows[i]["prod_code"].ToString();
                            drugupdcommand.UpdateCommand.ExecuteNonQuery();
                            drugupdcommand.UpdateCommand.Parameters.Clear();
              //              MessageBox.Show("going to update drugs with " + gcProdCode);
                            updatedrug(gcProdCode, gnQty);
                        }
                    }
                }
                ndConnHandle3.Close();
            }
            checkoutstanding(gcCustCode);     //        Thisform.checkoutstanding
            if (glCanDischarge)
            {
                client_discharge(gcCustCode);
            }
            finaDiag.Text = "";
            provDiag.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            SaveButton.Enabled = false;
            SaveButton.BackColor = Color.Gainsboro;
//            MessageBox.Show("Now we will refresh the form");
            getclientList();
        }//end of savebutton

        private void updateaccounts(string tcProd)
        {
            string gcVoucherNo = genbill.genvoucher(cs, globalvar.gdSysDate);
            string tcContra = globalvar.gcIntSuspense;
            bool llfreebee = false;
            decimal tnPNewBal = 0.00m;
            decimal tnCNewBal = 0.00m;
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                SqlDataReader cprodread = null;
                SqlCommand cgetserv = new SqlCommand("select inc_acc,cog_acc,acc_pay,acc_rec,inv_acc,buy_price from products where prod_code   = @tcPro", ndConnHandle1);
                cgetserv.Parameters.Add("@tcPro", SqlDbType.Char).Value = tcProd;
                ndConnHandle1.Open();
                cprodread = cgetserv.ExecuteReader();
                cprodread.Read();
                if (cprodread.HasRows == true)
                {
                    string gcIncAcct = cprodread["inc_acc"].ToString();
                    string gcExpAcct = cprodread["cog_acc"].ToString();
                    string gcAccPay = cprodread["acc_pay"].ToString();
                    string gcAccRec = cprodread["acc_rec"].ToString();
                    string gcInvAcc = cprodread["inv_acc"].ToString();
                }
//                MessageBox.Show("Accounts " + gcIncAcct + "," + gcExpAcct + "," + gcAccPay + "," + gcAccRec + "," + gcInvAcc);
                updateGlmast gls = new updateGlmast();
                updateTranhist tls = new updateTranhist();

                //**********update Cover institution Account

                gls.updGlmast(cs, gcCoverAcct, gnPostAmt);                                          //update glmast posting account
                tnPNewBal = TclassLibrary.CheckLastBalance.lastbalance(cs, gcCoverAcct);        // CheckLastBalance.lastbalance(cs1, tcAcctNumb); CheckLastBalance.las . la CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                tls.updTranhist(cs, gcCoverAcct, gnPostAmt, gcDrugName, gcVoucherNo, "000001", tcUserid, tnPNewBal, "93", 1, false, tcContra, 0.00m, 1, gnPostAmt, "", false, gnVisno, true, 7, "", gcProdCode, llfreebee, gcCustCode, ncompid);                   //update tranhist posting account

                gls.updGlmast(cs, tcContra, gnContAmt);                                          //update glmast contra account
                tnCNewBal = TclassLibrary.CheckLastBalance.lastbalance(cs, tcContra);        // CheckLastBalance.lastbalance(cs1, tcAcctNumb); CheckLastBalance.las . la CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                tls.updTranhist(cs, tcContra, gnContAmt, gcDrugName, gcVoucherNo, "000001", tcUserid, tnCNewBal, "92", 1, false, gcCoverAcct, 0.00m, 1, gnContAmt, "", false, gnVisno, true, 7, "", gcProdCode, llfreebee, gcCustCode, ncompid);                   //update tranhist posting account

                gls.updGlmast(cs, gcIncAcct, gnPostAmt);                                          //update glmast posting account
                tnPNewBal = TclassLibrary.CheckLastBalance.lastbalance(cs, gcIncAcct);        // CheckLastBalance.lastbalance(cs1, tcAcctNumb); CheckLastBalance.las . la CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                tls.updTranhist(cs, gcIncAcct, gnPostAmt, gcDrugName, gcVoucherNo, "000001", tcUserid, tnPNewBal, "92", 1, false, gcAccRec, 0.00m, 1, gnPostAmt, "", false, gnVisno, true, 7, "", gcProdCode, llfreebee, gcCustCode, ncompid);                   //update tranhist posting account

                gls.updGlmast(cs, gcAccRec, gnContAmt);                                          //update glmast posting account
                tnPNewBal = TclassLibrary.CheckLastBalance.lastbalance(cs, gcAccRec);        // CheckLastBalance.lastbalance(cs1, tcAcctNumb); CheckLastBalance.las . la CheckLastBalance.lastbalance(cs1, tcAcctNumb);       //  0.00m;
                tls.updTranhist(cs, gcAccRec, gnContAmt, gcDrugName, gcVoucherNo, "000001", tcUserid, tnPNewBal, "93", 1, false, gcIncAcct, 0.00m, 1, gnContAmt, "", false, gnVisno, true, 7, "", gcProdCode, llfreebee, gcCustCode, ncompid);                   //update tranhist posting account
            }
        }
        private void updatedrug(string tcprodcode, int lnqty)
        {
            string dsql4 = "exec tsp_UpdateDrugDispensed @tcProdcode,@lnNewBal,@nque";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                    ndConnHandle.Open();
                    SqlDataAdapter updgrcomm = new SqlDataAdapter();
                    updgrcomm.UpdateCommand = new SqlCommand(dsql4, ndConnHandle);
                    updgrcomm.UpdateCommand.Parameters.Add("@tcProdcode", SqlDbType.Char).Value = tcprodcode;
                    updgrcomm.UpdateCommand.Parameters.Add("@lnNewBal", SqlDbType.Int).Value = lnqty;
                    updgrcomm.UpdateCommand.Parameters.Add("@nque", SqlDbType.Int).Value = 1;
                    updgrcomm.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
            }
        }

        private void checkoutstanding(string tcCustCode)
        {
            string chksql = "exec tsp_OutstandingServices  "+ncompid+","+ "'"+tcCustCode+"'" ;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da33 = new SqlDataAdapter(chksql, ndConnHandle);
                DataTable ds33 = new DataTable();
                da33.Fill(ds33);
                if (ds33.Rows.Count > 0)
                {
                    bool lislab = Convert.ToBoolean(ds33.Rows[0]["islab"]);         // && Iif(Isnull(islab),.F.,.T.)
                    bool lisrad = Convert.ToBoolean(ds33.Rows[0]["israd"]);         // && Iif(Isnull(israd),.F.,.T.)
                    bool lisopt = Convert.ToBoolean(ds33.Rows[0]["isopt"]);         //&& Iif(Isnull(isopt),.F.,.T.)
                    bool lispha = Convert.ToBoolean(ds33.Rows[0]["ispha"]);         //&& Iif(Isnull(ispha),.F.,.T.)
                    if (lislab || lisrad || lisopt || lispha)                       ///If lislab> 0 Or lisrad> 0 Or lisopt> 0 Or lispha> 0
                    {
                        glCanDischarge = false;
                    }
                    else
                    {
                        glCanDischarge = true;
                    }
                }
                else
                {

                }
            }
        }

        private void client_discharge(string tcCustCode)
        {
            if (MessageBox.Show("Are you sure you want to discharge client", "Client Discharge Check", MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                string cldis = "update pat_visit set activesession=0,discdate=convert(date,getdate()), disctime=convert(time,getdate()) where ccustcode=@gcCustCode and activesession=1";
                string visdel = "delete from todayvisit where ccustcode=@gcCustCode";
                string hisdel = "delete from todayhist where ccustcode=@gcCustCode";
                string patdel = "delete from todaypats where ccustcode=@gcCustCode";
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    SqlDataAdapter cldisupd = new SqlDataAdapter();
                    cldisupd.UpdateCommand = new SqlCommand(cldis, ndConnHandle);
                    cldisupd.UpdateCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCustCode;

                    SqlDataAdapter visdelcom = new SqlDataAdapter();
                    visdelcom.DeleteCommand = new SqlCommand(visdel, ndConnHandle);
                    visdelcom.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCustCode;

                    SqlDataAdapter hisdelcom = new SqlDataAdapter();
                    hisdelcom.DeleteCommand = new SqlCommand(hisdel, ndConnHandle);
                    hisdelcom.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCustCode;

                    SqlDataAdapter patdelcom = new SqlDataAdapter();
                    patdelcom.DeleteCommand = new SqlCommand(patdel, ndConnHandle);
                    patdelcom.DeleteCommand.Parameters.Add("@gcCustCode", SqlDbType.Char).Value = tcCustCode;

                    cldisupd.UpdateCommand.ExecuteNonQuery();
                    visdelcom.DeleteCommand.ExecuteNonQuery();
                    hisdelcom.DeleteCommand.ExecuteNonQuery();
                    patdelcom.DeleteCommand.ExecuteNonQuery();
                    ndConnHandle.Close();
                }
            }
            else
            {
           //     MessageBox.Show("Client will be sent back to consultation");
            }
        }//end of client discharge
        
        
    }
}
