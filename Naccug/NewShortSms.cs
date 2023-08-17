using System;
using System.Collections.Generic;
using System.Text;

namespace WinTcare
{
    class NewShortSms
    {
    }
/*
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Font = new Font(dataGridView3.Font, FontStyle.Bold);            
            style.BackColor = Color.Green;
            style.ForeColor = Color.White;
            

                int i = dataGridView3.CurrentRow.Index;
                

                try
                {
                    Comm_Port =     Convert.ToInt16(dataGridView3.Rows[i].Cells[0].Value.ToString().Substring(3));
                    Comm_BaudRate = Convert.ToInt32(dataGridView3.Rows[i].Cells[2].Value.ToString());
                    Comm_TimeOut =  Convert.ToInt32(dataGridView3.Rows[i].Cells[3].Value.ToString());
                }
                catch (Exception E1)
                {
                    MessageBox.Show("Error Converting COM Port Settings Values", "Check COM Port Values", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                comm = new GsmCommMain(Comm_Port, Comm_BaudRate, Comm_TimeOut);

                try
                {

                        comm.Open();
                        if (comm.IsConnected())
                        {

                            pictureBox3.Image = imageList1.Images[1];

                            MessageBox.Show("Connected Successfully To GSM Phone / Modem...!!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dataGridView3.Rows[i].Cells[4].Value = "Connected";
                            dataGridView3.Rows[i].DefaultCellStyle = style;
                            dataGridView3.ClearSelection();
                            Single_SMS.Enabled = true;

                        }

                        try
                        {
                            Phone_Name.Text = comm.IdentifyDevice().Manufacturer.ToUpper().ToString();
                            Phone_Model.Text = comm.IdentifyDevice().Model.ToUpper().ToString();
                            Revision_Num.Text = comm.IdentifyDevice().Revision.ToUpper().ToString();
                            Serial_Num.Text = comm.IdentifyDevice().SerialNumber.ToUpper().ToString();
                        }
                        catch (Exception e50)
                        {
                            MessageBox.Show("Error Retriving COM Port Phone Information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    

                }
                catch (Exception E2)
                {
                    MessageBox.Show("Error While Connecting To GSM Phone / Modem", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dataGridView3.ClearSelection();

                }
        }

*/
}

