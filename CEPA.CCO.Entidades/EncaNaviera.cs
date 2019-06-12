using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace CEPA.CCO.Entidades
{
    public class EncaNaviera
    {
        public int num_manif { get; set; }
        public string a_manifiesto { get; set; }
        public int CantArch { get; set; }
        public int IdReg { get; set; }
        public string c_imo { get; set; }
        public string c_llegada { get; set; }
        public string c_naviera { get; set; }      
        public string c_usuario { get; set; }
        public string c_voyage { get; set; }
        public DateTime f_llegada { get; set; }
        public string d_buque { get; set; }
        public int b_noti { get; set; }

        public int Total_Imp { get; set; }
        public int Total_Trans { get; set; }
        public int Total_TransA { get; set; }
        public int Total_PTrans { get; set; }
        public int Total_PTransA { get; set; }
        public int IdDoc { get; set; }
        public string d_naviera_p { get; set; }
        public int Total_Cancel { get; set; }

        public int b_sidunea { get; set; }
        public int b_DP { get; set; }
    }
}
