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
    public partial class updPayID : Form
    {
        string cs = string.Empty;
        int ncompid = 0;
        string cloca = string.Empty;
        string mfname = string.Empty;
        string mlname = string.Empty;
        string mcode = string.Empty;
        string gcCustCode = string.Empty;
        string gcPayrollID = string.Empty;
        DataTable custview = new DataTable();
        public updPayID(string tccos, int tncompid, string tcmemfname, string tcmemlname, string tcPayroll)
        {
            InitializeComponent();
            mfname = tcmemfname;
            mlname = tcmemlname;
            ncompid = tncompid;
            cs = tccos;
//            mcode = memcode;
        }



        private void updPayID_Load(object sender, EventArgs e)
        {
            this.Text = cloca + " << Member Payroll ID Update >> ";
            getClients();
        }


        private void getClients()
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "select ltrim(rtrim(ccustfname)) as fname,ltrim(rtrim(ccustlname)) as lname,ccustcode, '' as pay_id from cusreg ";
                cpatquery+= "where ccustfname =@tcfname and ccustlname=@tclname and compid=@tnCompid and (payroll_id is null or payroll_id = '')";
                ndConnHandle3.Open();
                SqlDataAdapter tempCommand = new SqlDataAdapter();
                tempCommand.SelectCommand = new SqlCommand(cpatquery, ndConnHandle3);

                tempCommand.SelectCommand.Parameters.Add("@tcfname", SqlDbType.VarChar).Value = mfname;
                tempCommand.SelectCommand.Parameters.Add("@tclname", SqlDbType.VarChar).Value = mlname;
                tempCommand.SelectCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;
                tempCommand.SelectCommand.ExecuteNonQuery();
                tempCommand.Fill(custview);
                if (custview.Rows.Count > 0)
                {
                    cusGrid.AutoGenerateColumns = false;
                    cusGrid.DataSource = custview.DefaultView;
                    cusGrid.Columns[0].DataPropertyName = "ccustcode";
                    cusGrid.Columns[1].DataPropertyName = "fname";
                    cusGrid.Columns[2].DataPropertyName = "lname";
                    cusGrid.Columns[3].DataPropertyName = "pay_id";
                    //clientGrid.Columns[4].DataPropertyName = "datejoin";
                }
                else
                {
                    MessageBox.Show("Member does not exist, Please add new member ");
                    this.Close();
                }

                ndConnHandle3.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  

        private void cusGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if(cusGrid.CurrentCell.ColumnIndex==3)
            {
//                string gcCustCode = string.Empty;
//              string gcPayrollID = string.Empty;

               gcPayrollID = cusGrid.CurrentCell.Value.ToString().Trim();
                gcCustCode = cusGrid.CurrentRow.Cells["dcode"].Value.ToString(); 

                if(gcPayrollID !="")
                {
                    saveButton.Enabled = true;
                    saveButton.BackColor = Color.LawnGreen;
                    saveButton.Focus();
//                    MessageBox.Show("Cell has been validated and we will update the payroll with custcode = "+gcCustCode +" and payroll = "+gcPayrollID);
                }
                else
                {
                    saveButton.Enabled = false;
                    saveButton.BackColor = Color.Gainsboro;
                }
            }
        }


        private void updateNow(string tcCode,string tcPayroll)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatquery = "update cusreg set payroll_id = @tcPayRoll where ccustcode = @tcustcode";
                ndConnHandle3.Open();
                SqlDataAdapter tempCommand = new SqlDataAdapter();
                tempCommand.UpdateCommand  = new SqlCommand(cpatquery, ndConnHandle3);

                tempCommand.UpdateCommand.Parameters.Add("@tcPayRoll", SqlDbType.VarChar).Value = tcPayroll;
                tempCommand.UpdateCommand.Parameters.Add("@tcustcode", SqlDbType.VarChar).Value = tcCode;
                tempCommand.UpdateCommand.Parameters.Add("@tnCompid", SqlDbType.Int).Value = ncompid;

                tempCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
                MessageBox.Show("Cusreg has been updated successfully");
            }
        }



        private void cusGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if(cusGrid.CurrentCell.ColumnIndex == 3)
            {
                cusGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            updateNow(gcCustCode,gcPayrollID);
            this.Close();
        }
    }
}
