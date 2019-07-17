using CustomerLibrary.Rai;
using SharedLibrary;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace logisticCustomerGrabber
{
    public partial class frm_customerGrabber : Form
    {
        public frm_customerGrabber()
        {
            InitializeComponent();
        }

        private void btn_grab_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.date_fromDate.Text = "1397/07/01";
            this.date_toDate.Text = Functions.miladiToSolar(DateTime.Now);
            this.ResumeLayout();
            if (Functions.IsNull(this.date_fromDate.Text))
            {
                MessageBox.Show("تاریخ شروع مشخص نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime fromDate, toDate;
            fromDate = DateTime.MinValue;
            toDate = DateTime.MaxValue;
            try
            {
                fromDate = Functions.solarToMiladi(this.date_fromDate.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در تاریخ انتخاب شده شروع", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Functions.IsNull(this.date_toDate.Text))
            {
                MessageBox.Show("تاریخ پایان مشخص نشده است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                toDate = Functions.solarToMiladi(this.date_toDate.Text);
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
            long wagonNo;
            using (var entityLogistic = new logisticEntities())
            {
                if (!string.IsNullOrEmpty(this.txtbx_wagonNos.Text))
                {

                    if (!long.TryParse(this.txtbx_wagonNos.Text, out wagonNo))
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
                    lstWagonNo = (from wagon in entityLogistic.Wagons
                                  where !entityLogistic.customersHistories.Any(o => o.wagonControlNo == wagon.wagonControlNo.ToString())
                                  && !entityLogistic.customersHistoryFetchLogs.Any(o => o.wagonNo == wagon.wagonControlNo.ToString().Substring(0, 6))
                                  select wagon.wagonNo).ToList();
                    //lstWagonNo.Add(342222);
                    //entityLogistic.Wagons.Select(o => o.wagonNo).ToList();
                }

                wagonInfoSeirHistory trackingHistory = new wagonInfoSeirHistory();
                List<Task<bool>> lstTsk = new List<Task<bool>>();
                int i = 0, j;

                i = lstWagonNo.Count - 1;
                while (i >= 10000)
                {
                    if (lstTsk.Count <= 10)
                    {
                        SharedVariables.logs.Info(i.ToString() + "," + lstWagonNo[i].Value);
                        long wagonNoTemp = lstWagonNo[i].Value;
                        var tsk = new Task<bool>(() => { return trackingHistory.readAndSaveWagonToDB(wagonNoTemp, fromDate, toDate, 0); });
                        tsk.Start();
                        lstTsk.Add(tsk);
                    }
                    else
                    {
                        Task.WaitAny(lstTsk.ToArray());
                        for (j = lstTsk.Count - 1; j >= 0; j--)
                        {
                            if (lstTsk[j].IsCanceled
                                || lstTsk[j].IsFaulted
                                || lstTsk[j].IsCompleted)
                            {
                                lstTsk.Remove(lstTsk[j]);
                            }
                        }
                    }
                    i--;
                }
                for (j = 0; j <= lstTsk.Count - 1; j++)
                {
                    lstTsk[j].Start();
                }
                if (lstTsk.Count > 0)
                    Task.WaitAll(lstTsk.ToArray());
                lstTsk.Clear();

            }
        }

        private void cmbx_fromDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbx_fromDate.SelectedIndex == 0)
                this.date_fromDate.Visible = true;
            else this.date_fromDate.Visible = false;

        }

        private void cmbx_toDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbx_toDate.SelectedIndex == 0)
                this.date_toDate.Visible = true;
            else this.date_toDate.Visible = false;
        }

        private void cmbx_wagonNos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbx_wagonNos.SelectedIndex == 0 || this.cmbx_wagonNos.SelectedIndex == 1)
                this.txtbx_wagonNos.Visible = false;
            else this.txtbx_wagonNos.Visible = true;
        }
    }
}
