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
    public partial class cashMgrcs : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable cashview = new DataTable();
        string duser = globalvar.gcUserid;
        string gcPostAcct = string.Empty;
        string gcContAcct = string.Empty;
        decimal gnTranAmt = 0.00m;
        bool glTillBal = false;
        public cashMgrcs()
        {
            InitializeComponent();
        }



        private void cashMgrcs_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Member Deposit >>";
            getMainCash(duser);
            getCashiers();
            cashGrid.Columns["curbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            cashGrid.Columns["curbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            cashGrid.Columns["tillamt"].SortMode = DataGridViewColumnSortMode.NotSortable;
            cashGrid.Columns["tillamt"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            cashGrid.Columns["endbal"].SortMode = DataGridViewColumnSortMode.NotSortable;
            cashGrid.Columns["endbal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            textBox4.Text = gnTranAmt.ToString("N2");
            textBox5.Text = gnTranAmt.ToString("N2");
        }


        private void getMainCash(string userid)
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************checking if user is a main cashier               
                string dsql1r = " exec tsp_getMainCashiers " + ncompid + ",'" + userid + "'";
                SqlDataAdapter da1r = new SqlDataAdapter(dsql1r, ndConnHandle1);
                DataTable manview = new DataTable();
                da1r.Fill(manview);
                if (manview != null)
                {
                    textBox1.Text = manview.Rows[0]["cacctname"].ToString();
                    textBox2.Text = manview.Rows[0]["cacctnumb"].ToString();
                    textBox3.Text = Convert.ToDecimal(manview.Rows[0]["nbookbal"]).ToString("N2");
                    ndConnHandle1.Close();
                }
            }
        }


        private void getCashiers()
        {
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                //************Getting accounts                
                                decimal curbal = 0.00m;
                 string casSql = " exec tsp_getCashiers " + ncompid;
                SqlDataAdapter dalCas = new SqlDataAdapter(casSql, ndConnHandle1);
                dalCas.Fill(cashview);
                if (cashview != null)
                {
                    cashGrid.AutoGenerateColumns = false;
                    cashGrid.DataSource = cashview.DefaultView;
                    cashGrid.Columns[0].DataPropertyName = "username";
                    cashGrid.Columns[1].DataPropertyName = "cacctnumb";
                    cashGrid.Columns[2].DataPropertyName = "cacctname";
                    cashGrid.Columns[3].DataPropertyName = "nbookbal";
                    for(int j=0;j<cashview.Rows.Count;j++)
                    {
                      decimal bkbal = Convert.ToDecimal(cashview.Rows[j]["nbookbal"]);
                      curbal = curbal + bkbal; 
                    }
                     textBox6.Text = curbal.ToString("N2");
                }
                ndConnHandle1.Close();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cashGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("inside cell validated");
   /*         if (cashGrid.Columns[e.ColumnIndex].Name == "tillamt" && cashGrid.CurrentRow.Cells["cname"].Value.ToString() != "")
            {
                decimal currAmount = Convert.ToDecimal(cashGrid.CurrentRow.Cells["curbal"].Value);
                decimal tillAmount = -Math.Abs(Convert.ToDecimal(cashGrid.CurrentRow.Cells["tillamt"].Value));

                cashGrid.CurrentRow.Cells["tillamt"].Value = tillAmount.ToString("N2");
                decimal endbal = currAmount + tillAmount;
                cashGrid.CurrentRow.Cells["endbal"].Value = endbal.ToString("N2");// (currAmount + tillAmount).ToString("N2");

                textBox4.Text = (Convert.ToDecimal(textBox4.Text) + tillAmount).ToString("N2");
                textBox5.Text = (Convert.ToDecimal(textBox5.Text) + endbal).ToString("N2");
            }
            else
            {
                cashGrid.CurrentRow.Cells["tillamt"].Value = 0.00m.ToString("N2");
            }
            glTillBal = Convert.ToDecimal(textBox4.Text) > 0.00m ? true : false;
            MessageBox.Show("before all clear go ");
//            AllClear2Go();
*/
        }
    }
}
