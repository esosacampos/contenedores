using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class DANEstadistica
    {
        public string mes { get; set; }
        public int nmes { get; set; }
        public int retenidos { get; set; }
        public int liberados { get; set; }
        public int pendientes { get; set; }
        public int ayear { get; set; }
        public int libmes { get; set; }
    }
}
