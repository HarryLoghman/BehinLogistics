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
    class wagonInfoSeirHistory
    {
        string v_url = "http://customers.rai.ir/Wagon_Info/Wagon_Seir_History/Result.asp";
        wagonInfoSeirHistoryDataTable v_dt;
        public wagonInfoSeirHistory()
        {
            this.v_dt = new wagonInfoSeirHistoryDataTable();

        }

        public void readAndSaveWagonToDB(long wagonNo, string solarDateFrom, string solarDateTo, int cycleNumber)
        {
            wagonInfoTrackingDetail wagonDetail = new wagonInfoTrackingDetail();
            try
            {
                DateTime dateFrom, dateTo;
                dateFrom = DateTime.MinValue;
                dateTo = DateTime.MaxValue;
                try
                {
                    dateFrom = Functions.solarToMiladi(solarDateFrom);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error in date format solarDateFrom", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                if (solarDateFrom.Length == 10)
                    solarDateFrom = solarDateFrom.Remove(0, 2);
                solarDateFrom = solarDateFrom.Replace("/", "");

                try
                {
                    dateTo = Functions.solarToMiladi(solarDateTo);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error in date format solarDateTo", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
                if (solarDateTo.Length == 10)
                    solarDateTo = solarDateTo.Remove(0, 2);
                solarDateTo = solarDateTo.Replace("/", "");

                if (dateFrom > dateTo)
                {
                    System.Windows.Forms.MessageBox.Show("SolarDateFrom should be lower than SolarDateTo", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }
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
                uri = new Uri(this.v_url);
                string postData;
                int pageNumber = 1;
                do
                {

                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
                    
                    //webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                    webRequest.Proxy = null;
                    webRequest.Method = "POST";

                    //webRequest.Headers.Add("Cache-Control", "no-cache");
                    webRequest.ContentType = "application/x-www-form-urlencoded";

                    webRequest.CookieContainer = new CookieContainer();
                    foreach (Cookie cookie in login.v_cookieCollection)
                    {
                        webRequest.CookieContainer.Add(cookie);
                    }




                    postData = "WagonNo=" + wagonNo.ToString()
                        + "&From_Date=" + solarDateFrom.ToString()
                        + "&To_Date=" + solarDateTo.ToLower()
                        + "&Page=" + pageNumber.ToString();
                    byte[] formData = Encoding.ASCII.GetBytes(postData);

                    webRequest.ContentLength = formData.Length;
                    Stream streamRequest = webRequest.GetRequestStream();
                    streamRequest.Write(formData, 0, formData.Length);
                    streamRequest.Flush();
                    streamRequest.Close();

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
                    pageNumber++;
                } while (this.v_dt.Rows.Count > 0);

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



                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName]))
                        entry_raiWagon_Info_Tracking.currentStationId = PWS0.stations.fnc_getStationId(null, dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName].ToString(), true);

                    entry_raiWagon_Info_Tracking.CycleNumber = cycleNumber;

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName]))
                        entry_raiWagon_Info_Tracking.destinationStationId = PWS0.stations.fnc_getStationId(null, dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName].ToString(), true);

                    #region enteranceDate
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceDate] != null))
                    {
                        var solarDateArr = dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceDate].ToString().Split('/');
                        Array.Reverse(solarDateArr);
                        solarDate = string.Join("", solarDateArr);
                        solarDate = solarDate.Replace("/", "");
                    }

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceTime]))
                        solarTime = int.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceTime].ToString().Replace(":", ""));
                    entry_raiWagon_Info_Tracking.enteranceDateTime = enteranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime); ;
                    #endregion

                    entry_raiWagon_Info_Tracking.exitDateTime = null;
                    entry_raiWagon_Info_Tracking.FetchTime = DateTime.Now;

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_barType]))
                        entry_raiWagon_Info_Tracking.goodsID = PWS0.goods.fnc_getGoodsId(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_barType].ToString()
                            , false, true);

                    entry_raiWagon_Info_Tracking.Source = "History";
                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName]))
                        entry_raiWagon_Info_Tracking.sourceStationId = PWS0.stations.fnc_getStationId(null, dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName].ToString(), true);

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo]))
                        entry_raiWagon_Info_Tracking.trainId = PWS0.trainBarry.fnc_getTrainIdFromDB(int.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo].ToString()), enteranceDateTime);

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo]))
                        entry_raiWagon_Info_Tracking.wagonId = PWS0.trainBarryWagons.fnc_getWagonIdCheckWithControlNo(long.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo].ToString()), true);

                    entry_raiWagon_Info_Tracking.wAreaName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_areaName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_areaName].ToString());
                    entry_raiWagon_Info_Tracking.wBarType = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_barType]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_barType].ToString());
                    entry_raiWagon_Info_Tracking.wBillOfLadingNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo]) ? null : (long?)long.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo].ToString()));
                    entry_raiWagon_Info_Tracking.wCurrentStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName].ToString());
                    entry_raiWagon_Info_Tracking.wDestinationStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName].ToString());
                    entry_raiWagon_Info_Tracking.wDistance = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_distance]) ? null : (int?)int.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_distance].ToString()));
                    entry_raiWagon_Info_Tracking.wEnteranceDate = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceDate]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceDate].ToString());
                    entry_raiWagon_Info_Tracking.wEnteranceTime = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceTime]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_enteranceTime].ToString());
                    entry_raiWagon_Info_Tracking.wExitDate = null;
                    entry_raiWagon_Info_Tracking.wExitTime = null;
                    entry_raiWagon_Info_Tracking.wRowNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_rowNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_rowNo].ToString()));
                    entry_raiWagon_Info_Tracking.wSourceStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName].ToString());
                    entry_raiWagon_Info_Tracking.wTrainDestinationStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainDestinationStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainDestinationStationName].ToString());
                    entry_raiWagon_Info_Tracking.wTrainNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo].ToString()));
                    entry_raiWagon_Info_Tracking.wTrainSourceStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainSourceStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainSourceStationName].ToString());
                    entry_raiWagon_Info_Tracking.wWagonNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo]) ? null : (long?)long.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo].ToString()));

                    entityLogistic.raiWagon_Info_Tracking.Add(entry_raiWagon_Info_Tracking);
                    entityLogistic.SaveChanges();
                }
            }
        }
    }

    public class wagonInfoSeirHistoryDataTable : DataTable
    {
        public wagonInfoSeirHistoryDataTable()
        {
            this.Columns.Add(new DataColumn(fld_trainDestinationStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_trainSourceStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_billOfLadingNo, typeof(long)));
            this.Columns.Add(new DataColumn(fld_destinationStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_sourceStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_barType, typeof(string)));
            this.Columns.Add(new DataColumn(fld_distance, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceTime, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_currentStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_areaName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_trainNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_wagonNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_rowNo, typeof(int)));
        }


        public static string fld_rowNo = "rowNo";
        public static string fld_wagonNo = "wagonNo";
        public static string fld_trainNo = "trainNo";
        public static string fld_areaName = "areaName";
        public static string fld_currentStationName = "currentStationName";
        public static string fld_enteranceDate = "enteranceDate";
        public static string fld_enteranceTime = "enteranceTime";
        public static string fld_distance = "distance";
        public static string fld_barType = "barType";
        public static string fld_sourceStationName = "sourceStationName";
        public static string fld_destinationStationName = "destinationStationName";
        public static string fld_billOfLadingNo = "billOfLadingNo";
        public static string fld_trainSourceStationName = "trainSourceStationName";
        public static string fld_trainDestinationStationName = "trainDestinationStationName";

    }
}
