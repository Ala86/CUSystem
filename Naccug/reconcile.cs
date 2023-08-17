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
using TclassLibrary;

namespace WinTcare
{
    public partial class reconcile : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable bnkview = new DataTable();
        DataTable debview = new DataTable();
        DataTable creview = new DataTable();
        int gnSelcount = 0;
        decimal gnTotalReconciled = 0.00m;
        decimal gnDebitReconciled = 0.00m;
        decimal gnCreditReconciled = 0.00m;
        decimal gnLastReconBalance = 0.00m;
        decimal gnFinalReconBalance = 0.00m;
        decimal gnEndingBalance = 0.00m;
        string gcAccountNumber = string.Empty;

        public reconcile()
        {
            InitializeComponent();
        }

        private void reconcile_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Account Reconciliation >>";
            BnkAccts();
         
            //creditGrid.Columns["credamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            creditGrid.Columns["credamt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            //creditGrid.Columns["debamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            creditGrid.Columns["debamt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xls",
                Filter = "Data Files (*.xls;*.csv)|*.xls;*.xlsx;*.xlsm;*.csv",
                FilterIndex = 1,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox11.Text = openFileDialog1.SafeFileName;
//                button7.BackColor = Color.LawnGreen;
  //              button7.Enabled = true;
            }
            else
            {
                textBox11.Text = "";
    //            button7.BackColor = Color.Gainsboro;
      //          button7.Enabled = false;
            }

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
//                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
  //              AllClear2Go();
            }
        }
        private void BnkAccts()
        {
            string dsql1 = "exec tsp_getBankAccounts  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(bnkview);
                if (bnkview.Rows.Count > 0)
                {
                    comboBox1.DataSource = bnkview.DefaultView;
                    comboBox1.DisplayMember = "cacctname";
                    comboBox1.ValueMember = "cacctnumb";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused )
            {
                gnTotalReconciled = 0.00m;
                textBox4.Text = Convert.ToDateTime(bnkview.Rows[Convert.ToInt32(comboBox1.SelectedIndex)]["lrecdate"]).ToLongDateString();// .ToString("N2");
                gcAccountNumber = comboBox1.SelectedValue.ToString();
                textBox13.Text = gcAccountNumber;
                getLastReconDate(gcAccountNumber);
                MessageBox.Show("This is the first Issued");
                getAllTrans(gcAccountNumber);
                   MessageBox.Show("This is the first Issued");
            }
        }

        private void getAllTrans(string acctnumb)
        {
            creview.Clear();
            gnDebitReconciled = 0.00m;
            gnCreditReconciled = 0.00m;
            decimal tnDebAmt = 0.00m;
            decimal tnCredAmt = 0.00m;

            string dsql1 = "exec tsp_getBankTrans @tncompid,@tcAct";
            using (SqlConnection ndcon = new SqlConnection(cs))
            {
                ndcon.Open();
                SqlDataAdapter reconTrans = new SqlDataAdapter();
                reconTrans.SelectCommand = new SqlCommand(dsql1, ndcon);
                reconTrans.SelectCommand.Parameters.Add("@tncompid", SqlDbType.Int).Value = ncompid;
                reconTrans.SelectCommand.Parameters.Add("@tcAct", SqlDbType.VarChar).Value = acctnumb;
                reconTrans.SelectCommand.ExecuteNonQuery();
                reconTrans.Fill(creview);
                if (creview.Rows.Count > 0)
                {
                    creditGrid.AutoGenerateColumns = false;
                    creditGrid.DataSource = creview.DefaultView;
                    creditGrid.Columns[0].DataPropertyName = "lrecon";
                    creditGrid.Columns[1].DataPropertyName = "dpostdate";
                    creditGrid.Columns[2].DataPropertyName = "debitamt";
                    creditGrid.Columns[3].DataPropertyName = "creditamt";
                    creditGrid.Columns[4].DataPropertyName = "cchqno";
                    creditGrid.Columns[5].DataPropertyName = "ctrandesc";
                    creditGrid.Columns[6].DataPropertyName = "itemid";
                    ndcon.Close();
                    foreach (DataGridViewRow dgv in creditGrid.Rows)
                    {
                        if (Convert.ToBoolean(dgv.Cells["trnSelect"].Value))
                        {
                            tnDebAmt = Convert.ToDecimal(dgv.Cells["debamt"].Value);
                            tnCredAmt = Convert.ToDecimal(dgv.Cells["credamt"].Value);
                            dgv.DefaultCellStyle.BackColor = Color.DarkGreen;
                            dgv.DefaultCellStyle.ForeColor = Color.White;
                            gnDebitReconciled = gnDebitReconciled + Math.Abs(tnDebAmt);
                            gnCreditReconciled = gnCreditReconciled + Math.Abs(tnCredAmt);
                        }
                    }
                    textBox5.Text = gnDebitReconciled.ToString("N2");
                    textBox1.Text = gnCreditReconciled.ToString("N2");
                    gnFinalReconBalance = Convert.ToDecimal(gnLastReconBalance + Math.Abs(gnCreditReconciled) - Math.Abs(gnDebitReconciled));
                    textBox7.Text = gnFinalReconBalance.ToString("N2");
                    textBox12.Text = (Math.Abs(gnFinalReconBalance) - Math.Abs(gnEndingBalance)).ToString("N2");
                }
            }
        }

        private void getLastReconDate(string acctnumb)
        {
            string dsql1 = "exec tsp_getLastReconDate " + ncompid + "," + acctnumb;
            textBox3.Text = textBox4.Text = textBox5.Text = textBox1.Text = textBox7.Text = textBox12.Text =  "";
            gnCreditReconciled = gnDebitReconciled = gnEndingBalance = gnLastReconBalance = gnTotalReconciled = 0.00m;
            gnSelcount = 0;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                DataTable reconview = new DataTable();
                bnkAdp.Fill(reconview);
                if (reconview.Rows.Count > 0)
                {
                    gnLastReconBalance = Convert.ToDecimal(reconview.Rows[0]["nnewbal"]);
                    textBox3.Text = Convert.ToDecimal(reconview.Rows[0]["nnewbal"]).ToString("N2");
                    textBox4.Text = Convert.ToDateTime(reconview.Rows[0]["recon_date"]).ToLongDateString();
                    ndConnHandle.Close();
                }
            }
        }
        //private void credtrans(string acctnumb)
        //{
        //    string dsql1 = "exec tsp_getBankTrans_Credit " + ncompid+","+ acctnumb;
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        ndConnHandle.Open();
        //        SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
        //        bnkAdp.Fill(creview);
        //        if (creview.Rows.Count > 0)
        //        {
        //            creditGrid.AutoGenerateColumns = false;
        //            creditGrid.DataSource = creview.DefaultView;
        //            creditGrid.Columns[0].DataPropertyName = "lrecon";
        //            creditGrid.Columns[1].DataPropertyName = "dpostdate";
        //            creditGrid.Columns[2].DataPropertyName = "debitamt";
        //            creditGrid.Columns[3].DataPropertyName = "creditamt";
        //            creditGrid.Columns[4].DataPropertyName = "ctrandesc";
        //            ndConnHandle.Close();
        //        }
        //    }
        //}

        //private void debittrans(string acctnumb)
        //{
        //    string dsql1 = "exec tsp_getBankTrans_Debit " + ncompid + "," + acctnumb;
        //    using (SqlConnection ndConnHandle = new SqlConnection(cs))
        //    {
        //        ndConnHandle.Open();
        //        SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
        //        bnkAdp.Fill(debview);
        //        if (debview.Rows.Count > 0)
        //        {
        //            debitGrid.AutoGenerateColumns = false;
        //            debitGrid.DataSource = debview.DefaultView;
        //            debitGrid.Columns[0].DataPropertyName = "lrecon";
        //            debitGrid.Columns[1].DataPropertyName = "dpostdate";
        //            debitGrid.Columns[2].DataPropertyName = "transamt";
        //            debitGrid.Columns[3].DataPropertyName = "ctrandesc";
        //            ndConnHandle.Close();
        //            for (int i = 0; i < 6; i++)
        //            {
        //                debview.Rows.Add();
        //            }
        //        }
        //    }
        //}

        private void creditGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            creditGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void creditGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (creditGrid.Focused)
            {
                int Selcount = 0;
                decimal lnReconciled = 0.00m;
                try
                {
                    if (creditGrid.Columns[e.ColumnIndex].Name == "trnSelect")
                    {
                        bool transel = Convert.ToString(creditGrid.CurrentRow.Cells[e.ColumnIndex].Value) !="" ?  
                            Convert.ToBoolean(creditGrid.CurrentRow.Cells[e.ColumnIndex].Value) : false; 
                        string tcCode = textBox5.Text.ToString();
                        decimal tnDebAmt = Convert.ToDecimal(creditGrid.CurrentRow.Cells["debamt"].Value);
                        decimal tnCredAmt = Convert.ToDecimal(creditGrid.CurrentRow.Cells["credamt"].Value);
                        int lnItemid = Convert.ToInt32(creview.Rows[creditGrid.CurrentRow.Index]["itemid"]);
                        int erow = e.RowIndex;
                        if (transel)
                        {
                            creditGrid.Rows[erow].DefaultCellStyle.BackColor = Color.DarkGreen;
                            creditGrid.Rows[erow].DefaultCellStyle.ForeColor = Color.White;
                            gnDebitReconciled = gnDebitReconciled + Math.Abs(tnDebAmt);
                            gnCreditReconciled = gnCreditReconciled + Math.Abs(tnCredAmt);
                            updateReconcile(lnItemid, true);
                        }
                        else
                        {
                            creditGrid.Rows[erow].DefaultCellStyle.BackColor = Color.White;
                            creditGrid.Rows[erow].DefaultCellStyle.ForeColor = Color.Black;
                            gnDebitReconciled = gnDebitReconciled - Math.Abs(tnDebAmt);
                            gnCreditReconciled = gnCreditReconciled - Math.Abs(tnCredAmt);
                            updateReconcile(lnItemid, false);
                        }

                        foreach (DataGridViewRow dg in creditGrid.Rows)
                        {
                            string drowval = Convert.ToString(dg.Cells["trnSelect"].Value).ToString().Trim();
                            int cnt = Convert.ToString(dg.Cells["trnSelect"].Value).ToString().Trim() != "" ?
                                Convert.ToInt16(Convert.ToBoolean(dg.Cells["trnSelect"].Value)) : 0;
                            Selcount = Selcount + cnt;
                        }
                        textBox5.Text  = gnDebitReconciled.ToString("N2");
                        textBox1.Text  = gnCreditReconciled.ToString("N2");
                        gnFinalReconBalance = Convert.ToDecimal(gnLastReconBalance + Math.Abs(gnCreditReconciled) - Math.Abs(gnDebitReconciled));
                        textBox7.Text = gnFinalReconBalance.ToString("N2");
                        textBox12.Text = (Math.Abs(gnFinalReconBalance) - Math.Abs(gnEndingBalance)).ToString("N2");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
//                MessageBox.Show("The number of items = " +Selcount);
                AllClear2Go(Selcount);
            }
        }

        private void updateReconcile(int tnitemid,bool tlStatus)
        {
            string cglquery = string.Empty; 
            using (SqlConnection ndcon = new SqlConnection(cs))
            {
                if(tlStatus)     //select reconciliation update flag
                {
                     cglquery = "update tranhist set recupd=1 where itemid = @ntranid";
                }
                else            //deselect reconciliation update flag
                {
                     cglquery = "update tranhist set recupd = 0 where itemid = @ntranid";
                }

                SqlDataAdapter updCommand = new SqlDataAdapter();
                updCommand.UpdateCommand = new SqlCommand(cglquery, ndcon);
                updCommand.UpdateCommand.Parameters.Add("@ntranid", SqlDbType.Int).Value = tnitemid;
                
                ndcon.Open();
                updCommand.UpdateCommand.ExecuteNonQuery();
                ndcon.Close();
            }
        }
        private void AllClear2Go(int tncnt)
        {
            if(tncnt > 0)
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.Green;
                SaveButton.ForeColor = Color.White;
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.Gainsboro;
                SaveButton.ForeColor = Color.Black;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveButton.Enabled = false;
            SaveButton.BackColor = Color.Gainsboro;
            SaveButton.ForeColor = Color.Black;

            using (SqlConnection ncon = new SqlConnection(cs))
            {
                ncon.Open();
                string lcAcct = Convert.ToString(comboBox1.SelectedValue);
                string dsql1 = "update tranhist set reconciled = 1,recon_date = convert(date,getdate()) where itemid = @lnitemid";
                SqlDataAdapter recUPd = new SqlDataAdapter();

                foreach (DataGridViewRow dg in creditGrid.Rows)
                {
                    bool transel = Convert.ToString(dg.Cells["trnSelect"].Value) != "" ?
                    Convert.ToBoolean(dg.Cells["trnSelect"].Value) : false;
                    int tnItemid = Convert.ToInt32(dg.Cells["tranid"].Value);
                    if (transel)
                    {
                        recUPd.UpdateCommand = new SqlCommand(dsql1, ncon);
                        recUPd.UpdateCommand.Parameters.Add("@lnitemid", SqlDbType.Int).Value = tnItemid;
                        recUPd.UpdateCommand.ExecuteNonQuery();
                        recUPd.UpdateCommand.Parameters.Clear();
                    }
                }
                ncon.Close();
                textBox5.Text = "";
                getAllTrans(lcAcct);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getAllTrans(gcAccountNumber);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Journal djo = new Journal();
            djo.ShowDialog();
            if(comboBox1.SelectedIndex > -1)
            {
                string lcAct = Convert.ToString(comboBox1.SelectedValue);
                getAllTrans(lcAct);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tranupdate ver = new tranupdate(cs, ncompid, globalvar.cLocalCaption, globalvar.gcUserid, globalvar.gnBranchid, globalvar.gnCurrCode, globalvar.gcWorkStation, globalvar.gcWinUser);
            ver.ShowDialog();
            if (comboBox1.SelectedIndex > -1)
            {
                string lcAct = Convert.ToString(comboBox1.SelectedValue);
                getAllTrans(lcAct);
            }
        }

        private void textBox2_Validated(object sender, EventArgs e)
        {
            if(textBox2.Focused )
            {
                gnEndingBalance = Convert.ToDecimal(textBox2.Text);
                textBox2.Text = gnEndingBalance.ToString("N2");
                if(textBox7.Text.ToString().Trim()!="")
                {
                    textBox12.Text = (Math.Abs(gnFinalReconBalance) - Math.Abs(gnEndingBalance)).ToString("N2");
                }
            }
        }
    }
}
