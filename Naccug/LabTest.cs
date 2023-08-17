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
    public partial class LabTest : Form
    {
        DataTable clientview = new DataTable();
        DataTable docview = new DataTable();
        DataTable resultview = new DataTable();
        string gcCustCode = "";
        bool glDocSelect = false;
        bool glAdmitted = false;
        bool glCoverPay = false;
        bool glResultIn = false;
        int gnVisno;
        int ncompid = globalvar.gnCompid;
        string cs = globalvar.cos;
        int gnDr_id = 0;
        string gcSrv_code = "";
        string gcReq_Numb = "";
        string gcTestName = "";
        int gnTestID = 0;
        int gnTestBy = 0;
        int gnRec_by = 0;
        int gnSpecId = 0;
        int gnHiResult = 0;
        

        public LabTest()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LabTest_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Laboratory Tests >>";
            getclientList();
            firstclient();
            getdoctor();
        }


        private void getclientList()
        {
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
//            sn = SQLExec(gnConnHandle, "exec tsp_LabList_All ?gnCompid", "labView")
            string dsql = "exec tsp_LabList_All " + ncompid ;
            clientview.Clear();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "clientcode";
                    clientgrid.Columns[1].DataPropertyName = "clname";
                    clientgrid.Columns[2].DataPropertyName = "item_name";
                    clientgrid.Columns[3].DataPropertyName = "labreason";      
                    clientgrid.Columns[4].DataPropertyName = "drname";
                    clientgrid.Columns[5].DataPropertyName = "req_date";
                    clientgrid.Columns[6].DataPropertyName = "spectype";
                    clientgrid.Columns[7].DataPropertyName = "received_by";
                    clientgrid.Columns[8].DataPropertyName = "srv_code";
                    clientgrid.Columns[9].DataPropertyName = "req_numb";
                    clientgrid.Columns[10].DataPropertyName = "spe_id";
                    clientgrid.Columns[11].DataPropertyName = "tes_id";
                    //                  clientgrid.Columns[5].DataPropertyName = "drname";
                    //                clientgrid.Columns[5].DataPropertyName = "drname";
                    textBox6.Text = clientview.Rows.Count.ToString();
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                        clientview.Rows.Add();
                    }
                    radioButton13.Enabled = true;
                    radioButton24.Enabled = true;
                    comboBox3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No more tests to process");
                    radioButton13.Enabled = false;
                    radioButton24.Enabled = false;
                    comboBox3.Enabled = false;
                }
            }
        }//end of getclientlist

        private void firstclient()
        {
            if (clientview.Rows.Count > 0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                gnVisno = Convert.ToInt16(drow["visno"]);
                gcCustCode = drow["clientcode"].ToString();
                textBox5.Text = gnVisno.ToString();
                textBox7.Text = (Convert.ToBoolean(drow["gender"]) ? "Male" : "Female");
                DateTime ddob =Convert.ToDateTime(drow["ddatebirth"]);
//                MessageBox.Show("Date of birth is " + ddob);
                textBox1.Text = (DateTime.Now.Year - ddob.Year).ToString();
                textBox3.Text = (((DateTime.Now - ddob).Days % 364)/30).ToString();
                textBox4.Text = (((DateTime.Now - ddob).Days % 364) % 30).ToString();
            }
        }//firstclient

        private void getdoctor()
        {
            string docsql = "exec tsp_LabTech   " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter docadp = new SqlDataAdapter(docsql, ndConnHandle);
                docadp.Fill(docview);
                if (docview != null)
                {
                    comboBox3.DataSource = docview;
                    comboBox3.DisplayMember = "fullname";
                    comboBox3.ValueMember = "dr_ID";
                    comboBox3.SelectedIndex = -1;
                    //                    MessageBox.Show("We are here");
                }
                else
                {
                    MessageBox.Show("Laboratory Technicians table is empty, inform IT DEPT immediately");
                }
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
//            MessageBox.Show("cust,dr,docs " + gcCustCode + "," + gnDr_id + "," + glDocSelect);
            if (gcCustCode != "" && glResultIn  && glDocSelect)
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
        //        SaveButton.Select();
            }
            else
            {
                SaveButton.Enabled = false;
            }

        }
        #endregion

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dselval = Convert.ToString(comboBox3.SelectedValue).Trim();
            if (dselval != "System.Data.DataRowView" && dselval != "")
            {
                glDocSelect = true;
                gnDr_id = Convert.ToInt32(comboBox3.SelectedValue);   
                AllClear2Go();
            }

        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton24.Checked)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }



        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (clientview.Rows.Count > 0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                gnVisno = Convert.ToInt16(drow["visno"]);
                gcCustCode = drow["clientcode"].ToString();
                textBox5.Text = gnVisno.ToString();
                textBox7.Text = (Convert.ToBoolean(drow["gender"]) ? "Male" : "Female");
                DateTime ddob =Convert.ToDateTime(drow["ddatebirth"]);
//                MessageBox.Show("Date of birth is " + ddob);
                textBox1.Text = (DateTime.Now.Year - ddob.Year).ToString();
                textBox3.Text = (DateTime.Now.Month - ddob.Month).ToString();
                textBox4.Text = (DateTime.Now.Day - ddob.Day).ToString();
            }
            */
        }

        private void manualresults()
        {
//            MessageBox.Show("We will be doing manual results");
            string docsql = "exec tsp_LabTestResults_One @nCompid,@cSrv_code,@cCustCode,@cReq_Numb";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dolab = new SqlDataAdapter();
                dolab.SelectCommand = new SqlCommand(docsql, ndConnHandle);
                dolab.SelectCommand.Parameters.Add("@nCompid", SqlDbType.Int).Value = ncompid;
                dolab.SelectCommand.Parameters.Add("@cSrv_code", SqlDbType.Char).Value = gcSrv_code;
                dolab.SelectCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                dolab.SelectCommand.Parameters.Add("@cReq_Numb", SqlDbType.Char).Value = gcReq_Numb;
                dolab.SelectCommand.ExecuteNonQuery();
                dolab.Fill(resultview);
                if (resultview.Rows.Count > 0)
                {
                    resultGrid.AutoGenerateColumns = false;
                    resultGrid.DataSource = resultview.DefaultView;
                    resultGrid.Columns[0].DataPropertyName = "par_name";
                    resultGrid.Columns[1].DataPropertyName = "dresult";
                    resultGrid.Columns[2].DataPropertyName = "dunit";
                    resultGrid.Columns[3].DataPropertyName = "dlow";
                    resultGrid.Columns[4].DataPropertyName = "dhigh";
                   resultGrid.Columns[8].DataPropertyName = "par_id";
                }
                else
                {
                    MessageBox.Show("Result parameters have not been set for Test, inform IT DEPT immediately");
                }
            }
            /*
                .column1.ControlSource = 'labresults.par_name'
                .column2.ControlSource = 'labresults.dunit'
                .column3.ControlSource = 'labresults.dlow'
                .column4.ControlSource = 'labresults.dhigh'
                .column5.ControlSource = 'labresults.dresult'
                .column6.ControlSource = 'labresults.low'
                .column7.ControlSource = 'labresults.hih'
                .column8.ControlSource = 'labresults.nor'
                .column9.ControlSource = 'labresults.testresult'
                .column10.ControlSource = 'labresults.testresult'
                 */

        }
        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton13.Checked )
            {
                DataRow drow1 = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                gcCustCode = drow1["clientcode"].ToString();
                gcSrv_code = drow1["srv_code"].ToString(); 
                gcReq_Numb = drow1["req_numb"].ToString();
                gnSpecId = Convert.ToInt32(drow1["spe_id"]);
                gnRec_by = Convert.ToInt32(drow1["rec_by"]);
                gnTestID = Convert.ToInt32(drow1["tes_id"]);
                gcTestName= drow1["item_name"].ToString();
                manualresults();
            }
        }//end of checkedchanged

