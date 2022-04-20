using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;

namespace CEPA.CCO.BL
{
    public class Cont_ExpBL
    {
        public List<Cont_Exp> GetContenedor(string c_contenedor)
        {
            List<Cont_Exp> lista = new List<Cont_Exp>();
            try
            {
                lista = Cont_ExpDAL.GetContenedor(c_contenedor);
                return lista;
            }
            catch (Exception ex)
            {
                return lista;
            }

        }

        public List<Cont_Exp_Rev> FindContenedor(string c_contenedor, ref string msgError)
        {
            List<Cont_Exp_Rev> lista = new List<Cont_Exp_Rev>();
            try
            {
                lista = Cont_ExpDAL.FindContenedor(c_contenedor, ref msgError);
                return lista;
            }
            catch (Exception ex)
            {
                return lista;
            }

        }

        public string Rev_Cont_Exp(int iddeta, string justifica)
        {
            string resultado = "NO";
            try
            {
                resultado = Cont_ExpDAL.Rev_Cont_Exp(iddeta, justifica);
                return resultado;
            }
            catch (Exception ex)
            {
                return resultado;
            }
        }
    }
}
