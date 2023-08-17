using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WinTcare
{
    public partial class bioDetails : Form
    {
        public bioDetails()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bioDetails_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Employee Bio Details >>";
            this.label36.Text = globalvar.gcCopyRight;
            string cs = globalvar.cos;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                string dsql = "exec tsp_getActiveStaff "+ncompid;
                SqlDataAdapter da = new SqlDataAdapter(dsql,ndConnHandle);
                DataSet ds = new DataSet();
                da.Fill(ds, "staff");
                ClientGrid.AutoGenerateColumns = false;
                ClientGrid.DataSource = ds.Tables["staff"].DefaultView ;
                ClientGrid.Columns[0].DataPropertyName = "staffno";
                ClientGrid.Columns[1].DataPropertyName = "fullname";
                ClientGrid.Columns[2].DataPropertyName = "dep_name";
                ClientGrid.Columns[3].DataPropertyName = "des_name";
                ClientGrid.Columns[4].DataPropertyName = "dage";
                ClientGrid.Columns[5].DataPropertyName = "dgender";
                ndConnHandle.Close();
            }
        }

        private void ClientGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("You have clicked in the grid");
        }
    }
}
//exec tsp_OutstandingReceipts ?gnCompid,tsp_getAtvstaff 24
/*
SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Student", "server = MCNDESKTOP33; Database = Avinash; UID = sa; password = *******");  
DataSet ds = new DataSet();
da.Fill(ds, "Student");  
dataGridView1.DataSource = ds.Tables["Student"].DefaultView;  */