using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;



namespace TclassLibrary
{
    public partial class onecode : Form
    {
        string cs = "";
        string tcDbf = "";
        string tcField = "";
        public onecode(string cos, string cCaption, string ddbf, string dfield)             //string cs, int ncompid, string tcDbf, string tcField)
        {
            InitializeComponent();
            cs = cos;
            tcDbf = ddbf;
            tcField = dfield;
            this.Text = cCaption;
            //          MessageBox.Show("we are inside twocode with cos, dbf, field, dcompid "+cs+","+tcDbf+","+tcField+","+dcompid);
        }

        private void onecode_Load(object sender, EventArgs e)
        {
            doform();
            textBox1.Focus();
        }
        public void doform()
        {
            string dsql = "Select " + tcField + " from " + tcDbf + " order by " + tcField;
            DataTable dtable = new DataTable();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    //               MessageBox.Show("We have found "+dtable.Rows.Count+" records");
                    tabGrid.AutoGenerateColumns = false;
                    tabGrid.DataSource = dtable.DefaultView;
                    tabGrid.Columns[0].DataPropertyName = tcField;// dtable.Rows[0].ToString();        // "fname";
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
                string dsql1 = "insert into " + tcDbf + " (" + tcField + ")" + " values (" + "'" + tcDesc + "'" + ")";
        //        MessageBox.Show("we will be using " + dsql1);

                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            doform();
        }// end of saveButton  
    }
}
