using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class EncaBooking
    {
        public int IdBooking { get; set; }
        public string c_naviera { get; set; }
        public DateTime f_registro { get; set; }
        public string s_archivo { get; set; }
        public bool b_estado { get; set; }
    }
}
