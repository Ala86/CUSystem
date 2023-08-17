using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace TclassLibrary
{
    public partial class twocode : Form
    {
        string cs = "";
        string tcDbf = "";
        string tcField = "";
        int ncompid = 0;
        public twocode(string cos, string cCaption, string ddbf, string dfield, int dcompid)             //string cs, int ncompid, string tcDbf, string tcField)
        {
            InitializeComponent();
            cs = cos;
            tcDbf = ddbf;
            tcField = dfield;
            ncompid = dcompid;
            this.Text = cCaption;
        }

        private void twocode_Load(object sender, EventArgs e)
        {
            doform();
            textBox1.Focus();
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
            if (textBox1.Text != "")
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
        public void doform()
        {
            string dsql = "Select " + tcField + " from " + tcDbf + " where compid = " + ncompid + " order by " + tcField;
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
                    tabGrid.Columns[0].DataPropertyName = tcField; 
                    ndConnHandle.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string tcDesc = textBox1.Text.Trim().ToString();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                ndConnHandle1.Open();
                string dsql1 = "insert into "+tcDbf+" ("+tcField+", compid)"+" values ("+"'"+tcDesc+"'"+","+ncompid+")";
//                MessageBox.Show("we will be using " + dsql1);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            doform();
        }// end of savebutton
    }
}
