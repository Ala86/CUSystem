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
    public partial class spcselect : Form
    {
        int gnServiceCentre = -1;
        public spcselect()
        {
            InitializeComponent();
        }

        private void spcselect_Load(object sender, EventArgs e)
        {
            this.Text = globalvar.cLocalCaption + "<< Queue Selection >>";
            label22.Text = globalvar.gcCopyRight;
            string ncompid = globalvar.gnCompid.ToString().Trim();
            string cs = globalvar.cos;
            using (SqlConnection ndConnHandle = new SqlConnection(cs))
            {
                ndConnHandle.Open();
                //************Getting Service centres for: normal or walkin service centres combobox5
                string dsql5 = "exec tsp_TcarelClinics " + ncompid;       //normal service centre
                SqlDataAdapter da5 = new SqlDataAdapter(dsql5, ndConnHandle);
                DataSet ds5 = new DataSet();
                da5.Fill(ds5);
                if (ds5 != null)
                {
                    //                    MessageBox.Show("company is " + ncompid);
                    comboBox5.DataSource = ds5.Tables[0];
                    comboBox5.DisplayMember = "srv_name";
                    comboBox5.ValueMember = "srv_id";
                }
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down || e.KeyCode == Keys.LButton)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                e.Handled = true;
                //          MessageBox.Show("ABOUT TO to to allclear down");
                AllClear2Go();
            }
            else if (e.KeyCode == Keys.Up)
            {
                SelectNextControl(ActiveControl, false, true, true, true);
                e.Handled = true;
                //        MessageBox.Show("ABOUT TO to to allclear up");
                AllClear2Go();
            }
        }


        private void AllClear2Go()
        {
            if (Convert.ToInt16(comboBox5.SelectedValue)>0)
            {
      //         MessageBox.Show("All clear to go");
                SaveButton.Enabled = true;
                SaveButton.BackColor = Color.LawnGreen;
                SaveButton.Select();
            }
            else
            {
        //        MessageBox.Show("Not yet");
                SaveButton.Enabled = false;
                SaveButton.BackColor = Color.FromArgb(224, 224, 224);        // Color.Red;
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            SaveButton.Enabled = true;
            SaveButton.BackColor = Color.LawnGreen;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            globalvar.gcFormCaption = comboBox5.Text;// comboBox5.SelectedText.Trim().ToString();// .DisplayMember;
            globalvar.gnQueueID = Convert.ToInt16(comboBox5.SelectedValue);
            int lnoption = globalvar.gnSpSelect;
//            MessageBox.Show("The queue is "+ gnQueueID);
            switch (lnoption)
            {
                case 1:
                    Triage dtriage = new Triage();
                    dtriage.ShowDialog();
                    break;
                case 2:
//                    Consultation dconsul = new Consultation();
  //                  dconsul.ShowDialog();
                    break;
                case 3:
                    MessageBox.Show("service/product selection");
           //         gnQueueID = gnClinicid
           //         Do Form app_q With 132
                    break;
                case 4:
                    MessageBox.Show("Special delivery");
               //     gnQueueID = gnClinicid
               //     Do Form specialdelivery
                    break;
                case 5:
                    MessageBox.Show("special clinic reporting");
                 //   Do Form spcprint
                    break;
                case 6:
                    //                    MessageBox.Show("Bulk orders");
                    gnServiceCentre = Convert.ToInt32(comboBox5.SelectedValue);
                    Orders dord = new Orders(gnServiceCentre,1);            //second parameter is destination type, here meaning clinics
                    dord.ShowDialog();
                    break;
                case 7:
                    MessageBox.Show("clinic reports");
      //              gnServiceCentre = gnClinicid
      //              Do Form ClinicReports.scx
                    break;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
