using CEPA.CCO.Entidades;
using Sybase.Data.AseClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;

namespace CEPA.CCO.DAL
{
    public class VaciadoDAL
    {
        public static int InsertSolVac(Vaciado _soliVaciado)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_INSERT_SOLI_VAC", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@IdTipoVa", _soliVaciado.IdTipoVa));
                _command.Parameters.Add(new SqlParameter("@a_manifiesto", _soliVaciado.a_manifiesto));
                _command.Parameters.Add(new SqlParameter("@n_manifiesto", _soliVaciado.n_manifiesto));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", _soliVaciado.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_contacto", _soliVaciado.n_contacto));
                _command.Parameters.Add(new SqlParameter("@t_contacto", _soliVaciado.t_contacto));
                _command.Parameters.Add(new SqlParameter("@e_contacto", _soliVaciado.e_contacto));
                _command.Parameters.Add(new SqlParameter("@s_archivo_aut", _soliVaciado.s_archivo_aut));                
            

                int resultado = Convert.ToInt32(_command.ExecuteScalar().ToString());
                _conn.Close();
                return resultado;

            }
        }

        public static List<TipoVaciado> ObtenerResultado(DBComun.Estado pTipo)
        {
            List<TipoVaciado> notiLista = new List<TipoVaciado>();

            

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_SELECT_VAC", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                              


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoVaciado _notificacion = new TipoVaciado
                    {              
                        IdTipoVac = Convert.ToInt32(_reader.GetInt32(0)),
                        Descripcion = _reader.GetString(1)                       
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
