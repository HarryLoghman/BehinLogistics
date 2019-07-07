using RailSiteDataGrabber.Rai;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RailSiteDataGrabber
{
    public partial class frm_customersRai : Form
    {
        public frm_customersRai()
        {
            InitializeComponent();
        }

        private void frm_customersRai_Load(object sender, EventArgs e)
        {
            this.date_get30DaysBefore.Text = Functions.miladiToSolar(DateTime.Now);
            this.date_history_fromDate.Text = "";
            this.date_history_toDate.Text = "";
        }

        private void btn_trackWagons_Click(object sender, EventArgs e)
        {
            long? wagonNoStart, wagonNoEnd;
            wagonNoStart = wagonNoEnd = null;
            long temp;
            if (!string.IsNullOrEmpty(this.txtbx_wagonNoStart.Text))
            {
                if (!long.TryParse(this.txtbx_wagonNoStart.Text, out temp))
                {
                    MessageBox.Show("WagonNoStart is not an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (temp < 0)
                {
                    MessageBox.Show("WagonNoStart should be a positive value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                wagonNoStart = temp;

            }
            if (!string.IsNullOrEmpty(this.txtbx_wagonNoEnd.Text))
            {
                if (!long.TryParse(this.txtbx_wagonNoEnd.Text, out temp))
                {
                    MessageBox.Show("WagonNoEnd is not an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (temp < 0)
                {
                    MessageBox.Show("WagonNoEnd should be a positive value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                wagonNoEnd = temp;
            }

            if (wagonNoStart.HasValue && wagonNoEnd.HasValue
                && wagonNoStart.Value >= wagonNoEnd.Value)
            {

                MessageBox.Show("WagonNoEnd should be greater than than WagonNoStart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            wagonInfoTrackingResult trackingResult = new wagonInfoTrackingResult();
            using (var entityLogistic = new Model.logisticEntities())
            {
                do
                {
                    wagonNoStart = entityLogistic.Wagons.Where(o => !wagonNoStart.HasValue || o.wagonNo >= wagonNoStart.Value).Min(o => o.wagonNo);
                    if (wagonNoStart.HasValue)
                    {
                        wagonNoEnd = entityLogistic.Wagons.Where(o => o.wagonNo <= wagonNoStart.Value + 50).Max(o => o.wagonNo);
                        if (wagonNoEnd.HasValue)
                        {
                            trackingResult.readAndSaveWagonToDB(wagonNoStart.Value, wagonNoEnd.Value
                                , int.Parse(this.numeric_cycleNumber.Value.ToString()), this.chkbx_trackWagons_get30DaysAgo.Checked);
                            wagonNoStart = wagonNoEnd.Value + 1;
                        }
                    }
                } while (wagonNoStart.HasValue && wagonNoEnd.HasValue);
            }



        }

        private void btn_get30DaysBefore_Click(object sender, EventArgs e)
        {
            if (Functions.IsNull(this.date_get30DaysBefore.Text))
            {
                MessageBox.Show("تاریخی انتخاب نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                DateTime date = Functions.solarToMiladi(this.date_get30DaysBefore.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در تاریخ انتخاب شده", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<long?> lstWagonNo = new List<long?>();
            using (var entityLogistic = new Model.logisticEntities())
            {
                if (!string.IsNullOrEmpty(this.txtbx_get30DaysBeforeWagonNo.Text))
                {
                    long wagonNo;
                    if (!long.TryParse(this.txtbx_get30DaysBeforeWagonNo.Text, out wagonNo))
                    {
                        MessageBox.Show("WagonNo is not an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (wagonNo < 0)
                    {
                        MessageBox.Show("WagonNo should be a positive value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    lstWagonNo.Add(wagonNo);
                }
                else
                {
                    lstWagonNo = entityLogistic.Wagons.Select(o => o.wagonNo).ToList();
                }

                wagonInfoTrackingDetail trackingDetail = new wagonInfoTrackingDetail();
                for (int i = 0; i <= lstWagonNo.Count - 1; i++)
                {
                    trackingDetail.readAndSaveWagonToDB(lstWagonNo[i].Value, this.date_get30DaysBefore.Text, (int)this.numeric_cycleNumber.Value);
                }

            }
        }


        private void btn_history_Click(object sender, EventArgs e)
        {
            if (Functions.IsNull(this.date_history_fromDate.Text))
            {
                MessageBox.Show("تاریخ شروع مشخص نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime fromDate, toDate;
            fromDate = DateTime.MinValue;
            toDate = DateTime.MaxValue;
            try
            {
                fromDate = Functions.solarToMiladi(this.date_history_fromDate.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در تاریخ انتخاب شده شروع", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Functions.IsNull(this.date_history_toDate.Text))
            {
                MessageBox.Show("تاریخ پایان مشخص نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                toDate = Functions.solarToMiladi(this.date_history_toDate.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در تاریخ انتخاب شده شروع", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fromDate > toDate)
            {
                MessageBox.Show("تاریخ شروع باید قبل از تاریخ پایان باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<long?> lstWagonNo = new List<long?>();
            using (var entityLogistic = new Model.logisticEntities())
            {
                if (!string.IsNullOrEmpty(this.txtbx_history_wagonNo.Text))
                {
                    long wagonNo;
                    if (!long.TryParse(this.txtbx_history_wagonNo.Text, out wagonNo))
                    {
                        MessageBox.Show("WagonNo is not an integer value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (wagonNo < 0)
                    {
                        MessageBox.Show("WagonNo should be a positive value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    lstWagonNo.Add(wagonNo);
                }
                else
                {
                    lstWagonNo = entityLogistic.Wagons.Select(o => o.wagonNo).ToList();
                }

                wagonInfoSeirHistory trackingHistory = new wagonInfoSeirHistory();
                for (int i = 0; i <= lstWagonNo.Count - 1; i++)
                {
                    trackingHistory.readAndSaveWagonToDB(lstWagonNo[i].Value, this.date_history_fromDate.Text, this.date_history_toDate.Text, (int)this.numeric_cycleNumber.Value);
                }

            }
        }

        private void btn_billOfLadingTracking_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtbx_billOfLadingTracking_numbers.Text))
            {
                MessageBox.Show("شماره های بارنامه مشخص نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var billOfLadingsArr = this.txtbx_billOfLadingTracking_numbers.Text.Split(',');
            string[] rangeArr;
            string strBillOfLadingsSeperatedWithCommas = "";
            int i, j, range20, commaListIndex;
            int tempStartRange, tempEndRange, tempBillOfLading;
            commaListIndex = -1;
            List<string> lstBillOfLadings = new List<string>();
            Rai.billOfLadingsTracking billOfLadingsTracking = new billOfLadingsTracking();
            for (i = 0; i <= billOfLadingsArr.Length - 1; i++)
            {
                if (billOfLadingsArr[i].Contains("-"))
                {
                    //we have range
                    rangeArr = billOfLadingsArr[i].Split('-');
                    if (rangeArr.Length != 2)
                    {
                        MessageBox.Show("خطا در شماره بارنامه ها" + billOfLadingsArr[i], "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(rangeArr[0], out tempStartRange) || !int.TryParse(rangeArr[1], out tempEndRange))
                    {
                        MessageBox.Show("خطا در شماره بارنامه ها" + billOfLadingsArr[i], "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (tempStartRange > tempEndRange)
                    {
                        MessageBox.Show("خطا در شماره بارنامه ها" + billOfLadingsArr[i], "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    range20 = ((tempEndRange - tempStartRange) / 20) ;
                    j = 0;
                    while (j < range20)
                    {
                        lstBillOfLadings.Add(tempStartRange.ToString() + "-" + Math.Min(tempStartRange + 20, tempEndRange).ToString());
                        j++;
                        tempStartRange = tempStartRange + 20 + 1;
                    }
                }
                else
                {
                    if (!int.TryParse(billOfLadingsArr[i], out tempBillOfLading))
                    {
                        MessageBox.Show("خطا در شماره بارنامه ها" + billOfLadingsArr[i], "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (commaListIndex == -1)
                    {
                        lstBillOfLadings.Add("");
                        commaListIndex = lstBillOfLadings.Count - 1;
                    }
                    strBillOfLadingsSeperatedWithCommas = lstBillOfLadings[commaListIndex];

                    if (strBillOfLadingsSeperatedWithCommas.Split(',').Length == 20)
                    {
                        //we have 20 items => add new item
                        lstBillOfLadings.Add(billOfLadingsArr[i]);
                        commaListIndex = lstBillOfLadings.Count - 1;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(lstBillOfLadings[i]))
                        {
                            lstBillOfLadings[commaListIndex] = billOfLadingsArr[i];
                        }
                        else
                        {
                            lstBillOfLadings[commaListIndex] = lstBillOfLadings[commaListIndex] + "," + billOfLadingsArr[i];
                        }
                    }

                }
            }
            for (i = 0; i <= lstBillOfLadings.Count - 1; i++)
            {
                billOfLadingsTracking.readAndSaveWagonToDB(lstBillOfLadings[i], (int)this.numeric_cycleNumber.Value);
            }
        }

        private void control_gotFocused(object sender, EventArgs e)
        {
            Functions.sb_control_gotFocused(sender, e);
        }

        private void btn_passengersLastStatus_Click(object sender, EventArgs e)
        {
            trainPassengersLastStatus passengers = new trainPassengersLastStatus();
            passengers.readAndSaveToDB((int)this.numeric_cycleNumber.Value);
        }
    }
}
