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
    public partial class tbscreen : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        DataTable tbView = new DataTable();
        string tcCustCode = "";
        string firstname = "";
        string midname = "";
        string lastname = "";
        int gnVisno = 0;
        bool glSelectionMade = false;
        int gnyesdone = 0;

        public tbscreen(string tcCode, string fname, string mname, string lname)
        {
            InitializeComponent();
            tcCustCode = tcCode;
            firstname = fname;
            midname = mname;
            lastname = lname;

        }

        private void tbscreen_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< TB Screening Items >>";
            getSpecList();
            textBox1.Text = tcCustCode;
            textBox2.Text = firstname;
            textBox3.Text = midname;
            textBox4.Text = lastname;
        }

        private void getSpecList()
        {
            string dsql = "exec tsp_GetTBScreenItems";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                da.Fill(tbView); //MessageBox.Show("You have access );
                if (tbView.Rows.Count > 0)
                {
                    tbGrid.AutoGenerateColumns = false;
                    tbGrid.DataSource = tbView.DefaultView;
                    tbGrid.Columns[0].DataPropertyName = "tb_name";
                    tbGrid.Columns[1].DataPropertyName = "yesdone";
                    tbGrid.Columns[2].DataPropertyName = "notdone";
                    tbGrid.Columns[3].DataPropertyName = "tb_id";
                    ndConnHandle.Close();
                    tbGrid.Focus();
               //     for (int i = 0; i < 2; i++)
                 //   {
                        //                        DataGridViewRow drow = new  DataGridViewRow();
                   //     tbView.Rows.Add();
                   // }
                }
            }
        }//end of getspeclist

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
//            MessageBox.Show("We will update the selected tb items");
            //gnVisno = getVisitNumber.visitno(cs, ncompid.ToString(), tcCustCode);
            string cquery = "insert into tbResults (tb_id,tb_done,ccustcode,compid,visno,screendate) values (@ntb_id,1,@tcCode,@gnCompid,@nVisno,convert(date,getdate()))";
            SqlDataAdapter tbins = new SqlDataAdapter();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                tbins.InsertCommand = new SqlCommand(cquery, ndConnHandle);
                foreach (DataGridViewRow drow in tbGrid.Rows)
                {
                    if (drow.Cells["yesClick"].Value != null && Convert.ToBoolean(drow.Cells["yesClick"].Value) == true)
                    {
                        int lnID = (drow.Cells["tbid"].Value != null && Convert.ToInt32(drow.Cells["tbid"].Value) > 0 ? Convert.ToInt32(drow.Cells["tbid"].Value) : 0);
                        tbins.InsertCommand.Parameters.Add("@ntb_id", SqlDbType.Int).Value = lnID;
                        tbins.InsertCommand.Parameters.Add("@tcCode", SqlDbType.Char).Value = tcCustCode;
                        tbins.InsertCommand.Parameters.Add("@gnCompid", SqlDbType.Int).Value = ncompid;
                        tbins.InsertCommand.Parameters.Add("@nVisno", SqlDbType.Int).Value = gnVisno;
                        ndConnHandle.Open();
                        tbins.InsertCommand.ExecuteNonQuery();
                        tbins.InsertCommand.Parameters.Clear();
                        ndConnHandle.Close();
                    }
                }
            }
            Close();
        }//end of savebutton


        private void tbGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(tbGrid.Columns[e.ColumnIndex].Name =="yesClick")
            {
                if (Convert.ToBoolean(tbGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    gnyesdone++;
                    tbGrid.Rows[e.RowIndex].Cells["noClick"].Value = false;
                    if(gnyesdone>0)
                    {
                        SaveButton.Enabled = true;
                        SaveButton.BackColor = Color.LawnGreen;
                    }
                }
            }

            if (tbGrid.Columns[e.ColumnIndex].Name == "noClick")
            {
                if (Convert.ToBoolean(tbGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    gnyesdone--;
                    tbGrid.Rows[e.RowIndex].Cells["yesClick"].Value = false;
                    if (gnyesdone ==0)
                    {
                        SaveButton.Enabled = false ;
                        SaveButton.BackColor = Color.Gainsboro;
                    }
                }
            }
        }

        private void AllClear2Go()
        {
            DataView dviu = new DataView(tbView);
            dviu.RowFilter = "yesdone = true";
            int dcount = dviu.Count;
            MessageBox.Show("The count in all clear 2 go  is " + dcount);
/*            if (dviu.Count  textBox12.Text.Trim() != "" && maskedTextBox1.Text.ToString().Trim() != "" && maskedTextBox2.Text.Replace(".", "").Trim() != "")
            {
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
                //                MessageBox.Show("masked 1 " + maskedTextBox2.Text.ToString().Trim());
                //                SaveButton.Select();
            }
            else
            {
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.FromArgb(224, 224, 224);        // Color.Red;
            }*/
        }

        private void tbGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
       //     MessageBox.Show("current cell dirty ");
//            tbGrid.CellEndEdit().t;
            tbGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void tbGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
    //        MessageBox.Show("cell leave");
        }

    
    }
}
