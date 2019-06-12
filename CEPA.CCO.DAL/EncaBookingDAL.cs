using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;

namespace CEPA.CCO.DAL
{
	public class EncaBookingDAL
	{
		public static string Insertar(EncaBooking _enca)
		{

			using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
			{
				_conn.Open();
				SqlCommand _command = new SqlCommand("pa_enca_booking_save", _conn as SqlConnection);
				_command.CommandType = CommandType.StoredProcedure;

				_command.Parameters.Add(new SqlParameter("@c_naviera", _enca.c_naviera));
				_command.Parameters.Add(new SqlParameter("@s_archivo", _enca.s_archivo));
				_command.Parameters.Add(new SqlParameter("@b_estado", _enca.b_estado));                

				string resultado = _command.ExecuteScalar().ToString();
				_conn.Close();
				return resultado;

			}
		}        

		public static string ActualizarCabecera(string c_naviera)
		{

			using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
			{
				_conn.Open();
				string _consulta = @"UPDATE CCO_ENCA_BOOKING SET b_estado = 0 WHERE c_naviera = '{0}' AND b_estado = 1; 
                                    SELECT @@ROWCOUNT";

				SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera), _conn as SqlConnection);
				_command.CommandType = CommandType.Text;

				string resultado = _command.ExecuteScalar().ToString();
				_conn.Close();
				return resultado;

			}
		}

		public static string ObtenerCabecera(string c_naviera)
		{

			using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
			{
				_conn.Open();
				string resultado;
					string _consulta = @"DECLARE @CANTIDAD INT
										SET @CANTIDAD = (SELECT COUNT(*) FROM CCO_ENCA_BOOKING WHERE c_naviera = '{0}' AND b_estado = 1)
										IF @CANTIDAD = 0
											SELECT 0
										ELSE
											SELECT IdBooking FROM CCO_ENCA_BOOKING WHERE c_naviera = '{0}' AND b_estado = 1 ";

				SqlCommand _command = new SqlCommand(string.Format(_consulta, c_naviera), _conn as SqlConnection);
				_command.CommandType = CommandType.Text;


				resultado = _command.ExecuteScalar().ToString();           

				_conn.Close();
				return resultado;

			}
		}      
	}

}
