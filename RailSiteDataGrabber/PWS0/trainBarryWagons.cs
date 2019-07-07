using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.PWS0
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
                using (var entityLogistic = new Model.logisticEntities())
                {
                    wagonsLst = (from w in entityLogistic.Wagons
                                 where !(from b in entityLogistic.PWS0BillOfLadings
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
                            using (var entityLogistic = new Model.logisticEntities())
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
                        using (var entityLogistic = new Model.logisticEntities())
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
            using (var entityLogistic = new Model.logisticEntities())
            {
                var entryWagon = entityLogistic.Wagons.FirstOrDefault(o => o.wagonControlNo == wagonNo.ToString());
                if (entryWagon == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        strNotif = "WagonNo = " + wagonNo + " does not exist in Wagons table.";// + (add ? "? It will be created" : "");
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
                else return entryWagon.Id;
            }
        }

        public static int? fnc_getWagonTypeName(string wagonTypeName, bool add, bool notifNotExist)
        {
            if (string.IsNullOrEmpty(wagonTypeName)) return null;
            using (var entityLogistic = new Model.logisticEntities())
            {
                var entryWagonType = entityLogistic.WagonsTypes.FirstOrDefault(o => o.typeName == wagonTypeName.ToString()
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
                        entryWagonType = new Model.WagonsType();
                        entryWagonType.typeName = wagonTypeName;
                        entityLogistic.WagonsTypes.Add(entryWagonType);
                        entityLogistic.SaveChanges();
                        return entryWagonType.Id;
                    }
                    else return null;

                }
                else return entryWagonType.Id;
            }
        }

        public static bool fnc_saveWagonFromRWMMS(rwmms.declerationListDataTable dt, int rowIndex)
        {
            if (dt == null) throw new Exception("dt is null");
            if (rowIndex < 0 || rowIndex >= dt.Rows.Count) throw new Exception("rowIndex " + rowIndex + " is outofbound");
            DataRow dr = dt.Rows[rowIndex];
            long wagonNo = long.Parse(dr[rwmms.declerationListDataTable.fld_wagonNo].ToString());
            try
            {
                using (var entityLogistic = new Model.logisticEntities())
                {
                    bool add = false;
                    var entryWagon = entityLogistic.Wagons.FirstOrDefault(o => o.wagonNo == wagonNo);

                    if (entryWagon == null)
                    {
                        add = true;
                        entryWagon = new Model.Wagon();
                    }
                    //entryWagon.capacity ;
                    entryWagon.companyId = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_ownerName]) ? null : rwmms.vehicleOwners.Fnc_getVehicleOwnerId(dr[rwmms.declerationListDataTable.fld_ownerName].ToString());
                    entryWagon.wagonNo = wagonNo;
                    entryWagon.wagonTypeId = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonTypeName]) ? null : trainBarryWagons.fnc_getWagonTypeName(dr[rwmms.declerationListDataTable.fld_wagonTypeName].ToString(), true, true);
                    entryWagon.wrAxisCount = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonAxisCount]) ? null : (int?)int.Parse(dr[rwmms.declerationListDataTable.fld_wagonAxisCount].ToString());
                    entryWagon.wrBoogieType = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonBoogieType]) ? null : dr[rwmms.declerationListDataTable.fld_wagonBoogieType].ToString();
                    entryWagon.wrBrakeType = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonBrakeType]) ? null : dr[rwmms.declerationListDataTable.fld_wagonBrakeType].ToString();
                    entryWagon.wrCapacity = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonCapacity]) ? null : (double?)double.Parse(dr[rwmms.declerationListDataTable.fld_wagonCapacity].ToString());
                    entryWagon.wrChassisSerial = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonChassisSerialNo]) ? null : dr[rwmms.declerationListDataTable.fld_wagonChassisSerialNo].ToString();
                    entryWagon.wrCompanyManufacturer = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonCompanyManufacturer]) ? null : dr[rwmms.declerationListDataTable.fld_wagonCompanyManufacturer].ToString();
                    entryWagon.wrCountry = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonCountry]) ? null : dr[rwmms.declerationListDataTable.fld_wagonCountry].ToString();
                    entryWagon.wrHookType = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonHookType]) ? null : dr[rwmms.declerationListDataTable.fld_wagonHookType].ToString();
                    entryWagon.wrNetWeight = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonNetWeight]) ? null : (double?)double.Parse(dr[rwmms.declerationListDataTable.fld_wagonNetWeight].ToString());
                    entryWagon.wrProductionYear = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonProductionYear]) ? null : (int?)int.Parse(dr[rwmms.declerationListDataTable.fld_wagonProductionYear].ToString());
                    entryWagon.wrRivNo = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonRIVNo]) ? null : dr[rwmms.declerationListDataTable.fld_wagonRIVNo].ToString();
                    entryWagon.wrWagonTypeName = Functions.IsNull(dr[rwmms.declerationListDataTable.fld_wagonTypeName]) ? null : dr[rwmms.declerationListDataTable.fld_wagonTypeName].ToString();

                    if (add)
                    {
                        entityLogistic.Wagons.Add(entryWagon);
                    }
                    else entityLogistic.Entry(entryWagon).State = EntityState.Modified;
                    entityLogistic.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
