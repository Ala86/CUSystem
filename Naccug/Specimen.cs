using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;
using System.Data.SqlClient;

namespace WinTcare
{
    public partial class Specimen : Form
    {
        string cs = globalvar.cos;
        DataTable clientview = new DataTable();
        DataTable specview = new DataTable();
        DataTable docview = new DataTable();
        //        string gcDrawnBy = "";
        string gcSrvCode = "";
        int gnRecBy = 0;
        int gnTestID = 0;
        int ncounter = 0;

        bool glSpecSelect = false;
        string gcCustCode = "";
        string ncompid = globalvar.gnCompid.ToString().Trim();
        public Specimen()
        {
            InitializeComponent();
        }


        private void Specimen_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Specimen Collection >>";
            getclientList();
            firstclient();
            getdoctor();
            //            comboBox1.SelectedIndex = -1;
        }

        private void getclientList()
        {
            string dsql = "exec tsp_SpecList_All  " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    clientgrid.AutoGenerateColumns = false;
                    clientgrid.DataSource = clientview.DefaultView;
                    clientgrid.Columns[0].DataPropertyName = "ccustcode";
                    clientgrid.Columns[1].DataPropertyName = "clname";
                    clientgrid.Columns[2].DataPropertyName = "item_name";
                    clientgrid.Columns[3].DataPropertyName = "labreason";
                    clientgrid.Columns[4].DataPropertyName = "drname";
                    clientgrid.Columns[5].DataPropertyName = "req_date";
                    ndConnHandle.Close();
                    //          firstclient();
                    clientgrid.Focus();
                }
            }
        }

        private void firstclient()
        {
            if (clientgrid.Rows.Count > 0)
            {
                string srvcode = clientview.Rows[0]["srv_code"].ToString();
                string tcCode = clientview.Rows[0]["ccustcode"].ToString();
                gcSrvCode = clientview.Rows[0]["srv_code"].ToString();
                gcCustCode = clientview.Rows[0]["ccustcode"].ToString(); ;
                if (checkspecs(srvcode))
                {
                    getspecs(tcCode, srvcode);
                }
            }
        }

        private void getdoctor()
        {
            string docsql = "exec tsp_LabTech   " + ncompid;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter docadp = new SqlDataAdapter(docsql, ndConnHandle);
                docadp.Fill(docview);
                if (docview != null)
                {
                    comboBox1.DataSource = docview;
                    comboBox1.DisplayMember = "fullname";
                    comboBox1.ValueMember = "dr_ID";
                    comboBox1.SelectedIndex = -1;
                    //                    MessageBox.Show("We are here");
                }
                else
                {
                    MessageBox.Show("Laboratory Technicians table is empty, inform IT DEPT immediately");
                }
            }
        }

        private bool checkspecs(string tcSrvCode)
        {
            string dsql = "exec tsp_CheckSpecimen  " + ncompid + "," + "'" + tcSrvCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da1 = new SqlDataAdapter(dsql, ndConnHandle);
                da1.Fill(clientview);
                if (clientview.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Specimens have not been defined for this service, inform IT DEPT immediately");
                    return false;
                }
            }
        }


        private void getspecs(string tcCustCode, string tcSrvCode)
        {
            string dsql = "exec tsp_LabSpecimen_One " + ncompid + "," + "'" + tcCustCode + "'" + "," + "'" + tcSrvCode + "'";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter daspec = new SqlDataAdapter(dsql, ndConnHandle);
                daspec.Fill(specview);
                if (specview.Rows.Count > 0)
                {
                    gnTestID = Convert.ToInt32(specview.Rows[0]["tes_id"]);
                    //                    MessageBox.Show("Test id is " + gnTestID);
                    specGrid.AutoGenerateColumns = false;
                    specGrid.DataSource = specview.DefaultView;
                    specGrid.Columns[0].DataPropertyName = "item_name";
                    specGrid.Columns[1].DataPropertyName = "spe_name";
                    ndConnHandle.Close();
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void clientgrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //   MessageBox.Show("inside cell content cliek");
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
            if (gcCustCode != "" && Convert.ToInt16(comboBox1.SelectedValue) > 0 && glSpecSelect)
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
                SaveButton.Select();
            }
            else
            {
                SaveButton.Enabled = false;
            }

        }
        #endregion

        private void specGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            specGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void specGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (specGrid.Columns[e.ColumnIndex].Name == "testSelect")
            {
                if (Convert.ToBoolean(specGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    glSpecSelect = true;
                    AllClear2Go();
                }
                else
                {
                    glSpecSelect = false;
                    AllClear2Go();
                }
            }

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //              AllClear2Go();
            //            gcDrawnBy = LabTechView.fullname
            //            gnRecBy = Convert.ToInt32(comboBox1.SelectedValue);
            //          MessageBox.Show("Received by " + gnRecBy);
        }

        private void updaterecby()
        {
            string cs1 = globalvar.cos;
            string tcUserid = globalvar.gcUserid;
            string tcReceipt = genreceipt.getreceipt(cs1, globalvar.gdSysDate);
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs1))
            {
                string cpatquery = "update labtestitems set rec_date=convert(date,getdate()),  rec_time=convert(time,getdate()), rec_by = @nRecBy,lspecimen=1 where tes_id=@nTestID AND srv_code= @srvcode";
                if (specview.Rows.Count > 0)
                {
                    for (int i = 0; i < specview.Rows.Count; i++)
                    {
                        SqlDataAdapter itemsupd = new SqlDataAdapter();
                        itemsupd.UpdateCommand = new SqlCommand(cpatquery, ndConnHandle3);
                        itemsupd.UpdateCommand.Parameters.Add("@nRecBy", SqlDbType.Int).Value = gnRecBy;
                        itemsupd.UpdateCommand.Parameters.Add("@nTestID", SqlDbType.Int).Value = gnTestID;
                        itemsupd.UpdateCommand.Parameters.Add("@srvcode", SqlDbType.Char).Value = gcSrvCode;
                        ndConnHandle3.Open();
                        itemsupd.UpdateCommand.ExecuteNonQuery();
                        ndConnHandle3.Close();
                    }
                }
            }
        }
        private void printreceipt()
        {
            /*
             gcReceiptHash=''
            Select specRecs
            Set Filter To lrec=1
            Locate
            Report Form "new_specimen.frx" To Print Noconsole
                         */
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            gnRecBy = Convert.ToInt32(comboBox1.SelectedValue);
            updaterecby();
            printreceipt();
            clientview.Clear();
            gcCustCode = "";
            glSpecSelect = false;
            SaveButton.Enabled = false;
            SaveButton.BackColor = Color.LightGray;// .ligghtg\\ .Gray;
//            AllClear2Go();
            getclientList();
            firstclient();

      //      MessageBox.Show("after all we are done");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dselval = Convert.ToString(comboBox1.SelectedValue).Trim();
            if (dselval != "System.Data.DataRowView" && dselval != "")
            {
//                MessageBox.Show("inside selected index changed");
                AllClear2Go();
            }
        }

    }
}

