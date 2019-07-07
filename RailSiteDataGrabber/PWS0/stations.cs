using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.PWS0
{
    public class stations
    {
        public string v_url = "http://pws0.rai.ir/mapproxy/Http/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=rai:Station&outputFormat=application%2Fjson";
        public void sb_readAndSaveToDB()
        {
            try
            {
                Uri uri = new Uri(this.v_url);
                stationsJsonModel stationsJson = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "GET";
                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse != null)
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string result = rd.ReadToEnd();
                        stationsJson = Newtonsoft.Json.JsonConvert.DeserializeObject<stationsJsonModel>(result);
                    }
                    webResponse.Close();

                    if (stationsJson != null && stationsJson.features != null)
                    {
                        using (var entityLogistic = new Model.logisticEntities())
                        {
                            foreach (station_featureJsonModel feature in stationsJson.features)
                            {
                                feature.sb_saveToDB();
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
            }
        }

        public static int? fnc_getStationId(int? stationCode, string stationName, bool notifNotExist)
        {
            if (!stationCode.HasValue && string.IsNullOrEmpty(stationName)) return null;

            using (var entityLogistic = new Model.logisticEntities())
            {
                Model.PWS0Stations station;
                if (stationCode.HasValue)
                    station = entityLogistic.PWS0Stations.FirstOrDefault(o => o.stcode == stationCode);
                else
                {
                    station = entityLogistic.PWS0Stations.FirstOrDefault(o => o.name == stationName);
                    if (station == null)
                    {
                        station = entityLogistic.PWS0Stations.FirstOrDefault(o => o.name2 == stationName);
                        if (station == null)
                            station = entityLogistic.PWS0Stations.FirstOrDefault(o => o.alternateNames.Contains(stationName + ";"));
                    }
                }

                if (station == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        if (stationCode.HasValue)
                            strNotif = "stationCode = " + stationCode + " does not exist in PWS0Stations table.";//+ (add ? "? It will be created" : "");
                        else strNotif = "stationName1  or stationName2 = " + stationName + " does not exist in PWS0Stations table.";//+ (add ? "? It will be created" : "");
                    }
                    //if (add)
                    //{
                    //    entryStation = new Model.PWS0Goods();
                    //    entryStation.name = stationName;
                    //    entityLogistic.PWS0Stations.Add(entryStation);
                    //    entityLogistic.SaveChanges();
                    //    return entryStation.Id;
                    //}
                    //else return null;

                    return null;
                }
                else return station.Id;
            }
        }


    }
    public class stationsJsonModel
    {
        public string type;
        public int totalFeatures;
        public List<station_featureJsonModel> features;
        public crsJsonModel crs;
    }

    public class stationsStation_propertiesJsonModel
    {
        public float? lat;
        public float? lon;
        public float? alt;
        public string name;
        public string name2;
        public int? stcode;
        public string bus;
        public string metro;
        public string taxi;
        public string other;
        public string cellOffice;
        public string parkingtim;
        public string sakohamlkh;
        public string daftarforo;
        public string kioskkhari;
        public string khodpardaz;
        public string telephonom;
        public string internetbi;
        public string balabar;
        public string ramp;
        public string postbehdas;
        public string ambolans;
        public string vilcher;
        public string masirnabin;
        public string amanatketa;
        public string tanaqolat;
        public string soqat;
        public string mobile;
        public string sanayedast;
        public string reservhote;
        public string irani;
        public string farangi;
        public string bajetahvil;
        public string salonentez;
        public string salontashr;
        public string namazkhone;
        public string restoran;
        public string vahedetela;
        public string ashia;
        public string sandoq;
        public string saatkaranb;
        public string saatkarbaj;
        public string agency;
        public int? areaid;
        public int? cityid;
        public string cityname;
        public string statename;
        public int? stateid;
        public int? TaghlilStationID;
        public int? taghlilid;

    }
    public class station_featureJsonModel
    {
        public string type;
        public string id;
        public geometry geometry;
        public stationsStation_propertiesJsonModel properties;
        public string geometry_name;


        public void sb_saveToDB()
        {
            using (var entityLogistic = new Model.logisticEntities())
            {
                bool add = false;
                Model.PWS0Stations entryPWS0Station = entityLogistic.PWS0Stations.FirstOrDefault(o => o.stcode == this.properties.stcode
                  && o.name == this.properties.name && o.name2 == this.properties.name2
                  && o.areaid == this.properties.areaid
                  && o.cityid == this.properties.cityid
                  && o.cityname == this.properties.cityname
                  && o.statename == this.properties.statename
                  && o.stateid == this.properties.stateid);
                if (entryPWS0Station == null)
                {
                    add = true;
                    entryPWS0Station = new Model.PWS0Stations();
                }
                entryPWS0Station.lat = this.properties.lat;
                entryPWS0Station.lon = this.properties.lon;
                entryPWS0Station.alt = this.properties.alt;
                entryPWS0Station.name = this.properties.name;
                entryPWS0Station.name2 = this.properties.name2;
                entryPWS0Station.stcode = this.properties.stcode;
                entryPWS0Station.bus = this.properties.bus;
                entryPWS0Station.metro = this.properties.metro;
                entryPWS0Station.taxi = this.properties.taxi;
                entryPWS0Station.other = this.properties.other;
                entryPWS0Station.cellOffice = this.properties.cellOffice;
                entryPWS0Station.parkingtim = this.properties.parkingtim;
                entryPWS0Station.sakohamlkh = this.properties.sakohamlkh;
                entryPWS0Station.daftarforo = this.properties.daftarforo;
                entryPWS0Station.kioskkhari = this.properties.kioskkhari;
                entryPWS0Station.khodpardaz = this.properties.khodpardaz;
                entryPWS0Station.telephonom = this.properties.telephonom;
                entryPWS0Station.internetbi = this.properties.internetbi;
                entryPWS0Station.balabar = this.properties.balabar;
                entryPWS0Station.ramp = this.properties.ramp;
                entryPWS0Station.postbehdas = this.properties.postbehdas;
                entryPWS0Station.ambolans = this.properties.ambolans;
                entryPWS0Station.vilcher = this.properties.vilcher;
                entryPWS0Station.masirnabin = this.properties.masirnabin;
                entryPWS0Station.amanatketa = this.properties.amanatketa;
                entryPWS0Station.tanaqolat = this.properties.tanaqolat;
                entryPWS0Station.soqat = this.properties.soqat;
                entryPWS0Station.mobile = this.properties.mobile;
                entryPWS0Station.sanayedast = this.properties.sanayedast;
                entryPWS0Station.reservhote = this.properties.reservhote;
                entryPWS0Station.irani = this.properties.irani;
                entryPWS0Station.farangi = this.properties.farangi;
                entryPWS0Station.bajetahvil = this.properties.bajetahvil;
                entryPWS0Station.salonentez = this.properties.salonentez;
                entryPWS0Station.salontashr = this.properties.salontashr;
                entryPWS0Station.namazkhone = this.properties.namazkhone;
                entryPWS0Station.restoran = this.properties.restoran;
                entryPWS0Station.vahedetela = this.properties.vahedetela;
                entryPWS0Station.ashia = this.properties.ashia;
                entryPWS0Station.sandoq = this.properties.sandoq;
                entryPWS0Station.saatkaranb = this.properties.saatkaranb;
                entryPWS0Station.saatkarbaj = this.properties.saatkarbaj;
                entryPWS0Station.agency = this.properties.agency;
                entryPWS0Station.areaid = this.properties.areaid;
                entryPWS0Station.cityid = this.properties.cityid;
                entryPWS0Station.cityname = this.properties.cityname;
                entryPWS0Station.statename = this.properties.statename;
                entryPWS0Station.stateid = this.properties.stateid;
                entryPWS0Station.TaghlilStationID = this.properties.TaghlilStationID;
                entryPWS0Station.taghlilid = this.properties.taghlilid;
                entryPWS0Station.FetchTime = DateTime.Now;
                if (add)
                {
                    entityLogistic.PWS0Stations.Add(entryPWS0Station);
                }

                entityLogistic.SaveChanges();
                //to get id of stationid
                int StationId = entryPWS0Station.Id;

                add = false;
                Model.PWS0StationsGeos entryPWS0StationsGeos = entityLogistic.PWS0StationsGeos.FirstOrDefault(o => o.StationId == StationId
                    && o.id == this.id);
                if (entryPWS0StationsGeos == null)
                {
                    entryPWS0StationsGeos = new Model.PWS0StationsGeos();
                    add = true;
                }
                entryPWS0StationsGeos.id = this.id;
                entryPWS0StationsGeos.geometry_name = this.geometry_name;
                if (this.geometry != null)
                {
                    entryPWS0StationsGeos.geometryType = this.geometry.type;
                    if (this.geometry.coordinates != null)
                        entryPWS0StationsGeos.geometryCoordinates = string.Join(",", this.geometry.coordinates.ToArray());
                }
                entryPWS0StationsGeos.StationId = StationId;
                entryPWS0StationsGeos.type = this.type;

                if (add)
                {
                    entityLogistic.PWS0StationsGeos.Add(entryPWS0StationsGeos);
                }
                entityLogistic.SaveChanges();
            }
        }
    }
}
