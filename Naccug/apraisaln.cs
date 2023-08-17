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

namespace WinTcare
{
    public partial class apraisaln : Form
    {
        string dcos = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        public apraisaln()
        {
            InitializeComponent();
        }

        private void apraisaln_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Appraisal Item Setup >>";
            getAppr(); 
        }

        public void getAppr()
        {
            string dsql = "select appr_name,maxscore from apraisal order by appr_name" ;
            DataTable dtable = new DataTable();
            using (SqlConnection ndConnHandle = new SqlConnection(dcos))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    tabGrid.AutoGenerateColumns = false;
                    tabGrid.DataSource = dtable.DefaultView;
                    tabGrid.Columns[0].DataPropertyName = "appr_name";
                    tabGrid.Columns[1].DataPropertyName = "maxscore";
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
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            //            string newpass = UserPassWord.Text.ToUpper();
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
                saveButton.Select();
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
                //*            MessageBox.Show("Invalid User or Password");
            }

        }
        #endregion 
        private void saveButton_Click(object sender, EventArgs e)
        {
            string tcDesc = textBox1.Text.Trim().ToString();
            using (SqlConnection ndConnHandle1 = new SqlConnection(dcos))
            {
                ndConnHandle1.Open();
                string dsql1 = "Insert Into apraisal (appr_name,maxscore,compid) values ("+"'"+textBox1.Text.Trim().ToString()+"'"+","+textBox2.Text.Trim().ToString()+","+ncompid+")";
                 MessageBox.Show("we will be using " + dsql1);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            textBox2.Text = "";
            getAppr();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
