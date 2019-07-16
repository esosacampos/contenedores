using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;
using CEPA.CCO.Entidades;
using System.Data.SqlClient;
using Sybase.Data.AseClient;

namespace CEPA.CCO.DAL
{
    public class PagoDAL
    {
        public static List<Pago> ConsultarPago(string c_tarja, string n_contenedor)
        {
            List<Pago> pLista = new List<Pago>();
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                AseCommand _command = new AseCommand("sp_fport_muellaje_conte", _conn as AseConnection);
                _command.CommandType = CommandType.StoredProcedure; 

                
                AseParameter p_tarja = _command.Parameters.Add("@as_c_tarja", AseDbType.VarChar);
                p_tarja.Value = c_tarja;

                AseParameter p_contenedor = _command.Parameters.Add("@c_contenedor", AseDbType.VarChar);
                p_contenedor.Value = n_contenedor;
                
               
                // _command.Parameters.Add(new SqlParameter("@c_correlativo", _Encanaviera.c_correlativo));

                AseDataReader _reader = _command.ExecuteReader();               

               while (_reader.Read())
               {
                   Pago _pago = new Pago
                   {
                       n_contenedor = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                       ValTransfer = _reader.IsDBNull(2) ? "" : (_reader.GetString(2) == "1" ? "Si" : "No"),
                       ValDespacho = _reader.IsDBNull(3) ? "" : (_reader.GetString(3) == "1" ? "Si" : "No"),
                       ValManejo = _reader.IsDBNull(4) ? "" : (_reader.GetString(4) == "1" ? "Si" : "No"),
                       ValAlmacenaje = _reader.IsDBNull(5) ? "" : (_reader.GetString(5) == "1" ? "Si" : "No"),
                       validacion = _reader.IsDBNull(6) ? "" : (_reader.GetString(6) == "1" ? "Si" : "No")

                   };

                   pLista.Add(_pago);
               }
               _reader.Close();
               _conn.Close();
               return pLista;
            }
        }

        public static List<Pago> ConsultaDAN(string c_llegada, string n_contenedor, DBComun.TipoBD pBD)
        {
            List<Pago> pLista = new List<Pago>();
            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string _sql = @"SELECT CASE WHEN b_detenido = 1 THEN CONVERT(CHAR(10), f_reg_dan, 103) + ' ' + CONVERT(CHAR(10), f_reg_dan, 108) + ' #Oficio: ' + ISNULL(n_folio, '') ELSE 'LIBRE' END b_detenido, 'DAN' tipo
                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg WHERE n_contenedor = '{0}' 
                                AND c_llegada = '{1}'
                                UNION ALL
                                SELECT CASE WHEN b_ucc = 1 THEN CONVERT(CHAR(10), f_retencion_ucc, 103) + ' ' + CONVERT(CHAR(10), f_retencion_ucc, 108) + ' #Oficio: ' + ISNULL(n_ofiucc, '') ELSE 'LIBRE' END b_detenido, 'UCC' tipo
                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg WHERE n_contenedor = '{2}' 
                                AND c_llegada = '{3}'";

                SqlCommand command = new SqlCommand(string.Format(_sql, n_contenedor, c_llegada, n_contenedor, c_llegada), _conn as SqlConnection);



                command.CommandType = CommandType.Text;
                SqlDataReader _reader = command.ExecuteReader();

                while (_reader.Read())
                {

                    Pago _pago = new Pago
                    {
                        b_dan = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        tipoReten = _reader.IsDBNull(1) ? "" : _reader.GetString(1)
                    };
                    pLista.Add(_pago);
                }
                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

       
    }
}
