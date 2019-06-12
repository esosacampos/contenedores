using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEPA.CCO.Entidades
{
    public class CorreoError
    {
        public string c_asunto { get; set; }
        public string c_mail { get; set; }
        public int b_envio { get; set; }
        public DateTime f_registro { get; set; }
    }
}
