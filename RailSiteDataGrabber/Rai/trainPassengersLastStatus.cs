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
    public class trainPassengersLastStatus
    {
        string v_url = "http://customers.rai.ir/Reports/Mosaferi_LastStatus/Result.asp";
        trainPassengersLastStatusDataTable v_dt;
        public trainPassengersLastStatus()
        {
            this.v_dt = new trainPassengersLastStatusDataTable();
        }

        public void readAndSaveToDB(int cycleNumber)
        {
            try
            {
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


                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

                //webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                webRequest.Proxy = null;
                webRequest.Method = "GET";

                //webRequest.Headers.Add("Cache-Control", "no-cache");
                //webRequest.ContentType = "application/x-www-form-urlencoded";

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
                        trainPassengersLastStatus.sb_fillDatatableWithHtmlTable(result, 2, 0, this.v_dt);
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
            Model.raiTrainPassengersLastStatu entry_raiTrainPassengersLastStatus;
            using (var entityLogestic = new Model.logesticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    entry_raiTrainPassengersLastStatus = new Model.raiTrainPassengersLastStatu();

                    entry_raiTrainPassengersLastStatus.areaId = (Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_areaName]) ? null : PWS0.areas.fnc_getAreaId(dt.Rows[i][trainPassengersLastStatusDataTable.fld_areaName].ToString(), false, true));
                    entry_raiTrainPassengersLastStatus.currentStationId = (Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_currentStationName]) ? null : PWS0.stations.fnc_getStationId(null, dt.Rows[i][trainPassengersLastStatusDataTable.fld_currentStationName].ToString(), true));
                    entry_raiTrainPassengersLastStatus.CycleNumber = cycleNumber;
                    entry_raiTrainPassengersLastStatus.destinationStationId = (Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_destinationStationName]) ? null : PWS0.stations.fnc_getStationId(null, dt.Rows[i][trainPassengersLastStatusDataTable.fld_destinationStationName].ToString(), true));

                    #region enteranceDate
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_enteranceDate]))
                    {
                        var solarDateTime = dt.Rows[i][trainPassengersLastStatusDataTable.fld_enteranceDate].ToString();
                        if (solarDateTime.Length == 13)
                        {
                            solarTime = int.Parse(solarDateTime.Substring(8, 5).Replace(":", ""));
                            var solarDateArr = solarDateTime.Substring(0, 8).Split('/');
                            Array.Reverse(solarDateArr);
                            solarDate = string.Join("", solarDateArr);
                            solarDate = solarDate.Replace("/", "");
                        }

                    }

                    entry_raiTrainPassengersLastStatus.enteranceDateTime = enteranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime); ;
                    #endregion

                    #region exitDate
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_exitDate]))
                    {
                        var solarDateTime = dt.Rows[i][trainPassengersLastStatusDataTable.fld_exitDate].ToString();
                        if (solarDateTime.Length == 13)
                        {
                            solarTime = int.Parse(solarDateTime.Substring(8, 5).Replace(":", ""));
                            var solarDateArr = solarDateTime.Substring(0, 8).Split('/');
                            Array.Reverse(solarDateArr);
                            solarDate = string.Join("", solarDateArr);
                            solarDate = solarDate.Replace("/", "");
                        }


                    }

                    entry_raiTrainPassengersLastStatus.exitDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime); ;
                    #endregion

                    entry_raiTrainPassengersLastStatus.FetchTime = DateTime.Now;
                    entry_raiTrainPassengersLastStatus.sourceStationId = (Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_sourceStationName]) ? null : PWS0.stations.fnc_getStationId(null, dt.Rows[i][trainPassengersLastStatusDataTable.fld_sourceStationName].ToString(), true));
                    entry_raiTrainPassengersLastStatus.trainId = (Functions.IsNull(dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainNo]) ? null : PWS0.trainBarry.fnc_getTrainIdFromDB(int.Parse(dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainNo].ToString()), enteranceDateTime));


                    entry_raiTrainPassengersLastStatus.wAreaName = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_areaName]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_areaName].ToString());
                    entry_raiTrainPassengersLastStatus.wCurrentStationName = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_currentStationName]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_currentStationName].ToString());
                    entry_raiTrainPassengersLastStatus.wDestinationStationName = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_destinationStationName]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_destinationStationName].ToString());
                    entry_raiTrainPassengersLastStatus.wEnteranceDateTime = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_enteranceDate]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_enteranceDate].ToString());
                    entry_raiTrainPassengersLastStatus.wExitDateTime = null;
                    entry_raiTrainPassengersLastStatus.wRowNo = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_rowNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_rowNo].ToString()));
                    entry_raiTrainPassengersLastStatus.wSourceStationName = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_sourceStationName]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_sourceStationName].ToString());
                    entry_raiTrainPassengersLastStatus.wTrainBossName = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainBossName]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainBossName].ToString());
                    entry_raiTrainPassengersLastStatus.wTrainNo = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainNo].ToString()));
                    entry_raiTrainPassengersLastStatus.wTrainType = (Functions.IsNull(this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainType]) ? null : this.v_dt.Rows[i][trainPassengersLastStatusDataTable.fld_trainType].ToString());

                    entityLogestic.raiTrainPassengersLastStatus.Add(entry_raiTrainPassengersLastStatus);
                    entityLogestic.SaveChanges();
                }
            }
        }

        public static void sb_fillDatatableWithHtmlTable(string html, int skipRowCount, int skipColCount, DataTable dt)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (skipRowCount < 0) throw new Exception("headerdRowCount is a negative number");
            if (dt == null) throw new Exception("dt is null");
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            var tableList = document.DocumentNode.SelectNodes("//table");
            if (tableList == null || tableList.Count == 0)
                return;
            int i, j, k;
            DataRow dr;
            HtmlAgilityPack.HtmlNode table;
            for (i = 0; i <= tableList.Count - 1; i++)
            {
                if (tableList[i].Id.ToLower() == "table6" && i <= tableList.Count - 2)
                {
                    //the next two table after table6 is important
                    table = tableList[i + 2];
                    if (table == null) return;
                    var rows = table.SelectNodes("tr");

                    string cellContent;
                    for (j = skipRowCount; j <= rows.Count - 1; j++)
                    {
                        var cells = rows[j].SelectNodes("td");
                        dr = dt.NewRow();
                        for (k = skipColCount; k <= cells.Count - 1; k++)
                        {
                            cellContent = cells[k].InnerText.Replace("&nbsp;", "").TrimEnd().TrimStart();
                            cellContent = cellContent.Replace("<br>", " ");
                            cellContent = cellContent.Replace("\r\n", " ");
                            if (j - skipColCount < dt.Columns.Count)
                                dr[k - skipColCount] = string.IsNullOrEmpty(cellContent) ? Convert.DBNull : cellContent;
                        }
                        dt.Rows.Add(dr);
                    }
                }


            }
        }
    }

    public class trainPassengersLastStatusDataTable : DataTable
    {
        public trainPassengersLastStatusDataTable()
        {
            this.Columns.Add(new DataColumn(fld_areaName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_exitDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_currentStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_trainBossName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_destinationStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_sourceStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_trainType, typeof(string)));
            this.Columns.Add(new DataColumn(fld_trainNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_rowNo, typeof(int)));
        }


        public static string fld_areaName = "areaName";
        public static string fld_exitDate = "exitDate";
        public static string fld_enteranceDate = "enteranceDate";
        public static string fld_currentStationName = "stationName";
        public static string fld_trainBossName = "trainBossName";
        public static string fld_destinationStationName = "distancestationName";
        public static string fld_sourceStationName = "sourceStationName";
        public static string fld_trainType = "trainType";
        public static string fld_trainNo = "trainNo";
        public static string fld_rowNo = "rowNo";


    }
}
