using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using Sybase.Data.AseClient;


namespace CEPA.CCO.DAL
{
    public class AutorizacionDAL
    {
        public static List<Autorizacion> ObtenerSupervisores(DBComun.Estado pEstado)
        {
            List<Autorizacion> notiLista = new List<Autorizacion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string consulta = @"SELECT '', '-- Supervisores de Estiba --'
                                    UNION
                                    SELECT 'elsa.sosa', 'ELSA BEATRIZ SOSA CAMPOS'
                                    UNION
                                    SELECT 'samuel.ortega', 'SAMUEL ANTONIO LOPEZ ORTEGA'
                                    UNION
                                    SELECT distinct(a.c_id_usuario), ISNULL(a.s_nombres, '') + ' ' + ISNULL(a.s_prim_ape, '') + ' ' + ISNULL(a.s_seg_ape, '')
                                    FROM pl_expediente as a inner JOIN pl_sit_rev as b ON a.c_expediente = b.c_expediente and a.c_empresa = b.c_empresa and a.c_cen_cos = b.c_cen_cos inner JOIN
                                    pl_cargo as c ON b.c_cargo = c.c_cargo
                                    WHERE a.c_empresa = '04' and b.c_cargo = '285' and a.s_email = 'supervisor.estiba@cepa.gob.sv'";

                AseCommand _command = new AseCommand(consulta, _conn as AseConnection);

                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    Autorizacion _notificacion = new Autorizacion
                    {
                        c_id_usuario = _reader.GetString(0),
                        s_nombre = _reader.GetString(1)
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
