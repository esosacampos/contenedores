using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CEPA.CCO.Linq
{
    public class DetaNavieraLINQ
    {
        public static DateTime FechaBD()
        {
            DateTime _fecha;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("SELECT Fecha = GETDATE() ", _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _fecha = Convert.ToDateTime(_command.ExecuteScalar());

                return _fecha;

            }
        }

        public static int YearBD()
        {
            int _fecha;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("SELECT Year = YEAR(GETDATE()) ", _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _fecha = Convert.ToInt32(_command.ExecuteScalar());

                return _fecha;

            }
        }

        public static DateTime FechaBDS()
        {
            DateTime _fecha;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.falso))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("SELECT Fecha = GETDATE() ", _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _fecha = Convert.ToDateTime(_command.ExecuteScalar());

                return _fecha;

            }
        }

        public static DateTime FechaBD(DBComun.Estado pEstado)
        {
            DateTime _fecha;

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, pEstado))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("SELECT Fecha = GETDATE() ", _conn as SqlConnection)
                {
                    CommandType = CommandType.Text
                };

                _fecha = Convert.ToDateTime(_command.ExecuteScalar());

                return _fecha;

            }
        }


        public static List<ResultadoValidacion> ValidarDetalleEx(string pRuta, DBComun.Estado pEstado)
        {
            List<ResultadoValidacion> validandoLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> listaCurrent = new List<ResultadoValidacion>();

            List<ArchivoExport> pListaC = new List<ArchivoExport>();

            DBComun.sRuta = pRuta;



            System.Data.DataTable dt = ArchivoExcelDAL.ExtractExcelSheetValuesToDataTable(pRuta, "1");

            if (dt.Rows.Count > 0)
            {
                pListaC = ArchivoExcelDAL.ConvertToList(dt);

                List<ResultadoValidacion> resulVal = ResultadoValidacionDAL.ValidarArchivoExport(pListaC);


                List<ResultadoValidacion> resulRe = ResultadoValidacionDAL.ValoresRepetidosEx(pListaC);

                if (resulVal.Count > 0)
                {
                    listaCurrent.AddRange(resulVal);
                }

                if (resulRe.Count > 0)
                {
                    listaCurrent.AddRange(resulRe);
                }
            }


            return listaCurrent;
        }


        public static List<ResultadoValidacion> ValidarDetalle(string pRuta, DBComun.Estado pEstado)
        {
            List<ResultadoValidacion> validandoLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> listaCurrent = new List<ResultadoValidacion>();
            DBComun.sRuta = pRuta;

            List<ArchivoExcel> _list = ArchivoExcelDAL.GetRango(pRuta, DBComun.Estado.verdadero);

            foreach (ArchivoExcel item in _list)
            {
                ArchivoExcel _Archivo = new ArchivoExcel
                {
                    num_fila = item.num_fila,
                    n_BL = item.n_BL,
                    n_contenedor = item.n_contenedor,
                    c_tamaño = item.c_tamaño,
                    v_peso = item.v_peso,
                    b_estado = item.b_estado,
                    s_consignatario = item.s_consignatario,
                    n_sello = item.n_sello,
                    c_pais_destino = item.c_pais_destino,
                    c_pais_origen = item.c_pais_origen,
                    c_detalle_pais = item.c_detalle_pais,
                    s_comodity = item.s_comodity,
                    s_prodmanifestado = item.s_prodmanifestado,
                    v_tara = item.v_tara,
                    b_reef = item.b_reef,
                    b_ret_dir = item.b_ret_dir,
                    c_imo_imd = item.c_imo_imd,
                    c_un_number = item.c_un_number,
                    b_transhipment = item.b_transhipment,
                    c_condicion = item.c_condicion,
                    b_shipper = item.b_shipper,
                    b_transferencia = item.b_transferencia,
                    b_manejo = item.b_manejo,
                    b_despacho = item.b_despacho
                };

                List<ResultadoValidacion> resulVal = ResultadoValidacionDAL.ValidarArchivo(_Archivo);


            }



            List<ResultadoValidacion> resulRe = ResultadoValidacionDAL.ValoresRepetidos(_list);


            listaCurrent = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;




            return listaCurrent;
        }

        public static List<ResultadoValidacion> ValidarDetalle(string pRuta, List<DetaNaviera> pTabla, DBComun.Estado pEstado)
        {
            List<ResultadoValidacion> validandoLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> listaCurrent = new List<ResultadoValidacion>();

            DBComun.sRuta = pRuta;

            List<ArchivoExcel> _list = ArchivoExcelDAL.GetRango(pRuta, DBComun.Estado.verdadero);


            List<ResultadoValidacion> resulRe = ResultadoValidacionDAL.ValoresRepetidos(_list, pTabla);


            listaCurrent = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;


            return listaCurrent;
        }

        public static List<ResultadoValidacion> ValidarDetalleEx(string pRuta, List<DetaNaviera> pTabla, DBComun.Estado pEstado)
        {
            List<ResultadoValidacion> validandoLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> listaCurrent = new List<ResultadoValidacion>();

            List<ArchivoExport> pListaC = new List<ArchivoExport>();
            DBComun.sRuta = pRuta;


            System.Data.DataTable dt = ArchivoExcelDAL.ExtractExcelSheetValuesToDataTable(pRuta, "1");

            if (dt.Rows.Count > 0)
            {
                pListaC = ArchivoExcelDAL.ConvertToList(dt);

                List<ResultadoValidacion> resulRe = ResultadoValidacionDAL.ValoresRepetidosEx(pListaC);



                if (resulRe.Count > 0)
                {
                    listaCurrent.AddRange(resulRe);
                }
            }

            return listaCurrent;
        }

        public static List<DetaNaviera> AlmacenarArchivo(List<DetaNaviera> pLista, DBComun.Estado pEstado, int pCondicion)
        {


            List<DetaNaviera> listResul = new List<DetaNaviera>();
            List<DetaNaviera> list = new List<DetaNaviera>();

            list = (from a in pLista
                    join b in ContenedorDAL.ObtenerPrimerPos(pEstado) on a.c_tamaño.Substring(1 - 1, 1) equals b.IdValue
                    join c in ContenedorDAL.ObtenerSegundaPos(pEstado) on a.c_tamaño.Substring(2 - 1, 1) equals c.IdValue
                    join d in ContenedorDAL.ObtenerTerceraPos(pEstado) on a.c_tamaño.Substring(3 - 1, 1) equals d.IdCode
                    join f in ContenedorDAL.ObtenerOrden(pEstado) on a.b_estado.ToUpper() equals f.Valor.ToUpper()
                    orderby
                      f.Orden,
                      b.IdValue,
                      c.IdValue,
                      d.IdCode,
                      a.n_contenedor
                    select new DetaNaviera
                    {
                        IdDeta = a.IdDeta,
                        IdReg = a.IdReg,
                        n_BL = a.n_BL,
                        n_contenedor = a.n_contenedor,
                        c_tamaño = a.c_tamaño,
                        v_peso = a.v_peso,
                        b_estado = a.b_estado,
                        s_consignatario = a.s_consignatario,
                        n_sello = a.n_sello,
                        c_pais_destino = a.c_pais_destino,
                        c_pais_origen = a.c_pais_origen,
                        c_detalle_pais = a.c_detalle_pais,
                        s_comodity = a.s_comodity,
                        s_prodmanifestado = a.s_prodmanifestado,
                        v_tara = a.v_tara,
                        b_reef = a.b_reef,
                        b_ret_dir = a.b_ret_dir,
                        c_imo_imd = a.c_imo_imd,
                        c_un_number = a.c_un_number,
                        b_transhipment = a.b_transhipment,
                        c_condicion = a.c_condicion,
                        c_correlativo = a.c_correlativo
                    }).ToList();



            switch (pCondicion)
            {
                // Llenos
                case 1:
                    listResul = list.Where(a => a.b_estado.Contains('F') && !a.b_reef.Contains('Y') && !a.b_ret_dir.Contains('Y') && !a.b_transhipment.Contains('Y')).ToList();
                    break;
                // Vacios
                case 2:
                    listResul = list.Where(a => a.b_estado.Contains('E') && !a.b_reef.Contains('Y') & !a.b_ret_dir.Contains('Y') && !a.b_transhipment.Contains('Y')).ToList();
                    break;
                // Reef
                case 3:
                    listResul = list.Where(a => a.b_reef.Contains('Y') && !a.b_ret_dir.Contains('Y') && !a.b_transhipment.Contains('Y')).ToList();
                    break;
                // Trasbordo
                case 4:
                    listResul = list.Where(a => a.b_transhipment.Contains('Y') && !a.b_ret_dir.Contains('Y')).ToList();
                    break;
                // Despacho directo
                case 5:
                    listResul = list.Where(a => a.b_ret_dir.Contains('Y') && a.b_estado.Contains('F')).ToList();
                    break;
                // Despachi directo vacío
                case 6:
                    listResul = list.Where(a => a.b_ret_dir.Contains('Y') && a.b_estado.Contains('E')).ToList();
                    break;



            }

            // Revisar si es la misma cantidad que entro al orden numerico
            return listResul;
        }


        public static List<DetaNaviera> AlmacenarArchivoEx(List<DetaNaviera> pLista, DBComun.Estado pEstado, int pCondicion)
        {


            List<DetaNaviera> listResul = new List<DetaNaviera>();
            List<DetaNaviera> list = new List<DetaNaviera>();

            list = (from a in pLista
                    join b in ContenedorDAL.ObtenerPrimerPos(pEstado) on a.c_tamaño.Substring(1 - 1, 1) equals b.IdValue
                    join c in ContenedorDAL.ObtenerSegundaPos(pEstado) on a.c_tamaño.Substring(2 - 1, 1) equals c.IdValue
                    join d in ContenedorDAL.ObtenerTerceraPos(pEstado) on a.c_tamaño.Substring(3 - 1, 1) equals d.IdCode
                    join f in ContenedorDAL.ObtenerOrden(pEstado) on a.b_estado.ToUpper() equals f.Valor.ToUpper()
                    orderby
                      f.Orden,
                      b.IdValue,
                      c.IdValue,
                      d.IdCode,
                      a.n_contenedor
                    select new DetaNaviera
                    {
                        IdDeta = a.IdDeta,
                        IdReg = a.IdReg,
                        n_BL = a.n_BL,
                        n_contenedor = a.n_contenedor,
                        c_tamaño = a.c_tamaño,
                        v_peso = a.v_peso,
                        b_estado = a.b_estado,
                        s_consignatario = a.s_consignatario,
                        n_sello = a.n_sello,
                        c_pais_destino = a.c_pais_destino,
                        c_pais_origen = a.c_pais_origen,
                        c_detalle_pais = a.c_detalle_pais,
                        s_comodity = a.s_comodity,
                        s_prodmanifestado = a.s_prodmanifestado,
                        v_tara = a.v_tara,
                        b_reef = a.b_reef,
                        b_ret_dir = a.b_ret_dir,
                        c_imo_imd = a.c_imo_imd,
                        c_un_number = a.c_un_number,
                        b_transhipment = a.b_transhipment,
                        c_condicion = a.c_condicion,
                        c_correlativo = a.c_correlativo
                    }).ToList();



            switch (pCondicion)
            {
                // Llenos
                case 1:
                    listResul = list.Where(a => a.b_estado.Contains('F') && !a.b_reef.Contains('Y')).ToList();
                    break;
                // Vacios
                case 2:
                    listResul = list.Where(a => a.b_estado.Contains('E') && !a.b_reef.Contains('Y')).ToList();
                    break;
                // Reef
                case 3:
                    listResul = list.Where(a => a.b_reef.Contains('Y')).ToList();
                    break;
            }

            // Revisar si es la misma cantidad que entro al orden numerico
            return listResul;
        }

        public static List<ResultadoValidacion> ValidarBooking(string pRuta, DBComun.Estado pEstado)
        {
            List<ResultadoValidacion> validandoLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> listaCurrent = new List<ResultadoValidacion>();
            DBComun.sRuta = pRuta;

            List<ArchivoBooking> _list = ArchivoBookingDAL.GetRango(pRuta, DBComun.Estado.verdadero);

            foreach (ArchivoBooking item in _list)
            {
                if (item.n_contenedor != null && item.n_contenedor != "")
                {
                    ArchivoBooking _Archivo = new ArchivoBooking
                    {

                        num_fila = item.num_fila,
                        shipment = item.shipment.Trim().TrimEnd().TrimStart(),
                        n_contenedor = item.n_contenedor.Trim().TrimEnd().TrimStart(),
                        n_sello = item.n_sello.Trim().TrimEnd().TrimStart()
                    };

                    List<ResultadoValidacion> resulVal = ResultadoValidacionDAL.ValidarBooking(_Archivo);

                    listaCurrent = HttpContext.Current.Session["listaBooking"] as List<ResultadoValidacion>;
                }
            }
            return listaCurrent;
        }
        public static List<ArchivoAduana> EnvioArchivo(List<ArchivoAduana> pLista, DBComun.Estado pEstado, int pCondicion)
        {
            List<ArchivoAduana> listResul = new List<ArchivoAduana>();
            List<ArchivoAduana> list = new List<ArchivoAduana>();

            list = (from a in pLista
                    join b in ContenedorDAL.ObtenerPrimerPos(pEstado) on a.c_tamaño.Substring(1 - 1, 1) equals b.IdValue
                    join c in ContenedorDAL.ObtenerSegundaPos(pEstado) on a.c_tamaño.Substring(2 - 1, 1) equals c.IdValue
                    join d in ContenedorDAL.ObtenerTerceraPos(pEstado) on a.c_tamaño.Substring(3 - 1, 1) equals d.IdCode
                    join f in ContenedorDAL.ObtenerOrden(pEstado) on a.b_estado.ToUpper() equals f.Valor.ToUpper()
                    orderby
                      f.Orden,
                      b.IdValue,
                      c.IdValue,
                      d.IdCode,
                      a.n_contenedor
                    select new ArchivoAduana
                    {
                        n_contenedor = a.n_contenedor,
                        c_tamaño = a.c_tamaño,
                        v_peso = a.v_peso,
                        b_estado = a.b_estado,
                        s_consignatario = a.s_consignatario,
                        n_pais_destino = a.n_pais_destino,
                        n_pais_origen = a.n_pais_origen,
                        v_tara = a.v_tara,
                        b_reef = a.b_reef,
                        b_ret_dir = a.b_ret_dir,
                        b_tranship = a.b_tranship,
                        b_condicion = a.b_condicion,
                        c_correlativo = a.c_correlativo,
                        c_tamaño_c = a.c_tamaño_c,
                        b_estado_c = a.b_estado_c,
                        c_imo_imd = a.c_imo_imd,
                        c_un_number = a.c_un_number,
                        n_sello = a.n_sello,
                        s_comodity = a.s_comodity,
                        num_manif = a.num_manif,
                        b_transferencia = a.b_transferencia,
                        b_manejo = a.b_manejo,
                        b_despacho = a.b_despacho
                    }).ToList();



            switch (pCondicion)
            {
                // Llenos
                case 1:
                    listResul = list.Where(a => a.b_estado.Contains('F') && !a.b_reef.Contains("SI") && !a.b_ret_dir.Contains("SI") && !a.b_tranship.Contains("SI")).ToList();
                    break;
                // Vacios
                case 2:
                    listResul = list.Where(a => a.b_estado.Contains('E') && !a.b_reef.Contains("SI") && !a.b_ret_dir.Contains("SI") && !a.b_tranship.Contains("SI")).ToList();
                    break;
                // Reef
                case 3:
                    listResul = list.Where(a => a.b_reef.Contains("SI") && !a.b_ret_dir.Contains("SI") && !a.b_tranship.Contains("SI")).ToList();
                    break;
                // Trasbordo
                case 4:
                    listResul = list.Where(a => a.b_tranship.Contains("SI") && !a.b_ret_dir.Contains("SI")).ToList();
                    break;
                // Despacho directo
                case 5:
                    listResul = list.Where(a => a.b_ret_dir.Contains("SI") && a.b_estado.Contains('F')).ToList();
                    break;
                //Despacho directo vacío
                case 6:
                    listResul = list.Where(a => a.b_ret_dir.Contains("SI") && a.b_estado.Contains('E')).ToList();
                    break;
                // Peligrosidad
                case 7:
                    listResul = list.Where(a => a.c_imo_imd != "").ToList();
                    break;
            }

            // Revisar si es la misma cantidad que entro al orden numerico
            return listResul;
        }

        public static List<ArchivoAduana> orderByListado(List<ArchivoAduana> pLista, DBComun.Estado pEstado, int pCondicion)
        {
            List<ArchivoAduana> listResul = new List<ArchivoAduana>();
            List<ArchivoAduana> list = new List<ArchivoAduana>();

            list = (from a in pLista
                    select new ArchivoAduana
                    {
                        n_contenedor = a.n_contenedor,
                        c_tamaño = a.c_tamaño,
                        v_peso = a.v_peso,
                        b_estado = a.b_estado,
                        s_consignatario = a.s_consignatario,
                        n_pais_destino = a.n_pais_destino,
                        n_pais_origen = a.n_pais_origen,
                        v_tara = a.v_tara,
                        b_reef = a.b_reef,
                        b_ret_dir = a.b_ret_dir,
                        b_tranship = a.b_tranship,
                        b_condicion = a.b_condicion,
                        c_correlativo = a.c_correlativo,
                        c_tamaño_c = a.c_tamaño_c,
                        b_estado_c = a.b_estado_c,
                        c_imo_imd = a.c_imo_imd,
                        c_un_number = a.c_un_number,
                        n_sello = a.n_sello,
                        s_comodity = a.s_comodity,
                        num_manif = a.num_manif,
                        b_transferencia = a.b_transferencia,
                        b_manejo = a.b_manejo,
                        b_despacho = a.b_despacho
                    }).OrderBy(y => y.c_correlativo).ToList();



            switch (pCondicion)
            {
                // Llenos
                case 1:
                    listResul = list.Where(a => a.b_estado.Contains('F') && !a.b_reef.Contains("SI") && !a.b_ret_dir.Contains("SI") && !a.b_tranship.Contains("SI")).OrderBy(y => y.c_correlativo).ToList();
                    break;
                // Vacios
                case 2:
                    listResul = list.Where(a => a.b_estado.Contains('E') && !a.b_reef.Contains("SI") && !a.b_ret_dir.Contains("SI") && !a.b_tranship.Contains("SI")).OrderBy(y => y.c_correlativo).ToList();
                    break;
                // Reef
                case 3:
                    listResul = list.Where(a => a.b_reef.Contains("SI") && !a.b_ret_dir.Contains("SI") && !a.b_tranship.Contains("SI")).OrderBy(y => y.c_correlativo).ToList();
                    break;
                // Trasbordo
                case 4:
                    listResul = list.Where(a => a.b_tranship.Contains("SI") && !a.b_ret_dir.Contains("SI")).OrderBy(y => y.c_correlativo).ToList();
                    break;
                // Despacho directo
                case 5:
                    listResul = list.Where(a => a.b_ret_dir.Contains("SI") && a.b_estado.Contains('F')).OrderBy(y => y.c_correlativo).ToList();
                    break;
                //Despacho directo vacío
                case 6:
                    listResul = list.Where(a => a.b_ret_dir.Contains("SI") && a.b_estado.Contains('E')).OrderBy(y => y.c_correlativo).ToList();
                    break;
                // Peligrosidad
                case 7:
                    listResul = list.Where(a => a.c_imo_imd != "").OrderBy(y => y.c_correlativo).ToList();
                    break;
            }

            // Revisar si es la misma cantidad que entro al orden numerico
            return listResul;
        }

        public static List<Usuario> ObtenerUsuarios()
        {

            var _query = (from a in MenuDAL.ObtenerUser()
                          join b in EncaBuqueDAL.ObtenerNavieras1(DBComun.Estado.verdadero) on a.c_naviera equals b.c_cliente
                          select new Usuario
                          {
                              IdReg = a.IdReg,
                              c_usuario = a.c_usuario,
                              d_usuario = a.d_usuario,
                              d_naviera = b.d_nombre,
                              Habilitado = a.Habilitado
                          }).OrderBy(c => c.c_usuario).ToList();

            return _query;
        }
    }
}
