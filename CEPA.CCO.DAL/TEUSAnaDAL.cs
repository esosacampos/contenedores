using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Globalization;
using Sybase.Data.AseClient;


namespace CEPA.CCO.DAL
{
    public class TEUSAnaDAL
    {
        public static List<TEUSAna> ObtenerTEUS(DBComun.Estado pEstado, int year)
        {
            List<TEUSAna> _teus = new List<TEUSAna>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"set nocount on
                            declare @empresa varchar(20), @as_c_tipo_info varchar(20), @as_c_empresa varchar(20), @as_c_genera  varchar(20) 
                            declare @year int

                            set @as_c_empresa = '04'    
                            set @as_c_tipo_info = 'FAT'
                            set @as_c_genera  = 'FF'
                            set @year = {0}

                            CREATE TABLE #tmp_contlinea
                            (
                            c_nul varchar(20) null ,
                            f_atraque datetime null ,
                            f_desatraque datetime null ,        

                            f_inicio_ope datetime null ,
                            f_fin_ope datetime null ,        

                            s_nombre_buque varchar(50),
                            s_condicion varchar(50) null ,
                            s_condicion_nva varchar(50) null ,
                            c_tipo_mov varchar(20) ,
                            s_movimiento varchar(20) ,

                            id_linea varchar(20) null ,   
                            d_linea varchar(80) null ,         
                            id_producto  varchar(20) null ,   
                            s_producto varchar(50) null ,   
 
                            v_unidades numeric(14,4) null ,   
                            v_teus numeric(14,4) null  ,   
                            v_peso numeric(14,4) null ,   

                            v_equi_teus numeric(14,4) null ,   
                            tamanio varchar(10) null,
                            año int ,
                            id_agencia varchar(20) null 
                            )

                            INSERT INTO #tmp_contlinea
                            SELECT fa_conte_lineas.c_nul as Nul,
                                    fa_llegadas.f_atraque as Atraque ,
                                    fa_llegadas.f_desatraque as Desatraque ,        

                                    fa_llegadas.f_ini_oper as Inicio_Ope ,
                                    fa_llegadas.f_fin_oper as Fin_Ope ,        
        
                                    fa_buques.s_nom_buque as Nombre_buque ,
                                    fa_producto.c_condicion as  Condicion,
                                    fa_producto.c_condicion as  Condicion_nva,        
                                     fa_conte_lineas.b_movimiento as tipo_mov ,
                                    case 
                                        when fa_conte_lineas.b_movimiento = 'C'
                                        then 'Exportacion'
            
                                        when fa_conte_lineas.b_movimiento = 'D'
                                        then 'Importacion'

                                    end Movimiento ,
                                     fa_conte_lineas.c_linea as id_linea,   
                                     fa_linea_naviera.s_descripcion as Linea,         
                                     fa_conte_lineas.c_producto as id_producto ,   
                                     fa_producto.d_producto as Producto ,   
         
                                     fa_conte_lineas.v_unidades as Unidades,   
                                     ( fa_conte_lineas.v_unidades * fa_producto.v_equivale_teus ) as TEUS,
                                     fa_conte_lineas.v_peso as Peso ,   

                                     fa_producto.v_equivale_teus as Equi_TEUS,   
                                     fa_producto.c_tamanio as Tamanio, 
                                     year(fa_llegadas.f_atraque) as Año,
                                     fa_linea_naviera.c_agencia as id_agencia
                                FROM fa_conte_lineas,   
                                     fa_linea_naviera,   
                                     fa_producto  , 
                                     fa_llegadas , 
                                     fa_buques
                               WHERE ( fa_linea_naviera.c_linea = fa_conte_lineas.c_linea ) and  
                                     ( fa_producto.c_producto = fa_conte_lineas.c_producto ) and
                                        fa_llegadas.c_nul = fa_conte_lineas.c_nul and
                                        fa_conte_lineas.b_estado = 'A' and 
                                        fa_buques.c_buque = fa_llegadas.c_buque and
                                        fa_llegadas.c_empresa = @as_c_empresa and
                                        year(fa_llegadas.f_atraque) between @year - 1 and @year  and @as_c_tipo_info = 'FAT' 

                            order by fa_llegadas.f_atraque , fa_llegadas.c_nul 
     
                                declare @t_teus1  numeric(14,4), @t_teus2 numeric(14, 4)
                                set @t_teus1 = (select sum(v_teus) from #tmp_contlinea where año = @year)
                                set @t_teus2 = (select sum(v_teus) from #tmp_contlinea where año = @year - 1)
    
                                select d_linea, convert(int, sum(Teus2)) teu2, convert(numeric(14, 2), round((round((sum(Teus2) / @t_teus2),4) * 100), 4)) t2,
                                convert(int, sum(Teus1)) teu1, convert(numeric(14, 2), round((round((sum(Teus1) / @t_teus1),4) * 100), 4)) t, id_agencia
                                from(select 
                                    d_linea,          
                                    sum(v_teus) as Teus1, 0 Teus2, id_agencia       
                                from #tmp_contlinea   
                                where año = @year
                                group by d_linea, id_agencia     
                                union     
                                select 
                                    d_linea,        
                                    0 Teus1,    
                                    sum(v_teus) as Teus2, id_agencia       
                                from #tmp_contlinea   
                                where año = @year - 1
                                group by d_linea, id_agencia) as a
                                group by d_linea, id_agencia
                                order by 2 desc, 4 desc
    
                                drop table #tmp_contlinea";

                _command = new AseCommand(string.Format(_consulta, year), _conn as AseConnection);



                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TEUSAna _tmpEmpleado = new TEUSAna
                    {
                        d_linea = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        teu2 = (int)_reader.GetInt32(1),
                        t2 = (double)_reader.GetDecimal(2),
                        teu1 = (int)_reader.GetInt32(3),
                        t = (double)_reader.GetDecimal(4),
                        c_agencia = _reader.IsDBNull(5) ? "" : _reader.GetString(5)
                    };

                    _teus.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _teus;
            }
        }

        public static List<TEUSResu> ObtenerResumen(DBComun.Estado pEstado, int year)
        {
            List<TEUSResu> _teus = new List<TEUSResu>();


            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SyBaseNET, pEstado))
            {
                _conn.Open();
                string _consulta = null;
                AseCommand _command = null;

                _consulta = @"set nocount on
                            declare @empresa varchar(20), @as_c_tipo_info varchar(20), @as_c_empresa varchar(20), @as_c_genera  varchar(20) 
                            declare @year int

                            set @as_c_empresa = '04'    
                            set @as_c_tipo_info = 'FAT'
                            set @as_c_genera  = 'FF'
                            set @year = {0}

                            CREATE TABLE #tmp_contlinea
                            (
                            c_nul varchar(20) null ,
                            f_atraque datetime null ,
                            f_desatraque datetime null ,        

                            f_inicio_ope datetime null ,
                            f_fin_ope datetime null ,        

                            s_nombre_buque varchar(50),
                            s_condicion varchar(50) null ,
                            s_condicion_nva varchar(50) null ,
                            c_tipo_mov varchar(20) ,
                            s_movimiento varchar(20) ,

                            id_linea varchar(20) null ,   
                            d_linea varchar(80) null ,         
                            id_producto  varchar(20) null ,   
                            s_producto varchar(50) null ,   

                            v_unidades numeric(14,4) null ,   
                            v_teus numeric(14,4) null  ,   
                            v_peso numeric(14,4) null ,   

                            v_equi_teus numeric(14,4) null ,   
                            tamanio varchar(10) null,
                            año int ,
                            id_agencia varchar(20) null 
                            )

                            INSERT INTO #tmp_contlinea
                            SELECT fa_conte_lineas.c_nul as Nul,
                                fa_llegadas.f_atraque as Atraque ,
                                fa_llegadas.f_desatraque as Desatraque ,        

                                fa_llegadas.f_ini_oper as Inicio_Ope ,
                                fa_llegadas.f_fin_oper as Fin_Ope ,        

                                fa_buques.s_nom_buque as Nombre_buque ,
                                fa_producto.c_condicion as  Condicion,
                                fa_producto.c_condicion as  Condicion_nva,        
                                    fa_conte_lineas.b_movimiento as tipo_mov ,
                                case 
                                    when fa_conte_lineas.b_movimiento = 'C'
                                    then 'Exportacion'

                                    when fa_conte_lineas.b_movimiento = 'D'
                                    then 'Importacion'

                                end Movimiento ,
                                    fa_conte_lineas.c_linea as id_linea,   
                                    fa_linea_naviera.s_descripcion as Linea,         
                                    fa_conte_lineas.c_producto as id_producto ,   
                                    fa_producto.d_producto as Producto ,   

                                    fa_conte_lineas.v_unidades as Unidades,   
                                    ( fa_conte_lineas.v_unidades * fa_producto.v_equivale_teus ) as TEUS,
                                    fa_conte_lineas.v_peso as Peso ,   

                                    fa_producto.v_equivale_teus as Equi_TEUS,   
                                    fa_producto.c_tamanio as Tamanio, 
                                    year(fa_llegadas.f_atraque) as Año,
                                    fa_linea_naviera.c_agencia as id_agencia
                            FROM fa_conte_lineas,   
                                    fa_linea_naviera,   
                                    fa_producto  , 
                                    fa_llegadas , 
                                    fa_buques
                            WHERE ( fa_linea_naviera.c_linea = fa_conte_lineas.c_linea ) and  
                                    ( fa_producto.c_producto = fa_conte_lineas.c_producto ) and
                                    fa_llegadas.c_nul = fa_conte_lineas.c_nul and
                                    fa_conte_lineas.b_estado = 'A' and 
                                    fa_buques.c_buque = fa_llegadas.c_buque and
                                    fa_llegadas.c_empresa = @as_c_empresa and
                                    year(fa_llegadas.f_atraque) = @year  and @as_c_tipo_info = 'FAT' 

                            order by fa_llegadas.f_atraque , fa_llegadas.c_nul 


                            CREATE TABLE #tmp_cnul
                            (
                                c_nul varchar(20) null
                            )

                            INSERT INTO #tmp_cnul
                            select TOP 1 c_nul from fa_conte_lineas WHERE YEAR(f_registro) = @year  ORDER BY f_registro DESC


                            CREATE TABLE #tmp_consul
                            (
                                f_desatraque datetime null ,
                                s_nombre_buque varchar(50)
                            )

                            INSERT INTO #tmp_consul
                            select a.f_desatraque, b.s_nom_buque
                            from fa_llegadas a inner join fa_buques b on a.c_buque = b.c_buque
                            where a.c_nul = (SELECT c_nul FROM #tmp_cnul) 

                            select d_linea, convert(int, sum(v_lleno)) v_lleno, convert(int,sum(t_llenos)) t_llenos, convert(int,sum(v_vacios)) v_vacios, 
                            convert(int,sum(t_vacios)) t_vacios, convert(int,sum(tot_import)) t_import, convert(int,sum(ve_llenos)) ve_llenos, 
                            convert(int,sum(te_llenos)) te_llenos, convert(int,sum(ve_vacios)) ve_vacios, convert(int,sum(te_vacios)) te_vacios, 
                            convert(int,sum(tot_export)) t_export, convert(int,sum(t_uni)) t_unidades, convert(int,sum(t_teu)) t_teus, s_nombre_buque, f_desatraque
                            FROM(SELECT  d_linea ,      
                                    sum(v_unidades ) as v_lleno ,   
                                    sum(v_teus ) as t_llenos ,
                                    0 v_vacios, 0 t_vacios ,
                                    0 ve_llenos , 0 te_llenos ,
                                    0 ve_vacios, 0 te_vacios,
                                    0 tot_import, 0 tot_export,
                                    0 t_uni, 0 t_teu              
                                from #tmp_contlinea
                                where s_movimiento = 'Importacion'
                                AND s_condicion <> 'VAC'
                                group by d_linea 
                            union
                            SELECT  d_linea ,      
                                    0 v_lleno, 0 t_llenos, 
                                    sum(v_unidades ) as v_vacios ,   
                                    sum(v_teus ) as t_vacios ,
                                    0 ve_llenos , 0 te_llenos ,
                                    0 ve_vacios, 0 te_vacios,
                                    0 tot_import, 0 tot_export,
                                    0 t_uni, 0 t_teu                   
                                from #tmp_contlinea
                                where s_movimiento = 'Importacion'
                                AND s_condicion = 'VAC'
                                group by d_linea
                                union
                                SELECT  d_linea ,      
                                    0 v_lleno, 0 t_llenos, 
                                    0 v_vacios, 0 t_vacios ,
                                    sum(v_unidades ) as ve_llenos ,   
                                    sum(v_teus ) as te_llenos ,
                                    0 ve_vacios, 0 te_vacios,
                                    0 tot_import, 0 tot_export,
                                    0 t_uni, 0 t_teu                    
                                from #tmp_contlinea
                                where s_movimiento = 'Exportacion'
                                AND s_condicion <> 'VAC'
                                group by d_linea 
                            union
                            SELECT  d_linea ,      
                                    0 v_lleno, 0 t_llenos, 
                                    0 v_vacios, 0 t_vacios,
                                    0 ve_llenos , 0 te_llenos ,
                                        sum(v_unidades ) as ve_vacios ,   
                                    sum(v_teus ) as te_vacios,
                                    0 tot_import, 0 tot_export,
                                    0 t_uni, 0 t_teu                  
                                from #tmp_contlinea
                                where s_movimiento = 'Exportacion'
                                AND s_condicion = 'VAC'
                                group by d_linea 
                            union
                            SELECT  d_linea ,      
                                    0 v_lleno, 0 t_llenos, 
                                    0 v_vacios, 0 t_vacios,
                                    0 ve_llenos , 0 te_llenos ,
                                    0 as ve_vacios ,   
                                    0 as te_vacios,
                                    sum(v_teus) tot_import, 0 tot_export,
                                    0 t_uni, 0 t_teu                  
                                from #tmp_contlinea
                                where s_movimiento = 'Importacion'    
                                group by d_linea
                            union
                            SELECT  d_linea ,      
                                    0 v_lleno, 0 t_llenos, 
                                    0 v_vacios, 0 t_vacios,
                                    0 ve_llenos , 0 te_llenos ,
                                    0 as ve_vacios ,   
                                    0 as te_vacios,
                                    0 tot_import, sum(v_teus) tot_export,
                                    0 t_uni, 0 t_teu                  
                                from #tmp_contlinea
                                where s_movimiento = 'Exportacion'
                                group by d_linea
                            union
                            SELECT  d_linea ,      
                                    0 v_lleno, 0 t_llenos, 
                                    0 v_vacios, 0 t_vacios,
                                    0 ve_llenos , 0 te_llenos ,
                                    0 as ve_vacios ,   
                                    0 as te_vacios,
                                    0 tot_import, 0 tot_export,
                                    sum(v_unidades) t_uni, sum(v_teus) t_teu                  
                                from #tmp_contlinea        
                                group by d_linea) AS A, #tmp_consul 
                            group by d_linea
                            order by t_teus desc

                            drop table #tmp_contlinea

                            drop table #tmp_cnul

                            drop table #tmp_consul";

                _command = new AseCommand(string.Format(_consulta, year), _conn as AseConnection);


                
                _command.CommandType = CommandType.Text;

                AseDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    TEUSResu _tmpEmpleado = new TEUSResu
                    {
                        d_linea = _reader.IsDBNull(0) ? "" : _reader.GetString(0),
                        vi_luni = (int)_reader.GetInt32(1),
                        vi_lteus = (int)_reader.GetInt32(2),
                        vi_vuni = (int)_reader.GetInt32(3),
                        vi_vteus = (int)_reader.GetInt32(4),
                        t_import = (int)_reader.GetInt32(5),
                        ve_luni = (int)_reader.GetInt32(6),
                        ve_lteus = (int)_reader.GetInt32(7),
                        ve_vuni = (int)_reader.GetInt32(8),
                        ve_vteus = (int)_reader.GetInt32(9),
                        t_export = (int)_reader.GetInt32(10),
                        t_uni = (int)_reader.GetInt32(11),
                        t_teus = (int)_reader.GetInt32(12),
                        s_nombre_buque = _reader.IsDBNull(13) ? "" : _reader.GetString(13),
                        f_desatraque = Convert.ToDateTime(_reader.GetDateTime(14), CultureInfo.CreateSpecificCulture("es-SV"))
                    };

                    _teus.Add(_tmpEmpleado);
                }

                _reader.Close();
                _conn.Close();
                return _teus;
            }
        }
    }
}
