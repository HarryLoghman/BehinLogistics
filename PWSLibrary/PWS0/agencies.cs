using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Models;

namespace PWSLibrary.PWS0
{
    public class agencies
    {
        public string v_url = "http://pws0.rai.ir/mapproxy/Http/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=rai:Agency&outputFormat=application%2Fjson";
        public void sb_readAndSaveToDB()
        {
            try
            {
                Uri uri = new Uri(this.v_url);
                agenciesJsonModel agenciesJson = null;
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Proxy = null;
                webRequest.Method = "GET";
                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse != null)
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        string result = rd.ReadToEnd();
                        agenciesJson = Newtonsoft.Json.JsonConvert.DeserializeObject<agenciesJsonModel>(result);
                    }
                    webResponse.Close();

                    if (agenciesJson != null && agenciesJson.features != null)
                    {
                        using (var entityLogestic = new logisticEntities())
                        {
                            foreach (agency_featureJsonModel feature in agenciesJson.features)
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
    }

    public class agenciesJsonModel
    {
        public string type;
        public int totalFeatures;
        public List<agency_featureJsonModel> features;
        public crsJsonModel crs;
    }

    public class agency_featureJsonModel
    {
        public string type;
        public string id;
        public geometry geometry;
        public agency_propertiesJsonModel properties;
        public string geometry_name;

        public void sb_saveToDB()
        {
            using (var entityLogestic = new logisticEntities())
            {
                bool add = false;
                PWS0Agencies entryPWS0Agency = entityLogestic.PWS0Agencies.FirstOrDefault(o => o.AgencyCode == this.properties.AgencyCode
                  && o.AgencyName == this.properties.AgencyName
                  && o.CountryCode == this.properties.CountryCode
                  && o.ProvinceCode == this.properties.ProvinceCode
                  && o.cityCode == this.properties.cityCode);
                if (entryPWS0Agency == null)
                {
                    add = true;
                    entryPWS0Agency = new PWS0Agencies();
                }
                entryPWS0Agency.AgencyCode = this.properties.AgencyCode;
                entryPWS0Agency.AgencyName = this.properties.AgencyName;
                entryPWS0Agency.AgencyStreet = this.properties.AgencyStreet;
                entryPWS0Agency.AgencyTel = this.properties.AgencyTel;
                entryPWS0Agency.cityCode = this.properties.cityCode;
                entryPWS0Agency.cityName = this.properties.cityName;
                entryPWS0Agency.CountryCode = this.properties.CountryCode;
                entryPWS0Agency.CountryName = this.properties.CountryName;
                entryPWS0Agency.Lat = this.properties.Lat;
                entryPWS0Agency.Lng = this.properties.Lng;
                entryPWS0Agency.ProvinceCode = this.properties.ProvinceCode;
                entryPWS0Agency.provinceName = this.properties.provinceName;
                entryPWS0Agency.FetchTime = DateTime.Now;
                if (add)
                {
                    entityLogestic.PWS0Agencies.Add(entryPWS0Agency);
                }

                entityLogestic.SaveChanges();
                //to get id of stationid
                int AgencyId = entryPWS0Agency.Id;

                add = false;
                PWS0AgenciesGeos entryPWS0AgenciesGeos = entityLogestic.PWS0AgenciesGeos.FirstOrDefault(o => o.AgencyId == AgencyId
                   && o.id == this.id);
                if (entryPWS0AgenciesGeos == null)
                {
                    entryPWS0AgenciesGeos = new PWS0AgenciesGeos();
                    add = true;
                }
                entryPWS0AgenciesGeos.id = this.id;
                entryPWS0AgenciesGeos.geometry_name = this.geometry_name;
                if (this.geometry != null)
                {
                    entryPWS0AgenciesGeos.geometryType = this.geometry.type;
                    if (this.geometry.coordinates != null)
                        entryPWS0AgenciesGeos.geometryCoordinates = string.Join(",", this.geometry.coordinates.ToArray());
                }
                entryPWS0AgenciesGeos.AgencyId = AgencyId;
                entryPWS0AgenciesGeos.type = this.type;

                if (add)
                {
                    entityLogestic.PWS0AgenciesGeos.Add(entryPWS0AgenciesGeos);
                }
                entityLogestic.SaveChanges();
            }
        }
    }

    public class agency_propertiesJsonModel
    {
        public string AgencyCode;
        public string AgencyName;
        public string AgencyStreet;
        public string AgencyTel;
        public string CountryCode;
        public string CountryName;
        public string Lat;
        public string Lng;
        public string ProvinceCode;
        public string cityCode;
        public string cityName;
        public string provinceName;
    }
}
