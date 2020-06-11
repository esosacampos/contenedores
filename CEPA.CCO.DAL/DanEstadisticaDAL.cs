using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.DAL
{
    public class DanEstadisticaDAL
    {
        public static List<DANEstadistica> ObtenerEstadistica(string a_consulta, string c_cliente)
        {
            List<DANEstadistica> notiLista = new List<DANEstadistica>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_DAN_ESTADISTICAS", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@year", a_consulta));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_cliente));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DANEstadistica _notificacion = new DANEstadistica
                    {
                        mes = _reader.GetString(0),
                        nmes = (int)_reader.GetInt32(1),
                        retenidos = (int)_reader.GetInt32(2),
                        pendientes = (int)_reader.GetInt32(3),
                        libmes = (int)_reader.GetInt32(4),
                        ayear = (int)_reader.GetInt32(5),
                        liberados = (int)_reader.GetInt32(7)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }


        public static List<DANEstadistica> ObtenerEstadisticaUCC(string a_consulta)
        {
            List<DANEstadistica> notiLista = new List<DANEstadistica>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_UCC_ESTADISTICAS", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@year", a_consulta));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DANEstadistica _notificacion = new DANEstadistica
                    {
                        mes = _reader.GetString(0),
                        nmes = (int)_reader.GetInt32(1),
                        retenidos = (int)_reader.GetInt32(2),
                        liberados = (int)_reader.GetInt32(3),
                        pendientes = (int)_reader.GetInt32(4),
                        ayear = (int)_reader.GetInt32(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
    }
}
