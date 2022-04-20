using System;
using System.Collections.Generic;

using CEPA.CCO.Entidades;
using System.Data;

using Sybase.Data.AseClient;

namespace CEPA.CCO.DAL
{
    public class ClienteDAL
    {
        public static List<TipoCliente> getTipoCliente(DBComun.Estado pEstado)
        {
            List<TipoCliente> notiLista = new List<TipoCliente>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string consulta = @"select a.c_cliente, c_cliente_light,s_nombre_comercial, isnull(s_razon_social, '') s_razon_social, isnull(s_numero_registro, '') s_numero_registro, isnull(s_dui, '') s_dui, isnull(s_nit, '' ) s_nit, isnull(tipoCliente, '') tipoCliente, case when b.b_facilidad = '1' then 'SI' else 'NO' end facilidadPago, isnull(s_direccion1, '') s_direccion
                                    from (select c_cliente, '' as c_cliente_light,s_nombre_comercial, s_razon_social, s_numero_registro, s_dui, s_nit, '' tipoCliente, (select max(s_direccion1) from cn_direcc_clie where c_cliente = d.c_cliente) s_direccion1
                                    from cn_cliente as d
                                    where s_estatus='A' and s_nombre_comercial is not null and s_nombre_comercial != '' 
                                    union all
                                    select c_cliente, c_cliente_light, s_nombre_comercial, '' as razon_social, s_num_reg, s_numero_ref, s_nit, 'CO' tipoCliente, s_direccion s_direccion1
                                    from cn_client_light 
                                    where b_estado='A' and s_nombre_comercial is not null and s_nombre_comercial !='') as a left join cn_carac_clie b on a.c_cliente = b.c_cliente 
                                    where b.b_tipo_fac = 'FA' and b.b_estado = 'S' order by 1";

                AseCommand _command = new AseCommand(consulta, _conn as AseConnection);

                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoCliente _notificacion = new TipoCliente
                    {                      
                        c_cliente = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        c_cliente_light = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        s_nombre_comercial = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        s_razon_social = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        s_numero_registro = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        s_dui = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        s_nit = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        tipoCliente = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        facilidadPago = _reader.IsDBNull(8) ? "" : _reader.GetString(8),
                        s_direccion = _reader.IsDBNull(9) ? "" : _reader.GetString(9)
                    };

                    notiLista.Add(_notificacion);
                }

                _reader.Close();
                _conn.Close();
                return notiLista;
            }

        }

        public static List<TipoCliente> getTipoClienteLL(DBComun.Estado pEstado, string c_cliente, string lcliente)
        {
            List<TipoCliente> notiLista = new List<TipoCliente>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string consulta = @"set nocount on
                                    declare @c_cliente varchar(20)
                                    declare @lcliente varchar(5)
                                    set @c_cliente = '{0}'
                                    set @lcliente = '{1}'

                                    select case when a.c_cliente ='999' then c_cliente_light else a.c_cliente end c_cliente, s_nombre_comercial, isnull(s_razon_social, '') s_razon_social, isnull(s_numero_registro, '') s_numero_registro, 
                                    isnull(s_dui, '') s_dui, isnull(s_nit, '' ) s_nit, isnull(tipoCliente, '') tipoCliente, case when b.b_facilidad = '1' then 'CON FACILIDAD DE PAGO' else 'SIN FACILIDAD DE PAGO' end facilidadPago, 
                                    isnull(s_direccion1, '') s_direccion from (select c_cliente, '' as c_cliente_light,s_nombre_comercial, s_razon_social, s_numero_registro, s_dui, s_nit, '' tipoCliente, (select max(s_direccion1) 
                                    from cn_direcc_clie where c_cliente = d.c_cliente) s_direccion1, 'N' tcli from cn_cliente as d where s_estatus='A' and s_nombre_comercial is not null and s_nombre_comercial != '' union select c_cliente, 
                                    c_cliente_light, s_nombre_comercial, '' as razon_social, s_num_reg, s_numero_ref, s_nit, 'CO' tipoCliente, s_direccion s_direccion1, 'L' tcli from cn_client_light where b_estado='A' 
                                    and s_nombre_comercial is not null and s_nombre_comercial !='') as a left join cn_carac_clie b on a.c_cliente = b.c_cliente where b.b_tipo_fac = 'FA' and b.b_estado = 'S' and (case when a.c_cliente = '999' 
                                    then c_cliente_light else a.c_cliente end) = @c_cliente and a.tcli = @lcliente order by 1";
                AseCommand _command = new AseCommand(string.Format(consulta, c_cliente, lcliente), _conn as AseConnection);


                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TipoCliente _notificacion = new TipoCliente
                    {
                        c_cliente = _reader.IsDBNull(0) ? "" : _reader.GetString(0),                        
                        s_nombre_comercial = _reader.IsDBNull(1) ? "" : _reader.GetString(1),
                        s_razon_social = _reader.IsDBNull(2) ? "" : _reader.GetString(2),
                        s_numero_registro = _reader.IsDBNull(3) ? "" : _reader.GetString(3),
                        s_dui = _reader.IsDBNull(4) ? "" : _reader.GetString(4),
                        s_nit = _reader.IsDBNull(5) ? "" : _reader.GetString(5),
                        tipoCliente = _reader.IsDBNull(6) ? "" : _reader.GetString(6),
                        facilidadPago = _reader.IsDBNull(7) ? "" : _reader.GetString(7),
                        s_direccion = _reader.IsDBNull(8) ? "" : _reader.GetString(8)
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
