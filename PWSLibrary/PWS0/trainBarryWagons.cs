using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PWSLibrary.PWS0
{
    public class trainBarryWagons
    {
        string v_urlFindWagon = "http://pws0.rai.ir/Bari/findBariWagon";
        string v_urlFindBillOfLading = "http://pws0.rai.ir/Bari/findBarnameh";
        public void readAndSaveWagonToDB(long? wagonNo, int cycleNumber)
        {
            List<long?> wagonsLst = new List<long?>();
            if (!wagonNo.HasValue)
            {
                
                using (var entityLogestic = new logisticEntities())
                {
                    wagonsLst = (from w in entityLogestic.Wagons
                                 where !(from b in entityLogestic.PWS0BillOfLadings
                                         where b.CycleNumber == cycleNumber &&
                                         DbFunctions.TruncateTime(b.FetchTime) == DbFunctions.TruncateTime(DateTime.Now)
                                         select b.jWagon_NO
                                         ).Contains(w.wagonNo)
                                 select wagonNo).ToList();

                }
            }
            else
            {
                wagonsLst.Add(wagonNo.Value);
            }

            for (int i = 0; i <= wagonsLst.Count - 1; i++)
            {
                if (!wagonNo.HasValue)
                {
                    continue;
                }
                try
                {
                    Uri uri;
                    uri = new Uri(this.v_urlFindWagon);

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
                        + "\r\nContent-Disposition: form-data; name=\"wagon_no\"\r\n\r\n" + wagonNo.ToString() + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
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
                            using (var entityLogestic = new logisticEntities())
                            {
                                foreach (trainBillOfLadingsJsonModel billOfLading in trainBillOfLadingsListJson)
                                {

                                    if (billOfLading.Train_No.HasValue)
                                    {
                                        PWS0.trainBarryBillOfLadings bill = new trainBarryBillOfLadings();
                                        bill.sb_saveToDB(billOfLading, billOfLading.Train_No.Value, null, cycleNumber, "findBarnameh");
                                    }
                                }
                            }
                        }


                    }
                }
                catch (Exception ex)
                {

                }
            }


        }

        public void readAndSaveBarnamehToDB(long barnameh_no, int cycleNumber)
        {

            try
            {
                Uri uri;
                uri = new Uri(this.v_urlFindBillOfLading);

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
                    + "\r\nContent-Disposition: form-data; name=\"barnameh_no\"\r\n\r\n" + barnameh_no.ToString() + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
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
                        using (var entityLogestic = new logisticEntities())
                        {
                            foreach (trainBillOfLadingsJsonModel billOfLading in trainBillOfLadingsListJson)
                            {

                                if (billOfLading.Train_No.HasValue)
                                {
                                    PWS0.trainBarryBillOfLadings bill = new trainBarryBillOfLadings();
                                    bill.sb_saveToDB(billOfLading, billOfLading.Train_No.Value, null, cycleNumber, "findBariWagon");
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

            }



        }
        public static int? fnc_getWagonIdCheckWithControlNo(long? wagonNo/*,bool add*/, bool notifNotExist)
        {
            if (!wagonNo.HasValue) return null;
            using (var entityLogestic = new logisticEntities())
            {
                var entryWagon = entityLogestic.Wagons.FirstOrDefault(o => o.wagonControlNo == wagonNo.ToString());
                if (entryWagon == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        strNotif = "WagonNo = " + wagonNo + " does not exist in Wagons table.";// + (add ? "? It will be created" : "");
                    }
                    //if (add)
                    //{
                    //    entryWagon = new Wagon();
                    //    entryWagon.wagonNo = wagonNo;
                    //    entityLogestic.Wagons.Add(entryWagon);
                    //    entityLogestic.SaveChanges();
                    //    return entryWagon.Id;
                    //}
                    //else return null;
                    return null;
                }
                else return entryWagon.Id;
            }
        }

        public static int? fnc_getWagonTypeName(string wagonTypeName, bool add, bool notifNotExist)
        {
            if (string.IsNullOrEmpty(wagonTypeName)) return null;
            using (var entityLogestic = new logisticEntities())
            {
                var entryWagonType = entityLogestic.WagonsTypes.FirstOrDefault(o => o.typeName == wagonTypeName.ToString()
               || o.alternateNames.Contains(wagonTypeName + ";"));
                if (entryWagonType == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        strNotif = "WagonTypeName = " + wagonTypeName + " does not exist in WagonsType table." + (add ? "? It will be created" : "");
                    }
                    if (add)
                    {
                        entryWagonType = new WagonsType();
                        entryWagonType.typeName = wagonTypeName;
                        entityLogestic.WagonsTypes.Add(entryWagonType);
                        entityLogestic.SaveChanges();
                        return entryWagonType.Id;
                    }
                    else return null;

                }
                else return entryWagonType.Id;
            }
        }
        
    }
}
