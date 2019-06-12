using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class ArchivoBooking
    {
        public int num_fila { get; set; }
        public string shipment { get; set; }
        public string n_contenedor { get; set; }
        public object c_tamaño { get; set; }
        public string n_sello { get; set; }
    }
}
