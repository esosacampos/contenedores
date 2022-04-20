using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
        //public class Cont_Exp
        //{
        //    public string Contenedor { get; set; }
        //    public string Notificar { get; set; }
        //    public DateTime? Ingreso { get; set; }
        //    public DateTime? Exportacion { get; set; }
        //    public double Peso { get; set; }
        //    public Int64 Ndias { get; set; }
        //    public string Leyenda { get; set; }

        //}

        public class Cont_Exp_Rev
        {
            public string Contenedor { get; set; }
            public string us_autorizado { get; set; }
            public string us_cancelado { get; set; }
            public DateTime? f_autorizado { get; set; }
            public DateTime? f_cancelado { get; set; }
            public bool b_autorizado { get; set; }
            public bool b_cancelado { get; set; }
            public int IdReg { get; set; }
            public int IdDeta { get; set; }
        }
}
