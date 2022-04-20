using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class ResultadoValidacion
    {
        public int IdReg { get; set; }
        public int NumFila { get; set; }
        public string Descripcion { get; set; }
        public string Campo { get; set; }
        public int Resultado { get; set; }
        public string NumCelda { get; set; }
    }
    
    public class RetenidoCEPA
    {
        public string contenedor { get; set; }
        public string motivo { get; set; }
    }
}
