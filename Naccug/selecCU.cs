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
    public partial class selecCU : Form
    {
        string cs = globalvar.cos;
        DataTable cuview = new DataTable();

        public const string sharcos = "Server=.;Database =NA_SOF001;User ID = sa; Password=Whatever4999!; connection Timeout = 3000000"; //connection to local tcaretemplate
//        public const string newsharcos = "Server = DESKTOP-JKM7P6P\\TCAREINS; Database = ShareBranch;Integrated Security=True"; //connection to local tcaretemplate
        public selecCU()
        {
            InitializeComponent();
        }

        private void selecCU_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Credit Union Selection >>";
            getCus();
        }

        /*
         insert into cu_meta (compid,com_name,catalog_name) values (32,'LIVESTOCK COOPERATIVE CREDIT UNION','na_sof001')
select * from cu_meta 
                    comboBox1.DataSource = dsb.DefaultView;
                    comboBox1.DisplayMember = "br_name";
                    comboBox1.ValueMember = "branchid";
                    comboBox1.SelectedIndex = -1;
             */
        private void getCus()
        {
            MessageBox.Show("Thsis is the Connection String " + cs);
            using (SqlConnection ndcon = new SqlConnection(cs))
            {
                ndcon.Open();
                string dsql0 = "select compid,com_name,server_name, catalog_name,spassword, susername from cu_meta  ";
                SqlDataAdapter da0 = new SqlDataAdapter(dsql0, ndcon);
                da0.Fill(cuview);
                if (cuview.Rows.Count > 0)
                {
                    comboBox1.DataSource = cuview.DefaultView;
                    comboBox1.DisplayMember = "com_name";
                    comboBox1.ValueMember = "compid";
                    comboBox1.SelectedIndex = -1;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Focused)
            {
                if(Convert.ToInt32(comboBox1.SelectedValue)>0)
                {
                    SaveButton.Enabled = true;
                    SaveButton.BackColor = Color.Green;
                    SaveButton.ForeColor = Color.White;
                }
                else
                {
                    SaveButton.Enabled = false;
                    SaveButton.BackColor = Color.Gainsboro;
                    SaveButton.ForeColor = Color.Black;
                }
                //string lcSrvName = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["server_name"]).Trim();
                //string lcDbName = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["catalog_name"]).Trim();
                //string lcCompName = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["com_name"]).Trim();
                //globalvar.gnCompid = Convert.ToInt32(cuview.Rows[comboBox1.SelectedIndex]["compid"]);
                //string gcNewString  = "Server = "+lcSrvName+"; Database = " + lcDbName + ";Integrated Security=True";
                //string gcNewString1 = "Server = DESKTOP-JKM7P6P\\TCAREINS; Database = " + lcDbName + ";Integrated Security=True"; 
                //globalvar.cos = gcNewString;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            globalvar.gcServername = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["server_name"]).Trim();
            // MessageBox.Show("This is the Server Name " + lcSrvName);
            globalvar.gcDatabaseName = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["catalog_name"]).Trim();
            // MessageBox.Show("This is the Server Name " + lcDbName);
            globalvar.gcspassword = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["spassword"]).Trim();
            //  MessageBox.Show("This is the Server Name " + lcPassword);
            globalvar.gcsusername = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["susername"]).Trim();
           // MessageBox.Show("This is the Server Name " + lcSuser);
            string lcCompName = Convert.ToString(cuview.Rows[comboBox1.SelectedIndex]["com_name"]).Trim();
            globalvar.gnCompid = Convert.ToInt32(cuview.Rows[comboBox1.SelectedIndex]["compid"]);


            globalvar.cos = "Server=" + globalvar.gcServername.Trim() + ";Database =" + globalvar.gcDatabaseName.Trim() + ";User ID = " + globalvar.gcsusername.Trim() + ";Password=" + globalvar.gcspassword.Trim() + "; connection Timeout = 3000000";  //connection to local database
            cs = globalvar.cos;
            MessageBox.Show("This is the Server Name " + cs);
            //string gcNewString = "Server = " + lcSrvName.Trim() + "; Database = " + lcDbName.Trim() + ";Integrated Security=True";
            // MessageBox.Show("This is the New Connection " + gcNewString);
            // string gcNewString1 = "Server = ITMANAGERALA; Database = " + lcDbName + ";Integrated Security=True";
            //  globalvar.cos = gcNewString;
            this.Close();
        }
    }
}
