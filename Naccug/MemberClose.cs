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
    public partial class MemberClose : Form
    {
        string cs = globalvar.cos;
        int ncompid = globalvar.gnCompid;
        string dloca = globalvar.cLocalCaption;

        string gcActivityType = string.Empty;
        public MemberClose(string actype)
        {
            InitializeComponent();
            gcActivityType = actype;
        }

        private void MemberClose_Load(object sender, EventArgs e)
        {
            this.Text = dloca +(gcActivityType =="C"? " << Member Close >>" : " << Member Activate >>"); 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            FindClient fc = new  FindClient(cs,ncompid,dloca, 1, "Cusreg");
            fc.ShowDialog();
            string retval = string.Empty;
            retval = fc.returnValue;
            getmember(retval);
        }

        private void guarcode_Validated(object sender, EventArgs e)
        {
            if (membcode.Text.ToString().Trim() != null && membcode.Text.ToString().Trim() != "")
            {
                getmember(membcode.Text.ToString());
            }
            AllClear2Go();
        }

        private void getmember(string mcode)
        {
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                string tcCode = mcode.PadLeft(6, '0');// Convert.ToString(membcode.Text).Trim().PadLeft(6, '0');
                membcode.Text = tcCode;
                ndConnHandle.Open();
                DataTable ds2 = new DataTable();
                if (gcActivityType=="C")
                {
                    string dsql2 = "select ccustfname, ccustmname, ccustlname, ccustname,";
                    dsql2 += "      nsaveBal = case when(select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('250', '251'))> 0 then";
                    dsql2 += "     (select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('250', '251')) else 0.00 end,";
                    dsql2 += "      nshareBal = case when(select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('270', '271'))> 0 then";
                    dsql2 += "     (select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('270', '271')) else 0.00 end,";
                    dsql2 += "      nloanBal = case when(select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('130', '131'))> 0 then";
                    dsql2 += "     (select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('130', '131')) else 0.00 end";
                    dsql2 += "          from cusreg, glmast";
                    dsql2 += "          where cusreg.ccustcode = glmast.ccustcode and cusreg.lactive=1 and  cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    da2.Fill(ds2);
                }
                else
                {
                    string dsql2 = "select ccustfname, ccustmname, ccustlname, ccustname,";
                    dsql2 += "      nsaveBal = case when(select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('250', '251'))> 0 then";
                    dsql2 += "     (select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('250', '251')) else 0.00 end,";
                    dsql2 += "      nshareBal = case when(select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('270', '271'))> 0 then";
                    dsql2 += "     (select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('270', '271')) else 0.00 end,";
                    dsql2 += "      nloanBal = case when(select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('130', '131'))> 0 then";
                    dsql2 += "     (select nbookbal where cusreg.ccustcode = glmast.ccustcode and glmast.intcode = 0 and acode in ('130', '131')) else 0.00 end";
                    dsql2 += "          from cusreg, glmast";
                    dsql2 += "          where cusreg.ccustcode = glmast.ccustcode and cusreg.lactive= 0 and  cusreg.ccustcode = " + "'" + tcCode + "'";
                    SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                    da2.Fill(ds2);
                }

                //                SqlDataAdapter da2 = new SqlDataAdapter(dsql2, ndConnHandle);
                //                DataTable ds2 = new DataTable();
//                da2.Fill(ds2);
                if (ds2 != null && ds2.Rows.Count > 0)
                {
                    textBox1.Text = ds2.Rows[0]["ccustname"].ToString().Trim() + ds2.Rows[0]["ccustfname"].ToString().Trim();
                    textBox2.Text = ds2.Rows[0]["ccustmname"].ToString();
                    textBox3.Text = ds2.Rows[0]["ccustlname"].ToString();
                    textBox4.Text = Convert.ToDecimal(ds2.Rows[0]["nsaveBal"]).ToString("N2"); 
                    textBox5.Text = Convert.ToDecimal(ds2.Rows[0]["nshareBal"]).ToString("N2");
                    textBox6.Text = Convert.ToDecimal(ds2.Rows[0]["nloanBal"]).ToString("N2");
                }
                else
                {
//                    MessageBox.Show("Activity type is "+gcActivityType );
                    MessageBox.Show("Member ID not found or "+(gcActivityType=="C" ? "Member Closed" : "Member Active"));
                    membcode.Text = "";
                    membcode.Focus();
                }
                AllClear2Go();
            }
        }
    
    protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.Tab)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                AllClear2Go();
            }
        }

        #region Checking if all the mandatory conditions are satisfied
        private void AllClear2Go()
        {
            if (membcode.Text.ToString().Trim() != "" && Convert.ToDecimal(textBox4.Text) == 0.00m && Convert.ToDecimal(textBox5.Text) == 0.00m && Convert.ToDecimal(textBox6.Text) == 0.00m)

            {
                saveButton.Enabled = true;
                saveButton.BackColor = Color.LawnGreen;
            }
            else
            {
                saveButton.Enabled = false;
                saveButton.BackColor = Color.Gainsboro;
            }
        }
        #endregion

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection ndConnHandle3 = new SqlConnection(cs))
            {
                string cpatsql = gcActivityType=="C" ? "update cusreg set lactive = 0 where ccustcode = @membcode" : "update cusreg set lactive = 1 where ccustcode = @membcode";
                SqlDataAdapter glUpdCommand = new SqlDataAdapter();
                glUpdCommand.UpdateCommand = new SqlCommand(cpatsql, ndConnHandle3);
                glUpdCommand.UpdateCommand.Parameters.Add("@membcode", SqlDbType.Char).Value = membcode.Text;

                ndConnHandle3.Open();
                glUpdCommand.UpdateCommand.ExecuteNonQuery();
                ndConnHandle3.Close();
                string auditDesc = (gcActivityType == "C" ? "Member Closure  -> " : "Member Activate  -> ") +membcode.Text.Trim();
                AuditTrail au = new AuditTrail();
                au.upd_audit_trail(cs, auditDesc, 0.00m, 0.00m, globalvar.gcUserid, "U", "Old Label", "New Label", "Our remarks", "tcID",globalvar.gcWorkStation,globalvar.gcWinUser);
                initvariables();
                membcode.Focus();
            }
        }     
        
        private void initvariables()
        {
            membcode.Text = textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = "";
            AllClear2Go();
        }       
    }
}

