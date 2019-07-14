using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWSLibrary.PWS0
{
    public class SharedJsonModel
    {
        
    }

    public class geometry
    {
        public string type;
        public List<float> coordinates;

    }

    public class crsJsonModel
    {
        public string type;
        public crs_propertiesJsonModel properties;

    }

    public class crs_propertiesJsonModel
    {
        public string name;
    }
}
