using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.BL
{
    public class UsuarioBL
    {
        public List<Usuario> ObtenerUsuariAC(DBComun.Estado pTipo, string c_usuario)
        {
            return UsuarioDAL.ObtenerUsuarioAC(pTipo, c_usuario);
        }
    }
}
