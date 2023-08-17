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
    public partial class othcontr : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public othcontr (string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void othcontr_Load(object sender, EventArgs e)
        {
            this.Text = dloca+ "<< Deduction Type Setup >>";
            getDeduct();

        }


        public void getDeduct()
        {
            string dsql = " select cont_name,mployer,mployee,mployerpay,mployeepay,lconfirm from contrib order by cont_name";
            DataTable dtable = new DataTable();

            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    tabGrid.AutoGenerateColumns = false;
                    tabGrid.DataSource = dtable.DefaultView;
                    tabGrid.Columns[0].DataPropertyName = "cont_name";
                    tabGrid.Columns[1].DataPropertyName = "mployer";
                    tabGrid.Columns[2].DataPropertyName = "mployee";
                    tabGrid.Columns[3].DataPropertyName = "mployerpay";
                    tabGrid.Columns[4].DataPropertyName = "mployeepay";
                    tabGrid.Columns[5].DataPropertyName = "lconfirm";
                    ndConnHandle.Close();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down)
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
        }//end of onkeydown

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (textBox1.Text != "" && ((textBox2.Text.Trim()!="" && textBox3.Text.Trim()!="") || (textBox4.Text.Trim() != "" && textBox5.Text.Trim() != "")))
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string tcDesc = textBox1.Text.Trim().ToString();
            decimal mployer = (textBox2.Text.ToString() != "" ? Convert.ToDecimal(textBox2.Text.ToString()) : 0.00m);
            decimal mployee = (textBox3.Text.ToString() != "" ? Convert.ToDecimal(textBox3.Text.ToString()) : 0.00m);
            decimal mploypay =(textBox4.Text.ToString()!="" ? Convert.ToDecimal(textBox4.Text.ToString()) : 0.00m);
            decimal mployepay = (textBox5.Text.ToString() != "" ? Convert.ToDecimal(textBox5.Text.ToString()) : 0.00m); 

            int lconfirm = (checkBox1.Checked ? 1 : 0);
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
              string dsql=  "INSERT into contrib (compid,cont_name,mployer,mployee,mployerpay,mployeepay,lconfirm) values ("+ncompid+","+"'"+tcDesc+"'"+","+mployer+","+mployee+","+mploypay+","+mployepay+","+lconfirm+")";
                MessageBox.Show("going with " + dsql);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            getDeduct();
        }
    }
}
