using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Web;


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

                    _command.Parameters.Add(new SqlParameter("@n_BL", pArchivo.n_BL));
                    _command.Parameters.Add(new SqlParameter("@n_booking", pArchivo.n_booking));
                    _command.Parameters.Add(new SqlParameter("@n_contenedor", pArchivo.n_contenedor));
                    _command.Parameters.Add(new SqlParameter("@c_tamaño", pArchivo.c_tamaño));
                    _command.Parameters.Add(new SqlParameter("@v_peso", Convert.ToDecimal(pArchivo.v_peso)));
                    _command.Parameters.Add(new SqlParameter("@b_estado", pArchivo.b_estado));
                    _command.Parameters.Add(new SqlParameter("@s_exportador", pArchivo.s_exportador));
                    _command.Parameters.Add(new SqlParameter("@s_consignatario", pArchivo.s_consignatario));                
                    _command.Parameters.Add(new SqlParameter("@s_notificador", pArchivo.s_notificador));
                    _command.Parameters.Add(new SqlParameter("@n_sello", pArchivo.n_sello));
                    _command.Parameters.Add(new SqlParameter("@c_pais_destino", pArchivo.c_pais_destino));
                    _command.Parameters.Add(new SqlParameter("@c_detalle_pais", pArchivo.c_detalle_puerto));
                    _command.Parameters.Add(new SqlParameter("@s_comodity", pArchivo.s_comodity));
                    _command.Parameters.Add(new SqlParameter("@v_tara", Convert.ToInt32(pArchivo.v_tara)));
                    _command.Parameters.Add(new SqlParameter("@num_celda", pArchivo.num_fila));
                    _command.Parameters.Add(new SqlParameter("@b_emb_dir", pArchivo.b_emb_dir));
                    _command.Parameters.Add(new SqlParameter("@b_reef", pArchivo.b_reef));
                    _command.Parameters.Add(new SqlParameter("@c_tipo_doc", pArchivo.c_tipo_doc));
                    _command.Parameters.Add(new SqlParameter("@c_arivu", pArchivo.c_arivu));
                    _command.Parameters.Add(new SqlParameter("@c_dm", pArchivo.c_dm));
                    _command.Parameters.Add(new SqlParameter("@c_dut", pArchivo.c_dut));
                    _command.Parameters.Add(new SqlParameter("@c_dmti", pArchivo.c_dmti));
                    _command.Parameters.Add(new SqlParameter("@c_manifiesto", pArchivo.c_manifiesto));
                    _command.Parameters.Add(new SqlParameter("@c_pais_origen", pArchivo.c_pais_origen));
                    _command.Parameters.Add(new SqlParameter("@b_transferencia", pArchivo.b_transferencia.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@b_manejo", pArchivo.b_manejo.Trim().TrimEnd().TrimStart()));
                    _command.Parameters.Add(new SqlParameter("@b_recepcion", pArchivo.b_recepcion.Trim().TrimEnd().TrimStart()));
                                                          	
                                                           
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
    }
}
