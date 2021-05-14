using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class Vaciado
    {
        public int IdTipoVa { get; set; }
        public int a_manifiesto { get; set; }
        public int n_manifiesto { get; set; }
        public string n_contenedor { get; set; }
        public string n_contacto { get; set; }
        public string t_contacto { get; set; }

        public string e_contacto { get; set; }

        public string s_archivo_aut { get; set; }

        public int b_shipper { get; set; }

    }

    public class TipoVaciado
    {
        public int IdTipoVac { get; set; }
        public string Descripcion { get; set; }
        
    }
}
