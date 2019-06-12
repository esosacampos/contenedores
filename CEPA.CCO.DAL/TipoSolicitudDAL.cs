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
    }
}
