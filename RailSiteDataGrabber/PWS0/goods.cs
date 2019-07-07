using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.PWS0
{
    class goods
    {
        public static int? fnc_getGoodsId(string goodsName, bool add, bool notifNotExist)
        {
            if (string.IsNullOrEmpty(goodsName)) return null;
            if (goodsName == "واگن بدون بار") return -1;
            using (var entityLogistic = new Model.logisticEntities())
            {
                var entryGoods = entityLogistic.PWS0Goods.FirstOrDefault(o => o.goodsName == goodsName);
                if (entryGoods == null)
                {
                    string strNotif = "";
                    if (notifNotExist)
                    {
                        strNotif = "goodsName = " + goodsName + " does not exist in PWS0Goods table." + (add ? "? It will be created" : "");
                    }
                    if (add)
                    {
                        entryGoods = new Model.PWS0Goods();
                        entryGoods.goodsName = goodsName;
                        entityLogistic.PWS0Goods.Add(entryGoods);
                        entityLogistic.SaveChanges();
                        return entryGoods.Id;
                    }
                    else return null;
                }
                else return entryGoods.Id;
            }
        }
    }
}
