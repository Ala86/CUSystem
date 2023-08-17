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
    public partial class opdDischarge : Form
    {
        DataTable clientview = new DataTable();
        DataTable visitview = new DataTable();
        string cs = globalvar.cos;
        string ncompid = globalvar.gnCompid.ToString().Trim();
        int gnVisno = 0;
        string gcCustCode = "";
        bool glCanDischarge=false;

        public opdDischarge()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void opdDischarge_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Out-Patient Client Discharge >>";
                getclientList();
                firstclient();
        }

        private void getclientList()
        {
            string dsql = "exec [tsp_Ready2Discharge]  " + ncompid;
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
                    clientgrid.Columns[0].DataPropertyName = "fname";
                    clientgrid.Columns[1].DataPropertyName = "mname";
                    clientgrid.Columns[2].DataPropertyName = "lname";
                    clientgrid.Columns[3].DataPropertyName = "visdate";      // "age";
                    clientgrid.Columns[4].DataPropertyName = "vistime";
                    clientgrid.Columns[5].DataPropertyName = "visno";
                    ndConnHandle.Close();
                    clientgrid.Focus();
                    for (int i = 0; i < 10; i++)
                    {
                        clientview.Rows.Add();
                    }
                }
            }
        }//end of getclientlist

        private void firstclient()
        {
            if (clientview.Rows.Count > 0)
            {
                DataRow drow = clientview.Rows[clientgrid.CurrentCell.RowIndex];
                textBox6.Text = drow["ccustcode"].ToString();
                textBox16.Text = Convert.ToDateTime(drow["triagedate"]).ToLongDateString();
                textBox8.Text = drow["triagetime"].ToString();
                textBox15.Text = Convert.ToDateTime(drow["consdate"]).ToLongDateString();
                textBox5.Text = drow["constime"].ToString();

                gnVisno = Convert.ToInt16(drow["visno"]);
                gcCustCode = textBox6.Text;
                visitdetails(gcCustCode);
            }
        }//firstclient

        private void visitdetails(string cust)
        {
            string dsql1 = "select * from pat_visit where ccustcode=" + "'"+cust+"'"+" and activesession =1 and larchived=0 and compid="+ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql1, ndConnHandle);
                da.Fill(visitview);
                if (visitview.Rows.Count > 0)
                {
                    textBox16.Text = Convert.ToDateTime(visitview.Rows[0]["triagedate"]).ToLongDateString();
                    textBox8.Text = Convert.ToString(visitview.Rows[0]["triagetime"]);
                    textBox15.Text = Convert.ToDateTime(visitview.Rows[0]["consdate"]).ToLongDateString();
                    textBox5.Text =Convert.ToString( visitview.Rows[0]["constime"]);
                    SaveButton.Enabled = true;
                    SaveButton.BackColor = Color.LawnGreen;
                }
            }
        }// end of visitdetails

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
//            MessageBox.Show("Lazy you !");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to discharge client", "Client Discharge Check", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                //            if (MessageBox.Show("Are you sure you want to Discharge Client","Client Discharge",MessageBoxButtons.YesNo,MessageBoxDefaultButton.Button2,MessageBoxIcon.Question)==DialogResult.Yes)
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                {
                    string cdisquery = "update pat_visit set activesession = 0,discdate = convert(date, getdate()), disctime = convert(time, getdate()) where ccustcode =@cCustCode and activesession = 1";
  //                  string cdisquery1 = "delete from todayvisit where ccustcode=@cCustCode";
    //                string cdisquery2 = "delete from todayhist where ccustcode=@cCustCode";
      //              string cdisquery3 = "delete from todaypatients where ccustcode=@cCustCode";

                    SqlDataAdapter updpat = new SqlDataAdapter();
                    updpat.UpdateCommand = new SqlCommand(cdisquery, ndConnHandle3);
                    updpat.UpdateCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;

        //            SqlDataAdapter delvis = new SqlDataAdapter();
          //          delvis.DeleteCommand = new SqlCommand(cdisquery1, ndConnHandle3);
            //        delvis.DeleteCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;

              //      SqlDataAdapter delhis = new SqlDataAdapter();
                //    delhis.DeleteCommand = new SqlCommand(cdisquery2, ndConnHandle3);
                  //  delhis.DeleteCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;

               //     SqlDataAdapter delpat = new SqlDataAdapter();
                 //   delpat.DeleteCommand = new SqlCommand(cdisquery3, ndConnHandle3);
                   // delpat.DeleteCommand.Parameters.Add("@cCustCode", SqlDbType.Char).Value = gcCustCode;

                    ndConnHandle3.Open();
                    updpat.UpdateCommand.ExecuteNonQuery();
                    //   delvis.DeleteCommand.ExecuteNonQuery();
                    //   delhis.DeleteCommand.ExecuteNonQuery();
                    //   delpat.DeleteCommand.ExecuteNonQuery();
                    deletetables dtabledel = new deletetables();
                    dtabledel.zaptempfiles(cs, "todayvisit", gcCustCode);
                    dtabledel.zaptempfiles(cs, "todayhist", gcCustCode);
                    dtabledel.zaptempfiles(cs, "todaypats", gcCustCode);

                    textBox5.Text = "";
                    textBox8.Text = "";
                    textBox15.Text = "";
                    textBox16.Text = "";
                    textBox6.Text = "";
                    checkBox1.Checked = false;
//                    SaveButton.Enabled = false;
  //                  SaveButton.BackColor = Color.Gainsboro;
                    getclientList();
                    firstclient();
                }
            }
        }// end of savebutton

        private void clientgrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clientgrid.EndEdit();
            if(clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString()!="")
            {
                textBox6.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString();
                textBox6.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString();
                textBox16.Text = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["triagedate"]).ToLongDateString();
                textBox8.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["triagetime"].ToString();
                textBox15.Text = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["consdate"]).ToLongDateString();
                textBox5.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["constime"].ToString();
            }
        }

        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /*MessageBox.Show("cell content click");
            clientgrid.EndEdit();
            textBox6.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString();
            textBox6.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString();
            textBox16.Text = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["triagedate"]).ToLongDateString();
            textBox8.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["triagetime"].ToString();
            textBox15.Text = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["consdate"]).ToLongDateString();
            textBox5.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["constime"].ToString();
            */
        }

        private void clientgrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*MessageBox.Show("key press");
            clientgrid.EndEdit();
            textBox6.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString();
            textBox6.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["ccustcode"].ToString();
            textBox16.Text = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["triagedate"]).ToLongDateString();
            textBox8.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["triagetime"].ToString();
            textBox15.Text = Convert.ToDateTime(clientview.Rows[clientgrid.CurrentCell.RowIndex]["consdate"]).ToLongDateString();
            textBox5.Text = clientview.Rows[clientgrid.CurrentCell.RowIndex]["constime"].ToString();
            */
        }

        private void clientgrid_CellClick(object sender, KeyPressEventArgs e)
        {

        }
    }
}
