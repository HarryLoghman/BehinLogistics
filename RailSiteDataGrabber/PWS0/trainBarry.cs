using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.PWS0
{
    public class trainBarry
    {
        public string v_url = "http://pws0.rai.ir/Bari/getBariTrainTrackList";
        public string v_urlFindTrain = "http://pws0.rai.ir/Bari/findTrain";
        public void sb_readAndSaveToDB(int cycleNo, bool getLocomotives, bool getBillOfLadings)
        {

            try
            {
                Uri uri;
                uri = new Uri(this.v_url);

                trainBarryJsonModel[] trainBarryListJson = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("Cookie", "__RequestVerificationToken=Gm4hoEFS8RkxaSfJxpaeorLztRy6-rfjyKm1pRyPYQP-4dGSPAKrHWG62K3vaJOz_k7xEJBWigJHetij4ID_TmbP-27KPhaINnSun-IMnZc1;");
                webRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW";

                //  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--"

                string postData;
                postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
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
                    trainBarryListJson = Newtonsoft.Json.JsonConvert.DeserializeObject<trainBarryJsonModel[]>(result);

                    if (trainBarryListJson != null)
                    {
                        using (var entityLogistic = new Model.logisticEntities())
                        {
                            foreach (trainBarryJsonModel train in trainBarryListJson)
                            {
                                this.sb_saveToDB(train, cycleNo, getLocomotives, getBillOfLadings);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }

        public void sb_readAndSaveTrainToDB(int trainNo, int cycleNo, bool getLocomotives, bool getBillOfLadings)
        {

            try
            {
                trainBarryJsonModel[] trainBarryJsonList = this.fnc_findTrainBarry(trainNo);
                if (trainBarryJsonList != null)
                {
                    foreach (trainBarryJsonModel train in trainBarryJsonList)
                    {
                        this.sb_saveToDB(train, cycleNo, getLocomotives, getBillOfLadings);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public trainBarryJsonModel[] fnc_findTrainBarry(int train_no)
        {

            trainBarryJsonModel[] trainBarryListJson = null;
            try
            {
                Uri uri;
                uri = new Uri(this.v_urlFindTrain);

                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("Cookie", "__RequestVerificationToken=Gm4hoEFS8RkxaSfJxpaeorLztRy6-rfjyKm1pRyPYQP-4dGSPAKrHWG62K3vaJOz_k7xEJBWigJHetij4ID_TmbP-27KPhaINnSun-IMnZc1;");
                webRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW";

                //  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--"

                string postData;
                postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW"
                    + "\r\nContent-Disposition: form-data; name=\"train_no\"\r\n\r\n" + train_no.ToString() + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
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
                    trainBarryListJson = Newtonsoft.Json.JsonConvert.DeserializeObject<trainBarryJsonModel[]>(result);
                }
            }
            catch (Exception ex)
            {

            }
            return trainBarryListJson;
        }

        private void sb_saveToDB(trainBarryJsonModel train, int cycleNumber, bool getLocomotives, bool getBillOfLadings)
        {
            trainBarryBillOfLadings billOfLadings = new trainBarryBillOfLadings();
            trainBarryLocomotives locos = new trainBarryLocomotives();
            using (var entityLogistic = new Model.logisticEntities())
            {
                bool add = false;
                Model.PWS0TrainsBarry entryPWS0Train = entityLogistic.PWS0TrainsBarry.FirstOrDefault(o =>
                o.jCurrent_Station_Code == train.Current_Station_Code
                  && o.jDestination_Station_Code == train.Destination_Station_Code
                  //&& o.jEntrance_Date== train.Entrance_Date
                  //&& o.jEntrance_DateTime== train.Entrance_DateTime
                  //&& o.jEntrance_Time== train.Entrance_Time
                  && o.jF15Rec_ID == train.F15Rec_ID
                  && o.jSource_Station_Code == train.Source_Station_Code
                  && o.jTashkil_Date == train.Tashkil_Date
                  && o.jTashkil_Time == train.Tashkil_Time
                  && o.jTrain_No == train.Train_No
                  
                  //&& o.jUpdate_DateTime == train.Update_DateTime
                  );
                if (entryPWS0Train == null)
                {
                    add = true;
                    entryPWS0Train = new Model.PWS0TrainsBarry();
                }
                try
                {
                    entryPWS0Train.CurrentStationId = stations.fnc_getStationId(train.Current_Station_Code, null, true);
                    entryPWS0Train.DestinationStationId = stations.fnc_getStationId(train.Destination_Station_Code, null, true);
                    entryPWS0Train.FetchTime = DateTime.Now;
                    entryPWS0Train.jCurrent_Station_Code = train.Current_Station_Code;
                    entryPWS0Train.jDestination_Station_Code = train.Destination_Station_Code;
                    entryPWS0Train.jEntrance_Date = train.Entrance_Date;
                    entryPWS0Train.jEntrance_DateTime = train.Entrance_DateTime;
                    entryPWS0Train.jEntrance_Time = train.Entrance_Time;
                    entryPWS0Train.jF15Rec_ID = train.F15Rec_ID;
                    entryPWS0Train.jLocomotiveNumbers = (train.F15Rec_ID.HasValue && getLocomotives ? locos.fnc_getTrainsBarryLocomotives(train.F15Rec_ID.Value) : null);
                    entryPWS0Train.jSource_Station_Code = train.Source_Station_Code;
                    entryPWS0Train.jTashkil_Date = train.Tashkil_Date;
                    entryPWS0Train.jTashkil_Time = train.Tashkil_Time;
                    entryPWS0Train.jTrain_No = train.Train_No;
                    entryPWS0Train.jUpdate_DateTime = train.Update_DateTime;
                    entryPWS0Train.SourceStationId = stations.fnc_getStationId(train.Source_Station_Code, null, true);
                    entryPWS0Train.TashkilDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(train.Tashkil_Date, string.IsNullOrEmpty(train.Tashkil_Time) ? null : (int?)int.Parse(train.Tashkil_Time));
                    entryPWS0Train.CycleNumber = cycleNumber;
                    if (add)
                    {
                        entityLogistic.PWS0TrainsBarry.Add(entryPWS0Train);
                    }


                    entityLogistic.SaveChanges();
                    if (train.F15Rec_ID.HasValue && train.Train_No.HasValue && getBillOfLadings)
                        billOfLadings.readAndSaveToDB(train.F15Rec_ID.Value, train.Train_No.Value, entryPWS0Train.Id, cycleNumber, train);
                }
                catch (Exception ex)
                {
                }
            }

        }

        public static int? fnc_getTrainIdFromDB(int train_no, DateTime? enteranceDateTime)
        {
            using (var entityLogistic = new Model.logisticEntities())
            {
                var entryTrainBarry = entityLogistic.PWS0TrainsBarry.FirstOrDefault(o => o.jTrain_No == train_no
                && o.jEntrance_DateTime == enteranceDateTime);
                if (entryTrainBarry == null) return null;
                return entryTrainBarry.Id;
            }
        }
    }
    public class trainBarryJsonModel
    {
        public int? Train_No;
        public int? F15Rec_ID;
        public string Tashkil_Date;
        public string Tashkil_Time;
        public int? Current_Station_Code;
        public int? Source_Station_Code;
        public int? Destination_Station_Code;
        public string Entrance_Date;
        public DateTime? Entrance_DateTime;
        public string Entrance_Time;
        public DateTime? Update_DateTime;
        public string LocomotiveNumbers;
    }
}
