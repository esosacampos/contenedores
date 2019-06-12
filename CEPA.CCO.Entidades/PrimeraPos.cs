using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class PrimeraPos
    {
        public int IdCode { get; set; }
        public string IdValue { get; set; }
        public string Descripcion { get; set; }
    }

    public class SegundaPos
    {
        public int IdConWi { get; set; }
        public string IdValue { get; set; }
        public string Descripcion { get; set; }
    }

    public class TerceraPos
    {
        public int IdConE { get; set; }
        public string IdCode { get; set; }
        public string Descripcion { get; set; }
    }

    public class EstadoOrden
    {
        public int IdOrden { get; set; }
        public string Valor { get; set; }
        public int Orden { get; set; }
    }

    public class Transporte
    {
        public int IdTransporte { get; set; }
        public string d_descripcion { get; set; }
        public int c_value { get; set; }
    }
}
