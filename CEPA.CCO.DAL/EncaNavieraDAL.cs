using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.DAL
{
    public class EncaNavieraDAL
    {
        public static string AlmacenarEncaNaviera(EncaNaviera _Encanaviera)
        {
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
                {
                    _conn.Open();
                    SqlCommand _command = new SqlCommand("pa_enca_naviera_save", _conn as SqlConnection);
                    _command.CommandType = CommandType.StoredProcedure;

                    _command.Parameters.Add(new SqlParameter("@c_imo", _Encanaviera.c_imo));
                    _command.Parameters.Add(new SqlParameter("@c_llegada", _Encanaviera.c_llegada));
                    _command.Parameters.Add(new SqlParameter("@c_naviera", _Encanaviera.c_naviera));
                    _command.Parameters.Add(new SqlParameter("@c_usuario", _Encanaviera.c_usuario));
                    _command.Parameters.Add(new SqlParameter("@c_voyage", _Encanaviera.c_voyage));
                    _command.Parameters.Add(new SqlParameter("@f_llegada", _Encanaviera.f_llegada));
                    _command.Parameters.Add(new SqlParameter("@c_DP", _Encanaviera.b_DP));
                    //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));



                    string resultado = _command.ExecuteScalar().ToString();
                    _conn.Close();
                    return resultado;

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public static string AlmacenarEncaNavieraEx(EncaNaviera _Encanaviera)
        {
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
                {
                    _conn.Open();
                    SqlCommand _command = new SqlCommand("pa_enca_exp_navi_save", _conn as SqlConnection);
                    _command.CommandType = CommandType.StoredProcedure;

                    _command.Parameters.Add(new SqlParameter("@c_imo", _Encanaviera.c_imo));
                    _command.Parameters.Add(new SqlParameter("@c_llegada", _Encanaviera.c_llegada));
                    _command.Parameters.Add(new SqlParameter("@c_naviera", _Encanaviera.c_naviera));
                    _command.Parameters.Add(new SqlParameter("@c_usuario", _Encanaviera.c_usuario));
                    _command.Parameters.Add(new SqlParameter("@c_voyage", _Encanaviera.c_voyage));
                    _command.Parameters.Add(new SqlParameter("@f_llegada", _Encanaviera.f_llegada));
                    //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                    string resultado = _command.ExecuteScalar().ToString();
                    _conn.Close();
                    return resultado;

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public static string ObtenerIdReg(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"DECLARE @CANTIDAD INT
										SET @CANTIDAD = (SELECT COUNT(IdReg) FROM CCO_ENCA_NAVIERAS WHERE c_imo = '{0}' AND c_llegada = '{1}' AND c_naviera = '{2}')
										IF @CANTIDAD = 0
											SELECT 0
										ELSE
											SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_imo = '{0}' AND c_llegada = '{1}' AND c_naviera = '{2}' ";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ObtenerIdRegEx(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"DECLARE @CANTIDAD INT
										SET @CANTIDAD = (SELECT COUNT(IdReg) FROM CCO_ENCA_EXPO_NAVI WHERE c_imo = '{0}' AND c_llegada = '{1}' AND c_naviera = '{2}')
										IF @CANTIDAD = 0
											SELECT 0
										ELSE
											SELECT IdReg FROM CCO_ENCA_EXPO_NAVI WHERE c_imo = '{0}' AND c_llegada = '{1}' AND c_naviera = '{2}' ";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<EncaNaviera> ObtenerDetalle()
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT c_imo, c_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS WHERE ISNULL(b_noti, 0) = 0 GROUP BY c_imo, c_llegada, c_naviera, n_manifiesto ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_imo = _reader.GetString(0),
                        c_llegada = _reader.GetString(1),
                        c_naviera = _reader.GetString(2),
                        num_manif = (int)_reader.GetInt32(3)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerNoEnviados(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT a.IdReg, a.c_llegada, a.c_imo, f_llegada, a.c_naviera, b.n_manifiesto
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdReg = b.IdReg
                                    WHERE b_noti = 0 and (CONVERT(CHAR(10),GETDATE(), 103) >= CONVERT(CHAR(10), dateadd(hh, -48, f_llegada), 103) AND 
                                    CONVERT(CHAR(10),GETDATE(), 103) <= CONVERT(CHAR(10), dateadd(hh, -4, f_llegada), 103)) ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        IdReg = (int)_reader.GetInt32(0),
                        c_llegada = _reader.GetString(1),
                        c_imo = _reader.GetString(2),
                        f_llegada = (DateTime)_reader.GetDateTime(3),
                        c_naviera = _reader.GetString(4),
                        num_manif = (int)_reader.GetInt32(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActualizarNoti(DBComun.Estado pEstado, int pId, int pValor)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_ENCA_NAVIERAS 
									SET b_noti = {0} 
									WHERE IdReg = {1}; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pValor, pId), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static List<EncaNaviera> ObtenerCabecera(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0)
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 0 AND b_cancelado = 0)
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto"; and a.idReg = 663 --*/

                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0), b.IdDoc, ISNULL(b.a_manifiesto, '')
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS c ON b.IdDoc = c.IdDoc
                                    WHERE b_noti = 1 AND b_sustitucion = 0 AND a.IdReg != 18 AND b.b_estado = 1  --AND b.IdDoc NOT IN (SELECT IsNull(IdDoc, 0) FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 OR b_cancelado = 1 OR b_detenido = 1 GROUP BY IsNull(IdDoc, 0))
                                    AND b_autorizado = 1 AND b_cancelado = 0 AND YEAR(c.f_registro) >= 2016 --AND f_recepcion IS NULL
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto, b.IdDoc, b.a_manifiesto";*/

                string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, a.c_naviera, ISNULL(b.n_manifiesto, 0), b.IdDoc, ISNULL(b.a_manifiesto, ''), CASE WHEN b.b_siduneawd = 1 THEN 1 ELSE 0 END b_siduneawd
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b_sustitucion = 0 AND a.IdReg != 18 AND b.b_estado = 1 and b.b_valid != 1 AND b.IdDoc NOT IN (SELECT IsNull(IdDoc, 0) FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 OR b_cancelado = 1 OR b_detenido = 1 GROUP BY IsNull(IdDoc, 0))
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, a.c_naviera, n_manifiesto, b.IdDoc, b.a_manifiesto, b.b_siduneawd";

                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0), b.IdDoc, ISNULL(b.a_manifiesto, '')
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b_sustitucion = 0 AND a.IdReg != 18 AND b.b_estado = 1 and b.b_valid != 1 AND b.IdDoc NOT IN (SELECT IsNull(IdDoc, 0) FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 OR b_cancelado = 1 OR b_detenido = 1 GROUP BY IsNull(IdDoc, 0))
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto, b.IdDoc, b.a_manifiesto";*/

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        f_llegada = (DateTime)_reader.GetDateTime(4),
                        c_naviera = _reader.GetString(5),
                        num_manif = (int)_reader.GetInt32(6),
                        IdDoc = (int)_reader.GetInt32(7),
                        a_manifiesto = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        b_sidunea = _reader.IsDBNull(9) ? 0 :(int)_reader.GetInt32(9)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraEx(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0)
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 0 AND b_cancelado = 0)
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto"; and a.idReg = 663 --*/

                string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, a.c_voyage
                                    FROM CCO_ENCA_EXPO_NAVI a INNER JOIN CCO_DETA_DOC_EXP_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b_sustitucion = 0 AND a.IdReg != 18 AND b.b_estado = 1 and b.b_valid != 1 AND b.IdDoc NOT IN (SELECT IsNull(IdDoc, 0) FROM CCO_DETA_EXPO_NAVI WHERE b_autorizado = 1 OR b_cancelado = 1 OR b_detenido = 1 GROUP BY IsNull(IdDoc, 0))
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, a.c_voyage";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        f_llegada = (DateTime)_reader.GetDateTime(4),
                        c_naviera = _reader.GetString(5),
                        c_voyage = _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCancelados(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0)
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 0 AND b_cancelado = 0)
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto"; and a.idReg = 663 --*/

                string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, b.c_llegada, a.c_imo, a.c_naviera, c_voyage, c_prefijo
                                    FROM CCO_DETA_DOC_NAVI b INNER JOIN CCO_ENCA_NAVIERAS a ON b.IdReg = a.IdReg 
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON d.c_naviera = a.c_naviera 
                                    WHERE b.IdReg IN(SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND f_rpatio IS NULL) AND b.a_manifiesto >= year(getdate())
                                    AND b.b_estado = 1 AND f_valid IS NOT NULL 
                                    GROUP BY a.IdReg, b.c_llegada, a.c_imo, c_voyage, a.c_naviera, c_voyage, c_prefijo
                                    ORDER BY A.IdReg DESC";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),                        
                        c_naviera = _reader.GetString(4),
                        c_voyage = _reader.GetString(5),
                        d_naviera_p = _reader.GetString(6) 
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCambiosRD(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0)
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 0 AND b_cancelado = 0)
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto"; and a.idReg = 663 --*/

                string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, b.c_llegada, a.c_imo, a.c_naviera, c_voyage, c_prefijo
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS c ON b.IdDoc = c.IdDoc
									INNER JOIN CCO_USUARIOS_NAVIERAS d ON a.c_naviera = d.c_naviera
                                    WHERE b_noti = 1 AND b_sustitucion = 0 AND a.IdReg != 18 AND b.b_estado = 1  --AND b.IdDoc NOT IN (SELECT IsNull(IdDoc, 0) FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 OR b_cancelado = 1 OR b_detenido = 1 GROUP BY IsNull(IdDoc, 0))
                                    AND b_autorizado = 1 AND b_cancelado = 0 AND YEAR(c.f_registro) >= 2016 AND f_recepcion IS NULL
                                    GROUP BY  a.IdReg, b.c_llegada, a.c_imo, a.c_naviera, c_voyage, c_prefijo";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        c_naviera = _reader.GetString(4),
                        c_voyage = _reader.GetString(5),
                        d_naviera_p = _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ObtenerNaviera(DBComun.Estado pEstado, int pId)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT c_naviera
									FROM CCO_ENCA_NAVIERAS WHERE IdReg = {0}";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ObtenerNoti(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"DECLARE @CANTIDAD INT
                                    SET @CANTIDAD = (SELECT b_noti FROM cco_enca_navieras WHERE c_imo = '{0}' AND c_llegada = '{1}' AND c_naviera = '{2}')
                                    IF @CANTIDAD = 1
	                                    SELECT 1
                                    ELSE
	                                    SELECT 0 ";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string ObtenerNotiS(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();



                string _consulta = @"DECLARE @CANTIDAD INT
                                    SET @CANTIDAD = (SELECT COUNT(b_autorizado) FROM CCO_ENCA_NAVIERAS A INNER JOIN CCO_DETA_NAVIERAS B 
                                                    ON A.IdReg = B.IdReg WHERE c_imo = '{0}' AND c_llegada = '{1}' AND c_naviera = '{2}'
                                                    AND b_autorizado = 0)
                                    IF @CANTIDAD > 0
                                        SELECT 1
                                    ELSE
                                        SELECT 0";



                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static List<EncaNaviera> ObtenerCabeceraCancel(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /* string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0) 
                                     FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                     WHERE a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_cancelado = 0)
                                     GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, b.n_manifiesto";*/

                string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, b.c_llegada, a.c_imo, a.c_naviera, c_voyage, c_prefijo
                                    FROM CCO_DETA_DOC_NAVI b INNER JOIN CCO_ENCA_NAVIERAS a ON b.IdReg = a.IdReg 
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON d.c_naviera = a.c_naviera 
                                    WHERE b.IdReg IN(SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND f_rpatio IS NULL) AND b.a_manifiesto >= year(getdate())
                                    AND b.b_estado = 1 AND f_valid IS NOT NULL 
                                    GROUP BY a.IdReg, b.c_llegada, a.c_imo, c_voyage, a.c_naviera, c_voyage, c_prefijo
                                    ORDER BY A.IdReg DESC";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),                        
                        c_naviera = _reader.GetString(4),
                        c_voyage = _reader.GetString(5),
                        d_naviera_p = _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraDAN(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

//                string consulta = @"SELECT CantArch, IdReg, c_llegada, c_imo, f_llegada, c_naviera, COUNT(n_manifiesto)
//                                    FROM (SELECT COUNT(DISTINCT b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0) n_manifiesto, ISNULL(a_manifiesto, 0) a_manifiesto
//                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
//                                    INNER JOIN CCO_DETA_NAVIERAS c ON a.IdReg = b.IdReg
//                                    WHERE b_noti = 1  AND b.b_estado = 1 AND c.b_autorizado = 1 AND c.b_cancelado = 0 AND c.b_detenido = 0 AND YEAR(f_llegada) > = 2017
//                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto, a_manifiesto) A 
//                                    GROUP BY CantArch, IdReg, c_llegada, c_imo, f_llegada, c_naviera
//                                    ORDER BY IdReg desc";

                SqlCommand _command = new SqlCommand("PA_DAN_PRIN_RETEN", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

               
                SqlDataReader _reader = _command.ExecuteReader();


                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        f_llegada = (DateTime)_reader.GetDateTime(4),
                        c_naviera = _reader.GetString(5),
                        num_manif = (int)_reader.GetInt32(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

       


        public static List<EncaNaviera> ObtenerCabeceraTrans(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT a.c_llegada, a.c_imo, convert(char(10), a.f_llegada, 103) + ' 00:00:00'
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS z ON b.IdReg = z.IdReg AND b.IdDoc = z.IdDoc
                                    WHERE b_noti = 1  AND b.b_estado = 1 AND b_autorizado = 1 AND b_cancelado = 0 --AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0)
                                    GROUP BY a.c_llegada, a.c_imo, convert(char(10), a.f_llegada, 103)";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        
                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1),
                        f_llegada = Convert.ToDateTime(_reader.GetString(2))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraTransSw(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT a.c_llegada, a.c_imo, MAX(f_llegada) f_llegada
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS z ON a.IdReg = z.IdReg
                                    WHERE b_noti = 1  AND b.b_estado = 1 AND b_autorizado = 1 AND b_cancelado = 0 AND b.b_siduneawd = 1 --AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0)
                                    GROUP BY a.c_llegada, a.c_imo";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {

                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1),
                        f_llegada = (DateTime)_reader.GetDateTime(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraDANId(DBComun.Estado pEstado, string c_llegada)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT a.c_llegada, a.c_imo, MAX(f_llegada) f_llegada
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS z ON a.IdReg = z.IdReg
                                    WHERE b_noti = 1  AND b.b_estado = 1 AND b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 1 AND a.c_llegada = '{0}'
                                    GROUP BY a.c_llegada, a.c_imo";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {                        
                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1),
                        f_llegada = (DateTime)_reader.GetDateTime(2)                       
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraDANId(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT a.c_llegada, a.c_imo, MAX(f_llegada) f_llegada
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS z ON a.IdReg = z.IdReg
                                    WHERE b_noti = 1  AND b.b_estado = 1 AND b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 1
                                    GROUP BY a.c_llegada, a.c_imo";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1),
                        f_llegada = (DateTime)_reader.GetDateTime(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraDANIdR(DBComun.Estado pEstado, int pId)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

//                string consulta = @"SELECT CantArch, IdReg, c_llegada, c_imo, f_llegada, c_naviera, n_manifiesto, c_voyage, a_manifiesto
//                                    FROM (SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_voyage, c_naviera, ISNULL(b.n_manifiesto, 0) n_manifiesto, b.a_manifiesto
//                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
//                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
//                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_voyage, c_naviera, n_manifiesto, a_manifiesto) A 
//                                    WHERE IdReg = {0}
//                                    GROUP BY CantArch, IdReg, c_llegada, c_imo, f_llegada, c_naviera, n_manifiesto, c_voyage, a_manifiesto";

//                SqlCommand _command = new SqlCommand(string.Format(consulta, pId),  _conn as SqlConnection);
//                _command.CommandType = CommandType.Text;

//                SqlDataReader _reader = _command.ExecuteReader();

                SqlCommand _command = new SqlCommand("PA_DAN_PRIN_RETEN_ID", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));


                SqlDataReader _reader = _command.ExecuteReader();



                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        f_llegada = (DateTime)_reader.GetDateTime(4),
                        c_naviera = _reader.GetString(5),
                        num_manif = (int)_reader.GetInt32(6),
                        c_voyage = _reader.GetString(7),
                        a_manifiesto = _reader.GetString(8)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraDANL(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*  string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
                                      FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                      WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 1)
                                      GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT a.c_llegada, a.c_imo, MAX(f_llegada) f_llegada
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_NAVIERAS z ON a.IdReg = z.IdReg
                                    WHERE b_noti = 1  AND b.b_estado = 1 AND b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 1 --AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0)
                                    GROUP BY a.c_llegada, a.c_imo";


                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                       
                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1),
                        f_llegada = (DateTime)_reader.GetDateTime(2)                        
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerNavierasValid(DBComun.Estado pEstado, int n_manifiesto, int IdReg, string a_manifiesto, int IdDoc)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT c_naviera, c_voyage FROM cco_enca_navieras WHERE IdReg IN(SELECT IdReg FROM CCO_DETA_DOC_NAVI WHERE n_manifiesto = {0} AND IdReg = {1} AND a_manifiesto = '{2}' AND IdDoc = {3}) ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_manifiesto, IdReg, a_manifiesto, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_naviera = _reader.GetString(0),
                        c_voyage = _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerNavierasValidEx(DBComun.Estado pEstado, int IdReg)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT c_naviera, c_voyage FROM CCO_ENCA_EXPO_NAVI WHERE IdReg IN(SELECT IdReg FROM CCO_DETA_DOC_EXP_NAVI WHERE IdReg = {0}) ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, IdReg), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_naviera = _reader.GetString(0),
                        c_voyage = _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ObtenerNavi(DBComun.Estado pEstado, string pNaviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT TOP 1 c_navi_corto FROM CCO_USUARIOS WHERE c_naviera = '{0}' ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pNaviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string _resultado = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _resultado;
            }

        }

        public static List<EncaNaviera> ObtenerDetalle(int pId)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT c_imo, c_llegada, c_naviera, c_voyage
                                    FROM CCO_ENCA_NAVIERAS 
                                    WHERE IdReg = {0} ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_imo = _reader.GetString(0),
                        c_llegada = _reader.GetString(1),
                        c_naviera = _reader.GetString(2),
                        c_voyage = _reader.GetString(3)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerValidas()
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdRegNavi, c_naviera FROM CCO_NAVIERAS_VALIDAS";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_naviera = _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerCabeceraAuto(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT CantArch, IdReg, c_llegada, c_imo, f_llegada, c_naviera, COUNT(n_manifiesto)
                                    FROM (SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0) n_manifiesto
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1  AND b.b_estado = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1)
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto) A 
                                    GROUP BY CantArch, IdReg, c_llegada, c_imo, f_llegada, c_naviera";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        f_llegada = (DateTime)_reader.GetDateTime(4),
                        c_naviera = _reader.GetString(5),
                        num_manif = (int)_reader.GetInt32(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> ObtenerIdTrans(DBComun.Estado pEstado, string c_llegada)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                SqlCommand _command = new SqlCommand("PA_ENCA_TRANSMISION", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;
                
                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                  
                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1),                        
                        Total_Imp = _reader.IsDBNull(2) ? 0 : (int)_reader.GetInt32(2),
                        Total_TransA = _reader.IsDBNull(3) ? 0 : (int)_reader.GetInt32(3),
                        Total_PTransA = _reader.IsDBNull(4) ? 0 : (int)_reader.GetInt32(4),
                        Total_Trans = _reader.IsDBNull(5) ? 0 : (int)_reader.GetInt32(5),
                        Total_PTrans = _reader.IsDBNull(6) ? 0 : (int)_reader.GetInt32(6),
                        Total_Cancel = _reader.IsDBNull(7) ? 0 : (int)_reader.GetInt32(7)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActSustitucion(int idReg, int b_susti)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_ACT_BSUSTI", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", idReg));
                _command.Parameters.Add(new SqlParameter("@b_susti", b_susti));


                //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<EncaNaviera> ObtenerCambiosTransi(DBComun.Estado pEstado)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(b.n_manifiesto, 0)
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 0 AND b_cancelado = 0)
                                    GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, n_manifiesto"; and a.idReg = 663 --*/

                string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, b.c_llegada, a.c_imo, a.c_naviera, c_voyage, c_prefijo
                                    FROM CCO_DETA_DOC_NAVI b INNER JOIN CCO_ENCA_NAVIERAS a ON b.IdReg = a.IdReg 
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON d.c_naviera = a.c_naviera 
                                    WHERE b.IdReg IN(SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND f_rpatio IS NULL) AND b.a_manifiesto >= year(getdate())
                                    AND b.b_estado = 1 AND f_valid IS NOT NULL 
                                    GROUP BY a.IdReg, b.c_llegada, a.c_imo, c_voyage, a.c_naviera, c_voyage, c_prefijo
                                    ORDER BY A.IdReg DESC";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        CantArch = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        c_naviera = _reader.GetString(4),
                        c_voyage = _reader.GetString(5),
                        d_naviera_p = _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<CorteCOTECNA> obtCabeceraDGA(DBComun.Estado pEstado)
        {
            List<CorteCOTECNA> notiLista = new List<CorteCOTECNA>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_CABECERA_DGA", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorteCOTECNA _notificacion = new CorteCOTECNA
                    {
                        c_llegada = _reader.IsDBNull(0) ? "":_reader.GetString(0),
                        c_cliente = _reader.IsDBNull(1) ? "" :_reader.GetString(1),
                        n_manifiesto = _reader.IsDBNull(2) ? "" :_reader.GetString(2),
                        t_contenedores = _reader.IsDBNull(3) ? 0 : (int)_reader.GetInt32(3),
                        t_dga = _reader.IsDBNull(4) ? 0 :(int)_reader.GetInt32(4),
                        c_prefijo = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        b_solidga = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_voyage = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        IdDoc = _reader.IsDBNull(8) ? 0 : (int)_reader.GetInt32(8)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaNaviera> cabeDetaDGA(DBComun.Estado pEstado, string n_manifiesto, string c_cliente)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT COUNT(b.IdDoc) CantArch, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera, ISNULL(n_manifiesto, 0)
									FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
									WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
									GROUP BY a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_naviera";*/

                string consulta = @"SELECT b.IdDoc, a.IdReg, a.c_llegada, a.c_imo, f_llegada, c_voyage, c_naviera, ISNULL(b.n_manifiesto, 0) n_manifiesto, b.a_manifiesto
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND a.IdReg IN (SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado = 1 AND b_cancelado = 0 AND b_detenido = 0)
                                    AND b.a_manifiesto + CAST(b.n_manifiesto AS VARCHAR(4)) = '{0}' and a.c_naviera = '{1}' and b_estado = 1";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_manifiesto, c_cliente),  _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        IdDoc = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_llegada = _reader.GetString(2),
                        c_imo = _reader.GetString(3),
                        f_llegada = (DateTime)_reader.GetDateTime(4),
                        c_voyage = _reader.GetString(5),
                        c_naviera = _reader.GetString(6),
                        num_manif = (int)_reader.GetInt32(7),                        
                        a_manifiesto = _reader.GetString(8)
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
