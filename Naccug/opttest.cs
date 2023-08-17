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
//using TclassLibrary;

namespace WinTcare
{
    public partial class opttest : Form
    {
        DataTable clientview = new DataTable();
        DataTable surgview = new DataTable();
        DataTable anesview = new DataTable();
        DataTable assview = new DataTable();
        DataTable scrubview = new DataTable();
        DataTable circview = new DataTable();
        int ncompid = globalvar.gnCompid;
        string cs = globalvar.cos;
        int gnVisno = 0;
        string gcCustCode = "";
        string gcSrv_code = "";
        int gnTestID = 0;
        bool glScrubSelect = false;
        bool glCircSelect = false;
        bool glDiagSelect = false;
        
        public opttest()
        {
            InitializeComponent();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void opttest_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Operating Theatre Procedures >>";
            getclientList();
            firstclient();
            getdoctor();
        }

        private void getclientList()
        {
            string dsql = "exec tsp_OptList_All " + ncompid;
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
                    clientgrid.Columns[0].DataPropertyName = "clientcode";
                    clientgrid.Columns[1].DataPropertyName = "clname";
                    clientgrid.Columns[2].DataPropertyName = "item_name";
                    clientgrid.Columns[3].DataPropertyName = "optreason";
                    clientgrid.Columns[4].DataPropertyName = "drname";
                    clientgrid.Columns[5].DataPropertyName = "req_date";
                    clientgrid.Columns[6].DataPropertyName = "visno";
                    textBox6.Text = clientview.Rows.Count.ToString();
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
                gcCustCode = drow["clientcode"].ToString();
                gnVisno = Convert.ToInt32(drow["visno"]);// TclassLibrary.getVisitNumber.visitno(cs,ncompid.ToString(),gcCustCode);
                gcSrv_code = drow["srv_code"].ToString();
                gnTestID = Convert.ToInt32(drow["tes_id"]);
                DateTime ddob = Convert.ToDateTime(drow["ddatebirth"]);
                textBox14.Text = (Convert.ToBoolean(drow["gender"]) ? "Male" : "Female"); 
                textBox17.Text = (DateTime.Now.Year - ddob.Year).ToString();
                textBox16.Text = (((DateTime.Now - ddob).Days % 364) / 30).ToString();
                textBox15.Text = (((DateTime.Now - ddob).Days % 364) % 30).ToString();
            }
        }//firstclient

        private void getdoctor()
        {
            string docsql = "exec tsp_OptTech    " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter docadp = new SqlDataAdapter(docsql, ndConnHandle);
                docadp.Fill(surgview);
                docadp.Fill(anesview);
                docadp.Fill(assview);
                docadp.Fill(scrubview);
                docadp.Fill(circview);
                if (surgview != null)
                {
                    comboBox1.DataSource = assview;
                    comboBox1.DisplayMember = "fullname";
                    comboBox1.ValueMember = "dr_ID";
                    comboBox1.SelectedIndex = -1;

                    comboBox2.DataSource = surgview;
                    comboBox2.DisplayMember = "fullname";
                    comboBox2.ValueMember = "dr_ID";
                    comboBox2.SelectedIndex = -1;


                    comboBox3.DataSource = anesview;
                    comboBox3.DisplayMember = "fullname";
                    comboBox3.ValueMember = "dr_ID";
                    comboBox3.SelectedIndex = -1;

                    scrubGrid.AutoGenerateColumns = false;
                    scrubGrid.DataSource = scrubview.DefaultView;
                    scrubGrid.Columns[0].DataPropertyName = "fullname";
//                    scrubGrid.Columns[1].DataPropertyName = "dr_id";

                    circGrid.AutoGenerateColumns = false;
                    circGrid.DataSource = circview.DefaultView;
                    circGrid.Columns[0].DataPropertyName = "fullname";
  //                  circGrid.Columns[1].DataPropertyName = "dr_id";

                    //                    MessageBox.Show("We are here");
                }
                else
                {
                    MessageBox.Show("Operating Theatre table is empty, inform IT DEPT immediately");
                }
            }
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
            if (gcCustCode != "" && glScrubSelect && glCircSelect && glDiagSelect)
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                SaveButton.Enabled = false;
            }

        }//end of all clare
        #endregion
    }
}
