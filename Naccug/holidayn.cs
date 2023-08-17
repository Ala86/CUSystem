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
using System.Globalization;

namespace WinTcare
{
    public partial class holidayn : Form
    {
        string dcos = globalvar.cos;
        CultureInfo culture = new CultureInfo("en-GB");
//        public string ToMyString(string format)
  //      {
    //        return DateTime.ParseExact(format,"dd/MM/yyyy HH:mm:ss",CultureInfoConverter.StandardValuesCollection); 
      //  }
        public holidayn()
        {
            InitializeComponent();
        }

        private void holidayn_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Holiday Setup >>";
            getHoli();
        }

        public void getHoli()
        {
            string dsql = "select hol_name,fromdate,todate,holitype FROM holiday order by fromdate";
            DataTable dtable = new DataTable();
            string dt1 = DateTime.Parse(DateTime.Now.ToString(), CultureInfo.GetCultureInfo("en-GB")).ToString();
            MessageBox.Show("The date is " +dt1); 

            using (SqlConnection ndConnHandle = new SqlConnection(dcos))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    tabGrid.AutoGenerateColumns = false;
                    tabGrid.DataSource = dtable.DefaultView;
                    tabGrid.Columns[0].DataPropertyName = "hol_name";
                    tabGrid.Columns[1].DataPropertyName = "holitype";
                    tabGrid.Columns[2].DataPropertyName = "fromdate";
                    tabGrid.Columns[3].DataPropertyName = "todate";
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
            string date1 = dateTimePicker1.Text;
            string date2 = dateTimePicker2.Text;
            //            string newpass = UserPassWord.Text.ToUpper();
            if (textBox1.Text != "" && date1!="" && date2!="")
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string tcDesc = textBox1.Text.Trim().ToString();
//            DateTime date1 = Convert.ToDateTime(dateTimePicker1.Value.ToShortDateString());
  //          DateTime date2 = Convert.ToDateTime(dateTimePicker2.Value.ToShortDateString());
            DateTime date1 = dateTimePicker1.Value.Date;
            DateTime date2 = dateTimePicker2.Value.Date;
            using (SqlConnection ndConnHandle1 = new SqlConnection(dcos))
            {
                ndConnHandle1.Open();
//                "Insert Into holiday (hol_name,fromdate,todate) values (?.text2.value,?.text3.value,?.text4.value)", "holinsert")
//                string dsql1 = "Insert Into apraisal (appr_name,maxscore,compid) values (" + "'" + textBox1.Text.Trim().ToString() + "'" + "," + textBox2.Text.Trim().ToString() + "," + ncompid + ")";
                string dsql1 = "Insert Into holiday (hol_name,fromdate,todate,holitype) values (" + "'" + textBox1.Text.Trim().ToString() + "'" + "," +"'"+date1+"'" + "," +"'"+date2+"'"+","+1+ ")";
  //              MessageBox.Show("we will be using " + dsql1);
                SqlDataAdapter medcommand = new SqlDataAdapter();
                medcommand.InsertCommand = new SqlCommand(dsql1, ndConnHandle1);
                medcommand.InsertCommand.ExecuteNonQuery();
            }
            saveButton.Enabled = false;
            saveButton.BackColor = Color.Gainsboro;
            textBox1.Text = "";
            getHoli();
        }
    }
}
