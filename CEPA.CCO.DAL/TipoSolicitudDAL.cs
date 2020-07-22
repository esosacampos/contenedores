using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;

using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class TipoSolicitudDAL
    {
        public static List<TipoRevision> ObtenerRevision()
        {
            List<TipoRevision> list = new List<TipoRevision>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdRevision, Clave, Tipo, CASE WHEN Habilitado = 1 THEN 'Activo' ELSE 'Inactivo' END Habilitado FROM CCO_TIPO_REVISION WHERE Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoRevision _tmp = new TipoRevision
                    {
                        IdRevision = (int)_reader.GetInt32(0),
                        Clave = _reader.GetString(1),
                        Tipo = _reader.GetString(2),
                        Habilitado = _reader.GetString(3)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<TipoRevision> ObtenerRevisionTo()
        {
            List<TipoRevision> list = new List<TipoRevision>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdRevision, Clave, Tipo, CASE WHEN Habilitado = 1 THEN 'Activo' ELSE 'Inactivo' END Habilitado FROM CCO_TIPO_REVISION";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoRevision _tmp = new TipoRevision
                    {
                        IdRevision = (int)_reader.GetInt32(0),
                        Clave = _reader.GetString(1),
                        Tipo = _reader.GetString(2),
                        Habilitado = _reader.GetString(3)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<AgenteDAN> ObtenerAgentes()
        {
            List<AgenteDAN> list = new List<AgenteDAN>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdAgente, n_agente, d_agente, Habilitado FROM CCO_AGENTE_DAN WHERE Habilitado = 1 ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    AgenteDAN _tmp = new AgenteDAN
                    {
                        IdAgente = (int)_reader.GetInt32(0),
                        n_agente = _reader.GetString(1),
                        d_agente = _reader.GetString(2)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static string InsertarUsuario(TipoRevision pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_TREVISION", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Clave", pTipo.Clave));
                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo.Tipo));

                //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<TipoRevision> ObtenerRevision(int IdRevision)
        {
            List<TipoRevision> list = new List<TipoRevision>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string consulta = @"SELECT IdRevision, Clave, Tipo, CASE WHEN Habilitado = 1 THEN 'Activo' ELSE 'Inactivo' END Habilitado FROM CCO_TIPO_REVISION WHERE IdRevision = {0}";

                SqlCommand _command = new SqlCommand(string.Format(consulta, IdRevision), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoRevision _tmp = new TipoRevision
                    {
                        IdRevision = (int)_reader.GetInt32(0),
                        Clave = _reader.GetString(1),
                        Tipo = _reader.GetString(2),
                        Habilitado = _reader.GetString(3)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static string Actualizar(TipoRevision pTipo)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_ACT_TREVISION", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdRevision", pTipo.IdRevision));
                _command.Parameters.Add(new SqlParameter("@Clave", pTipo.Clave));
                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo.Tipo));
                _command.Parameters.Add(new SqlParameter("@Habilitado", pTipo.Hab));

                //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }


        public static List<Yearss> ObtenerYears(string pTipo)
        {
            List<Yearss> list = new List<Yearss>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_YEARS", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Yearss _tmp = new Yearss
                    {
                        IdValue = (int)_reader.GetInt32(0),
                        sYearss = (int)_reader.GetInt32(1)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<Months> ObtenerMeses()
        {
            List<Months> list = new List<Months>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_MESES", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Months _tmp = new Months
                    {
                        Mes = (int)_reader.GetInt32(0),
                        s_mes = _reader.GetString(1)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<RptIngreImport> ObtenerDetaRptIng(int pYears, int pMes, string pTipo)
        {
            List<RptIngreImport> list = new List<RptIngreImport>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_DAN_REPORTE", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@year", pYears));
                _command.Parameters.Add(new SqlParameter("@mes", pMes));
                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    RptIngreImport _tmp = new RptIngreImport
                    {
                        IdDeta = (int)_reader.GetInt32(0),
                        c_llegada = _reader.GetString(1),
                        n_contenedor = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        p_procedencia = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        s_consignatario = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        s_mercaderia = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        f_retencion = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        f_liberacion = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        b_cancelado = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        navicorto = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        c_naviera = _reader.IsDBNull(10) ? "" : _reader.GetString(10)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }
    }
}
