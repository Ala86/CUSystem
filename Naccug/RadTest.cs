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
    public partial class RadTest : Form
    {
        DataTable clientview = new DataTable();
        DataTable docview = new DataTable();
        DataTable tempview = new DataTable();
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int gnVisno = 0;
        string gcCustCode = "";
        int gnReceivedBy = 0;
        int gnExaminedBy = 0;
        int gnTestID = 0;
        string gcExamResults = "";
        string gcExamsCons = "";
        String gcSrv_code = "";
        bool glCoverPay = false;
        bool glAdmitted = false;

        public RadTest()
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

        private void RadTest_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Radiology Examinations >>";
            getclientList();
            firstclient();
            getdoctor();
            gettemp();
        }

        private void getclientList()
        {
            string dsql = "exec tsp_radList_All " + ncompid;
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
                    clientgrid.Columns[3].DataPropertyName = "radreason";
                    clientgrid.Columns[4].DataPropertyName = "drname";
                    clientgrid.Columns[5].DataPropertyName = "req_date";
                    clientgrid.Columns[6].DataPropertyName = "visno";
                    textBox6.Text = clientview.Rows.Count.ToString();
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                    comboBox3.Enabled = true;
                }
                else
                {
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
                gcSrv_code = drow["srv_code"].ToString();
                gnTestID = Convert.ToInt32(drow["tes_id"]);
                //                textBox5.Text = gnVisno.ToString();
                //                textBox7.Text = (Convert.ToBoolean(drow["gender"]) ? "Male" : "Female");
                DateTime ddob = Convert.ToDateTime(drow["ddatebirth"]);
                textBox1.Text = (DateTime.Now.Year - ddob.Year).ToString();
                textBox3.Text = (((DateTime.Now - ddob).Days % 364) / 30).ToString();
                textBox4.Text = (((DateTime.Now - ddob).Days % 364) % 30).ToString();
            }
        }//firstclient

        private void getdoctor()
        {
            string docsql = "exec tsp_RadTech   " + ncompid;
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

                    comboBox1.DataSource = docview;
                    comboBox1.DisplayMember = "fullname";
                    comboBox1.ValueMember = "dr_ID";
                    comboBox1.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Radiology Technicians table is empty, inform IT DEPT immediately");
                }
            }
        }// end of getdoctor

        private void gettemp()
        {
      //      DataTable tempview = new DataTable();
            string docsql = "exec tsp_radstandardresults   " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter docadp = new SqlDataAdapter(docsql, ndConnHandle);
                docadp.Fill(tempview);
                if (tempview != null)
                {
                    /*radrView.rd_title,rd_id*/
                    comboBox2.DataSource = tempview;
                    comboBox2.DisplayMember = "rd_title";
                    comboBox2.ValueMember = "rd_id";
                    comboBox2.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Radiology Technicians table is empty, inform IT DEPT immediately");
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
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
            if (gcCustCode != "" && gnReceivedBy >0 && gnExaminedBy >0 && examResults.Text.ToString().Trim()!="" && examCons.Text.ToString().Trim()!="")
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
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
               gnReceivedBy = Convert.ToInt32(comboBox3.SelectedValue);
                AllClear2Go();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dselval1 = Convert.ToString(comboBox1.SelectedValue).Trim();
            if (dselval1 != "System.Data.DataRowView" && dselval1 != "")
            {
                gnExaminedBy = Convert.ToInt32(comboBox1.SelectedValue);
                AllClear2Go();
            }

        }

        private void examResults_Leave(object sender, EventArgs e)
        {
            gcExamResults = examResults.Text.ToString();
            //            MessageBox.Show("exam results leave "+gcExamResults);
            AllClear2Go();
        }

        private void examCons_Leave(object sender, EventArgs e)
        {
            gcExamsCons = examCons.Text.ToString();
            //          MessageBox.Show("exam cons =" + gcExamsCons);
            AllClear2Go();
        }


        private void SaveButton_Click(object sender, EventArgs e)
        {
            getadmission(gcCustCode);
            if(glCoverPay)          //update accounts for cover clients, because they have not been paid for
            {
                updateaccounts();
            }
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                ndConnHandle3.Open();

                string cpatquery = "exec tsp_UpdateRadQueue @gnCompid,@nTestID,@srvcode,@nTestBy,@nRec_by,@cCustCode ";

                string cpatquery1 = "exec tsp_UpdateradResult @gnCompid,@nTestID,@srvcode,@cRadResult,@cRadConclude";

                string cpatquery2 = "update pat_visit set lconsult =0 where ccustcode = @cCustCode and activesession =1 and larchived =0";  //outpatient
                string cpatquery3 = "update todayvisit set lconsult =0 where ccustcode = @cCustCode and activesession =1 and larchived =0"; //outpatient

                string cpatquery4 = "update pat_visit set lconsult =1 where ccustcode = @cCustCode and activesession =1 and larchived =0"; //inpatient
                string cpatquery5 = "update todayvisit set lconsult =1 where ccustcode = @cCustCode and activesession =1 and larchived =0"; //inpatient

                SqlDataAdapter updradq = new SqlDataAdapter();
                updradq.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                updradq.UpdateCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                updradq.UpdateCommand.Parameters.Add("@nTestID", SqlDbType.Int).Value = gnTestID;
                updradq.UpdateCommand.Parameters.Add("@srvcode", SqlDbType.Char).Value = gcSrv_code;
                updradq.UpdateCommand.Parameters.Add("@nTestBy", SqlDbType.Int).Value = gnExaminedBy;
                updradq.UpdateCommand.Parameters.Add("@nRec_by", SqlDbType.Int).Value = gnReceivedBy;
                updradq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                updradq.UpdateCommand.ExecuteNonQuery();

                SqlDataAdapter updradr = new SqlDataAdapter();
                updradr.UpdateCommand = new SqlCommand(cpatquery1, ndConnHandle3);
                updradr.UpdateCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                updradr.UpdateCommand.Parameters.Add("@nTestID", SqlDbType.Int).Value = gnTestID;
                updradr.UpdateCommand.Parameters.Add("@srvcode", SqlDbType.Char).Value = gcSrv_code;
                updradr.UpdateCommand.Parameters.Add("@cRadResult", SqlDbType.Char).Value = gcExamResults;
                updradr.UpdateCommand.Parameters.Add("@cRadConclude", SqlDbType.Char).Value = gcExamsCons;
                updradr.UpdateCommand.ExecuteNonQuery();

                if (glAdmitted)
                {
                    SqlDataAdapter updpatq = new SqlDataAdapter();
                    updpatq.UpdateCommand = new SqlCommand(cpatquery4, ndConnHandle3);
                    updpatq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                    updpatq.UpdateCommand.ExecuteNonQuery();

                    SqlDataAdapter updtodq = new SqlDataAdapter();
                    updtodq.UpdateCommand = new SqlCommand(cpatquery5, ndConnHandle3);
                    updtodq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                    updtodq.UpdateCommand.ExecuteNonQuery();
                }
                else
                {
                    SqlDataAdapter updpatq = new SqlDataAdapter();
                    updpatq.UpdateCommand = new SqlCommand(cpatquery2, ndConnHandle3);
                    updpatq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                    updpatq.UpdateCommand.ExecuteNonQuery();

                    SqlDataAdapter updtodq = new SqlDataAdapter();
                    updtodq.UpdateCommand = new SqlCommand(cpatquery3, ndConnHandle3);
                    updtodq.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;
                    updtodq.UpdateCommand.ExecuteNonQuery();
                }

                ndConnHandle3.Close();
            }
            examResults.Clear();
            examCons.Clear();
            getclientList();
            firstclient();
        }// end of savebutton

        private void getadmission(string tcCode)
        {
            //            MessageBox.Show("inside getadmission");
            string visSql = "exec tsp_Admitted_Clients_One  @tnCompid,@tcCustCode";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter doAdm = new SqlDataAdapter();
                DataTable dvisview = new DataTable();
                doAdm.SelectCommand = new SqlCommand(visSql, ndConnHandle);
                doAdm.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
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

        private void updateaccounts()
        {
            /*
             With This
	gcVoucherNo=genbill('2')

**********update Cover institution Account
	=UpdateGlmast(gcCoverAcct,gnPostAmt)			&&Cover Institution Account - Posting Account
	gnPNewBal=CheckLastBalance(gcCoverAcct)
	=UpdateTransactionHistory(gcCoverAcct,gnPostAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'93',1,gnPatID,0,gcIntSuspense,0.00,1,gnPostAmt,gcReceiptNo,.F.,gnVisno,.F.,6,gcSrv_code,'')

	=UpdateGlmast(gcIntSuspense,gnContAmt)			&&update internal Suspense Account - Contra account
	gncNewBal=CheckLastBalance(gcIntSuspense)
	=UpdateTransactionHistory(gcIntSuspense,gnContAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'92',1,gnPatID,0,gcCoverAcct,0.00,1,gnContAmt,gcReceiptNo,.F.,gnVisno,.F.,6,gcSrv_code,'')

**********update Service Income Account and Account Receivable Account
	gnPostAmt=Abs(gnPostAmt)
	gnContAmt=-Abs(gnPostAmt)

	=UpdateGlmast(gcIncAcct,gnPostAmt)				&&Update Service Income Account - Posting Account
	gnPNewBal=CheckLastBalance(gcIncAcct)
	=UpdateTransactionHistory(gcIncAcct,gnPostAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'92',1,gnPatID,0,gcAccRec,0.00,1,gnPostAmt,gcReceiptNo,.F.,gnVisno,.F.,6,gcSrv_code,'')

	=UpdateGlmast(gcAccRec,gnContAmt)				&&Update Account Receivable Account - Contra Account
	gncNewBal=CheckLastBalance(gcAccRec)
	=UpdateTransactionHistory(gcAccRec,gnContAmt,gcTestName,gcVoucherNo,'000001',gcUserID,gnPNewBal,'93',1,gnPatID,0,gcIncAcct,0.00,1,gnContAmt,gcReceiptNo,.F.,gnVisno,.F.,6,gcSrv_code,'')

	.Refresh
Endwith

             */
        }

        private void button6_Click(object sender, EventArgs e)
        {
            radtemplate rdt = new radtemplate();
            rdt.ShowDialog();
            tempview.Clear();
            gettemp();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dselval = Convert.ToString(comboBox2.SelectedValue).Trim();
            if (dselval != "System.Data.DataRowView" && dselval != "")
            {
                examResults.Text = tempview.Rows[Convert.ToInt32(comboBox2.SelectedIndex)]["rd_name"].ToString();
            } 

        }
    }
}
