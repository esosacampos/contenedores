using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;


namespace CEPA.CCO.DAL
{
    public class ResulNavieraDAL
    {
        public static List<ResulNaviera> ObtenerArchivo(DBComun.Estado pEstado, string c_naviera, int pManifiesto)
        {
            List<ResulNaviera> notiLista = new List<ResulNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT IdDeta, n_BL, n_contenedor, c.d_descripcion + ' ' + d.d_tipo as c_tamaño, v_peso, CASE WHEN b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END as b_estado, s_consignatario, a.c_naviera, a.c_llegada, a.c_imo, a.f_llegada, ISNULL(a.n_manifiesto,0) 
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    WHERE b_autorizado = 0 AND b_cancelado = 0  
                                    AND EXISTS(SELECT IdReg, c_llegada, c_imo, f_llegada, c_naviera, c_voyage
                                    FROM CCO_ENCA_NAVIERAS
                                    WHERE b_noti = 0 AND (CONVERT(CHAR(10),GETDATE(), 103) >= CONVERT(CHAR(10), dateadd(hh, -48, f_llegada), 103)) AND
                                    CONVERT(CHAR(10),GETDATE(), 103) <= CONVERT(CHAR(10), dateadd(hh, -4, f_llegada), 103) AND c_naviera = '{0}')
                                    ORDER BY n_contenedor";*/

                string consulta = @"SELECT IdDeta, n_BL, n_contenedor, c.d_descripcion + ' ' + d.d_tipo as c_tamaño, v_peso, CASE WHEN b.b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END as b_estado, s_consignatario, a.c_naviera, a.c_llegada, a.c_imo, a.f_llegada, ISNULL(z.n_manifiesto,0) 
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON a.IdReg = z.IdReg AND b.IdDoc = z.IdDoc
                                    WHERE b_autorizado = 0 AND b_cancelado = 0  AND z.n_manifiesto = {0}
                                    AND EXISTS(SELECT IdReg, c_llegada, c_imo, f_llegada, c_naviera, c_voyage
                                    FROM CCO_ENCA_NAVIERAS
                                    WHERE b_noti = 0 AND (CONVERT(CHAR(10),GETDATE(), 103) >= CONVERT(CHAR(10), dateadd(hh, -48, f_llegada), 103)) AND
                                    CONVERT(CHAR(10),GETDATE(), 103) <= CONVERT(CHAR(10), dateadd(hh, -4, f_llegada), 103) AND c_naviera = '{1}')
                                    ORDER BY n_contenedor";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pManifiesto, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ResulNaviera _notificacion = new ResulNaviera
                    {
                        IdDeta = (int)_reader.GetInt32(0),
                        n_BL = _reader.GetString(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        b_estado = _reader.GetString(5),
                        c_naviera = _reader.GetString(7),
                        c_llegada = _reader.GetString(8),
                        f_llegada = (DateTime)_reader.GetSqlDateTime(10),
                        num_manif = (int)_reader.GetInt32(11)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ResulNaviera> ObtenerArchivoCarga(DBComun.Estado pEstado, int IdReg)
        {
            List<ResulNaviera> notiLista = new List<ResulNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, n_BL, n_contenedor, c.d_descripcion + ' ' + d.d_tipo as c_tamaño, v_peso, CASE WHEN b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END as b_estado, s_consignatario, a.c_naviera, a.c_llegada, a.c_imo, a.f_llegada, ISNULL(n_manifiesto, 0)
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    WHERE b_autorizado = 0 AND b_cancelado = 0 AND a.IdReg = {0} 
                                    AND EXISTS(SELECT IdReg, c_llegada, c_imo, f_llegada, c_naviera, c_voyage
                                    FROM CCO_ENCA_NAVIERAS
                                    WHERE b_noti = 1 AND IdReg = {1})
                                    ORDER BY n_contenedor";

                SqlCommand _command = new SqlCommand(string.Format(consulta, IdReg, IdReg), _conn as SqlConnection);                
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ResulNaviera _notificacion = new ResulNaviera
                    {
                        IdDeta = (int)_reader.GetInt32(0),
                        n_BL = _reader.GetString(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        b_estado = _reader.GetString(5),
                        c_naviera = _reader.GetString(7),
                        c_llegada = _reader.GetString(8),
                        f_llegada = (DateTime)_reader.GetSqlDateTime(10),
                        num_manif = (int)_reader.GetInt32(11)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ResulNaviera> ObtenerArchivoPost(DBComun.Estado pEstado, int IdReg, int pMin, int pMax)
        {
            List<ResulNaviera> notiLista = new List<ResulNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, n_BL, n_contenedor, c.d_descripcion + ' ' + d.d_tipo as c_tamaño, v_peso, CASE WHEN b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END as b_estado, s_consignatario, a.c_naviera, a.c_llegada, a.c_imo, a.f_llegada, a.n_manifiesto
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    WHERE b_autorizado = 0 AND b_cancelado = 0 AND a.IdReg = {0} AND IdDeta BETWEEN {1} AND {2}
                                    AND EXISTS(SELECT IdReg, c_llegada, c_imo, f_llegada, c_naviera, c_voyage
                                    FROM CCO_ENCA_NAVIERAS
                                    WHERE b_noti = 1 AND IdReg = {3})
                                    ORDER BY n_contenedor";

                SqlCommand _command = new SqlCommand(string.Format(consulta, IdReg, pMin, pMax, IdReg), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ResulNaviera _notificacion = new ResulNaviera
                    {
                        IdDeta = (int)_reader.GetInt32(0),
                        n_BL = _reader.GetString(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        b_estado = _reader.GetString(5),
                        c_naviera = _reader.GetString(7),
                        c_llegada = _reader.GetString(8),
                        f_llegada = (DateTime)_reader.GetSqlDateTime(10),
                        num_manif = (int)_reader.GetInt32(11)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerAutorizadosADUANA(DBComun.Estado pEstado, int n_manifiesto, string c_naviera, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_aduana_valid_autorizados", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };
                    

                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerAutorizadosADUANAEx(DBComun.Estado pEstado, int IdReg, string c_naviera)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_autorizados_ex", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", IdReg));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };


                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<IncoAduana> ObtenerInconsistencias(DBComun.Estado pEstado, int IdReg, int n_manif, string c_naviera)
        {
            List<IncoAduana> notiLista = new List<IncoAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                string consulta = @"SELECT n_contenedor
                                    FROM CCO_INCONSISTENCIA_ADUANA
                                    WHERE idreg = {0} and num_manif = {1} AND c_naviera = '{2}'";

                SqlCommand _command = new SqlCommand(string.Format(consulta, IdReg, n_manif, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    IncoAduana _notificacion = new IncoAduana
                    {                       
                        n_contenedor = _reader.GetString(0)                       
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerNoInco(DBComun.Estado pEstado, int n_manifiesto, string c_naviera, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_deta_inco_validacion", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manif", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@c_navi", c_naviera));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));
                _command.CommandTimeout = 120;
                SqlDataReader _reader = _command.ExecuteReader();
                

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };


                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }



        public static List<ContenedoresAduana> ObtenerAutorizadosNOADUANA(DBComun.Estado pEstado, int n_manifiesto, string c_naviera, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_no_aduana", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(1),
                        c_correlativo = correlativo
                    };

                    
                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerAutorizadosNOWEIGTH(DBComun.Estado pEstado, int n_manifiesto, string c_naviera, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_no_weigth", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        v_aduana = Convert.ToDouble(_reader.GetDecimal(1)),
                        v_cepa = Convert.ToDouble(_reader.GetDecimal(3)),
                        c_correlativo = correlativo
                    };


                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerAutorizadosSHIPPER(DBComun.Estado pEstado, int n_manifiesto, string c_naviera, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_no_shipper", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        b_ship_aduana = _reader.GetString(1),
                        b_ship_cepa = _reader.GetString(3),
                        c_correlativo = correlativo
                    };


                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerAutorizadosNOADUANAEx(DBComun.Estado pEstado, int IdReg, string c_naviera)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_no_aduana_ex", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", IdReg));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(1),
                        c_correlativo = correlativo
                    };


                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }



        public static List<ContenedoresAduana> ObtenerAutorizadosNONAVIERA(DBComun.Estado pEstado, int n_manifiesto, string c_naviera, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_no_naviera", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };

                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerAutorizadosNONAVIERAEx(DBComun.Estado pEstado, int IdReg, string c_naviera)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_no_naviera_ex", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", IdReg));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };

                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerRepetidos(DBComun.Estado pEstado, int n_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_repetidos", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));                

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };

                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerRepetidosEx(DBComun.Estado pEstado, int IdReg)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            int correlativo = 1;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_aduana_valid_repetidos_ex", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", IdReg));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_contenedor = _reader.GetString(0),
                        c_correlativo = correlativo
                    };

                    notiLista.Add(_notificacion);
                    correlativo = correlativo + 1;
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string EliminarInco(DBComun.Estado pEstado, int IdReg, int n_manifiesto, string c_naviera)
        {

            string resultado = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                IDbTransaction tranDel = _conn.BeginTransaction();



                string consulta = @"DELETE  
                                FROM CCO_INCONSISTENCIA_ADUANA
                                WHERE idreg = {0} and num_manif = {1} and c_naviera = '{2}'
                                SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, IdReg, n_manifiesto, c_naviera), _conn as SqlConnection);

                _command.CommandType = CommandType.Text;
                _command.Transaction = (SqlTransaction)tranDel;

                try
                {                  

                    resultado = _command.ExecuteScalar().ToString();
                    tranDel.Commit();
                  
                }
                catch (Exception ex)
                {
                    tranDel.Rollback();
                    throw new Exception("Problemas al eliminar inconsistencias " + ex.Message);
                }
                finally
                {
                    _conn.Close();
                }
            }
            return resultado;
        }

