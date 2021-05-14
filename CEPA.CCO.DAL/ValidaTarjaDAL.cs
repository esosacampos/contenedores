using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;
using System.Globalization;
using Sybase.Data.AseClient;

namespace CEPA.CCO.DAL
{
    public class ValidaTarjaDAL
    {
       
        
        public static List<Manifiesto> ObtenerContValida(string c_tarja)
        {
            List<Manifiesto> pLista = new List<Manifiesto>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"select top 1 a.c_tarja, a.b_estado, b.d_clase_contenido, b.v_pes_rec, b.d_remarcas
                                    from fa_manifiestos a, fa_bl_det b 
                                    where a.c_tarja = b.c_tarja and 
                                    a.c_tarja = '{0}' and b.d_remarcas <>''";


                AseCommand _command = new AseCommand(string.Format(_consulta, c_tarja), _conn as AseConnection);
                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Manifiesto _tmp = new Manifiesto
                    {
                       n_contenedor = _reader.IsDBNull(4) ? "NO EXISTE ASOCIADO" : _reader.GetString(4) 
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static List<string> GetContenedor(string prefix, int n_manifiesto, int a_mani)
        {
            List<string> pLista = new List<string>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT n_contenedor, IdDeta
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    WHERE b.b_estado = 1 AND b.n_manifiesto = {0} AND a_manifiesto = '{1}' AND a.b_autorizado = 1 AND b_cancelado = 0
                                    AND a.n_contenedor LIKE '_______{2}%'";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, n_manifiesto, a_mani, prefix), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {                  

                    pLista.Add(string.Format("{0}-{1}", _reader.IsDBNull(0) ? "" : _reader.GetString(0), _reader.IsDBNull(1) ? "0" : Convert.ToString(_reader.GetInt32(1))));
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static List<string> GetContenedor(string prefix, int n_manifiesto, int a_mani, string b_dga)
        {
            List<string> pLista = new List<string>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT n_contenedor, IdDeta
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    WHERE b.b_estado = 1 AND b.n_manifiesto = {0} AND a_manifiesto = '{1}' AND a.b_autorizado = 1 AND f_retencion_dga IS NULL AND b_cancelado = 0
                                    AND a.n_contenedor LIKE '_______{2}%'";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, n_manifiesto, a_mani, prefix), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    pLista.Add(string.Format("{0}-{1}", _reader.IsDBNull(0) ? "" : _reader.GetString(0), _reader.IsDBNull(1) ? "0" : Convert.ToString(_reader.GetInt32(1))));
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static List<DetaNaviera> GetContenedor(string prefix)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                /*string _consulta = @"SELECT a.n_contenedor, a.IdDeta, b.c_llegada
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                    WHERE a.b_autorizado = 1 AND b_cotecna = 1 AND b_cancelado = 0 AND b_recepcion = 0
                                    AND a.n_contenedor  LIKE '_______{0}%'
                                    ORDER BY IdDeta DESC ";*/

                /*string _consulta = @"SELECT a.n_contenedor, a.IdDeta, b.c_llegada
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                    WHERE a.b_autorizado = 1 AND a.b_recepcion = 0 AND b_cancelado = 0
                                    AND a.n_contenedor LIKE '_______{0}%' AND b.c_llegada = '4.11877'";*/

                SqlCommand _command = new SqlCommand("pa_getContenedor", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@prefix", prefix));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    //pLista.Add(string.Format("{0}-{1}", _reader.IsDBNull(0) ? "" : _reader.GetString(0), _reader.IsDBNull(1) ? "0" : Convert.ToString(_reader.GetInt32(1))));
                    DetaNaviera _tmp = new DetaNaviera
                    {
                        n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        IdDeta = _reader.IsDBNull(1) ? 0 : _reader.GetInt32(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2)
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }


        public static List<DetaNaviera> GetContenedorDecla(string prefix, int years)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT a.n_contenedor, MAX(a.IdRegAduana)
                                    FROM CCO_ESTADOS_DECLA a 
                                    WHERE a.n_contenedor LIKE '_______{0}%' AND YEAR(f_reg_aduana) = {1}
                                    GROUP BY a.n_contenedor ";




                SqlCommand _command = new SqlCommand(string.Format(_consulta, prefix, years), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    //pLista.Add(string.Format("{0}-{1}", _reader.IsDBNull(0) ? "" : _reader.GetString(0), _reader.IsDBNull(1) ? "0" : Convert.ToString(_reader.GetInt32(1))));
                    DetaNaviera _tmp = new DetaNaviera
                    {
                        n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        IdDeta = _reader.IsDBNull(1) ? 0 : _reader.GetInt32(1)                        
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static List<DetaNaviera> GetNumDecla(string prefix, int years)
        {
            List<DetaNaviera> pLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT CONCAT(a_decla, '-', s_decla, '-', c_decla) n_declaracion, MAX(a.IdRegAduana) IdReg
                                    FROM CCO_ESTADOS_DECLA a 
                                    WHERE a_decla is not null and CONCAT(a_decla, '-', s_decla, '-', c_decla) LIKE '_______{0}%' AND YEAR(f_reg_aduana) = {1}
                                    GROUP BY a_decla, s_decla, c_decla 
                                    union all
                                    SELECT CONCAT(a_transito, '-', r_transito) n_declaracion, MAX(a.IdRegAduana) IdReg
                                    FROM CCO_ESTADOS_DECLA a 
                                    WHERE a_transito is not null and CONCAT(a_transito, '-', r_transito) LIKE '%{2}' AND YEAR(f_reg_aduana) = {3}
                                    GROUP BY a_transito, r_transito
                                    ORDER BY 1 ";




                SqlCommand _command = new SqlCommand(string.Format(_consulta, prefix, years, prefix, years), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    //pLista.Add(string.Format("{0}-{1}", _reader.IsDBNull(0) ? "" : _reader.GetString(0), _reader.IsDBNull(1) ? "0" : Convert.ToString(_reader.GetInt32(1))));
                    DetaNaviera _tmp = new DetaNaviera
                    {
                        n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        IdDeta = _reader.IsDBNull(1) ? 0 : _reader.GetInt32(1)
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }


        public static List<InfOperaciones> GetContenedorInfor(string n_contenedor)
        {
            List<InfOperaciones> pLista = new List<InfOperaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_MOV_RECEPCION", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));               

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    InfOperaciones _tmp = new InfOperaciones
                    {
                        c_tamaño = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        v_tara = _reader.IsDBNull(1) ? 0 : (int)_reader.GetInt32(1),
                        c_trafico = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        c_llegada = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        c_estado = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_naviera = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_cliente = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        IdDeta = _reader.IsDBNull(7) ? 0 : (int)_reader.GetInt32(7),
                        b_detenido = _reader.IsDBNull(8) ? "": _reader.GetString(8),
                        b_style = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_aduana = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_staduana = _reader.IsDBNull(11) ? "" : _reader.GetString(11)
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }


        public static string SaveConfirmacion(DBComun.Estado pEstado, int pIdDeta, string s_observaciones, string c_marcacion, string b_directo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
       
                string consulta = @"DECLARE @b_ret_dir AS CHAR(1), @numrow INT
                                SET @b_ret_dir = (SELECT CASE WHEN b_ret_dir = 'Y' THEN 'Y' ELSE 'N' END b_ret_Dir FROM CCO_DETA_NAVIERAS WHERE IdDeta = @IdDeta)

                                UPDATE CCO_DETA_NAVIERAS 
                                SET b_recepcion = 1, f_recepcion = GETDATE(), s_obser_recep = @Observaciones, c_marcacion = @c_marcacion
                                WHERE IdDeta = @IdDeta
                                SET @numrow = @@ROWCOUNT

                                IF (@b_ret_dir = 'Y')
                                BEGIN
	                                UPDATE CCO_DETA_NAVIERAS
	                                SET f_rpatio = GETDATE(), sitio = 'RETIRO DIRECTO'
	                                WHERE IdDeta = @IdDeta
	                                SET @numrow = @@ROWCOUNT
                                END

                                SELECT @numrow";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
               
                _command.Parameters.AddWithValue("@IdDeta", pIdDeta);
                _command.Parameters.AddWithValue("@Observaciones", s_observaciones);
                _command.Parameters.AddWithValue("@c_marcacion", c_marcacion);

                _command.CommandType = CommandType.Text;
                string _reader = _command.ExecuteScalar().ToString();               

                _conn.Close();

                //if (b_directo == "1")
                //{
                //    SaveVFPDirecto(pIdDeta);                    
                //}

                return _reader;
            }

        }

        public static string ActRetDirPATIO(DBComun.Estado pEstado, int pIdDeta)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS
	                                SET f_rpatio = GETDATE(), sitio = 'RETIRO DIRECTO'
	                                WHERE IdDeta = {0}
                                    SELECT @@ROWCOUNT";
	                                

                SqlCommand _command = new SqlCommand(string.Format(consulta, pIdDeta), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();                

                return _reader;
            }

        }

        public static string SaveVFPDirecto(int IdDeta)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlLink, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("exc_vfpDirectos", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdDeta", IdDeta));

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static List<InfOperaciones> getAlertaRecepcion(DBComun.Estado pEstado)
        {
            List<InfOperaciones> pLista = new List<InfOperaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string _consulta = @"SELECT CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'R' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, v_tara, CASE WHEN b_ret_dir = 'Y' THEN 'RETIRO DIRECTO' ELSE 'DESPACHO NORMAL' END trafico, 
                                    c_llegada, CASE WHEN a.b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estadoc, b.c_naviera, z.c_prefijo, a.IdDeta,
                                    ISNULL((CONVERT(CHAR(10), f_rpatio, 103) + ' '  + CONVERT(CHAR(10), f_rpatio, 108)), '') f_rpatio, n_contenedor, CASE WHEN LEN(a.s_comentarios) = 0 THEN '' ELSE a.s_comentarios END  s_comentarios, f_rpatio
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_USUARIOS_NAVIERAS z ON b.c_naviera = z.c_naviera
                                    WHERE f_rpatio IS NOT NULL AND f_recepcion IS NULL AND b_envio = 0 AND b_cancelado = 0";


                SqlCommand _command = new SqlCommand(_consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    InfOperaciones _tmp = new InfOperaciones
                    {
                        c_tamaño = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        v_tara = _reader.IsDBNull(1) ? 0 : (int)_reader.GetInt32(1),
                        c_trafico = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        c_llegada = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        c_estado = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_naviera = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_cliente = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        IdDeta = _reader.IsDBNull(7) ? 0 : (int)_reader.GetInt32(7),
                        f_recepcion = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        n_contenedor = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        s_comentarios = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        f_realrpatio = Convert.ToDateTime(_reader.GetDateTime(11))
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static List<InfOperaciones> shoSummaryBuque()
        {
            List<InfOperaciones> pLista = new List<InfOperaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT a.c_llegada, count(*) total,0 oirsa, 0 patio INTO #Total
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b.b_autorizado = 1 AND b.b_cancelado = 0 
                                    GROUP BY c_llegada

                                    SELECT a.c_llegada INTO #Llegadas
                                    FROM CCO_ENCA_NAVIERAS a inner JOIN CCO_DETA_NAVIERAS b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b.f_recepcion IS NULL AND b.b_autorizado = 1 AND b.b_cancelado = 0 AND b_recepcion = 0
                                    GROUP BY c_llegada


                                    select c_llegada, 0 total, sum(oirsa) oirsa, sum(patio) patio into #sumas
                                    FROM(SELECT a.c_llegada, 0 total, SUM(CASE WHEN f_recepcion IS NOT NULL THEN 1 ELSE 0 END) oirsa, 0 patio 
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b.b_autorizado = 1 AND b.b_cancelado = 0 AND
                                    c_llegada IN (SELECT c_llegada FROM #Llegadas)
                                    GROUP BY c_llegada
                                    union all
                                    SELECT a.c_llegada, 0 total, 0 oirsa, SUM(CASE WHEN f_rpatio IS NOT NULL THEN 1 ELSE 0 END) patio
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b.b_autorizado = 1 AND b.b_cancelado = 0 AND
                                    c_llegada IN (SELECT c_llegada FROM #Llegadas)
                                    GROUP BY c_llegada) A
                                    GROUP by c_llegada, total


                                    select b.c_llegada, sum(a.total) Total, sum(b.oirsa) OIRSA, sum(a.total) - sum(b.oirsa) PO, sum(b.patio) PATIO, sum(a.total) - sum(b.patio) PP from #Total a right join #sumas b on a.c_llegada = b.c_llegada
                                    group by b.c_llegada
                                    ORDER BY c_llegada



                                    drop table #sumas
                                    drop table #Total
                                    DROP TABLE #Llegadas";


                SqlCommand _command = new SqlCommand(_consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    InfOperaciones _tmp = new InfOperaciones
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        Total = _reader.IsDBNull(1) ? 0 : (int)_reader.GetInt32(1),
                        OIRSA = _reader.IsDBNull(2) ? 0 : (int)_reader.GetInt32(2),
                        PO = _reader.IsDBNull(3) ? 0 : (int)_reader.GetInt32(3),
                        PATIO = _reader.IsDBNull(4) ? 0 : (int)_reader.GetInt32(4),
                        PP = _reader.IsDBNull(5) ? 0 : (int)_reader.GetInt32(5)                       
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static List<InfOperaciones> getDataRecepcion()
        {
            List<InfOperaciones> pLista = new List<InfOperaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT a.c_llegada INTO #Llegadas
                                    FROM CCO_ENCA_NAVIERAS a inner JOIN CCO_DETA_NAVIERAS b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b.f_recepcion IS NULL AND b.b_autorizado = 1 AND b.b_cancelado = 0 AND b_recepcion = 0
                                    GROUP BY c_llegada

                                    SELECT n_contenedor, ISNULL((CONVERT(CHAR(10), f_recepcion, 103) + '  ' + CONVERT(CHAR(10), f_recepcion, 108)), '') f_recepcion, c_marcacion, c_llegada
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON  a.IdReg = b.IdReg
                                    WHERE b_noti = 1 AND b.b_autorizado = 1 AND b.b_cancelado = 0 AND 
                                    c_llegada IN (SELECT c_llegada FROM #Llegadas) and f_recepcion is not null
                                    order by 2 desc


                                    drop table #Llegadas";


                SqlCommand _command = new SqlCommand(_consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    InfOperaciones _tmp = new InfOperaciones
                    {
                        n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        f_recepcion = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_marcacion = _reader.IsDBNull(2) ? 0 : (int)_reader.GetInt32(2),
                        c_llegada = _reader.IsDBNull(3) ? "" : _reader.GetString(3)
                    };

                    pLista.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return pLista;
            }
        }

        public static string ValidFact(DBComun.Estado pEstado, string n_contenedor, string n_manifiesto, DBComun.TipoBD pDB)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(pDB, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_valid_fact", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }
    }
}
