using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Models;

namespace PWSLibrary.PWS0
{
    public class areas
    {
        public static int? fnc_getAreaId(string areaName, bool add, bool notifNotExist)
        {
            if (string.IsNullOrEmpty(areaName)) return null;
            //if (areaName == "واگن بدون بار") return -1;
            using (var entityLogestic = new logisticEntities())
            {
                var entryArea = entityLogestic.Areas.FirstOrDefault(o => o.areaName == areaName);
                if (entryArea == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        strNotif = "areaName = " + areaName + " does not exist in Areas table." + (add ? "? It will be created" : "");
                    }
                    if (add)
                    {
                        entryArea = new Area();
                        entryArea.areaName= areaName;
                        entityLogestic.Areas.Add(entryArea);
                        entityLogestic.SaveChanges();
                        return entryArea.Id;
                    }
                    else return null;
                }
                else return entryArea.Id;
            }
        }
    }
}
