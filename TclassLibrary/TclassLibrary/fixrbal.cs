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
using System.Threading;
namespace TclassLibrary
{
    public partial class fixrbal : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        DataTable acctview = new DataTable();
        DataTable transview = new DataTable();
        DataTable runbalview = new DataTable();
        DataTable fixbalview = new DataTable();
        DataTable bkbalview = new DataTable();
        decimal gnRunBal = 0.00m;
        decimal gnBookBal = 0.00m;
        public fixrbal(string tcCos, int tnCompid, string tcLoca, DateTime tdSysDate, string tcUserID, int tnBranchid)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }


        private void fixrbal_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Running Balance Check >>";
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if (radioButton1.Checked && maskedTextBox1.Text!="")
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion 
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                fixRunBal(maskedTextBox1.Text, Convert.ToDecimal(textBox4.Text),true);
                MessageBox.Show("Running Balance fix successful");
                radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
                textBox1.Text = textBox2.Text = textBox4.Text = textBox5.Text = textBox6.Text =  maskedTextBox1.Text = "";
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
            else
            {
                string sqlString = (radioButton2.Checked ? "Select cacctnumb,nbookbal,cacctname from glmast where acode in ('250','251') order by cacctnumb" : 
                    (radioButton3.Checked ? "Select cacctnumb,nbookbal,cacctname from glmast where acode in ('130','131') order by cacctnumb" :
                    (radioButton5.Checked ? "Select cacctnumb,nbookbal,cacctname from glmast where acode in ('270','271') order by cacctnumb" :
                    "Select cacctnumb,nbookbal,cacctname from glmast where intcode=1 order by cacctnumb")));
                getAccounts(sqlString);
            }
        }

        private void getAccounts(string tcquery)
        {
            transview.Clear();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(tcquery, ndConnHandle);
                da1.Fill(transview);
                if (transview != null && transview.Rows.Count > 0)
                {
                    string tcAcct = string.Empty;
         //           decimal tnBookBal = 0.00m;
                    int dcount = 0;
                    int j = 0;
                    int transcount = transview.Rows.Count;
                   foreach (DataRow drow in transview.Rows)
                    {
                        tcAcct = drow["cacctnumb"].ToString();
                        textBox5.Text = tcAcct;
//                        tnBookBal = Convert.ToDecimal(drow["nbookbal"]);
                        dcount++;
                        j = dcount + 1;
                        progressBar1.Value = j * progressBar1.Maximum / transcount;
                        getBookBal(tcAcct);
                        getRunBal(tcAcct);
                        if(gnBookBal != gnRunBal)
                        {
                            fixRunBal(tcAcct, gnBookBal, false);
                        }
                    }
                    saveButton.Enabled = false;
                    saveButton.BackColor = Color.Gainsboro;
                    progressBar1.Value = 0;
                    radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = radioButton4.Checked = false;
                    MessageBox.Show("Running Balance fix successful");
                }
            }
        }

        private void getBookBal(string tcAcct)
        {
            bkbalview.Clear();
            string sqlString0 = "select nbookbal= case when sum(tranhist.ntranamnt) is not null then sum(ntranamnt) else 0.00 end from tranhist where cacctnumb =  " + "'" + tcAcct + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter dam0 = new SqlDataAdapter(sqlString0, ndConnHandle);
                dam0.Fill(bkbalview);
                if (bkbalview != null && bkbalview.Rows.Count > 0)
                {
                    gnBookBal = Convert.ToDecimal(bkbalview.Rows[0]["nbookbal"]);// bkbalview.Rows[0]["nbookbal"].ToString().Trim()!="null" ? Convert.ToDecimal(bkbalview.Rows[0]["nbookbal"]) : 0.00m;
                    updateBkBal(tcAcct, gnBookBal);
                }
            }
        }

        private void getRunBal(string tcAcctNo)
        {
            runbalview.Clear();
            string runbalsql0 = "select nnewbal, cstack, itemid from tranhist where cacctnumb  = " + "'"+tcAcctNo +"'"+ " and ctrancode <> '97' order by dtrandate desc ";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter darunsql0 = new SqlDataAdapter(runbalsql0, ndConnHandle);
                darunsql0.Fill(runbalview);
                if (runbalview != null && runbalview.Rows.Count > 0)
                {
                    object val = runbalview.Rows[0]["nnewbal"];
                    string valtype = val.GetType().ToString();
                    if(valtype != "System.Decimal")
                    {
                        MessageBox.Show("The culprit is " + val);
                    }
                    else
                    {
                        gnRunBal = Convert.ToDecimal(runbalview.Rows[0]["nnewbal"]);
                    }
                }
            }
        }

        private void updateBkBal(string tcAct, decimal tnBkBal)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cqueryb = "update glmast set nbookbal = @tnBkBal where cacctnumb=@tcAcctNumb";
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cqueryb, ndConnHandle1);
                cuscommand.UpdateCommand.Parameters.Add("@tnBkBal", SqlDbType.Decimal).Value = tnBkBal;
                cuscommand.UpdateCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = tcAct; 

                ndConnHandle1.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }


        private void fixRunBal(string tcfAcct, decimal tnfBkBal,bool act)
        {
            if (act)
            {
                if (MessageBox.Show("Have you ensured that the book balance is correct and would like to continue", "Running Balance Fix Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    string sqlString = "select cacctnumb,ntranamnt,cstack,itemid,dpostdate from tranhist where cacctnumb = " + "'"+ tcfAcct+"'" + " and ctrancode <> '13'and ctrancode <> '17'and ctrancode <> '97' order by dtrandate, itemid desc";
                    fixbalview.Clear();
                    using (SqlConnection ndConnHandle = new SqlConnection(cs))
                    {
                        ndConnHandle.Open();
                        SqlDataAdapter dam = new SqlDataAdapter(sqlString, ndConnHandle);
                        dam.Fill(fixbalview);
                        if (fixbalview != null && fixbalview.Rows.Count > 0)
                        {
                            for (int k = 0; k < fixbalview.Rows.Count; k++)
                            {
                                string lcStack = fixbalview.Rows[k]["cstack"].ToString();
                                decimal lnTranAmt = Convert.ToDecimal(fixbalview.Rows[k]["ntranamnt"]);
                                updateTrans(tcfAcct, tnfBkBal, lcStack);
                                tnfBkBal = tnfBkBal - lnTranAmt;
                            }
                        }
                    }
                }
            }
            else
            {
                string sqlString = "select cacctnumb,ntranamnt,cstack,itemid,dtrandate from tranhist where cacctnumb = " + "'" + tcfAcct + "'" + " and ctrancode <> '97' order by dtrandate desc";
                fixbalview.Clear();
                using (SqlConnection ndConnHandle = new SqlConnection(cs))
                {
                    ndConnHandle.Open();
                    SqlDataAdapter dan = new SqlDataAdapter(sqlString, ndConnHandle);
                    dan.Fill(fixbalview);
                    if (fixbalview != null && fixbalview.Rows.Count > 0)
                    {
                        for (int l = 0; l < fixbalview.Rows.Count; l++)
                        {
                            string lcStack =fixbalview.Rows[l]["cstack"].ToString();
                            decimal lnTranAmt = Convert.ToDecimal(fixbalview.Rows[l]["ntranamnt"]);
                            updateTrans(tcfAcct, tnfBkBal, lcStack);
                            tnfBkBal = tnfBkBal - lnTranAmt;
                        }
                    }
                }
            }
        }

        private void updateTrans(string tcuAcctNumb, decimal tnuBkBal, string tcuStack)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string cquery = "update tranhist set nnewbal=@tnBkBal where cacctnumb=@tcAcctNumb and cstack=@lcStack";
                SqlDataAdapter cuscommand = new SqlDataAdapter();
                cuscommand.UpdateCommand = new SqlCommand(cquery, ndConnHandle1);
                cuscommand.UpdateCommand.Parameters.Add("@tnBkBal", SqlDbType.Decimal).Value = tnuBkBal;
                cuscommand.UpdateCommand.Parameters.Add("@tcAcctNumb", SqlDbType.VarChar).Value = tcuAcctNumb;
                cuscommand.UpdateCommand.Parameters.Add("@lcStack", SqlDbType.VarChar).Value = tcuStack;

                ndConnHandle1.Open();
                cuscommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle1.Close();
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            maskedTextBox1.Enabled = radioButton1.Checked ? true : false;
        }

        private void maskedTextBox1_Validated(object sender, EventArgs e)
        {
            string tcAcctNumb = maskedTextBox1.Text.Trim();
            string sqlString = "select cacctname,nbookbal from glmast where cacctnumb  = " + tcAcctNumb;
            DataTable indview = new DataTable();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(sqlString, ndConnHandle);
                da1.Fill(indview);
                if (indview != null && indview.Rows.Count > 0)
                {
                    textBox4.Text = indview.Rows[0]["nbookbal"].ToString();
                    textBox5.Text = indview.Rows[0]["cacctname"].ToString();
                    getRunBal(tcAcctNumb);
                }
                else
                { MessageBox.Show("account could not be found"); maskedTextBox1.Text = ""; }
            }
        }



        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked )
            {
                maskedTextBox1.Enabled = false;
                maskedTextBox1.Text = "";                     
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                maskedTextBox1.Enabled = false;
                maskedTextBox1.Text = "";
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                maskedTextBox1.Enabled = false;
                maskedTextBox1.Text = "";
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                maskedTextBox1.Enabled = false;
                maskedTextBox1.Text = "";
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
