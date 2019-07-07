using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHelperSample
{

    public enum enum_entryState
    {
        delete,update,insert
    }
    public class PWS0StationsGeos
    {
        public enum_entryState state { get; set; }
        public string TableName = "";
        public static class columns
        {
            public static string type = "type";
        }

        public class_values values;
        public class class_values
        {
            public int id;
        }
        public bool Save()
        {
            return true;    
        }
        
      
    }
}
