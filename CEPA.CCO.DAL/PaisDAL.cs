using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;
using CEPA.CCO.Entidades;
using System.Data.SqlClient;
using Sybase.Data.AseClient;
using System.Web.UI.WebControls;

namespace CEPA.CCO.DAL
{
    public class PaisDAL
    {
        public static List<Pais> getPaises(DBComun.TipoBD pBD)
        {
            List<Pais> list = new List<Pais>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(pBD, DBComun.Estado.verdadero))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_OBTENER_PAISES", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;               


                SqlDataReader reader = _command.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(LoadCountries(reader));
                }
            }

            return list;

        }

        private static Pais LoadCountries(IDataReader reader)
        {
            Pais item = new Pais();

            item.CountryCode = Convert.ToString(reader["CountryCode"]);
            item.CountryName = Convert.ToString(reader["CountryName"]);
            item.b_oirsa = Convert.ToBoolean(reader["b_oirsa"]);
            item.s_oirsa = Convert.ToBoolean(reader["b_oirsa"]) == false ? "Falso" : "Verdadero";
            return item;
        }

        public static string saveCountry(string CountryCode, bool b_oirsa)
        {
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
			{
				_conn.Open();
				string consulta = @"UPDATE CCO_COD_PAISES SET b_oirsa = @b_oirsa WHERE CountryCode = @CountryCode;
                                    SELECT @@ROWCOUNT";

				SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.Parameters.AddWithValue("@CountryCode", CountryCode);
                _command.Parameters.AddWithValue("@b_oirsa", b_oirsa == false ? 0 : 1);
				_command.CommandType = CommandType.Text;

				string _reader = _command.ExecuteScalar().ToString();

				_conn.Close();
                return _reader; 
                    
			}            
        }
    }
}