//        private void resultGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
  //      {
    //        resultGrid.CurrentCell = resultGrid.Rows[(e.RowIndex)].Cells[(e.ColumnIndex + 1)];
      //  }

        private void resultGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            resultGrid.EndEdit();
        }

        private void updateaccounts()
        {
            MessageBox.Show("inside update accounts");
            /*
             With This
	gcVoucherNo=genbill('2')

**********update Cover institution Account
	=UpdateGlmast(gcCoverAcct,gnPostAmt)			&&Cover Institution Account - Posting Account
	gnPNewBal=CheckLastBalance(gcCoverAcct)
	=UpdateTransactionHistory(gcCoverAcct,gnPostAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'93',1,gnPatID,0,gcIntSuspense,0.00,1,gnPostAmt,gcReceiptNo,.F.,gnVisno,.F.,5,gcSrv_code,'')

	=UpdateGlmast(gcIntSuspense,gnContAmt)			&&update internal Suspense Account - Contra account
	gncNewBal=CheckLastBalance(gcIntSuspense)
	=UpdateTransactionHistory(gcIntSuspense,gnContAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'92',1,gnPatID,0,gcCoverAcct,0.00,1,gnContAmt,gcReceiptNo,.F.,gnVisno,.F.,5,gcSrv_code,'')

**********update Service Income Account and Account Receivable Account
	gnPostAmt=Abs(gnPostAmt)
	gnContAmt=-Abs(gnPostAmt)

	=UpdateGlmast(gcIncAcct,gnPostAmt)				&&Update Service Income Account - Posting Account
	gnPNewBal=CheckLastBalance(gcIncAcct)
	=UpdateTransactionHistory(gcIncAcct,gnPostAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'92',1,gnPatID,0,gcAccRec,0.00,1,gnPostAmt,gcReceiptNo,.F.,gnVisno,.F.,5,gcSrv_code,'')

	=UpdateGlmast(gcAccRec,gnContAmt)				&&Update Account Receivable Account - Contra Account
	gncNewBal=CheckLastBalance(gcAccRec)
	=UpdateTransactionHistory(gcAccRec,gnContAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'93',1,gnPatID,0,gcIncAcct,0.00,1,gnContAmt,gcReceiptNo,.F.,gnVisno,.F.,5,gcSrv_code,'')

	.Refresh
Endwith

             */
        }

        private void getadmission(string tcCode)
        {
//            MessageBox.Show("inside getadmission");
            string visSql = "select 1 from TodayVisit where ccustcode=@tcCustCode and activesession=1 and larchived=0 and sent2admit=1";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter doAdm = new SqlDataAdapter();
                DataTable dvisview = new DataTable();
                doAdm.SelectCommand = new SqlCommand(visSql, ndConnHandle);
                doAdm.SelectCommand.Parameters.Add("@tcCustCode", SqlDbType.Char).Value = tcCode;
                doAdm.SelectCommand.ExecuteNonQuery();
                doAdm.Fill(dvisview);
                if (dvisview.Rows.Count > 0)
                {
                    glAdmitted = true;
//                    MessageBox.Show("Client has been admitted");
                }
                else
                {
                    glAdmitted = false;
  //                  MessageBox.Show("outpatient Client ");
                }
            }
    //        MessageBox.Show("getting out of getadmission");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
    //        MessageBox.Show("save step 0");
            if (glCoverPay)                     //	If glCoverPay  //&&update accounts for cover clients, because they have not been paid for lab services. They will be billed after we save this record
            {
                updateaccounts();
            }

            getadmission(gcCustCode);           //            .getadmission(gcCustCode)

            if (resultview.Rows.Count > 0)
            {
      //          MessageBox.Show("save step 1");
                if (gnHiResult > 0)              //          If N>0
                {
                    if (MessageBox.Show("You have " + gnHiResult + " Results flagged as high. Do you want to continue", "Flagged results", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        //      Return
                        return;
                    }
                }
            }
            //**********************************************
            //            string cs1 = globalvar.cos;
            //          string tcUserid = globalvar.gcUserid;
            //        int visno = gnVisno;
            //      string tcReceipt = genreceipt.getreceipt(cs1, globalvar.gdSysDate);
//            MessageBox.Show("save step 2");
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "exec tsp_UpdateLabQueue @gnCompid,@nTestID,@srvcode,@nTestBy,@nRec_by,@nSpecId,@cCustCode";
                string cpatquery1 = "exec tsp_UpdateLabResultNew @gnCompid,@nItemNo,@lcParName,@dunit,@dlow,@dhigh,@dvalue,@nTestID,@cCustCode,@gcReq_Numb,@llow,@lhih,@lnor,@nVisno,@cTestName,@lcSrvCode,@lnparid";
                string cpatquery2 = "update pat_visit set lconsult =0 where ccustcode = @cCustCode and activesession =1 and larchived =0";
                string cpatquery3 = "update todayvisit set lconsult =0 where ccustcode = @cCustCode and activesession =1 and larchived =0";

                SqlDataAdapter updlabq = new SqlDataAdapter();
                updlabq.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                updlabq.UpdateCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                updlabq.UpdateCommand.Parameters.Add("@nTestID", SqlDbType.Int).Value = gnTestID;
                updlabq.UpdateCommand.Parameters.Add("@srvcode", SqlDbType.Char).Value = gcSrv_code;
                updlabq.UpdateCommand.Parameters.Add("@nTestBy", SqlDbType.Int).Value = gnDr_id;
                updlabq.UpdateCommand.Parameters.Add("@nRec_by", SqlDbType.Int).Value = gnRec_by;
                updlabq.UpdateCommand.Parameters.Add("@nSpecId", SqlDbType.Int).Value = gnSpecId;
                updlabq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;

                ndConnHandle3.Open();
                updlabq.UpdateCommand.ExecuteNonQuery();
      //          MessageBox.Show("save step 3");
                //                if (resultview.Rows.Count > 0)
                //              {
                for (int i = 0; i < resultview.Rows.Count; i++)
                {
                    SqlDataAdapter updlabr = new SqlDataAdapter();
                    updlabr.UpdateCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                    updlabr.UpdateCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                    updlabr.UpdateCommand.Parameters.Add("@nItemNo", SqlDbType.Int).Value = 0;
                    updlabr.UpdateCommand.Parameters.Add("@lcParName", SqlDbType.Char).Value = resultGrid.Rows[i].Cells["par_name"].Value.ToString();
                    updlabr.UpdateCommand.Parameters.Add("@dunit", SqlDbType.Char).Value = resultGrid.Rows[i].Cells["dunit"].Value.ToString();
                    updlabr.UpdateCommand.Parameters.Add("@dlow", SqlDbType.Char).Value = resultGrid.Rows[i].Cells["rlow"].Value.ToString();
                    updlabr.UpdateCommand.Parameters.Add("@dhigh", SqlDbType.Char).Value = resultGrid.Rows[i].Cells["rhih"].Value.ToString();
                    updlabr.UpdateCommand.Parameters.Add("@dvalue", SqlDbType.Char).Value = resultGrid.Rows[i].Cells["dresult"].Value.ToString();
                    updlabr.UpdateCommand.Parameters.Add("@nTestID", SqlDbType.Int).Value = gnTestID;
       //             MessageBox.Show("save step 3.1");

                    updlabr.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                    updlabr.UpdateCommand.Parameters.Add("@gcReq_Numb", SqlDbType.Char).Value = gcReq_Numb;
                    updlabr.UpdateCommand.Parameters.Add("@llow", SqlDbType.Bit).Value = Convert.ToBoolean(resultGrid.Rows[i].Cells["llow"].Value);
                    updlabr.UpdateCommand.Parameters.Add("@lhih", SqlDbType.Bit).Value = Convert.ToBoolean(resultGrid.Rows[i].Cells["lhih"].Value); ;
                    updlabr.UpdateCommand.Parameters.Add("@lnor", SqlDbType.Bit).Value = Convert.ToBoolean(resultGrid.Rows[i].Cells["lnor"].Value); ;
                    updlabr.UpdateCommand.Parameters.Add("@nVisno", SqlDbType.Int).Value = gnVisno;
                    updlabr.UpdateCommand.Parameters.Add("@cTestName", SqlDbType.Char).Value = gcTestName;
                    //******************************************************************
                    updlabr.UpdateCommand.Parameters.Add("@lcSrvCode", SqlDbType.Char).Value = gcSrv_code;
                    updlabr.UpdateCommand.Parameters.Add("@lnparid", SqlDbType.Int).Value = Convert.ToInt32(resultGrid.Rows[i].Cells["par_id"].Value);

                    updlabr.UpdateCommand.ExecuteNonQuery();
                }
                //"update pat_visit set lconsult =0 where ccustcode = ?gcCustCode and activesession =1 and larchived =0", "patupd")
                // "update TodayVisit set lconsult =0 where ccustcode = ?gcCustCode and activesession =1 and larchived =0", "patupd")
       //         MessageBox.Show("save step 4");

                SqlDataAdapter updpatq = new SqlDataAdapter();
                updpatq.UpdateCommand = new SqlCommand(cpatquery2, ndConnHandle3);
                updpatq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                updpatq.UpdateCommand.ExecuteNonQuery();

                SqlDataAdapter updtodq = new SqlDataAdapter();
                updtodq.UpdateCommand = new SqlCommand(cpatquery3, ndConnHandle3);
                updtodq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                updtodq.UpdateCommand.ExecuteNonQuery();

                ndConnHandle3.Close();
            }
            //                updDashBoard();
      //      MessageBox.Show("save step 5");

            resultview.Clear();
            radioButton13.Checked = false;
            radioButton24.Checked = false;
      //      MessageBox.Show("save step 6");
            getclientList();
            firstclient();
            /*
                .definegrid
                .definetestgrid
                .defineresultgrid
                .getlablist
                         */
        }//end of savebutton

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
DO FORM testRsetup.scx 
glFormOpened=.f.

             */
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*
             thisform.Visible =.f.
