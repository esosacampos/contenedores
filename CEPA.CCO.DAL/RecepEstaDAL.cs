using CEPA.CCO.Entidades;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;

namespace CEPA.CCO.DAL
{
    public class RecepEstaDAL
    {
        public static List<RecepEsta> getResumenDAL(DBComun.Estado pTipo, string c_llegada)
        {
            List<RecepEsta> notiLista = new List<RecepEsta>();
                       

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_consulta_est_patio", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));
            


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    RecepEsta _notificacion = new RecepEsta
                    {
                        c_tamaño = _reader.GetString(0),
                        manifestados = _reader.GetInt32(1),
                        cancelados = _reader.GetInt32(2),
                        total = _reader.GetInt32(3),
                        recibidos = _reader.GetInt32(4),
                        pendientes = _reader.GetInt32(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();              
                return notiLista;
            }

        }
        public static List<RecepEsta> getDetalleDAL(DBComun.Estado pTipo, string c_llegada)
        {
            List<RecepEsta> notiLista = new List<RecepEsta>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_consulta_deta_patio", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));



                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    RecepEsta _notificacion = new RecepEsta
                    {
                        c_naviera = _reader.GetString(0),
                        c_tamaño = _reader.GetString(1),
                        manifestados = _reader.GetInt32(2),
                        cancelados = _reader.GetInt32(3),
                        total = _reader.GetInt32(4),
                        recibidos = _reader.GetInt32(5),
                        pendientes = _reader.GetInt32(6)
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
