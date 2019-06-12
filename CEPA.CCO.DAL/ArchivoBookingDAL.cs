using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;
using CEPA.CCO.Entidades;
using CEPA.CCO.DAL.DataSet;
using System.Web;

namespace CEPA.CCO.DAL
{
    public class ArchivoBookingDAL
    {
        public static List<ArchivoBooking> GetRango(string sRuta, DBComun.Estado pTipo)
        {
           
            List<ArchivoBooking> listaArch = new List<ArchivoBooking>();

            string nombreHoja = ArchivoExcelDAL.GetNombre(sRuta);

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pTipo))
            {
                _conn.Open();

                int num_fila = 2;
                object Valor = null;
                
                object Valor3 = null;
                

                int fila = ArchivoExcelDAL.GetRowExcel(sRuta, DBComun.Estado.verdadero, nombreHoja) + 1;

                // string _consulta = @"SELECT * FROM [{0}$A1:V{1}]";

                //string _consulta = @"SELECT * FROM [{0}$A1:V{1}]";
                //string _consulta = @"SELECT * FROM [{0}$A1:W{1}]";
                string _consulta = @"SELECT * FROM [{0}$A1:C{1}]";

                OleDbCommand _command = new OleDbCommand(string.Format(_consulta, nombreHoja, fila), _conn as OleDbConnection);
                _command.CommandType = CommandType.Text;

                OleDbDataReader _reader = _command.ExecuteReader();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        if (!_reader.IsDBNull(0))
                        {

                            if (!_reader.IsDBNull(0))
                            {
                                if (ArchivoBookingDAL.isNumeric(_reader[0]))
                                    Valor = Convert.ToString(_reader[0]);
                                else
                                    Valor = _reader[0];
                            }

                           

                            if (!_reader.IsDBNull(2))
                            {
                                if (ArchivoBookingDAL.isNumeric(_reader[2]))
                                    Valor3 = Convert.ToString(_reader[2]);
                                else
                                    Valor3 = _reader[2];

                            }



                            ArchivoBooking _tmpArc = new ArchivoBooking
                            {
                                num_fila = num_fila,
                                //num_manif = (int)_reader.GetDouble(0),
                                shipment = _reader.IsDBNull(0) ? "" : Valor.ToString(),
                                n_contenedor = _reader.IsDBNull(1) ? "" : _reader.GetString(1),                              
                                n_sello = _reader.IsDBNull(2) ? "" : Valor3.ToString()
                                
                            };
                            listaArch.Add(_tmpArc);
                            num_fila = num_fila + 1;
                        }
                        else
                            break;
                    }
                }

                _reader.Close();
                _conn.Close();
                return listaArch;
            }

        }


        public static bool isNumeric(object value)
        {
            try
            {
                double d = System.Double.Parse(value.ToString(), System.Globalization.NumberStyles.Any);
                return true;
            }
            catch (System.FormatException)
            {                
                return false;
            }
        }

        public static bool isFecha(object value)
        {
            try
            {
                DateTime d = System.DateTime.Parse(value.ToString());
                return true;
            }
            catch (System.FormatException)
            {
                return false;
            }
        }

        //public static dsData  GetRangoBooking(string sRuta, DBComun.Estado pEstado)
        //{
        //    dsData _datos = new dsData();

        //    string nombreHoja = ArchivoExcelDAL.GetNombre(sRuta);

        //    using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.Excel, pEstado))
        //    {

        //    }
        //}
    }
}
