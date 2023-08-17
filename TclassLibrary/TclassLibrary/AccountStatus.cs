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
    public partial class AccountStatus : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        bool glnewProduct = false;
        string dloca = string.Empty;

        public AccountStatus(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void AccountStatus_Load(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                {
                    string cpatsql = "update glmast set caccstat = 'I' where cacctnumb = @membcode";
                    SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                    glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                    glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = comboBox1.SelectedValue;

                    ndConnHandle3.Open();
                    glUpdCommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                //    string auditDesc = "SMS -> " + membcode.Text.Trim();
                //    AuditTrail au = new AuditTrail();
                //    au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID", globalvar.gcWorkStation, globalvar.gcWinUser);
                    membcode.Focus();
                }
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
                   comboBox1.SelectedIndex = -1;
                membcode.Text = "";
                textBox1.Text = "";
                textBox4.Text = "";
            }

            if (checkBox3.Checked)
            {
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                {
                    string cpatsql = "update glmast set caccstat = 'D' where cacctnumb = @membcode";
                    SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                    glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                    glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = comboBox1.SelectedValue;

                    ndConnHandle3.Open();
                    glUpdCommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                 //   string auditDesc = "SMS -> " + membcode.Text.Trim();
                  //  AuditTrail au = new AuditTrail();
                  //  au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID", globalvar.gcWorkStation, globalvar.gcWinUser);
                    membcode.Focus();
                }
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
                //   comboBox6.SelectedIndex = -1;
            }

            if (checkBox4.Checked)
            {
                using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
                {
                    string cpatsql = "update glmast set caccstat = 'F' where cacctnumb = @membcode";
                    SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                    glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                    glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = comboBox1.SelectedValue;

                    ndConnHandle3.Open();
                    glUpdCommand.UpdateCommand.ExecuteNonQuery();
                    ndConnHandle3.Close();
                //    string auditDesc = "SMS -> " + membcode.Text.Trim();
                 //   AuditTrail au = new AuditTrail();
                //    au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID", globalvar.gcWorkStation, globalvar.gcWinUser);
                    membcode.Focus();
                }
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
                //   comboBox6.SelectedIndex = -1;
            }
        }
        private void checkCustomer(string tcCode)
        {
            string ncompid = globalvar.gnCompid.ToString().Trim();
            using (SqlConnection ndConnHandle1 = new SqlConnection(cs))
            {
                string dsql = "exec tsp_getMembers_one  " + ncompid + ",'" + tcCode + "'";
                //   ndConnHandle.Open();
                SqlDataAdapter da = new SqlDataAdapter(dsql, ndConnHandle1);
                DataTable vtable = new DataTable();
                da.Fill(vtable);
                if (vtable.Rows.Count > 0)
                //  if (vtable != null) 
                {
                    // MessageBox.Show("You are Inside");
                   // textBox4.Text = vtable.Rows[0]["membname"].ToString();
                  //  textBox3.Text = vtable.Rows[0]["membname"].ToString();
                  //  textBox1.Text = tcCode;
                }
               // else
            //    {
             //       MessageBox.Show("Member has been removed, inform IT DEPT immediately");
                  //  initvariables();
             //   }
            }
        }
        void getAccount()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
                ndConnHandle.Open();
                string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,glmast.cacctname,nbookbal from cusreg, glmast ";
                dsql2 += "where cusreg.ccustcode = glmast.ccustcode and cusreg.ccustcode = " + "'" + membcode.Text + "' and acode = '250' ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    comboBox1.DataSource = ds2.DefaultView;
                    comboBox1.DisplayMember = "cacctnumb";
                    comboBox1.ValueMember = "cacctnumb";
                    comboBox1.SelectedIndex = -1;
                  //  textBox1.Text = ds2.Rows[0]["cacctname"].ToString().Trim(); // + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                   //   textBox4.Text = ds2.Rows[0]["nbookbal"].ToString().Trim();

                }
            }
        }


        //
        void getAccount1()
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcCode = textBox1.Text.ToString().Trim().PadLeft(6, '0');
                ndConnHandle.Open();
                string dsql2 = "select ccustfname,ccustmname,ccustlname,glmast.ccustcode,glmast.cacctnumb,glmast.cacctname,nbookbal from cusreg, glmast ";
                dsql2 += "where cusreg.ccustcode = glmast.ccustcode and glmast.cacctnumb = " + "'" + comboBox1.SelectedValue + "' and acode = '250' ";
                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                DataTable ds2 = new DataTable();
                da2.Fill(ds2);
                if (ds2 != null)
                {
                    //comboBox1.DataSource = ds2.DefaultView;
                    //comboBox1.DisplayMember = "cacctnumb";
                    //comboBox1.ValueMember = "cacctnumb";
                    //comboBox1.SelectedIndex = -1;
                    textBox1.Text = ds2.Rows[0]["cacctname"].ToString().Trim(); // + ' ' + ds2.Rows[0]["ccustlname"].ToString().Trim();
                    textBox4.Text = ds2.Rows[0]["nbookbal"].ToString().Trim();

                }
            }
        }


        private void membcode_Validated(object sender, EventArgs e)
        {
            string tcCustCode = membcode.Text.ToString().Trim().PadLeft(6, '0');
            membcode.Text = tcCustCode;
            checkCustomer(tcCustCode);
            getAccount();
        }

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            getAccount1();
        }
    }
}
