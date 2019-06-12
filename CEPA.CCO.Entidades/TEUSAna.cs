using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class TEUSAna
    {
        public string d_linea { get; set; }
        public int teu1 { get; set; }
        public double t { get; set; }
        public int teu2 { get; set; }
        public double t2 { get; set; }
        public string c_agencia { get; set; }
    }

    public class TEUSResu
    {
        public string d_linea { get; set; }
        public int vi_luni { get; set; }
        public int vi_lteus { get; set; }
        public int vi_vuni { get; set; }
        public int vi_vteus { get; set; }
        public int t_import { get; set; }
        public int ve_luni { get; set; }
        public int ve_lteus { get; set; }
        public int ve_vuni { get; set; }
        public int ve_vteus { get; set; }
        public int t_export { get; set; }
        public int t_uni { get; set; }
        public int t_teus { get; set; }
        public string s_nombre_buque { get; set; }
        public DateTime f_desatraque { get; set; }

    }
}
