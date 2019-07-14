using SharedLibrary;
using SharedLibrary.Models;
using PWSLibrary.PWS0;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomerLibrary.Rai
{
    public class billOfLadingsTracking
    {
        string v_url = "http://customers.rai.ir/Barname/Result.asp";
        billOfLadingsTrackingDataTable v_dt;
        public billOfLadingsTracking()
        {
            this.v_dt = new billOfLadingsTrackingDataTable();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billOfLadingRange">1-10 or 1,2,3,4,5,6,7,8,9,10</param>
        /// <param name="cycleNumber"></param>
        /// <param name="get30DaysBeforeInformation"></param>
        public void readAndSaveWagonToDB(string billOfLadingRange, int cycleNumber)
        {
            try
            {
                string postData;

                #region check and assign post parameters
                if (string.IsNullOrEmpty(billOfLadingRange))
                {
                    throw new Exception("billOfLadingRange is not specified");
                }
                int i, temp1, temp2;
                if (billOfLadingRange.Contains("-"))
                {
                    //we have 1-10
                    var billOfLadingRangeArr = billOfLadingRange.Split('-');
                    if (billOfLadingRangeArr.Length != 2)
                    {
                        throw new Exception("billOfLadingRange is not specified correctly(" + billOfLadingRange + ")");
                    }
                    if (!int.TryParse(billOfLadingRangeArr[0], out temp1)
                        || !int.TryParse(billOfLadingRangeArr[1], out temp2))
                    {
                        throw new Exception("billOfLadingRange is not specified correctly(" + billOfLadingRange + ")");
                    }
                    if (temp1 > temp2)
                    {
                        throw new Exception("StartNumber of billOfLadingRange is greater than EndNumber (" + billOfLadingRange + ")");
                    }
                    if (temp2 - temp1 > 20)
                    {
                        throw new Exception("EndNumber - StartNumber >20 (" + billOfLadingRange + ")");
                    }

                    postData = "FBarnameNo=" + temp1.ToString() + "&TBarnameNo=" + temp2.ToString()
                    + "&BNumber=&Barname_No=";
                }
                else
                {
                    //we have 1,2,3,4,5,6,7,8,9,10
                    var billOfLadingsArr = billOfLadingRange.Split(',');
                    if (billOfLadingsArr.Length > 20)
                    {
                        throw new Exception("You can specify atleast 20 numbers which is seperated by , (" + billOfLadingRange + ")");
                    }
                    for (i = 0; i <= billOfLadingsArr.Length - 1; i++)
                    {
                        if (!int.TryParse(billOfLadingsArr[i], out temp1))
                        {
                            throw new Exception("billOfLadingRange is not specified correctly(" + billOfLadingsArr[i] + ")");
                        }
                    }
                    postData = "FBarnameNo=&TBarnameNo=&BNumber=&Barname_No=" + billOfLadingRange;
                }
                #endregion


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
                webRequest.Method = "POST";

                //webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.ContentType = "application/x-www-form-urlencoded";

                webRequest.CookieContainer = new CookieContainer();
                foreach (Cookie cookie in login.v_cookieCollection)
                {
                    webRequest.CookieContainer.Add(cookie);
                }


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
                        Functions.sb_fillDatatableWithHtmlTableId(result, "AutoNumber2", 2, 0, 0, 0, this.v_dt);
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
            raiBillOfLadingsTracking entry_raiBillOfLadingsTracking;
            using (var entityLogistic = new logisticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    entry_raiBillOfLadingsTracking = new raiBillOfLadingsTracking();

                    entry_raiBillOfLadingsTracking.billOfLadingId = (Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_billOfLadingNo]) ? null : trainBarryBillOfLadings.fnc_getBillOfLadingId(long.Parse(dt.Rows[i][billOfLadingsTrackingDataTable.fld_billOfLadingNo].ToString()), true));
                    entry_raiBillOfLadingsTracking.currentStationId = (Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_currentStationName]) ? null : stations.fnc_getStationId(null, dt.Rows[i][billOfLadingsTrackingDataTable.fld_currentStationName].ToString(), true));
                    entry_raiBillOfLadingsTracking.CycleNumber = cycleNumber;
                    entry_raiBillOfLadingsTracking.destinationStationId = (Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_destinationStationName]) ? null : stations.fnc_getStationId(null, dt.Rows[i][billOfLadingsTrackingDataTable.fld_destinationStationName].ToString(), true));

                    #region enteranceDate
                    solarDate = null;
                    solarTime = null;
                    if (!Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceDate] != null))
                    {
                        var solarDateArr = dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceDate].ToString().Split('/');
                        Array.Reverse(solarDateArr);
                        solarDate = string.Join("", solarDateArr);
                        solarDate = solarDate.Replace("/", "");
                    }

                    if (!Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceTime]))
                        solarTime = int.Parse(dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceTime].ToString().Replace(":", ""));
                    entry_raiBillOfLadingsTracking.enteranceDateTime = enteranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(solarDate, solarTime); ;
                    #endregion

                    entry_raiBillOfLadingsTracking.FetchTime = DateTime.Now;
                    entry_raiBillOfLadingsTracking.sourceStationId = (Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_sourceStationName]) ? null : stations.fnc_getStationId(null, dt.Rows[i][billOfLadingsTrackingDataTable.fld_sourceStationName].ToString(), true));
                    entry_raiBillOfLadingsTracking.trainId = (Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_trainNo]) ? null : trainBarry.fnc_getTrainIdFromDB(int.Parse(dt.Rows[i][billOfLadingsTrackingDataTable.fld_trainNo].ToString()), enteranceDateTime));
                    entry_raiBillOfLadingsTracking.wagonId = (Functions.IsNull(dt.Rows[i][billOfLadingsTrackingDataTable.fld_wagonNo]) ? null : trainBarryWagons.fnc_getWagonIdCheckWithControlNo(long.Parse(dt.Rows[i][billOfLadingsTrackingDataTable.fld_wagonNo].ToString()), true));

                    entry_raiBillOfLadingsTracking.wBillOfLadingNo = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_billOfLadingNo]) ? null : (long?)long.Parse(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_billOfLadingNo].ToString()));
                    entry_raiBillOfLadingsTracking.wCurrentStationName = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_currentStationName]) ? null : this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_currentStationName].ToString());
                    entry_raiBillOfLadingsTracking.wDestinationStationName = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_destinationStationName]) ? null : this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_destinationStationName].ToString());
                    entry_raiBillOfLadingsTracking.wDistance = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_distance]) ? null : (int?)int.Parse(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_distance].ToString()));
                    entry_raiBillOfLadingsTracking.wEnteranceDate = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceDate]) ? null : this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceDate].ToString());
                    entry_raiBillOfLadingsTracking.wEnteranceTime = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceTime]) ? null : this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_enteranceTime].ToString());
                    entry_raiBillOfLadingsTracking.wRowNo = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_rowNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_rowNo].ToString()));
                    entry_raiBillOfLadingsTracking.wSourceStationName = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_sourceStationName]) ? null : this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_sourceStationName].ToString());
                    entry_raiBillOfLadingsTracking.wTrainNo = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_trainNo]) ? null : (int?)int.Parse(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_trainNo].ToString()));
                    entry_raiBillOfLadingsTracking.wWagonNo = (Functions.IsNull(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_wagonNo]) ? null : (long?)long.Parse(this.v_dt.Rows[i][billOfLadingsTrackingDataTable.fld_wagonNo].ToString()));

                    entityLogistic.raiBillOfLadingsTrackings.Add(entry_raiBillOfLadingsTracking);
                    entityLogistic.SaveChanges();
                }
            }
        }
    }

    public class billOfLadingsTrackingDataTable : DataTable
    {
        public billOfLadingsTrackingDataTable()
        {
            this.Columns.Add(new DataColumn(fld_rowNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_billOfLadingNo, typeof(long)));
            this.Columns.Add(new DataColumn(fld_wagonNo, typeof(long)));
            this.Columns.Add(new DataColumn(fld_currentStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_enteranceTime, typeof(string)));
            this.Columns.Add(new DataColumn(fld_trainNo, typeof(int)));
            this.Columns.Add(new DataColumn(fld_sourceStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_destinationStationName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_distance, typeof(int)));

        }


        public static string fld_rowNo = "rowNo";
        public static string fld_billOfLadingNo = "billOfLadingNo";
        public static string fld_wagonNo = "wagonNo";
        public static string fld_currentStationName = "currentStationName";
        public static string fld_enteranceDate = "enteranceDate";
        public static string fld_enteranceTime = "enteranceTime";
        public static string fld_trainNo = "trainNo";
        public static string fld_sourceStationName = "sourceStationName";
        public static string fld_destinationStationName = "destinationStationName";
        public static string fld_distance = "distance";

    }
}
