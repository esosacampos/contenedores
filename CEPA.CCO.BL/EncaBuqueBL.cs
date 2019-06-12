using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;

namespace CEPA.CCO.BL
{
    public class EncaBuqueBL
    {
        public List<EncaBuque> ObtenerBuque(DBComun.Estado pTipo, string c_cliente)
        {
            return EncaBuqueDAL.ObtenerBuques(pTipo, c_cliente);
        }

        public List<EncaBuque> ObtenerBuqueID(DBComun.Estado pTipo, string c_cliente, string c_buque, string c_llegada)
        {
            return EncaBuqueDAL.ObtenerBuquesID(pTipo, c_cliente, c_buque, c_llegada);
        }
    }
}
