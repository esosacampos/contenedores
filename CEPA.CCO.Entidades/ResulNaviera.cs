using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class ResulNaviera
    {
        public int IdDeta { get; set; }
        public string n_BL { get; set; }
        public string n_contenedor { get; set; }
        public string c_tamaño { get; set; }
        //public double v_peso { get; set; }
        public string b_estado { get; set; }
        public string c_naviera { get; set; }
        public string c_llegada { get; set; }
        public DateTime f_llegada { get; set; }
        public string d_buque { get; set; }
        public int num_manif { get; set; }

    }
}
