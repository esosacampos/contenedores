using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CEPA.CCO.Entidades;

namespace CEPA.CCO.DAL
{
    public class ContenedorDAL
    {
        public static List<PrimeraPos> ObtenerPrimerPos(DBComun.Estado pTipo)
        {
            List<PrimeraPos> list = new List<PrimeraPos>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT IdCon, IdValue, d_descripcion FROM CCO_ENCA_CON_LEN ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    PrimeraPos _tmp = new PrimeraPos
                    {
                        IdCode = (int)_reader.GetInt32(0),
                        IdValue = _reader.GetString(1),
                        Descripcion = _reader.GetString(2)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<SegundaPos> ObtenerSegundaPos(DBComun.Estado pTipo)
        {
            List<SegundaPos> list = new List<SegundaPos>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT IdConWi, IdValue, d_descripcion FROM CCO_ENCA_CON_WIDTH ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    SegundaPos _tmp = new SegundaPos
                    {
                        IdConWi = (int)_reader.GetInt32(0),
                        IdValue = _reader.GetString(1),
                        Descripcion = _reader.GetString(2)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<TerceraPos> ObtenerTerceraPos(DBComun.Estado pTipo)
        {
            List<TerceraPos> list = new List<TerceraPos>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT IdConE, IdCode, d_descripcion FROM CCO_CON_ESTANDAR ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TerceraPos _tmp = new TerceraPos
                    {
                        IdConE = (int)_reader.GetInt32(0),
                        IdCode = _reader.GetString(1),
                        Descripcion = _reader.GetString(2)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }

        public static List<EstadoOrden> ObtenerOrden(DBComun.Estado pTipo)
        {
            List<EstadoOrden> list = new List<EstadoOrden>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pTipo))
            {
                _conn.Open();
                string consulta = @"SELECT IdOrden, Valor, Orden FROM CCO_ESTADO_ORDEN ";

                SqlCommand _command = new SqlCommand(consulta, _conn as SqlConnection);
                _command.CommandType = CommandType.Text;

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    EstadoOrden _tmp = new EstadoOrden
                    {
                        IdOrden = (int)_reader.GetInt32(0),
                        Valor = _reader.GetString(1),
                        Orden = (int)_reader.GetInt32(2)
                    };

                    list.Add(_tmp);
                }

                _reader.Close();
                _conn.Close();
                return list;
            }
        }
    }
}
