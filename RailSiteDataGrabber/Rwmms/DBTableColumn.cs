using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class DBTableColumn : DataColumn
    {
        public DBTableColumn(string columnName, Type dataType, webpageControl control) : base(columnName, dataType)
        {
            this.prp_pageControl = control;
        }

        public webpageControl prp_pageControl { get; set; }

        public object fnc_getValue(webpage wbpg)
        {
            wbpg.prp_controls.IndexOf(this.prp_pageControl);
            
            object value = Convert.ChangeType(this.prp_pageControl.prp_value, this.DataType);
            return value;
        }
    }
}
