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
    public partial class SocialData : Form
    {
        public SocialData()
        {
            InitializeComponent();
        }
        string cs = globalvar.cos;
        decimal gnbal = 0.00m;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SocialData_Load(object sender, EventArgs e)
        {
            totalmalesavingsbal();
            totalfemalesavingsbal();
            totalgroupsavingsbal();
            gnbal = Convert.ToDecimal(textBox9.Text) + Convert.ToDecimal(textBox10.Text)+ Convert.ToDecimal(textBox11.Text);
            textBox12.Text = gnbal.ToString("N2");
            totalsharesbal();
        }
        public void totalmalesavingsbal()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
           ndConnHandle1.Open();
            string dsql2 = "select SUM(dbo.glmast.nbookbal) AS totalmalebal from cusreg, glmast ";
            dsql2 += "where glmast.ccustcode = cusreg.ccustcode and (dbo.cusreg.gender = 1) and cusreg.dgend = 'M' AND (dbo.glmast.intcode = 0) and acode in ('250', '251')";
            SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
            DataTable ds2 = new DataTable();
            da2.Fill(ds2);
            if (ds2 != null)
            {
               textBox9.Text = Convert.ToDecimal(ds2.Rows[0]["totalmalebal"]).ToString("N2");
            }
        }
        public void totalfemalesavingsbal()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            string dsql2 = "select SUM(dbo.glmast.nbookbal) AS totalfemalebal from cusreg, glmast ";
            dsql2 += "where glmast.ccustcode = cusreg.ccustcode and (dbo.cusreg.gender = 0) and cusreg.dgend = 'F' AND (dbo.glmast.intcode = 0) and acode in ('250', '251')";
            SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
            DataTable ds2 = new DataTable();
            da2.Fill(ds2);
            if (ds2 != null)
            {
                textBox10.Text = Convert.ToDecimal(ds2.Rows[0]["totalfemalebal"]).ToString("N2");
            }
        }
        
        public void totalgroupsavingsbal()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            string dsql2 = "select SUM(dbo.glmast.nbookbal) AS totalfemalebal from cusreg, glmast ";
            dsql2 += "where glmast.ccustcode = cusreg.ccustcode and (dbo.cusreg.mem_type = 3) AND (dbo.glmast.intcode = 0) and acode in ('250', '251')";
            SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
            DataTable ds2 = new DataTable();
            da2.Fill(ds2);
            if (ds2 != null)
            {
                textBox11.Text = Convert.ToDecimal(ds2.Rows[0]["totalfemalebal"]).ToString("N2");
            }
        }
        public void totalsharesbal()
        {
            SqlConnection ndConnHandle1 = new SqlConnection(cs);
            ndConnHandle1.Open();
            string dsql2 = "select SUM(dbo.glmast.nbookbal) AS totalsharesbal from cusreg, glmast ";
            dsql2 += "where glmast.ccustcode = cusreg.ccustcode AND (dbo.glmast.intcode = 0) and acode in ('270', '271')";
            SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle1);
            DataTable ds2 = new DataTable();
            da2.Fill(ds2);
            if (ds2 != null)
            {
                textBox13.Text = Convert.ToDecimal(ds2.Rows[0]["totalsharesbal"]).ToString("N2");
            }
        }
    }
}
