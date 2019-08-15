using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class BlDAL
    {
        public static List<BL> getBL(string n_BL, DBComun.Estado pTipo)
        {
            List<BL> notiLista = new List<BL>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_CONSUL_BL", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@n_BL", n_BL));               


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    BL _notificacion = new BL
                    {
                        c_nul = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_prefijo = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        n_manifiesto = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        n_BL = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        n_contenedor = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        c_tamaño = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        v_peso = _reader.IsDBNull(7) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(7)),
                        v_teus = _reader.IsDBNull(8) ? 0 : Convert.ToInt32(_reader.GetInt32(8)),
                        c_trafico = _reader.IsDBNull(9) ? "" : _reader.GetString(9),
                        f_salida = _reader.IsDBNull(10) ? Convert.ToDateTime(_reader.GetDateTime(10)) : _reader.GetDateTime(10),
                        n_manejo = _reader.IsDBNull(11) ? "" : _reader.GetString(11),
                        c_manejo = _reader.IsDBNull(12) ? "" : _reader.GetString(12),
                        n_transfer = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        c_transfer = _reader.IsDBNull(14) ? "" : _reader.GetString(14),
                        n_desp = _reader.IsDBNull(15) ? "" : _reader.GetString(15),
                        c_desp = _reader.IsDBNull(16) ? "" : _reader.GetString(16),
                        n_alm = _reader.IsDBNull(17) ? "" : _reader.GetString(17),
                        c_alm = _reader.IsDBNull(18) ? "" : _reader.GetString(18)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }
    }
}
