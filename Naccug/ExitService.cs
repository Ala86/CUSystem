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
    public partial class ExitService : Form
    {
        public ExitService()
        {
            InitializeComponent();
        }

        private void ExitService_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Exit from Service >>";
            label22.Text = globalvar.gcCopyRight;
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveStaff " + ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle);
                DataSet ds = new DataSet();
                da.Fill(ds, "staff");
                ClientGrid.AutoGenerateColumns = false;
                ClientGrid.DataSource = ds.Tables["staff"].DefaultView;
                ClientGrid.Columns[0].DataPropertyName = "staffno";
                ClientGrid.Columns[1].DataPropertyName = "fullname";
                ClientGrid.Columns[2].DataPropertyName = "dep_name";
                ClientGrid.Columns[3].DataPropertyName = "des_name";
                ClientGrid.Columns[4].DataPropertyName = "dage";
                ClientGrid.Columns[5].DataPropertyName = "dgender";
                ndConnHandle.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
