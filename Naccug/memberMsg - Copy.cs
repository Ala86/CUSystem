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
    public partial class memberMsg : Form
    {
        string cs = globalvar.cos;
       // string cs = globalvar.cos;
        string gcMembCode ;
        public memberMsg()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            // string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');

            MessageBox.Show("This is the connection string " + cs);
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
                MessageBox.Show("This is the Member Code " + tcCode);
              //  textBox1.Text = tcCode;
                ndConnHandle1.Open();
                string dsql2 = "select ccustfname,ccustmname,ccustlname,ccustcode,MemberMsg from cusreg where ccustcode = " + "'" + tcCode.Trim() + "'";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null && ds2.Rows.Count > 0)
                {
                     string MemName = ds2.Rows[0]["ccustfname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustmname"].ToString().Trim() + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                     textBox3.Text = MemName;
                    textBox2.Text = ds2.Rows[0]["MemberMsg"].ToString();
                }
                else { MessageBox.Show("No savings account found for this member"); }
                ndConnHandle1.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateloan();
       }
        private void updateloan()
        {
            using (SqlConnection nConnHandle2 = new SqlConnection(cs))
            {
                string cglquery = "update cusreg set MemberMsg ='"+ textBox2.Text +"'  where ccustcode=@ccustcode";
                SqlDataAdapter insCommand = new SqlDataAdapter();
                insCommand.InsertCommand = new SqlCommand(cglquery, nConnHandle2);
                insCommand.InsertCommand.Parameters.Add("@ccustcode", SqlDbType.VarChar).Value = textBox1.Text;
                nConnHandle2.Open();
                insCommand.InsertCommand.ExecuteNonQuery();
                nConnHandle2.Close();
            }
        }
    }
}
