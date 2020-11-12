using System;
using System.Collections.Generic;


using CEPA.CCO.Entidades;
using System.Data;

using Sybase.Data.AseClient;

namespace CEPA.CCO.DAL
{
    public class OperacionesDAL
    {
        public static List<Operaciones> ObtenerOperaciones(DBComun.Estado pEstado, string c_operadora)
        {
            List<Operaciones> notiLista = new List<Operaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT a.c_nul, a.c_llegada, b.s_nom_buque, a.c_operadora, d.d_operadora, c.d_tip_buque, a.f_arribo, a.f_fin_oper
                                    FROM fa_llegadas a, fa_buques b, fa_tip_buques c, fa_operadoras d
                                    WHERE a.c_buque = b.c_buque and
                                    b.c_tip_buque = c.c_tip_buque and
                                    a.c_operadora = d.c_operadora and
                                    a.c_empresa ='04' and year(a.f_atraque)=year(getdate()) and a.f_fin_oper is null and a.c_operadora = '{0}' and b.c_tip_buque='4' -- BUQUES DE CONTENEDORES
                                    ORDER BY a.f_arribo";

                AseCommand _command = new AseCommand(string.Format(consulta, c_operadora), _conn as AseConnection);
                
                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Operaciones _notificacion = new Operaciones
                    {
                        c_llegada = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        f_arribo = (DateTime)_reader.GetDateTime(6)                        
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<Operaciones> ObtenerOperaciones(DBComun.Estado pEstado)
        {
            List<Operaciones> notiLista = new List<Operaciones>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT a.c_nul, a.c_llegada, b.s_nom_buque, a.c_operadora, d.d_operadora, c.d_tip_buque, a.f_arribo, a.f_fin_oper
                                    FROM fa_llegadas a, fa_buques b, fa_tip_buques c, fa_operadoras d
                                    WHERE a.c_buque = b.c_buque and
                                    b.c_tip_buque = c.c_tip_buque and
                                    a.c_operadora = d.c_operadora and
                                    a.c_empresa ='04' and year(a.f_atraque)=year(getdate()) and a.f_fin_oper is null and b.c_tip_buque='4' -- BUQUES DE CONTENEDORES
                                    ORDER BY a.f_arribo";

                AseCommand _command = new AseCommand(consulta, _conn as AseConnection);

                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Operaciones _notificacion = new Operaciones
                    {
                        c_llegada = _reader.GetString(1),
                        d_buque = _reader.GetString(2),
                        f_arribo = (DateTime)_reader.GetDateTime(6)
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
