using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class BlDAL
    {
        public static List<BL> getBL(string c_llegada, DBComun.Estado pTipo)
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

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));       


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    BL _notificacion = new BL
                    {
                        c_nul = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_prefijo = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        n_manifiesto = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        n_contenedor = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_tamaño = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        v_peso = _reader.IsDBNull(6) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(6)),
                        v_teus = _reader.IsDBNull(7) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(7)),
                        c_trafico = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        f_salida = _reader.IsDBNull(9) ? Convert.ToDateTime(_reader.GetDateTime(9)) : _reader.GetDateTime(9),
                        vn_manejo = _reader.IsDBNull(10) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(10)),
                        vc_manejo = _reader.IsDBNull(11) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(11)),
                        p_manejo = _reader.IsDBNull(12) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(12)),
                        vn_transfer = _reader.IsDBNull(13) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(13)),
                        vc_transfer = _reader.IsDBNull(14) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(14)),
                        p_transfer = _reader.IsDBNull(15) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(15)),
                        vn_desp = _reader.IsDBNull(16) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(16)),
                        vc_desp = _reader.IsDBNull(17) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(17)),
                        p_desp = _reader.IsDBNull(18) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(18)),
                        n_alm = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        c_alm = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                        b_cancelado = _reader.IsDBNull(21) ? "" : _reader.GetString(21),
                        t_retencion = _reader.IsDBNull(22) ? "" : _reader.GetString(22),
                        ta_alm = _reader.IsDBNull(23) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(23)),
                        reff = _reader.IsDBNull(24) ? "" : _reader.GetString(24),
                        peso_entregado = _reader.IsDBNull(25) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(25)),
                        f_retencion = _reader.IsDBNull(26) ? Convert.ToDateTime(_reader.GetDateTime(26)) : _reader.GetDateTime(26)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static List<BL> getBL(string c_llegada, DBComun.Estado pTipo, string tmp)
        {
            List<BL> notiLista = new List<BL>();

            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();


                SqlCommand _command = new SqlCommand("PA_CONSUL_BL_tmp", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    BL _notificacion = new BL
                    {
                        c_nul = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_prefijo = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        c_llegada = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        n_manifiesto = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        n_contenedor = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        c_tamaño = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        v_peso = _reader.IsDBNull(6) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(6)),
                        v_teus = _reader.IsDBNull(7) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(7)),
                        c_trafico = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        f_salida = _reader.IsDBNull(9) ? Convert.ToDateTime(_reader.GetDateTime(9)) : _reader.GetDateTime(9),
                        vn_manejo = _reader.IsDBNull(10) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(10)),
                        vc_manejo = _reader.IsDBNull(11) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(11)),
                        p_manejo = _reader.IsDBNull(12) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(12)),
                        vn_transfer = _reader.IsDBNull(13) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(13)),
                        vc_transfer = _reader.IsDBNull(14) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(14)),
                        p_transfer = _reader.IsDBNull(15) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(15)),
                        vn_desp = _reader.IsDBNull(16) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(16)),
                        vc_desp = _reader.IsDBNull(17) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(17)),
                        p_desp = _reader.IsDBNull(18) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(18)),
                        n_alm = _reader.IsDBNull(19) ? "" : _reader.GetString(19),
                        c_alm = _reader.IsDBNull(20) ? "" : _reader.GetString(20),
                        b_cancelado = _reader.IsDBNull(21) ? "" : _reader.GetString(21),
                        t_retencion = _reader.IsDBNull(22) ? "" : _reader.GetString(22),
                        ta_alm = _reader.IsDBNull(23) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(23)),
                        reff = _reader.IsDBNull(24) ? "" : _reader.GetString(24),
                        peso_entregado = _reader.IsDBNull(25) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(25)),
                        f_retencion = _reader.IsDBNull(26) ? Convert.ToDateTime(_reader.GetDateTime(26)) : _reader.GetDateTime(26),
                        v_peso_tm = _reader.IsDBNull(27) ? 0.00 : Convert.ToDouble(_reader.GetDecimal(27)),
                        f_recepcion = _reader.IsDBNull(28) ? Convert.ToDateTime(_reader.GetDateTime(28)) : _reader.GetDateTime(28),
                        f_retiro = _reader.IsDBNull(29) ? Convert.ToDateTime(_reader.GetDateTime(29)) : _reader.GetDateTime(29)
                    };
                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return notiLista;
            }

        }

        public static List<BL2> getBL2(string c_llegada, DBComun.Estado pTipo, string n_cotenedor, string b_facilidad, DateTime f_salida_user)
        {
            List<BL2> _lista = new List<BL2>();
            DataTable tabla = new DataTable();
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-SV");
            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("PA_CONSUL_BL_TarifaNueva", _conn as SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                _command.Parameters.Add(new SqlParameter("@c_llegada", c_llegada));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", n_cotenedor));
                _command.Parameters.Add(new SqlParameter("@b_facilidad", b_facilidad));
                _command.Parameters.Add(new SqlParameter("@f_salida_user", f_salida_user.ToString("yyyy-MM-dd")));
                using (var _reader = _command.ExecuteReader())
                {
                    tabla.Load(_reader);
                    if (tabla.Rows.Count > 0)
                    {
                        _lista = tabla.ToList<BL2>();
                        //msgError = "OK";
                    }
                    else
                    {
                        //msgError = "Ningun resultado";
                    }
                }
                System.Threading.Thread.CurrentThread.CurrentCulture = CurrentCI;
                return _lista;
            }

        }
    }
}
