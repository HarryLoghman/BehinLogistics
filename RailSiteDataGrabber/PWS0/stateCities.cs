using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.PWS0
{
    public class stateCities
    {
        public string v_urlState = "http://pws0.rai.ir/Home/getStateList";
        public string v_urlCity = "http://pws0.rai.ir/Home/getCityList";
        public void sb_readAndSaveStatesToDB(bool saveCitiesForEachState)
        {
            try
            {
                Uri uri = new Uri(this.v_urlState);
                StateCityJsonModel[] statesJson = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "GET";
                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse != null)
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string result = rd.ReadToEnd();
                        statesJson = Newtonsoft.Json.JsonConvert.DeserializeObject<StateCityJsonModel[]>(result);
                    }
                    webResponse.Close();

                    if (statesJson != null)
                    {
                        using (var entityLogistic = new Model.logisticEntities())
                        {
                            foreach (StateCityJsonModel state in statesJson)
                            {
                                this.sb_saveStateToDB(state, saveCitiesForEachState);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }

        private void sb_saveStateToDB(StateCityJsonModel stateJson, bool saveCitiesForEachState)
        {
            using (var entityLogistic = new Model.logisticEntities())
            {
                bool add = false;
                Model.PWS0StateCities entryPWS0State = entityLogistic.PWS0StateCities.FirstOrDefault(o => o.StateId == stateJson.StateID
                  && o.isState.HasValue && o.isState.Value);

                if (entryPWS0State == null)
                {
                    add = true;
                    entryPWS0State = new Model.PWS0StateCities();
                }
                entryPWS0State.CityId = null;
                entryPWS0State.CityName = null;
                entryPWS0State.isState = true;
                entryPWS0State.ParentIdInDB = null;
                entryPWS0State.ParentStateIdInJson = null;
                entryPWS0State.StateId = stateJson.StateID;
                entryPWS0State.StateName = stateJson.StateName;

                if (add)
                {
                    entityLogistic.PWS0StateCities.Add(entryPWS0State);
                }

                entityLogistic.SaveChanges();
                //to get id of stationid
                int parentIdInDB = entryPWS0State.Id;

                if (saveCitiesForEachState && stateJson.StateID.HasValue)
                {
                    this.sb_readAndSaveCitiesToDB(stateJson.StateID.Value, parentIdInDB);
                }
            }
        }

        public void sb_readAndSaveCitiesToDB(int stateId, int? parentId)
        {
            try
            {
                Uri uri;
                uri = new Uri(this.v_urlCity);

                StateCityJsonModel[] cities = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("Cookie", "__RequestVerificationToken=Gm4hoEFS8RkxaSfJxpaeorLztRy6-rfjyKm1pRyPYQP-4dGSPAKrHWG62K3vaJOz_k7xEJBWigJHetij4ID_TmbP-27KPhaINnSun-IMnZc1;");
                webRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW";

                //  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--"

                string postData;
                postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ostanid\"\r\n\r\n" + stateId.ToString() + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
                byte[] formData = Encoding.UTF8.GetBytes(postData);

                Stream streamRequest = webRequest.GetRequestStream();
                streamRequest.Write(formData, 0, formData.Length);
                streamRequest.Flush();
                streamRequest.Close();

                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse != null)
                {
                    string result;
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        result = rd.ReadToEnd();
                    }

                    webResponse.Close();
                    cities = Newtonsoft.Json.JsonConvert.DeserializeObject<StateCityJsonModel[]>(result);

                    if (cities != null)
                    {
                        using (var entityLogistic = new Model.logisticEntities())
                        {
                            foreach (StateCityJsonModel city in cities)
                            {
                                this.sb_saveCityToDB(city, stateId, parentId);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityJson"></param>
        /// <param name="parentIdInDB">refer to Id which is saved automatically in db</param>
        /// <param name="parentStateIdInJson">refer to stateId which comes from JSON</param>
        private void sb_saveCityToDB(StateCityJsonModel cityJson, int parentStateIdInJson, int? parentIdInDB)
        {
            using (var entityLogistic = new Model.logisticEntities())
            {
                if (!parentIdInDB.HasValue)
                {
                    var entryState = entityLogistic.PWS0StateCities.FirstOrDefault(o => o.ParentStateIdInJson == parentStateIdInJson && (o.isState.HasValue && o.isState.Value));
                    if (entryState != null)
                        parentIdInDB = entryState.Id;
                }
                bool add = false;
                Model.PWS0StateCities entryPWS0City = entityLogistic.PWS0StateCities.FirstOrDefault(o => o.CityId == cityJson.CityID
                  && (!o.isState.HasValue || !o.isState.Value));

                if (entryPWS0City == null)
                {
                    add = true;
                    entryPWS0City = new Model.PWS0StateCities();
                }
                entryPWS0City.CityId = cityJson.CityID;
                entryPWS0City.CityName = cityJson.CityName;
                entryPWS0City.isState = false;
                entryPWS0City.ParentIdInDB = parentIdInDB;
                entryPWS0City.ParentStateIdInJson = parentStateIdInJson;
                entryPWS0City.StateId = null;
                entryPWS0City.StateName = null;
                if (add)
                {
                    entityLogistic.PWS0StateCities.Add(entryPWS0City);
                }

                entityLogistic.SaveChanges();


            }
        }
    }

    public class StateCityJsonModel
    {
        public int? StateID;
        public int? CityID;
        public string StateName;
        public string CityName;
    }

}
