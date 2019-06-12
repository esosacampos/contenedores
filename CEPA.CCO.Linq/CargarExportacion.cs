using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using CEPA.CCO.Linq;
using Microsoft.Office.Interop.Excel;
using System.Web;
using System.IO;
using ClosedXML.Excel;

namespace CEPA.CCO.Linq
{
    [System.Runtime.InteropServices.GuidAttribute("7888C528-1639-4509-AEC2-7D57590667E5")]
    public class CargarExportacion
    {
        #region "propiedades"
        public bool Save { get; set; }
        string a;
        int IdRegEnca;
        int IdRegReal;
        string Html;
        DateTime _fecha;
               
        public string c_imo { get; set; }
        public string c_llegada { get; set; }
        public string c_usuario { get; set; }
        public DateTime f_llegada { get; set; }
        public string d_buque { get; set; }
        public int n_manifiesto { get; set; }
        public string c_manifiesto { get; set; }
        public string c_voyage { get; set; }
        public int IdRegC { get; set; }
        public int IdDocC { get; set; }
        public string a_manip { get; set; }
        public int sustitucion { get; set; }
        public string arch_susti { get; set; }

        #endregion


        public void CorreoRecepcion1(string cn_voyage)
        {
            string Html1;
            EnvioCorreo _correo1 = new EnvioCorreo();

            Html1 = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html1 += "<br />MÓDULO : RECEPCIÓN DE LISTADO DE EXPORTACIÓN POR SUSTITUCIÓN <br />";
            Html1 += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE ARCHIVO POR SUSTITUCIÓN <br /><br />";
            Html1 += string.Format("Se notifica que la recepción del archivo de exportación por sustitución, de {0} para el buque {1}, número de Viaje {2}, en la cual solicitan la sustitución del archivo {4}, y queda en espera de resultados de validación", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, arch_susti);


            Html1 += "<br /><br />";
            Html1 += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
            Html1 += "<font color=blue> En espera de validación de CEPA </font>";

            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);

            _correo1.Subject = string.Format("PASO 1 de 4: Recepción CEPA: Listado de Exportación por SUSTITUCIÓN de {0} para el Buque: {1}, # de Viaje {2}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);
            _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga_exp", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga_exp", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

            //Notificaciones _notiCC = new Notificaciones
            //{
            //    IdNotificacion = -1,
            //    sMail = "elsa.sosa@cepa.gob.sv",
            //    dMail = "Elsa Sosa"
            //};

            //List<Notificaciones> _ccList = new List<Notificaciones>();
            //_ccList.Add(_notiCC);

            //_correo1.ListaNoti = _ccList;

            _correo1.Asunto = Html1;
            _correo1.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);

        }

        private void CorreoRecepcion(string cn_voyage)
        {
            string Html1 = null;
            EnvioCorreo _correo1 = new EnvioCorreo();

            Html1 = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html1 += "<br />MÓDULO : RECEPCIÓN DE LISTADO DE EXPORTACIÓN <br />";
            Html1 += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE ARCHIVO <br /><br />";
            Html1 += string.Format("Se notifica que la recepción del archivo de exportación, de {0} para el buque {1}, número de Viaje {2}, ha sido recibido y queda en espera de resultados de validación", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);
            Html1 += "<br /><br />";
            Html1 += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
            Html1 += "<font color=blue> En espera de validación de CEPA </font>";

            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);

            _correo1.Subject = string.Format("PASO 1 de 4: Recepción CEPA: Listado de Exportación de {0} para el Buque: {1}, # de Viaje {2}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);
            _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga_exp", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga_exp", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

            //Notificaciones _notiCC = new Notificaciones
            //{
            //    IdNotificacion = -1,
            //    sMail = "elsa.sosa@cepa.gob.sv",
            //    dMail = "Elsa Sosa"
            //};

            //List<Notificaciones> _ccList = new List<Notificaciones>();
            //_ccList.Add(_notiCC);

            //_correo1.ListaNoti = _ccList;


            _correo1.Asunto = Html1;
            _correo1.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
            _correo1 = null;
        }     

