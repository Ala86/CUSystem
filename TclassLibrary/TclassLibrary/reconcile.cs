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

namespace TclassLibrary 
{
    public partial class reconcile : Form
    {
        DataTable bnkview = new DataTable();
        DataTable debview = new DataTable();
        DataTable creview = new DataTable();
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public reconcile(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void reconcile_Load(object sender, EventArgs e)
        {
            this.Text = dloca + "<< Account Reconciliation >>";
            BnkAccts();
            creditGrid.Columns["credamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            creditGrid.Columns["credamt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            creditGrid.Columns["debamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
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
                textBox2.Text = Convert.ToDecimal(bnkview.Rows[Convert.ToInt32(comboBox1.SelectedIndex)]["nbookbal"]).ToString("N2");
                textBox4.Text = Convert.ToDateTime(bnkview.Rows[Convert.ToInt32(comboBox1.SelectedIndex)]["lrecdate"]).ToLongDateString();// .ToString("N2");
                string act = comboBox1.SelectedValue.ToString();
                credtrans(act);
//                debittrans(act);
            }
        }

        private void credtrans(string acctnumb)
        {
            string dsql1 = "exec tsp_getBankTrans " + ncompid+","+ acctnumb;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(creview);
                if (creview.Rows.Count > 0)
                {
                    creditGrid.AutoGenerateColumns = false;
                    creditGrid.DataSource = creview.DefaultView;
                    creditGrid.Columns[0].DataPropertyName = "lrecon";
                    creditGrid.Columns[1].DataPropertyName = "dpostdate";
                    creditGrid.Columns[2].DataPropertyName = "debitamt";
                    creditGrid.Columns[3].DataPropertyName = "creditamt";
                    creditGrid.Columns[4].DataPropertyName = "ctrandesc";
                    ndConnHandle.Close();
//                    for (int i = 0; i < 6; i++)
  //                  {
    //                    creview.Rows.Add();
      //              }
                }
            }
        }

 /*       private void debittrans(string acctnumb)
        {
            string dsql1 = "exec tsp_getBankTrans_Debit " + ncompid + "," + acctnumb;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter bnkAdp = new SqlDataAdapter(dsql1, ndConnHandle);
                bnkAdp.Fill(debview);
                if (debview.Rows.Count > 0)
                {
                    debitGrid.AutoGenerateColumns = false;
                    debitGrid.DataSource = debview.DefaultView;
                    debitGrid.Columns[0].DataPropertyName = "lrecon";
                    debitGrid.Columns[1].DataPropertyName = "dpostdate";
                    debitGrid.Columns[2].DataPropertyName = "transamt";
                    debitGrid.Columns[3].DataPropertyName = "ctrandesc";
                    ndConnHandle.Close();
                    for (int i = 0; i < 6; i++)
                    {
                        debview.Rows.Add();
                    }
                }
            }
        }*/
        private void debittrans(string acctnumb,bool crdr)
        {

        }
    }
}
