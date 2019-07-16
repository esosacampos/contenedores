using CEPA.CCO.Entidades;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace CEPA.CCO.DAL
{
    public class EncaBuqueDAL
    {
        public static List<EncaBuque> ObtenerBuques(DBComun.Estado pTipo, string c_cliente)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pTipo))
            {


                _conn.Open();

                string _consulta = null;
                AseCommand _command = null;
                if (c_cliente == "219" || c_cliente == "289")
                {
                    //                    _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '')
                    //                                FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                    //                                INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                    //                                INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                    //                                WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND YEAR(a.f_llegada) >= 2013 AND MONTH(a.f_llegada) >=  MONTH(GETDATE()) AND 
                    //                                a.c_llegada NOT IN (SELECT g.c_llegada FROM fa_llegadas g INNER JOIN fa_tarifa_unica e ON g.c_llegada = e.c_llegada WHERE e.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND g.f_atraque IS NOT NULL AND g.f_desatraque IS NOT NULL )  
                    //                                ORDER BY f_llegada DESC";
                    _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '')
                                FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                                INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                                INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                                WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND a.f_llegada >= DATEADD( dd, -30, getdate()) AND 
                                a.c_llegada NOT IN (SELECT g.c_llegada FROM fa_llegadas g INNER JOIN fa_tarifa_unica e ON g.c_llegada = e.c_llegada WHERE g.f_atraque IS NOT NULL AND g.f_desatraque IS NOT NULL )  
                                ORDER BY f_llegada DESC";
                    _command = new AseCommand(_consulta, _conn as AseConnection);

                }
                else
                {
                    _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '')
                                FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                                INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                                INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                                WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente = '{0}' AND a.f_llegada >= DATEADD( dd, -30, getdate()) AND 
                                a.c_llegada NOT IN (SELECT g.c_llegada FROM fa_llegadas g INNER JOIN fa_tarifa_unica e ON g.c_llegada = e.c_llegada WHERE e.c_cliente = '{1}' AND g.f_atraque IS NOT NULL AND g.f_desatraque IS NOT NULL )  
                                ORDER BY f_llegada DESC";


                    _command = new AseCommand(string.Format(_consulta, c_cliente, c_cliente), _conn as AseConnection);
                }

                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = Convert.ToDateTime(_reader.GetDateTime(5), CultureInfo.CreateSpecificCulture("es-SV")),
                        c_imo = _reader.GetString(6),
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }



        public static List<EncaBuque> ObtenerBuquesID(DBComun.Estado pTipo, string c_cliente, string c_buque, string c_llegada)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;
                if (c_cliente == "219")
                {

                    _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '')
                                FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                                INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                                INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                                WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' /*AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388')*/ AND b.c_buque = '{0}' AND a.c_llegada = '{1}' AND YEAR(a.f_llegada) >= 2016 AND 
                                a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE /*c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND*/ f_atraque IS NOT NULL AND f_desatraque IS NOT NULL)  
                                ORDER BY f_llegada DESC";
                    _command = new AseCommand(string.Format(_consulta, c_buque, c_llegada), _conn as AseConnection);
                }
                else
                {
                    _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '')
                                FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                                INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                                INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                                WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND d.c_cliente = '{0}' AND a.c_llegada = '{1}' AND b.c_buque = '{2}' AND YEAR(a.f_llegada) >= 2016 /*AND 
                                a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente = '{3}' AND f_atraque IS NOT NULL AND f_desatraque IS NOT NULL)  */
                                ORDER BY f_llegada DESC";
                    _command = new AseCommand(string.Format(_consulta, c_cliente, c_llegada, c_buque, c_cliente), _conn as AseConnection);
                }


                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerNaviera(DBComun.Estado pTipo, string c_cliente)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pTipo))
            {
                _conn.Open();
                string _consulta = @"SELECT s_razon_social FROM cn_cliente WHERE c_cliente = '{0}'";


                AseCommand _command = new AseCommand(string.Format(_consulta, c_cliente), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        d_cliente = _reader.GetString(0)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerBuquesJoin(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                //                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                //                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                //                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                //                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                //                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND YEAR(a.f_llegada) >= YEAR(GETDATE()) AND MONTH(a.f_llegada) >= MONTH(GETDATE()) /*AND 
                //                            a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND f_desatraque IS NOT NULL)*/
                //                            ORDER BY f_llegada DESC";

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 /*AND MONTH(a.f_llegada) >= MONTH(GETDATE())*/
                            ORDER BY f_llegada DESC";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerBuquesJoin(DBComun.Estado pEstado, string s_atraque)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                //                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                //                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                //                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                //                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                //                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND YEAR(a.f_llegada) >= YEAR(GETDATE()) AND MONTH(a.f_llegada) >= MONTH(GETDATE()) /*AND 
                //                            a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND f_desatraque IS NOT NULL)*/
                //                            ORDER BY f_llegada DESC";

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, ISNULL((select f_atraque from fa_llegadas where c_llegada = a.c_llegada), ''), ISNULL(b.c_imo, '') c_imo
                        FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                        INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                        INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                        WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 
                        /*AND MONTH(a.f_llegada) >= MONTH(GETDATE())*/ 
                        ORDER BY 5 DESC";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerBuquesJoinIN(DBComun.Estado pEstado, string s_atraque, List<string> c_llegada)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = new AseCommand();
                _command.CommandType = CommandType.Text;

                //                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                //                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                //                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                //                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                //                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND YEAR(a.f_llegada) >= YEAR(GETDATE()) AND MONTH(a.f_llegada) >= MONTH(GETDATE()) /*AND 
                //                            a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND f_desatraque IS NOT NULL)*/
                //                            ORDER BY f_llegada DESC";

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, ISNULL((select f_atraque from fa_llegadas where c_llegada = a.c_llegada), ''), ISNULL(b.c_imo, '') c_imo
                        FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                        INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                        INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                        WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 
                        AND a.c_llegada IN({0})
                        /*AND MONTH(a.f_llegada) >= MONTH(GETDATE())*/ 
                        ORDER BY 5 DESC";

                string parameterPrefix = "c_llegada";

                _consulta = SqlWhereIn.BuildWhereInClause(_consulta, parameterPrefix, c_llegada);


                _command = new AseCommand(_consulta, _conn as AseConnection);
                _command.AddParamsToCommand(parameterPrefix, c_llegada);




                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }
        public static List<EncaBuque> ObtenerBuquesTransmi(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                //                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                //                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                //                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                //                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                //                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND YEAR(a.f_llegada) >= YEAR(GETDATE()) AND MONTH(a.f_llegada) >= MONTH(GETDATE()) /*AND 
                //                            a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND f_desatraque IS NOT NULL)*/
                //                            ORDER BY f_llegada DESC";

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 /*AND MONTH(a.f_llegada) >= MONTH(GETDATE())*/
                            ORDER BY f_llegada DESC";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerBuquesJoinA(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                //                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                //                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                //                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                //                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                //                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND YEAR(a.f_llegada) >= YEAR(GETDATE()) AND MONTH(a.f_llegada) >= MONTH(GETDATE()) /*AND 
                //                            a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente IN('884', '1204', '449', '690', '809', '354', '453', '882', '1241', '1388') AND f_desatraque IS NOT NULL)*/
                //                            ORDER BY f_llegada DESC";

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 --AND MONTH(a.f_llegada) >= MONTH(GETDATE())
                            ORDER BY f_llegada DESC";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }
        public static List<EncaBuque> ObtenerBuquesJoinC(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;



                _consulta = @" SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 /*AND MONTH(a.f_llegada) >= MONTH(GETDATE()) */ ";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerBuquesJoinOf(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2014 /*AND MONTH(a.f_llegada) >= MONTH(GETDATE())*/ 
                            ORDER BY f_llegada DESC";

                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }


        public static List<Naviera> ObtenerNavieras(DBComun.Estado pTipo)
        {
            List<Naviera> _empleados = new List<Naviera>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT c_cliente, s_razon_social FROM cn_cliente  
                            WHERE c_cliente IN('219', '289', '354', '449', '453', '690',  '809', '882', '884', '1204', '1241', '1388', '11')  
                            ORDER BY c_cliente DESC";
                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Naviera _tmpEmpleado = new Naviera
                    {
                        c_cliente = _reader.GetString(0),
                        d_nombre = _reader.GetString(1)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Naviera> ObtenerNavieras1(DBComun.Estado pTipo)
        {
            List<Naviera> _empleados = new List<Naviera>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT c_cliente, s_razon_social FROM cn_cliente where s_razon_social <> ''";

                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Naviera _tmpEmpleado = new Naviera
                    {
                        c_cliente = _reader.GetString(0),
                        d_nombre = _reader.GetString(1)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerBuquesJoin(DBComun.Estado pEstado, string c_cliente, string c_llegada)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada, ISNULL(b.c_imo, '') c_imo
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND a.c_llegada = '{0}' AND c.c_cliente = '{1}' AND 
                            a.c_llegada NOT IN (SELECT c_llegada FROM fa_llegadas WHERE c_cliente = '{2}' AND f_atraque IS NOT NULL AND f_desatraque IS NOT NULL)  
                            ORDER BY f_llegada DESC";


                _command = new AseCommand(string.Format(_consulta, c_llegada, c_cliente, c_cliente), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Facturacion> ObtenerTarjas()
        {
            List<Facturacion> _empleados = new List<Facturacion>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"select a.c_llegada, c.c_tarja, c.d_remarcas
                            from fa_enc_manifies a 
                            inner join fa_manifiestos b on a.c_manifiesto = b.c_manifiesto 
                            inner join fa_bl_det c on b.c_tarja = c.c_tarja
                            where a.c_llegada = '4.6833' and a.b_mani_embar='2'";

                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Facturacion _tmpEmpleado = new Facturacion
                    {
                        c_llegada = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_tarja = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }


        public static List<Tarjas> TarjasLlegada(string c_llegada, string n_contenedor, string b_dif)
        {
            List<Tarjas> _empleados = new List<Tarjas>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"set nocount on
                            declare @c_llegada varchar(20), @c_contenedor varchar(11)
                            set @c_llegada = '{0}'
                            set @c_contenedor = '{1}'


                            select  fa_manifiestos.c_tarja , fa_bl_det.d_remarcas,fa_tarifa_unica.c_llegada, fa_manifiestos.f_tarja, fa_bl_det.c_contenedor

                            from fa_manifiestos , fa_bl_det, fa_producto , fa_enc_manifies , fa_tarifa_unica
                            where fa_manifiestos.c_tarja = fa_bl_det.c_tarja
                            and fa_bl_det.c_producto = fa_producto.c_producto

                            and fa_tarifa_unica.c_cliente = fa_manifiestos.c_agencia 
                            and fa_enc_manifies.c_manifiesto = fa_manifiestos.c_manifiesto
                            and fa_enc_manifies.c_llegada = fa_tarifa_unica.c_llegada 
                            and fa_tarifa_unica.c_llegada =  @c_llegada and fa_manifiestos.f_tarja is not null
                            and fa_bl_det.c_contenedor = @c_contenedor";

                _command = new AseCommand(string.Format(_consulta, c_llegada, n_contenedor), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Tarjas _tmpEmpleado = new Tarjas
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        d_marcas = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = _reader.IsDBNull(3) ? Convert.ToDateTime(_reader.GetDateTime(3)) : _reader.GetDateTime(3),
                        c_contenedor = _reader.IsDBNull(4) ? "" : _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Tarjas> TarjasLlegada(string c_llegada)
        {
            List<Tarjas> _empleados = new List<Tarjas>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"set nocount on
                            declare @c_llegada varchar(20)
                            set @c_llegada = '{0}'                           


                            select  fa_manifiestos.c_tarja , fa_bl_det.d_remarcas,fa_tarifa_unica.c_llegada, fa_manifiestos.f_tarja, fa_bl_det.c_contenedor

                            from fa_manifiestos , fa_bl_det, fa_producto , fa_enc_manifies , fa_tarifa_unica
                            where fa_manifiestos.c_tarja = fa_bl_det.c_tarja
                            and fa_bl_det.c_producto = fa_producto.c_producto

                            and fa_tarifa_unica.c_cliente = fa_manifiestos.c_agencia 
                            and fa_enc_manifies.c_manifiesto = fa_manifiestos.c_manifiesto
                            and fa_enc_manifies.c_llegada = fa_tarifa_unica.c_llegada 
                            and fa_tarifa_unica.c_llegada =  @c_llegada and fa_manifiestos.f_tarja is not null
                            ";

                _command = new AseCommand(string.Format(_consulta, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Tarjas _tmpEmpleado = new Tarjas
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        d_marcas = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = _reader.IsDBNull(3) ? Convert.ToDateTime(_reader.GetDateTime(3)) : _reader.GetDateTime(3),
                        c_contenedor = _reader.IsDBNull(4) ? "" : _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Tarjas> TarjasLlegada(string c_llegada, string n_contenedor)
        {
            List<Tarjas> _empleados = new List<Tarjas>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                /*_consulta = @"set nocount on
                            declare @c_llegada varchar(20), @n_contenedor varchar(20)
                            set @c_llegada = '{0}'
                            set @n_contenedor = '{1}'                          


                            select max(b.c_tarja) c_tarja, c.c_contenedor, a.c_llegada, b.f_tarja, sum(c.v_pes_rec_ton) as v_pesotm, c.d_clase_contenido
                            from fa_enc_manifies a, fa_manifiestos b, fa_bl_det c 
                            where a.c_manifiesto=b.c_manifiesto and
                                                b.c_tarja=c.c_tarja and 
                                  b.b_estado='E' and a.b_mani_embar='2' and a.c_llegada=@c_llegada and c.d_remarcas like '%' + @n_contenedor + '%'
                            group by c.c_contenedor, a.c_llegada, b.f_tarja, c.d_clase_contenido";*/

                _consulta = @"set nocount on
                            select a.c_tarja, b.c_llegada, v_pes_rec_ton
                            from fa_bl_det a, fa_enc_manifies b, fa_manifiestos c
                            where  b.c_manifiesto=c.c_manifiesto and
                            c.c_tarja=a.c_tarja /*and c.b_estado='E'*/ and b.b_mani_embar='2' and
                            rtrim(ltrim(c_contenedor)) = '{0}' and b.c_llegada = '{1}'";

                _command = new AseCommand(string.Format(_consulta, n_contenedor, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Tarjas _tmpEmpleado = new Tarjas
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_llegada = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        /*d_marcas = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = (DateTime)_reader.GetDateTime(3),*/
                        v_peso = _reader.IsDBNull(2) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(2))
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Tarjas> TarjasDetalle(string c_tarja, string c_llegada)
        {
            List<Tarjas> _empleados = new List<Tarjas>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                /*_consulta = @"set nocount on
                            declare @c_llegada varchar(20), @n_contenedor varchar(20)
                            set @c_llegada = '{0}'
                            set @n_contenedor = '{1}'                          


                            select max(b.c_tarja) c_tarja, c.c_contenedor, a.c_llegada, b.f_tarja, sum(c.v_pes_rec_ton) as v_pesotm, c.d_clase_contenido
                            from fa_enc_manifies a, fa_manifiestos b, fa_bl_det c 
                            where a.c_manifiesto=b.c_manifiesto and
                                                b.c_tarja=c.c_tarja and 
                                  b.b_estado='E' and a.b_mani_embar='2' and a.c_llegada=@c_llegada and c.d_remarcas like '%' + @n_contenedor + '%'
                            group by c.c_contenedor, a.c_llegada, b.f_tarja, c.d_clase_contenido";*/

                _consulta = @"set nocount on
                            select a.c_tarja, a.c_contenedor, a.d_clase_contenido, c.f_tarja, b.c_llegada from fa_bl_det a, fa_enc_manifies b, fa_manifiestos c
                            where  b.c_manifiesto=c.c_manifiesto and
                            c.c_tarja=a.c_tarja /*and c.b_estado='E'*/ and b.b_mani_embar='2' and a.c_tarja = '{0}' and c_llegada = '{1}'";

                _command = new AseCommand(string.Format(_consulta, c_tarja, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Tarjas _tmpEmpleado = new Tarjas
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        d_marcas = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        s_descripcion = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = _reader.GetDateTime(3),
                        c_llegada = _reader.IsDBNull(4) ? "" : _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Tarjas> TarjasDetalle(string c_tarja)
        {
            List<Tarjas> _empleados = new List<Tarjas>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                /*_consulta = @"set nocount on
                            declare @c_llegada varchar(20), @n_contenedor varchar(20)
                            set @c_llegada = '{0}'
                            set @n_contenedor = '{1}'                          


                            select max(b.c_tarja) c_tarja, c.c_contenedor, a.c_llegada, b.f_tarja, sum(c.v_pes_rec_ton) as v_pesotm, c.d_clase_contenido
                            from fa_enc_manifies a, fa_manifiestos b, fa_bl_det c 
                            where a.c_manifiesto=b.c_manifiesto and
                                                b.c_tarja=c.c_tarja and 
                                  b.b_estado='E' and a.b_mani_embar='2' and a.c_llegada=@c_llegada and c.d_remarcas like '%' + @n_contenedor + '%'
                            group by c.c_contenedor, a.c_llegada, b.f_tarja, c.d_clase_contenido";*/

                _consulta = @"set nocount on
                            select a.c_tarja, a.c_contenedor, a.d_clase_contenido, a.f_registro, b.c_llegada from fa_bl_det a, fa_enc_manifies b, fa_manifiestos c
                            where  b.c_manifiesto=c.c_manifiesto and
                            c.c_tarja=a.c_tarja /*and c.b_estado='E'*/ and b.b_mani_embar='2' and a.c_tarja = '{0}'";

                _command = new AseCommand(string.Format(_consulta, c_tarja), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Tarjas _tmpEmpleado = new Tarjas
                    {
                        c_tarja = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        d_marcas = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        s_descripcion = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        f_tarja = _reader.GetDateTime(3),
                        c_llegada = _reader.IsDBNull(4) ? "" : _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static double TarjasPeso(string c_tarja)
        {
            List<Tarjas> _empleados = new List<Tarjas>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                /*_consulta = @"set nocount on
                            declare @c_llegada varchar(20), @n_contenedor varchar(20)
                            set @c_llegada = '{0}'
                            set @n_contenedor = '{1}'                          


                            select max(b.c_tarja) c_tarja, c.c_contenedor, a.c_llegada, b.f_tarja, sum(c.v_pes_rec_ton) as v_pesotm, c.d_clase_contenido
                            from fa_enc_manifies a, fa_manifiestos b, fa_bl_det c 
                            where a.c_manifiesto=b.c_manifiesto and
                                                b.c_tarja=c.c_tarja and 
                                  b.b_estado='E' and a.b_mani_embar='2' and a.c_llegada=@c_llegada and c.d_remarcas like '%' + @n_contenedor + '%'
                            group by c.c_contenedor, a.c_llegada, b.f_tarja, c.d_clase_contenido";*/

                _consulta = @"set nocount on
                            select case when sum(v_pes_rec_ton) > 0.00 then cast(ROUND(sum(v_pes_rec_ton)/MAX(v_n_contenedores), 2) as decimal(18,2)) else sum(v_pes_rec_ton) end v_pes_rec_ton from fa_bl_det where c_tarja = '{0}'";

                _command = new AseCommand(string.Format(_consulta, c_tarja), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                double vPeso = Convert.ToDouble(_command.ExecuteScalar().ToString());

                _conn.Close();
                return vPeso;
            }
        }

        public static List<CorteCOTECNA> BuquesZarpe(DBComun.Estado pEstado)
        {
            List<CorteCOTECNA> _empleados = new List<CorteCOTECNA>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"SELECT a.c_llegada, b.s_nom_buque, d.c_cliente, c.s_razon_social, a.f_llegada
                            FROM fa_llegadas a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                            INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_arribo) >= 2016 AND a.f_zarpe IS not NULL 
                            ORDER BY a.c_llegada DESC";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    CorteCOTECNA _tmpEmpleado = new CorteCOTECNA
                    {
                        c_llegada = _reader.GetString(0),
                        d_buque = _reader.GetString(1),
                        c_cliente = _reader.GetString(2),
                        d_cliente = _reader.GetString(3),
                        f_atraque = _reader.GetDateTime(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> showShipping(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"select a.c_llegada, b.s_nom_buque
                            from fa_llegadas a, fa_buques b
                            where a.c_buque=b.c_buque and a.c_empresa='04' and a.c_operadora<>'14' and a.f_desatraque is null and year(a.f_atraque)>=2016 --in c_llegada (
                            order by a.f_atraque desc";

                /*_consulta = @"select a.c_llegada, b.s_nom_buque
                            from fa_aviso_lleg a, fa_buques b
                            where a.c_buque=b.c_buque and a.c_empresa='04' and isnull(a.c_operadora, '') <>'14' and c_llegada = '4.11608'";*/



                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        d_buque = _reader.GetString(1)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> ObtenerShipping(DBComun.Estado pEstado)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;



                _consulta = @" SELECT a.c_llegada, b.c_buque, b.s_nom_buque
                            FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                            WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND YEAR(a.f_llegada) >= 2016 
                            ORDER BY 1 desc /*AND MONTH(a.f_llegada) >= MONTH(GETDATE()) */ ";


                _command = new AseCommand(_consulta, _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> getNaviera(DBComun.Estado pEstado, string c_naviera)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;



                _consulta = @"select s_razon_social from cn_cliente where c_cliente = '{0}'";


                _command = new AseCommand(string.Format(_consulta, c_naviera), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        d_cliente = _reader.IsDBNull(0) ? "" : _reader.GetString(0)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<EncaBuque> getHeaderLleg(DBComun.Estado pEstado, string c_cliente, string c_llegada)
        {
            List<EncaBuque> _empleados = new List<EncaBuque>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;



                _consulta = @" SELECT a.c_llegada, b.c_buque, b.s_nom_buque, d.c_cliente, c.s_razon_social,                    a.f_llegada, ISNULL(b.c_imo, '') c_imo
                                FROM fa_aviso_lleg a INNER JOIN fa_buques b ON a.c_buque = b.c_buque
                                INNER JOIN fa_tarifa_unica d ON a.c_llegada = d.c_llegada 
                                INNER JOIN cn_cliente c ON d.c_cliente = c.c_cliente
                                WHERE a.c_empresa = '04' AND b.c_tip_buque = '4' AND c.c_cliente = '{0}' AND a.c_llegada = '{1}'   
                                ORDER BY f_llegada DESC ";


                _command = new AseCommand(string.Format(_consulta, c_cliente, c_llegada), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EncaBuque _tmpEmpleado = new EncaBuque
                    {
                        c_llegada = _reader.GetString(0),
                        c_buque = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        c_cliente = _reader.GetString(3),
                        d_cliente = _reader.GetString(4),
                        f_llegada = _reader.IsDBNull(5) ? Convert.ToDateTime(_reader.GetDateTime(5)) : _reader.GetDateTime(5),
                        c_imo = _reader.GetString(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static int getDocValid(DBComun.Estado pEstado, string c_factura, string c_cliente)
        {
            List<Documentos> _empleados = new List<Documentos>();

            int valid = 0;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;               

                _consulta = @"select count(*) 
                            from fa_enc_factura 
                            where c_empresa='04' and c_factura = '{0}' and c_cliente='{1}' and m_factura_estado<>'A'";


                _command = new AseCommand(string.Format(_consulta, c_factura, c_cliente), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };               

                valid = Convert.ToInt32(_command.ExecuteScalar());
                
                _conn.Close();
                return valid;
            }
        }


        public static List<Documentos> getListDocuments(DBComun.Estado pEstado, string c_factura, string c_cliente)
        {
            List<Documentos> _empleados = new List<Documentos>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;



                _consulta = @"select c_contenedor as contenedor, Left(CONVERT(VARCHAR,Convert(money, cast(v_tara as int)),1), 5) as tara, b_20, b_40, b_45, b_fcl, b_lcl, b_vac, b_trans, c_tarifa as tarifa, datediff(dd, f_ini_cobro, f_fin_cobro)+1 as real, cast(v_dias_20 as int) as c_20, cast(v_dias_4045 as int) as c_40, cast(v_dias_45 as int) as c_45, cast(v_dias_48 as int) as c_48, a.c_nul, s_nom_buque, convert(varchar(10), f_ini_cobro, 105) as ingreso, convert(varchar(10), f_fin_cobro, 105) as despacho, d.v_valor, s_nombre_comercial, d.s_nom_esp
                from fa_conte_arch a inner join fa_llegadas b on a.c_nul = b.c_nul
                inner join fa_buques c on b.c_buque = c.c_buque
                inner join fa_cat_servicios d on a.c_tarifa = d.c_servicio 
                inner join cn_cliente e on a.c_agencia = e.c_cliente
                where c_factura = '{0}' and b_facturado='1' and a.c_empresa='04' and a.c_agencia = '{1}'
                order by f_ini_cobro, c_contenedor";


                _command = new AseCommand(string.Format(_consulta, c_factura, c_cliente), _conn as AseConnection)
                {
                    CommandType = CommandType.Text
                };

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    
                    Documentos _doc = new Documentos
                    {
                        n_contenedor = _reader.GetString(0),
                        v_tara = _reader.GetString(1),
                        b_20 = _reader.GetInt32(2),
                        b_40 = _reader.GetInt32(3),
                        b_45 = _reader.GetInt32(4),                        
                        b_fcl = _reader.GetInt32(5),
                        b_lcl = _reader.GetInt32(6),
                        b_vac = _reader.GetInt32(7),
                        b_trans = _reader.GetInt32(8),
                        c_tarifa =_reader.GetString(9),
                        real = _reader.GetInt32(10),
                        c_20 = _reader.GetInt32(11),
                        c_40 = _reader.GetInt32(12),
                        c_45 = _reader.GetInt32(13),
                        c_48 = _reader.GetInt32(14),
                        c_nul = _reader.GetString(15),
                        d_buque = _reader.GetString(16),
                        f_ingreso = _reader.GetString(17),
                        f_despacho = _reader.GetString(18),
                        v_valor = _reader.IsDBNull(19) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(19)),
                        d_cliente = _reader.GetString(20),
                        d_servicio = _reader.GetString(21)

                    };

                    _empleados.Add(_doc);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

    }
}
