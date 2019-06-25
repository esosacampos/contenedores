using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace CEPA.CCO.Linq
{
    public class CargarArchivosLINQ
    {
        #region "Propiedades"
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

        public int c_sidunea { get; set; }
        public int c_booking { get; set; }
        #endregion

        public void AlmacenaCarga()
        {
            Save = false;
            string _actSusti = null;
            List<int> _listaDeta = new List<int>();
            int _resulDeta = 0;
            //int Correlativo = 0;
            //string anum_manif = null;

            if (sustitucion == 1)
            {
                if (arch_susti == string.Empty || arch_susti.Length == 0)
                {

                    throw new Exception("Seleccione el archivo a reemplazar");
                }


            }

            //Si existe archivo a cargar 
            if (HttpContext.Current.Session["ruta"] != null && HttpContext.Current.Session["archivo"] != null)
            {
                a = HttpContext.Current.Session["ruta"].ToString();
            }
            else
            {
                throw new System.InvalidOperationException("Debe seleccionar el archivo a cargar");
            }


            //Si el archivo ya existe para el caso de la carga 

            if (sustitucion == 0)
            {
                int _exis = Convert.ToInt32(DetaDocDAL.ArchivosExistentes(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString(), HttpContext.Current.Session["archivo"].ToString()));

                if (_exis > 0)
                {
                    throw new System.InvalidOperationException("Ya existe un archivo almacenado con este nombre");
                }
            }


            // Buscar el voyage del archivo //Validar el voyage     

            // Aqui incluir validación iBooking


            int c_DP = 0;

            string cn_voyage = null;
            DBComun.sRuta = a;

            if (c_booking == 1)
            {
                List<ArchivoExcel> listVoyage = ArchivoExcelDAL.GetVoyage(a, DBComun.Estado.verdadero);

                foreach (ArchivoExcel v in listVoyage)
                {
                    cn_voyage = v.c_voyage;
                    break;
                }

                string[] cadenaVoyage = cn_voyage.Split('-');
                cn_voyage = cadenaVoyage[0].ToString();
                c_DP = Convert.ToInt32(cadenaVoyage[1].ToString());
            }
            else
            {
                List<ArchivoExcel> listVoyage = ArchivoExcelDAL.GetVoyage(a, DBComun.Estado.verdadero);

                foreach (ArchivoExcel v in listVoyage)
                {
                    cn_voyage = v.c_voyage;
                    break;
                }
                c_DP = 0;
            }




            //Verificar archivos almacenados.

            int _arch = Convert.ToInt32(DetaDocDAL.ArchivosAlmacenados(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString()));

            if (_arch > 0)
            {
                IdRegEnca = Convert.ToInt32(EncaNavieraDAL.ObtenerIdReg(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString()));
            }

            if (sustitucion == 1)
            {
                _actSusti = EncaNavieraDAL.ActSustitucion(IdRegEnca, 1);
            }





            //Validar que el imo coincida con el del archivo

            string c_imoVa = ArchivoExcelDAL.GetImo(a, DBComun.Estado.verdadero);
            string Mensaje = "";

            if (c_imoVa == null)
            {
                throw new Exception("El número de imo indicado presenta errores");
            }

            if (Convert.ToDouble(c_imoVa) > 0)
            {
                if (c_imo != c_imoVa)
                {
                    Mensaje = "El IMO del archivo no coincide con el del buque seleccionado";
                    //HttpContext.Current.Response.Write("<script>alert('" + Mensaje + "');</script>");
                    throw new Exception(Mensaje);

                }
            }
            else
            {
                throw new FormatException("El número de imo indicado presenta errores en su formato, elimine espacios y verifique número");
            }


            //Buscar manifiesto

            List<ArchivoExcel> listManif = ArchivoExcelDAL.GetManifiesto(a, DBComun.Estado.verdadero);
            string manif = null;
            string a_manif = null;

            foreach (ArchivoExcel s in listManif)
            {
                c_manifiesto = s.c_num_manif;
                //anum_manif = s.anum_manif;

                string[] words = c_manifiesto.TrimEnd().TrimStart().Trim().Split('-');

                if (words.Count() > 0 && words.Count() <= 2)
                {
                    for (int i = 0; i < words.Count(); i++)
                    {
                        if (words[0].Count() == 4)
                        {
                            a_manif = words[0].ToString();
                        }
                        else
                        {
                            throw new Exception("El año del manifiesto no posee entrada válida");
                        }

                        if (words[1].Count() >= 1 && words[1].Count() <= 4)
                        {
                            manif = words[1].ToString();
                        }
                        else
                        {
                            throw new Exception("El número del manifiesto no posee entrada válida");
                        }

                    }
                }
                else
                {
                    throw new Exception("El número del manifiesto no posee entrada válida Ej. 2015-356");
                }

                MemoryStream memoryStream = new MemoryStream();
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    ConformanceLevel = ConformanceLevel.Document,
                    Indent = true
                };

                XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                string _Aduana = null;
                string _mensaje = null;
                xml.WriteStartDocument();

                xml.WriteStartElement("MDS4");

                xml.WriteElementString("CAR_REG_YEAR", a_manif);
                xml.WriteElementString("KEY_CUO", "02");
                xml.WriteElementString("CAR_REG_NBER", manif);

                xml.WriteEndDocument();
                xml.Flush();
                xml.Close();

                //Generar XML para enviar parametros al servicio.
                _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                CepaSW.WSManifiestoCEPAClient _proxy = new CepaSW.WSManifiestoCEPAClient();


                string _user = ConfigurationManager.AppSettings["userSidunea"];
                string _psw = ConfigurationManager.AppSettings["pswSidunea"];

                _proxy.ClientCredentials.UserName.UserName = _user;
                _proxy.ClientCredentials.UserName.Password = _psw;

                _mensaje = _proxy.getContenedorData(_Aduana);
                if (_mensaje.Substring(0, 1) == "0")
                {
                    throw new Exception("El manifiesto # " + string.Concat(a_manif, "-", manif) + " no produjó resultados, por favor verificar que la información es correcta ");
                }


                break;
            }




            // Validación Nombre de Archivo.
            string nombre = Path.GetFileName(a);
            bool ErrorNombre;

            _fecha = DetaNavieraLINQ.FechaBD();

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

            if (nombre.Trim().Length == 0)
            {
                throw new System.InvalidOperationException("NO SE HA ESPECIFICADO UN NOMBRE DE ARCHIVO");
            }

            if (sustitucion == 1)
            {
                if (nombre != arch_susti)
                {
                    throw new System.InvalidOperationException("El nombre del archivo debe ser igual que el nombre del archivo a sustituir");
                }
            }

            string extension = Path.GetExtension(a);
            if (Path.GetExtension(a) != ".xls" && Path.GetExtension(a) != ".xlsx")
            {
                throw new System.InvalidOperationException("La extensión del archivo no es válida, revisar que sea un libro de Excel");
            }

            int ar = Path.GetFileName(a).Length;
            int ar1 = Path.GetExtension(a).Length;
            int ar3 = 19 + d_buque.Length;

            if ((Path.GetFileName(a).Length - Path.GetExtension(a).Length) != (19 + d_buque.Length))
            {
                throw new System.InvalidOperationException("Longitud de nombre de archivo no es válida");
            }

            int countHojas = ArchivoExcelDAL.GetCountHojas(a);

            if (countHojas > 1)
            {
                throw new System.InvalidOperationException("Libro de Excel posee más de una hoja activa");
            }

            EnvioCorreo _correo = new EnvioCorreo();

            Html += "<font size=2>";

            if (sustitucion == 0)
            {
                ErrorNombre = ValidarArchivoD(nombre, IdRegEnca);
            }
            else
            {
                ErrorNombre = ValidarArchivoD(nombre, IdRegEnca, d_buque, f_llegada, arch_susti);
            }


            // Validar viaje
            if (sustitucion == 0)
            {
                string _resultadoV = null;

                if (nombre.Trim().Substring(17 + d_buque.Length, 2) == "01")
                {
                    _resultadoV = ArchivoExcelDAL.ValidarViajeC(DBComun.Estado.verdadero, c_imoVa, cn_voyage, HttpContext.Current.Session["c_naviera"].ToString());
                    if (_resultadoV != "NULL" || _resultadoV == "Error")
                    {
                        throw new Exception("ESTE IMO YA PRESENTO ESTE NÚMERO DE VIAJE");
                    }
                }
            }

            if (ErrorNombre == false)
            {
                int del = 0;
                if (sustitucion == 1)
                {
                    del = Convert.ToInt32(DetaNavieraDAL.EliminarDetaNaviera(IdRegEnca, nombre));
                }

                List<ResultadoValidacion> validarPrueba = DetaNavieraLINQ.ValidarDetalle(a, DBComun.Estado.verdadero);
                List<ResultadoValidacion> listaValid = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;


                List<DetaNaviera> pListaExist = DetaNavieraDAL.ObtenerRegAnter(c_imo, c_llegada, cn_voyage);

                if (pListaExist == null)
                {
                    pListaExist = new List<DetaNaviera>();
                }

                if (pListaExist.Count > 0)
                {
                    List<ResultadoValidacion> listaVaTa = DetaNavieraLINQ.ValidarDetalle(a, pListaExist, DBComun.Estado.verdadero);
                    listaValid = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;
                }


                //Errores en la validacion
                if (listaValid.Count > 0)
                {
                    if (sustitucion == 0)
                    {
                        CorreoRecepcion(cn_voyage, Convert.ToInt32(manif));
                    }
                    else
                    {
                        CorreoRecepcion1(Convert.ToInt32(manif), cn_voyage);
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
                    Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>CONSIGNEE</font></th>";
                    Html += "<td width=\"8%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>SEAL NUMBER</font></th>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>COUNTRY</font></th>";
                    Html += "<td width=\"7%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>Receipt Country/Port Code</font></th>";
                    Html += "<td width=\"20%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>DESCRIPCION</font></th>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>TARE</font></th>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>IMO IMDG</font></th>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>UN NUMBER</font></th>";
                    Html += "<td width=\"5%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=1>CONDICION DEL CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    var query = (from arch in ArchivoExcelDAL.GetRango(a, DBComun.Estado.verdadero)
                                 join valid in listaValid.Where(t => t.Resultado == 1) on arch.num_fila equals valid.NumFila
                                 orderby arch.num_fila
                                 select new
                                 {
                                     valid.NumFila,
                                     valid.NumCelda,
                                     valid.Descripcion
                                 }).ToList();

                    var queryTodos = (from arch in ArchivoExcelDAL.GetRango(a, DBComun.Estado.verdadero)
                                      where
                                         (from valid in listaValid
                                          where valid.Resultado == 1
                                          select new
                                          {
                                              valid.NumFila
                                          }).Contains(new { NumFila = arch.num_fila })
                                      select new ArchivoExcel
                                      {
                                          num_manif = Convert.ToInt32(manif),
                                          num_fila = arch.num_fila,
                                          n_BL = arch.n_BL,
                                          n_contenedor = arch.n_contenedor,
                                          c_tamaño = arch.c_tamaño,
                                          v_peso = arch.v_peso,
                                          b_estado = arch.b_estado,
                                          s_consignatario = arch.s_consignatario,
                                          n_sello = arch.n_sello,
                                          c_pais_destino = arch.c_pais_destino,
                                          c_pais_origen = arch.c_pais_origen,
                                          s_comodity = arch.s_comodity,
                                          s_prodmanifestado = arch.s_prodmanifestado,
                                          v_tara = arch.v_tara,
                                          b_reef = arch.b_reef,
                                          b_ret_dir = arch.b_ret_dir,
                                          c_imo_imd = arch.c_imo_imd,
                                          c_un_number = arch.c_un_number,
                                          b_transhipment = arch.b_transhipment,
                                          c_condicion = arch.c_condicion
                                      }).ToList();


                    foreach (ArchivoExcel itemArch in queryTodos)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + c_imoVa + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + cn_voyage + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.n_BL + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.n_contenedor + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_tamaño + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.v_peso + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.b_estado + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.s_consignatario + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.n_sello + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_pais_destino + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_pais_origen + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.s_prodmanifestado + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.v_tara + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_imo_imd + "</font></td>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=1 color=blue>" + itemArch.c_un_number + "</font></td>";
                        Html += "<td height=\"25\"><font size=1 color=blue>" + itemArch.c_condicion + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        Html += "<tr>";
                        Html += "<td colspan=16>";

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
                    {
                        _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA: DENEGADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, manif);
                    }
                    else
                    {
                        _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): DENEGADA Listado de Importación Buque: {0}, # de Viaje {1}, Manifiesto de Aduana # {2}", d_buque, cn_voyage, manif);
                    }

                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                    if (_listaCC == null)
                    {
                        _listaCC = new List<Notificaciones>();
                    }

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
                    int _actNoti = 0;
                    if (sustitucion == 1 && IdRegEnca > 0)
                    {
                        _actNoti = Convert.ToInt32(EncaNavieraDAL.ActualizarNoti(DBComun.Estado.verdadero, IdRegEnca, 0));
                    }

                    throw new Exception("Archivo presento inconsistencias revisar correo electrónico");

                }
                else
                {
                    if (sustitucion == 0)
                    {
                        CorreoRecepcion(cn_voyage, Convert.ToInt32(manif));
                    }
                    else
                    {
                        CorreoRecepcion1(Convert.ToInt32(manif), cn_voyage);
                    }


                    if (sustitucion == 0)
                    {
                        int _resultado = 0;
                        if (_arch > 0)
                        {
                            IdRegReal = IdRegEnca;
                        }
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
                                f_llegada = Convert.ToDateTime(f_llegada.ToString("dd/MM/yyyy hh:mm tt")),
                                b_DP = c_DP
                            };

                            _resultado = Convert.ToInt32(EncaNavieraDAL.AlmacenarEncaNaviera(_encaNavi));
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
                                c_llegada = c_llegada,
                                num_manif = Convert.ToInt32(manif),
                                a_manif = a_manif,
                                b_siduneawd = c_sidunea
                                /* a_manif = manif,
                                 a_manifiesto = a_manif*/
                            };

                            int _docResul = Convert.ToInt32(DetaDocDAL.AlmacenarDocNavi(_detaDoc));


                            if (_docResul > 0)
                            {

                                DBComun.sRuta = a;
                                List<ArchivoExcel> _list = new List<ArchivoExcel>();

                                _list = ArchivoExcelDAL.GetRango(a, DBComun.Estado.verdadero);
                                if (_list.Count > 0)
                                {
                                    foreach (ArchivoExcel item in _list)
                                    {
                                        if (item.n_contenedor != null && item.n_contenedor != "")
                                        {
                                            DetaNaviera _detaNavi = new DetaNaviera
                                            {
                                                IdDeta = -1,
                                                IdReg = IdRegReal,
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
                                                IdDoc = _docResul,
                                                b_shipper = item.b_shipper,
                                                b_transferencia = item.b_transferencia,
                                                b_manejo = item.b_manejo,
                                                b_despacho = item.b_despacho
                                            };

                                            _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarDetaNaviera(_detaNavi));


                                            if (_resulDeta > 0)
                                            {
                                                _listaDeta.Add(_resulDeta);
                                                Save = true;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("Archivo no posee registros a cargar");
                                }
                            }

                            _actSusti = EncaNavieraDAL.ActSustitucion(IdRegReal, 0);
                        }


                        Html += "<b><u>DETALLE DE CARGA DE LISTADO</b></u><br />";
                        Html += "<br /> Módulo : Carga de listado de contenedores de importación <br />";
                        Html += "Mensaje : Notificación carga de listado exitosa <br /><br />";
                        Html += string.Format("Se notifica que la carga del archivo de importación, de {0} para el buque: {1}, # de Viaje {2}, Manifiesto de Aduana #{3}, se validaron {4} contenedores de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, Convert.ToInt32(manif), _listaDeta.Count);
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=blue> En espera de validación de ADUANA </font>";

                        Html += "</font>";


                        //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
                        _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA: ACEPTADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, Convert.ToInt32(manif));
                        _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                        List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                        if (_listaCC == null)
                        {
                            _listaCC = new List<Notificaciones>();
                        }

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
                        {
                            throw new Exception("Archivo registrado correctamente");
                        }
                    }
                    else
                    {
                        int IdDoc = 0;
                        int IdReg = 0;
                        List<DetaDoc> listaDoc = DetaDocDAL.ObtenerDocA(DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString(), c_llegada, arch_susti);

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
                                        c_llegada = c_llegada,
                                        num_manif = Convert.ToInt32(manif),
                                        a_manif = a_manif,
                                        b_siduneawd = c_sidunea
                                    };

                                    int _docResul = Convert.ToInt32(DetaDocDAL.AlmacenarDocNavi(_detaDoc));

                                    if (_docResul > 0)
                                    {
                                        List<ArchivoExcel> _list = new List<ArchivoExcel>();
                                        _list = ArchivoExcelDAL.GetRango(a, DBComun.Estado.verdadero);


                                        foreach (ArchivoExcel item in _list)
                                        {
                                            if (item.n_contenedor != null && item.n_contenedor != "")
                                            {
                                                DetaNaviera _detaNavi = new DetaNaviera
                                                {
                                                    IdDeta = -1,
                                                    IdReg = item1.IdReg,
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
                                                    IdDoc = _docResul,
                                                    b_shipper = item.b_shipper,
                                                    b_transferencia = item.b_transferencia,
                                                    b_manejo = item.b_manejo,
                                                    b_despacho = item.b_despacho
                                                };

                                                _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarDetaNaviera(_detaNavi));

                                                if (_resulDeta > 0)
                                                {
                                                    _listaDeta.Add(_resulDeta);
                                                    Save = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                int _resultado = Convert.ToInt32(DetaDocDAL.ActualizarDocNavi(IdDoc));

                                // Html += HttpContext.Current.Session["Html"].ToString();

                                if (_resultado > 0)
                                {
                                    Html += "<b><u>DETALLE DE CARGA DE LISTADO POR SUSTITUCIÓN</b></u><br />";
                                    Html += "<br /> Módulo : Carga de listado de contenedores de importación por sustitución <br />";
                                    Html += "Mensaje : Notificación carga de listado por sustitución exitosa <br /><br />";
                                    Html += string.Format("Se notifica que la carga del archivo de importación, de {0} para el buque: {1}, # de Viaje {2}, Manifiesto de Aduana #{3}, se validaron {4} contenedores de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, Convert.ToInt32(manif), _listaDeta.Count);


                                    Html += "<br /><br />";
                                    Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                                    Html += "<font color=blue> En espera de validación de ADUANA </font>";

                                    Html += "</font>";
                                    //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
                                    _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): ACEPTADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, Convert.ToInt32(manif));
                                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                    if (_listaCC == null)
                                    {
                                        _listaCC = new List<Notificaciones>();
                                    }

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
                                    {
                                        throw new Exception("Archivo registrado correctamente");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Seleccione el archivo a reemplazar");
                                }
                            }
                        }
                    }
                }

            }
            else
            {
                if (sustitucion == 0)
                {
                    CorreoRecepcion(cn_voyage, Convert.ToInt32(manif));

                    Html += "<b><u>DETALLE DE CARGA DE LISTADO DENEGADO</b></u><br />";
                    Html += "<br /> Módulo : Carga de listado de contenedores de importación <br />";
                    Html += "Mensaje : Notificación carga de listado denegada <br /><br />";
                    Html += string.Format("Se notifica que la carga del archivo de importación, de {0} para el buque: {1}, # de Viaje {2}, Manifiesto de Aduana #{3} ha sido rechazado por las inconsistencias detalladas en el presente", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, Convert.ToInt32(manif));

                    Html += "</font>";
                    Html += "<br /><br />";
                    Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                    Html += "<font color=red>En espera de correcciones por la Naviera </font>";



                    _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA: DENEGADA Listado de Importación Buque: {0}, # de Viaje {1}, Manifiesto de Aduana # {2}", d_buque, cn_voyage, Convert.ToInt32(manif));
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
                    CorreoRecepcion1(Convert.ToInt32(manif), cn_voyage);


                    Html += "<b><u>DETALLE DE CARGA DE LISTADO POR SUSTITUCIÓN DENEGADO</b></u><br />";
                    Html += "<br /> Módulo : Carga de listado de contenedores de importación por sustitución de archivo <br />";
                    Html += "Mensaje : Notificación carga de listado por sustitución denegada <br /><br />";
                    Html += string.Format("Se notifica que la carga del archivo de importación por sustitución, de {0} para el buque: {1}, # de Viaje {2}, Manifiesto de Aduana #{3} ha sido rechazado por las inconsistencias detalladas en el presente", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, Convert.ToInt32(manif));

                    Html += "</font>";

                    Html += "<br /><br />";
                    Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                    Html += "<font color=red>En espera de correcciones por la Naviera </font>";

                    _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): DENEGADA Listado de Importación Buque: {0}, # de Viaje {1}, Manifiesto de Aduana # {2}", d_buque, cn_voyage, Convert.ToInt32(manif));
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
                    {
                        _listaCC = new List<Notificaciones>();
                    }

                    _correo.ListaCC = _listaCC;
                    _correo.Asunto = Html;
                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                    _correo = null;
                    _actSusti = EncaNavieraDAL.ActSustitucion(IdRegEnca, 0);

                    throw new Exception("Archivo presento inconsistencias revisar correo electrónico");
                }
            }


        }

        public void CorreoRecepcion1(int n_manifiesto, string cn_voyage)
        {
            string Html1;
            EnvioCorreo _correo1 = new EnvioCorreo();

            Html1 = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html1 += "<br />MÓDULO : RECEPCIÓN DE LISTADO DE IMPORTACIÓN POR SUSTITUCIÓN <br />";
            Html1 += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE ARCHIVO POR SUSTITUCIÓN <br /><br />";
            Html1 += string.Format("Se notifica que la recepción del archivo de importación por sustitución, de {0} para el buque {1}, número de Viaje {2} y manifiesto de ADUANA # {3}, en la cual solicitan la sustitución del archivo {4}, y queda en espera de resultados de validación", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, n_manifiesto, arch_susti);


            Html1 += "<br /><br />";
            Html1 += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
            Html1 += "<font color=blue> En espera de validación de CEPA </font>";

            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);

            _correo1.Subject = string.Format("PASO 1 de 4: Recepción CEPA: Listado de Importación por SUSTITUCIÓN de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, n_manifiesto);
            _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

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

        private void CorreoRecepcion(string cn_voyage, int manif)
        {
            string Html1 = null;
            EnvioCorreo _correo1 = new EnvioCorreo();

            Html1 = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html1 += "<br />MÓDULO : RECEPCIÓN DE LISTADO DE IMPORTACIÓN <br />";
            Html1 += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE ARCHIVO <br /><br />";
            Html1 += string.Format("Se notifica que la recepción del archivo de importación, de {0} para el buque {1}, número de Viaje {2} y manifiesto de ADUANA # {3} ha sido recibido y queda en espera de resultados de validación", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, manif);
            Html1 += "<br /><br />";
            Html1 += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
            Html1 += "<font color=blue> En espera de validación de CEPA </font>";

            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);

            _correo1.Subject = string.Format("PASO 1 de 4: Recepción CEPA: Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, manif);
            _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

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

        private void CorreoBooking(string c_naviera)
        {
            string Html1 = null;
            EnvioCorreo _correo1 = new EnvioCorreo();
            _fecha = DetaNavieraLINQ.FechaBD();

            Html1 = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html1 += "<br />MÓDULO : RECEPCIÓN DE LISTADO DE BOOKING <br />";
            Html1 += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE ARCHIVO <br /><br />";
            Html1 += string.Format("Se notifica que la recepción del listado de bookings de {0}, con fecha de actualización {1} ha sido recibido", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"));
            Html1 += "<br /><br />";
            Html1 += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
            Html1 += "<font color=blue> En espera de validación de CEPA </font>";

            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);

            _correo1.Subject = string.Format("PASO 1 de 2: Recepción CEPA: Listado de Bookings de {0} - {1}", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"));
            _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

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
            {
                Html += "OK.<br />";
            }

            Html += "b. Módulo : " + nombre.Trim().Substring(5, 3).ToUpper() + " - ";

            if (nombre.Trim().Substring(5, 3).ToUpper() != "IMP")
            {
                Html += "<font color=\"red\">INVALIDO MODULO DE IMPORTACION</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "c. Nombre del Buque : " + nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() + " - ";

            if (nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() != d_buque.ToUpper())
            {
                Html += "<font color=\"red\">INVALIDO ERROR EN NOMBRE DE BUQUE</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "d. Fecha de Llegada : " + nombre.Trim().Substring(10 + d_buque.Length, 6) + " - ";

            string fe = nombre.Trim().Substring(10 + d_buque.Length, 6);
            string fi = (f_llegada.Day > 9 ? f_llegada.Day.ToString() : "0" + f_llegada.Day.ToString()) + (f_llegada.Month > 9 ? f_llegada.Month.ToString() : "0" + f_llegada.Month.ToString()) + f_llegada.Year.ToString().Substring(2, 2);

            if (fe != fi)
            {
                Html += "<font color=\"red\">INVALIDO FECHAS NO COINCIDEN</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

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
                        int correlativo_c = DetaDocDAL.ObtenerCorrelativoDoc(IdRegEnca, 1);

                        if ((correlativo_c + 1) != Convert.ToInt32(cor))
                        {
                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", correlativo_c + 1);
                            ErrorNombre = true;
                        }
                        else
                        {
                            Html += "OK.<br />";
                        }
                    }
                    else
                        if (Convert.ToInt32(cor) > 1)
                    {
                        Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", "01");
                        ErrorNombre = true;
                    }
                    else
                    {
                        Html += "OK.<br />";
                    }
                }
            }

            Html += "<br />";
            return ErrorNombre;
        }

        public bool ValidarArchivoBook(string nombre)
        {
            bool ErrorNombre = false;

            Html += "a. Código Internacional de Naviera : " + nombre.TrimStart().Substring(0, 4) + " - ";

            if (HttpContext.Current.Session["c_iso_navi"].ToString().TrimStart() != nombre.TrimStart().Substring(0, 4))
            {
                Html += "<font color=\"red\">INVALIDO CODIGO INCORRECTO</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "b. Módulo : " + nombre.Trim().Substring(5, 7).ToUpper() + " - ";

            if (nombre.Trim().Substring(5, 7).ToUpper() != "BOOKING")
            {
                Html += "<font color=\"red\">INVALIDO MODULO DE CARGA DE BOOKINGS</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
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
            {
                Html += "OK.<br />";
            }

            Html += "b. Módulo : " + nombre.Trim().Substring(5, 3).ToUpper() + " - ";

            if (nombre.Trim().Substring(5, 3).ToUpper() != "IMP")
            {
                Html += "<font color=\"red\">INVALIDO MODULO DE IMPORTACION</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "c. Nombre del Buque : " + nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() + " - ";

            if (nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() != d_buque.ToUpper())
            {
                Html += "<font color=\"red\">INVALIDO ERROR EN NOMBRE DE BUQUE</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "d. Fecha de Llegada : " + nombre.Trim().Substring(10 + d_buque.Length, 6) + " - ";

            string fe = nombre.Trim().Substring(10 + d_buque.Length, 6);
            string fi = (f_llegada.Day > 9 ? f_llegada.Day.ToString() : "0" + f_llegada.Day.ToString()) + (f_llegada.Month > 9 ? f_llegada.Month.ToString() : "0" + f_llegada.Month.ToString()) + f_llegada.Year.ToString().Substring(2, 2);

            if (fe != fi)
            {
                Html += "<font color=\"red\">INVALIDO FECHAS NO COINCIDEN</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

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
                            {
                                cor1 = cor1.ToString();
                            }
                            else
                            {
                                cor1 = "0" + cor1;
                            }

                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", cor1);
                            ErrorNombre = true;
                        }
                        else
                        {
                            Html += "OK.<br />";
                        }
                    }
                    else
                        if (Convert.ToInt32(cor) > 1)
                    {
                        Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", "01");
                        ErrorNombre = true;
                    }
                    else
                    {
                        Html += "OK.<br />";
                    }
                }
            }

            Html += "<br />";

            HttpContext.Current.Session["Html"] = Html;
            return ErrorNombre;
        }

        public bool ValidarArchivoRe(string nombre, int IdRegEnca)
        {
            bool ErrorNombre = false;

            Html += "a. Código Internacional de Naviera : " + nombre.TrimStart().Substring(0, 4) + " - ";

            if (HttpContext.Current.Session["c_iso_navi"].ToString().TrimStart() != nombre.TrimStart().Substring(0, 4))
            {
                Html += "<font color=\"red\">INVALIDO CODIGO INCORRECTO</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "b. Módulo : " + nombre.Trim().Substring(5, 3).ToUpper() + " - ";

            if (nombre.Trim().Substring(5, 3).ToUpper() != "IRE")
            {
                Html += "<font color=\"red\">INVALIDO MODULO DE REESTIBA</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "c. Nombre del Buque : " + nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() + " - ";

            if (nombre.TrimStart().Substring(9, d_buque.Trim().Length).ToUpper() != d_buque.ToUpper())
            {
                Html += "<font color=\"red\">INVALIDO ERROR EN NOMBRE DE BUQUE</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

            Html += "d. Fecha de Llegada : " + nombre.Trim().Substring(10 + d_buque.Length, 6) + " - ";

            string fe = nombre.Trim().Substring(10 + d_buque.Length, 6);
            string fi = (f_llegada.Day > 9 ? f_llegada.Day.ToString() : "0" + f_llegada.Day.ToString()) + (f_llegada.Month > 9 ? f_llegada.Month.ToString() : "0" + f_llegada.Month.ToString()) + f_llegada.Year.ToString().Substring(2, 2);

            if (fe != fi)
            {
                Html += "<font color=\"red\">INVALIDO FECHAS NO COINCIDEN</font><br />";
                ErrorNombre = true;
            }
            else
            {
                Html += "OK.<br />";
            }

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
                        int correlativo_c = DetaDocDAL.ObtenerCorrelativoDoc(IdRegEnca, 2);

                        if ((correlativo_c + 1) != Convert.ToInt32(cor))
                        {
                            Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", correlativo_c + 1);
                            ErrorNombre = true;
                        }
                        else
                        {
                            Html += "OK.<br />";
                        }
                    }
                    else
                        if (Convert.ToInt32(cor) > 1)
                    {
                        Html += string.Format("<font color=\"red\">INVALIDO SIGUIENTE CORRELATIVO VALIDO : {0} </font><br />", "01");
                        ErrorNombre = true;
                    }
                    else
                    {
                        Html += "OK.<br />";
                    }
                }
            }

            Html += "<br />";
            return ErrorNombre;
        }


        public void Imprimir()
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append("<div id='updiv' style='font-family: Segoe UI Light; color: black;font-size: 14px; border-style: none;border-color: inherit;border-width: 0; margin: 0;padding: 0;'>");

            sb.Append("<div id='updiv'>");
            //sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;;COLOR:black;font-family:verdana;Position:relative;Text-Align:center;left:5%;'>");
            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
            sb.Append("<script language=javascript>");
            sb.Append("var dts=0; var dtmax=10;");
            sb.Append("window.onload = detectarCarga;");
            sb.Append("function detectarCarga(){ document.getElementById('carga').style.display='none'; }");

            sb.Append("function ShowWait(){var output;output='Archivo esta siendo procesado...';dts++;if(dts>=dtmax)dts=1;");
            //sb.Append("for(var x=0;x < dts; x++){output+='" & ChrW(127) & "';}up_div.innerText=output;up_div.style.color='red';}");
            sb.Append("function StartShowWait(){carga.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
            sb.Append("StartShowWait();</script>");
            //sb.Append("</script>");
            //sb.Append("<div id='fondo' style='background-color: #000;filter:alpha(opacity=60);-moz-opacity: 0.6;opacity: 0.6;'>");
            sb.Append("<div id='carga' style='position: absolute;left:50%;top:50%;width:300px;height:200px;margin-top:-100px;margin-left:-150px;text-align:center;'><img src='../CSS/Images/loading1.gif' /><br/> ");
            sb.Append("<span>Cargando... </span></div>");
            sb.Append("</div>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
        }

        //Codigo javascript para borrar el progressbar
        public void Clear(string Pagina)
        {
            StringBuilder sbc = new StringBuilder();
            sbc.Append("<script language='JavaScript'>");
            sbc.Append("alert('Procesamiento Finalizado..');");
            sbc.Append("var childnode=document.getElementById('carga');");
            //sbc.Append("var removednode=document.getElementById('updiv').removeChild(childnode);");
            sbc.Append("window.location.href= '" + Pagina + "'; ");
            //sbc.Append("history.go(1)")   
            //sbc.Append("window.close();")
            sbc.Append("</script>");
            sbc.Append("");
            HttpContext.Current.Response.Write(sbc);
        }

        public void Clear(string Pagina, string Mensaje)
        {
            StringBuilder sbc = new StringBuilder();
            sbc.Append("<script language='JavaScript'>");
            sbc.Append(string.Format("alert('{0}');", Mensaje));
            sbc.Append("alert('Procesamiento Finalizado..');");
            sbc.Append("var childnode=document.getElementById('carga');");
            //sbc.Append("var removednode=document.getElementById('updiv').removeChild(childnode);");
            sbc.Append("window.location.href= '" + Pagina + "'; ");
            //sbc.Append("history.go(1)")   
            //sbc.Append("window.close();")
            sbc.Append("</script>");
            sbc.Append("");
            HttpContext.Current.Response.Write(sbc);
        }

        public void MensajeConfirm(string Pagina)
        {
            StringBuilder sbc = new StringBuilder();
            sbc.Append("<script language='JavaScript'>");
            sbc.Append(@"function Confirm() {
                var confirm_value = document.createElement('INPUT');
                confirm_value.type = 'hidden';
                confirm_value.name = 'confirm_value';
                if (confirm('Quieres omitir la validación?')) {
                    confirm_value.value = 'Yes';
                } else {
                    confirm_value.value = 'No';
                }
                document.forms[0].appendChild(confirm_value);
            }");
            sbc.Append("var childnode=document.getElementById('carga');");
            sbc.Append("var removednode=document.getElementById('updiv').removeChild(childnode);");
            //sbc.Append("window.location.href= '" + Pagina + "'; ");
            //sbc.Append("history.go(1)")   
            //sbc.Append("window.close();")
            sbc.Append("</script>");
            sbc.Append("");
            HttpContext.Current.Response.Write(sbc);
        }

        // Cargar Booking
        public void AlmacenarBooking()
        {
            Save = false;

            //Verificando si carga archivo
            #region "Carga Archivo"
            if (HttpContext.Current.Session["ruta"] != null && HttpContext.Current.Session["archivo"] != null)
            {
                a = HttpContext.Current.Session["ruta"].ToString();
            }
            else
            {
                throw new System.InvalidOperationException("Debe seleccionar el archivo a cargar");
            }
            #endregion

            string nombre = Path.GetFileName(a);
            bool ErrorNombre;

            _fecha = DetaNavieraLINQ.FechaBD();



            CorreoBooking(HttpContext.Current.Session["c_naviera"].ToString());
            Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";


            Html += "<b><u> CARGA DE BOOKING </b></u><br />";
            Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;\">";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
            Html += "</tr>";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + c_usuario + "</font></td>";
            Html += "</tr>";
            Html += "</table>";
            Html += "<br />";
            Html += "<b><u> VALIDANDO NOMBRE </b></u><br />";

            if (nombre.Trim().Length == 0)
            {
                throw new System.InvalidOperationException("NO SE HA ESPECIFICADO UN NOMBRE DE ARCHIVO");
            }

            string extension = Path.GetExtension(a);
            if (Path.GetExtension(a) != ".xls" && Path.GetExtension(a) != ".xlsx")
            {
                throw new System.InvalidOperationException("La extensión del archivo no es válida, revisar que sea un libro de Excel");
            }

            int ar = Path.GetFileName(a).Length;
            int ar1 = Path.GetExtension(a).Length;


            if ((Path.GetFileName(a).Length - Path.GetExtension(a).Length) != 12)
            {
                throw new System.InvalidOperationException("Longitud de nombre de archivo no es válida");
            }

            int countHojas = ArchivoExcelDAL.GetCountHojas(a);

            if (countHojas > 1)
            {
                throw new System.InvalidOperationException("Libro de Excel posee más de una hoja activa");
            }

            EnvioCorreo _correo = new EnvioCorreo();

            Html += "<font size=2>";

            ErrorNombre = ValidarArchivoBook(nombre);
            int cont = 0;

            if (ErrorNombre == false)
            {
                List<ResultadoValidacion> validarPrueba = new List<ResultadoValidacion>();
                List<ResultadoValidacion> listaValid = new List<ResultadoValidacion>();

                // Variables en el entorno

                validarPrueba = DetaNavieraLINQ.ValidarBooking(a, DBComun.Estado.verdadero);
                listaValid = HttpContext.Current.Session["listaBooking"] as List<ResultadoValidacion>;





                if (listaValid.Count > 0)
                {
                    #region "Archivo Con Errores"



                    #region "Inicio Correo"

                    Html += "<b><u>DETALLE DE INCONSISTENCIAS</b></u><br />";
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"35%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>SHIPMENT NUMBER</font></th>";
                    Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTAINER</font></th>";
                    Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>SEAL NUMBER</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    #endregion

                    #region "Consultas"

                    List<ArchivoBooking> pBooks = new List<ArchivoBooking>();

                    pBooks = ArchivoBookingDAL.GetRango(a, DBComun.Estado.verdadero);

                    var query = (from arch in pBooks
                                 join valid in listaValid.Where(t => t.Resultado == 1) on arch.num_fila equals valid.NumFila
                                 orderby arch.num_fila
                                 select new
                                 {
                                     valid.NumFila,
                                     valid.NumCelda,
                                     valid.Descripcion
                                 }).ToList();

                    var queryTodos = (from arch in pBooks
                                      where
                                         (from valid in listaValid
                                          where valid.Resultado == 1
                                          select new
                                          {
                                              valid.NumFila
                                          }).Contains(new { NumFila = arch.num_fila })
                                      select new ArchivoBooking
                                      {
                                          num_fila = arch.num_fila,
                                          shipment = arch.shipment,
                                          n_contenedor = arch.n_contenedor,
                                          n_sello = arch.n_sello
                                      }).ToList();
                    #endregion

                    #region "Detalle Correo"

                    foreach (ArchivoBooking itemArch in queryTodos)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemArch.shipment + "</font></td>";
                        Html += "<td height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemArch.n_contenedor + "</font></td>";
                        Html += "<td height=\"30\"><font size=2 color=blue>" + itemArch.n_sello + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        Html += "<tr>";
                        Html += "<td colspan=3>";

                        //List<ResultadoValidacion> _lista = queryTodos.Where(p => p.num_fila == itemArch.num_fila).ToList() as List<ResultadoValidacion>;
                        var _lista = query.Where(p => p.NumFila == itemArch.num_fila).ToList();
                        if (_lista.Count > 0)
                        {
                            Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                            Html += "<tr>";
                            Html += "<td width=\"5%\" height=\"25\"><font color=red size=2></font></th>";
                            Html += "<td width=\"5%\" height=\"25\"><font color=red size=2>FILA</font></th>";
                            Html += "<td width=\"5%\" height=\"25\"><font color=red size=2>CELDA</font></th>";
                            Html += "<td width=\"25%\" height=\"25\"><font color=red size=2>DESCRIPCION</font></th>";
                            Html += "</tr>";
                            foreach (var itemRela in _lista)
                            {
                                Html += "<td height=\"25\"><font size=2 color=red>  </font></td>";
                                Html += "<td height=\"25\"><font size=2 color=red>" + itemRela.NumFila + "</font></td>";
                                Html += "<td height=\"25\"><font size=2 color=red>" + itemRela.NumCelda + "</font></td>";
                                Html += "<td height=\"25\"><font size=2 color=red>" + itemRela.Descripcion + "</font></td>";
                                Html += "</tr>";
                            }
                            Html += "</font></table>";
                        }
                    }
                    Html += "</td></tr></font></table>";
                    #endregion

                    _correo.Subject = string.Format("PASO 2 de 2: Validación CEPA: DENEGADA Listado de Bookings de {0} - {1}", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"));
                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString()); ;

                    if (_listaCC == null)
                    {
                        _listaCC = new List<Notificaciones>();
                    }

                    _correo.ListaCC = _listaCC;
                    _correo.ListArch.Add(a);
                    _correo.Asunto = Html;
                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                    _correo = null;

                    throw new Exception("Archivo presento inconsistencias revisar correo electrónico");
                    #endregion
                }
                else
                {
                    #region "Almacenar"

                    #region "Desactivar Encabezado"



                    int IdBooking = Convert.ToInt32(EncaBookingDAL.ObtenerCabecera(HttpContext.Current.Session["c_naviera"].ToString()));

                    if (IdBooking > 0)
                    {
                        int CantBooking = Convert.ToInt32(DetaBookingDAL.CantidadBooking(HttpContext.Current.Session["c_naviera"].ToString(), IdBooking));
                        if (CantBooking > 0)
                        {
                            int ActBooking = Convert.ToInt32(DetaBookingDAL.DesactivarBooking(HttpContext.Current.Session["c_naviera"].ToString()));
                            if (ActBooking > 0)
                            {
                                int ActEnca = Convert.ToInt32(EncaBookingDAL.ActualizarCabecera(HttpContext.Current.Session["c_naviera"].ToString()));
                            }
                        }
                    }


                    #endregion

                    #region "Almacenando Encabezado"

                    EncaBooking _enca = new EncaBooking
                    {
                        IdBooking = -1,
                        c_naviera = HttpContext.Current.Session["c_naviera"].ToString(),
                        s_archivo = HttpContext.Current.Session["archivo"].ToString(),
                        b_estado = true
                    };


                    int _resultado = Convert.ToInt32(EncaBookingDAL.Insertar(_enca));
                    #endregion


                    if (_resultado > 0)
                    {

                        #region "Almacenando Detalle"

                        DBComun.sRuta = a;
                        List<ArchivoBooking> _list = new List<ArchivoBooking>();

                        _list = ArchivoBookingDAL.GetRango(a, DBComun.Estado.verdadero);


                        foreach (ArchivoBooking item in _list)
                        {
                            if (item.n_contenedor != null && item.n_contenedor != "")
                            {
                                DetaBooking _detaNavi = new DetaBooking
                                {
                                    IdDetaBooking = -1,
                                    IdBooking = _resultado,
                                    shipment_name = item.shipment,
                                    n_contenedor = item.n_contenedor,
                                    n_sello = item.n_sello,
                                    b_estado = true,
                                    b_marca = false
                                };

                                int _resulDeta = Convert.ToInt32(DetaBookingDAL.Insertar(_detaNavi));


                                if (_resulDeta > 0)
                                {
                                    cont = cont + 1;
                                    Save = true;
                                }
                            }
                        }

                        Html += "<b><u>DETALLE DE CARGA DE LISTADO</b></u><br />";
                        Html += "<br /> Módulo : Carga de listado de bookings <br />";
                        Html += "Mensaje : Notificación carga de listado exitosa <br /><br />";
                        Html += string.Format("Se notifica que la carga del archivo de bookings, de {0} con fecha de actualización {1}, se validaron {2} asiganaciones de bookings de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"), cont.ToString());
                        Html += "<br /><br />";


                        _correo.Subject = string.Format("PASO 2 de 2: Validación CEPA: ACEPTADA Listado de Bookings de {0} - {1}", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"));
                        _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                        List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString()); ;

                        if (_listaCC == null)
                        {
                            _listaCC = new List<Notificaciones>();
                        }

                        _correo.ListaCC = _listaCC;
                        _correo.Asunto = Html;
                        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                        _correo = null;


                        if (Save == true)
                        {
                            throw new Exception("Archivo registrado correctamente");
                        }

                        #endregion

                    }
                }
            }

            #endregion

            HttpContext.Current.Session["listaBooking"] = null;
        }

        public void AlmacenarAduana()
        {
            List<string> pListaNo = new List<string>();
            EnvioCorreo _correo = new EnvioCorreo();

            string Html;
            bool mEstado = false;



            if (HttpContext.Current.Session["ruta"] != null && HttpContext.Current.Session["archivo"] != null)
            {
                a = HttpContext.Current.Session["ruta"].ToString();
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert('Debe de seleccionar archivo a cargar');</script>");
                return;
            }

            int _resulDeta = 0;

            DBComun.sRuta = a;
            List<ArchivoAduanaValid> _list = new List<ArchivoAduanaValid>();

            int n_manif = 0;

            _list = ArchivoExcelDAL.GetValidAduana(a, DBComun.Estado.verdadero);

            //List<ArchivoAduanaValid> pListV = new List<ArchivoAduanaValid>();
            //pListV = ArchivoExcelDAL.GetValidAduana1(a, DBComun.Estado.verdadero);

            foreach (ArchivoAduanaValid itemCR in _list)
            {
                n_manif = itemCR.n_manifiesto;
                break;
            }

            if (n_manif != n_manifiesto)
            {
                throw new Exception("El número de manifiesto seleccionado no coincide con el del archivo.");
            }

            string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.verdadero, n_manifiesto, "0", 1);
            string n_contenedor = null;
            string n_BL = null;
            string a_mani = null;
            if (_list.Count > 0)
            {
                foreach (ArchivoAduanaValid item in _list)
                {
                    n_contenedor = item.n_contenedor.Trim().TrimEnd().TrimStart();
                    n_contenedor = n_contenedor.Replace("-", "");
                    n_BL = item.n_BL.Trim().TrimEnd().TrimStart();
                    a_mani = item.a_mani.Trim().TrimEnd().TrimStart();

                    if ((n_contenedor != null && n_contenedor != "") && n_BL != null && n_BL != "")
                    {
                        if (n_contenedor.TrimEnd().TrimStart().Trim().Length == 11)
                        {
                            ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                            {
                                IdValid = -1,
                                n_contenedor = n_contenedor,
                                n_manifiesto = n_manifiesto,
                                n_BL = n_BL,
                                a_mani = a_mani
                            };

                            _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.verdadero));

                            if (_resulDeta == 2)
                            {
                                pListaNo.Add(n_contenedor);
                            }
                        }
                        else
                        {
                            //pListaNo.Add(item.n_contenedor);
                        }
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert('Archivo no posee registros a cargar');</script>");
            }



            List<EncaNaviera> pEnca = EncaNavieraDAL.ObtenerNavierasValid(DBComun.Estado.verdadero, n_manifiesto, IdRegC, a_manip, IdDocC);

            if (pEnca.Count > 0)
            {
                foreach (EncaNaviera itemEnca in pEnca)
                {
                    mEstado = false;
                    string c_navi_corto = EncaNavieraDAL.ObtenerNavi(DBComun.Estado.verdadero, itemEnca.c_naviera);

                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                    Html += "<b><u> CARGA DE ARCHIVO  </b></u><br />";
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                    Html += "<tr>";
                    _fecha = DetaNavieraLINQ.FechaBD();
                    Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                    Html += "</tr>";
                    Html += "<tr>";
                    Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + c_usuario + "</font></td>";
                    Html += "</tr>";
                    Html += "</table>";
                    Html += "<br />";

                    List<ContenedoresAduana> pLista = ResulNavieraDAL.ObtenerAutorizadosADUANA(DBComun.Estado.verdadero, n_manifiesto, itemEnca.c_naviera, Convert.ToInt32(a_mani));


                    Html += "<b><u>DETALLE DE VALIDACIÓN</b></u><br />";
                    Html += "<br />";
                    Html += string.Format("<b><u>AUTORIZADOS ({0})</b></u><br />", pLista.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    if (pLista.Count > 0)
                    {
                        foreach (ContenedoresAduana item in pLista)
                        {
                            Html += "<tr>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                            Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                            Html += "</tr>";
                            Html += "</font>";

                        }
                    }
                    else
                    {
                        foreach (ContenedoresAduana item in pLista)
                        {
                            Html += "<tr>";
                            Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                            Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                            Html += "</tr>";
                            Html += "</font>";
                            mEstado = true;
                        }

                        mEstado = true;
                    }

                    Html += "</table>";

                    List<ContenedoresAduana> pLista1 = ResulNavieraDAL.ObtenerAutorizadosNOADUANA(DBComun.Estado.verdadero, n_manifiesto, itemEnca.c_naviera, Convert.ToInt32(itemEnca.a_manifiesto));

                    Html += "<br />";
                    Html += string.Format("<b><u>NO REPORTADOS A ADUANA ({0})</b></u><br />", pLista1.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (ContenedoresAduana item in pLista1)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        mEstado = true;
                    }

                    Html += "</table>";

                    List<ContenedoresAduana> pLista2 = ResulNavieraDAL.ObtenerAutorizadosNONAVIERA(DBComun.Estado.verdadero, n_manifiesto, itemEnca.c_naviera, Convert.ToInt32(itemEnca.a_manifiesto));

                    Html += "<br />";
                    Html += string.Format("<b><u>NO REPORTADOS A CEPA ({0})</b></u><br />", pLista2.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (ContenedoresAduana item in pLista2)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                    }

                    Html += "</table>";



                    Html += "<br />";
                    Html += string.Format("<b><u>CONTENEDORES QUE NO PASAN VALIDACIÓN SEGUN BIC ({0})</b></u><br />", pListaNo.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    int cont = 0;

                    foreach (string item in pListaNo)
                    {
                        cont = cont + 1;
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + cont.ToString() + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        //mEstado = true;
                    }

                    Html += "</table>";


                    List<ContenedoresAduana> pLista3 = ResulNavieraDAL.ObtenerRepetidos(DBComun.Estado.verdadero, n_manifiesto);

                    Html += "<br />";
                    Html += string.Format("<b><u>REPETIDOS EN MANIFIESTO SEGÚN ADUANA ({0})</b></u><br />", pLista3.Count);
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    foreach (ContenedoresAduana item in pLista3)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                        Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                    }

                    Html += "</table>";

                    if (mEstado == true)
                    {
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=red>En espera de correcciones por la Naviera </font>";
                        _correo.Subject = string.Format("PASO 3 de 4: Validación ADUANA: DENEGADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_navi_corto, d_buque, itemEnca.c_voyage, n_manifiesto);
                        int resuly = Convert.ToInt32(DetaDocDAL.ActualizarValidacion(0, n_manifiesto, IdRegC, DBComun.Estado.verdadero, IdDocC));
                    }
                    else
                    {
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=blue>En espera autorización de ADUANA </font>";
                        _correo.Subject = string.Format("PASO 3 de 4: Validación ADUANA: ACEPTADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", c_navi_corto, d_buque, itemEnca.c_voyage, n_manifiesto);
                        int resuly = Convert.ToInt32(DetaDocDAL.ActualizarValidacion(1, n_manifiesto, IdRegC, DBComun.Estado.verdadero, IdDocC));
                    }


                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                    if (_listaCC == null)
                    {
                        _listaCC = new List<Notificaciones>();
                    }

                    _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.verdadero, itemEnca.c_naviera));
                    _correo.ListaCC = _listaCC;

                    //Notificaciones noti = new Notificaciones
                    //{
                    //    sMail = "elsa.sosa@cepa.gob.sv",
                    //    dMail = "Elsa Sosa"
                    //};

                    //List<Notificaciones> pLisN = new List<Notificaciones>();

                    //pLisN.Add(noti);

                    //_correo.ListaNoti = pLisN;

                    _correo.Asunto = Html;
                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);

                }
                _correo = null;
                HttpContext.Current.Response.Write("Archivo registrado correctamente");

            }
            else
            {
                HttpContext.Current.Response.Write("<script>alert('No se encontraron navieras a emitir validación');</script>");
            }


        }

        // Cargar Booking
        public void AlmacenarReestiba()
        {
            Save = false;

            //Verificando si carga archivo
            #region "Carga Archivo"
            if (HttpContext.Current.Session["ruta"] != null && HttpContext.Current.Session["archivo"] != null)
            {
                a = HttpContext.Current.Session["ruta"].ToString();
            }
            else
            {
                throw new System.InvalidOperationException("Debe seleccionar el archivo a cargar");
            }
            #endregion

            int _exis = Convert.ToInt32(DetaDocDAL.ArchivosExistentes(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString(), HttpContext.Current.Session["archivo"].ToString()));

            if (_exis > 0)
            {
                throw new System.InvalidOperationException("Ya existe un archivo almacenado con este nombre");
            }

            string cn_voyage = null;
            DBComun.sRuta = a;
            List<ArchivoExcel> listVoyage = ArchivoExcelDAL.GetVoyage(a, DBComun.Estado.verdadero);

            foreach (ArchivoExcel v in listVoyage)
            {
                cn_voyage = v.c_voyage;
                break;
            }

            int _arch = Convert.ToInt32(DetaDocDAL.archReestiba(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString()));

            if (_arch > 0)
            {
                IdRegEnca = Convert.ToInt32(EncaNavieraDAL.ObtenerIdReg(c_imo, c_llegada, HttpContext.Current.Session["c_naviera"].ToString()));
            }

            string c_imoVa = ArchivoExcelDAL.GetImoRe(a, DBComun.Estado.verdadero);
            string Mensaje = "";

            if (c_imoVa == null)
            {
                throw new Exception("El número de imo indicado presenta errores");
            }

            if (Convert.ToDouble(c_imoVa) > 0)
            {
                if (c_imo != c_imoVa)
                {
                    Mensaje = "El IMO del archivo no coincide con el del buque seleccionado";
                    //HttpContext.Current.Response.Write("<script>alert('" + Mensaje + "');</script>");
                    throw new Exception(Mensaje);

                }
            }
            else
            {
                throw new FormatException("El número de imo indicado presenta errores en su formato, elimine espacios y verifique número");
            }

            string nombre = Path.GetFileName(a);
            bool ErrorNombre;

            _fecha = DetaNavieraLINQ.FechaBD();



            CorreoBooking(HttpContext.Current.Session["c_naviera"].ToString());
            Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";


            Html += "<b><u> CARGA DE ARCHIVO  </b></u><br />";
            Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;\">";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
            Html += "</tr>";
            Html += "<tr>";
            Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
            Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + c_usuario + "</font></td>";
            Html += "</tr>";
            Html += "</table>";
            Html += "<br />";
            Html += "<b><u> VALIDANDO NOMBRE </b></u><br />";

            if (nombre.Trim().Length == 0)
            {
                throw new System.InvalidOperationException("NO SE HA ESPECIFICADO UN NOMBRE DE ARCHIVO");
            }

            string extension = Path.GetExtension(a);
            if (Path.GetExtension(a) != ".xls" && Path.GetExtension(a) != ".xlsx")
            {
                throw new System.InvalidOperationException("La extensión del archivo no es válida, revisar que sea un libro de Excel");
            }

            int ar = Path.GetFileName(a).Length;
            int ar1 = Path.GetExtension(a).Length;
            int ar3 = 19 + d_buque.Length;

            if ((Path.GetFileName(a).Length - Path.GetExtension(a).Length) != (19 + d_buque.Length))
            {
                throw new System.InvalidOperationException("Longitud de nombre de archivo no es válida");
            }

            int countHojas = ArchivoExcelDAL.GetCountHojas(a);

            if (countHojas > 1)
            {
                throw new System.InvalidOperationException("Libro de Excel posee más de una hoja activa");
            }

            EnvioCorreo _correo = new EnvioCorreo();

            Html += "<font size=2>";

            ErrorNombre = ValidarArchivoBook(nombre);
            int cont = 0;

            if (ErrorNombre == false)
            {
                List<ResultadoValidacion> validarPrueba = new List<ResultadoValidacion>();
                List<ResultadoValidacion> listaValid = new List<ResultadoValidacion>();

                // Variables en el entorno

                validarPrueba = DetaNavieraLINQ.ValidarBooking(a, DBComun.Estado.verdadero);
                listaValid = HttpContext.Current.Session["listaBooking"] as List<ResultadoValidacion>;





                if (listaValid.Count > 0)
                {
                    #region "Archivo Con Errores"



                    #region "Inicio Correo"

                    Html += "<b><u>DETALLE DE INCONSISTENCIAS</b></u><br />";
                    Html += "<br />";

                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    Html += "<tr>";
                    Html += "<center>";
                    Html += "<td width=\"35%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>SHIPMENT NUMBER</font></th>";
                    Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTAINER</font></th>";
                    Html += "<td width=\"25%\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>SEAL NUMBER</font></th>";
                    Html += "</center>";
                    Html += "</tr>";

                    #endregion

                    #region "Consultas"

                    List<ArchivoBooking> pBooks = new List<ArchivoBooking>();

                    pBooks = ArchivoBookingDAL.GetRango(a, DBComun.Estado.verdadero);

                    var query = (from arch in pBooks
                                 join valid in listaValid.Where(t => t.Resultado == 1) on arch.num_fila equals valid.NumFila
                                 orderby arch.num_fila
                                 select new
                                 {
                                     valid.NumFila,
                                     valid.NumCelda,
                                     valid.Descripcion
                                 }).ToList();

                    var queryTodos = (from arch in pBooks
                                      where
                                         (from valid in listaValid
                                          where valid.Resultado == 1
                                          select new
                                          {
                                              valid.NumFila
                                          }).Contains(new { NumFila = arch.num_fila })
                                      select new ArchivoBooking
                                      {
                                          num_fila = arch.num_fila,
                                          shipment = arch.shipment,
                                          n_contenedor = arch.n_contenedor,
                                          n_sello = arch.n_sello
                                      }).ToList();
                    #endregion

                    #region "Detalle Correo"

                    foreach (ArchivoBooking itemArch in queryTodos)
                    {
                        Html += "<tr>";
                        Html += "<td height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemArch.shipment + "</font></td>";
                        Html += "<td height=\"35\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + itemArch.n_contenedor + "</font></td>";
                        Html += "<td height=\"30\"><font size=2 color=blue>" + itemArch.n_sello + "</font></td>";
                        Html += "</tr>";
                        Html += "</font>";
                        Html += "<tr>";
                        Html += "<td colspan=3>";

                        //List<ResultadoValidacion> _lista = queryTodos.Where(p => p.num_fila == itemArch.num_fila).ToList() as List<ResultadoValidacion>;
                        var _lista = query.Where(p => p.NumFila == itemArch.num_fila).ToList();
                        if (_lista.Count > 0)
                        {
                            Html += "<table style=\"font-family: 'Arial' ; font-size: 12px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                            Html += "<tr>";
                            Html += "<td width=\"5%\" height=\"25\"><font color=red size=2></font></th>";
                            Html += "<td width=\"5%\" height=\"25\"><font color=red size=2>FILA</font></th>";
                            Html += "<td width=\"5%\" height=\"25\"><font color=red size=2>CELDA</font></th>";
                            Html += "<td width=\"25%\" height=\"25\"><font color=red size=2>DESCRIPCION</font></th>";
                            Html += "</tr>";
                            foreach (var itemRela in _lista)
                            {
                                Html += "<td height=\"25\"><font size=2 color=red>  </font></td>";
                                Html += "<td height=\"25\"><font size=2 color=red>" + itemRela.NumFila + "</font></td>";
                                Html += "<td height=\"25\"><font size=2 color=red>" + itemRela.NumCelda + "</font></td>";
                                Html += "<td height=\"25\"><font size=2 color=red>" + itemRela.Descripcion + "</font></td>";
                                Html += "</tr>";
                            }
                            Html += "</font></table>";
                        }
                    }
                    Html += "</td></tr></font></table>";
                    #endregion

                    _correo.Subject = string.Format("PASO 2 de 2: Validación CEPA: DENEGADA Listado de Bookings de {0} - {1}", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"));
                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString()); ;

                    if (_listaCC == null)
                    {
                        _listaCC = new List<Notificaciones>();
                    }

                    _correo.ListaCC = _listaCC;
                    _correo.ListArch.Add(a);
                    _correo.Asunto = Html;
                    _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                    _correo = null;

                    throw new Exception("Archivo presento inconsistencias revisar correo electrónico");
                    #endregion
                }
                else
                {
                    #region "Almacenar"

                    #region "Desactivar Encabezado"



                    int IdBooking = Convert.ToInt32(EncaBookingDAL.ObtenerCabecera(HttpContext.Current.Session["c_naviera"].ToString()));

                    if (IdBooking > 0)
                    {
                        int CantBooking = Convert.ToInt32(DetaBookingDAL.CantidadBooking(HttpContext.Current.Session["c_naviera"].ToString(), IdBooking));
                        if (CantBooking > 0)
                        {
                            int ActBooking = Convert.ToInt32(DetaBookingDAL.DesactivarBooking(HttpContext.Current.Session["c_naviera"].ToString()));
                            if (ActBooking > 0)
                            {
                                int ActEnca = Convert.ToInt32(EncaBookingDAL.ActualizarCabecera(HttpContext.Current.Session["c_naviera"].ToString()));
                            }
                        }
                    }


                    #endregion

                    #region "Almacenando Encabezado"

                    EncaBooking _enca = new EncaBooking
                    {
                        IdBooking = -1,
                        c_naviera = HttpContext.Current.Session["c_naviera"].ToString(),
                        s_archivo = HttpContext.Current.Session["archivo"].ToString(),
                        b_estado = true
                    };


                    int _resultado = Convert.ToInt32(EncaBookingDAL.Insertar(_enca));
                    #endregion


                    if (_resultado > 0)
                    {

                        #region "Almacenando Detalle"

                        DBComun.sRuta = a;
                        List<ArchivoBooking> _list = new List<ArchivoBooking>();

                        _list = ArchivoBookingDAL.GetRango(a, DBComun.Estado.verdadero);


                        foreach (ArchivoBooking item in _list)
                        {
                            if (item.n_contenedor != null && item.n_contenedor != "")
                            {
                                DetaBooking _detaNavi = new DetaBooking
                                {
                                    IdDetaBooking = -1,
                                    IdBooking = _resultado,
                                    shipment_name = item.shipment,
                                    n_contenedor = item.n_contenedor,
                                    n_sello = item.n_sello,
                                    b_estado = true,
                                    b_marca = false
                                };

                                int _resulDeta = Convert.ToInt32(DetaBookingDAL.Insertar(_detaNavi));


                                if (_resulDeta > 0)
                                {
                                    cont = cont + 1;
                                    Save = true;
                                }
                            }
                        }

                        Html += "<b><u>DETALLE DE CARGA DE LISTADO</b></u><br />";
                        Html += "<br /> Módulo : Carga de listado de bookings <br />";
                        Html += "Mensaje : Notificación carga de listado exitosa <br /><br />";
                        Html += string.Format("Se notifica que la carga del archivo de bookings, de {0} con fecha de actualización {1}, se validaron {2} asiganaciones de bookings de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"), cont.ToString());
                        Html += "<br /><br />";


                        _correo.Subject = string.Format("PASO 2 de 2: Validación CEPA: ACEPTADA Listado de Bookings de {0} - {1}", HttpContext.Current.Session["c_navi_corto"].ToString(), _fecha.ToString("dd/MM/yyyy HH:mm"));
                        _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                        List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_booking", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString()); ;

                        if (_listaCC == null)
                        {
                            _listaCC = new List<Notificaciones>();
                        }

                        _correo.ListaCC = _listaCC;
                        _correo.Asunto = Html;
                        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                        _correo = null;


                        if (Save == true)
                        {
                            throw new Exception("Archivo registrado correctamente");
                        }

                        #endregion

                    }
                }
            }

            #endregion

            HttpContext.Current.Session["listaBooking"] = null;
        }

    }

}


