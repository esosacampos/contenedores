using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.DAL
{
    public class MenuDAL
    {
        public static List<Menu> ObtenerMenu(string pUsuario)
        {
            List<Menu> pLista = new List<Menu>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_MENU", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Usuario", pUsuario));

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Menu _nivel = new Menu
                    {  
                        MenuId = (int)_reader.GetInt32(0),
                        NombreMenu = _reader.GetString(1),
                        DescripcionMenu = _reader.GetString(2),
                        PadreId = (int)_reader.GetInt32(3),
                        Posicion = (int)_reader.GetInt32(4),
                        Icono = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        Habilitado = _reader.GetBoolean(6),
                        Url = _reader.GetString(7)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static List<Menu> ObtenerPefil(int pIdPefil)
        {
            List<Menu> pLista = new List<Menu>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_PERFIL", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdPerfil", pIdPefil));

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Menu _nivel = new Menu
                    {
                        IdPerfil = (int)_reader.GetInt32(0),
                        MenuId = (int)_reader.GetInt32(1),
                        NombreMenu = _reader.GetString(2),
                        DescripcionMenu = _reader.GetString(3),
                        PadreId = (int)_reader.GetInt32(4),
                        Posicion = (int)_reader.GetInt32(5),
                        Icono = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        Habilitado = _reader.GetBoolean(7),
                        Url = _reader.GetString(8)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static List<Perfil> ObtenerPerfiles()
        {
            List<Perfil> pLista = new List<Perfil>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_PERFILES", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Perfil _nivel = new Perfil
                    {
                        IdPerfil = (int)_reader.GetInt32(0),
                        NombrePerfil = _reader.GetString(1),
                        Habilitado = _reader.GetString(2)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static List<Menu> ObtenerMenu()
        {
            List<Menu> pLista = new List<Menu>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_MENUS", _conn as SqlConnection);

                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Menu _nivel = new Menu
                    {
                        MenuId = (int)_reader.GetInt32(0),
                        NombreMenu = _reader.GetString(1),
                        DescripcionMenu = _reader.GetString(2),
                        PadreId = (int)_reader.GetInt32(3),
                        Posicion = (int)_reader.GetInt32(4),
                        Icono = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        Habilitado = _reader.GetBoolean(6),
                        Url = _reader.GetString(7)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static int InsertarPerfil(string pNombre)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_PERFIL", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Nombre", pNombre));

                int _reader = Convert.ToInt32(_command.ExecuteScalar());

                _conn.Close();
                return _reader;
            }

        }

        public static int InsertarMenuPefil(PerfilMenu pMenu)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_PERFIL_MENU", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdPerfil", pMenu.IdPerfil));
                _command.Parameters.Add(new SqlParameter("@MenuId", pMenu.MenuId));

                int _reader = Convert.ToInt32(_command.ExecuteScalar());

                _conn.Close();
                return _reader;
            }

        }

        public static int EliminarPerfil(int pId)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_ELIMINAR_PERFILES_ID", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdPerfil", pId));

                int _reader = Convert.ToInt32(_command.ExecuteScalar());

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarPerfil(string pNombrePerfil, int pValor, int pId)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_ACT_PERFILES_ID", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Nombre", pNombrePerfil));
                _command.Parameters.Add(new SqlParameter("@Habilitado", pValor));
                _command.Parameters.Add(new SqlParameter("@IdPerfil", pId));


                string _reader = _command.ExecuteScalar().ToString();
                _conn.Close();

                return _reader;
            }
        }

        public static List<Perfil> ObtenerPerfiles(int pIdPerfil)
        {
            List<Perfil> pLista = new List<Perfil>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_PERFILES_ID", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdPerfil", pIdPerfil));

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Perfil _nivel = new Perfil
                    {
                        IdPerfil = (int)_reader.GetInt32(0),
                        NombrePerfil = _reader.GetString(1),
                        Habilitado = _reader.GetString(2)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static List<ProcesoRelacionado> ObtenerEstados()
        {
            List<ProcesoRelacionado> _lista = new List<ProcesoRelacionado>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_OBT_VALORES", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    ProcesoRelacionado _proceso = new ProcesoRelacionado
                    {
                        IdProceso = (int)_reader.GetInt32(0),
                        Descripcion = _reader.GetString(1)
                    };

                    _lista.Add(_proceso);
                }
            }
            return _lista;
        }

        public static List<Usuario> ObtenerUser()
        {
            List<Usuario> _lista = new List<Usuario>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                SqlCommand _command = new SqlCommand("PA_OBT_USERS", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Usuario _proceso = new Usuario
                    {                        
                        c_usuario = _reader.GetString(0),
                        d_usuario = _reader.GetString(1),
                        c_naviera = _reader.GetString(2),
                        Habilitado = _reader.GetString(3),
                        IdReg = (int)_reader.GetInt32(4)
                    };

                    _lista.Add(_proceso);
                }
            }


            return _lista;
        }

        public static List<Usuario> ObtenerPerfilesUser(string pIdUser)
        {
            List<Usuario> pLista = new List<Usuario>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBT_PERFIL_USER", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdUser", pIdUser));

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Usuario _nivel = new Usuario
                    {
                        c_usuario = _reader.GetString(0),
                        d_usuario = _reader.GetString(1),
                        c_naviera = _reader.GetString(2),
                        Habilitado = _reader.GetString(3)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static List<Perfil> ObtenerPerfilUser(string pUsuario)
        {
            List<Perfil> pLista = new List<Perfil>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBTENER_MENU_USUARIO", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Usuario", pUsuario));

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Perfil _nivel = new Perfil
                    {
                        IdPerfil = (int)_reader.GetInt32(0),
                        NombrePerfil = _reader.GetString(1)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static List<Menu> ObtenerPefilIdM(params string[] pIdPefil)
        {
            List<Menu> pLista = new List<Menu>();

            string join = string.Empty;
            if (pIdPefil.Count() > 0)
                join = string.Join(", ", pIdPefil.ToArray());

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_OBT_PERFIL_IDM", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Parametros", join));

                SqlDataReader _reader = _command.ExecuteReader(CommandBehavior.CloseConnection);

                while (_reader.Read())
                {
                    Menu _nivel = new Menu
                    {
                        IdPerfil = (int)_reader.GetInt32(0),
                        MenuId = (int)_reader.GetInt32(1),
                        NombreMenu = _reader.GetString(2),
                        DescripcionMenu = _reader.GetString(3),
                        PadreId = (int)_reader.GetInt32(4),
                        Posicion = (int)_reader.GetInt32(5),
                        Icono = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        Habilitado = _reader.GetBoolean(7),
                        Url = _reader.GetString(8)
                    };

                    pLista.Add(_nivel);
                }
            }
            return pLista;
        }

        public static void AlmacenarPerfilUsuario(List<PerfilUsuario> _tareaDoc)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                if (_tareaDoc.Count > 0)
                {
                    foreach (PerfilUsuario item in _tareaDoc)
                    {
                        SqlCommand _command = new SqlCommand("PA_INSERTAR_PERFILES_USUARIOS", _conn as SqlConnection);
                        _command.CommandType = CommandType.StoredProcedure;

                        _command.Parameters.Add(new SqlParameter("@IdPerfil", item.IdPerfil));
                        _command.Parameters.Add(new SqlParameter("@IdUser", item.IdUser));

                        _command.ExecuteNonQuery();

                    }
                }

                _conn.Close();
            }
        }

        public static int EliminarUsuario(string pUser)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_DEL_USER", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdUser", pUser));

                int _reader = Convert.ToInt32(_command.ExecuteScalar());

                _conn.Close();
                return _reader;
            }

        }

        public static string ActualizarUsuario(int pValor, string pUser)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_ACT_PERFIL_USER", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@Habilitado", pValor));
                _command.Parameters.Add(new SqlParameter("@IdUser", pUser));

                string _reader = _command.ExecuteScalar().ToString();
                _conn.Close();

                return _reader;
            }
        }
    }
}
