using CEPA.CCO.Entidades;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class TrasnferXMLDAL
    {

        public static string validBL(DBComun.Estado pTipo, string a_mani, int n_mani, string n_BL)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_CONSULTAR_BL", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                _command.Parameters.Add(new SqlParameter("@n_BL", n_BL));
                _command.Parameters.Add(new SqlParameter("@n_mani", n_mani));
                _command.Parameters.Add(new SqlParameter("@a_mani", a_mani));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string updateBL(int pIdValue)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"update CCO_ADUANA_VALID
                                    set b_envio = 1, f_envio = GETDATE()
                                    where IdValue = {0}";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pIdValue), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<TransferXML> Transferencia(DBComun.Estado pEstado)
        {
            List<TransferXML> notiLista = new List<TransferXML>();

            object year = null;
            object aduana = null;
            object manifiesto = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
             
                SqlCommand _command = new SqlCommand("pa_pendientes_dga", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    if (ArchivoBookingDAL.isNumeric(_reader[0].ToString().TrimEnd().TrimStart()))
                    {
                        year = Convert.ToString(_reader[0]);
                    }
                    else
                    {
                        year = _reader[0];
                    }

                    if (ArchivoBookingDAL.isNumeric(_reader[1].ToString().TrimEnd().TrimStart()))
                    {
                        aduana = Convert.ToString(_reader[1]);
                    }
                    else
                    {
                        aduana = _reader[1];
                    }

                    if (ArchivoBookingDAL.isNumeric(_reader[2].ToString().TrimEnd().TrimStart()))
                    {
                        manifiesto = Convert.ToString(_reader[2]);
                    }
                    else
                    {
                        manifiesto = _reader[2];
                    }

                    //if (ArchivoBookingDAL.isNumeric(_reader[3].ToString().TrimEnd().TrimStart()))
                    //    nbl = Convert.ToString(_reader[3]);
                    //else
                    //    nbl = _reader[3];

                    TransferXML _notificacion = new TransferXML
                    {
                        year = year.ToString(),
                        aduana = aduana.ToString(),
                        nmanifiesto = manifiesto.ToString(),
                        nbl = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        contenedor = _reader.GetString(4),
                        f_rpatio = _reader.GetString(5),
                        sitio = _reader.GetString(6),
                        comentarios = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        IdDeta = _reader.GetInt32(8),
                        c_llegada = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        b_sidunea = _reader.IsDBNull(10) ? 0 : _reader.GetInt32(10)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<TransferXML> TransferenciaCabecera(DBComun.Estado pEstado, string n_manifiesto, string a_mani)
        {
            List<TransferXML> notiLista = new List<TransferXML>();
            object nmanifiesto = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*  string consulta = @"SELECT DISTINCT n_bl 
                                      FROM CCO_ADUANA_VALID  a INNER JOIN CCO_DETA_DOC_NAVI b ON a.n_manifiesto = b.n_manifiesto
                                      WHERE IdDoc IN(SELECT IdDoc FROM CCO_DETA_NAVIERAS WHERE b_transmision = 0 AND f_rpatio IS NOT NULL)
                                      ORDER BY 1";*/

                /*string consulta = @"SELECT DISTINCT c.n_bl, c.n_manifiesto, c.a_mani
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE b_transmision = 0 AND f_rpatio IS NOT NULL AND c.n_manifiesto = {0} AND c.a_mani = '{1}'                                     
                                    ORDER BY 2, 1";*/

                string consulta = @"SELECT DISTINCT c.n_bl, c.n_manifiesto, c.a_mani, b.c_llegada
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE b_transmision = 0  AND f_recepcion IS NOT NULL AND c.n_manifiesto = {0} AND c.a_mani = '{1}'
                                    ORDER BY 2, 1";

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_manifiesto, a_mani), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    //if (ArchivoBookingDAL.isNumeric(_reader[0].ToString().TrimEnd().TrimStart()))
                    //    nbl = Convert.ToString(_reader[0]);
                    //else
                    //    nbl = _reader[0];

                    if (ArchivoBookingDAL.isNumeric(_reader[1].ToString().TrimEnd().TrimStart()))
                    {
                        nmanifiesto = Convert.ToString(_reader[1]);
                    }
                    else
                    {
                        nmanifiesto = _reader[1];
                    }

                    TransferXML _notificacion = new TransferXML
                    {
                        nbl = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        nmanifiesto = nmanifiesto.ToString(),
                        c_llegada = _reader.IsDBNull(3) ? "" : _reader.GetString(3)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<TransferXML> TransferenciaCabeceraM(DBComun.Estado pEstado)
        {
            List<TransferXML> notiLista = new List<TransferXML>();

            object nbl = null;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*  string consulta = @"SELECT DISTINCT n_bl 
                                      FROM CCO_ADUANA_VALID  a INNER JOIN CCO_DETA_DOC_NAVI b ON a.n_manifiesto = b.n_manifiesto
                                      WHERE IdDoc IN(SELECT IdDoc FROM CCO_DETA_NAVIERAS WHERE b_transmision = 0 AND f_rpatio IS NOT NULL)
                                      ORDER BY 1";*/

                /*string consulta = @"SELECT DISTINCT c.n_manifiesto, c.a_mani
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE b_transmision = 0 AND f_rpatio IS NOT NULL                                   
                                    ORDER BY 2, 1";*/

                string consulta = @"SELECT DISTINCT c.n_manifiesto, c.a_mani, CASE WHEN b.b_siduneawd = 1 THEN 1 ELSE 0 END b_siduneawd
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS d ON a.IdReg = d.IdReg 
                                    INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto AND b.a_manifiesto = c.a_mani
                                    WHERE b_transmision = 0 AND f_recepcion IS NOT NULL  AND c.c_tipo_bl IS NOT NULL 
                                    ORDER BY 2, 1";


                /*string consulta = @"SELECT DISTINCT c.n_manifiesto, c.a_mani
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE b_transmision = 0 AND f_recepcion IS NOT NULL  AND c.c_tipo_bl IS NOT NULL
                                    ORDER BY 2, 1";*/

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    if (ArchivoBookingDAL.isNumeric(_reader[0].ToString().TrimEnd().TrimStart()))
                    {
                        nbl = Convert.ToString(_reader[0]);
                    }
                    else
                    {
                        nbl = _reader[0];
                    }

                    TransferXML _notificacion = new TransferXML
                    {
                        nmanifiesto = nbl.ToString(),
                        year = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        b_sidunea = _reader.IsDBNull(2) ? 0 : _reader.GetInt32(2)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<TransferXML> lstSidunea(DBComun.Estado pEstado)
        {
            List<TransferXML> notiLista = new List<TransferXML>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                string consulta = @"SELECT DISTINCT b_sidunea, e.c_naviera
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS d ON a.IdReg = d.IdReg 
                                    INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    INNER JOIN CCO_USUARIOS_NAVIERAS e ON d.c_naviera = e.c_naviera
                                    WHERE b_transmision = 0 AND f_recepcion IS NOT NULL  AND c.c_tipo_bl IS NOT NULL
                                    ORDER BY 2, 1";


                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {

                    TransferXML _notificacion = new TransferXML
                    {
                        b_sidunea = _reader.IsDBNull(0) ? 0 : _reader.GetInt32(0),
                        c_naviera = _reader.IsDBNull(1) ? "" : _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }


        public static string ActTrasmision(int pIdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_NAVIERAS
                                    SET b_transmision = 1, f_trasmision = GETDATE()
                                    WHERE IdDeta = {0}
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pIdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActCOARRI(int pValor, int pIdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_NAVIERAS
                                    SET b_coarri = {0}, f_coarri = GETDATE()
                                    WHERE IdDeta = {1}
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pValor, pIdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }


        public static List<CorteCOTECNA> shipperZarpe(DBComun.Estado pEstado, string c_llegada)
        {
            List<CorteCOTECNA> _empleados = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT DISTINCT a.c_llegada
                            FROM fa_llegadas a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND YEAR(a.f_arribo) >= 2015 AND a.c_llegada = '{0}' AND a.f_desatraque IS NOT NULL
                            ORDER BY a.c_llegada DESC";


                _command = new AseCommand(string.Format(_consulta, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorteCOTECNA _tmpEmpleado = new CorteCOTECNA
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<TransferXML> listaSADFI_Mani(DBComun.Estado pEstado, int a_mani, int n_mani)
        {
            List<TransferXML> notiLista = new List<TransferXML>();



            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*  string consulta = @"SELECT DISTINCT n_bl 
                                      FROM CCO_ADUANA_VALID  a INNER JOIN CCO_DETA_DOC_NAVI b ON a.n_manifiesto = b.n_manifiesto
                                      WHERE IdDoc IN(SELECT IdDoc FROM CCO_DETA_NAVIERAS WHERE b_transmision = 0 AND f_rpatio IS NOT NULL)
                                      ORDER BY 1";*/

                /*string consulta = @"SELECT DISTINCT c.n_manifiesto, c.a_mani
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE b_transmision = 0 AND f_rpatio IS NOT NULL                                   
                                    ORDER BY 2, 1";*/

                string consulta = @"select n_contenedor, c_llegada
                                    from cco_deta_navieras a inner join cco_deta_doc_navi b on a.IdDoc = b.IdDoc
                                    where a.f_recepcion is not null and a.f_trasmision is null and b.b_estado = 1 
                                    and n_manifiesto = {0} and a_manifiesto = {1}
                                    order by n_contenedor";


                /*string consulta = @"SELECT DISTINCT c.n_manifiesto, c.a_mani
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE b_transmision = 0 AND f_recepcion IS NOT NULL  AND c.c_tipo_bl IS NOT NULL
                                    ORDER BY 2, 1";*/

                SqlCommand _command = new SqlCommand(string.Format(consulta, n_mani, a_mani), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    TransferXML _notificacion = new TransferXML
                    {

                        contenedor = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_llegada = _reader.IsDBNull(1) ? "" : _reader.GetString(1)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }


        }



    }

    public class TransmiDANDAL
    {
        public static List<TransferXML> TransDANR(DBComun.Estado pEstado, string pCondicion, string pTipo)
        {
            List<TransferXML> notiLista = new List<TransferXML>();

            object year = null;
            object aduana = null;
            object manifiesto = null;


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT c.a_mani y_mani, '02' ADUANA, b.n_manifiesto, c.n_contenedor, IdDeta
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE {0}                        
                                    ORDER BY c.n_contenedor";*/

                string consulta = @"SELECT c.a_mani y_mani, '02' ADUANA, b.n_manifiesto, c.n_contenedor, IdDeta, CASE WHEN b.b_siduneawd = 1 THEN 1 ELSE 0 END b_siduneawd, RTRIM(LTRIM(CONVERT(CHAR(10), {0}, 103) +' ' + CONVERT(CHAR(10), {1}, 108)))
                                    FROM CCO_DETA_NAVIERAS a INNER JOIN CCO_ENCA_NAVIERAS d ON a.IdReg = d.IdReg
                                    INNER JOIN CCO_DETA_DOC_NAVI b ON a.IdDoc = b.IdDoc
                                    INNER JOIN CCO_ADUANA_VALID c ON a.n_contenedor = c.n_contenedor AND b.n_manifiesto = c.n_manifiesto
                                    WHERE {2}                         
                                    ORDER BY c.n_contenedor";

                SqlCommand _command = new SqlCommand(string.Format(consulta,pTipo, pTipo, pCondicion), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    if (ArchivoBookingDAL.isNumeric(_reader[0].ToString().TrimEnd().TrimStart()))
                    {
                        year = Convert.ToString(_reader[0]);
                    }
                    else
                    {
                        year = _reader[0];
                    }

                    if (ArchivoBookingDAL.isNumeric(_reader[1].ToString().TrimEnd().TrimStart()))
                    {
                        aduana = Convert.ToString(_reader[1]);
                    }
                    else
                    {
                        aduana = _reader[1];
                    }

                    if (ArchivoBookingDAL.isNumeric(_reader[2].ToString().TrimEnd().TrimStart()))
                    {
                        manifiesto = Convert.ToString(_reader[2]);
                    }
                    else
                    {
                        manifiesto = _reader[2];
                    }

                    TransferXML _notificacion = new TransferXML
                    {
                        year = year.ToString(),
                        aduana = aduana.ToString(),
                        nmanifiesto = manifiesto.ToString(),
                        contenedor = _reader.GetString(3),
                        IdDeta = _reader.GetInt32(4),
                        b_sidunea = _reader.GetInt32(5),
                        f_retencion = _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActTrasmision(int pIdDeta, string pCondicion)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_NAVIERAS
                                    SET {0}
                                    WHERE IdDeta = {1}
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pCondicion, pIdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }
    }

    public class CorteCOTECNADAL
    {

        public static List<CorteCOTECNA> BuquesCorteEnca(DBComun.Estado pEstado, string c_llegada)
        {
            List<CorteCOTECNA> _empleados = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_nul, a.c_llegada, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_atraque
                            FROM fa_llegadas a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND YEAR(a.f_arribo) >= year(getdate())-1 AND a.c_llegada = '{0}'  /*A.f_atraque IS not NULL */
                            ORDER BY a.c_llegada DESC";


                _command = new AseCommand(string.Format(_consulta, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorteCOTECNA _tmpEmpleado = new CorteCOTECNA
                    {
                        c_nul = _reader.GetString(0),
                        c_llegada = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_atraque = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5)

                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }
        public static List<CorteCOTECNA> BuquesCorte(DBComun.Estado pEstado, string c_llegada)
        {
            List<CorteCOTECNA> _empleados = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_nul, a.c_llegada, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_atraque
                            FROM fa_llegadas a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND YEAR(a.f_arribo) >= year(getdate())-1 AND a.c_llegada = '{0}'  /*A.f_atraque IS not NULL */
                            ORDER BY a.c_llegada DESC";


                _command = new AseCommand(string.Format(_consulta, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorteCOTECNA _tmpEmpleado = new CorteCOTECNA
                    {
                        c_nul = _reader.GetString(0),
                        c_llegada = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_atraque = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5)
                        
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<CorteCOTECNA> BuquesCorte(DBComun.Estado pEstado)
        {
            List<CorteCOTECNA> _empleados = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_llegada
                            FROM fa_llegadas a 
                            WHERE a.c_empresa = '04' AND a.f_desatraque is null AND YEAR(a.f_arribo) BETWEEN YEAR(GETDATE()) - 1 AND YEAR(GETDATE()) AND MONTH(a.f_arribo) BETWEEN MONTH(GETDATE()) - 1 AND MONTH(GETDATE()) and a.c_llegada is not null  
                            order by a.f_atraque desc";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorteCOTECNA _tmpEmpleado = new CorteCOTECNA
                    {                        
                        c_llegada = _reader.GetString(0)                       
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }



        public static List<CorteCOTECNA> CorteLlegadas(DBComun.Estado pEstado, string c_llegada)
        {
            List<CorteCOTECNA> notiLista = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT a.c_llegada, a.c_naviera, c.a_manifiesto + CAST(c.n_manifiesto AS VARCHAR(4)) mani, COUNT(b.n_contenedor) Total, d.c_prefijo
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_DOC_NAVI c ON b.IdDoc = c.IdDoc AND a.IdReg = c.IdReg  
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON a.c_naviera = d.c_naviera
                                    WHERE a.c_llegada = '{0}' AND b.b_autorizado = 1 AND c_correlativo IS NOT NULL AND a.b_cotecna = 0
                                    GROUP BY a.c_llegada, a.c_naviera, c.n_manifiesto, c.a_manifiesto, d.c_prefijo
                                    ORDER BY A.c_llegada DESC";*/

                SqlCommand _command = new SqlCommand("PA_COTECNA_ALERTA", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    CorteCOTECNA _notificacion = new CorteCOTECNA
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_cliente = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_manifiesto = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        t_contenedores = _reader.IsDBNull(3) ? 0 : _reader.GetInt32(3),
                        t_dan = _reader.IsDBNull(4) ? 0 : _reader.GetInt32(4),
                        t_dga = _reader.IsDBNull(5) ? 0 : _reader.GetInt32(5),
                        c_prefijo = _reader.IsDBNull(6) ? "" : _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static string ActCOTECNA(string c_llegada, string c_nul)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_ENCA_NAVIERAS
                                    SET b_cotecna = 1, f_cotecna = GETDATE(), c_nul = '{0}'
                                    WHERE c_llegada = '{1}'
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_nul, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActNotiCOTECNA(string c_llegada)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_ENCA_NAVIERAS
                                    SET b_noti_cotecna = 1, f_cotecna = GETDATE()
                                    WHERE c_llegada = '{0}'
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActNotiAUTO(string c_llegada, DBComun.Estado pEstado)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_ENCA_NAVIERAS
                                    SET b_noti_t_auto = 1
                                    WHERE c_llegada = '{0}'
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActLIQUID(string c_llegada)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_ENCA_NAVIERAS
                                    SET b_liquid = 1, f_liquid = GETDATE()
                                    WHERE c_llegada = '{0}'
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_llegada), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActRecep(int IdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_NAVIERAS
                                    SET b_envio = 1, f_envio = GETDATE()
                                    WHERE IdDeta = {0}
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, IdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActRecepPatio(string s_comentarios, int IdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_NAVIERAS
                                    SET f_recepcion = f_rpatio, s_obser_recep = '{0}', b_recepcion = 1
                                    WHERE IdDeta = {1}
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, s_comentarios, IdDeta), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<CorteCOTECNA> CodLlegadasGroup(DBComun.Estado pEstado)
        {
            List<CorteCOTECNA> notiLista = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("pa_cotecna_resumen", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    CorteCOTECNA _notificacion = new CorteCOTECNA
                    {
                        c_llegada = _reader.GetString(0)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<DetaNaviera> CodLlegadasGroupAuto(DBComun.Estado pEstado)
        {
            List<DetaNaviera> notiLista = new List<DetaNaviera>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("pa_consulta_transmi_auto", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    DetaNaviera _notificacion = new DetaNaviera
                    {
                        c_llegada = _reader.GetString(0)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
    }

    public class LiquidAduanaDAL
    {
        public static List<DetaillLiquid> LiquidDetalle(DBComun.Estado pEstado, string c_llegada)
        {
            List<DetaillLiquid> notiLista = new List<DetaillLiquid>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT a.c_llegada, a.c_naviera, c.a_manifiesto + CAST(c.n_manifiesto AS VARCHAR(4)) mani, COUNT(b.n_contenedor) Total, d.c_prefijo
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_DOC_NAVI c ON b.IdDoc = c.IdDoc AND a.IdReg = c.IdReg  
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON a.c_naviera = d.c_naviera
                                    WHERE a.c_llegada = '{0}' AND b.b_autorizado = 1 AND c_correlativo IS NOT NULL AND a.b_cotecna = 0
                                    GROUP BY a.c_llegada, a.c_naviera, c.n_manifiesto, c.a_manifiesto, d.c_prefijo
                                    ORDER BY A.c_llegada DESC";*/

                SqlCommand _command = new SqlCommand("PA_LIQUID_ALERTA_DETALLE", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    DetaillLiquid _notificacion = new DetaillLiquid
                    {
                        c_correlativo = _reader.IsDBNull(0) ? 0 : _reader.GetInt32(0),
                        a_manifiesto = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_manifiesto = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        d_cliente = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        n_contenedor = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_naviera = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_llegada = _reader.IsDBNull(6) ? "" : _reader.GetString(6)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
        public static List<LiquidADUANA> LiquidResumen(DBComun.Estado pEstado, string c_llegada)
        {
            List<LiquidADUANA> notiLista = new List<LiquidADUANA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                /*string consulta = @"SELECT a.c_llegada, a.c_naviera, c.a_manifiesto + CAST(c.n_manifiesto AS VARCHAR(4)) mani, COUNT(b.n_contenedor) Total, d.c_prefijo
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_DOC_NAVI c ON b.IdDoc = c.IdDoc AND a.IdReg = c.IdReg  
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON a.c_naviera = d.c_naviera
                                    WHERE a.c_llegada = '{0}' AND b.b_autorizado = 1 AND c_correlativo IS NOT NULL AND a.b_cotecna = 0
                                    GROUP BY a.c_llegada, a.c_naviera, c.n_manifiesto, c.a_manifiesto, d.c_prefijo
                                    ORDER BY A.c_llegada DESC";*/

                SqlCommand _command = new SqlCommand("PA_LIQUID_ALERTA", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    LiquidADUANA _notificacion = new LiquidADUANA
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_cliente = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        t_manifestados = _reader.IsDBNull(2) ? 0 : _reader.GetInt32(2),
                        t_recibidos = _reader.IsDBNull(3) ? 0 : _reader.GetInt32(3),
                        t_cancelados = _reader.IsDBNull(4) ? 0 : _reader.GetInt32(4),
                        c_prefijo = _reader.IsDBNull(5) ? "" : _reader.GetString(5)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<EncaLiquid> EncaLiquidADUANA(DBComun.Estado pEstado, string c_llegada)
        {
            List<EncaLiquid> _empleados = new List<EncaLiquid>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT DISTINCT a.c_llegada, b.s_nom_buque, a.f_atraque, a.f_desatraque, a.c_nul, b.c_imo
                            FROM fa_llegadas a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            WHERE a.c_empresa = '04' AND YEAR(a.f_arribo) >= year(getdate())-1 AND a.c_llegada = '{0}' AND a.f_desatraque IS not NULL 
                            ORDER BY a.c_llegada DESC";


                _command = new AseCommand(string.Format(_consulta, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaLiquid _tmpEmpleado = new EncaLiquid
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        n_buque = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        f_atraque = _reader.IsDBNull(2) ? Convert.ToDateTime(_reader.GetDateTime(2)) : _reader.GetDateTime(2),
                        f_desatraque = _reader.IsDBNull(3) ? Convert.ToDateTime(_reader.GetDateTime(3)) : _reader.GetDateTime(3),
                        c_null = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_imo = _reader.IsDBNull(5) ? "" : _reader.GetString(5)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaLiquid> EncaLiquidSADFI(DBComun.Estado pEstado, string c_llegada)
        {
            List<EncaLiquid> _empleados = new List<EncaLiquid>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_llegada, b.s_nom_buque, a.f_atraque, a.f_desatraque, a.c_nul, b.c_imo, d.c_cliente, c.s_razon_social
                            FROM fa_llegadas a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND YEAR(a.f_arribo) >= year(getdate())-1 AND a.c_llegada = '{0}' AND a.f_desatraque IS not NULL 
                            ORDER BY a.c_llegada DESC";


                _command = new AseCommand(string.Format(_consulta, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaLiquid _tmpEmpleado = new EncaLiquid
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        n_buque = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        f_atraque = _reader.IsDBNull(2) ? Convert.ToDateTime(_reader.GetDateTime(2)) : _reader.GetDateTime(2),
                        f_desatraque = _reader.IsDBNull(3) ? Convert.ToDateTime(_reader.GetDateTime(3)) : _reader.GetDateTime(3),
                        c_null = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_imo = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_cliente = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        d_cliente = _reader.IsDBNull(7) ? "" : _reader.GetString(7)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaLiquid> CodLlegadasLiquid(DBComun.Estado pEstado)
        {
            List<EncaLiquid> notiLista = new List<EncaLiquid>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT a.c_llegada
                                    FROM CCO_ENCA_NAVIERAS a INNER JOIN CCO_DETA_NAVIERAS b ON a.IdReg = b.IdReg
                                    INNER JOIN CCO_DETA_DOC_NAVI c ON b.IdDoc = c.IdDoc AND a.IdReg = c.IdReg  
                                    INNER JOIN CCO_USUARIOS_NAVIERAS d ON a.c_naviera = d.c_naviera 
                                    WHERE b.b_autorizado = 1 AND c_correlativo IS NOT NULL AND a.b_liquid = 0
                                    GROUP BY a.c_llegada
                                    ORDER BY A.c_llegada DESC";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {


                    EncaLiquid _notificacion = new EncaLiquid
                    {
                        c_llegada = _reader.GetString(0)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }
    }

    public class AlertaDanTarjaDAL
    {
        public static List<AlertaDANTarja> ObtenerTarjas(DBComun.Estado pTipo, string pParametro)
        {
            List<AlertaDANTarja> _empleados = new List<AlertaDANTarja>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"select c_tarja, '{0}' from fa_bl_det where d_clase_contenido like '%{1}%' and year(f_registro)>=YEAR(GETDATE())
                            union all
                            select ISNULL(c_tarja, '') c_tarja, '{2}' from fa_manifiestos where d_remarcas like '%{3}%' and year(f_tarja)>=YEAR(GETDATE())
                            union all
                            select ISNULL(c_tarja, '') c_tarja, '{4}' from fa_manifiestos where d_detalle like '%{5}%' and year(f_tarja)>=YEAR(GETDATE())";

                _command = new AseCommand(string.Format(_consulta, pParametro, pParametro, pParametro, pParametro, pParametro, pParametro), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    AlertaDANTarja _alerta = new AlertaDANTarja
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_parametro = _reader.IsDBNull(1) ? "" : _reader.GetString(1)
                    };

                    _empleados.Add(_alerta);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }
    }

    public class TarjasNotificadasDAL
    {
        public static List<TarjaNotificada> ObtenerTarjasNoti(DBComun.Estado pTipo)
        {
            List<TarjaNotificada> _empleados = new List<TarjaNotificada>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                SqlCommand _command = null;

                _consulta = @"SELECT c_tarja, c_parametro, y_parametro
                            FROM CCO_TARJAS_NOTIFICADAS
                            WHERE y_parametro = YEAR(GETDATE()) ";

                _command = new SqlCommand(_consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TarjaNotificada _alerta = new TarjaNotificada
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_parametro = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        y_parametro = _reader.IsDBNull(2) ? 0 : _reader.GetInt32(2)
                    };

                    _empleados.Add(_alerta);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static string AlmacenarTarjas(TarjaNotificada pTarjas, DBComun.Estado pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_valid_tarjas", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };


                _command.Parameters.Add(new SqlParameter("@c_tarja", pTarjas.c_tarja));
                _command.Parameters.Add(new SqlParameter("@c_parametro", pTarjas.c_parametro));



                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;
            }
        }

    }

    public class ParametroAlertaDAL
    {
        public static List<ParametroAlerta> ObtenerPara(DBComun.Estado pTipo)
        {
            List<ParametroAlerta> _empleados = new List<ParametroAlerta>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                SqlCommand _command = null;

                _consulta = @"SELECT Parametro
                            FROM CCO_PARAMETRO_ALERTA
                            WHERE Habilitado = 1";

                _command = new SqlCommand(_consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ParametroAlerta _alerta = new ParametroAlerta
                    {

                        c_parametro = _reader.IsDBNull(0) ? "" : _reader.GetString(0)

                    };

                    _empleados.Add(_alerta);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }
    }

}
