using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class EstadosDeclaDAL
    {
        public static string Insertar(EstadosDecla _esta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_REGADUANA", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                if (_esta.a_decla.ToString().Length == 4)
                {
                    _command.Parameters.Add(new SqlParameter("@c_aduana", _esta.c_aduana));
                    _command.Parameters.Add(new SqlParameter("@n_manifiesto", _esta.n_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@a_manifiesto", _esta.a_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@n_contenedor", _esta.n_contenedor));
                    _command.Parameters.Add(new SqlParameter("@a_decla", _esta.a_decla));
                    _command.Parameters.Add(new SqlParameter("@s_decla", _esta.s_decla));
                    _command.Parameters.Add(new SqlParameter("@c_decla", _esta.c_decla));
                    _command.Parameters.Add(new SqlParameter("@IdEstado", _esta.IdEstado));
                    _command.Parameters.Add(new SqlParameter("@f_reg_aduana", _esta.f_reg_aduana));
                    _command.Parameters.Add(new SqlParameter("@IdSelectividad", _esta.IdSelectividad));
                    _command.Parameters.Add(new SqlParameter("@n_nit", _esta.n_nit == "" ? (object)DBNull.Value : _esta.n_nit));
                    _command.Parameters.Add(new SqlParameter("@b_sidunea", _esta.b_siduneawd));
                    _command.Parameters.Add(new SqlParameter("@s_consignatario", _esta.s_consignatario));
                    _command.Parameters.Add(new SqlParameter("@n_bl", _esta.n_BL));
                    _command.Parameters.Add(new SqlParameter("@s_descripcion", _esta.s_descripcion));
                    _command.Parameters.Add("@resulta", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                else
                {
                    _command.Parameters.Add(new SqlParameter("@c_aduana", _esta.c_aduana));
                    _command.Parameters.Add(new SqlParameter("@n_manifiesto", _esta.n_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@a_manifiesto", _esta.a_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@n_contenedor", _esta.n_contenedor));
                    _command.Parameters.Add(new SqlParameter("@a_transito", _esta.a_transito));
                    _command.Parameters.Add(new SqlParameter("@r_transito", _esta.r_transito));
                    _command.Parameters.Add(new SqlParameter("@IdEstado", _esta.IdEstado));
                    _command.Parameters.Add(new SqlParameter("@f_reg_aduana", _esta.f_reg_aduana));
                    _command.Parameters.Add(new SqlParameter("@IdSelectividad", _esta.IdSelectividad));
                    _command.Parameters.Add(new SqlParameter("@n_nit", _esta.n_nit));
                    _command.Parameters.Add(new SqlParameter("@b_sidunea", _esta.b_siduneawd));
                    _command.Parameters.Add(new SqlParameter("@s_consignatario", _esta.s_consignatario));
                    _command.Parameters.Add(new SqlParameter("@n_bl", _esta.n_BL));
                    _command.Parameters.Add(new SqlParameter("@s_descripcion", _esta.s_descripcion));
                    _command.Parameters.Add("@resulta", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                _command.ExecuteNonQuery();
                string resultado = _command.Parameters["@resulta"].Value.ToString();
                _conn.Close();
                return resultado;
            }
        }

        public static string InsertarTest(EstadosDecla _esta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERTAR_REGADUANA_T", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                if (_esta.a_decla.ToString().Length == 4)
                {
                    _command.Parameters.Add(new SqlParameter("@c_aduana", _esta.c_aduana));
                    _command.Parameters.Add(new SqlParameter("@n_manifiesto", _esta.n_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@a_manifiesto", _esta.a_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@n_contenedor", _esta.n_contenedor));
                    _command.Parameters.Add(new SqlParameter("@a_decla", _esta.a_decla));
                    _command.Parameters.Add(new SqlParameter("@s_decla", _esta.s_decla));
                    _command.Parameters.Add(new SqlParameter("@c_decla", _esta.c_decla));
                    _command.Parameters.Add(new SqlParameter("@IdEstado", _esta.IdEstado));
                    _command.Parameters.Add(new SqlParameter("@f_reg_aduana", _esta.f_reg_aduana));
                    _command.Parameters.Add(new SqlParameter("@IdSelectividad", _esta.IdSelectividad));
                    _command.Parameters.Add(new SqlParameter("@n_nit", _esta.n_nit));
                    _command.Parameters.Add(new SqlParameter("@b_sidunea", _esta.b_siduneawd));
                    _command.Parameters.Add(new SqlParameter("@s_consignatario", _esta.s_consignatario));
                    _command.Parameters.Add(new SqlParameter("@n_bl", _esta.n_BL));
                    _command.Parameters.Add(new SqlParameter("@s_descripcion", _esta.s_descripcion));
                    _command.Parameters.Add("@resulta", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                else
                {
                    _command.Parameters.Add(new SqlParameter("@c_aduana", _esta.c_aduana));
                    _command.Parameters.Add(new SqlParameter("@n_manifiesto", _esta.n_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@a_manifiesto", _esta.a_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@n_contenedor", _esta.n_contenedor));
                    _command.Parameters.Add(new SqlParameter("@a_transito", _esta.a_transito));                    
                    _command.Parameters.Add(new SqlParameter("@r_transito", _esta.r_transito));
                    _command.Parameters.Add(new SqlParameter("@IdEstado", _esta.IdEstado));
                    _command.Parameters.Add(new SqlParameter("@f_reg_aduana", _esta.f_reg_aduana));
                    _command.Parameters.Add(new SqlParameter("@IdSelectividad", _esta.IdSelectividad));
                    _command.Parameters.Add(new SqlParameter("@n_nit", _esta.n_nit));
                    _command.Parameters.Add(new SqlParameter("@b_sidunea", _esta.b_siduneawd));
                    _command.Parameters.Add(new SqlParameter("@s_consignatario", _esta.s_consignatario));
                    _command.Parameters.Add(new SqlParameter("@n_bl", _esta.n_BL));
                    _command.Parameters.Add(new SqlParameter("@s_descripcion", _esta.s_descripcion));
                    _command.Parameters.Add("@resulta", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                _command.ExecuteNonQuery();
                string resultado = _command.Parameters["@resulta"].Value.ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string ConsulDAN(string c_llegada, string n_contenedor)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_webService_dan", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));                
                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_contenedor));
                
                _command.Parameters.Add("@resulta", SqlDbType.Int).Direction = ParameterDirection.Output;

                _command.ExecuteNonQuery();
                string resultado = _command.Parameters["@resulta"].Value.ToString();
                _conn.Close();
                return resultado;

            }
        }        
    }
}