DO FORM medhistory.scx
thisform.Visible = .t.
             */
        }

        private void button15_Click(object sender, EventArgs e)
        {
            /*
             DO FORM unarchive.scx

             */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
reprnt=Createobject('twocode','Specimen type Setup','spectype','spe_name',gnCompID)
reprnt.Show
glFormOpened=.f.

             */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
             glFormOpened=.t.
DO FORM labscanprint WITH 'L'
glFormOpened=.f.
             */
        }

        private void resultGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
         resultGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void resultGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (resultGrid.Columns[e.ColumnIndex].Name == "dresult")
            {
                if (resultGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()!="")
                {
                    int lnLow = Convert.ToInt32(resultGrid.Rows[e.RowIndex].Cells["rlow"].Value);
                    int lnHih = Convert.ToInt32(resultGrid.Rows[e.RowIndex].Cells["rhih"].Value);
                    int dres = Convert.ToInt32(resultGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    resultGrid.Rows[e.RowIndex].Cells["llow"].Value = (dres < lnLow ? true : false);
                    resultGrid.Rows[e.RowIndex].Cells["lnor"].Value = (dres >= lnLow && dres<=lnHih ? true : false);
                    resultGrid.Rows[e.RowIndex].Cells["lhih"].Value = (dres > lnHih ? true : false);
                    gnHiResult = gnHiResult + (dres > lnHih ? 1 : 0);
                    glResultIn = true;      //  tempfiles.temporary_files_update(cs, 2, tcCode, gnVisno, srvcode, srvname, srvFee, false, true);  //we will update temporary lab files
                }
                else
                {
                    glResultIn = false;     // tempfiles.temporary_files_update(cs, 2, tcCode, gnVisno, srvcode, srvname, srvFee, false, false);  //we will update temporary lab files
                }
  //              AllClear2Go();
            }

        }

        private void resultGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            AllClear2Go();
        }
    }
}
