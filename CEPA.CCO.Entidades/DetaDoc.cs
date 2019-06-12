using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class DetaDoc
    {
        public int IdDoc { get; set; }
        public int IdReg { get; set; }
        public string c_imo { get; set; }
        public string s_archivo { get; set; }
        public string c_usuario { get; set; }
        public string c_llegada { get; set; }
        public string c_naviera { get; set; }
        public int num_manif { get; set; }
        public int CantArch { get; set; }
        public string c_voyage { get; set; }

        public int b_valid { get; set; }
        public int b_omitir { get; set; }
        public DateTime f_valid { get; set; }
        public DateTime f_omitir { get; set; }
        public string a_manifiesto { get; set; }
        public string a_manif { get; set; }

        public int IdTipoMov { get; set; }
        public int b_siduneawd { get; set; }
    }
}
