using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace CEPA.CCO.DAL
{
    public class AlertaDANDAL
    {

        public static List<AlertaDAN> ObtenerAlerta(DBComun.Estado pTipo, string c_tipo)
        {
            List<AlertaDAN> notiLista = new List<AlertaDAN>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_ALERTA_PRIN_DAN_UCC", _conn as SqlConnection);
                _command.Parameters.Add(new SqlParameter("@Tipo", c_tipo));
                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    AlertaDAN _notificacion = new AlertaDAN
                    {
                        c_numeral = _reader.GetInt32(0),
                        c_naviera = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_liberacion = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        ClaveP = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        ClaveQ = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_llegada = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_transporte = ObtenerTransporte(_reader.GetString(6), _reader.GetString(2), _reader.GetString(1), pTipo),
                        f_salida = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        f_confir_salida = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        tipo = _reader.IsDBNull(9) ? "" : _reader.GetString(9)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<AlertaDAN> ObtenerAlerta(DBComun.Estado pTipo, string pFecha, string c_tipo)
        {
            List<AlertaDAN> notiLista = new List<AlertaDAN>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_ALERTA_PRIN_DAN_UCC_FEC", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@fecha", pFecha));
                _command.Parameters.Add(new SqlParameter("@Tipo", c_tipo));

                SqlDataReader _reader = _command.ExecuteReader();


                while (_reader.Read())
                {
                    AlertaDAN _notificacion = new AlertaDAN
                    {
                        c_numeral = _reader.GetInt32(0),
                        c_naviera = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_liberacion = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        ClaveP = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        ClaveQ = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_llegada = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_transporte = ObtenerTransporte(_reader.GetString(6), _reader.GetString(2), _reader.GetString(1), pTipo),
                        f_salida = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        f_confir_salida = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        tipo = _reader.IsDBNull(9) ? "" : _reader.GetString(9)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ObtenerTransporte(string c_llegada, string n_contenedor, string c_naviera, DBComun.Estado pTipo)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlLink, pTipo))
            {
                _conn.Open();
                string consulta = "select return_value transporte_prv from openquery(CONTENEDORES, 'EXEC Sqlvprovitransp(\"{0}\", \"{1}\", \"{2}\")')";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada, n_contenedor, c_naviera), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static List<DetaNaviera> ObtenerOIRSAO(DBComun.Estado pTipo, string c_llegada, int pList)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OIRSA", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));
                _command.Parameters.Add(new SqlParameter("@pList", pList));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        c_correlativo = _reader.GetInt32(0),
                        c_cliente = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        c_tamaño = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        c_tamañoc = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_pais_origen = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_pais_destino = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_tratamiento = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_llegada = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        c_navi = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        c_voyage = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        d_20 = _reader.IsDBNull(11) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(11)),
                        d_4045 = _reader.IsDBNull(12) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(12)),
                        s_20 = _reader.IsDBNull(13) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(13)),
                        s_4045 = _reader.IsDBNull(14) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(14)),
                        s_consignatario = _reader.IsDBNull(15) ? "" : _reader.GetString(15)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }


        public static List<DetaNaviera> getListESC(DBComun.Estado pTipo, string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_DAN_DGA", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        c_correlativo = _reader.GetInt32(0),
                        c_navi = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        b_estado = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        c_tamaño = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        b_retenido = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        b_aduanas = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_cliente = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_llegada = _reader.IsDBNull(8) ? "" : _reader.GetString(8)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }


        public static List<DetaNaviera> getNavierasOpe(DBComun.Estado pTipo, string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_NALIST_OPERACIONES", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        c_navi = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> getPrefiNavi(DBComun.Estado pTipo)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_NALIST_PREFI", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };



                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_naviera = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        d_naviera_p = _reader.IsDBNull(1) ? "" : _reader.GetString(1)
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
