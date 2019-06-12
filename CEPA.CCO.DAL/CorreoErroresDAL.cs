using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.DAL
{
    public class CorreoErroresDAL
    {
        public static string Insertar(CorreoError _correo)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_CORREO_ERROR", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@c_asunto", _correo.c_asunto));
                _command.Parameters.Add(new SqlParameter("@c_mail", _correo.c_mail));
                
                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
 
       }
        public static List<CorreoError> Consultar(string c_asunto)
        {
            List<CorreoError> _correos = new List<CorreoError>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_CONSULTAR_CORREO_ERROR", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@c_asunto", c_asunto));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorreoError _tmpCorreo = new CorreoError
                    {                        
                        c_asunto = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_mail = _reader.IsDBNull(1) ? "" : _reader.GetString(1),                     
                        b_envio = (int)_reader.GetInt32(2)
                    };

                    _correos.Add(_tmpCorreo);
                }

                _reader.Close();
                _conn.Close();
                return _correos;
            }
        }

        public static string Actualizar(string c_asunto, string c_mail)
        {
            
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_ACT_CORREOS_ERROR", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@c_asunto", c_asunto));
                _command.Parameters.Add(new SqlParameter("@c_mail", c_mail));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

    }
}
