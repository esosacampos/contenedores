using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.DAL
{
    public class DetaDocDAL
    {
        public static string AlmacenarDocNavi(DetaDoc _doc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_doc_navi", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdReg", _doc.IdReg));
                _command.Parameters.Add(new SqlParameter("@c_imo", _doc.c_imo));
                _command.Parameters.Add(new SqlParameter("@s_archivo", _doc.s_archivo));
                _command.Parameters.Add(new SqlParameter("@c_usuario", _doc.c_usuario));
                _command.Parameters.Add(new SqlParameter("@c_llegada", _doc.c_llegada));
                _command.Parameters.Add(new SqlParameter("@n_manifiesto", _doc.num_manif));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", _doc.a_manif));
                _command.Parameters.Add(new SqlParameter("@b_siduneawd", _doc.b_siduneawd));
                /*_command.Parameters.Add(new SqlParameter("@n_manifiesto", _doc.a_manif));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", _doc.a_manifiesto));   */

               

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string AlmacenarDocNaviEx(DetaDoc _doc)
        {
            try
            {
                using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
                {
                    _conn.Open();
                    SqlCommand _command = new SqlCommand("pa_deta_doc_exp_navi", _conn as SqlConnection);
                    _command.CommandType = CommandType.StoredProcedure;

                    _command.Parameters.Add(new SqlParameter("@IdReg", _doc.IdReg));
                    _command.Parameters.Add(new SqlParameter("@c_imo", _doc.c_imo));
                    _command.Parameters.Add(new SqlParameter("@s_archivo", _doc.s_archivo));
                    _command.Parameters.Add(new SqlParameter("@c_usuario", _doc.c_usuario));
                    _command.Parameters.Add(new SqlParameter("@c_llegada", _doc.c_llegada));

                    /*_command.Parameters.Add(new SqlParameter("@n_manifiesto", _doc.a_manif));
                    _command.Parameters.Add(new SqlParameter("@a_manifiesto", _doc.a_manifiesto));   */



                    string resultado = _command.ExecuteScalar().ToString();
                    _conn.Close();
                    return resultado;

                }
            }
            catch(SqlException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }

        }
        
        public static string ArchivosAlmacenados(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*) FROM CCO_DETA_DOC_NAVI a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg WHERE a.c_imo = '{0}' AND a.c_llegada = '{1}' AND c_naviera = '{2}' AND a.IdTipoMov = 1";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string archReestiba(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*) FROM CCO_DETA_DOC_NAVI a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg WHERE a.c_imo = '{0}' AND a.c_llegada = '{1}' AND c_naviera = '{2}' AND a.IdTipoMov = 2";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ArchivosAlmacenadosEx(string c_imo, string c_llegada, string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*) FROM CCO_DETA_DOC_EXP_NAVI a INNER JOIN CCO_ENCA_EXPO_NAVI b ON a.IdReg = b.IdReg WHERE a.c_imo = '{0}' AND a.c_llegada = '{1}' AND c_naviera = '{2}'";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_imo, c_llegada, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static int ObtenerCorrelativoDoc(int pId, int pIdTipoMov)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"DECLARE @i AS INT
                                    SET @i = (SELECT MAX(LEFT(RIGHT(s_archivo, 7),2)) FROM CCO_DETA_DOC_NAVI WHERE IdReg = {0} AND IdTipoMov = {1})
                                    SELECT LTRIM(RTRIM(@i))";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pId, pIdTipoMov), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                int resultado = Convert.ToInt32(_command.ExecuteScalar().ToString());
                _conn.Close();
                return resultado;

            }
        }

        public static int ObtenerCorrelativoDocEx(int pId)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"DECLARE @i AS INT
                                    SET @i = (SELECT MAX(LEFT(RIGHT(s_archivo, 7),2)) FROM CCO_DETA_DOC_EXP_NAVI WHERE IdReg = {0})
                                    SELECT @i";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pId), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                int resultado = Convert.ToInt32(_command.ExecuteScalar().ToString());
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaDoc> ObtenerDoc(DBComun.Estado pTipo, string c_naviera)
        {
            List<DetaDoc> _empleados = new List<DetaDoc>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_NAV_DOCUMENTOS", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@c_naviera", c_naviera));                
               

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDoc _tmpEmpleado = new DetaDoc
                    {
                        IdDoc = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_imo = _reader.GetString(2),
                        s_archivo = _reader.GetString(3),
                        c_llegada = _reader.GetString(4),
                        c_naviera = _reader.GetString(5),
                        IdTipoMov = (int)_reader.GetInt32(6)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }


        public static List<DetaDoc> ObtenerDocEx(DBComun.Estado pTipo, string c_naviera)
        {
            List<DetaDoc> _empleados = new List<DetaDoc>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                SqlCommand _command = null;
                if (c_naviera == "219")
                {
                    /*_consulta = @"SELECT IdDoc, IdReg, c_imo, s_archivo, c_llegada
                                     FROM CCO_DETA_DOC_NAVI WHERE 
                                     EXISTS(SELECT IdReg FROM CCO_ENCA_NAVIERAS) AND b_estado = 1";*/
                    _consulta = @"SELECT IdDoc, a.IdReg, a.c_imo, s_archivo, a.c_llegada, c_naviera
                                FROM CCO_DETA_DOC_EXP_NAVI a INNER JOIN CCO_ENCA_EXPO_NAVI b ON a.IdReg = b.IdReg
                                WHERE b_estado = 1";
                    _command = new SqlCommand(_consulta, _conn as SqlConnection);
                }
                else
                {
                    /*_consulta = @"SELECT IdDoc, IdReg, c_imo, s_archivo, c_llegada
                                     FROM CCO_DETA_DOC_NAVI WHERE 
                                     EXISTS(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_naviera = '{0}') AND b_estado = 1";*/
                    _consulta = @"SELECT IdDoc, a.IdReg, b.c_imo, s_archivo, b.c_llegada, c_naviera
                                FROM CCO_DETA_DOC_EXP_NAVI a INNER JOIN CCO_ENCA_EXPO_NAVI b ON a.IdReg = b.IdReg
                                WHERE c_naviera = '{0}' AND b_estado = 1";

                    /* _consulta = @"SELECT a.IdReg, a.c_imo, max(s_archivo) s_archivo, a.c_llegada, c_naviera,  COUNT(a.IdDoc) CantArch
                                 FROM CCO_DETA_DOC_NAVI a INNER JOIN CCO_ENCA_NAVIERAS b ON a.IdReg = b.IdReg
                                 WHERE c_naviera = '{0}' AND b_estado = 1
                                 GROUP BY a.IdReg, a.c_imo,  a.c_llegada, c_naviera";*/
                    _command = new SqlCommand(string.Format(_consulta, c_naviera), _conn as SqlConnection);
                }


                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDoc _tmpEmpleado = new DetaDoc
                    {
                        IdDoc = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_imo = _reader.GetString(2),
                        s_archivo = _reader.GetString(3),
                        c_llegada = _reader.GetString(4),
                        c_naviera = _reader.GetString(5)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<DetaDoc> ObtenerDocS(DBComun.Estado pTipo, string c_naviera, string c_llegada)
        {
            List<DetaDoc> _empleados = new List<DetaDoc>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = @"SELECT IdDoc, IdReg, c_imo, s_archivo, c_llegada
                                    FROM CCO_DETA_DOC_NAVI WHERE 
                                    IdReg IN(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_naviera = '{0}'  AND c_llegada = '{1}') AND b_estado = 1
                                    AND ISNULL(b_valid, 0) = 0";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera, c_llegada), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDoc _tmpEmpleado = new DetaDoc
                    {
                        IdDoc = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_imo = _reader.GetString(2),
                        s_archivo = _reader.GetString(3),
                        c_llegada = _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static string ActualizarDocNavi(int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_doc_navi_act", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdDoc", IdDoc));          


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActualizarDocNaviEx(int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_doc_exp_navi_act", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdDoc", IdDoc));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaDoc> ObtenerDocA(DBComun.Estado pTipo, string c_naviera, string c_llegada, string s_archivo)
        {
            List<DetaDoc> _empleados = new List<DetaDoc>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = @"SELECT IdDoc, IdReg, c_imo, s_archivo, c_llegada
                                     FROM CCO_DETA_DOC_NAVI WHERE 
                                     EXISTS(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_naviera = '{0}' ) AND c_llegada = '{1}' AND b_estado = 1 AND s_archivo = '{2}'";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera, c_llegada, s_archivo), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDoc _tmpEmpleado = new DetaDoc
                    {
                        IdDoc = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_imo = _reader.GetString(2),
                        s_archivo = _reader.GetString(3),
                        c_llegada = _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<DetaDoc> ObtenerDocAEx(DBComun.Estado pTipo, string c_naviera, string c_llegada, string s_archivo)
        {
            List<DetaDoc> _empleados = new List<DetaDoc>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = @"SELECT IdDoc, IdReg, c_imo, s_archivo, c_llegada
                                     FROM CCO_DETA_DOC_EXP_NAVI WHERE 
                                     EXISTS(SELECT IdReg FROM CCO_ENCA_EXPO_NAVI WHERE c_naviera = '{0}' ) AND c_llegada = '{1}' AND b_estado = 1 AND s_archivo = '{2}'";


                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera, c_llegada, s_archivo), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDoc _tmpEmpleado = new DetaDoc
                    {
                        IdDoc = (int)_reader.GetInt32(0),
                        IdReg = (int)_reader.GetInt32(1),
                        c_imo = _reader.GetString(2),
                        s_archivo = _reader.GetString(3),
                        c_llegada = _reader.GetString(4)
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static string ArchivosExistentes(string c_imo, string c_llegada, string c_naviera, string s_archivo)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*)
                                     FROM CCO_DETA_DOC_NAVI WHERE s_archivo = '{0}'
                                     AND EXISTS(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_naviera = '{1}' AND c_imo = '{2}' and c_llegada = '{3}' ) AND b_estado = 1";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, s_archivo, c_naviera, c_imo, c_llegada), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ArchivosExistentesEx(string c_imo, string c_llegada, string c_naviera, string s_archivo)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*)
                                     FROM CCO_DETA_DOC_EXP_NAVI WHERE s_archivo = '{0}'
                                     AND EXISTS(SELECT IdReg FROM CCO_ENCA_NAVIERAS WHERE c_naviera = '{1}' AND c_imo = '{2}' and c_llegada = '{3}' ) AND b_estado = 1";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, s_archivo, c_naviera, c_imo, c_llegada), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static int ObtenerIdDoc(int pManif)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"DECLARE @i AS INT
                                    SET @i = (SELECT IdDoc FROM CCO_DETA_DOC_NAVI WHERE n_manifiesto = {0})
                                    SELECT @i";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, pManif), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                int resultado = Convert.ToInt32(_command.ExecuteScalar().ToString());
                _conn.Close();
                return resultado;

            }
        }

        public static List<DetaDoc> ObtenerDocO(DBComun.Estado pTipo)
        {
            List<DetaDoc> _empleados = new List<DetaDoc>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = null;
                SqlCommand _command = null;

                /*_consulta = @"SELECT a.IdReg, a.c_imo, a.c_llegada, a.c_voyage, a.c_naviera
                            FROM CCO_ENCA_NAVIERAS a
                            WHERE IdReg IN(SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado= 1 AND b_cancelado = 0) ";*/

                _consulta = @"SELECT DISTINCT(a.c_llegada) c_llegada, a.c_imo
                            FROM CCO_ENCA_NAVIERAS a
                            WHERE IdReg IN(SELECT IdReg FROM CCO_DETA_NAVIERAS WHERE b_autorizado= 1 AND b_cancelado = 0)
                            ORDER BY c_llegada DESC";
                
                _command = new SqlCommand(_consulta, _conn as SqlConnection);
                

                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    DetaDoc _tmpEmpleado = new DetaDoc
                    {
                        c_llegada = _reader.GetString(0),
                        c_imo = _reader.GetString(1)       
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Transporte> ObtenerTransporte()
        {
            List<Transporte> _empleados = new List<Transporte>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = null;
                SqlCommand _command = null;

                _consulta = @"SELECT IdTransporte, d_descripcion, c_value
                            FROM CCO_TRANSPORTE 
                            WHERE b_estado = 1 ORDER BY c_value ";

                _command = new SqlCommand(_consulta, _conn as SqlConnection);


                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Transporte _tmpEmpleado = new Transporte
                    {
                        IdTransporte = (int)_reader.GetInt32(0),
                        d_descripcion = _reader.GetString(1),
                        c_value = (int)_reader.GetInt32(2)                        
                    };

                    _empleados.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static string ActualizarValidacion(int b_valid, int n_manifiesto, int IdReg, DBComun.Estado pTipo, int IdDoc)
        {
            string resultado = null;
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                IDbTransaction tranUp = _conn.BeginTransaction();

                string _consulta = @"UPDATE CCO_DETA_DOC_NAVI
                                     SET b_valid = {0}, f_valid = GETDATE()
                                     WHERE n_manifiesto = {1} AND IdReg = {2} AND IdDoc = {3}
                                     SELECT @@ROWCOUNT";
                SqlCommand _command = new SqlCommand(string.Format(_consulta, b_valid, n_manifiesto, IdReg, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;
                _command.Transaction = (SqlTransaction)tranUp;
                try
                {
                    

                    resultado = _command.ExecuteScalar().ToString();
                    tranUp.Commit();
                }
                catch(Exception ex)
                {
                    tranUp.Rollback();
                    throw new Exception("Problemas en validación de listado " + ex.Message);
                }
                _conn.Close();
                return resultado;

            }
        }

        public static string RevertirValidacion(int n_manifiesto, int IdReg, DBComun.Estado pTipo, int IdDoc)
        {
            string resultado = null;
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                

                string _consulta = @"UPDATE CCO_DETA_DOC_NAVI
                                     SET b_valid = 0, f_valid = NULL
                                     WHERE n_manifiesto = {0} AND IdReg = {1} AND IdDoc = {2}
                                     SELECT @@ROWCOUNT";
                IDbTransaction tranUp = _conn.BeginTransaction();
                SqlCommand _command = new SqlCommand(string.Format(_consulta, n_manifiesto, IdReg, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;
                _command.Transaction = (SqlTransaction)tranUp;

                try
                {                    
                    resultado = _command.ExecuteScalar().ToString();
                    tranUp.Commit();
                }
                catch (Exception ex)
                {
                    tranUp.Rollback();
                    throw new Exception("Problemas en validación de listado " + ex.Message);
                }
                _conn.Close();
                return resultado;

            }
        }

        public static string ActualizarValidacionEx(int b_valid, DBComun.Estado pTipo, int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = @"BEGIN TRY
	                                    BEGIN TRAN
	                                    UPDATE CCO_DETA_DOC_EXP_NAVI
	                                    SET b_valid = {0}, f_valid = GETDATE()
	                                    WHERE IdDoc = {1}	                                    
	                                    SELECT @@ROWCOUNT
                                        COMMIT
                                    END TRY
                                    BEGIN CATCH
	                                    ROLLBACK
                                    END CATCH";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, b_valid, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string CountDoc(DBComun.Estado pTipo, int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = @"  DECLARE @a_totalDoc INT
                                       DECLARE @a_totalCon INT

                                       SET @a_totalDoc = (SELECT COUNT(*) from CCO_DETA_EXPO_NAVI WHERE IdDoc = {0})

                                       SET @a_totalCon = (SELECT COUNT(*) from CCO_DETA_EXPO_NAVI WHERE IdDoc = {1} AND b_autorizado = 1 AND c_correlativo IS NOT NULL)

                                       IF @a_totalDoc = @a_totalCon
	                                      SELECT 1
                                       ELSE
	                                      SELECT 0";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, IdDoc, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string CountConts(DBComun.Estado pTipo, int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*) from CCO_DETA_EXPO_NAVI WHERE IdDoc = {0}";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ActualizarOmitir(string s_comentarios, int n_manifiesto, string s_usuario, int IdReg)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_DOC_NAVI
                                    SET b_valid = 1, b_omitido = 1, s_comentarios = '{0}', f_omitido = GETDATE(), s_usuarios = '{1}'
                                    WHERE n_manifiesto = {2} AND IdReg = {3}
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, s_comentarios, s_usuario, n_manifiesto, IdReg), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string VerificarValid(int n_manifiesto, int IdReg, DBComun.Estado pTipo, int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                //string _consulta = @"SELECT COUNT(*) Canti FROM CCO_DETA_DOC_NAVI WHERE n_manifiesto = {0} AND b_valid = 0 AND f_valid IS NOT NULL";
                string _consulta = @"SELECT CASE WHEN f_valid IS NULL and f_omitido is null THEN 2 ELSE b_valid END valid FROM  CCO_DETA_DOC_NAVI WHERE n_manifiesto = {0} AND IdReg = {1} /*AND b_valid != 1*/ AND IdDoc = {2}";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, n_manifiesto, IdReg, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string VerificarValidEx(int IdReg, DBComun.Estado pTipo, int IdDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                //string _consulta = @"SELECT COUNT(*) Canti FROM CCO_DETA_DOC_NAVI WHERE n_manifiesto = {0} AND b_valid = 0 AND f_valid IS NOT NULL";
                string _consulta = @"SELECT CASE WHEN f_valid IS NULL and f_omitido IS NULL THEN 2 ELSE b_valid END valid FROM  CCO_DETA_DOC_EXP_NAVI WHERE IdReg = {0} AND IdDoc = {1}";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, IdReg, IdDoc), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string RetenValidacion(int IdDeta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                //string _consulta = @"SELECT COUNT(*) Canti FROM CCO_DETA_DOC_NAVI WHERE n_manifiesto = {0} AND b_valid = 0 AND f_valid IS NOT NULL";
                string _consulta = @"DECLARE @var VARCHAR(11) 
                                    SET @var = (SELECT n_contenedor
                                    FROM CCO_DETA_NAVIERAS WHERE IdDeta = {0} AND f_confir_salida IS NOT NULL)
                                    SELECT ISNULL(@var, 'LIBRE') Resultado";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, IdDeta), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

    }
}
