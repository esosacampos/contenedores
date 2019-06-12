using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class AlertaDAN
    {
        public string c_naviera { get; set; }
        public string d_naviera { get; set; }
        public string n_contenedor { get; set; }
        public string f_liberacion { get; set; }
        public int c_numeral { get; set; }
        public string ClaveP { get; set; }
        public string ClaveQ { get; set; }
        public string c_llegada { get; set; }
        public string c_transporte { get; set; }

        public string f_salida { get; set; }
        public string f_confir_salida { get; set; }
        public string tipo { get; set; }

    }
}
