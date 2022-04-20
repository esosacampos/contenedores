using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace CEPA.CCO.DAL
{
    public class ResultadoValidacionDAL
    {      

        public static List<ResultadoValidacion> ValidarArchivo(ArchivoExcel pArchivo)
        {
            List<ResultadoValidacion> pLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> pListaAcumulada = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;

            if (pListaAcumulada == null)
                pListaAcumulada = new List<ResultadoValidacion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_naviera_validacion", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;

                
                _command.Parameters.Add(new SqlParameter("@n_BL", pArchivo.n_BL));
                _command.Parameters.Add(new SqlParameter("@n_contenedor", pArchivo.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@c_tamaño", pArchivo.c_tamaño));
                _command.Parameters.Add(new SqlParameter("@v_peso", Convert.ToDecimal(pArchivo.v_peso)));
                _command.Parameters.Add(new SqlParameter("@b_estado", pArchivo.b_estado));
                _command.Parameters.Add(new SqlParameter("@s_consignatario", pArchivo.s_consignatario));
                _command.Parameters.Add(new SqlParameter("@n_sello", pArchivo.n_sello));
                _command.Parameters.Add(new SqlParameter("@c_pais_destino", pArchivo.c_pais_destino));
                _command.Parameters.Add(new SqlParameter("@c_pais_origen", pArchivo.c_pais_origen));
                _command.Parameters.Add(new SqlParameter("@c_detalle_pais", pArchivo.c_detalle_pais));
                _command.Parameters.Add(new SqlParameter("@s_comodity", pArchivo.s_comodity));               
                _command.Parameters.Add(new SqlParameter("@v_tara", Convert.ToInt32(pArchivo.v_tara)));                
                _command.Parameters.Add(new SqlParameter("@c_imo_imd", pArchivo.c_imo_imd));
                _command.Parameters.Add(new SqlParameter("@c_un_number", pArchivo.c_un_number));
                _command.Parameters.Add(new SqlParameter("@c_condicion", pArchivo.c_condicion));
                _command.Parameters.Add(new SqlParameter("@b_reef", pArchivo.b_reef));
                _command.Parameters.Add(new SqlParameter("@b_shipper", pArchivo.b_shipper));
                _command.Parameters.Add(new SqlParameter("@b_ret_dir", pArchivo.b_ret_dir));
                _command.Parameters.Add(new SqlParameter("@b_tranship", pArchivo.b_transhipment));
                _command.Parameters.Add(new SqlParameter("@num_celda", pArchivo.num_fila));
                _command.Parameters.Add(new SqlParameter("@b_transferencia", pArchivo.b_transferencia.Trim().TrimEnd().TrimStart()));
                _command.Parameters.Add(new SqlParameter("@b_manejo", pArchivo.b_manejo.Trim().TrimEnd().TrimStart()));
                _command.Parameters.Add(new SqlParameter("@b_despacho", pArchivo.b_despacho.Trim().TrimEnd().TrimStart()));


                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ResultadoValidacion _resultado = new ResultadoValidacion
                    {
                        IdReg = (int)_reader.GetInt32(0),
                        NumFila = (int)_reader.GetInt32(1),
                        Descripcion = _reader.GetString(2),
                        Campo = _reader.GetString(3),
                        Resultado = (int)_reader.GetInt32(4),
                        NumCelda = _reader.GetString(5)
                    };

                    pLista.Add(_resultado);
                }

                pListaAcumulada.AddRange(pLista);
                HttpContext.Current.Session["listaValid"] = pListaAcumulada;
                _reader.Close();
                _conn.Close();
                return pLista;

            }
        }


        public static List<ResultadoValidacion> ValidarArchivoExport(List<ArchivoExport> pLista)
        {
            List<ResultadoValidacion> pListaC = new List<ResultadoValidacion>();
            List<ResultadoValidacion> pListaAcumulada = new List<ResultadoValidacion>();

            if (pLista == null)
                pLista = new List<ArchivoExport>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();

                foreach (ArchivoExport pArchivo in pLista)
	            {
		 
	
                    SqlCommand _command = new SqlCommand("pa_deta_exp_navi_validacion", _conn as SqlConnection);
                    _command.CommandType = CommandType.StoredProcedure;                        
                    
                    _command.Parameters.Add(new SqlParameter("@n_booking", pArchivo.n_booking));
                    _command.Parameters.Add(new SqlParameter("@n_contenedor", pArchivo.n_contenedor));
                    _command.Parameters.Add(new SqlParameter("@c_tamaño", pArchivo.c_tamaño));
                    _command.Parameters.Add(new SqlParameter("@v_peso", Convert.ToDecimal(pArchivo.v_peso)));
                    _command.Parameters.Add(new SqlParameter("@b_estado", pArchivo.b_estado));
                    _command.Parameters.Add(new SqlParameter("@s_consignatario", pArchivo.s_consignatario));
                    _command.Parameters.Add(new SqlParameter("@s_exportador", pArchivo.s_exportador));                    
                    _command.Parameters.Add(new SqlParameter("@nit_exportador", pArchivo.nit_exportador.Replace("-","")));
                    _command.Parameters.Add(new SqlParameter("@tel_exportador", pArchivo.tel_exportador.Replace("-","")));
                    _command.Parameters.Add(new SqlParameter("@em_exportador", pArchivo.em_exportador));
                    _command.Parameters.Add(new SqlParameter("@n_sello", pArchivo.n_sello));
                    _command.Parameters.Add(new SqlParameter("@c_pais_origen", pArchivo.c_pais_origen));
                    _command.Parameters.Add(new SqlParameter("@c_pais_destino", pArchivo.c_pais_destino));                    
                    _command.Parameters.Add(new SqlParameter("@c_pais_trasbordo", pArchivo.c_pais_trasbordo));
                    _command.Parameters.Add(new SqlParameter("@c_puerto_trasbordo", pArchivo.c_puerto_trasbordo));
                    _command.Parameters.Add(new SqlParameter("@s_comodity", pArchivo.s_comodity));
                    _command.Parameters.Add(new SqlParameter("@v_tara", Convert.ToInt32(pArchivo.v_tara)));
                    _command.Parameters.Add(new SqlParameter("@c_imo_imd", pArchivo.c_imo_imd));
                    _command.Parameters.Add(new SqlParameter("@c_un_number", pArchivo.c_un_number));
                    _command.Parameters.Add(new SqlParameter("@c_condicion", pArchivo.c_condicion));                    
                    _command.Parameters.Add(new SqlParameter("@b_emb_dir", pArchivo.b_emb_dir));                    
                    _command.Parameters.Add(new SqlParameter("@t_doc", pArchivo.c_tipo_doc));
                    _command.Parameters.Add(new SqlParameter("@n_doc", pArchivo.n_documento));
                    _command.Parameters.Add(new SqlParameter("@b_shipper", pArchivo.b_shipper));
                    _command.Parameters.Add(new SqlParameter("@s_almacenaje", pArchivo.s_almacenaje.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@s_transferencia", pArchivo.s_transferencia.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@s_manejo", pArchivo.s_manejo.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@s_recepcion", pArchivo.s_recepcion.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@b_trans", pArchivo.b_transhipment.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@s_nom_predio", pArchivo.s_nom_predio.Trim().TrimEnd().TrimStart()));                   
                    SqlParameter param = new SqlParameter("@f_venc_arivu", SqlDbType.Date);
                    param.Value = pArchivo.f_venc_arivu; //<-- Asegurate de que FechaClausura es de tipo DateTim
                    _command.Parameters.Add(param);

                    _command.Parameters.Add(new SqlParameter("@num_celda", pArchivo.num_fila));



                    SqlDataReader _reader = _command.ExecuteReader();
                                                          	 
                    while (_reader.Read())
                    {
                        ResultadoValidacion _resultado = new ResultadoValidacion
                        {
                            IdReg = (int)_reader.GetInt32(0),
                            NumFila = (int)_reader.GetInt32(1),
                            Descripcion = _reader.GetString(2),
                            Campo = _reader.GetString(3),
                            Resultado = (int)_reader.GetInt32(4),
                            NumCelda = _reader.GetString(5)
                        };

                        pListaC.Add(_resultado);
                    }                    

                    _reader.Close();

                }
                
                _conn.Close();
                return pListaC;

            }
        }

        public static List<ResultadoValidacion> ValoresRepetidos(List<ArchivoExcel> pLista)
        {
            List<ResultadoValidacion> pListaV = new List<ResultadoValidacion>();
            List<ResultadoValidacion> pListaAcumulada = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;

            List<ArchivoExcel> pListaClon = new List<ArchivoExcel>();

            ArrayList celdaLista = new ArrayList();
            int celda = 2;

              if (pListaAcumulada == null)
                  pListaAcumulada = new List<ResultadoValidacion>();


              Hashtable hTabla = new Hashtable();
              ArrayList duplicateList = new ArrayList();
              pListaClon = pLista;

              foreach (ArchivoExcel dtrow in pLista)
              {
                  if (hTabla.ContainsKey(dtrow.n_contenedor))
                  {
                      duplicateList.Add(dtrow.n_contenedor);
                      celdaLista.Add("E" + celda.ToString());
                  }
                  else
                  {
                      hTabla.Add(dtrow.n_contenedor, string.Empty);                      
                  }

                  celda = celda + 1;
              }
              
               
              foreach (var item in celdaLista)
              {

                  ResultadoValidacion _resultado = new ResultadoValidacion
                  {
                      IdReg = -1,
                      NumFila = Convert.ToInt32(item.ToString().Substring(1,item.ToString().Length - 1)),                      
                      Descripcion = " - ESTE NÚMERO DE CONTENEDOR SE ENCUENTRA REPETIDO EN ESTE LISTADO",
                      Campo = "n_contenedor",
                      Resultado = 1,
                      NumCelda = item.ToString()
                  };

                  pListaV.Add(_resultado);
              }



            pListaAcumulada.AddRange(pListaV);
            HttpContext.Current.Session["listaValid"] = pListaAcumulada;
            return pListaV;
        }

        public static List<ResultadoValidacion> ValoresRepetidosEx(List<ArchivoExport> pLista)
        {
            List<ResultadoValidacion> pListaV = new List<ResultadoValidacion>();
           

            List<ArchivoExport> pListaClon = new List<ArchivoExport>();

            ArrayList celdaLista = new ArrayList();
            int celda = 2;

          


            Hashtable hTabla = new Hashtable();
            ArrayList duplicateList = new ArrayList();
            pListaClon = pLista;

            foreach (ArchivoExport dtrow in pLista)
            {
                if (hTabla.ContainsKey(dtrow.n_contenedor))
                {
                    duplicateList.Add(dtrow.n_contenedor);
                    celdaLista.Add("E" + celda.ToString());
                }
                else
                {
                    hTabla.Add(dtrow.n_contenedor, string.Empty);
                }

                celda = celda + 1;
            }


            foreach (var item in celdaLista)
            {

                ResultadoValidacion _resultado = new ResultadoValidacion
                {
                    IdReg = -1,
                    NumFila = Convert.ToInt32(item.ToString().Substring(1, item.ToString().Length - 1)),
                    Descripcion = " - ESTE NÚMERO DE CONTENEDOR SE ENCUENTRA REPETIDO EN ESTE LISTADO",
                    Campo = "n_contenedor",
                    Resultado = 1,
                    NumCelda = item.ToString()
                };

                pListaV.Add(_resultado);
            }
           

            return pListaV;
        }

        public static List<ResultadoValidacion> ValoresRepetidos(List<ArchivoExcel> pLista, List<DetaNaviera> pTabla)
        {
            List<ResultadoValidacion> pListaV = new List<ResultadoValidacion>();
            List<ResultadoValidacion> pListaAcumulada = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;

            List<ArchivoExcel> pListaClon = new List<ArchivoExcel>();

            ArrayList celdaLista = new ArrayList();            

            if (pListaAcumulada == null)
                pListaAcumulada = new List<ResultadoValidacion>();



            var query = (from a in pTabla
                         join b in pLista on a.n_contenedor.TrimStart().TrimEnd() equals b.n_contenedor.TrimStart().TrimEnd()
                         select new
                         {
                             NumFila = b.num_fila,
                             Contenedor = b.n_contenedor,
                             Celda = "E" + b.num_fila
                         }).ToList();
                    


            foreach (var item in query)
            {

                ResultadoValidacion _resultado = new ResultadoValidacion
                {
                    IdReg = -1,
                    NumFila = item.NumFila,
                    Descripcion = " - ESTE NÚMERO DE CONTENEDOR YA HA SIDO REPORTADO REVISAR LISTADO(S) ANTERIOR(ES)",
                    Campo = "n_contenedor",
                    Resultado = 1,
                    NumCelda = item.Celda
                };

                pListaV.Add(_resultado);
            }



            pListaAcumulada.AddRange(pListaV);
            HttpContext.Current.Session["listaValid"] = pListaAcumulada;
            return pListaV;

        }

        public static List<ResultadoValidacion> ValoresRepetidosEx(List<ArchivoExport> pLista, List<DetaNaviera> pTabla)
        {
            List<ResultadoValidacion> pListaV = new List<ResultadoValidacion>();
            //List<ResultadoValidacion> pListaAcumulada = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;

            List<ArchivoExport> pListaClon = new List<ArchivoExport>();

            ArrayList celdaLista = new ArrayList();


            var query = (from a in pTabla
                         join b in pLista on a.n_contenedor.TrimStart().TrimEnd() equals b.n_contenedor.TrimStart().TrimEnd()
                         select new
                         {
                             NumFila = b.num_fila,
                             Contenedor = b.n_contenedor,
                             Celda = "E" + b.num_fila
                         }).ToList();



            foreach (var item in query)
            {

                ResultadoValidacion _resultado = new ResultadoValidacion
                {
                    IdReg = -1,
                    NumFila = item.NumFila,
                    Descripcion = " - ESTE NÚMERO DE CONTENEDOR YA HA SIDO REPORTADO REVISAR LISTADO(S) ANTERIOR(ES)",
                    Campo = "n_contenedor",
                    Resultado = 1,
                    NumCelda = item.Celda
                };

                pListaV.Add(_resultado);
            }
            
            return pListaV;

        }

        public static List<ResultadoValidacion> ValidarBooking(ArchivoBooking pArchivo)
        {
            List<ResultadoValidacion> pLista = new List<ResultadoValidacion>();
            List<ResultadoValidacion> pListaAcumulada = HttpContext.Current.Session["listaBooking"] as List<ResultadoValidacion>;

            if (pListaAcumulada == null)
                pListaAcumulada = new List<ResultadoValidacion>();

            using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
            {
                _conn.Open();
                SqlCommand _command = new SqlCommand("pa_deta_booking_validacion", _conn as SqlConnection);
                _command.CommandType = CommandType.StoredProcedure;               
               
                _command.Parameters.Add(new SqlParameter("@n_contenedor", pArchivo.n_contenedor));
                _command.Parameters.Add(new SqlParameter("@n_sello", pArchivo.n_sello));
                _command.Parameters.Add(new SqlParameter("@shipment_name", pArchivo.shipment));
                _command.Parameters.Add(new SqlParameter("@num_celda", pArchivo.num_fila));

                SqlDataReader _reader = _command.ExecuteReader();

                while (_reader.Read())
                {
                    ResultadoValidacion _resultado = new ResultadoValidacion
                    {
                        IdReg = (int)_reader.GetInt32(0),
                        NumFila = (int)_reader.GetInt32(1),
                        Descripcion = _reader.GetString(2),
                        Campo = _reader.GetString(3),
                        Resultado = (int)_reader.GetInt32(4),
                        NumCelda = _reader.GetString(5)
                    };

                    pLista.Add(_resultado);
                }

                pListaAcumulada.AddRange(pLista);
                HttpContext.Current.Session["listaBooking"] = pListaAcumulada;
                _reader.Close();
                _conn.Close();
                return pLista;

            }
        }

        public static List<ResultadoValidacion> ValidarRetenidosCEPAExport(List<ArchivoExport> pLista)
        {
            List<ResultadoValidacion> pListaC = new List<ResultadoValidacion>();

            if (pLista == null)
                pLista = new List<ArchivoExport>();
            //List <RetenidoCEPA> retCEPA = GetRetenidosCEPA();
            //if (retCEPA.Count > 0)
            //{
                //int i = 0;
                var query = (from lstExc in pLista
                             join lstRet in GetRetenidosCEPA() on lstExc.n_contenedor equals lstRet.contenedor
                             orderby lstExc.num_fila
                                 select new ResultadoValidacion
                                 {
                                     IdReg = 0, //i++,
                                     NumFila = lstExc.num_fila,
                                     Descripcion = lstRet.motivo.Trim(),
                                     Campo = "n_contenedor",
                                     Resultado = 1,
                                     NumCelda = "D" + lstExc.num_fila.ToString()
                                 }
                             ).ToList();
            //}
            return pListaC;
        }

        public static List<RetenidoCEPA> GetRetenidosCEPA()
        {
            List<RetenidoCEPA> _contenedores = new List<RetenidoCEPA>();
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure();
            proceso.NBase = "CONTENEDORES";
            proceso.Procedimiento = "wa_retenido_cepa_todos";
            proceso.Parametros = new List<Parametros>();
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<RetenidoCEPA> Conectar(List<RetenidoCEPA> _contenedores, string apiUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file = string.Empty;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
                DataTable tabla = JsonConvert.DeserializeObject<DataTable>(file) as DataTable;
                if (tabla.Rows.Count > 0)
                {
                    if (!tabla.Rows[0][0].ToString().StartsWith("ERROR"))
                    {
                        _contenedores = tabla.ToList<RetenidoCEPA>();
                    }
                }
            }
            return _contenedores;
        }

        //public static List<ResultadoValidacion> ValidarRetenidosCEPAExport2(List<ArchivoExport> pLista)
        //{
        //    List<ResultadoValidacion> pListaC = new List<ResultadoValidacion>();
        //    List<ResultadoValidacion> pListaAcumulada = new List<ResultadoValidacion>();

        //    if (pLista == null)
        //        pLista = new List<ArchivoExport>();

        //    using (IDbConnection _conn = DBComun.ObtenerConexion(DBComun.TipoBD.SqlServer, DBComun.Estado.verdadero))
        //    {
        //        _conn.Open();

        //        foreach (ArchivoExport pArchivo in pLista)
        //        {


        //            SqlCommand _command = new SqlCommand("pa_deta_exp_navi_validacion", _conn as SqlConnection);
        //            _command.CommandType = CommandType.StoredProcedure;

        //            _command.Parameters.Add(new SqlParameter("@n_booking", pArchivo.n_booking));
        //            _command.Parameters.Add(new SqlParameter("@n_contenedor", pArchivo.n_contenedor));
        //            _command.Parameters.Add(new SqlParameter("@c_tamaño", pArchivo.c_tamaño));
        //            _command.Parameters.Add(new SqlParameter("@v_peso", Convert.ToDecimal(pArchivo.v_peso)));
        //            _command.Parameters.Add(new SqlParameter("@b_estado", pArchivo.b_estado));
        //            _command.Parameters.Add(new SqlParameter("@s_consignatario", pArchivo.s_consignatario));
        //            _command.Parameters.Add(new SqlParameter("@s_exportador", pArchivo.s_exportador));
        //            _command.Parameters.Add(new SqlParameter("@nit_exportador", pArchivo.nit_exportador.Replace("-", "")));
        //            _command.Parameters.Add(new SqlParameter("@tel_exportador", pArchivo.tel_exportador.Replace("-", "")));
        //            _command.Parameters.Add(new SqlParameter("@em_exportador", pArchivo.em_exportador));
        //            _command.Parameters.Add(new SqlParameter("@n_sello", pArchivo.n_sello));
        //            _command.Parameters.Add(new SqlParameter("@c_pais_origen", pArchivo.c_pais_origen));
        //            _command.Parameters.Add(new SqlParameter("@c_pais_destino", pArchivo.c_pais_destino));
        //            _command.Parameters.Add(new SqlParameter("@c_pais_trasbordo", pArchivo.c_pais_trasbordo));
        //            _command.Parameters.Add(new SqlParameter("@c_puerto_trasbordo", pArchivo.c_puerto_trasbordo));
        //            _command.Parameters.Add(new SqlParameter("@s_comodity", pArchivo.s_comodity));
        //            _command.Parameters.Add(new SqlParameter("@v_tara", Convert.ToInt32(pArchivo.v_tara)));
        //            _command.Parameters.Add(new SqlParameter("@c_imo_imd", pArchivo.c_imo_imd));
        //            _command.Parameters.Add(new SqlParameter("@c_un_number", pArchivo.c_un_number));
        //            _command.Parameters.Add(new SqlParameter("@c_condicion", pArchivo.c_condicion));
        //            _command.Parameters.Add(new SqlParameter("@b_emb_dir", pArchivo.b_emb_dir));
        //            _command.Parameters.Add(new SqlParameter("@t_doc", pArchivo.c_tipo_doc));
        //            _command.Parameters.Add(new SqlParameter("@n_doc", pArchivo.n_documento));
        //            _command.Parameters.Add(new SqlParameter("@b_shipper", pArchivo.b_shipper));
        //            _command.Parameters.Add(new SqlParameter("@s_almacenaje", pArchivo.s_almacenaje.Trim().TrimEnd().TrimStart()));
        //            _command.Parameters.Add(new SqlParameter("@s_transferencia", pArchivo.s_transferencia.Trim().TrimEnd().TrimStart()));
        //            _command.Parameters.Add(new SqlParameter("@s_manejo", pArchivo.s_manejo.Trim().TrimEnd().TrimStart()));
        //            _command.Parameters.Add(new SqlParameter("@s_recepcion", pArchivo.s_recepcion.Trim().TrimEnd().TrimStart()));
        //            _command.Parameters.Add(new SqlParameter("@b_trans", pArchivo.b_transhipment.Trim().TrimEnd().TrimStart()));
        //            _command.Parameters.Add(new SqlParameter("@s_nom_predio", pArchivo.s_nom_predio.Trim().TrimEnd().TrimStart()));
        //            SqlParameter param = new SqlParameter("@f_venc_arivu", SqlDbType.Date);
        //            param.Value = pArchivo.f_venc_arivu; //<-- Asegurate de que FechaClausura es de tipo DateTim
        //            _command.Parameters.Add(param);

        //            _command.Parameters.Add(new SqlParameter("@num_celda", pArchivo.num_fila));



        //            SqlDataReader _reader = _command.ExecuteReader();

        //            while (_reader.Read())
        //            {
        //                ResultadoValidacion _resultado = new ResultadoValidacion
        //                {
        //                    IdReg = (int)_reader.GetInt32(0),
        //                    NumFila = (int)_reader.GetInt32(1),
        //                    Descripcion = _reader.GetString(2),
        //                    Campo = _reader.GetString(3),
        //                    Resultado = (int)_reader.GetInt32(4),
        //                    NumCelda = _reader.GetString(5)
        //                };

        //                pListaC.Add(_resultado);
        //            }

        //            _reader.Close();

        //        }

        //        _conn.Close();
        //        return pListaC;

        //    }
        //}
    }
}
