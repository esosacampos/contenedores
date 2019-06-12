using CEPA.CCO.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class UsuarioDAL
    {
        public static List<Usuario> ObtenerUsuarios(DBComun.Estado pTipo)
        {
            List<Usuario> _empleados = new List<Usuario>();


            List<string> pLista = new List<string>();




            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string _consulta = "";
                _consulta = "SELECT IdReg, c_usuario, d_usuario, c_naviera, c_mail, c_iso_navi, c_navi_corto ";
                _consulta += "FROM CCO_USUARIOS ";
                _consulta += "ORDER BY c_usuario";

                SqlCommand _command = new SqlCommand(_consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Usuario _tmpUsuario = new Usuario
                    {
                        IdReg = _reader.GetInt32(0),
                        c_usuario = _reader.GetString(1),
                        d_usuario = _reader.GetString(2),
                        c_naviera = _reader.GetString(3),
                        c_mail = _reader.GetString(4),
                        c_iso_navi = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_navi_corto = _reader.IsDBNull(6) ? "" : _reader.GetString(6)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<Usuario> ObtenerUsuarioAC(DBComun.Estado pTipo, string c_usuario)
        {
            List<Usuario> _empleados = new List<Usuario>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();

                string _consulta = @"SELECT IdReg, c_usuario, d_usuario, a.c_naviera, ISNULL(c_mail, '') c_mail, a.c_iso_navi, a.c_navi_corto, CASE WHEN b.b_sidunea = 1 THEN 1 ELSE 0 END b_sidunea, CASE WHEN b.b_ibooking = 1 THEN 1 ELSE 0 END b_ibooking
                                    FROM CCO_USUARIOS a INNER JOIN CCO_USUARIOS_NAVIERAS b ON a.c_naviera = b.c_naviera
                                    WHERE c_usuario = '{0}' 
                                    ORDER BY c_usuario";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_usuario), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Usuario _tmpUsuario = new Usuario
                    {
                        IdReg = _reader.GetInt32(0),
                        c_usuario = _reader.GetString(1),
                        d_usuario = _reader.GetString(2),
                        c_naviera = _reader.GetString(3),
                        c_mail = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_iso_navi = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_navi_corto = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        b_sidunea = _reader.IsDBNull(7) ? 0 : _reader.GetInt32(7),
                        b_ibooking = _reader.IsDBNull(8) ? 0 : _reader.GetInt32(8)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<UsuarioContra> UserContratista(DBComun.Estado pTipo, int c_marcacion)
        {
            List<UsuarioContra> _empleados = new List<UsuarioContra>();


            List<string> pLista = new List<string>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlContratistas, pTipo))
            {
                _conn.Open();
                string _consulta = @"SELECT DISTINCT t.c_marcacion, t.p_nombre + ' ' + ISNULL(t.s_nombre, '') + ' ' + t.p_ape + ' ' + ISNULL(t.s_ape, '') AS NombreC, o.c_operadora, c.d_operadora, o.c_cargo 
                                    FROM CC_TRABAJADORES t INNER JOIN CC_OPERACIONES o ON t.c_marcacion = o.c_marcacion
                                    INNER JOIN CC_CONTRATISTAS c ON o.c_operadora = c.c_operadora
                                    WHERE c_periodo IN(SELECT c_periodo FROM CC_PERIODOS WHERE b_estado = 1) AND o.b_estado = 1 AND t.c_marcacion = {0}
                                    ORDER BY t.c_marcacion";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_marcacion), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    UsuarioContra _tmpUsuario = new UsuarioContra
                    {
                        c_marcacion = _reader.GetInt32(0),
                        NombreC = _reader.GetString(1),
                        c_operadora = _reader.GetString(2),
                        d_operadora = _reader.GetString(3),
                        c_cargo = _reader.GetInt32(4)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static string InsertarUsuario(Usuario pUsuario)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_USUARIOS", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_usuario", pUsuario.c_usuario));
                _command.Parameters.Add(new SqlParameter("@d_usuario", pUsuario.d_usuario));
                _command.Parameters.Add(new SqlParameter("@c_naviera", pUsuario.c_naviera));
                _command.Parameters.Add(new SqlParameter("@c_iso_navi", pUsuario.c_iso_navi));
                _command.Parameters.Add(new SqlParameter("@c_navi_corto", pUsuario.c_navi_corto));
                //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static List<UsuarioNaviera> ObtenerUsuNavi(string c_naviera)
        {
            List<UsuarioNaviera> _empleados = new List<UsuarioNaviera>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT c_iso c_iso_navi, c_prefijo c_navi_corto 
                                    FROM CCO_USUARIOS_NAVIERAS 
                                    WHERE c_naviera = '{0}'";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera), _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    UsuarioNaviera _tmpUsuario = new UsuarioNaviera
                    {
                        c_iso_navi = _reader.GetString(0),
                        c_navi_corto = _reader.GetString(1)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static string ActUsuario(Usuario pUsuario)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_ACT_USUARIOS", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_usuario", pUsuario.c_usuario));
                _command.Parameters.Add(new SqlParameter("@d_usuario", pUsuario.d_usuario));
                _command.Parameters.Add(new SqlParameter("@c_naviera", pUsuario.c_naviera));
                _command.Parameters.Add(new SqlParameter("@c_iso_navi", pUsuario.c_iso_navi));
                _command.Parameters.Add(new SqlParameter("@c_navi_corto", pUsuario.c_navi_corto));
                //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string InserUserNaviera(Usuario pUsuario)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_USUARIOS_NAVIERAS", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_naviera", pUsuario.c_naviera));
                _command.Parameters.Add(new SqlParameter("@c_iso_navi", pUsuario.c_iso_navi));
                _command.Parameters.Add(new SqlParameter("@c_navi_corto", pUsuario.c_navi_corto));
                //   _command.Parameters.Add(new SqlParameter("@n_manifiesto", _Encanaviera.num_manif));


                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }
    }

    public class TipoRevisionesDAL
    {
        public static List<TipoRevisiones> Revisiones()
        {
            List<TipoRevisiones> _empleados = new List<TipoRevisiones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT IdRevision, t_revision FROM CCO_REVISION_DAN
                                    WHERE Habilitado = 1";

                SqlCommand _command = new SqlCommand(_consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoRevisiones _tmpUsuario = new TipoRevisiones
                    {
                        IdRevision = _reader.GetInt32(0),
                        t_revision = _reader.GetString(1)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<TipoRevisiones> DetalleLib()
        {
            List<TipoRevisiones> _empleados = new List<TipoRevisiones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT Id, Detalle FROM CCO_DETALLE_LIB
                                    WHERE Habilitado = 1";

                SqlCommand _command = new SqlCommand(_consulta, _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoRevisiones _tmpUsuario = new TipoRevisiones
                    {
                        IdRevision = _reader.GetInt32(0),
                        t_revision = _reader.GetString(1)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }

        public static List<TipoRevisiones> Year(string pTipo)
        {
            List<TipoRevisiones> _empleados = new List<TipoRevisiones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_DAN_YEAR", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@Tipo", pTipo));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoRevisiones _tmpUsuario = new TipoRevisiones
                    {
                        IdRevision = _reader.GetInt32(0),
                        Years = _reader.GetInt32(1)
                    };

                    _empleados.Add(_tmpUsuario);
                }

                _reader.Close();
                _conn.Close();
                return _empleados;
            }
        }
    }
}
