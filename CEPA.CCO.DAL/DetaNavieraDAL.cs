using CEPA.CCO.Entidades;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class DetaNavieraDAL
    {
        public static string AlmacenarDetaNaviera(DetaNaviera _Encanaviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_naviera_save", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (_Encanaviera.b_estado == "E")
                {
                    _Encanaviera.b_transferencia = "Y";
                    _Encanaviera.b_manejo = "Y";
                    _Encanaviera.b_despacho = "Y";
                }

                _command.Parameters.Add(new SqlParameter("@IdReg", _Encanaviera.IdReg));
                _command.Parameters.Add(new SqlParameter("@n_BL", _Encanaviera.n_BL));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", _Encanaviera.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_tamaño", _Encanaviera.c_tamaño));
                _command.Parameters.Add(new SqlParameter("@v_peso", _Encanaviera.v_peso));
                _command.Parameters.Add(new SqlParameter("@b_estado", _Encanaviera.b_estado));
                _command.Parameters.Add(new SqlParameter("@s_consignatario", _Encanaviera.s_consignatario));
                _command.Parameters.Add(new SqlParameter("@n_sello", _Encanaviera.n_sello));
                _command.Parameters.Add(new SqlParameter("@c_pais_destino", _Encanaviera.c_pais_destino));
                _command.Parameters.Add(new SqlParameter("@c_pais_origen", _Encanaviera.c_pais_origen));
                _command.Parameters.Add(new SqlParameter("@c_detalle_pais", _Encanaviera.c_detalle_pais));
                _command.Parameters.Add(new SqlParameter("@s_comodity", _Encanaviera.s_comodity));
                _command.Parameters.Add(new SqlParameter("@s_prodmanifestado", _Encanaviera.s_prodmanifestado));
                _command.Parameters.Add(new SqlParameter("@v_tara", _Encanaviera.v_tara));
                _command.Parameters.Add(new SqlParameter("@b_reef", _Encanaviera.b_reef));
                _command.Parameters.Add(new SqlParameter("@b_ret_dir", _Encanaviera.b_ret_dir));
                _command.Parameters.Add(new SqlParameter("@c_imo_imd", _Encanaviera.c_imo_imd));
                _command.Parameters.Add(new SqlParameter("@c_un_number", _Encanaviera.c_un_number));
                _command.Parameters.Add(new SqlParameter("@b_transhipment", _Encanaviera.b_transhipment));
                _command.Parameters.Add(new SqlParameter("@c_condicion", _Encanaviera.c_condicion));
                _command.Parameters.Add(new SqlParameter("@IdDoc", _Encanaviera.IdDoc));
                _command.Parameters.Add(new SqlParameter("@b_shipper", _Encanaviera.b_shipper));
                _command.Parameters.Add(new SqlParameter("@b_transferencia", _Encanaviera.b_transferencia));
                _command.Parameters.Add(new SqlParameter("@b_manejo", _Encanaviera.b_manejo));
                _command.Parameters.Add(new SqlParameter("@b_despacho", _Encanaviera.b_despacho));


                // _command.Parameters.Add(new SqlParameter("@c_correlativo", _Encanaviera.c_correlativo));

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string AlmacenarDetaNavieraEx(List<ArchivoExport> pLista, int IdReg, int IdDoc)
        {
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
                {
                    int valor = 0;
                    _conn.Open();
                    foreach (ArchivoExport _Encanaviera in pLista)
                    {


                        SqlCommand _command = new SqlCommand("pa_deta_exp_navi_save", _conn as SqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        if (_Encanaviera.b_estado == "E")
                        {
                            _Encanaviera.b_transferencia = "Y";
                            _Encanaviera.b_manejo = "Y";
                            _Encanaviera.b_recepcion = "Y";
                        }

                        _command.Parameters.Add(new SqlParameter("@IdReg", IdReg));
                        _command.Parameters.Add(new SqlParameter("@n_BL", _Encanaviera.n_BL));
                        _command.Parameters.Add(new SqlParameter("@n_booking", _Encanaviera.n_booking));
                        _command.Parameters.Add(new SqlParameter("@n_contenedor", _Encanaviera.n_contenedor));
                        _command.Parameters.Add(new SqlParameter("@c_tamaño", _Encanaviera.c_tamaño));
                        _command.Parameters.Add(new SqlParameter("@v_peso", _Encanaviera.v_peso));
                        _command.Parameters.Add(new SqlParameter("@b_estado", _Encanaviera.b_estado));
                        _command.Parameters.Add(new SqlParameter("@s_exportador", _Encanaviera.s_exportador));
                        _command.Parameters.Add(new SqlParameter("@s_consignatario", _Encanaviera.s_consignatario));
                        _command.Parameters.Add(new SqlParameter("@s_notificador", _Encanaviera.s_notificador));
                        _command.Parameters.Add(new SqlParameter("@n_sello", _Encanaviera.n_sello));
                        _command.Parameters.Add(new SqlParameter("@c_pais_destino", _Encanaviera.c_pais_destino));
                        _command.Parameters.Add(new SqlParameter("@c_detalle_pais", _Encanaviera.c_detalle_puerto));
                        _command.Parameters.Add(new SqlParameter("@s_comodity", _Encanaviera.s_comodity));
                        _command.Parameters.Add(new SqlParameter("@s_promanifiesto", _Encanaviera.s_prodmanifestado));
                        _command.Parameters.Add(new SqlParameter("@v_tara", _Encanaviera.v_tara));
                        _command.Parameters.Add(new SqlParameter("@b_reef", _Encanaviera.b_reef));
                        _command.Parameters.Add(new SqlParameter("@b_emb_dir", _Encanaviera.b_emb_dir));
                        _command.Parameters.Add(new SqlParameter("@c_tipo_doc", _Encanaviera.c_tipo_doc));
                        _command.Parameters.Add(new SqlParameter("@c_arivu", _Encanaviera.c_arivu));
                        _command.Parameters.Add(new SqlParameter("@c_fauca", _Encanaviera.c_fauca));
                        _command.Parameters.Add(new SqlParameter("@c_dm", _Encanaviera.c_dm));
                        _command.Parameters.Add(new SqlParameter("@c_dut", _Encanaviera.c_dut));
                        _command.Parameters.Add(new SqlParameter("@c_dmti", _Encanaviera.c_dmti));
                        _command.Parameters.Add(new SqlParameter("@c_manifiesto", _Encanaviera.c_manifiesto)); ;
                        _command.Parameters.Add(new SqlParameter("@c_pais_origen", _Encanaviera.c_pais_origen));
                        _command.Parameters.Add(new SqlParameter("@b_transferencia", _Encanaviera.b_transferencia));
                        _command.Parameters.Add(new SqlParameter("@b_manejo", _Encanaviera.b_manejo));
                        _command.Parameters.Add(new SqlParameter("@b_recepcion", _Encanaviera.b_recepcion));
                        _command.Parameters.Add(new SqlParameter("@IdDoc", IdDoc));


                        // _command.Parameters.Add(new SqlParameter("@c_correlativo", _Encanaviera.c_correlativo));

                        string resultado = _command.ExecuteScalar().ToString();

                        valor = valor + 1;
                    }
                    _conn.Close();
                    return valor.ToString();

                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public static string EliminarDetaNaviera(int pId, string pArchivo)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_del_deta_naviera", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));
                _command.Parameters.Add(new SqlParameter("@s_archivo", pArchivo));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string EliminarDetaNavieraEx(int pId, string pArchivo)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_del_deta_exp_naviera", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));
                _command.Parameters.Add(new SqlParameter("@s_archivo", pArchivo));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ObtenerCorrelativo(string pC_imo, string pC_llegada, string pC_naviera, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT ISNULL(MAX(c_correlativo), 0) FROM CCO_DETA_NAVIERAS WHERE IdReg 
									IN (SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_imo = '{0}' 
									AND c_llegada = '{1}' AND c_naviera = '{2}') ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pC_imo, pC_llegada, pC_naviera), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ObtenerCorrelativoEx(string pC_imo, string pC_llegada, string pC_naviera, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT ISNULL(MAX(c_correlativo), 0) FROM CCO_DETA_EXPO_NAVI WHERE IdReg 
                                    IN (SELECT IdReg FROM CCO_ENCA_EXPO_NAVI WHERE c_imo = '{0}' 
                                    AND c_llegada = '{1}' AND c_naviera = '{2}')
 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pC_imo, pC_llegada, pC_naviera), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaNaviera> ObtenerDetalle(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                /*string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  c.d_descripcion + ' ' + 'HREF' ELSE c.d_descripcion + ' ' + d.d_tipo END c_tamaño
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    WHERE b_autorizado = 0 AND a.IdReg = {0} AND b_cancelado = 0  ";*/

                string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    WHERE b_autorizado = 0 AND a.IdReg = {0} AND b_cancelado = 0";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerDetalleEx(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                /*string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  c.d_descripcion + ' ' + 'HREF' ELSE c.d_descripcion + ' ' + d.d_tipo END c_tamaño
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    WHERE b_autorizado = 0 AND a.IdReg = {0} AND b_cancelado = 0  ";*/

                string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, b.IdDoc,
                                    b.v_peso, UPPER(g.CountryName) PaisDestino, b.c_detalle_pais
                                    FROM CCO_DETA_EXPO_NAVI b INNER JOIN CCO_ENCA_EXPO_NAVI a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_COD_PAISES g ON c_pais_destino = g.CountryCode
                                    WHERE b_autorizado = 0 AND a.IdReg = {0} AND b_cancelado = 0
                                    ORDER BY c_tamaño";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        IdDoc = _reader.GetInt32(5),
                        v_peso = (double)_reader.GetDecimal(6),
                        c_pais_destino = _reader.GetString(7),
                        c_detalle_pais = _reader.GetString(8)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerDetalle(int pId, int pManif)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                /*string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  c.d_descripcion + ' ' + 'HREF' ELSE c.d_descripcion + ' ' + d.d_tipo END c_tamaño
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN c ON SUBSTRING(c_tamaño, 1, 1) = c.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH d ON SUBSTRING(c_tamaño, 2, 1) = d.IdValue
                                    WHERE b_autorizado = 0 AND a.IdReg = {0} AND b_cancelado = 0  ";*/

                string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, b.IdDoc, z.n_manifiesto
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON a.IdReg = z.IdReg AND b.IdDoc = z.IdDoc
                                    WHERE b_autorizado = 0 AND a.IdReg = {0} AND b_cancelado = 0 AND n_manifiesto = {1}";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pManif), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        IdDoc = _reader.GetInt32(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerDetalleCancel(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, CASE WHEN b_detenido = 1 THEN 'RETENIDO' ELSE 'NADA' END b_detenido, c_correlativo
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    WHERE a.IdReg = {0} AND b_cancelado = 0 AND f_rpatio IS NULL
                                    ORDER BY n_contenedor";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        b_retenido = _reader.IsDBNull(5) ? "" : _reader.GetString(5) == "NADA" ? "" : "RETENIDO",
                        c_correlativo = _reader.GetInt32(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActualizarDeta(DBComun.Estado pEstado, int pId, int pIdDoc)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();



                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_autorizado = 1 
									WHERE IdReg = {0} AND b_autorizado = 0 AND b_cancelado = 0 AND IdDoc = {1}; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pIdDoc), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ManifiestoAuto(DBComun.Estado pEstado, string a_mani, int n_manifiesto)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                string consulta = @"INSERT INTO CCO_ADUANA_VALID_AUTO 
									SELECT n_manifiesto, n_contenedor, f_registro, n_BL, a_mani, c_tipo_bl, b_sidunea, c_tamaños, v_pesos, c_paquete, c_embalaje, d_embalaje, co_pais_origen, d_puerto_origen, co_pais_destino, d_puerto_destino, s_agencia, s_nit, s_consignatarios
                                    FROM CCO_ADUANA_VALID
                                    WHERE a_mani = {0} AND n_manifiesto = {1}                                     
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, a_mani, n_manifiesto), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string RevertirDeta(DBComun.Estado pEstado, int pId, int pIdDoc)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();



                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_autorizado = 0 
									WHERE IdReg = {0} AND IdDoc = {1}; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pIdDoc), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarDetaEx(DBComun.Estado pEstado, List<DetaNaviera> pLista)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                int valor = 0;
                _conn.Open();
                foreach (var item in pLista)
                {


                    SqlCommand _command = new SqlCommand("pa_actualizar_expo_deta", _conn as SqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    _command.Parameters.Add(new SqlParameter("@IdReg", item.IdReg));
                    _command.Parameters.Add(new SqlParameter("@IdDeta", item.IdDeta));


                    string resultado = _command.ExecuteScalar().ToString();
                    valor = valor + 1;

                }
                _conn.Close();
                return valor.ToString();
            }

        }

        public static string ActualizarDetaId(DBComun.Estado pEstado, int pId)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_autorizado = 1 
									WHERE IdDeta = {0} AND b_cancelado = 0 AND b_autorizado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarCancelar(DBComun.Estado pEstado, int pId)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_cancelado = 1 
									WHERE IdReg = {0} AND b_autorizado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarCancelarId(DBComun.Estado pEstado, int pId, string pObservacion, string c_usuarios)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_cancelado = 1, s_observaciones = '{0}', f_cancelado = GETDATE(), us_cancelado = '{1}'
									WHERE IdDeta = {2}; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pObservacion, c_usuarios, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static List<DetaNaviera> ObtenerAutorizados(int pId, int pIdDoc, DBComun.Estado pTipo)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, IdReg, n_BL, n_contenedor, c_tamaño, v_peso, b_estado, s_consignatario, n_sello, c_pais_destino, c_pais_origen, c_detalle_pais, s_comodity, s_prodmanifestado, v_tara, ISNULL(b_reef, '') b_reef, ISNULL(b_ret_dir, '') b_ret_dir, c_imo_imd, c_un_number, b_transhipment, c_condicion
                                    FROM CCO_DETA_NAVIERAS 
                                    WHERE b_autorizado = 1 AND IdReg = {0} AND b_cancelado = 0 AND c_correlativo IS NULL AND IdDoc = {1} ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pIdDoc), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(5)),
                        b_estado = _reader.GetString(6),
                        s_consignatario = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        n_sello = _reader.GetString(8),
                        c_pais_destino = _reader.GetString(9),
                        c_pais_origen = _reader.GetString(10),
                        c_detalle_pais = _reader.GetString(11),
                        s_comodity = _reader.GetString(12),
                        s_prodmanifestado = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        v_tara = Convert.ToDouble(_reader.GetInt32(14)),
                        b_reef = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        b_ret_dir = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        c_imo_imd = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_un_number = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        b_transhipment = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        c_condicion = _reader.IsDBNull(20) ? "" : _reader.GetString(20)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerAutorizados(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, IdReg, n_BL, n_contenedor, c_tamaño, v_peso, b_estado, s_consignatario, n_sello, c_pais_destino, c_pais_origen, c_detalle_pais, s_comodity, s_prodmanifestado, v_tara, ISNULL(b_reef, '') b_reef, ISNULL(b_ret_dir, '') b_ret_dir, c_imo_imd, c_un_number, b_transhipment, c_condicion
                                    FROM CCO_DETA_NAVIERAS 
                                    WHERE b_autorizado = 1 AND IdReg = {0} AND b_cancelado = 0 AND c_correlativo IS NULL";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(5)),
                        b_estado = _reader.GetString(6),
                        s_consignatario = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        n_sello = _reader.GetString(8),
                        c_pais_destino = _reader.GetString(9),
                        c_pais_origen = _reader.GetString(10),
                        c_detalle_pais = _reader.GetString(11),
                        s_comodity = _reader.GetString(12),
                        s_prodmanifestado = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        v_tara = Convert.ToDouble(_reader.GetInt32(14)),
                        b_reef = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        b_ret_dir = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        c_imo_imd = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_un_number = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        b_transhipment = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        c_condicion = _reader.IsDBNull(20) ? "" : _reader.GetString(20)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerAutorizadosEx(int pId, DBComun.Estado pTipo)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, IdReg, n_BL, n_contenedor, c_tamaño, v_peso, b_estado, s_consignatario, n_sello, c_pais_destino, c_pais_origen, c_detalle_pais, s_comodity, s_prodmanifestado, v_tara, ISNULL(b_reef, '') b_reef, ISNULL(b_emb_dir, '') b_emb_dir
                                    FROM CCO_DETA_EXPO_NAVI 
                                    WHERE b_autorizado = 1 AND IdReg = {0} AND b_cancelado = 0 AND c_correlativo IS NULL";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(5)),
                        b_estado = _reader.GetString(6),
                        s_consignatario = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        n_sello = _reader.GetString(8),
                        c_pais_destino = _reader.GetString(9),
                        c_pais_origen = _reader.GetString(10),
                        c_detalle_pais = _reader.GetString(11),
                        s_comodity = _reader.GetString(12),
                        s_prodmanifestado = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        v_tara = Convert.ToDouble(_reader.GetInt32(14)),
                        b_reef = _reader.IsDBNull(15) ? "" : _reader.GetString(15).Trim().TrimEnd().TrimStart().ToUpper(),
                        b_emb_dir = _reader.IsDBNull(16) ? "" : _reader.GetString(16).Trim().TrimEnd().TrimStart().ToUpper()
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActualizarCorrelativo(DBComun.Estado pEstado, int pCorrelativo, int pId)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET c_correlativo = {0}, f_autorizacion = GETDATE()  
									WHERE IdDeta = {1} AND b_autorizado = 1; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCorrelativo, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarCorrelativoEx(DBComun.Estado pEstado, int pCorrelativo, int pId)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_EXPO_NAVI 
                                    SET c_correlativo = {0}, f_autorizacion = GETDATE()  
                                    WHERE IdDeta = {1} AND b_autorizado = 1; 
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pCorrelativo, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }


        public static List<ArchivoAduana> ObtenerResultado(int pId, int pInferior, int pSuperior, DBComun.Estado pTipo)
        {
            List<ArchivoAduana> notiLista = new List<ArchivoAduana>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_MATRIZ_LISTADOS", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));
                _command.Parameters.Add(new SqlParameter("@pInferior", pInferior));
                _command.Parameters.Add(new SqlParameter("@pSuperior", pSuperior));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoAduana _notificacion = new ArchivoAduana
                    {
                        c_correlativo = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1),
                        n_pais_destino = _reader.GetString(2),
                        b_estado_c = _reader.GetString(3),
                        c_tamaño_c = _reader.GetString(4),
                        v_peso = _reader.IsDBNull(5) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(5)),
                        b_condicion = _reader.GetString(6),
                        s_consignatario = _reader.GetString(7),
                        v_tara = _reader.GetInt32(8),
                        n_pais_origen = _reader.GetString(9),
                        b_estado = _reader.GetString(10),
                        c_tamaño = _reader.GetString(11),
                        b_reef = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        b_ret_dir = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        b_tranship = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        c_imo_imd = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        c_un_number = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        c_imo = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_voyage = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        n_BL = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        n_sello = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                        s_comodity = _reader.IsDBNull(21) ? "" : _reader.GetString(21),
                        s_promanifiesto = _reader.IsDBNull(22) ? "" : _reader.GetString(22),
                        num_manif = _reader.IsDBNull(23) ? 0 : Convert.ToInt32(_reader.GetInt32(23)),
                        c_pais_destino = _reader.IsDBNull(27) ? "" : _reader.GetString(27),
                        c_pais_origen = _reader.IsDBNull(28) ? "" : _reader.GetString(28),
                        b_transferencia = _reader.IsDBNull(31) ? "" : _reader.GetString(31).Trim().TrimEnd().TrimStart(),
                        b_manejo = _reader.IsDBNull(32) ? "" : _reader.GetString(32).Trim().TrimEnd().TrimStart(),
                        b_despacho = _reader.IsDBNull(33) ? "" : _reader.GetString(33).Trim().TrimEnd().TrimStart(),
                        c_condicion = _reader.IsDBNull(34) ? "" : _reader.GetString(34),
                        b_req_tarja = _reader.IsDBNull(35) ? "" : _reader.GetString(35)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static List<ArchivoAduana> getCotecnaLst(int pId, int pIdDoc, DBComun.Estado pTipo)
        {
            List<ArchivoAduana> notiLista = new List<ArchivoAduana>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_cotecna", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));
                _command.Parameters.Add(new SqlParameter("@IdDoc", pIdDoc));



                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoAduana _notificacion = new ArchivoAduana
                    {
                        c_correlativo = _reader.GetInt32(0),
                        tipoDecla = _reader.GetString(1),
                        a_manifiesto = _reader.GetString(2),
                        s_consignatario = _reader.GetString(3),
                        n_BL = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        s_comodity = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_paquete = _reader.GetInt32(6),
                        c_embalaje = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        v_peso = _reader.IsDBNull(8) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(8)),
                        n_contenedor = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        d_cliente = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        c_pais_origen = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        c_pais_destino = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        c_condicion = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        c_tipo_bl = _reader.IsDBNull(14) ? "" : _reader.GetString(14)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static string validSize(string n_contenedor, string c_tamaño, DBComun.Estado pTipo)
        {
            

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_valid_size", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_tamaño", c_tamaño));

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return _reader;

                
            
            }

        }

        public static List<ArchivoAduana> getCotecnaLstWeb(string a_mani, int n_manifiesto, DBComun.Estado pTipo)
        {
            List<ArchivoAduana> notiLista = new List<ArchivoAduana>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_cotecna_web", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_mani));
                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));



                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoAduana _notificacion = new ArchivoAduana
                    {
                        c_correlativo = _reader.GetInt32(0),
                        tipoDecla = _reader.GetString(1),
                        a_manifiesto = _reader.GetString(2),
                        s_consignatario = _reader.GetString(3),
                        n_BL = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        s_comodity = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_paquete = _reader.GetInt32(6),
                        c_embalaje = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        v_peso = _reader.IsDBNull(8) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(8)),
                        n_contenedor = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        d_cliente = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        c_pais_origen = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        c_pais_destino = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        c_condicion = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        c_tipo_bl = _reader.IsDBNull(14) ? "" : _reader.GetString(14)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }


        public static List<ArchivoAduana> getOpeList(int pId, DBComun.Estado pTipo)
        {
            List<ArchivoAduana> notiLista = new List<ArchivoAduana>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_MATRIZ_LISTADOS_WEB", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));



                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoAduana _notificacion = new ArchivoAduana
                    {
                        c_correlativo = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1),
                        n_pais_destino = _reader.GetString(2),
                        b_estado_c = _reader.GetString(3),
                        c_tamaño_c = _reader.GetString(4),
                        v_peso = _reader.IsDBNull(5) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(5)),
                        b_condicion = _reader.GetString(6),
                        s_consignatario = _reader.GetString(7),
                        v_tara = _reader.GetInt32(8),
                        n_pais_origen = _reader.GetString(9),
                        b_estado = _reader.GetString(10),
                        c_tamaño = _reader.GetString(11),
                        b_reef = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        b_ret_dir = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        b_tranship = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        c_imo_imd = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        c_un_number = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        c_imo = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_voyage = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        n_BL = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        n_sello = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                        s_comodity = _reader.IsDBNull(21) ? "" : _reader.GetString(21),
                        s_promanifiesto = _reader.IsDBNull(22) ? "" : _reader.GetString(22),
                        num_manif = _reader.IsDBNull(23) ? 0 : Convert.ToInt32(_reader.GetInt32(23)),
                        c_pais_destino = _reader.IsDBNull(27) ? "" : _reader.GetString(27),
                        c_pais_origen = _reader.IsDBNull(28) ? "" : _reader.GetString(28),
                        b_transferencia = _reader.IsDBNull(31) ? "" : _reader.GetString(31).Trim().TrimEnd().TrimStart(),
                        b_manejo = _reader.IsDBNull(32) ? "" : _reader.GetString(32).Trim().TrimEnd().TrimStart(),
                        b_despacho = _reader.IsDBNull(33) ? "" : _reader.GetString(33).Trim().TrimEnd().TrimStart(),
                        c_condicion = _reader.IsDBNull(34) ? "" : _reader.GetString(34),
                        b_req_tarja = _reader.IsDBNull(35) ? "" : _reader.GetString(35)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static List<ArchivoAduana> getBL_SADFI(int pId, int pInferior, int pSuperior, DBComun.Estado pTipo)
        {
            List<ArchivoAduana> notiLista = new List<ArchivoAduana>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_MATRIZ_SADFI", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));
                _command.Parameters.Add(new SqlParameter("@pInferior", pInferior));
                _command.Parameters.Add(new SqlParameter("@pSuperior", pSuperior));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoAduana _notificacion = new ArchivoAduana
                    {
                        c_correlativo = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1),
                        n_pais_destino = _reader.GetString(2),
                        b_estado_c = _reader.GetString(3),
                        c_tamaño_c = _reader.GetString(4),
                        v_peso = _reader.IsDBNull(5) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(5)),
                        b_condicion = _reader.GetString(6),
                        s_consignatario = _reader.GetString(7),
                        v_tara = _reader.GetInt32(8),
                        n_pais_origen = _reader.GetString(9),
                        b_estado = _reader.GetString(10),
                        c_tamaño = _reader.GetString(11),
                        b_reef = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        b_ret_dir = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        b_tranship = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        c_imo_imd = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        c_un_number = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        c_imo = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_voyage = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        n_BL = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        n_sello = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                        s_comodity = _reader.IsDBNull(21) ? "" : _reader.GetString(21),
                        s_promanifiesto = _reader.IsDBNull(22) ? "" : _reader.GetString(22),
                        num_manif = _reader.IsDBNull(23) ? 0 : Convert.ToInt32(_reader.GetInt32(23)),
                        c_pais_destino = _reader.IsDBNull(27) ? "" : _reader.GetString(27),
                        c_pais_origen = _reader.IsDBNull(28) ? "" : _reader.GetString(28),
                        b_transferencia = _reader.IsDBNull(31) ? "" : _reader.GetString(31).Trim().TrimEnd().TrimStart(),
                        b_manejo = _reader.IsDBNull(32) ? "" : _reader.GetString(32).Trim().TrimEnd().TrimStart(),
                        b_despacho = _reader.IsDBNull(33) ? "" : _reader.GetString(33).Trim().TrimEnd().TrimStart(),
                        c_condicion = _reader.IsDBNull(34) ? "" : _reader.GetString(34),
                        b_req_tarja = _reader.IsDBNull(35) ? "" : _reader.GetString(35),
                        a_manifiesto = _reader.IsDBNull(36) ? "" : _reader.GetString(36)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }


        public static List<ArchivoExport> ObtenerResultadoEx(int pId, int pInferior, int pSuperior, DBComun.Estado pTipo)
        {
            List<ArchivoExport> notiLista = new List<ArchivoExport>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT c_correlativo, n_contenedor, UPPER(b.CountryName) PaisDestino, CASE WHEN r.b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estadoc, 
                                    CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'R' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, ISNULL(ROUND(v_peso, 2, 0), 0.00) v_peso, s_consignatario, 
                                    v_tara, UPPER(f.CountryName) PaisOrigen, r.b_estado, c_tamaño, CASE WHEN b_reef = 'Y' THEN 'SI' ELSE CASE WHEN b_reef = 'N' THEN 'NO' ELSE '' END END b_reef, CASE WHEN b_emb_dir = 'Y' THEN 'SI' ELSE CASE WHEN b_emb_dir = 'N' THEN 'NO' ELSE '' END END b_emb_dirc, h.c_imo, c_voyage, n_BL, n_sello, s_comodity, s_prodmanifestado, 
                                    CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'U' THEN 1 ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'T' THEN 2 ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'P' THEN 3 ELSE  CASE WHEN e.d_tipo = 'HC' THEN 4 ELSE 0 END END END END c_tipos,
                                    CASE WHEN LEFT(d.d_descripcion, 2) = '40' OR  LEFT(d.d_descripcion, 2) = '45' THEN 2 ELSE CASE WHEN  LEFT(d.d_descripcion, 2) = '20' THEN 1 END END c_longitud,
                                    CASE WHEN LEFT(d.d_descripcion, 2) = '40' THEN 0 ElSE CASE WHEN LEFT(d.d_descripcion, 2) = '45' THEN 45 ELSE 0 END END c_otro, c_pais_destino, c_pais_origen, b_autorizado, c_detalle_pais, ISNULL(s_transferencia, '') s_transferencia, ISNULL(s_manejo, '') s_manejo, ISNULL(s_recepcion, '') s_recepcion
                                    FROM CCO_DETA_EXPO_NAVI r  INNER JOIN CCO_COD_PAISES b ON c_pais_destino = b.CountryCode
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_COD_PAISES f ON SUBSTRING(c_pais_origen, 1, 2) = f.CountryCode
                                    INNER JOIN CCO_ENCA_EXPO_NAVI h ON h.IdReg = r.IdReg
                                    INNER JOIN CCO_DETA_DOC_EXP_NAVI z ON h.IdReg = z.IdReg AND r.IdDoc = z.IdDoc
                                    WHERE b_autorizado = 1 AND r.IdReg = {0} AND b_cancelado = 0   AND c_correlativo BETWEEN {1} AND {2}
                                    ORDER BY c_correlativo  ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pInferior, pSuperior), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoExport _notificacion = new ArchivoExport
                    {
                        c_correlativo = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1),
                        d_pais_destino = _reader.GetString(2),
                        b_estado_c = _reader.GetString(3),
                        c_tamaño_c = _reader.GetString(4),
                        v_peso = _reader.IsDBNull(5) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(5)),
                        s_consignatario = _reader.GetString(6),
                        s_notificador = _reader.GetString(7),
                        s_exportador = _reader.GetString(8),
                        v_tara = _reader.GetInt32(9),
                        d_pais_origen = _reader.GetString(10),
                        b_estado = _reader.GetString(11),
                        c_tamaño = _reader.GetString(12),
                        b_reef = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        b_emb_dir = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        c_imo = _reader.IsDBNull(15) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(15)),
                        c_voyage = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        n_BL = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        n_sello = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        s_comodity = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        s_prodmanifestado = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                        c_pais_destino = _reader.IsDBNull(24) ? "" : _reader.GetString(24),
                        c_pais_origen = _reader.IsDBNull(25) ? "" : _reader.GetString(25),
                        c_detalle_puerto = _reader.IsDBNull(27) ? "" : _reader.GetString(27),
                        b_transferencia = _reader.IsDBNull(28) ? "" : _reader.GetString(28).Trim().TrimEnd().TrimStart(),
                        b_manejo = _reader.IsDBNull(29) ? "" : _reader.GetString(29).Trim().TrimEnd().TrimStart(),
                        b_recepcion = _reader.IsDBNull(30) ? "" : _reader.GetString(30).Trim().TrimEnd().TrimStart()
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerAEnviar(int pId, int pInferior, int pSuperior)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, IdReg, n_BL, n_contenedor, c_tamaño, v_peso, b_estado, s_consignatario, n_sello, c_pais_destino, c_pais_origen, c_detalle_pais, s_comodity, s_prodmanifestado, v_tara, b_reef, b_ret_dir, c_imo_imd, c_un_number, b_transhipment, c_condicion
                                    FROM CCO_DETA_NAVIERAS 
                                    WHERE IdReg = {0} AND b_autorizado = 1 AND c_correlativo BETWEEN {1} AND {2}  ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pInferior, pSuperior), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(5)),
                        b_estado = _reader.GetString(6),
                        s_consignatario = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        n_sello = _reader.GetString(8),
                        c_pais_destino = _reader.GetString(9),
                        c_pais_origen = _reader.GetString(10),
                        c_detalle_pais = _reader.GetString(11),
                        s_comodity = _reader.GetString(12),
                        s_prodmanifestado = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        v_tara = Convert.ToDouble(_reader.GetInt32(14)),
                        b_reef = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        b_ret_dir = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        c_imo_imd = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_un_number = _reader.IsDBNull(18) ? "" : _reader.GetString(18),
                        b_transhipment = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        c_condicion = _reader.IsDBNull(20) ? "" : _reader.GetString(20)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerCancel(int pId, int pDeta)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, IdReg, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, s_observaciones  
                                    FROM CCO_DETA_NAVIERAS 
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    WHERE IdReg = {0} AND b_cancelado = 1 AND IdDeta = {1}  ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId, pDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        s_observaciones = _reader.GetString(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerDetalleDAN(int pId, string pTipo)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_DAN_RETENCIONES", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", pId));
                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        c_correlativo = _reader.GetInt32(5),
                        c_pais_origen = _reader.GetString(6),
                        b_rdt = _reader.GetString(7),
                        s_consignatario = _reader.GetString(8),
                        b_bloqueo = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        f_salidas = _reader.IsDBNull(10) ? "" : _reader.GetString(10)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }



        public static List<DetaNaviera> ObtenerDetalleDANL(string pTipo, int pYear)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_DAN_LIBERACIONES", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo));
                _command.Parameters.Add(new SqlParameter("@year", pYear));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        n_folio = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        f_retenidoc = Convert.ToString(_reader.GetString(6)),
                        f_recep_patioc = Convert.ToString(_reader.GetString(7)),
                        c_cliente = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        c_manifiesto = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        c_navi = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        c_tipo_doc = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        c_llegada = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        f_llegada = _reader.GetDateTime(13),
                        f_tramite_s = Convert.ToString(_reader.GetString(14)),
                        b_estado = _reader.IsDBNull(15) ? "" : _reader.GetString(15)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActualizarDAN(DBComun.Estado pEstado, int pId, string ClaveRe, string n_folio, string pUser, int b_escaner)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_detenido = 1, n_folio = '{0}', f_reg_dan = GETDATE(), ClaveRe = '{1}', us_detenido = '{2}', b_escaner = {3} 
									WHERE IdDeta = {4} AND b_autorizado = 1 AND b_cancelado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_folio, ClaveRe, pUser, b_escaner, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarUCC(DBComun.Estado pEstado, int pId, string ClaveRe, string n_folio, string pUser, int b_escaner)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_ucc = 1, n_ofiucc = '{0}', f_retencion_ucc = GETDATE(), ClaveReUCC = '{1}', us_det_ucc = '{2}', b_escaner_ucc = {3} 
									WHERE IdDeta = {4} AND b_autorizado = 1 AND b_cancelado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_folio, ClaveRe, pUser, b_escaner, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarDANL(DBComun.Estado pEstado, int pId, string pUser, string f_revision, int IdRevision, int IdDetalle, string s_marchamo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();



                string consulta = @"SET DATEFORMAT DMY
                                    UPDATE CCO_DETA_NAVIERAS 
									SET b_detenido = 0, f_reg_liberacion = GETDATE(), us_liberado = '{0}', f_ini_dan =  CAST('{1}' AS DATETIME), IdRevision = {2}, IdDetalle = {3} --, s_marchamo = '{4}'  
									WHERE IdDeta = {4} AND b_autorizado = 1 AND b_cancelado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pUser, f_revision, IdRevision, IdDetalle, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarDGAL(DBComun.Estado pEstado, int pId, string pUser)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();



                string consulta = @"SET DATEFORMAT DMY
                                    UPDATE CCO_DETA_NAVIERAS 
									SET b_dga = 0, f_lib_dga = GETDATE(), us_lib_dga = '{0}' --, s_marchamo = '{1}'  
									WHERE IdDeta = {1} AND b_autorizado = 1 AND b_cancelado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pUser, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarTransmiAut(DBComun.Estado pEstado, int pId, string pUser)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();



                string consulta = @"SET DATEFORMAT DMY
                                    UPDATE CCO_DETA_NAVIERAS 
									SET t_auto = 1  
									WHERE IdDeta = {1} AND b_autorizado = 1 AND b_cancelado = 0; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pUser, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarUCCL(DBComun.Estado pEstado, int pId, string pUser, string f_revision, int IdRevision, int IdDetalleUCC, string s_marchamo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                string consulta = @"SET DATEFORMAT DMY
                            UPDATE CCO_DETA_NAVIERAS 
                            SET b_ucc = 0, f_lib_ucc = GETDATE(), us_lib_ucc = '{0}', f_ini_ucc =  CAST('{1}' AS DATETIME), IdRevisionUCC = {2}, IdDetalleUCC = {3} --, s_marchamo = '{4}'  
							WHERE IdDeta = {4} AND b_autorizado = 1 AND b_cancelado = 0;                              
                            SELECT @@ROWCOUNT";


                SqlCommand _command = new SqlCommand(string.Format(consulta, pUser, f_revision, IdRevision, IdDetalleUCC, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }
        public static string AlmacenarValidMst(ArchivoAduanaValid _Encanaviera, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_valid_aduana_master", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", _Encanaviera.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_BL", _Encanaviera.n_BL));
                _command.Parameters.Add(new SqlParameter("@a_mani", _Encanaviera.a_mani));
                _command.Parameters.Add(new SqlParameter("@c_tipo_bl", _Encanaviera.c_tipo_bl));
                _command.Parameters.Add(new SqlParameter("@b_sidunea", _Encanaviera.b_sidunea));
                _command.Parameters.Add(new SqlParameter("@c_tamaño", _Encanaviera.c_tamaño));
                _command.Parameters.Add(new SqlParameter("@v_peso", _Encanaviera.v_peso));
                _command.Parameters.Add(new SqlParameter("@c_pais_destino", _Encanaviera.c_pais_destino));
                _command.Parameters.Add(new SqlParameter("@c_pais_origen", _Encanaviera.c_pais_origen));
                _command.Parameters.Add(new SqlParameter("@d_puerto_origen", _Encanaviera.d_puerto_origen));
                _command.Parameters.Add(new SqlParameter("@d_puerto_destino", _Encanaviera.d_puerto_destino));
                _command.Parameters.Add(new SqlParameter("@s_agencia", _Encanaviera.s_agencia));
                _command.Parameters.Add(new SqlParameter("@c_paquete", _Encanaviera.c_paquete));
                _command.Parameters.Add(new SqlParameter("@c_embalaje", _Encanaviera.c_embalaje));
                _command.Parameters.Add(new SqlParameter("@d_embalaje", _Encanaviera.d_embalaje));
                _command.Parameters.Add(new SqlParameter("@s_nit", _Encanaviera.s_nit));
                _command.Parameters.Add(new SqlParameter("@s_consignatario", _Encanaviera.s_consignatario));
                _command.Parameters.Add(new SqlParameter("@n_BL_master", _Encanaviera.n_BL_master));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string AlmacenarValid(ArchivoAduanaValid _Encanaviera, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_valid_aduana", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", _Encanaviera.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_BL", _Encanaviera.n_BL));
                _command.Parameters.Add(new SqlParameter("@a_mani", _Encanaviera.a_mani));
                _command.Parameters.Add(new SqlParameter("@c_tipo_bl", _Encanaviera.c_tipo_bl));
                _command.Parameters.Add(new SqlParameter("@b_sidunea", _Encanaviera.b_sidunea));
                _command.Parameters.Add(new SqlParameter("@c_tamaño", _Encanaviera.c_tamaño));
                _command.Parameters.Add(new SqlParameter("@v_peso", _Encanaviera.v_peso));
                _command.Parameters.Add(new SqlParameter("@c_pais_destino", _Encanaviera.c_pais_destino));
                _command.Parameters.Add(new SqlParameter("@c_pais_origen", _Encanaviera.c_pais_origen));
                _command.Parameters.Add(new SqlParameter("@d_puerto_origen", _Encanaviera.d_puerto_origen));
                _command.Parameters.Add(new SqlParameter("@d_puerto_destino", _Encanaviera.d_puerto_destino));
                _command.Parameters.Add(new SqlParameter("@s_agencia", _Encanaviera.s_agencia));
                _command.Parameters.Add(new SqlParameter("@c_paquete", _Encanaviera.c_paquete));
                _command.Parameters.Add(new SqlParameter("@c_embalaje", _Encanaviera.c_embalaje));
                _command.Parameters.Add(new SqlParameter("@d_embalaje", _Encanaviera.d_embalaje));
                _command.Parameters.Add(new SqlParameter("@s_nit", _Encanaviera.s_nit));
                _command.Parameters.Add(new SqlParameter("@s_consignatario", _Encanaviera.s_consignatario));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string delMani(ArchivoAduanaValid _Encanaviera, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_del_manifiesto", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@a_mani", _Encanaviera.a_mani));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string AlmacenarValidEx(ArchivoAduanaValid _Encanaviera, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_valid_aduana_export", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                _command.Parameters.Add(new SqlParameter("@n_contenedor", _Encanaviera.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@IdReg", _Encanaviera.IdReg));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }



        public static string AlmacenarInco(IncoAduana _Encanaviera, DBComun.Estado pTipo)
        {
            string resultado = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                IDbTransaction tranValid = _conn.BeginTransaction();

                SqlCommand _command = new SqlCommand("pa_valid_inco", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdReg", _Encanaviera.IdReg));
                _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@a_manif", _Encanaviera.a_mani));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", _Encanaviera.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_naviera", _Encanaviera.c_naviera));

                _command.Transaction = (SqlTransaction)tranValid;

                try
                {
                    resultado = _command.ExecuteScalar().ToString();
                    tranValid.Commit();
                }
                catch (Exception ex)
                {
                    tranValid.Rollback();
                    throw new Exception("Problemas insertando inconsistencias " + ex.Message);
                }
                finally
                {
                    _conn.Close();
                }
                return resultado;
            }
        }

        public static List<DetaNaviera> ObtenerRegAnter(string c_imo, string c_llegada, string c_voyage)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, n_contenedor FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON a.IdReg = b.IdReg
                                    WHERE  c_imo = '{0}' AND c_llegada = '{1}' /*AND c_naviera = '{2}'*/ AND c_voyage = '{2}'";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_imo, c_llegada, c_voyage), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerRegAnterEx(string c_imo, string c_llegada, string c_voyage)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdDeta, n_contenedor FROM CCO_ENCA_EXPO_NAVI a INNER JOIN CCO_DETA_EXPO_NAVI b ON a.IdReg = b.IdReg
                                    WHERE  c_imo = '{0}' AND c_llegada = '{1}' /*AND c_naviera = '{2}'*/ AND c_voyage = '{2}'";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_imo, c_llegada, c_voyage), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EnvioAuto> ObtenerDocAu(int pIdDoc, DBComun.Estado pEstado)
        {
            List<EnvioAuto> notiLista = new List<EnvioAuto>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT  MAX(a.c_navi_corto) c_navi_corto, b.n_manifiesto, c.c_voyage 
                                    FROM cco_usuarios a INNER JOIN cco_enca_navieras c on a.c_naviera = c.c_naviera
                                    INNER JOIN cco_deta_doc_navi b on b.Idreg = c.IdReg
                                    WHERE IdDoc = {0}
                                    GROUP BY b.n_manifiesto, c.c_voyage";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pIdDoc), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EnvioAuto _notificacion = new EnvioAuto
                    {
                        c_naviera_corto = _reader.GetString(0),
                        n_manifiesto = _reader.GetInt32(1),
                        c_voyaje = _reader.GetString(2)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EnvioAuto> ObtenerEncAu(int pIdReg, DBComun.Estado pEstado)
        {
            List<EnvioAuto> notiLista = new List<EnvioAuto>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT  MAX(a.c_navi_corto) c_navi_corto, c.c_voyage, c.IdReg, STUFF((
                                  SELECT ',' + CONVERT(VARCHAR(4), md.n_manifiesto)
                                  FROM cco_deta_doc_navi md
                                  WHERE md.IdReg = c.IdReg
                                  FOR XML PATH('')), 1, 1, '') n_manifiesto
                                    FROM cco_usuarios a INNER JOIN cco_enca_navieras c on a.c_naviera = c.c_naviera
                                    INNER JOIN cco_deta_doc_navi b on b.Idreg = c.IdReg
                                    WHERE c.IdReg = {0} and b.b_estado = 1 and b.f_valid is not null
                                    GROUP BY c.c_voyage, c.IdReg ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pIdReg), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EnvioAuto _notificacion = new EnvioAuto
                    {
                        c_naviera_corto = _reader.GetString(0),
                        c_voyaje = _reader.GetString(1),
                        IdReg = _reader.GetInt32(2),
                        an_manifiesto = _reader.GetString(3)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EnvioAuto> ObtenerDocAuEx(int pIdReg, DBComun.Estado pEstado)
        {
            List<EnvioAuto> notiLista = new List<EnvioAuto>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT  MAX(a.c_navi_corto) c_navi_corto, c.c_voyage 
                                    FROM cco_usuarios a INNER JOIN CCO_ENCA_EXPO_NAVI c on a.c_naviera = c.c_naviera
                                    INNER JOIN CCO_DETA_DOC_EXP_NAVI b on b.Idreg = c.IdReg
                                    WHERE b.IdReg = 1 
                                    GROUP BY c.c_voyage";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pIdReg), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EnvioAuto _notificacion = new EnvioAuto
                    {
                        c_naviera_corto = _reader.GetString(0),
                        c_voyaje = _reader.GetString(1)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ObtenerBusqueda(string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string consulta = @"SELECT IdDeta, r.IdReg, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, 
                                    ISNULL(v_peso, 0.00) v_precio, CASE WHEN r.b_estado = 'F' THEN 'X' ELSE '' END b_estadoF, CASE WHEN r.b_estado = 'E' THEN 'X' ELSE '' END b_estadoV,  ISNULL(v_tara, 0.00) v_tara, CASE WHEN b_reef = 'Y' THEN 'X' ELSE '' END b_reef, c_correlativo, ISNULL(grupo, 0) grupo, ISNULL(grua, 0) grua, ISNULL(b_chasis, 0) b_chasis,  ISNULL(b_rastra, 0) b_rastra
                                    FROM CCO_DETA_NAVIERAS r  
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_ENCA_NAVIERAS h ON h.IdReg = r.IdReg 
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON h.IdReg = z.IdReg AND r.IdDoc = z.IdDoc
                                    WHERE r.IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_llegada = '{0}') AND b_autorizado = 1 AND c_correlativo > 0 AND b_cancelado = 0 AND ISNULL(b_recepcion, 0) = 0 AND dbo.Contenedores(n_contenedor) = 'NULL'
                                    ORDER BY c_correlativo";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(4)),
                        b_estadoF = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        b_estadoV = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        v_tara = Convert.ToDouble(_reader.GetInt32(7)),
                        b_reef = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        c_correlativo = _reader.GetInt32(9)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<DetaNaviera> ObtenerSearch(string c_llegada, string prefix)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string consulta = @"SELECT IdDeta,n_contenedor
                                    FROM CCO_DETA_NAVIERAS r  
                                    INNER JOIN CCO_ENCA_NAVIERAS h ON h.IdReg = r.IdReg 
                                    WHERE n_contenedor LIKE '%{0}' AND
                                    r.IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_llegada = '{1}') AND b_autorizado = 1 AND c_correlativo > 0 AND b_cancelado = 0 AND ISNULL(b_recepcion, 0) = 0 AND dbo.Contenedores(n_contenedor) = 'NULL'
                                    ORDER BY c_correlativo";

                SqlCommand _command = new SqlCommand(string.Format(consulta, prefix, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }


        public static string ValidarNumero(string n_contenedor)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"DECLARE @valor VARCHAR(50)
                                    SET @valor = (SELECT dbo.Contenedores('{0}'))

                                    IF @valor = 'NULL'
	                                    SELECT 1
                                    ELSE
	                                    SELECT 0";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_contenedor), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static List<DetaNaviera> ObtenerBusqueda(string c_llegada, string n_contenedor)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string consulta = @"SELECT IdDeta, r.IdReg, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, 
                                    ISNULL(v_peso, 0.00) v_peso, CASE WHEN r.b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estado,  ISNULL(v_tara, 0.00) v_tara, CASE WHEN b_reef = 'Y' THEN 'X' ELSE '' END b_reef, b_ret_dir, c_correlativo, n_sello, ISNULL(grupo, 0) grupo, ISNULL(grua, 0) grua, ISNULL(b_chasis, 0) b_chasis,  ISNULL(b_rastra, 0) b_rastra,  CASE WHEN ISNULL(b_recepcion, 0) = 1 THEN 'SI' ELSE 'NO' END b_recepcion
                                    FROM CCO_DETA_NAVIERAS r  
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_ENCA_NAVIERAS h ON h.IdReg = r.IdReg 
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON h.IdReg = z.IdReg AND r.IdDoc = z.IdDoc
                                    WHERE r.IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_llegada = '{0}') AND r.n_contenedor = '{1}' AND b_autorizado = 1 AND c_correlativo > 0 AND b_cancelado = 0 --AND ISNULL(b_recepcion, 0) = 0
                                    ORDER BY c_correlativo";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada, n_contenedor), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(4)),
                        b_estadoF = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        v_tara = Convert.ToDouble(_reader.GetInt32(6)),
                        b_reef = _reader.IsDBNull(8) ? "" : _reader.GetString(7),
                        b_ret_dir = _reader.IsDBNull(9) ? "" : _reader.GetString(8).ToString().TrimEnd().TrimStart(),
                        c_correlativo = _reader.GetInt32(9),
                        n_sello = _reader.IsDBNull(9) ? "/" : _reader.GetString(10),
                        b_recepcionc = _reader.IsDBNull(15) ? "" : _reader.GetString(15)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static string ActualizarRecepcion(DetaNaviera _deta)
        {
            int b_chasis = 0, b_rastra = 0;

            if (_deta.b_chasis == true)
            {
                b_chasis = 1;
            }

            if (_deta.b_rastra == true)
            {
                b_rastra = 1;
            }

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {


                _conn.Open();
                string consulta = @"UPDATE CCO_DETA_NAVIERAS
                                    SET grupo = '{0}', grua = '{1}', c_marcacion = '{2}', b_chasis = {3}, b_rastra = {4}, v_tara = {5}, b_recepcion = 1, f_recepcion = getdate()
                                    WHERE IdReg = {6} AND c_correlativo = {7} AND n_contenedor = '{8}'
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, _deta.grupo, _deta.grua, _deta.c_marcacion, b_chasis,
                       b_rastra, _deta.v_tara, _deta.IdReg, _deta.c_correlativo, _deta.n_contenedor), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string ValidaSize(string pSize)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_recepcion", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_tamaño", pSize));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaNaviera> ObtenerTop(string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                /*string consulta = @"SELECT TOP 5 IdDeta, r.IdReg, n_contenedor, r.f_recepcion
                                    FROM CCO_DETA_NAVIERAS r  
                                    WHERE r.IdReg = {0} AND b_autorizado = 1 AND c_correlativo > 0 AND b_cancelado = 0 AND ISNULL(b_recepcion, 0) = 1
                                    ORDER BY r.f_recepcion DESC";*/

                string consulta = @"SELECT TOP 5 IdDeta, r.IdReg, n_contenedor, r.f_recepcion
                                    FROM CCO_DETA_NAVIERAS r  
                                    WHERE r.IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_llegada = '{0}') AND b_autorizado = 1 AND c_correlativo > 0 AND b_cancelado = 0 AND ISNULL(b_recepcion, 0) = 1
                                    ORDER BY r.f_recepcion DESC";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        f_recepcion = Convert.ToDateTime(_reader.GetDateTime(3))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<DetaNaviera> ObtenerRecibidos(string c_llegada, string grupo, string grua)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string consulta = @"SELECT IdDeta, r.IdReg, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, 
                                    CASE WHEN r.b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estadoF,  ISNULL(v_tara, 0.00) v_tara, CASE WHEN b_reef = 'Y' THEN 'X' ELSE '' END b_reef, c_correlativo, ISNULL(grupo, 0) grupo, ISNULL(grua, 0) grua, CASE WHEN ISNULL(b_chasis, 0) = 1 THEN 'SI' ELSE 'NO' END  b_chasis,  CASE WHEN ISNULL(b_rastra, 0) = 1 THEN 'SI' ELSE 'NO' END b_rastra, r.f_recepcion
                                    FROM CCO_DETA_NAVIERAS r  
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_ENCA_NAVIERAS h ON h.IdReg = r.IdReg 
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON h.IdReg = z.IdReg AND r.IdDoc = z.IdDoc
                                    WHERE r.IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_llegada = '{0}') AND b_autorizado = 1 AND grupo = '{1}' AND grua = '{2}' AND c_correlativo > 0 AND b_cancelado = 0 AND ISNULL(b_recepcion, 0) = 1 AND dbo.Contenedores(n_contenedor) = 'NULL'
                                    ORDER BY r.f_recepcion DESC";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada, grupo, grua), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        b_estadoF = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        v_tara = Convert.ToDouble(_reader.GetInt32(5)),
                        b_reef = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_correlativo = _reader.GetInt32(7),
                        grupo = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        grua = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_chasisc = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_rastrac = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        f_recepcion = Convert.ToDateTime(_reader.GetDateTime(12))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<DetaNaviera> ObtenerRecibidos(string c_llegada, string n_contenedor)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string consulta = @"SELECT IdDeta, r.IdReg, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, 
                                    CASE WHEN r.b_estado = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estadoF,  ISNULL(v_tara, 0.00) v_tara, CASE WHEN b_reef = 'Y' THEN 'X' ELSE '' END b_reef, c_correlativo, ISNULL(grupo, 0) grupo, ISNULL(grua, 0) grua, CASE WHEN ISNULL(b_chasis, 0) = 1 THEN 'SI' ELSE 'NO' END  b_chasis,  CASE WHEN ISNULL(b_rastra, 0) = 1 THEN 'SI' ELSE 'NO' END b_rastra, r.f_recepcion
                                    FROM CCO_DETA_NAVIERAS r  
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_ENCA_NAVIERAS h ON h.IdReg = r.IdReg 
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON h.IdReg = z.IdReg AND r.IdDoc = z.IdDoc
                                    WHERE r.IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_llegada = '{0}') AND b_autorizado = 1 AND c_correlativo > 0 AND b_cancelado = 0 AND ISNULL(b_recepcion, 0) = 1 AND dbo.Contenedores(n_contenedor) = 'NULL' AND r.n_contenedor = '{1}'
                                    ORDER BY r.f_recepcion DESC";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada, n_contenedor), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        IdReg = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        b_estadoF = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        v_tara = Convert.ToDouble(_reader.GetInt32(5)),
                        b_reef = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_correlativo = _reader.GetInt32(7),
                        grupo = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        grua = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_chasisc = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_rastrac = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        f_recepcion = Convert.ToDateTime(_reader.GetDateTime(12))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<DetaNaviera> ObtenerRecepcion(DBComun.Estado pEstado, int? pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                string consulta = @"SELECT IdReg, IdDeta, a.c_correlativo, n_contenedor, f_recepcion, grua, grupo, a.c_marcacion, b.p_nombre + ' ' + ISNULL(b.s_nombre, '') + ', ' + b.p_ape + ' ' + ISNULL(b.s_ape, '') NombreC, 
                                    c.c_operadora + ' - ' + d.d_operadora Operadora
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CEPA_CONTRATISTAS.dbo.CC_TRABAJADORES b ON a.c_marcacion = b.c_marcacion
                                    INNER JOIN CEPA_CONTRATISTAS.dbo.CC_OPERACIONES c ON a.c_marcacion = c.c_marcacion
                                    INNER JOIN CEPA_CONTRATISTAS.dbo.CC_CONTRATISTAS d ON c.c_operadora = d.c_operadora
                                    WHERE IdReg = {0} AND b_recepcion = 1 AND b_envio = 0 AND c.b_estado = 1
                                    ORDER BY f_recepcion";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        c_correlativo = _reader.GetInt32(2),
                        n_contenedor = _reader.GetString(3),
                        f_recepcion = Convert.ToDateTime(_reader.GetDateTime(4)),
                        grua = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        grupo = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_marcacion = _reader.GetInt32(7),
                        s_nombre = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        s_operadora = _reader.IsDBNull(9) ? "" : _reader.GetString(9)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static string ActualizarIdRece(DBComun.Estado pEstado, int IdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_actualizar_recepcion", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdDeta", IdDeta));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaDAN> ObtenerOficio(string n_oficio, int a_folio)
        {
            List<DetaDAN> notiLista = new List<DetaDAN>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_OBTENER_OFICIO", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_folio", n_oficio));
                _command.Parameters.Add(new SqlParameter("@a_folio", a_folio));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDAN _notificacion = new DetaDAN
                    {
                        Clave = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        b_escaner = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_correlativo = _reader.GetInt32(2),
                        n_contenedor = _reader.GetString(3),
                        c_consignatario = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_pais_origen = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        f_dan = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        n_oficio = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_naviera = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        c_viaje = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        c_llegada = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        c_imo = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        jefe_almacen = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        sub_inspector = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        ClaveP = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        c_prefijo = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        Total = _reader.GetInt32(16),
                        Cantidad = _reader.GetInt32(17)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }
        public static List<DetaDAN> ObtenerOficioUCC(string n_oficio, int a_folio)
        {
            List<DetaDAN> notiLista = new List<DetaDAN>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_OBTENER_OFICIO_UCC", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_folio", n_oficio));
                _command.Parameters.Add(new SqlParameter("@a_folio", a_folio));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDAN _notificacion = new DetaDAN
                    {
                        Clave = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        b_escaner = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_correlativo = _reader.GetInt32(2),
                        n_contenedor = _reader.GetString(3),
                        c_consignatario = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_pais_origen = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        f_dan = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        n_oficio = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_naviera = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        c_viaje = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        c_llegada = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        c_imo = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        jefe_almacen = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        sub_inspector = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        ClaveP = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        c_prefijo = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        Total = _reader.GetInt32(16),
                        Cantidad = _reader.GetInt32(17)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<Facturacion> ObtenerTarja()
        {
            List<Facturacion> notiLista = new List<Facturacion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string consulta = @"SELECT DISTINCT a.c_correlativo, a.n_contenedor, a.v_peso, a.s_consignatario, a.n_BL, c.c_llegada
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS c ON a.IdReg = c.IdReg 
                                    WHERE c.c_llegada = '4.6833' AND c.c_naviera = '884' 
                                    ORDER BY 1";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Facturacion _notificacion = new Facturacion
                    {
                        c_correlativo = _reader.GetInt32(0),
                        n_contenedor = _reader.GetString(1),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(2)),
                        s_consignatario = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        n_BL = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_llegada = _reader.IsDBNull(5) ? "" : _reader.GetString(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<DetaNaviera> ObNavi(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT c_naviera FROM CCO_ENCA_NAVIERAS
                                    WHERE IdReg IN(SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE IdDeta = {0}) ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        c_navi = _reader.IsDBNull(0) ? "" : _reader.GetString(0)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ConsultarDAN(string n_contenedor, string c_naviera)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_DAN_CONSULTA_T", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        n_contenedor = _reader.GetString(0),
                        c_tamaño = _reader.GetString(1),
                        n_folio = _reader.IsDBNull(2) ? "SIN" : _reader.GetString(2),
                        f_recep_patio = _reader.GetDateTime(3),
                        f_retenido = _reader.GetDateTime(4),
                        f_tramite = Convert.ToDateTime(_reader.GetDateTime(5)),
                        f_liberado = Convert.ToDateTime(_reader.GetDateTime(6)),
                        b_estadoV = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_correlativo = _reader.IsDBNull(8) ? 0 : _reader.GetInt32(8),
                        CalcDiasD = _reader.IsDBNull(9) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(9)),
                        f_recepcion = Convert.ToDateTime(_reader.GetDateTime(10)),
                        b_estado = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        f_ini_dan = Convert.ToDateTime(_reader.GetDateTime(12)),
                        TipoRevision = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        b_retenido = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        f_cancelado = Convert.ToDateTime(_reader.GetDateTime(15)),
                        TipoRe = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        IdDeta = _reader.IsDBNull(17) ? 0 : _reader.GetInt32(17)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ConsultarDAN_Web(string n_contenedor, string a_manifiesto, int n_manifiesto)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_DAN_CONSULTA_WEB", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        //n_contenedor = _reader.GetString(0),
                        //c_tamaño = _reader.GetString(1),
                        //n_folio = _reader.IsDBNull(2) ? "SIN" : _reader.GetString(2),
                        //f_recep_patio = (DateTime)_reader.GetDateTime(3),
                        //f_retenido = (DateTime)_reader.GetDateTime(4),
                        //f_tramite = Convert.ToDateTime(_reader.GetDateTime(5)),
                        //f_liberado = Convert.ToDateTime(_reader.GetDateTime(6)),
                        //b_estadoV = _reader.GetInt32(7).ToString(),
                        //c_correlativo = _reader.IsDBNull(8) ? 0 : (int)_reader.GetInt32(8),
                        //CalcDiasD = Convert.ToDouble(_reader.GetValue(9)),
                        //f_recepcion = Convert.ToDateTime(_reader.GetDateTime(10)),
                        //b_estado = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        //f_ini_dan = Convert.ToDateTime(_reader.GetDateTime(12)),
                        //TipoRevision = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        //b_retenido = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        //f_cancelado = Convert.ToDateTime(_reader.GetDateTime(15)),
                        //TipoRe = _reader.IsDBNull(16) ? "" : _reader.GetString(16)
                        n_contenedor = _reader.GetString(0),
                        c_tamaño = _reader.GetString(1),
                        n_folio = _reader.IsDBNull(2) ? "SIN" : _reader.GetString(2),
                        f_recep_patio = _reader.GetDateTime(3),
                        f_retenido = _reader.GetDateTime(4),
                        f_tramite = Convert.ToDateTime(_reader.GetDateTime(5)),
                        f_liberado = Convert.ToDateTime(_reader.GetDateTime(6)),
                        b_estadoV = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_correlativo = _reader.IsDBNull(8) ? 0 : _reader.GetInt32(8),
                        CalcDiasD = _reader.IsDBNull(9) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(9)),
                        f_recepcion = Convert.ToDateTime(_reader.GetDateTime(10)),
                        b_estado = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        f_ini_dan = Convert.ToDateTime(_reader.GetDateTime(12)),
                        TipoRevision = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        b_retenido = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        TipoRe = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        IdDeta = _reader.IsDBNull(16) ? 0 : _reader.GetInt32(16),
                        f_cancelado = Convert.ToDateTime(_reader.GetDateTime(17))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> ConsultarDANM(string n_contenedor, string c_naviera)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();
            string consulta = null;
            SqlCommand _command = null;
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                if (c_naviera != "11" && c_naviera != "216")
                {
                    consulta = @"SELECT TOP 1 n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, s_comodity, 
                                ISNULL(n_folio, '') n_folio, 
                                case when f_reg_dan is null then 0 else f_reg_dan end f_reg_dan,  case when f_tramite is null then 0 else f_tramite end f_tramite, case when f_reg_liberacion is null then 0 else f_reg_liberacion end f_liberado, CASE WHEN len(f_reg_dan) > 0 THEN 1 ELSE 0 END b_valido,   
                                case when f_reg_liberacion is null or f_tramite is null  then 0 else datediff(hour, f_tramite, f_reg_liberacion) end horas, case when f_reg_liberacion is null or f_tramite is null then 0 else round(cast(cast(datediff(hour, f_reg_dan, f_reg_liberacion) as numeric(10, 2)) / cast(24 as numeric(10, 2)) as numeric(10,2)), 2, 0) end tiempo, b.c_naviera 

                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                WHERE /*len(n_folio) > 0 and*/ c_naviera = '{0}' AND n_contenedor = '{1}'
                                order by 7 desc, 4 desc";

                    _command = new SqlCommand(string.Format(consulta, c_naviera, n_contenedor), _conn as SqlConnection);

                }
                else
                {
                    consulta = @"SELECT TOP 1 n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, s_comodity, 
                                ISNULL(n_folio, '') n_folio, 
                                case when f_reg_dan is null then 0 else f_reg_dan end f_reg_dan,  case when f_tramite is null then 0 else f_tramite end f_tramite, case when f_reg_liberacion is null then 0 else f_reg_liberacion end f_liberado, CASE WHEN len(f_reg_dan) > 0 THEN 1 ELSE 0 END b_valido,   
                                case when f_reg_liberacion is null or f_tramite is null  then 0 else datediff(hour, f_tramite, f_reg_liberacion) end horas, case when f_reg_liberacion is null or f_tramite is null then 0 else round(cast(cast(datediff(hour, f_reg_dan, f_reg_liberacion) as numeric(10, 2)) / cast(24 as numeric(10, 2)) as numeric(10,2)), 2, 0) end tiempo, b.c_naviera 

                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                WHERE n_contenedor = '{0}'
                                order by 7 desc, 4 desc";

                    _command = new SqlCommand(string.Format(consulta, n_contenedor), _conn as SqlConnection);



                }

                _command.CommandType = CommandType.Text;
                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        n_contenedor = _reader.GetString(0),
                        c_tamaño = _reader.GetString(1),
                        s_comodity = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        n_folio = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        f_recepcion = _reader.GetDateTime(4),
                        f_tramite = Convert.ToDateTime(_reader.GetDateTime(5)),
                        f_liberado = Convert.ToDateTime(_reader.GetDateTime(6)),
                        b_estadoV = _reader.GetInt32(7).ToString(),
                        c_correlativo = _reader.IsDBNull(8) ? 0 : _reader.GetInt32(8),
                        CalcDiasD = Convert.ToDouble(_reader.GetValue(9)),
                        c_navi = _reader.GetString(10)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<TrackingEnca> GetAllOrdersByCustomer(string n_contenedor, string c_naviera, DBComun.TipoBD pBD, int a_dm, int s_dm, int c_dm)
        {
            List<TrackingEnca> list = new List<TrackingEnca>();

            SqlCommand command = null;
            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                if (c_naviera != "11" && c_naviera != "289" && c_naviera != "216")
                {
                    string sql = @"SELECT a.iddeta, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, c_naviera, b.c_llegada, f_llegada,
                                CASE WHEN b_ret_dir = 'Y' THEN 'RETIRO DIRECTO' ELSE CASE WHEN rtrim(ltrim(b_transhipment)) = 'Y' THEN 'TRASBORDO' ELSE  'PATIO CEPA' END END b_trafico, CASE WHEN rtrim(ltrim(a.b_estado)) = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estado, a_manifiesto + '-' +  CONVERT(VARCHAR(4), n_manifiesto) manifiesto
                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                INNER JOIN CCO_DETA_DOC_NAVI g ON a.IdDoc = g.IdDoc
                                WHERE c_naviera = @c_naviera and n_contenedor = @n_contenedor AND YEAR(a.f_registro) >= 2015 AND g.b_estado = 1 AND a.b_autorizado = 1";

                    command = new SqlCommand(sql, _conn as SqlConnection);
                    command.Parameters.AddWithValue("@n_contenedor", n_contenedor);
                    command.Parameters.AddWithValue("@c_naviera", c_naviera);
                }
                else
                {
                    string sql = @"SELECT a.iddeta, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, c_naviera, b.c_llegada, f_llegada,
                                CASE WHEN b_ret_dir = 'Y' THEN 'RETIRO DIRECTO' ELSE CASE WHEN rtrim(ltrim(b_transhipment)) = 'Y' THEN 'TRASBORDO' ELSE  'PATIO CEPA' END END b_trafico, CASE WHEN rtrim(ltrim(a.b_estado)) = 'F' THEN 'LLENO' ELSE 'VACIO' END b_estado, a_manifiesto + '-' +  CONVERT(VARCHAR(4), n_manifiesto) manifiesto
                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                INNER JOIN CCO_DETA_DOC_NAVI g ON a.IdDoc = g.IdDoc
                                WHERE n_contenedor = @n_contenedor AND YEAR(a.f_registro) >= 2015 AND g.b_estado = 1 AND a.b_autorizado = 1";

                    command = new SqlCommand(sql, _conn as SqlConnection);
                    command.Parameters.AddWithValue("@n_contenedor", n_contenedor);
                }

                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadOrders(reader, pBD, a_dm, s_dm, c_dm));
                }
            }

            return list;

        }

        public static List<TrackingEnca> GetAllOrdersByCustomer(string n_contenedor, string c_naviera, DBComun.TipoBD pBD, int a_dm, int s_dm, int c_dm, string n_mani)
        {
            List<TrackingEnca> list = new List<TrackingEnca>();

            SqlCommand command = null;
            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                if (c_naviera != "11" && c_naviera != "289" && c_naviera != "216")
                {
                    string sql = @"SELECT a.iddeta, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, c_naviera, b.c_llegada, f_llegada,
                                CASE WHEN b_ret_dir = 'Y' THEN 'RETIRO DIRECTO' ELSE CASE WHEN rtrim(ltrim(b_transhipment)) = 'Y' THEN 'TRASBORDO' ELSE  'PATIO CEPA' END END b_trafico, CASE WHEN rtrim(ltrim(a.b_estado)) = 'F' THEN 'LLENO' ELSE 'VACIO' END + CASE WHEN c_condicion = 'F' THEN '/FCL' ELSE CASE WHEN c_condicion = 'L' THEN '/LCL' ELSE '' END END b_estado, a_manifiesto + '-' +  CONVERT(VARCHAR(4), n_manifiesto) manifiesto
                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                INNER JOIN CCO_DETA_DOC_NAVI g ON a.IdDoc = g.IdDoc
                                WHERE c_naviera = @c_naviera and n_contenedor = @n_contenedor AND YEAR(a.f_registro) >= 2015 AND g.b_estado = 1 AND a.b_autorizado = 1
                                AND CONCAT(g.a_manifiesto, '-', g.n_manifiesto) = @n_manifiesto";

                    command = new SqlCommand(sql, _conn as SqlConnection);
                    command.Parameters.AddWithValue("@n_contenedor", n_contenedor);
                    command.Parameters.AddWithValue("@c_naviera", c_naviera);
                    command.Parameters.AddWithValue("@n_manifiesto", n_mani);
                }
                else
                {
                    string sql = @"SELECT a.iddeta, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, c_naviera, b.c_llegada, f_llegada,
                                CASE WHEN b_ret_dir = 'Y' THEN 'RETIRO DIRECTO' ELSE CASE WHEN rtrim(ltrim(b_transhipment)) = 'Y' THEN 'TRASBORDO' ELSE  'PATIO CEPA' END END b_trafico, CASE WHEN rtrim(ltrim(a.b_estado)) = 'F' THEN 'LLENO' ELSE 'VACIO' END + CASE WHEN c_condicion = 'F' THEN '/FCL' ELSE CASE WHEN c_condicion = 'L' THEN '/LCL' ELSE '' END END b_estado, a_manifiesto + '-' +  CONVERT(VARCHAR(4), n_manifiesto) manifiesto
                                FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                INNER JOIN CCO_DETA_DOC_NAVI g ON a.IdDoc = g.IdDoc
                                WHERE n_contenedor = @n_contenedor AND YEAR(a.f_registro) >= 2015 AND g.b_estado = 1 AND a.b_autorizado = 1
                                AND CONCAT(g.a_manifiesto, '-', g.n_manifiesto) = @n_manifiesto";

                    command = new SqlCommand(sql, _conn as SqlConnection);
                    command.Parameters.AddWithValue("@n_contenedor", n_contenedor);
                    command.Parameters.AddWithValue("@n_manifiesto", n_mani);
                }

                command.CommandType = CommandType.Text;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadOrders(reader, pBD, a_dm, s_dm, c_dm));
                }
            }

            return list;

        }

        public static List<TrackingEnca> GetAllOrdersByCustomer(string n_contenedor, string c_naviera, DBComun.TipoBD pBD, string _valor)
        {
            List<TrackingEnca> list = new List<TrackingEnca>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_TRACKING_CABE_IN", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));

                SqlDataReader _reader = _command.ExecuteReader();


                while (_reader.Read())
                {
                    list.Add(LoadOrders(_reader, pBD, _valor));
                }
            }

            return list;

        }

        private static TrackingEnca LoadOrders(IDataReader reader, DBComun.TipoBD pBD, int a_dm, int s_dm, int c_dm)
        {
            TrackingEnca item = new TrackingEnca
            {
                IdDeta = Convert.ToInt32(reader["IdDeta"]),
                n_contenedor = Convert.ToString(reader["n_contenedor"]),
                c_tamaño = Convert.ToString(reader["c_tamaño"]),
                c_naviera = Convert.ToString(reader["c_naviera"]),
                c_llegada = Convert.ToString(reader["c_llegada"]),
                f_llegada = Convert.ToDateTime(reader["f_llegada"]),
                b_estado = Convert.ToString(reader["b_estado"]),
                b_trafico = Convert.ToString(reader["b_trafico"]),
                n_manifiesto = Convert.ToString(reader["manifiesto"])
            };
            item.TrackingList = GetOrderDetailsByOrder(item.IdDeta, pBD, item.n_manifiesto, a_dm, s_dm, c_dm);

            return item;
        }

        private static TrackingEnca LoadOrders(IDataReader reader, DBComun.TipoBD pBD, string _valor)
        {
            TrackingEnca item = new TrackingEnca
            {
                IdDeta = Convert.ToInt32(reader["IdDeta"]),
                n_contenedor = Convert.ToString(reader["n_contenedor"]),
                c_tamaño = Convert.ToString(reader["c_tamaño"]),
                c_naviera = Convert.ToString(reader["c_naviera"]),
                c_llegada = Convert.ToString(reader["c_llegada"]),
                f_llegada = Convert.ToDateTime(reader["f_llegada"]),
                b_estado = Convert.ToString(reader["b_estado"]),
                b_trafico = Convert.ToString(reader["b_trafico"]),
                n_manifiesto = Convert.ToString(reader["manifiesto"]),
                b_cancelado = Convert.ToString(reader["b_cancelado"]),
                b_requiere = Convert.ToString(reader["c_tipo_bl"]),
                b_shipper = Convert.ToString(reader["b_shipper"]),
                pais_origen =  Convert.ToString(reader["PaisOrigen"])
            };
            item.TrackingList = GetOrderDetailsByOrder(item.IdDeta, pBD, item.n_manifiesto, _valor);

            return item;
        }

        public static List<TrackingDatails> GetOrderDetailsByOrder(int IdDeta, DBComun.TipoBD pBD, string n_manifiesto, int a_dm, int s_dm, int c_dm)
        {
            List<TrackingDatails> list = new List<TrackingDatails>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string[] _manis = n_manifiesto.Split('-');
                string a_mani = _manis[0].ToString();
                string n_manis = _manis[1].ToString();

                #region "Consultar"
                /*string sql = @"select ISNULL(n_folio, '') n_folio, a.IdDeta, 
                            case when a.f_registro is null then 0 else a.f_registro end f_rep_navi, 
                            case when a.f_autorizacion is null then 0 else a.f_autorizacion end f_aut_aduana,
                            case when a.f_rpatio is null then 0 else a.f_rpatio end f_recep_patio,
                            case when f_reg_dan is null then '' else CONVERT(CHAR(10), f_reg_dan, 103) + ' ' + CONVERT(CHAR(10), f_reg_dan, 108) + ' # Oficio: ' + ISNULL(n_folio, '') end f_reg_dan,  case when f_tramite is null then 0 else f_tramite end f_tramite, case when f_reg_liberacion is null then 0 else f_reg_liberacion end f_liberado, 
                            case when a.f_salida is null then '' else convert(char(10), a.f_salida, 103) + ' ' + convert(char(8), a.f_salida, 108) + ' # Doc.: ' + isnull(cast(a.num_salida as varchar(10)), '') + ' Placa : '  + isnull(placa, '')  + ' Motorista : ' + isnull(motorista, '')  + ' Transporte : ' + isnull(transporte, '')    end f_salida_carga,
                            case when a.f_sturno_ing is null then 0 else a.f_sturno_ing end f_solic_ingre, 
                            case when a.f_aut_patio is null then 0 else a.f_aut_patio end f_aut_patio,
                            case when a.f_confir_salida is null then 0 else a.f_confir_salida end f_confir_puerta, b.c_llegada, a.n_contenedor, b.c_naviera, ISNULL(a.s_comentarios, '') s_comentarios,
                            case when a.f_trasmision is null then 0 else a.f_trasmision end f_trasmision, s_consignatario, s_comodity,
                            case when a.f_caseta is null then 0 else a.f_caseta end f_caseta,
                            case when a.f_reg_dan is not null then case when a.f_ini_dan is null then 'NO SE POSEE REGISTRO' else CONVERT(CHAR(10), f_ini_dan, 103) + ' ' + CONVERT(CHAR(10), f_ini_dan, 108) + ' Tipo de Revisión : ' +  (SELECT t_revision FROM CCO_REVISION_DAN WHERE IdRevision = a.IdRevision)  end else '' end f_marchamo_dan
                            FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                            INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                            INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                            WHERE iddeta = @IdDeta";*/
                #endregion 

                SqlCommand _command = new SqlCommand("PA_DETALLE_TRACKING", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@a_dm", a_dm));
                _command.Parameters.Add(new SqlParameter("@s_dm", s_dm));
                _command.Parameters.Add(new SqlParameter("@c_dm", c_dm));
                _command.Parameters.Add(new SqlParameter("@a_mani", a_mani));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_manis));
                _command.Parameters.Add(new SqlParameter("@IdDeta", IdDeta));

                SqlDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadOrderDetails(reader));
                }
            }

            return list;

        }

        public static List<TrackingDatails> GetOrderDetailsByOrder(int IdDeta, DBComun.TipoBD pBD, string n_manifiesto, string _valor)
        {
            List<TrackingDatails> list = new List<TrackingDatails>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                string[] _manis = n_manifiesto.Split('-');
                string a_mani = _manis[0].ToString();
                string n_manis = _manis[1].ToString();

                #region "Consultar"
                /*string sql = @"select ISNULL(n_folio, '') n_folio, a.IdDeta, 
                            case when a.f_registro is null then 0 else a.f_registro end f_rep_navi, 
                            case when a.f_autorizacion is null then 0 else a.f_autorizacion end f_aut_aduana,
                            case when a.f_rpatio is null then 0 else a.f_rpatio end f_recep_patio,
                            case when f_reg_dan is null then '' else CONVERT(CHAR(10), f_reg_dan, 103) + ' ' + CONVERT(CHAR(10), f_reg_dan, 108) + ' # Oficio: ' + ISNULL(n_folio, '') end f_reg_dan,  case when f_tramite is null then 0 else f_tramite end f_tramite, case when f_reg_liberacion is null then 0 else f_reg_liberacion end f_liberado, 
                            case when a.f_salida is null then '' else convert(char(10), a.f_salida, 103) + ' ' + convert(char(8), a.f_salida, 108) + ' # Doc.: ' + isnull(cast(a.num_salida as varchar(10)), '') + ' Placa : '  + isnull(placa, '')  + ' Motorista : ' + isnull(motorista, '')  + ' Transporte : ' + isnull(transporte, '')    end f_salida_carga,
                            case when a.f_sturno_ing is null then 0 else a.f_sturno_ing end f_solic_ingre, 
                            case when a.f_aut_patio is null then 0 else a.f_aut_patio end f_aut_patio,
                            case when a.f_confir_salida is null then 0 else a.f_confir_salida end f_confir_puerta, b.c_llegada, a.n_contenedor, b.c_naviera, ISNULL(a.s_comentarios, '') s_comentarios,
                            case when a.f_trasmision is null then 0 else a.f_trasmision end f_trasmision, s_consignatario, s_comodity,
                            case when a.f_caseta is null then 0 else a.f_caseta end f_caseta,
                            case when a.f_reg_dan is not null then case when a.f_ini_dan is null then 'NO SE POSEE REGISTRO' else CONVERT(CHAR(10), f_ini_dan, 103) + ' ' + CONVERT(CHAR(10), f_ini_dan, 108) + ' Tipo de Revisión : ' +  (SELECT t_revision FROM CCO_REVISION_DAN WHERE IdRevision = a.IdRevision)  end else '' end f_marchamo_dan
                            FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                            INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                            INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                            WHERE iddeta = @IdDeta";*/
                #endregion

                SqlCommand _command = new SqlCommand("PA_DETALLE_TRACKING_S", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                _command.Parameters.Add(new SqlParameter("@a_mani", a_mani));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_manis));
                _command.Parameters.Add(new SqlParameter("@IdDeta", IdDeta));


                SqlDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadOrderDetails(reader, _valor));
                }
            }

            return list;

        }

        private static TrackingDatails LoadOrderDetails(IDataReader reader)
        {
            TrackingDatails item = new TrackingDatails
            {
                IdDeta = Convert.ToInt32(reader["IdDeta"]),
                n_oficio = Convert.ToString(reader["n_folio"]),
                f_rep_naviera = Convert.ToDateTime(reader["f_rep_navi"]),
                f_aut_aduana = Convert.ToDateTime(reader["f_aut_aduana"]),
                f_recep_patio = Convert.ToDateTime(reader["f_recep_patio"]),
                f_ret_dan = Convert.ToString(reader["f_reg_dan"]),
                f_tramite_dan = reader.IsDBNull(6) ? (DateTime?)null : Convert.ToDateTime(reader["f_tramite"]),
                f_liberado_dan = reader.IsDBNull(7) ? (DateTime?)null : Convert.ToDateTime(reader["f_liberado"]),
                f_salida_carga = Convert.ToString(reader["f_salida_carga"]),
                f_solic_ingreso = reader.IsDBNull(9) ? (DateTime?)null : Convert.ToDateTime(reader["f_solic_ingre"]),
                f_auto_patio = reader.IsDBNull(10) ? (DateTime?)null : Convert.ToDateTime(reader["f_aut_patio"]),
                f_puerta1 = reader.IsDBNull(11) ? (DateTime?)null : Convert.ToDateTime(reader["f_confir_puerta"]),
                c_llegada = Convert.ToString(reader["c_llegada"]),
                n_contenedor = Convert.ToString(reader["n_contenedor"]),
                c_naviera = Convert.ToString(reader["c_naviera"])
            };
            //item.ubicacion = ObtenerUbicacion(item.c_llegada, item.n_contenedor, item.c_naviera);
            item.s_comentarios = Convert.ToString(reader["s_comentarios"]);
            item.f_trans_aduana = reader.IsDBNull(16) ? (DateTime?)null : Convert.ToDateTime(reader["f_trasmision"]);
            item.s_consignatario = Convert.ToString(reader["s_consignatario"]);
            item.descripcion = Convert.ToString(reader["s_comodity"]);
            item.f_caseta = reader.IsDBNull(19) ? (DateTime?)null : Convert.ToDateTime(reader["f_caseta"]);
            item.f_marchamo_dan = Convert.ToString(reader["f_marchamo_dan"]);
            item.f_recepA = reader.IsDBNull(21) ? (DateTime?)null : Convert.ToDateTime(reader["f_recepcion"]);
            item.f_reg_aduana = Convert.ToString(reader["f_reg_aduana"]);
            item.f_reg_selectivo = Convert.ToString(reader["f_reg_selectivo"]);
            item.f_lib_aduana = reader.IsDBNull(24) ? (DateTime?)null : Convert.ToDateTime(reader["f_lib_aduana"]);
            item.f_ret_mag = reader.IsDBNull(25) ? (DateTime?)null : Convert.ToDateTime(reader["f_ret_mag"]);
            item.f_lib_mag = reader.IsDBNull(26) ? (DateTime?)null : Convert.ToDateTime(reader["f_lib_mag"]);
            item.f_ret_ucc = Convert.ToString(reader["f_retencion_ucc"]);
            item.f_tramite_ucc = reader.IsDBNull(28) ? (DateTime?)null : Convert.ToDateTime(reader["f_trami_ucc"]);
            item.f_liberado_ucc = reader.IsDBNull(29) ? (DateTime?)null : Convert.ToDateTime(reader["f_liberado_ucc"]);
            item.f_marchamo_ucc = Convert.ToString(reader["f_marchamo_ucc"]);
            item.f_cancelado = Convert.ToString(reader["f_cancelado"]);
            item.f_deta_dan = Convert.ToString(reader["f_lib_Dan_det"]);
            item.f_deta_ucc = Convert.ToString(reader["f_lib_UCC_det"]);
            item.f_retencion_dga = Convert.ToString(reader["f_ret_dga"]);
            item.f_lib_dga = Convert.ToString(reader["f_lib_dga"]);
            return item;
        }

        private static TrackingDatails LoadOrderDetails(IDataReader reader, string _valor)
        {
            TrackingDatails item = new TrackingDatails
            {
                IdDeta = Convert.ToInt32(reader["IdDeta"]),
                n_oficio = Convert.ToString(reader["n_folio"]),
                f_rep_naviera = Convert.ToDateTime(reader["f_rep_navi"]),
                f_aut_aduana = Convert.ToDateTime(reader["f_aut_aduana"]),
                f_recep_patio = Convert.ToDateTime(reader["f_recep_patio"]),
                f_ret_dan = Convert.ToString(reader["f_reg_dan"]),
                f_tramite_dan = Convert.ToDateTime(reader["f_tramite"]),
                f_liberado_dan = Convert.ToDateTime(reader["f_liberado"]),
                f_salida_carga = Convert.ToString(reader["f_salida_carga"]),
                f_solic_ingreso = Convert.ToDateTime(reader["f_solic_ingre"]),
                f_auto_patio = Convert.ToDateTime(reader["f_aut_patio"]),
                f_puerta1 = Convert.ToDateTime(reader["f_confir_puerta"]),
                c_llegada = Convert.ToString(reader["c_llegada"]),
                n_contenedor = Convert.ToString(reader["n_contenedor"]),
                c_naviera = Convert.ToString(reader["c_naviera"])
            };
            item.s_comentarios = Convert.ToString(reader["s_comentarios"]);
            item.f_trans_aduana = Convert.ToDateTime(reader["f_trasmision"]);
            item.s_consignatario = Convert.ToString(reader["s_consignatario"]);
            item.descripcion = Convert.ToString(reader["s_comodity"]);
            item.f_caseta = Convert.ToDateTime(reader["f_caseta"]);
            item.f_marchamo_dan = Convert.ToString(reader["f_marchamo_dan"]);
            item.f_recepA = Convert.ToDateTime(reader["f_recepcion"]);
            item.f_cancelado = Convert.ToString(reader["f_cancelado"]);
            item.f_cambio = Convert.ToString(reader["f_cambio"]);
            item.f_ret_ucc = Convert.ToString(reader["f_retencion_ucc"]);
            item.f_tramite_ucc = Convert.ToDateTime(reader["f_trami_ucc"]);
            item.f_liberado_ucc = Convert.ToDateTime(reader["f_liberado_ucc"]);
            item.f_marchamo_ucc = Convert.ToString(reader["f_marchamo_ucc"]);
            item.f_deta_dan = Convert.ToString(reader["f_lib_Dan_det"]);
            item.f_deta_ucc = Convert.ToString(reader["f_lib_UCC_det"]);
            item.f_retencion_dga = Convert.ToString(reader["f_ret_dga"]);
            item.f_lib_dga = Convert.ToString(reader["f_lib_dga"]);
            //item.f_reg_aduana = Convert.ToString(reader["f_reg_aduana"]);
            //item.f_reg_selectivo = Convert.ToString(reader["f_reg_selectivo"]);
            //item.f_lib_aduana = Convert.ToDateTime(reader["f_lib_aduana"]);
            //item.f_ret_mag = Convert.ToDateTime(reader["f_ret_mag"]);
            //item.f_lib_mag = Convert.ToDateTime(reader["f_lib_mag"]);
            return item;
        }
        public static List<DetaNaviera> ObtenerDetaTrans(string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_CONSUL_RECEPCION", _conn as SqlConnection)
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
                        n_contenedor = _reader.GetString(1),
                        c_tamaño = _reader.GetString(2),
                        b_trans = _reader.GetString(3),
                        f_trans = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        f_recep = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        f_dan = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        c_llegada = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        c_cliente = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        c_manifiesto = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_recepcion_c = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_requiere = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        b_estado = _reader.IsDBNull(12) ? "" : _reader.GetString(12)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
        public static List<DetaNaviera> ObtenerDetaTransAuto(string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_CONSUL_RECEPCION_AUT", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        c_correlativo = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),                       
                        c_llegada = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_cliente = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_manifiesto = _reader.IsDBNull(6) ? "" : _reader.GetString(6),                     
                        b_estado = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        s_consignatario = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        b_despacho = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_manejo = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_transferencia = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(12))
                    };
                    notiLista.Add(_notificacion);
                }
                _reader.Close();
                _conn.Close();
                return notiLista;
            }
        }

        public static List<DetaNaviera> ObtenerDetaTransAutoSrv(string c_llegada)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_CONSUL_RECEPCION_AUT_SRV", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        c_correlativo = _reader.GetInt32(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño = _reader.GetString(3),
                        c_llegada = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_cliente = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_manifiesto = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        b_estado = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        s_consignatario = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        b_despacho = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_manejo = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_transferencia = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        v_peso = Convert.ToDouble(_reader.GetDecimal(12)),
                        f_recep = Convert.ToString(_reader.GetString(13)),
                        f_tramite_s = Convert.ToString(_reader.GetString(14))
                    };
                    notiLista.Add(_notificacion);
                }
                _reader.Close();
                _conn.Close();
                return notiLista;
            }
        }


        public static string AlmacenarSADFI(DetaNaviera pDeta, DBComun.Estado pEstado)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                AseCommand _command = new AseCommand("sp_fport_crea_listaconte_fox", _conn as AseConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                AseParameter p_Contenedor = _command.Parameters.Add("@c_contenedor", AseDbType.VarChar);
                p_Contenedor.Value = pDeta.n_contenedor;

                AseParameter p_Operacion = _command.Parameters.Add("@c_operacion", AseDbType.VarChar);
                p_Operacion.Value = "NULL";

                AseParameter p_Manejo = _command.Parameters.Add("@b_manejo", OleDbType.VarChar);
                p_Manejo.Value = pDeta.b_manejo.Trim().TrimEnd().TrimStart();

                AseParameter p_Transfer = _command.Parameters.Add("@b_transf", AseDbType.VarChar);
                p_Transfer.Value = pDeta.b_transferencia.Trim().TrimEnd().TrimStart();

                AseParameter p_Despacho = _command.Parameters.Add("@b_despacho", AseDbType.VarChar);
                p_Despacho.Value = pDeta.b_despacho.Trim().TrimEnd().TrimStart();

                AseParameter p_Cliente = _command.Parameters.Add("@c_cliente", AseDbType.VarChar);
                p_Cliente.Value = pDeta.c_cliente;

                AseParameter p_Cliente_li = _command.Parameters.Add("@c_cliente_light", AseDbType.VarChar);
                p_Cliente_li.Value = "NULL";


                AseParameter p_Llegada = _command.Parameters.Add("@c_llegada", AseDbType.VarChar);
                p_Llegada.Value = pDeta.c_llegada;

                AseParameter p_Tamano = _command.Parameters.Add("@c_tamano", AseDbType.VarChar);
                p_Tamano.Value = pDeta.c_tamaño;

                AseParameter p_Estado = _command.Parameters.Add("@b_vacio_lleno", AseDbType.VarChar);
                p_Estado.Value = pDeta.b_estado;

                AseParameter p_Movimiento = _command.Parameters.Add("@c_movimiento", AseDbType.VarChar);
                p_Movimiento.Value = "I";

                AseParameter p_Condicion = _command.Parameters.Add("@c_condicion", AseDbType.VarChar);
                p_Condicion.Value = pDeta.c_condicion;

                AseParameter p_Peso = _command.Parameters.Add("@v_peso", AseDbType.Decimal);
                p_Peso.Value = pDeta.v_peso;

                AseParameter p_Consignatario = _command.Parameters.Add("@s_consignatario", AseDbType.VarChar);
                p_Consignatario.Value = pDeta.s_consignatario;

                // _command.Parameters.Add(new SqlParameter("@c_correlativo", _Encanaviera.c_correlativo));

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string AlmacenarSADFI_AMP(DetaNaviera pDeta, DBComun.Estado pEstado)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                AseCommand _command = new AseCommand("sp_fp_crea_listaconte_fox_amp", _conn as AseConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                AseParameter p_Contenedor = _command.Parameters.Add("@c_contenedor", AseDbType.VarChar);
                p_Contenedor.Value = pDeta.n_contenedor;

                AseParameter p_Operacion = _command.Parameters.Add("@c_operacion", AseDbType.VarChar);
                p_Operacion.Value = "NULL";

                AseParameter p_Manejo = _command.Parameters.Add("@b_manejo", AseDbType.VarChar);
                p_Manejo.Value = pDeta.b_manejo.Trim().TrimEnd().TrimStart();

                AseParameter p_Transfer = _command.Parameters.Add("@b_transf", AseDbType.VarChar);
                p_Transfer.Value = pDeta.b_transferencia.Trim().TrimEnd().TrimStart();

                AseParameter p_Despacho = _command.Parameters.Add("@b_despacho", AseDbType.VarChar);
                p_Despacho.Value = pDeta.b_despacho.Trim().TrimEnd().TrimStart();

                AseParameter p_Cliente = _command.Parameters.Add("@c_cliente", AseDbType.VarChar);
                p_Cliente.Value = pDeta.c_cliente;

                AseParameter p_Cliente_li = _command.Parameters.Add("@c_cliente_light", AseDbType.VarChar);
                p_Cliente_li.Value = "NULL";


                AseParameter p_Llegada = _command.Parameters.Add("@c_llegada", AseDbType.VarChar);
                p_Llegada.Value = pDeta.c_llegada;

                AseParameter p_Tamano = _command.Parameters.Add("@c_tamano", AseDbType.VarChar);
                p_Tamano.Value = pDeta.c_tamaño;

                AseParameter p_Estado = _command.Parameters.Add("@b_vacio_lleno", AseDbType.VarChar);
                p_Estado.Value = pDeta.b_estado;

                AseParameter p_Movimiento = _command.Parameters.Add("@c_movimiento", AseDbType.VarChar);
                p_Movimiento.Value = "I";

                AseParameter p_Condicion = _command.Parameters.Add("@c_condicion", AseDbType.VarChar);
                p_Condicion.Value = pDeta.c_condicion;

                AseParameter p_Peso = _command.Parameters.Add("@v_peso", AseDbType.Decimal);
                p_Peso.Value = pDeta.v_peso;

                AseParameter p_Consignatario = _command.Parameters.Add("@s_consignatario", AseDbType.VarChar);
                p_Consignatario.Value = pDeta.s_consignatario;

                AseParameter p_numero_lista = _command.Parameters.Add("@e_numero_lista", AseDbType.Integer);
                p_numero_lista.Value = pDeta.c_correlativo;


                AseParameter p_v_tara = _command.Parameters.Add("@v_tara", AseDbType.Decimal);
                p_v_tara.Value = pDeta.v_tara;

                AseParameter p_descripcion_merca = _command.Parameters.Add("@s_descripcion_mercaderia", AseDbType.VarChar);
                p_descripcion_merca.Value = pDeta.s_comodity;

                AseParameter p_pais_transfer = _command.Parameters.Add("@s_pais_transferencia", AseDbType.VarChar);
                p_pais_transfer.Value = pDeta.c_pais_origen;

                AseParameter p_pais_Destino = _command.Parameters.Add("@s_pais_destino", AseDbType.VarChar);
                p_pais_Destino.Value = pDeta.c_pais_destino;
                // _command.Parameters.Add(new SqlParameter("@c_correlativo", _Encanaviera.c_correlativo));

                AseParameter p_ReqTarja = _command.Parameters.Add("@reqtarja", AseDbType.VarChar);
                p_ReqTarja.Value = pDeta.b_requiere;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string SADFI_BL(DetaNaviera pDeta, DBComun.Estado pEstado)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                AseCommand _command = new AseCommand("sp_fp_adu_crea_lista_conte", _conn as AseConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                AseParameter p_Contenedor = _command.Parameters.Add("@c_contenedor", AseDbType.VarChar);
                p_Contenedor.Value = pDeta.n_contenedor;

                AseParameter p_Operacion = _command.Parameters.Add("@c_operacion", AseDbType.VarChar);
                p_Operacion.Value = "NULL";

                AseParameter p_Manejo = _command.Parameters.Add("@b_manejo", AseDbType.VarChar);
                p_Manejo.Value = pDeta.b_manejo.Trim().TrimEnd().TrimStart();

                AseParameter p_Transfer = _command.Parameters.Add("@b_transf", AseDbType.VarChar);
                p_Transfer.Value = pDeta.b_transferencia.Trim().TrimEnd().TrimStart();

                AseParameter p_Despacho = _command.Parameters.Add("@b_despacho", AseDbType.VarChar);
                p_Despacho.Value = pDeta.b_despacho.Trim().TrimEnd().TrimStart();

                AseParameter p_Cliente = _command.Parameters.Add("@c_cliente", AseDbType.VarChar);
                p_Cliente.Value = pDeta.c_cliente;

                AseParameter p_Cliente_li = _command.Parameters.Add("@c_cliente_light", AseDbType.VarChar);
                p_Cliente_li.Value = "NULL";


                AseParameter p_Llegada = _command.Parameters.Add("@c_llegada", AseDbType.VarChar);
                p_Llegada.Value = pDeta.c_llegada;

                AseParameter p_Tamano = _command.Parameters.Add("@c_tamano", AseDbType.VarChar);
                p_Tamano.Value = pDeta.c_tamaño;

                AseParameter p_Estado = _command.Parameters.Add("@b_vacio_lleno", AseDbType.VarChar);
                p_Estado.Value = pDeta.b_estado;

                AseParameter p_Movimiento = _command.Parameters.Add("@c_movimiento", AseDbType.VarChar);
                p_Movimiento.Value = "I";

                AseParameter p_Condicion = _command.Parameters.Add("@c_condicion", AseDbType.VarChar);
                p_Condicion.Value = pDeta.c_condicion;

                AseParameter p_Peso = _command.Parameters.Add("@v_peso", AseDbType.Decimal);
                p_Peso.Value = pDeta.v_peso;

                AseParameter p_Consignatario = _command.Parameters.Add("@s_consignatario", AseDbType.VarChar);
                p_Consignatario.Value = pDeta.s_consignatario;

                AseParameter p_numero_lista = _command.Parameters.Add("@e_numero_lista", AseDbType.Integer);
                p_numero_lista.Value = pDeta.c_correlativo;


                AseParameter p_v_tara = _command.Parameters.Add("@v_tara", AseDbType.Decimal);
                p_v_tara.Value = pDeta.v_tara;

                AseParameter p_descripcion_merca = _command.Parameters.Add("@s_descripcion_mercaderia", AseDbType.VarChar);
                p_descripcion_merca.Value = pDeta.s_comodity;

                AseParameter p_pais_transfer = _command.Parameters.Add("@s_pais_transferencia", AseDbType.VarChar);
                p_pais_transfer.Value = pDeta.c_pais_origen;

                AseParameter p_pais_Destino = _command.Parameters.Add("@s_pais_destino", AseDbType.VarChar);
                p_pais_Destino.Value = pDeta.c_pais_destino;

                AseParameter p_ReqTarja = _command.Parameters.Add("@reqtarja", AseDbType.VarChar);
                p_ReqTarja.Value = pDeta.b_requiere;

                AseParameter p_s_bl = _command.Parameters.Add("@s_bl", AseDbType.VarChar);
                p_s_bl.Value = pDeta.n_BL;

                AseParameter p_s_manifiesto_aduana = _command.Parameters.Add("@s_manifiesto_aduana", AseDbType.VarChar);
                p_s_manifiesto_aduana.Value = pDeta.n_manifiesto;

                AseParameter p_s_anio_mani_adu = _command.Parameters.Add("@s_anio_mani_adu", AseDbType.VarChar);
                p_s_anio_mani_adu.Value = pDeta.a_manifiesto;

                AseParameter p_naduana = _command.Parameters.Add("@s_naduana", AseDbType.VarChar);
                p_naduana.Value = "02";

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string ActSADFI_AMP(List<TransferXML> pDeta, DBComun.Estado pEstado, string n_contenedor, string c_llegada, int IdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string resultado = null;

                AseCommand _command = new AseCommand("sp_fport_confirma_conte_patio", _conn as AseConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 120
                };

                AseParameter p_Contenedor = _command.Parameters.Add("@c_contenedor", AseDbType.VarChar);
                p_Contenedor.Value = n_contenedor.ToUpper();


                AseParameter p_Llegada = _command.Parameters.Add("@c_llegada", AseDbType.VarChar);
                p_Llegada.Value = c_llegada;


                resultado = _command.ExecuteScalar().ToString();

                string valorDe = ActSADFI_BD(pEstado, IdDeta, Convert.ToInt32(resultado));



                _conn.Close();
                return resultado;
            }

        }

        public static string ActSADFI_BD(DBComun.Estado pEstado, int pIdDeta, int b_sadfi)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                string _reader = null;
                _conn.Open();



                string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_sadfi = {0}
									WHERE IdDeta = {1}; 
									SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(consulta, b_sadfi, pIdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string ActSADFI_BD(DBComun.Estado pEstado, List<TransferXML> pDeta)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                string _reader = null;
                _conn.Open();
                foreach (var item in pDeta)
                {


                    string consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_sadfi = 1 
									WHERE IdDeta = {0}; 
									SELECT @@ROWCOUNT";

                    SqlCommand _command = new SqlCommand(string.Format(consulta, item.IdDeta), _conn as SqlConnection)
                    {
                        CommandType = CommandType.Text
                    };

                    _reader = _command.ExecuteScalar().ToString();
                }
                _conn.Close();
                return _reader;
            }

        }

        public static string ValidaContenedor(DBComun.Estado pEstado, string n_contenedor, DBComun.TipoBD pDB)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(pDB, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_validar_contenedor", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_contenedor", n_contenedor));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }
        public static string ValidRetDGA(DBComun.Estado pEstado, string n_contenedor, string a_manifiesto, int n_mani, DBComun.TipoBD pDB)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(pDB, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_validar_retdga", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_mani));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ValidaContenedorShipper(DBComun.Estado pEstado, string n_contenedor, string n_manifiesto, DBComun.TipoBD pDB)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(pDB, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_validar_contenedor_shipper", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_manifiesto", n_manifiesto));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<Pago> CalcPagos(string n_contenedor, string c_llegada, DateTime f_tarja, DBComun.TipoBD pBD, double v_peso)
        {
            List<Pago> notiLista = new List<Pago>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_calc_pagos", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));
                _command.Parameters.Add(new SqlParameter("@f_tarja", f_tarja));
                _command.Parameters.Add(new SqlParameter("@v_peso", v_peso));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Pago _notificacion = new Pago
                    {
                        n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        b_salida = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        f_salida = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        b_transito = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_tamaño = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        descripcion = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        s_naviero = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        s_cliente = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        s_pagos = _reader.IsDBNull(9) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(9)),
                        v_dias = _reader.IsDBNull(10) ? 0 : Convert.ToInt32(_reader.GetInt32(10)),
                        v_peso = _reader.IsDBNull(11) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(11)),
                        v_teus = _reader.IsDBNull(12) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(12))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<Pago> CalcPagos(string n_contenedor, string c_llegada, DateTime f_tarja, DateTime f_retiro, DBComun.TipoBD pBD, double v_peso)
        {
            List<Pago> notiLista = new List<Pago>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_calc_pagos_retiro", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));
                _command.Parameters.Add(new SqlParameter("@f_tarja", f_tarja));
                _command.Parameters.Add(new SqlParameter("@f_retiro", f_retiro));
                _command.Parameters.Add(new SqlParameter("@v_peso", v_peso));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Pago _notificacion = new Pago
                    {
                        n_contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        b_salida = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        f_salida = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        b_transito = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_tamaño = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        descripcion = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        s_naviero = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        s_cliente = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        s_pagos = _reader.IsDBNull(9) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(9)),
                        v_dias = _reader.IsDBNull(10) ? 0 : Convert.ToInt32(_reader.GetInt32(10)),
                        v_peso = _reader.IsDBNull(11) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(11)),
                        v_teus = _reader.IsDBNull(12) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(12))
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<DetaNaviera> ObtenerDetallesCambio(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT a.IdReg, IdDeta, n_BL, n_contenedor, CASE WHEN b_reef = 'Y' OR b_reef = 'N' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'U1' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'T1' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN RIGHT(RTRIM(LTRIM(c_tamaño)), 2) = 'P1' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamaño, CASE WHEN b_detenido = 1 THEN 'RETENIDO' ELSE 'NADA' END b_detenido, c_correlativo,
                                    CASE WHEN b_ret_dir = 'Y' THEN 'RETIRO DIRECTO' ELSE 'DESPACHO NORMAL' END b_estado_cambio
                                    FROM CCO_DETA_NAVIERAS b INNER JOIN CCO_ENCA_NAVIERAS a ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    WHERE a.IdReg = {0} AND b_cancelado = 0 AND f_rpatio IS NULL AND f_recepcion IS NULL
                                    ORDER BY n_contenedor";

                SqlCommand _command = new SqlCommand(string.Format(consulta, pId), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.GetInt32(0),
                        IdDeta = _reader.GetInt32(1),
                        n_BL = _reader.GetString(2),
                        n_contenedor = _reader.GetString(3),
                        c_tamaño = _reader.GetString(4),
                        b_retenido = _reader.IsDBNull(5) ? "" : _reader.GetString(5) == "NADA" ? "" : "RETENIDO",
                        c_correlativo = _reader.GetInt32(6),
                        b_estado = _reader.IsDBNull(7) ? "" : _reader.GetString(7)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActualizarCambiosId(DBComun.Estado pEstado, int pId, string pObservacion, string c_usuarios, string bc_cambio, string dc, string da, string c_tamaño, string b_reef)
        {
            SqlCommand _command = null;
            int pTipo = 0;
            string consulta = null;
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                if (!c_tamaño.Contains("HREF"))
                {
                    consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_ret_dir = '{0}', dc_anterior = '{1}', dc_actual = '{2}',  s_obser_cambio = '{3}', f_cambio = GETDATE(), us_cambio = '{4}'
									WHERE IdDeta = {5}; 
									SELECT @@ROWCOUNT";

                    _command = new SqlCommand(string.Format(consulta, bc_cambio, dc, da, pObservacion, c_usuarios, pId), _conn as SqlConnection)
                    {
                        CommandType = CommandType.Text
                    };
                }
                else
                {
                    consulta = @"UPDATE CCO_DETA_NAVIERAS 
									SET b_ret_dir = '{0}', dc_anterior = '{1}', dc_actual = '{2}',  s_obser_cambio = '{3}', f_cambio = GETDATE(), us_cambio = '{4}', b_reef = '{5}'
									WHERE IdDeta = {6}; 
									SELECT @@ROWCOUNT";

                    _command = new SqlCommand(string.Format(consulta, bc_cambio, dc, da, pObservacion, c_usuarios, b_reef, pId), _conn as SqlConnection)
                    {
                        CommandType = CommandType.Text
                    };
                }

                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();

                return _reader;
            }

        }
        public static string MaxOficio()
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT ISNULL(MAX(CAST(n_folio AS INT)), 0) + 1 MAXI FROM CCO_DETA_NAVIERAS 
                                    WHERE YEAR(f_reg_dan) = YEAR(GETDATE())";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string MaxUCC()
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT ISNULL(MAX(CAST(n_ofiucc AS INT)), 0) + 1 MAXI FROM CCO_DETA_NAVIERAS 
                                    WHERE YEAR(f_retencion_ucc) = YEAR(GETDATE())";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string MaxDocDGA()
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT ISNULL(MAX(n_aduana), 0) + 1 MAXI FROM CCO_DETA_NAVIERAS 
                                    WHERE year(ISNULL(f_det_aduana,'2017-03-01 00:00:00.000')) = YEAR(getdate())";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static List<ArchivoAduana> GetListOIRSA(string c_llegada, DBComun.Estado pTipo)
        {
            List<ArchivoAduana> notiLista = new List<ArchivoAduana>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                string consulta = @"SELECT rank() OVER (ORDER BY c_correlativo, IdDeta, r.IdReg) as rank, y.c_prefijo Naviera, n_contenedor, CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'R' THEN  d.d_descripcion + ' ' + 'HREF' ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'U' THEN d.d_descripcion + ' OPENTOP'  ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'T' THEN d.d_descripcion + ' TANQUE' ELSE CASE WHEN SUBSTRING(RTRIM(LTRIM(c_tamaño)), 3, 1) = 'P' THEN d.d_descripcion + ' FLAT' ELSE  CASE WHEN e.d_tipo = 'HC' THEN d.d_descripcion + ' HC' ELSE d.d_descripcion + ' ' + e.d_tipo END END END END END c_tamañoc, 
                                    s_consignatario, UPPER(b.CountryName) PaisProc, UPPER(f.CountryName) PaisDestino, CASE WHEN b.b_oirsa = 1 THEN 'MOSTYN' ELSE 'CARBONATO' END Tratamiento
                                    FROM CCO_DETA_NAVIERAS r  INNER JOIN CCO_COD_PAISES b ON c_pais_origen = b.CountryCode
                                    INNER JOIN CCO_ENCA_CON_LEN d ON SUBSTRING(c_tamaño, 1, 1) = d.IdValue
                                    INNER JOIN CCO_ENCA_CON_WIDTH e ON SUBSTRING(c_tamaño, 2, 1) = e.IdValue
                                    INNER JOIN CCO_COD_PAISES f ON SUBSTRING(c_pais_destino, 1, 2) = f.CountryCode
                                    INNER JOIN CCO_ENCA_NAVIERAS h ON h.IdReg = r.IdReg
                                    INNER JOIN CCO_DETA_DOC_NAVI z ON h.IdReg = z.IdReg AND r.IdDoc = z.IdDoc
                                    INNER JOIN CCO_USUARIOS_NAVIERAS y ON h.c_naviera = y.c_naviera
                                    WHERE b_autorizado = 1 AND h.c_llegada = '{0}' AND b_cancelado = 0 
                                    ORDER BY 1 ";

                SqlCommand _command = new SqlCommand(string.Format(consulta, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ArchivoAduana _notificacion = new ArchivoAduana
                    {
                        c_correlativo = _reader.GetInt32(0),
                        c_iso_navi = _reader.GetString(1),
                        n_contenedor = _reader.GetString(2),
                        c_tamaño_c = _reader.GetString(3),
                        s_consignatario = _reader.GetString(4),
                        c_pais_origen = _reader.GetString(5),
                        c_pais_destino = _reader.GetString(6),
                        b_tratamiento = _reader.GetString(7)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static List<Declaracion> getDeclaraciones(DBComun.Estado pTipo)
        {
            List<Declaracion> notiLista = new List<Declaracion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_DECLARACIONES", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 40
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Declaracion _notificacion = new Declaracion
                    {
                        IdRegAduana = _reader.GetInt32(0),
                        n_manifiesto = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_declaracion = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        n_contenedor = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        TipoEstado = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        f_aduana = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        Descripcion = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        s_consignatario = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        n_bl = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        n_nit = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        s_descripcion = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        s_tipo = _reader.IsDBNull(11) ? "" : _reader.GetString(11)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;
        }

        public static List<Declaracion> getDeclaracionesFilter(DBComun.Estado pTipo, string n_contenedor = "NULL", string n_declaracion = "NULL", int year = 0)
        {
            List<Declaracion> notiLista = new List<Declaracion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                if (n_contenedor == "")
                {
                    n_contenedor = "NULL";
                }

                if (n_declaracion == "")
                {
                    n_declaracion = "NULL";
                }


                SqlCommand _command = new SqlCommand("PA_DECLARACIONES_FND", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 40
                };

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_declaracion", n_declaracion));
                _command.Parameters.Add(new SqlParameter("@a_filtro", year));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Declaracion _notificacion = new Declaracion
                    {
                        /*IdRegAduana = (int)_reader.GetInt32(0),*/
                        n_manifiesto = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        n_declaracion = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        TipoEstado = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        f_aduana = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        Descripcion = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        s_consignatario = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        n_bl = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        n_nit = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        s_descripcion = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        s_tipo = _reader.IsDBNull(10) ? "" : _reader.GetString(10)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;
        }


        public static List<DetaNaviera> detaDGACnt(int pId)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_DGA_DETALLE", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdDoc", pId));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdReg = _reader.IsDBNull(0) ? 0 : _reader.GetInt32(0),
                        IdDeta = _reader.IsDBNull(1) ? 0 : _reader.GetInt32(1),
                        n_BL = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        n_contenedor = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        c_tamaño = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_correlativo = _reader.IsDBNull(5) ? 0 : _reader.GetInt32(5),
                        c_pais_origen = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        b_rdt = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        s_consignatario = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        b_cancelado = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_retenido = _reader.IsDBNull(10) ? "" : _reader.GetString(10),
                        b_aduanas = _reader.IsDBNull(11) ? "" : _reader.GetString(11)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string upDGA(DBComun.Estado pEstado, DetaNaviera pDeta)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_UP_DGA", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdDeta", pDeta.IdDeta));
                _command.Parameters.Add(new SqlParameter("@n_aduana", pDeta.n_aduana));
                _command.Parameters.Add(new SqlParameter("@us_detaduana", pDeta.us_aduana));


                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string upDocDGA(DBComun.Estado pEstado, int pIdDoc)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_UP_DOC_DGA", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdDoc", pIdDoc));



                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string upd_Cancelados(DBComun.Estado pEstado, int pIdDeta)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                string _reader = null;
                _conn.Open();



                string consulta = @"DECLARE @b_cancel CHAR(1), @numrow INT
                                    SET @b_cancel = (SELECT CASE WHEN b_cancelado = 1 THEN 'Y' ELSE 'N' END b_cancelado FROM CCO_DETA_NAVIERAS WHERE IdDeta = {0})
                                    SET @numrow = 0
                                    
                                    IF @b_cancel = 'N'
                                    BEGIN
                                        UPDATE CCO_DETA_NAVIERAS 
									    SET b_cancelado = 1, s_observaciones = 'NO SE REPORTA RECEPCION AL MOMENTO DEL ZARPE DEL BUQUE', f_cancelado = GETDATE(), us_cancelado = 'servicio.contenedores'
									    WHERE IdDeta = {1}; 
									    SET @numrow = @@ROWCOUNT
                                    END
                
                                    SELECT @numrow";


                SqlCommand _command = new SqlCommand(string.Format(consulta, pIdDeta, pIdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string obt_Sidunea(DBComun.Estado pEstado, int n_manifiesto, int a_manifiesto, string n_contenedor)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                string _reader = null;
                _conn.Open();



                string consulta = @"select CASE WHEN b_siduneawd = 1 THEN 1 ELSE 0 END b_sidunea
                                    from cco_deta_doc_navi a inner join cco_deta_navieras b on a.IdDoc = b.IdDoc
                                    where a_manifiesto = {0} and n_manifiesto = {1} and n_contenedor = '{2}'";


                SqlCommand _command = new SqlCommand(string.Format(consulta, a_manifiesto, n_manifiesto, n_contenedor), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static List<ProvisionalesEnca> getEncaProvi(int pId)
        {
            List<ProvisionalesEnca> notiLista = new List<ProvisionalesEnca>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_RESUMEN_PROVI", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdDeta", pId));
                

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ProvisionalesEnca _notificacion = new ProvisionalesEnca
                    {
                        IdDeta = _reader.IsDBNull(0) ? 0 : _reader.GetInt32(0),
                        Descripcion = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        Total = _reader.IsDBNull(2) ? 0 : _reader.GetInt32(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }
        public static List<ProvisionalesDeta> getDetaProvi(int pId)
        {
            List<ProvisionalesDeta> notiLista = new List<ProvisionalesDeta>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_DETALLE_PROVI", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdDeta", pId));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ProvisionalesDeta _notificacion = new ProvisionalesDeta
                    {
                        IdDeta = _reader.IsDBNull(0) ? 0 : _reader.GetInt32(0),
                        C_Llegada = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        Fecha_Prv = _reader.IsDBNull(2) ? Convert.ToDateTime(_reader.GetDateTime(2)) : _reader.GetDateTime(2),
                        Tipo = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        Motorista_Prv = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        Transporte_Prv = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        Placa_Prv = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        Chasis_Prv = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        Fec_Reserva = _reader.IsDBNull(8) ? (DateTime?)null : _reader.GetDateTime(8),
                        Fec_Valida = _reader.IsDBNull(9) ? (DateTime?)null : _reader.GetDateTime(9)

                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<Declaracion> getValidConten(string n_contenedor, string a_decla, string c_serie, string c_correlativo)
        {
            List<Declaracion> notiLista = new List<Declaracion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_valid_conten_track", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@a_decla", a_decla));
                _command.Parameters.Add(new SqlParameter("@s_decla", c_serie));
                _command.Parameters.Add(new SqlParameter("@c_decla", c_correlativo));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Declaracion _notificacion = new Declaracion
                    {
                        n_manifiesto = _reader.IsDBNull(0) ? "" : _reader.GetString(0)                      
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
            }
            return notiLista;

        }

        public static List<EncaNaviera> getEncaContenedor(string n_contenedor, string a_manifiesto, int n_mani, DBComun.Estado pTipo)
        {
            List<EncaNaviera> notiLista = new List<EncaNaviera>();    

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_validar_ubicacion", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_mani));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", a_manifiesto));               
                               
             
                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaNaviera _notificacion = new EncaNaviera
                    {
                        c_llegada = _reader.GetString(0),
                        c_naviera = _reader.GetString(1)                     
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                //System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static string upRetDGA(DBComun.Estado pEstado, string n_contenedor, string a_mani, int n_mani, string usuario, string comentarios)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_actualizar_retdga", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@usuario", usuario));
                _command.Parameters.Add(new SqlParameter("@comentarios", comentarios));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@a_mani", a_mani));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_mani));


                string _reader = _command.ExecuteScalar().ToString();

                _conn.Close();
                return _reader;
            }

        }

        public static string upRetDGA(DBComun.Estado pEstado, string n_contenedor, string a_mani, int n_mani, string usuario, string comentarios, int retencion)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_actualizar_retdga_srv", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@usuario", usuario));
                _command.Parameters.Add(new SqlParameter("@comentarios", comentarios));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                _command.Parameters.Add(new SqlParameter("@a_mani", a_mani));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_mani));
                _command.Parameters.Add(new SqlParameter("@b_dga", retencion));

                string _reader = _command.ExecuteScalar().ToString();


                _conn.Close();
                return _reader;
            }

        }

        public static List<DetaNaviera> getDescripcion()
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_inventario_descripcion", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };        


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        IdDeta = _reader.GetInt32(0),
                        s_comodity = _reader.IsDBNull(1) ? "" : _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                //System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static string validShipper(DBComun.Estado pEstado, string n_contenedor, DBComun.TipoBD pDB)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(pDB, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_valid_shipper", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_contenedor", n_contenedor));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaNaviera> getNaviValida()
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();             

                string consulta = @"select c_naviera, c_prefijo from CCO_USUARIOS_NAVIERAS";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        c_cliente = _reader.GetString(0),
                        c_navi = _reader.GetString(1)
                      
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
