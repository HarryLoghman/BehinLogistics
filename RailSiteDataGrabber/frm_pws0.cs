using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailSiteDataGrabber
{
    public partial class frm_pws0 : Form
    {
        public frm_pws0()
        {
            InitializeComponent();
        }

        private void btn_getAgencies_Click(object sender, EventArgs e)
        {
            PWS0.agencies agencies = new PWS0.agencies();
            agencies.sb_readAndSaveToDB();
            MessageBox.Show("Finished!!!");
        }

        private void btn_getStations_Click(object sender, EventArgs e)
        {
            PWS0.stations stations = new PWS0.stations();
            stations.sb_readAndSaveToDB();
            MessageBox.Show("Finished!!!");
        }

        private void btn_getStates_Click(object sender, EventArgs e)
        {
            PWS0.stateCities states = new PWS0.stateCities();
            states.sb_readAndSaveStatesToDB(this.chkbx_getAllCities.Checked);
            MessageBox.Show("Finished!!!");
        }

        private void btn_getCities_Click(object sender, EventArgs e)
        {
            int stateCode;
            if (!int.TryParse(this.txtbx_stateCode.Text, out stateCode))
            {
                MessageBox.Show("Wrong StateCode or the StateCode is not specified");
                return;
            }
            PWS0.stateCities cities = new PWS0.stateCities();
            cities.sb_readAndSaveCitiesToDB(stateCode, null);
            MessageBox.Show("Finished!!!");
        }

        private void btn_getTrainPassengers_Click(object sender, EventArgs e)
        {
            PWS0.trainPassenger trainPassenger = new PWS0.trainPassenger();
            trainPassenger.sb_readAndSaveToDB((int)this.numeric_cycleNumber.Value);
            MessageBox.Show("Finished!!!");
        }

        private void btn_getTrainBarries_Click(object sender, EventArgs e)
        {
            PWS0.trainBarry trainBarry = new PWS0.trainBarry();
            trainBarry.sb_readAndSaveToDB((int)this.numeric_cycleNumber.Value, this.chkbx_getAllLocomotives.Checked, this.chkbx_getAllBillOfLadings.Checked);
            MessageBox.Show("Finished!!!");
        }

        private void btn_getTrainBarryLocomotives_Click(object sender, EventArgs e)
        {
            int F15Rec_ID;
            if (!int.TryParse(this.txtbx_locomotives_F15Rec_ID.Text, out F15Rec_ID))
            {
                MessageBox.Show("Wrong F15Rec_ID or the F15Rec_ID is not specified");
                return;
            }
            PWS0.trainBarryLocomotives locomotives = new PWS0.trainBarryLocomotives();
            locomotives.fnc_getTrainsBarryLocomotives(F15Rec_ID);
            MessageBox.Show("Warning!!! 'get Train Barry Locomotives' does not save locomtives into database. To save locomotives you need to perform 'get Train Barries'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            MessageBox.Show("Finished!!!");

        }

        private void btn_getTrainBarryBillOfLadings_Click(object sender, EventArgs e)
        {
            int F15Rec_ID;
            if (!int.TryParse(this.txtbx_BillOfLading_F15Rec_ID.Text, out F15Rec_ID))
            {
                MessageBox.Show("Wrong F15Rec_ID or the F15Rec_ID is not specified");
                return;
            }
            int trainNo;
            if (!int.TryParse(this.txtbx_BillOfLading_Train_No.Text, out trainNo))
            {
                MessageBox.Show("Wrong Train_No or the Train_No is not specified");
                return;
            }
            PWS0.trainBarryBillOfLadings billOfLadings = new PWS0.trainBarryBillOfLadings();
            billOfLadings.readAndSaveToDB(F15Rec_ID, trainNo, null, (int)this.numeric_cycleNumber.Value, null);
            MessageBox.Show("Finished!!!");
        }

        private void btn_findTrainBarryWagon_Click(object sender, EventArgs e)
        {
            long temp = 0;
            long? wagonNo = null;
            if (!string.IsNullOrEmpty(this.txtbx_wagon_no.Text) && !long.TryParse(this.txtbx_wagon_no.Text, out temp))
            {
                MessageBox.Show("Wrong Wagon No");
                return;
            }
            wagonNo = temp;
            PWS0.trainBarryWagons billOfLadings = new PWS0.trainBarryWagons();
            billOfLadings.readAndSaveWagonToDB(wagonNo, (int)this.numeric_cycleNumber.Value);
            MessageBox.Show("Finished!!!");
        }

        private void btn_findBillOfLading_Click(object sender, EventArgs e)
        {
            long barnameh_no;
            if (!long.TryParse(this.txtbx_barnameh_no.Text, out barnameh_no))
            {
                MessageBox.Show("Wrong barnameh_no or the barnameh_no is not specified");
                return;
            }
            
            PWS0.trainBarryWagons billOfLadings = new PWS0.trainBarryWagons();
            billOfLadings.readAndSaveBarnamehToDB(barnameh_no, (int)this.numeric_cycleNumber.Value);
            MessageBox.Show("Finished!!!");
        }

        private void btn_findTrainBarry_Click(object sender, EventArgs e)
        {
            int train_no;
            if (!int.TryParse(this.txtbx_train_no.Text, out train_no))
            {
                MessageBox.Show("Wrong train_no or the train_no is not specified");
                return;
            }

            PWS0.trainBarry trainBarry = new PWS0.trainBarry();
            trainBarry.sb_readAndSaveTrainToDB(train_no, (int)this.numeric_cycleNumber.Value, this.chkbx_getTrainBarryLocomotives.Checked
                , this.chkbx_getTrainBarryBillOfLadings.Checked);
            MessageBox.Show("Finished!!!");
        }

        private void control_gotFocused(object sender, EventArgs e)
        {
            Functions.sb_control_gotFocused(sender, e);
        }
    }
}