        public static string delADUANA_Auto(DBComun.Estado pEstado, int n_manifiesto, string a_mani)
        {

            string resultado = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                IDbTransaction tranDel = _conn.BeginTransaction();



                string consulta = @"DELETE  
                                FROM CCO_ADUANA_VALID_AUTO
                                WHERE n_manifiesto = {0} and a_mani = '{1}'
                                SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_manifiesto, a_mani), _conn as SqlConnection);

                _command.CommandType = CommandType.Text;
                _command.Transaction = (SqlTransaction)tranDel;
                try
                {                  

                    resultado = _command.ExecuteScalar().ToString();
                    tranDel.Commit();
                    
                }
                catch (Exception ex)
                {
                    tranDel.Rollback();
                    throw new Exception("Problemas al eliminar inconsistencias " + ex.Message);
                }
                finally
                {
                    _conn.Close();
                }
            }
            return resultado;
        }



        public static string EliminarManifiesto(DBComun.Estado pEstado, int n_manifiesto, string a_mani, int b_sidunea)
        {
            if( int.Parse(a_mani) == 0)
                a_mani = "2015";
            

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"IF(EXISTS(SELECT a.* FROM CCO_ADUANA_VALID a INNER JOIN CCO_DETA_DOC_NAVI b ON a.a_mani = b.a_manifiesto and a.n_manifiesto = b.n_manifiesto WHERE a.n_manifiesto = {0} and a_mani = '{1}' and a.b_sidunea = {2}))
                                    BEGIN
	                                    DELETE a FROM CCO_ADUANA_VALID a INNER JOIN CCO_DETA_DOC_NAVI b ON a.a_mani = b.a_manifiesto and a.n_manifiesto = b.n_manifiesto WHERE a.n_manifiesto = {3} and a_mani = '{4}' and a.b_sidunea = {5};
	                                    SELECT @@ROWCOUNT
                                    END
                                    ELSE
                                    BEGIN
	                                    SELECT 0
                                    END";
               
                SqlCommand _command = new SqlCommand(string.Format(consulta, n_manifiesto, a_mani, b_sidunea, n_manifiesto, a_mani, b_sidunea), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }

        }

        public static List<ContenedoresAduana> ObtenerManiYear(DBComun.Estado pEstado, int a_manifiesto)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();
            
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_mani_year", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

            
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_manifiesto = (int)_reader.GetInt32(0),                        
                        b_sidunea = _reader.IsDBNull(1) ? 0 : (int)_reader.GetInt32(1),
                        a_manifiesto = _reader.IsDBNull(2) ? "" : _reader.GetString(2)

                    };


                    notiLista.Add(_notificacion);                    
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<ContenedoresAduana> ObtenerManiBuque(DBComun.Estado pEstado, string c_llegada)
        {
            List<ContenedoresAduana> notiLista = new List<ContenedoresAduana>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_mani_buque", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;


                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ContenedoresAduana _notificacion = new ContenedoresAduana
                    {
                        n_manifiesto = (int)_reader.GetInt32(0),
                        b_sidunea = _reader.IsDBNull(1) ? 0 : Convert.ToInt32(_reader.GetBoolean(1)),
                        a_manifiesto = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        c_navi_corto = _reader.IsDBNull(3) ? "" : _reader.GetString(3)
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
