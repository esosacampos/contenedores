using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class RecepEsta
    {
        public string d_buque { get; set; }
        public string c_naviera  { get; set; }
        public string c_tamaño { get; set; }
        public int  manifestados { get; set; }
        public int cancelados { get; set; }
        public int total { get; set; }
        public int recibidos { get; set; }
        public int pendientes { get; set; }
    }
}
