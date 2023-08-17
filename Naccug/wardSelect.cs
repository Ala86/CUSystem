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
    public partial class wardSelect : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        int gnWardID = 0;
        public wardSelect()
        {
            InitializeComponent();
        }

        private void wardSelect_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Ward Selection >>";
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                /*
                 sn=SQLExec(gnConnHandle,"exec tsp_hwards_All ?gnCompid","wardSelectView")
If !(sn>0 And Reccount()>0)
	=sysmsg('No Wards Found')
Endif
wardSelectView.hwa_name,hwa_id
                 */
                ndConnHandle.Open();
                string dsql5 = "exec tsp_hwards_All " + ncompid;       //normal service centre
                SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                DataSet ds5 = new DataSet();
                da5.Fill(ds5);
                if (ds5 != null)
                {
                    //                    MessageBox.Show("company is " + ncompid);
                    comboBox5.DataSource = ds5.Tables[0];
                    comboBox5.DisplayMember = "hwa_name";
                    comboBox5.ValueMember = "hwa_id";
                }
            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            gnWardID = Convert.ToInt32(comboBox5.SelectedValue);
            Orders dord = new Orders(gnWardID, 1);             //second parameter is destination type, here meaning ward basket (bulk orders)
            dord.ShowDialog();

        }
    }
}
