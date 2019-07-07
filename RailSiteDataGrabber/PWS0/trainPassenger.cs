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
    public class trainPassenger
    {
        public string v_url = "http://pws0.rai.ir/Home/getTrainTrackList";
        public string v_urlUpdate = "http://pws0.rai.ir/Home/getLastUpdateTrainTrackList";
        public void sb_readAndSaveToDB(int cycleNo)
        {
            DateTime? lastUpdateTime = null;
            using (var entityLogistic = new Model.logisticEntities())
            {
                //get the last updatetime for today    
                var entryTrainPassenger = entityLogistic.PWS0TrainsPassengers.OrderByDescending(o => o.jUpdate_DateTime).FirstOrDefault(o => DbFunctions.TruncateTime(o.jUpdate_DateTime) == DbFunctions.TruncateTime(DateTime.Now));
                if (entryTrainPassenger == null)
                    //we dont get any information for today
                    lastUpdateTime = null;
                else
                {
                    DateTime time = entryTrainPassenger.jUpdate_DateTime.Value;
                    if (time > DateTime.Now.AddMinutes(1))
                    {
                        return;
                    }
                    lastUpdateTime = time.AddMinutes(1);
                }
            }
            try
            {
                Uri uri;
                if (!lastUpdateTime.HasValue)
                    uri = new Uri(this.v_url);
                else
                    uri = new Uri(this.v_urlUpdate);

                trainPassengerJsonModel[] trainPassengersListJson = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("Cookie", "__RequestVerificationToken=Gm4hoEFS8RkxaSfJxpaeorLztRy6-rfjyKm1pRyPYQP-4dGSPAKrHWG62K3vaJOz_k7xEJBWigJHetij4ID_TmbP-27KPhaINnSun-IMnZc1;");
                webRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW";

                //  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--"

                string postData;
                if (!lastUpdateTime.HasValue)
                {
                    postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
                }
                else
                {
                    postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW"
                        + "\r\nContent-Disposition: form-data; name=\"lastupdatetime\"\r\n\r\n" + lastUpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
                }
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
                    trainPassengersListJson = Newtonsoft.Json.JsonConvert.DeserializeObject<trainPassengerJsonModel[]>(result);

                    if (trainPassengersListJson != null)
                    {
                        using (var entityLogistic = new Model.logisticEntities())
                        {
                            foreach (trainPassengerJsonModel train in trainPassengersListJson)
                            {
                                this.sb_saveToDB(train, cycleNo);
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }
        }

        private void sb_saveToDB(trainPassengerJsonModel jmodel, int cycleNumber)
        {
            using (var entityLogistic = new Model.logisticEntities())
            {
                bool add = false;
                Model.PWS0TrainsPassengers entryPWS0TrainPassenger = entityLogistic.PWS0TrainsPassengers.FirstOrDefault(
                    o => o.jTrain_No == jmodel.Train_No
                  && o.jCurrent_Station_Code == jmodel.Current_Station_Code
                  //&& o.jEntrance_Date == this.Entrance_Date
                  //&& o.jEntrance_Time == this.Entrance_Time
                  //&& o.jExit_Date == this.Exit_Date
                  //&& o.jExit_Time == this.Exit_Time
                  && o.jDeparture_Date == jmodel.Departure_Date
                  && o.jDeparture_Time == jmodel.Departure_Time
                  && o.jSource_Station_Code == jmodel.Source_Station_Code
                  && o.jDestination_Station_Code == jmodel.Destination_Station_Code
                  //&& o.jDestination_Program_Time_In == this.Destination_Program_Time_In
                  //&& o.jCurrent_Program_Time_In == this.Current_Program_Time_In
                  //&& o.jDeparture_Program_Time_Out == this.Departure_Program_Time_Out
                  //&& o.jUpdate_DateTime == this.Update_DateTime
                  //&& o.jDelayMinute == this.DelayMinute
                  //&& o.jArivalHours == this.ArivalHours
                  );
                if (entryPWS0TrainPassenger == null)
                {
                    add = true;
                    entryPWS0TrainPassenger = new Model.PWS0TrainsPassengers();
                }
                try
                {
                    entryPWS0TrainPassenger.CurrentProgramTimeIn = string.IsNullOrEmpty(jmodel.Current_Program_Time_In) ? null : Functions.fnc_convertTimeLongToTimespan(int.Parse(jmodel.Current_Program_Time_In));
                    entryPWS0TrainPassenger.CurrentStationId = stations.fnc_getStationId(jmodel.Current_Station_Code, null, true);
                    entryPWS0TrainPassenger.DepartureDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(jmodel.Departure_Date, string.IsNullOrEmpty(jmodel.Departure_Time) ? null : (int?)int.Parse(jmodel.Departure_Time));
                    entryPWS0TrainPassenger.DepartureProgramTimeOut = string.IsNullOrEmpty(jmodel.Departure_Program_Time_Out) ? null : Functions.fnc_convertTimeLongToTimespan(int.Parse(jmodel.Departure_Program_Time_Out));
                    entryPWS0TrainPassenger.DestinationProgramTimeIn = string.IsNullOrEmpty(jmodel.Destination_Program_Time_In) ? null : Functions.fnc_convertTimeLongToTimespan(int.Parse(jmodel.Destination_Program_Time_In));
                    entryPWS0TrainPassenger.DestinationStationId = stations.fnc_getStationId(jmodel.Destination_Station_Code, null, true);
                    entryPWS0TrainPassenger.EntranceDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(jmodel.Departure_Date, string.IsNullOrEmpty(jmodel.Entrance_Date) ? null : (int?)int.Parse(jmodel.Entrance_Time));
                    entryPWS0TrainPassenger.ExitDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(jmodel.Exit_Date, string.IsNullOrEmpty(jmodel.Exit_Time) ? null : (int?)int.Parse(jmodel.Exit_Time));
                    entryPWS0TrainPassenger.jArivalHours = jmodel.ArivalHours;
                    entryPWS0TrainPassenger.jCurrent_Program_Time_In = jmodel.Current_Program_Time_In;
                    entryPWS0TrainPassenger.jCurrent_Station_Code = jmodel.Current_Station_Code;
                    entryPWS0TrainPassenger.jDelayMinute = jmodel.DelayMinute;
                    entryPWS0TrainPassenger.jDeparture_Date = jmodel.Departure_Date;
                    entryPWS0TrainPassenger.jDeparture_Program_Time_Out = jmodel.Departure_Program_Time_Out;
                    entryPWS0TrainPassenger.jDeparture_Time = jmodel.Departure_Time;
                    entryPWS0TrainPassenger.jDestination_Program_Time_In = jmodel.Destination_Program_Time_In;
                    entryPWS0TrainPassenger.jDestination_Station_Code = jmodel.Destination_Station_Code;
                    entryPWS0TrainPassenger.jEntrance_Date = jmodel.Entrance_Date;
                    entryPWS0TrainPassenger.jEntrance_Time = jmodel.Entrance_Time;
                    entryPWS0TrainPassenger.jExit_Date = jmodel.Exit_Date;
                    entryPWS0TrainPassenger.jExit_Time = jmodel.Exit_Time;
                    entryPWS0TrainPassenger.jSource_Station_Code = jmodel.Source_Station_Code;
                    entryPWS0TrainPassenger.jTrain_No = jmodel.Train_No;
                    entryPWS0TrainPassenger.jUpdate_DateTime = jmodel.Update_DateTime;
                    entryPWS0TrainPassenger.SourceStationId = stations.fnc_getStationId(jmodel.Source_Station_Code, null, true);
                    entryPWS0TrainPassenger.FetchTime = DateTime.Now;
                    entryPWS0TrainPassenger.CycleNumber = cycleNumber;

                    if (add)
                    {
                        entityLogistic.PWS0TrainsPassengers.Add(entryPWS0TrainPassenger);
                    }

                    entityLogistic.SaveChanges();
                }
                catch (Exception ex)
                {
                }

            }
        }
    }
    public class trainPassengerJsonModel
    {
        public int? Train_No;
        public int? Current_Station_Code;
        public string Entrance_Date;
        public string Entrance_Time;
        public string Exit_Date;
        public string Exit_Time;
        public string Departure_Date;
        public string Departure_Time;
        public int? Source_Station_Code;
        public int? Destination_Station_Code;
        public string Destination_Program_Time_In;
        public string Current_Program_Time_In;
        public string Departure_Program_Time_Out;
        public DateTime Update_DateTime;
        public int? DelayMinute;
        public int? ArivalHours;
    }
}
