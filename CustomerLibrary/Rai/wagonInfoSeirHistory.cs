using PWSLibrary.PWS0;
using SharedLibrary;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLibrary.Rai
{
    public class wagonInfoSeirHistory
    {
        string v_url = "http://customers.rai.ir/Wagon_Info/Wagon_Seir_History/Result.asp";
        wagonInfoSeirHistoryDataTable v_dt;
        public wagonInfoSeirHistory()
        {
            this.v_dt = new wagonInfoSeirHistoryDataTable();

        }
        public bool readAndSaveWagonToDB(long wagonNo, DateTime fromDate, DateTime toDate, int cycleNumber)
        {
            return this.readAndSaveWagonToDB(wagonNo, Functions.miladiToSolar(fromDate), Functions.miladiToSolar(toDate), cycleNumber);
        }

        public bool readAndSaveWagonToDB(long wagonNo, string solarDateFrom, string solarDateTo, int cycleNumber)
        {
            SharedVariables.logs.Info("Start Fetch " + wagonNo + " from " + solarDateFrom + " to " + solarDateTo);

            using (var entityLogistics = new logisticEntities())
            {
                customersHistoryFetchLog entryCustomersHistoryFetchLog = new customersHistoryFetchLog();
                entryCustomersHistoryFetchLog.endDate = solarDateTo;
                entryCustomersHistoryFetchLog.fetchTimeEnd = null;
                entryCustomersHistoryFetchLog.fetchTimeStart = DateTime.Now;
                entryCustomersHistoryFetchLog.startDate = solarDateFrom;
                entryCustomersHistoryFetchLog.state = 0;
                entryCustomersHistoryFetchLog.wagonNo = wagonNo.ToString();
                entityLogistics.customersHistoryFetchLogs.Add(entryCustomersHistoryFetchLog);
                entityLogistics.SaveChanges();

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
                        SharedVariables.logs.Error("wagonInfoSeirHistory.readAndSaveWagonToDB.solarToMiladi,solarDateFrom", ex);
                        entryCustomersHistoryFetchLog.state = -1;
                        entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                        entityLogistics.SaveChanges();
                        return false;
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
                        SharedVariables.logs.Error("wagonInfoSeirHistory.readAndSaveWagonToDB.solarToMiladi,solarDateTo", ex);
                        entryCustomersHistoryFetchLog.state = -1;
                        entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                        entityLogistics.SaveChanges();
                        return false;
                    }
                    if (solarDateTo.Length == 10)
                        solarDateTo = solarDateTo.Remove(0, 2);
                    solarDateTo = solarDateTo.Replace("/", "");

                    if (dateFrom > dateTo)
                    {
                        SharedVariables.logs.Error("wagonInfoSeirHistory.readAndSaveWagonToDB.solarToMiladi, SolarDateFrom should be lower than SolarDateTo");
                        entryCustomersHistoryFetchLog.state = -1;
                        entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                        entityLogistics.SaveChanges();
                        return false;
                    }
                    login lg = new login();
                    start:
                    if (login.v_cookieCollection == null)
                    {

                        if (!lg.fnc_login())
                        {
                            //cannot login
                            entryCustomersHistoryFetchLog.state = -1;
                            entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                            entityLogistics.SaveChanges();
                            return false;
                        }
                    }
                    Uri uri;
                    uri = new Uri(this.v_url);
                    string postData;
                    int pageNumber = 1;
                    wagonInfoSeirHistoryDataTable dt = new wagonInfoSeirHistoryDataTable();
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
                        SharedVariables.logs.Info(postData);
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
                                entryCustomersHistoryFetchLog.state = -1;
                                entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                                entityLogistics.SaveChanges();
                                return false;
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
                                    entryCustomersHistoryFetchLog.state = -1;
                                    entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                                    entityLogistics.SaveChanges();
                                    return false;
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
                                dt.Rows.Clear();
                                Functions.sb_fillDatatableWithHtmlTableId(result, "AutoNumber2", 2, 0, 0, 0, dt);
                                if (dt.Rows.Count > 0)
                                {
                                    this.sb_saveToDB(dt, cycleNumber);
                                }
                            }

                        }
                        pageNumber++;
                    } while (dt.Rows.Count > 0);
                    SharedVariables.logs.Info("End Fetch " + wagonNo + " from " + solarDateFrom + " to " + solarDateTo);
                    entryCustomersHistoryFetchLog.state = 1;
                    entryCustomersHistoryFetchLog.fetchTimeEnd = DateTime.Now;
                    entityLogistics.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        SharedVariables.logs.Error("wagonInfoSeirHistory.readAndSaveWagonToDB.finalException   Entity of type \"" + eve.Entry.Entity.GetType().Name
                            + "\" in state \"" + eve.Entry.State + "\" has the following validation errors:");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            SharedVariables.logs.Error("- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"");
                        }
                    }
                    return false;
                    
                }
                catch (Exception ex)
                {
                    SharedVariables.logs.Error("wagonInfoSeirHistory.readAndSaveWagonToDB.finalException", ex);
                    return false;
                }
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
            string solarTime;
            DateTime? entranceDateTime;
            customersHistory entryCustomersHistory;
            bool add;
            string currentStationName;
            string wagonNo;
            string trainNo;
            string billOfLadingNo;
            using (var entityLogistic = new logisticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    try
                    {
                        wagonNo = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo].ToString());
                        trainNo = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo].ToString());
                        currentStationName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName].ToString());
                        billOfLadingNo = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo].ToString());
                        #region enteranceDate
                        solarDate = null;
                        solarTime = null;
                        if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate]))
                        {
                            solarDate = "13" + dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate].ToString().Substring(0, 2) + "/"
                                + dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate].ToString().Substring(2, 2) + "/"
                                + dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate].ToString().Substring(4, 2);
                        }

                        if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime]))
                            solarTime = dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime].ToString();
                        if (!Functions.IsNull(solarTime))
                            entranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, int.Parse(solarTime.Replace(":", "")));
                        else entranceDateTime = null;
                        #endregion

                        add = true;
                        entryCustomersHistory = entityLogistic.customersHistories.Where(o =>
                        o.wagonControlNo == wagonNo
                        && o.trainNo == trainNo
                        && o.currentStationName == currentStationName
                        && o.entranceSolarDate == solarDate
                        && o.entranceTime == solarTime
                        && o.billOfLadingNo == billOfLadingNo).FirstOrDefault();
                        if (entryCustomersHistory == null)
                        {
                            add = true;
                            entryCustomersHistory = new customersHistory();
                        }
                        else
                        {
                            add = false;
                        }



                        entryCustomersHistory.wagonControlNo = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo].ToString());
                        entryCustomersHistory.trainNo = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo].ToString());
                        entryCustomersHistory.areaName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_areaName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_areaName].ToString());
                        entryCustomersHistory.currentStationName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName].ToString());
                        entryCustomersHistory.entranceSolarDate = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate]) ? null : solarDate);
                        entryCustomersHistory.entranceTime = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime].ToString());
                        entryCustomersHistory.distance = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_distance]) ? null : (int?)int.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_distance].ToString()));
                        entryCustomersHistory.goodsType = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_goodsType]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_goodsType].ToString());
                        entryCustomersHistory.sourceStationName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName].ToString());
                        entryCustomersHistory.destinationStationName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName].ToString());
                        entryCustomersHistory.billOfLadingNo = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo].ToString());
                        entryCustomersHistory.trainSourcestationName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainSourceStationName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainDestinationStationName].ToString());
                        entryCustomersHistory.trainDestinationStationName = (Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainDestinationStationName]) ? null : dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainDestinationStationName].ToString());
                        entryCustomersHistory.entranceDateTime = entranceDateTime;
                        entryCustomersHistory.fetchTime = DateTime.Now;

                        if (add)
                            entityLogistic.customersHistories.Add(entryCustomersHistory);
                        entityLogistic.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            SharedVariables.logs.Error("wagonInfoSeirHistory.sb_savetoDb  Entity of type \"" + eve.Entry.Entity.GetType().Name
                                + "\" in state \"" + eve.Entry.State + "\" has the following validation errors:");
                            foreach (var ve in eve.ValidationErrors)
                            {
                                SharedVariables.logs.Error("- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SharedVariables.logs.Error("wagonInfoSeirHistory.sb_savetoDb", ex);
                    }
                }
            }
        }

        private void sb_saveToDBOld(DataTable dt, int cycleNumber)
        {
            if (dt == null)
            {
                return;
            }
            int i;
            string solarDate;
            int? solarTime;
            DateTime? enteranceDateTime;
            raiWagon_Info_Tracking entry_raiWagon_Info_Tracking;
            using (var entityLogistic = new logisticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    entry_raiWagon_Info_Tracking = new raiWagon_Info_Tracking();

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName]))
                        entry_raiWagon_Info_Tracking.currentStationId = stations.fnc_getStationId(null, dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName].ToString(), true);

                    entry_raiWagon_Info_Tracking.CycleNumber = cycleNumber;

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName]))
                        entry_raiWagon_Info_Tracking.destinationStationId = stations.fnc_getStationId(null, dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName].ToString(), true);

                    #region enteranceDate
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate] != null))
                    {
                        var solarDateArr = dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate].ToString().Split('/');
                        Array.Reverse(solarDateArr);
                        solarDate = string.Join("", solarDateArr);
                        solarDate = solarDate.Replace("/", "");
                    }

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime]))
                        solarTime = int.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime].ToString().Replace(":", ""));
                    entry_raiWagon_Info_Tracking.enteranceDateTime = enteranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime); ;
                    #endregion

                    entry_raiWagon_Info_Tracking.exitDateTime = null;
                    entry_raiWagon_Info_Tracking.FetchTime = DateTime.Now;

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_goodsType]))
                        entry_raiWagon_Info_Tracking.goodsID = goods.fnc_getGoodsId(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_goodsType].ToString()
                            , false, true);

                    entry_raiWagon_Info_Tracking.Source = "History";
                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName]))
                        entry_raiWagon_Info_Tracking.sourceStationId = stations.fnc_getStationId(null, dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_sourceStationName].ToString(), true);

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo]))
                        entry_raiWagon_Info_Tracking.trainId = trainBarry.fnc_getTrainIdFromDB(int.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_trainNo].ToString()), enteranceDateTime);

                    if (!Functions.IsNull(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo]))
                        entry_raiWagon_Info_Tracking.wagonId = trainBarryWagons.fnc_getWagonIdCheckWithControlNo(long.Parse(dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_wagonNo].ToString()), true);

                    entry_raiWagon_Info_Tracking.wAreaName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_areaName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_areaName].ToString());
                    entry_raiWagon_Info_Tracking.wBarType = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_goodsType]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_goodsType].ToString());
                    entry_raiWagon_Info_Tracking.wBillOfLadingNo = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo]) ? null : (long?)long.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_billOfLadingNo].ToString()));
                    entry_raiWagon_Info_Tracking.wCurrentStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_currentStationName].ToString());
                    entry_raiWagon_Info_Tracking.wDestinationStationName = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_destinationStationName].ToString());
                    entry_raiWagon_Info_Tracking.wDistance = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_distance]) ? null : (int?)int.Parse(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_distance].ToString()));
                    entry_raiWagon_Info_Tracking.wEnteranceDate = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceSolarDate].ToString());
                    entry_raiWagon_Info_Tracking.wEnteranceTime = (Functions.IsNull(this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime]) ? null : this.v_dt.Rows[i][wagonInfoSeirHistoryDataTable.fld_entranceTime].ToString());
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
            this.Columns.Add(new DataColumn(fld_goodsType, typeof(string)));
            this.Columns.Add(new DataColumn(fld_distance, typeof(string)));
            this.Columns.Add(new DataColumn(fld_entranceTime, typeof(string)));
            this.Columns.Add(new DataColumn(fld_entranceSolarDate, typeof(string)));
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
        public static string fld_entranceSolarDate = "entranceSolarDate";
        public static string fld_entranceTime = "entranceTime";
        public static string fld_distance = "distance";
        public static string fld_goodsType = "goodsType";
        public static string fld_sourceStationName = "sourceStationName";
        public static string fld_destinationStationName = "destinationStationName";
        public static string fld_billOfLadingNo = "billOfLadingNo";
        public static string fld_trainSourceStationName = "trainSourceStationName";
        public static string fld_trainDestinationStationName = "trainDestinationStationName";

    }
}
