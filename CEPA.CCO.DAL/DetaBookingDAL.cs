using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
    public class DetaBookingDAL
    {
        public static string Insertar(DetaBooking _deta)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_booking_save", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                _command.Parameters.Add(new SqlParameter("@IdBooking", _deta.IdBooking));
                _command.Parameters.Add(new SqlParameter("@shipment_name", _deta.shipment_name));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", _deta.n_contenedor));                
                _command.Parameters.Add(new SqlParameter("@n_sello", _deta.n_sello));
                _command.Parameters.Add(new SqlParameter("@b_estado", _deta.b_estado));
                _command.Parameters.Add(new SqlParameter("@b_marca", _deta.b_marca));   

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string DesactivarBooking(string c_naviera)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"UPDATE CCO_DETA_BOOKING
                                    SET b_estado = 0
                                    WHERE b_marca = 0 AND b_estado = 1 
                                    AND IdBooking IN(SELECT IdBooking FROM CCO_ENCA_BOOKING WHERE c_naviera = '{0}' AND b_estado = 1); 
                                    SELECT @@ROWCOUNT";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }

        public static string CantidadBooking(string c_naviera, int pId)
        {

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                string _consulta = @"SELECT COUNT(*) FROM CCO_DETA_BOOKING                                    
                                    WHERE b_marca = 0 AND b_estado = 1 
                                    AND IdBooking IN(SELECT IdBooking FROM CCO_ENCA_BOOKING WHERE c_naviera = '{0}' AND IdBooking = {1} AND b_estado = 1)";

                SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera, pId), _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                string resultado = _command.ExecuteScalar().ToString();
                _conn.Close();
                return resultado;

            }
        }


    }
}
