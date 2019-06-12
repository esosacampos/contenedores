using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class DetaBooking
    {
        public int IdDetaBooking {get; set;}
        public int IdBooking { get; set; }
        public string shipment_name { get; set; }
        public string n_contenedor { get; set; }
        public object c_tamaño { get; set; }
        public string n_sello { get; set; }
        public bool b_estado { get; set; }
        public bool b_marca { get; set; }
    }
}
