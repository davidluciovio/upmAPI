using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models.DataUPM
{
    public class DataActiveEmployees
    {
        public int CB_CODIGO { get; set; }
        public string CB_NOMBRES { get; set; }
        public string CB_APE_MAT { get; set; }
        public string CB_APE_PAT { get; set; }
        public string PRETTYNAME { get; set; }
        public Byte[] IM_BLOB { get; set; }
    }
}
