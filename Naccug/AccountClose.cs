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
    public partial class AccountClose : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string dloca = globalvar.cLocalCaption;

        string gcActivityType = string.Empty;
        public AccountClose(string actype)
        {
            InitializeComponent();
            gcActivityType = actype;
        }

        private void getmember(string mcode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
               // string tcCode = mcode.PadLeft(6, '0');// Convert.ToString(membcode.Text).Trim().PadLeft(6, '0');
             //   membcode.Text = tcCode;
                ndConnHandle.Open();
                DataTable ds2 = new DataTable();
                if (gcActivityType == "C" || gcActivityType == "I" || gcActivityType == "F" || gcActivityType == "D")
                {
                    string dsq12 = "select glmast.cacctnumb, glmast.cacctname,glmast.nbookbal from glmast where caccstat='A' and glmast.nbookbal = 0.00 and  acode in ('250', '251','270','271') and glmast.cacctnumb = " + "'" + membcode.Text.Trim() + "'"; 
                    SqlDataAdapter da2 = new SqlDataAdapter(dsq12, ndConnHandle);
                    da2.Fill(ds2);
                }
                else
                {

                    string dsq12 = "select glmast.cacctnumb, glmast.cacctname, glmast.nbookbal from glmast where (caccstat='C' or caccstat='I' or caccstat='D' or caccstat='F') and  acode in ('250', '251','270','271') and glmast.cacctnumb = " + "'" + membcode.Text.Trim() + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsq12, ndConnHandle);
                    da2.Fill(ds2);
                }

                if (ds2 != null && ds2.Rows.Count > 0)
                {
                     textBox1.Text = ds2.Rows[0]["cacctname"].ToString();
                     textBox4.Text = Convert.ToDecimal(ds2.Rows[0]["nbookbal"]).ToString("N2");
                }
                else
                {
                    //                    MessageBox.Show("Activity type is "+gcActivityType );
                    MessageBox.Show("Account not found or May have a Balance or" + (gcActivityType == "C" ? "Account Closed" : "Account Active"));
                    membcode.Text = "";
                    membcode.Focus();
                }
              //  AllClear2Go();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatsql = gcActivityType == "C" || gcActivityType == "I" || gcActivityType == "F" || gcActivityType == "D" ? "update glmast set caccstat = 'C' where cacctnumb = @membcode" : "update glmast set caccstat = 'A' where cacctnumb = @membcode";
                SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = membcode.Text;

                ndConnHandle3.Open();
                glUpdCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();


                string auditDesc = (gcActivityType == "C" ? "Account Closure  -> " : "Account Activate  -> ") + membcode.Text.Trim();
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID", globalvar.gcWorkStation, globalvar.gcWinUser);
                membcode.Focus();
            }
            membcode.Text = "";
            textBox1.Text = "";
            textBox4.Text = "";
        }

        private void AccountClose_Load(object sender, EventArgs e)
        {
            this.Text = dloca + (gcActivityType == "C" ? " << Account Close >>" : " << Account Activate >>");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void membcode_Validated(object sender, EventArgs e)
        {
            if (membcode.Text.ToString().Trim() != null && membcode.Text.ToString().Trim() != "")
            {
                getmember(membcode.Text.ToString());
            }
        }

        private void membcode_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
