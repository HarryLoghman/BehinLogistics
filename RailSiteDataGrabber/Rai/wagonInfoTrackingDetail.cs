using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.Rai
{
    class wagonInfoTrackingDetail
    {
        string v_url = "http://customers.rai.ir/Wagon_Info/Tracking/Details.asp";
        wagonInfoTrackingDetailDataTable v_dt;
        public wagonInfoTrackingDetail()
        {
            this.v_dt = new wagonInfoTrackingDetailDataTable();

        }

        public void readAndSaveWagonToDB(long wagonNo, string solarDateEnd, int cycleNumber)
        {

            try
            {
                try
                {
                    DateTime date = Functions.solarToMiladi(solarDateEnd);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error in date format", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                if (solarDateEnd.Length == 10)
                    solarDateEnd = solarDateEnd.Remove(0, 2);
                solarDateEnd = solarDateEnd.Replace("/", "");


                login lg = new login();
                start:
                if (login.v_cookieCollection == null)
                {

                    if (!lg.fnc_login())
                    {
                        //cannot login
                        return;
                    }
                }

                Uri uri;
                uri = new Uri(this.v_url + "?WagonNO=" + wagonNo + "&Date=" + solarDateEnd);


                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);


                //webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                webRequest.Proxy = null;
                webRequest.Method = "GET";

                //webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.ContentType = "application/x-www-form-urlencoded";

                webRequest.CookieContainer = new CookieContainer();
                foreach (Cookie cookie in login.v_cookieCollection)
                {
                    webRequest.CookieContainer.Add(cookie);
                }

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse != null)
                {
                    if (webResponse.ResponseUri == null || string.IsNullOrEmpty(webResponse.ResponseUri.AbsoluteUri))
                    {
                        webResponse.Close();
                        return;
                    }
                    if (login.fnc_isLoginPage(webResponse, this.v_url))
                    {
                        //we haven't loginned yet
                        if (lg.fnc_login())
                        {
                            goto start;
                        }
                        else
                        {
                            //cannot login inform or log !!!!!
                            webResponse.Close();
                            return;
                        }
                    }
                    string result;
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("windows-1256")))
                    {
                        result = rd.ReadToEnd();
                    }
                    webResponse.Close();
                    if (!string.IsNullOrEmpty(result))
                    {
                        this.v_dt.Rows.Clear();
                        Functions.sb_fillDatatableWithHtmlTableId(result, "AutoNumber2", 2,0,0, 0, this.v_dt);
                        if (this.v_dt.Rows.Count > 0)
                        {
                            this.sb_saveToDB(this.v_dt, cycleNumber);

                        }
                    }

                }


            }
            catch (Exception ex)
            {

            }
        }

        private void sb_saveToDB(DataTable dt, int cycleNumber)
        {
            if (dt == null)
            {
                return;
            }
            int i;
            string solarDate;
            int? solarTime;
            DateTime? enteranceDateTime;
            Model.raiWagon_Info_Tracking entry_raiWagon_Info_Tracking;
            using (var entityLogistic = new Model.logisticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    entry_raiWagon_Info_Tracking = new Model.raiWagon_Info_Tracking();

                    entry_raiWagon_Info_Tracking.currentStationId = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_currentStationName]) ? null : PWS0.stations.fnc_getStationId(null, this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_currentStationName].ToString(), true));
                    entry_raiWagon_Info_Tracking.CycleNumber = cycleNumber;
                    entry_raiWagon_Info_Tracking.destinationStationId = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_destinationStationName]) ? null : PWS0.stations.fnc_getStationId(null, this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_destinationStationName].ToString(), true));
                    #region enteranceDateTime
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceDate]))
                    {
                        var solarDateArr = dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceDate].ToString().Split('/');
                        Array.Reverse(solarDateArr);
                        solarDate = string.Join("", solarDateArr);
                        solarDate = solarDate.Replace("/", "");
                    }
                    if (!Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceTime]))
                        solarTime = int.Parse(dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceTime].ToString().Replace(":", ""));
                    entry_raiWagon_Info_Tracking.enteranceDateTime = enteranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime);
                    #endregion

                    #region exitDateTime
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitDate]))
                    {
                        var solarDateArr = dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitDate].ToString().Split('/');
                        Array.Reverse(solarDateArr);
                        solarDate = string.Join("", solarDateArr);
                        solarDate = solarDate.Replace("/", "");
                    }

                    if (!Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitTime]))
                        solarTime = int.Parse(dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitTime].ToString().Replace(":", ""));

                    entry_raiWagon_Info_Tracking.exitDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime);
                    #endregion
                    entry_raiWagon_Info_Tracking.FetchTime = DateTime.Now;

                    entry_raiWagon_Info_Tracking.goodsID = null;
                    entry_raiWagon_Info_Tracking.Source = "TrackingDetail";
                    entry_raiWagon_Info_Tracking.sourceStationId = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_sourceStationName]) ? null : PWS0.stations.fnc_getStationId(null, this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_sourceStationName].ToString(), true));
                    entry_raiWagon_Info_Tracking.trainId = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_trainNo]) ? null : PWS0.trainBarry.fnc_getTrainIdFromDB(int.Parse(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_trainNo].ToString()), enteranceDateTime));

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_wagonNo]))
                        entry_raiWagon_Info_Tracking.wagonId = PWS0.trainBarryWagons.fnc_getWagonIdCheckWithControlNo(long.Parse(dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_wagonNo].ToString()), true);

                    entry_raiWagon_Info_Tracking.wBarType = null;
                    entry_raiWagon_Info_Tracking.wCurrentStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_currentStationName]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_currentStationName].ToString());
                    entry_raiWagon_Info_Tracking.wDestinationStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_destinationStationName]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_destinationStationName].ToString());
                    entry_raiWagon_Info_Tracking.wDistance = null;
                    entry_raiWagon_Info_Tracking.wEnteranceDate = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceDate]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceDate].ToString());
                    entry_raiWagon_Info_Tracking.wEnteranceTime = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceTime]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_enteranceTime].ToString());
                    entry_raiWagon_Info_Tracking.wExitDate = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitDate]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitDate].ToString());
                    entry_raiWagon_Info_Tracking.wExitTime = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitTime]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_exitTime].ToString());
                    entry_raiWagon_Info_Tracking.wRowNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_rowNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_rowNo].ToString()));
                    entry_raiWagon_Info_Tracking.wSourceStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_sourceStationName]) ? null : this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_sourceStationName].ToString());
                    entry_raiWagon_Info_Tracking.wWagonNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_wagonNo]) ? null : (long?)long.Parse(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_wagonNo].ToString()));
                    entry_raiWagon_Info_Tracking.wTrainNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_trainNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][wagonInfoTrackingDetailDataTable.fld_trainNo].ToString()));

                    entityLogistic.raiWagon_Info_Tracking.Add(entry_raiWagon_Info_Tracking);
                    entityLogistic.SaveChanges();
                }
            }
        }
    }

    public class wagonInfoTrackingDetailDataTable : DataTable
    {
        public wagonInfoTrackingDetailDataTable()
        {
            this.Columns.Add(new DataColumn(fld_rowNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_wagonNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_trainNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_currentStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceTime, typeof(string)));
            this.Columns.Add(new DataColumn(fld_exitDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_exitTime, typeof(string)));
            this.Columns.Add(new DataColumn(fld_sourceStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_destinationStationName, typeof(string)));
        }


        public static string fld_rowNo = "rowNo";
        public static string fld_wagonNo = "wagonNo";
        public static string fld_trainNo = "trainNo";
        public static string fld_currentStationName = "currentStationName";
        public static string fld_enteranceDate = "enteranceDate";
        public static string fld_enteranceTime = "enteranceTime";
        public static string fld_exitDate = "exitDate";
        public static string fld_exitTime = "exitTime";
        public static string fld_sourceStationName = "sourceStationName";
        public static string fld_destinationStationName = "destinationStationName";
    }
}
