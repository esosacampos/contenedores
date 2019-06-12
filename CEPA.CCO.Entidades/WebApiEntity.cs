using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    class WebApiEntity
    {
    }

    public class EstadiaConte
    {
        public string Categoria{ get; set; }
        public string Cod_agen { get; set; }
        public string Corto { get; set; }
        public string Agencia { get; set; }
        public string Manifiesto { get; set; }
        public string Contenedor { get; set; }
        public string Tipo { get; set; }
        public string Condicion { get; set; }
        public DateTime Ingreso { get; set; }
        public DateTime Fec_vacio { get; set; }
        public Int64 Estadia { get; set; }
        public string Sitio { get; set; }
        public string Observa { get; set; }
        public string Cliente { get; set; }
        public string Estb3 { get; set; }
    }

    public class Procedure
    {
        public string NBase { get; set; }
        public string Servidor { get; set; }
        public string Procedimiento { get; set; }
        public List<Parametros> Parametros { get; set; }
    }

    public class Parametros
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class Cont_Exp
    {
        public string Contenedor { get; set; }
        public string Notificar { get; set; }
        public DateTime? Ingreso { get; set; }
        public DateTime? Exportacion { get; set; }
        public double Peso { get; set; }
        public Int64 Ndias { get; set; }
        public string Leyenda { get; set; }
    }

}
