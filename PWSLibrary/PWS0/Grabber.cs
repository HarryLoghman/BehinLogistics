using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Models;

namespace PWSLibrary.PWS0
{
    class Grabber
    {
        //private string v_urlStations = "http://pws0.rai.ir/mapproxy/Http/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=rai:Station&outputFormat=application%2Fjson";
        //private string v_urlAgencies = "http://pws0.rai.ir/mapproxy/Http/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=rai:Agency&outputFormat=application%2Fjson";
        //private string v_urlTrainPassenger = "http://pws0.rai.ir/Home/getTrainTrackList";
        //private string v_urlTrainPassengerUpdate = "http://pws0.rai.ir/Home/getLastUpdateTrainTrackList";
        //private string v_urlTrainBarry = "http://pws0.rai.ir/Bari/getBariTrainTrackList";
        //private string v_urlTrainBarryLocomotives = "http://pws0.rai.ir/Bari/GetLocomotiveList";
        //private string v_urlTrainBarryWagons = "http://pws0.rai.ir/Bari/getBariWgonList";
        //private string v_urlFindBariWagon = "http://pws0.rai.ir/Bari/findBariWagon";

        int v_cyleNumberBillOfLadings = 0;
        int v_cyleNumberTrainsBarry = 0;
        int v_cyleNumberTrainsPassenger = 0;
        public Grabber()
        {
            int? temp;
            using (var entityLogestic = new logisticEntities())
            {
                DateTime dateTime = DateTime.Now.Date;
                temp = entityLogestic.PWS0BillOfLadings.Where(o => DbFunctions.TruncateTime(o.FetchTime) == dateTime).Max(o => o.CycleNumber);
                this.v_cyleNumberBillOfLadings = (temp.HasValue ? temp.Value : 0) + 1;

                temp = entityLogestic.PWS0TrainsBarry.Where(o => DbFunctions.TruncateTime(o.FetchTime) == dateTime).Max(o => o.CycleNumber);
                this.v_cyleNumberTrainsBarry = (temp.HasValue ? temp.Value : 0) + 1;

                temp = entityLogestic.PWS0TrainsPassengers.Where(o => DbFunctions.TruncateTime(o.FetchTime) == dateTime).Max(o => o.CycleNumber);
                this.v_cyleNumberTrainsPassenger = (temp.HasValue ? temp.Value : 0) + 1;

            }
        }
        public void sb_startGrab()
        {
            //stations stations = new stations();
            //stations.sb_readAndSaveToDB();

            //agencies agencies = new agencies();
            //agencies.sb_readAndSaveToDB();

            //trainPassenger trainPs = new trainPassenger();
            //trainPs.sb_readAndSaveToDB(this.v_cyleNumberTrainsPassenger);

            trainBarry trainBs = new trainBarry();
            trainBs.sb_readAndSaveToDB(this.v_cyleNumberTrainsBarry, true, true);



        }


        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            Encoding encoding = Encoding.UTF8;
            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;


                //string postData = string.Format("multipart/form-data;{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                //    boundary,
                //    param.Key,
                //    param.Value);
                string postData = "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"__RequestVerificationToken\"\r\n\r\nGJKWN0U0ndGyC5fAKEdhhosJMhpjLmtfvNIieri7YR8Q37_BUn-lmEg6ucZPrWPefAcxG1-5TPMtYPZsa6fFIigJBBICLcfUlMrufrpIV6U1\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));

            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

    }
}
