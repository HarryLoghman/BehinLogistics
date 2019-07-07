using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.PWS0
{
    class trainBarryBillOfLadings
    {
        string v_url = "http://pws0.rai.ir/Bari/getBariWgonList";
        public void readAndSaveToDB(int F15Rec_ID, int train_no, int? trainId, int cycleNumber, trainBarryJsonModel trainBarryJson)
        {
            try
            {
                Uri uri;
                uri = new Uri(this.v_url);

                trainBillOfLadingsJsonModel[] trainBillOfLadingsListJson = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "POST";
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.Headers.Add("Cookie", "__RequestVerificationToken=Gm4hoEFS8RkxaSfJxpaeorLztRy6-rfjyKm1pRyPYQP-4dGSPAKrHWG62K3vaJOz_k7xEJBWigJHetij4ID_TmbP-27KPhaINnSun-IMnZc1;");
                webRequest.ContentType = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW";

                //  "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--"

                string postData;
                postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW"
                    + "\r\nContent-Disposition: form-data; name=\"f15rec_id\"\r\n\r\n" + F15Rec_ID.ToString() + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW"
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
                    trainBillOfLadingsListJson = Newtonsoft.Json.JsonConvert.DeserializeObject<trainBillOfLadingsJsonModel[]>(result);
                    if (trainBillOfLadingsListJson != null)
                    {
                        using (var entityLogistic = new Model.logisticEntities())
                        {
                            foreach (trainBillOfLadingsJsonModel billOfLading in trainBillOfLadingsListJson)
                            {
                                if (trainBarryJson != null)
                                {
                                    //billOfLading.Barnameh_NO= it has it already
                                    //billOfLading.Bar_Type = it has it already
                                    billOfLading.Current_Station_Code = trainBarryJson.Current_Station_Code;
                                    //billOfLading.Current_Station_Name=
                                    //billOfLading.Destination_Station_Name = it has it already
                                    billOfLading.Entrance_Date = trainBarryJson.Entrance_Date;
                                    billOfLading.Entrance_Date_M = trainBarryJson.Entrance_DateTime;
                                    billOfLading.Entrance_Time = trainBarryJson.Entrance_Time;
                                    billOfLading.F15Rec_ID = trainBarryJson.F15Rec_ID;
                                    billOfLading.Source_Station_Code = trainBarryJson.Source_Station_Code;
                                    //billOfLading.Source_Station_Name = it has it already
                                    billOfLading.Tashkil_Date = trainBarryJson.Tashkil_Date;
                                    billOfLading.Tashkil_Time = trainBarryJson.Tashkil_Time;
                                    billOfLading.Train_No = trainBarryJson.Train_No;
                                    //billOfLading.Wagon_NO = it has it already
                                }
                                else
                                {
                                    trainBarry train = new trainBarry();
                                    trainBarryJsonModel[] jModel = train.fnc_findTrainBarry(train_no);
                                    if (jModel != null)
                                    {
                                        trainBarryJson = jModel.OrderByDescending(o => o.Entrance_DateTime).FirstOrDefault();
                                        if (trainBarryJson != null)
                                        {
                                            //billOfLading.Barnameh_NO= it has it already
                                            //billOfLading.Bar_Type = it has it already
                                            billOfLading.Current_Station_Code = trainBarryJson.Current_Station_Code;
                                            //billOfLading.Current_Station_Name=
                                            //billOfLading.Destination_Station_Name = it has it already
                                            billOfLading.Entrance_Date = trainBarryJson.Entrance_Date;
                                            billOfLading.Entrance_Date_M = trainBarryJson.Entrance_DateTime;
                                            billOfLading.Entrance_Time = trainBarryJson.Entrance_Time;
                                            billOfLading.F15Rec_ID = trainBarryJson.F15Rec_ID;
                                            billOfLading.Source_Station_Code = trainBarryJson.Source_Station_Code;
                                            //billOfLading.Source_Station_Name = it has it already
                                            billOfLading.Tashkil_Date = trainBarryJson.Tashkil_Date;
                                            billOfLading.Tashkil_Time = trainBarryJson.Tashkil_Time;
                                            billOfLading.Train_No = trainBarryJson.Train_No;
                                            //billOfLading.Wagon_NO = it has it already
                                        }
                                    }
                                }
                                this.sb_saveToDB(billOfLading, train_no, trainId, cycleNumber, "getBariWgonList");
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }

        }

        public void sb_saveToDB(trainBillOfLadingsJsonModel billOfLading, int train_no, int? trainId, int cycleNumber, string fetchUrl)
        {
            bool add = false;
            using (var entityLogistic = new Model.logisticEntities())
            {
                Model.PWS0BillOfLadings entryPWS0BillOfLading = entityLogistic.PWS0BillOfLadings.FirstOrDefault(o =>
                o.jBarnameh_NO == billOfLading.Barnameh_NO
                  && o.jBar_Type == billOfLading.Bar_Type
                  //&& o.jDestination_Station_Name == trainWagon.Destination_Station_Name
                  //&& o.jSource_Station_Name== trainWagon.Source_Station_Name
                  && o.jWagon_NO == billOfLading.Wagon_NO);
                if (entryPWS0BillOfLading == null)
                {
                    add = true;
                    entryPWS0BillOfLading = new Model.PWS0BillOfLadings();
                }
                try
                {
                    entryPWS0BillOfLading.CurrentStationId = stations.fnc_getStationId(billOfLading.Current_Station_Code, billOfLading.Current_Station_Name, true); ;
                    entryPWS0BillOfLading.CycleNumber = cycleNumber;
                    entryPWS0BillOfLading.DestinationStationId = stations.fnc_getStationId(null, billOfLading.Destination_Station_Name, true);
                    entryPWS0BillOfLading.FetchTime = DateTime.Now;
                    entryPWS0BillOfLading.FetchUrl = fetchUrl;
                    entryPWS0BillOfLading.goodsId = goods.fnc_getGoodsId(billOfLading.Bar_Type, false, true);
                    entryPWS0BillOfLading.jBarnameh_NO = billOfLading.Barnameh_NO;
                    entryPWS0BillOfLading.jBar_Type = billOfLading.Bar_Type;
                    entryPWS0BillOfLading.jCurrent_Station_Code = billOfLading.Current_Station_Code;
                    entryPWS0BillOfLading.jCurrent_Station_Name = billOfLading.Current_Station_Name;
                    entryPWS0BillOfLading.jDestination_Station_Name = billOfLading.Destination_Station_Name;
                    entryPWS0BillOfLading.jEntrance_Date = billOfLading.Entrance_Date;
                    entryPWS0BillOfLading.jEntrance_Date_M = billOfLading.Entrance_Date_M;
                    entryPWS0BillOfLading.jEntrance_Time = billOfLading.Entrance_Time;
                    entryPWS0BillOfLading.jF15Rec_ID = billOfLading.F15Rec_ID;
                    entryPWS0BillOfLading.jSource_Station_Code = billOfLading.Source_Station_Code;
                    entryPWS0BillOfLading.jSource_Station_Name = billOfLading.Source_Station_Name;
                    entryPWS0BillOfLading.jTashkil_Date = billOfLading.Tashkil_Date;
                    entryPWS0BillOfLading.jTashkil_Time = billOfLading.Tashkil_Time;
                    entryPWS0BillOfLading.jTrain_No = billOfLading.Train_No;
                    entryPWS0BillOfLading.jWagon_NO = billOfLading.Wagon_NO;

                    entryPWS0BillOfLading.SourceStationId = stations.fnc_getStationId(billOfLading.Source_Station_Code, billOfLading.Source_Station_Name, true);
                    entryPWS0BillOfLading.TashkilDateTime = Functions.fnc_convertSolarDateAndTimeToDateTime(entryPWS0BillOfLading.jTashkil_Date, string.IsNullOrEmpty(billOfLading.Tashkil_Time) ? null : (int?)int.Parse(billOfLading.Tashkil_Time));
                    entryPWS0BillOfLading.TrainId = (trainId.HasValue ? trainId.Value : (billOfLading.Entrance_Date_M.HasValue ? (int?)trainBarry.fnc_getTrainIdFromDB(train_no, billOfLading.Entrance_Date_M.Value) : null));
                    entryPWS0BillOfLading.WagonId = trainBarryWagons.fnc_getWagonIdCheckWithControlNo(billOfLading.Wagon_NO, true);



                    if (add)
                    {
                        entityLogistic.PWS0BillOfLadings.Add(entryPWS0BillOfLading);
                    }

                    entityLogistic.SaveChanges();
                }
                catch (Exception ex)
                {
                }
            }


        }

        public static int? fnc_getBillOfLadingId(long? billOfLadingNo/*,bool add*/, bool notifNotExist)
        {
            if (!billOfLadingNo.HasValue) return null;
            using (var entityLogistic = new Model.logisticEntities())
            {
                var entryBillOfLading = entityLogistic.PWS0BillOfLadings.FirstOrDefault(o => o.jBarnameh_NO == billOfLadingNo.ToString());
                if (entryBillOfLading == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        strNotif = "BillOfLading = " + billOfLadingNo + " does not exist in BillOfLadings table.";// + (add ? "? It will be created" : "");
                    }
                    //if (add)
                    //{
                    //    entryWagon = new Model.Wagon();
                    //    entryWagon.wagonNo = wagonNo;
                    //    entityLogistic.Wagons.Add(entryWagon);
                    //    entityLogistic.SaveChanges();
                    //    return entryWagon.Id;
                    //}
                    //else return null;
                    return null;
                }
                else return entryBillOfLading.Id;
            }
        }
    }

    public class trainBillOfLadingsJsonModel
    {
        public int? Train_No;
        public long? Wagon_NO;
        public string Barnameh_NO;
        public string Source_Station_Name;
        public string Destination_Station_Name;
        public string Bar_Type;
        public int? Current_Station_Code;
        public string Current_Station_Name;
        public int? Source_Station_Code;
        public string Entrance_Date;
        public DateTime? Entrance_Date_M;
        public string Entrance_Time;
        public string Tashkil_Date;
        public string Tashkil_Time;
        public int? F15Rec_ID;
    }

}