        public void AlmacenaCargaExport()
        {
            Save = false;
            string _actSusti = null;
            List<int> _listaDeta = new List<int>();
            int _resulDeta = 0;

            if (sustitucion == 1)
            {
                if (arch_susti == string.Empty || arch_susti.Length == 0)
                {

                    throw new Exception("Seleccione el archivo a reemplazar");
                }


            }

            //Si existe archivo a cargar 
            if (HttpContext.Current.Session["ruta"] != null && HttpContext.Current.Session["archivo"] != null)
                a = HttpContext.Current.Session["ruta"].ToString();
            else
                throw new Exception("Debe seleccionar el archivo a cargar");


            //Si el archivo ya existe para el caso de la carga 

            if (sustitucion == 0)
            {
                int _exis = Convert.ToInt32(DetaDocDAL.ArchivosExistentesEx(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString(), HttpContext.Current.Session["archivo"].ToString()));

                if (_exis > 0)
                    throw new Exception("Ya existe un archivo almacenado con este nombre");

            }


            // Buscar el voyage del archivo //Validar el voyage           

            string cn_voyage = null;
            DBComun.sRuta = a;
            string listVoyage = ArchivoExcelDAL.GetVoyageEx(a, DBComun.Estado.verdadero);



            if (listVoyage.Length > 0)
                cn_voyage = listVoyage;
            else
                throw new Exception("No se encuentra número de Viaje");

            //Verificar archivos almacenados.

            int _arch = Convert.ToInt32(DetaDocDAL.ArchivosAlmacenadosEx(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString()));

            if (_arch > 0)
            {
                IdRegEnca = Convert.ToInt32(EncaNavieraDAL.ObtenerIdRegEx(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString()));
            }

            /*if (sustitucion == 1)
                _actSusti = EncaNavieraDAL.ActSustitucion(IdRegEnca, 1);*/

            //Validar que el imo coincida con el del archivo

            string c_imoVa = ArchivoExcelDAL.GetImoEx(a, DBComun.Estado.verdadero);
            string Mensaje = "";

            if (c_imoVa == null)
                throw new Exception("El número de imo indicado presenta errores");

            if (Convert.ToInt32(c_imoVa) > 0)
            {
                if (c_imo != c_imoVa)
                {
                    Mensaje = "El IMO del archivo no coincide con el del buque seleccionado";
                    //HttpContext.Current.Response.Write("<script>alert('" + Mensaje + "');</script>");
                    throw new Exception(Mensaje);

                }
            }
            else
                throw new Exception("El número de IMO indicado presenta errores en su formato, elimine espacios y verifique número");


            // Validación Nombre de Archivo.
            string nombre = Path.GetFileName(a);
            bool ErrorNombre;

            _fecha = DetaNavieraLINQ.FechaBD();

            #region "Codigo HTML"
            Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";

            Html += "<b><u> CARGA DE ARCHIVO </b></u><br />";
            Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;\">";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
            Html += "</tr>";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + c_usuario + "</font></td>";
            Html += "</tr>";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\" ><font size = 2>Archivo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + nombre + "</font></td>";
            Html += "</tr>";
            Html += "</table>";
            Html += "<br />";
            Html += "<b><u> VALIDANDO NOMBRE </b></u><br />";

            #endregion

            if (nombre.Trim().Length == 0)
                throw new Exception("NO SE HA ESPECIFICADO UN NOMBRE DE ARCHIVO");

            if (sustitucion == 1)
            {
                if (nombre != arch_susti)
                {
                    throw new Exception("El nombre del archivo debe ser igual que el nombre del archivo a sustituir");
                }
            }

            string extension = Path.GetExtension(a);
            if (Path.GetExtension(a) != ".xls" && Path.GetExtension(a) != ".xlsx")
                throw new System.InvalidOperationException("La extensión del archivo no es válida, revisar que sea un libro de Excel");

            int ar = Path.GetFileName(a).Length;
            int ar1 = Path.GetExtension(a).Length;
            int ar3 = 19 + d_buque.Length;

            if ((Path.GetFileName(a).Length - Path.GetExtension(a).Length) != (19 + d_buque.Length))
                throw new System.InvalidOperationException("Longitud de nombre de archivo no es válida");

            int countHojas = ArchivoExcelDAL.GetHojasEx(a, DBComun.Estado.verdadero);

            if (countHojas > 1)
                throw new Exception("Libro de Excel posee más de una hoja activa");

            int countColumnas = ArchivoExcelDAL.GetNumberOfColumns(a);

            if (countColumnas != 30)
                throw new Exception("Hoja debe tener 30 columnas que son (A-AD) las permitidas");
           
            EnvioCorreo _correo = new EnvioCorreo();

            Html += "<font size=2>";

            if (sustitucion == 0)
                ErrorNombre = ValidarArchivoD(nombre, IdRegEnca);
            else
                ErrorNombre = ValidarArchivoD(nombre, IdRegEnca, d_buque, f_llegada, arch_susti);


            // Validar viaje
            if (sustitucion == 0)
            {
                string _resultadoV = null;

                if (nombre.Trim().Substring(17 + d_buque.Length, 2) == "01")
                {
                    _resultadoV = ArchivoExcelDAL.ValidarViajeCEx(DBComun.Estado.verdadero, c_imoVa, cn_voyage, HttpContext.Current.Session["c_naviera"].ToString());
                    if (_resultadoV != "NULL" || _resultadoV == "Error")
                        throw new Exception("ESTE IMO YA PRESENTO ESTE NÚMERO DE VIAJE");
                }
            }

            //VALIDANDO ARCHIVO DE EXCEL

            List<ArchivoExport> pExport = new List<ArchivoExport>();
            
            pExport = ArchivoExcelDAL.GetListaEx(a, DBComun.Estado.verdadero);

            if(pExport.Count > 0)
            {
                
                    if (ErrorNombre == false)
                    {
                        int del = 0;
                        if (sustitucion == 1)
                        {
                            del = Convert.ToInt32(DetaNavieraDAL.EliminarDetaNavieraEx(IdRegEnca, nombre));
                        }


                        List<ResultadoValidacion> pTotalList = new List<ResultadoValidacion>();
                        List<ResultadoValidacion> validarPrueba = new List<ResultadoValidacion>();
                        List<ResultadoValidacion> listaVaTa = new List<ResultadoValidacion>();

                        validarPrueba = DetaNavieraLINQ.ValidarDetalleEx(a, DBComun.Estado.verdadero);

                        List<DetaNaviera> pListaExist = DetaNavieraDAL.ObtenerRegAnterEx(c_imo, c_llegada, cn_voyage);

                        if (pListaExist == null)
                            pListaExist = new List<DetaNaviera>();

                        if (pListaExist.Count > 0)
                            listaVaTa = DetaNavieraLINQ.ValidarDetalleEx(a, pListaExist, DBComun.Estado.verdadero);


                        if (validarPrueba.Count > 0)
                            pTotalList.AddRange(validarPrueba);

                        if (listaVaTa.Count > 0)
                            pTotalList.AddRange(listaVaTa);

                        //Errores en la validacion
                        if (pTotalList.Count > 0)
                        {
                            #region "INCONSISTENCIAS"
                            if (sustitucion == 0)
                                CorreoRecepcion(cn_voyage);

                            else
                            {
                                CorreoRecepcion1(cn_voyage);
                            }

                            Html += "<b><u>DETALLE DE INCONSISTENCIAS</b></u><br />";
                            Html += "<br />";

                            Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                            Html += "<tr>";
                            Html += "<center>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>IMO SHIP</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>VOYAGE</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>BL</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>CONTAINER</font></th>";
                            Html += "<td width=\"7%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>SYZE/TYPE</font></th>";
                            Html += "<td width=\"7%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>WEIGHT</font></th>";
                            Html += "<td width=\"7%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>FULL/EMPTY</font></th>";
                            Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>EXPORTADOR</font></th>";
                            Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>CONSIGNATARIO</font></th>";
                            Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>NOTIFICADOR</font></th>";
                            Html += "<td width=\"8%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>SEAL NUMBER</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>PAIS DESTINO</font></th>";
                            Html += "<td width=\"7%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>PAIS ORIGEN</font></th>";
                            Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>DESCRIPCION</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>TARE</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>REFEER</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>EMB DIR</font></th>";
                            Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>TIPO DOCUMENTO</font></th>";
                            Html += "</center>";
                            Html += "</tr>";

                            var query = (from arch in pExport
                                         join valid in pTotalList.Where(t => t.Resultado == 1) on arch.num_fila equals valid.NumFila
                                         orderby arch.num_fila
                                         select new
                                         {
                                             valid.NumFila,
                                             valid.NumCelda,
                                             valid.Descripcion
                                         }).ToList();

                            var queryTodos = (from arch in ArchivoExcelDAL.GetListaEx(a, DBComun.Estado.verdadero)
                                              where
                                                 (from valid in pTotalList
                                                  where valid.Resultado == 1
                                                  select new
                                                  {
                                                      valid.NumFila
                                                  }).Contains(new { NumFila = arch.num_fila })
                                              select new ArchivoExport
                                              {
                                                  num_fila = arch.num_fila,
                                                  n_BL = arch.n_BL,
                                                  n_booking = arch.n_booking,
                                                  n_contenedor = arch.n_contenedor,
                                                  c_tamaño = arch.c_tamaño,
                                                  v_peso = arch.v_peso,
                                                  b_estado = arch.b_estado,
                                                  s_exportador = arch.s_exportador,
                                                  s_consignatario = arch.s_consignatario,
                                                  s_notificador = arch.s_notificador,
                                                  n_sello = arch.n_sello,
                                                  c_pais_destino = arch.c_pais_destino,
                                                  c_pais_origen = arch.c_pais_origen,
                                                  s_comodity = arch.s_comodity,
                                                  s_prodmanifestado = arch.s_prodmanifestado,
                                                  v_tara = arch.v_tara,
                                                  b_reef = arch.b_reef,
                                                  b_emb_dir = arch.b_emb_dir,
                                                  c_tipo_doc = arch.c_tipo_doc
                                              }).ToList();


                            foreach (ArchivoExport itemArch in queryTodos)
                            {
                                Html += "<tr>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + c_imoVa + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + cn_voyage + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.n_BL + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.n_contenedor + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_tamaño + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.v_peso + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.b_estado + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.s_exportador + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.s_consignatario + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.s_notificador + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.n_sello + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_pais_destino + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_pais_origen + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.s_prodmanifestado + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.v_tara + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.b_reef + "</font></td>";
                                Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.b_emb_dir + "</font></td>";
                                Html += "<td height=\"25\"><font size=1 color=blue>" + itemArch.c_tipo_doc + "</font></td>";
                                Html += "</tr>";
                                Html += "</font>";
                                Html += "<tr>";
                                Html += "<td colspan=18>";

                                //List<ResultadoValidacion> _lista = queryTodos.Where(p => p.num_fila == itemArch.num_fila).ToList() as List<ResultadoValidacion>;
                                var _lista = query.Where(p => p.NumFila == itemArch.num_fila).ToList();
                                if (_lista.Count > 0)
                                {
                                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                                    Html += "<tr>";
                                    Html += "<td width=\"5%\" height=\"25\"><font color=red size=1></font></th>";
                                    Html += "<td width=\"5%\" height=\"25\"><font color=red size=1>FILA</font></th>";
                                    Html += "<td width=\"5%\" height=\"25\"><font color=red size=1>CELDA</font></th>";
                                    Html += "<td width=\"25%\" height=\"25\"><font color=red size=1>DESCRIPCION</font></th>";
                                    Html += "</tr>";
                                    foreach (var itemRela in _lista)
                                    {
                                        Html += "<td height=\"25\"><font size=1 color=red>  </font></td>";
                                        Html += "<td height=\"25\"><font size=1 color=red>" + itemRela.NumFila + "</font></td>";
                                        Html += "<td height=\"25\"><font size=1 color=red>" + itemRela.NumCelda + "</font></td>";
                                        Html += "<td height=\"25\"><font size=1 color=red>" + itemRela.Descripcion + "</font></td>";
                                        Html += "</tr>";
                                    }
                                    Html += "</font></table>";
                                }
                            }
                            Html += "</td></tr></font></table>";


                            Html += "</font>";
                            Html += "<br /><br />";
                            Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                            Html += "<font color=red>En espera de correcciones por la Naviera </font>";


                            if (sustitucion == 0)
                                _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA: DENEGADA Listado de Exportación de {0} para el Buque: {1}, # de Viaje {2}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);
                            else
                                _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): DENEGADA Listado de Exportación Buque: {0}, # de Viaje {1}", d_buque, cn_voyage);


                            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga_exp", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                            List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga_exp", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                            if (_listaCC == null)
                                _listaCC = new List<Notificaciones>();

                            _correo.ListaCC = _listaCC;

                            //Notificaciones _notiCC = new Notificaciones
                            //{
                            //    IdNotificacion = -1,
                            //    sMail = "elsa.sosa@cepa.gob.sv",
                            //    dMail = "Elsa Sosa"
                            //};

                            //List<Notificaciones> _ccList = new List<Notificaciones>();
                            //_ccList.Add(_notiCC);

                            //_correo.ListaNoti = _ccList;
                            _correo.ListArch.Add(a);
                            _correo.Asunto = Html;
                            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                            // _correo = null;
                            _actSusti = EncaNavieraDAL.ActSustitucion(IdRegEnca, 0);

                            throw new Exception("Archivo presento inconsistencias revisar correo electrónico");


                            #endregion
                        }
                        else
                        {
                            #region "ACEPTADO"
                            if (sustitucion == 0)
                            {

                                CorreoRecepcion(cn_voyage);

                                int _resultado = 0;
                                if (_arch > 0)
                                    IdRegReal = IdRegEnca;
                                else
                                {
                                    EncaNaviera _encaNavi = new EncaNaviera
                                    {
                                        IdReg = -1,
                                        c_imo = c_imo,
                                        c_usuario = HttpContext.Current.Session["c_usuario"].ToString(),
                                        c_naviera = HttpContext.Current.Session["c_naviera"].ToString(),
                                        c_llegada = c_llegada,
                                        c_voyage = cn_voyage,
                                        f_llegada = Convert.ToDateTime(f_llegada.ToString("dd/MM/yyyy hh:mm tt"))
                                    };

                                    _resultado = Convert.ToInt32(EncaNavieraDAL.AlmacenarEncaNavieraEx(_encaNavi));
                                    IdRegReal = _resultado;
                                }



                                if (IdRegReal > 0)
                                {
                                    DetaDoc _detaDoc = new DetaDoc
                                    {
                                        IdDoc = -1,
                                        IdReg = IdRegReal,
                                        s_archivo = HttpContext.Current.Session["archivo"].ToString(),
                                        c_imo = c_imo,
                                        c_usuario = HttpContext.Current.Session["c_usuario"].ToString(),
                                        c_llegada = c_llegada
                                    };

                                    int _docResul = Convert.ToInt32(DetaDocDAL.AlmacenarDocNaviEx(_detaDoc));


                                    if (_docResul > 0)
                                    {

                                        _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarDetaNavieraEx(pExport, IdRegReal, _docResul));

                                        if (_resulDeta > 0)
                                            Save = true;





                                    }
                                }


                                Html += "<b><u>DETALLE DE CARGA DE LISTADO</b></u><br />";
                                Html += "<br /> Módulo : Carga de listado de contenedores de exportación <br />";
                                Html += "Mensaje : Notificación carga de listado exitosa <br /><br />";
                                Html += string.Format("Se notifica que la carga del archivo de exportación, de {0} para el buque: {1}, # de Viaje {2}, se validaron {3} contenedores de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, pExport.Count);
                                Html += "<br /><br />";
                                Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                                Html += "<font color=blue> En espera de validación de ADUANA </font>";

                                Html += "</font>";


                                //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
                                _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA: ACEPTADA Listado de Exportación de {0} para el Buque: {1}, # de Viaje {2}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);
                                _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                                List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                if (_listaCC == null)
                                    _listaCC = new List<Notificaciones>();

                                _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.verdadero, "219"));
                                _correo.ListaCC = _listaCC;

                                //Notificaciones _noticc = new Notificaciones
                                //{
                                //    IdNotificacion = -1,
                                //    sMail = "elsa.sosa@cepa.gob.sv",
                                //    dMail = "elsa sosa"
                                //};

                                //List<Notificaciones> _cclist = new List<Notificaciones>();
                                //_cclist.Add(_noticc);

                                //_correo.ListaNoti = _cclist;

                                _correo.Asunto = Html;
                                _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                                _correo = null;

                                if (Save == true)
                                    throw new Exception("Archivo registrado correctamente");


                            }
                            else
                            {
                                CorreoRecepcion1(cn_voyage);
                                //SI ES SUSTITUCION
                                int IdDoc = 0;
                                int IdReg = 0;
                                List<DetaDoc> listaDoc = DetaDocDAL.ObtenerDocAEx(DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString(), c_llegada, arch_susti);

                                if (listaDoc.Count == 1)
                                {
                                    foreach (DetaDoc idRegV in listaDoc)
                                    {
                                        IdDoc = idRegV.IdDoc;
                                        IdReg = idRegV.IdReg;
                                        break;
                                    }
                                    foreach (DetaDoc item1 in listaDoc)
                                    {

                                        if (del >= 0)
                                        {
                                            DetaDoc _detaDoc = new DetaDoc
                                            {
                                                IdDoc = -1,
                                                IdReg = item1.IdReg,
                                                s_archivo = HttpContext.Current.Session["archivo"].ToString(),
                                                c_imo = c_imo,
                                                c_usuario = HttpContext.Current.Session["c_usuario"].ToString(),
                                                c_llegada = c_llegada
                                            };

                                            int _docResul = Convert.ToInt32(DetaDocDAL.AlmacenarDocNaviEx(_detaDoc));

                                            if (_docResul > 0)
                                            {
                                                _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarDetaNavieraEx(pExport, IdReg, _docResul));

                                                if (_resulDeta > 0)
                                                {
                                                    Save = true;
                                                }
                                            }
                                        }
                                        int _resultado = Convert.ToInt32(DetaDocDAL.ActualizarDocNaviEx(IdDoc));

                                        // Html += HttpContext.Current.Session["Html"].ToString();

                                        if (_resultado > 0)
                                        {
                                            Html += "<b><u>DETALLE DE CARGA DE LISTADO POR SUSTITUCIÓN</b></u><br />";
                                            Html += "<br /> Módulo : Carga de listado de contenedores de exportación por sustitución <br />";
                                            Html += "Mensaje : Notificación carga de listado por sustitución exitosa <br /><br />";
                                            Html += string.Format("Se notifica que la carga del archivo de exportación, de {0} para el buque: {1}, # de Viaje {2}, se validaron {3} contenedores de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, pExport.Count);


                                            Html += "<br /><br />";
                                            Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                                            Html += "<font color=blue> En espera de validación de ADUANA </font>";

                                            Html += "</font>";
                                            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
                                            _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): ACEPTADA Listado de Exportación de {0} para el Buque: {1}, # de Viaje {2}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);
                                            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                            List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                            if (_listaCC == null)
                                                _listaCC = new List<Notificaciones>();

                                            _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.verdadero, "219"));
                                            _correo.ListaCC = _listaCC;

                                            //Notificaciones _notiCC = new Notificaciones
                                            //{
                                            //    IdNotificacion = -1,
                                            //    sMail = "elsa.sosa@cepa.gob.sv",
                                            //    dMail = "Elsa Sosa"
                                            //};

                                            //List<Notificaciones> _ccList = new List<Notificaciones>();
                                            //_ccList.Add(_notiCC);

                                            //_correo.ListaNoti = _ccList;



                                            _correo.Asunto = Html;
                                            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                                            _correo = null;

                                            _actSusti = EncaNavieraDAL.ActSustitucion(IdRegEnca, 0);

                                            if (Save == true)
                                                throw new Exception("Archivo registrado correctamente");


                                        }
                                        else
                                            throw new Exception("Seleccione el archivo a reemplazar");
                                    }
                                }

                            }
                            #endregion
                        }

                    }
                    else
                    {
                        #region "NONBRE TRUE"

                        if (sustitucion == 0)
                        {
                           
                            Html += "<b><u>DETALLE DE CARGA DE LISTADO DENEGADO</b></u><br />";
                            Html += "<br /> Módulo : Carga de listado de contenedores de exportación <br />";
                            Html += "Mensaje : Notificación carga de listado denegada <br /><br />";
                            Html += string.Format("Se notifica que la carga del archivo de exportación, de {0} para el buque: {1}, # de Viaje {2} ha sido rechazado por las inconsistencias detalladas en el presente", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);

                            Html += "</font>";
                            Html += "<br /><br />";
                            Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                            Html += "<font color=red>En espera de correcciones por la Naviera </font>";



                            _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA: DENEGADA Listado de Exportación Buque: {0}, # de Viaje {1}", d_buque, cn_voyage);
                            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                            _correo.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                            //Notificaciones _notiCC = new Notificaciones
                            //{
                            //    IdNotificacion = -1,
                            //    sMail = "elsa.sosa@cepa.gob.sv",
                            //    dMail = "Elsa Sosa"
                            //};

                            //List<Notificaciones> _ccList = new List<Notificaciones>();
                            //_ccList.Add(_notiCC);

                            //_correo.ListaNoti = _ccList;

                            _correo.Asunto = Html;
                            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                            _correo = null;

                            throw new Exception("Archivo presento inconsistencias revisar correo electrónico");
                        }
                        else
                        {
                            

                            Html += "<b><u>DETALLE DE CARGA DE LISTADO POR SUSTITUCIÓN DENEGADO</b></u><br />";
                            Html += "<br /> Módulo : Carga de listado de contenedores de exportación por sustitución de archivo <br />";
                            Html += "Mensaje : Notificación carga de listado por sustitución denegada <br /><br />";
                            Html += string.Format("Se notifica que la carga del archivo de exportación por sustitución, de {0} para el buque: {1}, # de Viaje {2}, ha sido rechazado por las inconsistencias detalladas en el presente", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage);

                            Html += "</font>";

                            Html += "<br /><br />";
                            Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                            Html += "<font color=red>En espera de correcciones por la Naviera </font>";

                            _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): DENEGADA Listado de Exportación Buque: {0}, # de Viaje {1}", d_buque, cn_voyage);
                            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                            List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                            //Notificaciones _notiCC = new Notificaciones
                            //{
                            //    IdNotificacion = -1,
                            //    sMail = "elsa.sosa@cepa.gob.sv",
                            //    dMail = "Elsa Sosa"
                            //};

                            //List<Notificaciones> _ccList = new List<Notificaciones>();
                            //_ccList.Add(_notiCC);

                            //_correo.ListaNoti = _ccList;

                            if (_listaCC == null)
                                _listaCC = new List<Notificaciones>();

                            _correo.ListaCC = _listaCC;
                            _correo.Asunto = Html;
                            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                            _correo = null;
                            _actSusti = EncaNavieraDAL.ActSustitucion(IdRegEnca, 0);

                            throw new Exception("Archivo presento inconsistencias revisar correo electrónico");
                        }


                        #endregion
                    }
            }
            else if(pExport.Count == 0)
            {
                throw new Exception("El archivo no posee registros que validar");
            }
        }

        public bool ValidarArchivoD(string nombre, int IdRegEnca)
        {
            bool ErrorNombre = false;

            Html += "a. Código Internacional de Naviera : " + nombre.TrimStart().Substring(0, 4) + " - ";

            if (HttpContext.Current.Session["c_iso_navi"].ToString().TrimStart() != nombre.TrimStart().Substring(0, 4))
            {
                Html += "<font color=\"red\">INVALIDO CODIGO INCORRECTO</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";


            Html += "b. Módulo : " + nombre.Trim().Substring(5, 3).ToUpper() + " - ";

            if (nombre.Trim().Substring(5, 3).ToUpper() != "EXP")
            {
                Html += "<font color=\"red\">INVALIDO MODULO DE EXPORTACIÓN</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";

            Html += "c. Nombre del Buque : " + nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() + " - ";

            if (nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() != d_buque.ToUpper())
            {
                Html += "<font color=\"red\">INVALIDO ERROR EN NOMBRE DE BUQUE</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";

            Html += "d. Fecha de Llegada : " + nombre.Trim().Substring(10 + d_buque.Length, 6) + " - ";

            string fe = nombre.Trim().Substring(10 + d_buque.Length, 6);
            string fi = (f_llegada.Day > 9 ? f_llegada.Day.ToString() : "0" + f_llegada.Day.ToString()) + (f_llegada.Month > 9 ? f_llegada.Month.ToString() : "0" + f_llegada.Month.ToString()) + f_llegada.Year.ToString().Substring(2, 2);

            if (fe != fi)
            {
                Html += "<font color=\"red\">INVALIDO FECHAS NO COINCIDEN</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";

            Html += "e. Correlativo : " + nombre.Trim().Substring(17 + d_buque.Length, 2) + " - ";

            object cor = Convert.ToInt32(nombre.Trim().Substring(17 + d_buque.Length, 2));

            if (ArchivoBookingDAL.isNumeric(cor) == false)
            {
                Html += "<font color=\"red\">INVALIDO FORMATO DE CORRELATIVO </font><br />";
                ErrorNombre = true;
            }
            else
            {
                if (Convert.ToInt32(cor) > 0)
                {
                    if (IdRegEnca > 0)
                    {
                        int correlativo_c = DetaDocDAL.ObtenerCorrelativoDocEx(IdRegEnca);

                        if ((correlativo_c + 1) != Convert.ToInt32(cor))
                        {
                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", correlativo_c + 1);
                            ErrorNombre = true;
                        }
                        else
                            Html += "OK.<br />";
                    }
                    else
                        if (Convert.ToInt32(cor) > 1)
                        {
                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", "01");
                            ErrorNombre = true;
                        }
                        else
                            Html += "OK.<br />";
                }
            }

            Html += "<br />";
            return ErrorNombre;
        }


        public bool ValidarArchivoD(string nombre, int IdRegEnca, string d_buque, DateTime f_llegada, string sustitucion)
        {
            bool ErrorNombre = false;

            Html += "a. Código Internacional de Naviera : " + nombre.TrimStart().Substring(0, 4) + " - ";

            if (HttpContext.Current.Session["c_iso_navi"].ToString().TrimStart() != nombre.TrimStart().Substring(0, 4))
            {
                Html += "<font color=\"red\">INVALIDO CODIGO INCORRECTO</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";


            Html += "b. Módulo : " + nombre.Trim().Substring(5, 3).ToUpper() + " - ";

            if (nombre.Trim().Substring(5, 3).ToUpper() != "EXP")
            {
                Html += "<font color=\"red\">INVALIDO MODULO DE EXPORTACIÓN</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";

            Html += "c. Nombre del Buque : " + nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() + " - ";

            if (nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() != d_buque.ToUpper())
            {
                Html += "<font color=\"red\">INVALIDO ERROR EN NOMBRE DE BUQUE</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";

            Html += "d. Fecha de Llegada : " + nombre.Trim().Substring(10 + d_buque.Length, 6) + " - ";

            string fe = nombre.Trim().Substring(10 + d_buque.Length, 6);
            string fi = (f_llegada.Day > 9 ? f_llegada.Day.ToString() : "0" + f_llegada.Day.ToString()) + (f_llegada.Month > 9 ? f_llegada.Month.ToString() : "0" + f_llegada.Month.ToString()) + f_llegada.Year.ToString().Substring(2, 2);

            if (fe != fi)
            {
                Html += "<font color=\"red\">INVALIDO FECHAS NO COINCIDEN</font><br />";
                ErrorNombre = true;
            }
            else
                Html += "OK.<br />";

            Html += "e. Correlativo : " + nombre.Trim().Substring(17 + d_buque.Length, 2) + " - ";

            object cor = Convert.ToInt32(nombre.Trim().Substring(17 + d_buque.Length, 2));
            object cor1 = Convert.ToInt32(sustitucion.Trim().Substring(17 + d_buque.Length, 2));

            if (ArchivoBookingDAL.isNumeric(cor) == false)
            {
                Html += "<font color=\"red\">INVALIDO FORMATO DE CORRELATIVO </font><br />";
                ErrorNombre = true;
            }
            else
            {
                if (Convert.ToInt32(cor) > 0)
                {
                    if (IdRegEnca > 0)
                    {

                        if ((Convert.ToInt32(cor)) != Convert.ToInt32(cor1))
                        {
                            if (Convert.ToInt32(cor1) > 9)
                                cor1 = cor1.ToString();
                            else
                                cor1 = "0" + cor1;
                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", cor1);
                            ErrorNombre = true;
                        }
                        else
                            Html += "OK.<br />";
                    }
                    else
                        if (Convert.ToInt32(cor) > 1)
                        {
                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", "01");
                            ErrorNombre = true;
                        }
                        else
                            Html += "OK.<br />";
                }
            }

            Html += "<br />";

            HttpContext.Current.Session["Html"] = Html;
            return ErrorNombre;
        }

    }
}
