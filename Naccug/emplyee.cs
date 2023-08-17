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
    public partial class emplyee : Form
    {
        public emplyee()
        {
            InitializeComponent();
        }

        private void emplyee_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Employee basic Data >>";
            label115.Text = globalvar.gcCopyRight;
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

        private void tabPage13_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
