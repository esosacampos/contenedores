using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.BL;
using System.Text;
using CEPA.CCO.Linq;
using System.Threading;
using System.IO;

namespace CEPA.CCO.UI.Web
{
    public partial class wfSustituirArch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScript.GetPostBackEventReference(btnCargar, string.Empty);

            try
            {

                if (!IsPostBack)
                {
                    if (Request.QueryString["c_buque"] == null || Request.QueryString["c_llegada"] == null)
                    {
                        throw new Exception("Falta código de buque");
                    }
                    else
                    {
                        CargarEncabezado();
                        CargarArchivos();
                        if(lblArchivos.Items.Count <= 0)
                            throw new Exception("CEPA-Contenedores Error: Este buque no posee archivos disponibles a sustituir..");
                    }

                    // Propiedades del control.
                    ucMultiFileUpload1.Titulo = "Cargar Archivo";
                    ucMultiFileUpload1.Comment = "máx. 4 MB en total.";
                    ucMultiFileUpload1.MaxFilesLimit = 5;
                    ucMultiFileUpload1.DestinationFolder = "~/Archivos"; // única propiedad obligatoria.
                    ucMultiFileUpload1.FileExtensionsEnabled = ".xls|.xlsx";
                }
            }
            catch (Exception ex)
            {                
                CargarArchivosLINQ _carga = new CargarArchivosLINQ();
                _carga.Clear("wfPrincipalNavi.aspx", ex.Message);
            }
        }

        #region "Metodos"

        private void CargarEncabezado()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();
            List<EncaBuque> _encaList = new List<EncaBuque>();
            _encaList = _encaBL.ObtenerBuqueID(DBComun.Estado.verdadero, Session["c_naviera"].ToString(), Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString());

            if (_encaList.Count > 0)
            {
                foreach (EncaBuque item in _encaList)
                {
                    c_imo.Text = item.c_imo;
                    d_buque.Text = item.d_buque;
                    f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                    c_llegada.Text = item.c_llegada;
                }
            }
        }

        private void CargarArchivos()
        {
            lblArchivos.DataTextField = "s_archivo";
            lblArchivos.DataValueField = "s_archivo";
            lblArchivos.DataSource = DetaDocDAL.ObtenerDocS(DBComun.Estado.verdadero, Session["c_naviera"].ToString(), c_llegada.Text);
            lblArchivos.DataBind();
        }

        #endregion

        protected void lblArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("wfPrincipalNavi.aspx");
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
           
            ucMultiFileUpload1.UploadFiles(true);
            string a = null;         
            string Html;
            DateTime _fecha;
            bool Save = false;
            List<int> _listaDeta = new List<int>();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            int n_manifiesto = 0;

            try
            {                
                _cargar.Imprimir();
                Sustitucion(ref a, ref Save, _listaDeta, _cargar, ref n_manifiesto, out Html, out _fecha);
                Thread.Sleep(4000);
                _cargar.Clear(string.Format("wfSustituirArch.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()));

            }
            catch (FormatException ef)
            {              
                CargarArchivosLINQ _carga = new CargarArchivosLINQ();
                _carga.Clear(string.Format("wfSustituirArch.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()), ef.Message);
            }
            catch (Exception ex)
            {                
                CargarArchivosLINQ _carga = new CargarArchivosLINQ();
                _carga.Clear(string.Format("wfSustituirArch.aspx?c_buque={0}&c_llegada={1}", Request.QueryString["c_buque"].ToString(), Request.QueryString["c_llegada"].ToString()), ex.Message);
            }

        }

        private void Sustitucion(ref string a, ref bool Save, List<int> _listaDeta, CargarArchivosLINQ _cargar, ref int n_manifiesto, out string Html, out DateTime _fecha)
        {
            int IdDoc=0;
            if (lblArchivos.Items.Count > 0)
            {
                if (lblArchivos.SelectedValue == null || lblArchivos.SelectedItem == null || lblArchivos.SelectedIndex == -1)
                    throw new Exception("Seleccione el archivo a reemplazar");
                else
                {

                    ucMultiFileUpload1.UploadFiles(true);

                    if (Session["ruta"] != null && Session["archivo"] != null)
                        a = Session["ruta"].ToString();
                    else
                    {
                        throw new Exception("Debe de seleccionar archivo a cargar");
                    }

                    /*  int _exis = Convert.ToInt32(DetaDocDAL.ArchivosExistentes(c_imo.Text, c_llegada.Text, HttpContext.Current.Session["c_naviera"].ToString(), HttpContext.Current.Session["archivo"].ToString()));

                      if (_exis > 0)
                      {

                      }
                      else
                      {
                          HttpContext.Current.Response.Write("<script>alert('Nombre seleccionado no existe para este buque');</script>");
                          return;
                      }*/

                    int IdReg = 0;
                    string cn_voyage = null;
                    DBComun.sRuta = a;
                    List<ArchivoExcel> listVoyage = ArchivoExcelDAL.GetVoyage(a, DBComun.Estado.verdadero);

                    foreach (ArchivoExcel v in listVoyage)
                    {
                        cn_voyage = v.c_voyage;
                        break;
                    }

                    DBComun.sRuta = a;
                    string c_imoVa = ArchivoExcelDAL.GetImo(a, DBComun.Estado.verdadero);
                    string Mensaje = "";
                    int IdRegEnca = 0;

                    IdRegEnca = Convert.ToInt32(EncaNavieraDAL.ObtenerIdReg(c_imo.Text, c_llegada.Text, HttpContext.Current.Session["c_naviera"].ToString()));

                    if (Convert.ToDouble(c_imoVa) > 0)
                    {
                        if (c_imo.Text != c_imoVa)
                        {
                            Mensaje = "El IMO del archivo no coincide con el del buque seleccionado";
                            throw new Exception(Mensaje);
                        }
                    }




                    List<ArchivoExcel> listManif = ArchivoExcelDAL.GetManifiesto(a, DBComun.Estado.verdadero);

                    foreach (ArchivoExcel s in listManif)
                    {
                        n_manifiesto = s.num_manif;
                        break;
                    }

                    string nombre = Path.GetFileName(a);

                    if (nombre.Trim().Length == 0)
                        throw new System.InvalidOperationException("NO SE HA ESPECIFICADO UN NOMBRE DE ARCHIVO");

                   
                    // _correo = null;

                    // Validación Nombre de Archivo.

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
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + User.Identity.Name + "</font></td>";
                    Html += "</tr>";
                    Html += "<tr>";
                    Html += "<td style=\"text-align: left;\" ><font size = 2>Archivo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + nombre + "</font></td>";
                    Html += "</tr>";
                    Html += "</table>";
                    Html += "<br />";
                    Html += "<b><u> VALIDANDO NOMBRE </b></u><br />";



                    if (nombre != lblArchivos.SelectedValue)
                        throw new System.InvalidOperationException("El nombre del archivo debe ser igual que el nombre del archivo a sustituir");

                    string extension = Path.GetExtension(a);
                    if (Path.GetExtension(a) != ".xls" && Path.GetExtension(a) != ".xlsx")
                        throw new System.InvalidOperationException("La extensión del archivo no es válida, revisar que sea un libro de Excel");

                    if ((Path.GetFileName(a).Length - Path.GetExtension(a).Length) != (19 + d_buque.Text.Length))
                        throw new System.InvalidOperationException("Longitud de nombre de archivo no es válida");

                    int countHojas = ArchivoExcelDAL.GetCountHojas(a);

                    if (countHojas > 1)
                        throw new System.InvalidOperationException("Libro de Excel posee más de una hoja activa");

                    EnvioCorreo _correo = new EnvioCorreo();

                    Html += "<font size=2>";

                    CargarArchivosLINQ _carga = new CargarArchivosLINQ();
                    ErrorNombre = _carga.ValidarArchivoD(nombre, IdRegEnca, d_buque.Text, Convert.ToDateTime(f_llegada.Text), lblArchivos.SelectedValue);


/*                    string _resultadoV = null;

                    if (nombre.Trim().Substring(17 + d_buque.Text.Length, 2) == "01")
                    {
                        _resultadoV = ArchivoExcelDAL.ValidarViajeC(DBComun.Estado.verdadero, c_imoVa, cn_voyage);
                        if (_resultadoV != "NULL" || _resultadoV == "Error")
                            throw new Exception("ESTE IMO YA PRESENTO ESTE NÚMERO DE VIAJE");
                    }*/

                    

                    if (ErrorNombre == false)
                    {
                        //Session["lstRegistrados"] 

                        int del = Convert.ToInt32(DetaNavieraDAL.EliminarDetaNaviera(IdRegEnca, nombre));

                        List<ResultadoValidacion> validarPrueba = DetaNavieraLINQ.ValidarDetalle(a, DBComun.Estado.verdadero);
                        List<ResultadoValidacion> listaValid = Session["listaValid"] as List<ResultadoValidacion>;

                        List<DetaNaviera> pListaExist = DetaNavieraDAL.ObtenerRegAnter(c_imo.Text, c_llegada.Text, HttpContext.Current.Session["c_naviera"].ToString(), cn_voyage);


                        if (pListaExist == null)
                            pListaExist = new List<DetaNaviera>();

                        if (pListaExist.Count > 0)
                        {
                            List<ResultadoValidacion> listaVaTa = DetaNavieraLINQ.ValidarDetalle(a, pListaExist, DBComun.Estado.verdadero);
                            listaValid = HttpContext.Current.Session["listaValid"] as List<ResultadoValidacion>;
                        }



                        _cargar.Imprimir();

                        //Html = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
                        if (listaValid.Count > 0)
                        {

                            CorreoRecepcion(n_manifiesto, cn_voyage);
                            #region "ValidacionDetalle"

                            _fecha = DetaNavieraLINQ.FechaBD();
                            #region "Inicio Correo"

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

                            #endregion

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

                            _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): DENEGADA Listado de Importación Buque: {0}, # de Viaje {1}, Manifiesto de Aduana # {2}", d_buque.Text, cn_voyage, n_manifiesto);
                            _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                            List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                            if (_listaCC == null)
                                _listaCC = new List<Notificaciones>();
                            
                            _correo.ListaCC = _listaCC;             
                            _correo.Asunto = Html;
                            _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                            _correo = null;

                            Response.Write("<script>alert('Archivo presento inconsistencias revisar correo electrónico');</script>");
                            #endregion
                        }
                        else
                        {
                            CorreoRecepcion(n_manifiesto, cn_voyage);
                            #region "Almacer Sustitucion"

                            List<DetaDoc> listaDoc = DetaDocDAL.ObtenerDocA(DBComun.Estado.verdadero, Session["c_naviera"].ToString(), c_llegada.Text, lblArchivos.SelectedItem.Text);

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
                                            s_archivo = Session["archivo"].ToString(),
                                            c_imo = c_imo.Text,
                                            c_usuario = Session["c_usuario"].ToString(),
                                            c_llegada = c_llegada.Text,
                                            num_manif = n_manifiesto
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
                                                        IdDoc = _docResul
                                                    };

                                                    int _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarDetaNaviera(_detaNavi));

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

                                    Html += HttpContext.Current.Session["Html"].ToString();

                                    if (_resultado > 0)
                                    {
                                        Html += "<b><u>DETALLE DE CARGA DE LISTADO POR SUSTITUCIÓN</b></u><br />";
                                        Html += "<br /> Módulo : Carga de listado de contenedores de importación por sustitución <br />";
                                        Html += "Mensaje : Notificación carga de listado por sustitución exitosa <br /><br />";                                        
                                        Html += string.Format("Se notifica que la carga del archivo de importación, de {0} para el buque: {1}, # de Viaje {2}, Manifiesto de Aduana #{3}, se validaron {4} contenedores de forma exitosa sin presentar inconsistencias", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque.Text, cn_voyage, n_manifiesto, _listaDeta.Count);


                                        Html += "<br /><br />";
                                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                                        Html += "<font color=blue> En espera de validación de ADUANA </font>";

                                        Html += "</font>";
                                        //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
                                        _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): ACEPTADA Listado de Importación de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque.Text, cn_voyage, n_manifiesto);
                                        _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                        List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                                        if (_listaCC == null)
                                            _listaCC = new List<Notificaciones>();

                                        _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga", DBComun.Estado.verdadero, "219"));
                                        _correo.ListaCC = _listaCC;             
                                        _correo.Asunto = Html;
                                        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                                        _correo = null;


                                        if (Save == true)
                                            HttpContext.Current.Response.Write("<script>alert('Archivo registrado correctamente');</script>");

                                        //int b_noti = Convert.ToInt32(EncaNavieraDAL.ObtenerNoti(c_imo.Text, c_llegada.Text, HttpContext.Current.Session["c_naviera"].ToString()));


                                        //if (b_noti == 1)
                                        //{
                                        //    NotiNavieras _notiNavieras = new NotiNavieras();

                                        //    _notiNavieras.ProcesarCorreoId(DBComun.Estado.verdadero, c_llegada.Text, IdReg, _listaDeta.Min(), _listaDeta.Max(), HttpContext.Current.Session["c_naviera"].ToString());
                                        //}

                                    }
                                    else
                                        throw new Exception("Seleccione el archivo a reemplazar");
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        CorreoRecepcion(n_manifiesto, cn_voyage);

                        Html += "<b><u>DETALLE DE CARGA DE LISTADO POR SUSTITUCIÓN DENEGADO</b></u><br />";
                        Html += "<br /> Módulo : Carga de listado de contenedores de importación por sustitución de archivo <br />";
                        Html += "Mensaje : Notificación carga de listado por sustitución denegada <br /><br />";
                        Html += string.Format("Se notifica que la carga del archivo de importación por sustitución, de {0} para el buque: {1}, # de Viaje {2}, Manifiesto de Aduana #{3} ha sido rechazado por las inconsistencias detalladas en el presente", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque.Text, cn_voyage, n_manifiesto);
                        
                        Html += "</font>";

                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=red>En espera de correcciones por la Naviera </font>";

                        _correo.Subject = _correo.Subject = string.Format("PASO 2 de 4: Validación CEPA (SUSTITUCIÓN): DENEGADA Listado de Importación Buque: {0}, # de Viaje {1}, Manifiesto de Aduana # {2}", d_buque.Text, cn_voyage, n_manifiesto);
                        _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
                        List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());

                        if (_listaCC == null)
                            _listaCC = new List<Notificaciones>();

                        _correo.ListaCC = _listaCC;             
                        _correo.Asunto = Html;
                        _correo.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
                        _correo = null;

                        HttpContext.Current.Response.Write("<script>alert('Archivo presento inconsistencias revisar correo electrónico');</script>");
                    }
                   
                }
            }
            else
                throw new Exception("Seleccione el archivo a reemplazar");
        }

        private void CorreoRecepcion(int n_manifiesto, string cn_voyage)
        {
            string Html1;
            EnvioCorreo _correo1 = new EnvioCorreo();

            Html1 = "<dir style=\"font-family: 'Arial'; font-size: 12px; line-height: 1.2em\">";
            Html1 += "<br />MÓDULO : RECEPCIÓN DE LISTADO DE IMPORTACIÓN POR SUSTITUCIÓN <br />";
            Html1 += "TIPO DE MENSAJE : NOTIFICACIÓN RECEPCIÓN DE ARCHIVO POR SUSTITUCIÓN <br /><br />";
            Html1 += string.Format("Se notifica que la recepción del archivo de importación por sustitución, de {0} para el buque {1}, número de Viaje {2} y manifiesto de ADUANA # {3}, en la cual solicitan la sustitución del archivo {4}, y queda en espera de resultados de validación", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque, cn_voyage, n_manifiesto, lblArchivos.SelectedValue);
            

            Html1 += "<br /><br />";
            Html1 += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
            Html1 += "<font color=blue> En espera de validación de CEPA </font>";

            //_correo.Subject = string.Format("Importación de Archivo Buque {0} Número de Viaje {1}", d_buque, cn_voyage);
            _correo1.Subject = string.Format("PASO 1 de 4: Recepción CEPA: Listado de Importación por SUSTITUCIÓN de {0} para el Buque: {1}, # de Viaje {2}, Manifiesto de Aduana # {3}", HttpContext.Current.Session["c_navi_corto"].ToString(), d_buque.Text, cn_voyage, n_manifiesto);
            _correo1.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.ListaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga", DBComun.Estado.verdadero, HttpContext.Current.Session["c_naviera"].ToString());
            _correo1.Asunto = Html1;
            _correo1.EnviarCorreo(DBComun.TipoCorreo.CEPA, DBComun.Estado.verdadero);
            
        }


    }
}