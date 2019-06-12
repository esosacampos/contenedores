using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class Usuario
    {
        public int IdReg { get; set; }
        public string c_usuario { get; set; }
        public string d_usuario { get; set; }
        public string c_naviera { get; set; }
        public string c_mail { get; set; }
        public string c_iso_navi { get; set; }
        public string c_navi_corto { get; set; }

        public string d_naviera { get; set; }
        public string Habilitado { get; set; }
        public int b_sidunea { get; set; }

        public int b_ibooking { get; set; }
    }

    public class UsuarioContra
    {
        public int c_marcacion { get; set; }
        public string NombreC { get; set; }
        public string c_operadora { get; set; }
        public string d_operadora { get; set; }
        public int c_cargo { get; set; }
        public string d_cargo { get; set; }
    }

    public class UsuarioNaviera
    {
        public string c_iso_navi { get; set; }
        public string c_navi_corto { get; set; }
        public string c_naviera { get; set; }
    }
}
