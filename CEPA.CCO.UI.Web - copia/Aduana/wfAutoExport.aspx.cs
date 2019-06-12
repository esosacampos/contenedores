using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using System.Data.SqlClient;
using CEPA.CCO.Linq;
using System.Threading;
using System.Globalization;


namespace CEPA.CCO.UI.Web.Aduana
{
    public partial class wfAutoExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["IdReg"] == null)
                    {
                        throw new Exception("Falta código de cabecera");
                    }
                    else
                    {
                        List<DocBuque> _encaList = DocBuqueLINQ.ObtenerAduanaIdEx(Convert.ToInt32(Request.QueryString["IdReg"]));
                        //Sacar naviera
                        if (_encaList.Count > 0)
                        {
                            foreach (DocBuque item in _encaList)
                            {
                                c_imo.Text = item.c_imo;
                                d_buque.Text = item.d_buque;
                                f_llegada.Text = item.f_llegada.ToString("dd/MM/yyyy HH:mm:ss");
                                c_llegada.Text = item.c_llegada;
                                c_naviera.Text = item.d_cliente;
                                viaje.Text = item.c_voyage;
                            }

                            Cargar();


                        }
                        else
                        {
                            //btnCargar.Enabled = false;
                        }

                    }

                  

                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
                }
            }

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";


            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        private void Cargar()
        {
            GridView1.DataSource = DetaNavieraDAL.ObtenerDetalleEx(Convert.ToInt32(Request.QueryString["IdReg"]));
            GridView1.DataBind();       
        }

        protected void onRowCreate(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int colSpan = e.Row.Cells.Count;

                for (int i = (e.Row.Cells.Count - 1); i >= 1; i -= 1)
                {
                    e.Row.Cells.RemoveAt(i);
                    e.Row.Cells[0].ColumnSpan = colSpan;
                }

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul>"));
            }
        }

        protected void onPaging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Cargar();
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> pListaNo = new List<string>();
                List<int> pIdDeta = new List<int>();     
                int _resulDeta = 0;
                int PIdReg = 0;
                bool mEstado = false;
                bool mAuto = false;

                EnvioCorreo _correo = new EnvioCorreo();
               


                SeleccionManager.AutoExport((GridView)GridView1);
                List<DetaNaviera> _lit = new List<DetaNaviera>();

                _lit = HttpContext.Current.Session["Exportados"] as List<DetaNaviera>;

                if(_lit.Count > 0)
                {

                    foreach (var item in _lit)
                    {                        
                        PIdReg = item.IdReg;
                        break;
                    }

                    var _documentos = _lit.GroupBy(i => i.IdDoc).Select(group => group.First());

                    foreach (var item in _lit)
                    {                     

                        ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                        {
                            IdValid = -1,
                            n_contenedor = item.n_contenedor,                           
                            IdReg = item.IdReg,
                            IdDeta = item.IdDeta
                            
                        };

                        //Almacenar manifiesto devuelto por aduana
                        _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValidEx(validAduana, DBComun.Estado.verdadero));

                        if (_resulDeta == 2) 
                            pListaNo.Add(validAduana.n_contenedor);

                        pIdDeta.Add(validAduana.IdDeta);

                    }

                    //Verificar si ya fueron validados.
                   // int validR = Convert.ToInt32(DetaDocDAL.VerificarValidEx(PIdReg, DBComun.Estado.verdadero, pIdDoc));

                    List<EncaNaviera> pEnca = new List<EncaNaviera>();
                    //Cuando no posee validacion 
                    //if (validR >= 0)
                    //{
                    pEnca = EncaNavieraDAL.ObtenerNavierasValidEx(DBComun.Estado.verdadero, PIdReg);

                    EnvioValidacion(pListaNo, ref mEstado, ref mAuto, ref _correo, PIdReg, pEnca, d_buque.Text);

                    //}

                    if (mAuto == true)
                    {
                        DocBuque iPendiente = new DocBuque
                        {
                            IdReg = PIdReg,                            
                            c_llegada = c_llegada.Text,
                            f_llegada = Convert.ToDateTime(f_llegada.Text),
                            c_imo = c_imo.Text,
                            c_cliente = c_naviera.Text,
                            d_buque = d_buque.Text,
                            IdDeta = pIdDeta
                        };

                       GeneAuto(iPendiente);
                       mAuto = false;
                    }

                    int totalCount = 0;
                    int resuly = 0;

                    foreach (var item in _documentos)
                    {
                        totalCount = Convert.ToInt32(DetaDocDAL.CountDoc(DBComun.Estado.verdadero, item.IdDoc));

                        if(totalCount == 1)
                        {
                            resuly = Convert.ToInt32(DetaDocDAL.ActualizarValidacionEx(1, DBComun.Estado.verdadero, item.IdDoc));
                        }
                    }

                }
                else
                {
                    throw new Exception("No se poseen contenedores seleccionados a validar.");
                }

            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                HttpContext.Current.Session["Exportados"] = null;
            }
        }

        private static void EnvioValidacion(List<string> pListaNo, ref bool mEstado, ref bool mAuto, ref EnvioCorreo _correo, int IdReg, List<EncaNaviera> pEnca, string d_buque)
        {
            string Html;
            DateTime _fecha;

            List<ContenedoresAduana> pLista = new List<ContenedoresAduana>();
            List<ContenedoresAduana> pLista1 = new List<ContenedoresAduana>();
            List<ContenedoresAduana> pLista2 = new List<ContenedoresAduana>();
            List<ContenedoresAduana> pLista3 = new List<ContenedoresAduana>();


            if (pEnca.Count > 0)
            {
                foreach (EncaNaviera itemEnca in pEnca)
                {
                    pLista = new List<ContenedoresAduana>();
                    pLista1 = new List<ContenedoresAduana>();
                    pLista2 = new List<ContenedoresAduana>();
                    pLista3 = new List<ContenedoresAduana>();


                    //string _rinco = ResulNavieraDAL.EliminarInco(DBComun.Estado.falso, (int)iPendiente.IdReg, iPendiente.num_manif, itemEnca.c_naviera);

                    mEstado = false;
                    mAuto = false;                    

                    string c_navi_corto = EncaNavieraDAL.ObtenerNavi(DBComun.Estado.verdadero, itemEnca.c_naviera);

                    Html = "<dir style=\"font-family: 'Arial'; font-size: 11px; line-height: 1.2em\">";
                    Html += "<b><u> VALIDACIÓN DE ARCHIVO  </b></u><br />";
                    Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;\">";
                    Html += "<tr>";
                    _fecha = DetaNavieraLINQ.FechaBD(DBComun.Estado.verdadero);
                    Html += "<td style=\"text-align: left;\"><font size=2>Fecha/Hora&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp&nbsp;" + _fecha.ToString() + "</font></td>";
                    Html += "</tr>";
                    Html += "<tr>";
                    Html += "<td style=\"text-align: left;\" ><font size = 2>Usuario&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</font></td>";
                    Html += "<td style=\"text-align: left;\"><font size = 2>&nbsp;&nbsp;" + "servicio.contenedores" + "</font></td>";
                    Html += "</tr>";
                    Html += "</table>";
                    Html += "<br />";

                    pLista = ResulNavieraDAL.ObtenerAutorizadosADUANAEx(DBComun.Estado.verdadero, IdReg, itemEnca.c_naviera);


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

                    pLista1 = ResulNavieraDAL.ObtenerAutorizadosNOADUANAEx(DBComun.Estado.verdadero, IdReg, itemEnca.c_naviera);

                    Html += "<br />";
                    Html += string.Format("<b><u>PENDIENTES DE AUTORIZAR POR PATIO DE CONTENEDORES ({0})</b></u><br />", pLista1.Count);
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
                        //mEstado = true;


                        //IncoAduana _incoAduana = new IncoAduana
                        //{
                        //    IdReg = (int)iPendiente.IdReg,
                        //    c_naviera = itemEnca.c_naviera,
                        //    n_contenedor = item.n_contenedor,
                        //    n_manifiesto = iPendiente.num_manif,
                        //    a_mani = iPendiente.a_manifiesto
                        //};

                        //string _alInco = DetaNavieraDAL.AlmacenarInco(_incoAduana, DBComun.Estado.falso);


                    }

                    Html += "</table>";

                    /*pLista2 = ResulNavieraDAL.ObtenerAutorizadosNONAVIERAEx(DBComun.Estado.verdadero, IdReg, itemEnca.c_naviera);

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

                    Html += "</table>";*/



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


                    //pLista3 = ResulNavieraDAL.ObtenerRepetidosEx(DBComun.Estado.verdadero, IdReg);

                    //Html += "<br />";
                    //Html += string.Format("<b><u>REPETIDOS EN LISTADO ({0})</b></u><br />", pLista3.Count);
                    //Html += "<br />";

                    //Html += "<table style=\"font-family: 'Arial' ; font-size: 11px;  line-height: 1em;width: 100%;border: thin solid #4F81BD; border-collapse: separate; border-spacing:0px; \">";
                    //Html += "<tr>";
                    //Html += "<center>";
                    //Html += "<td width=\"10px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>No.</font></th>";
                    //Html += "<td width=\"40px\" height=\"25\" bgcolor=#1584CE style=\"font-weight:bold\"><font color=white size=2>CONTENEDOR</font></th>";
                    //Html += "</center>";
                    //Html += "</tr>";

                    //foreach (ContenedoresAduana item in pLista3)
                    //{
                    //    Html += "<tr>";
                    //    Html += "<td height=\"25\" style=\"border-right: thin solid #4F81BD\"><font size=2 color=blue>" + item.c_correlativo + "</font></td>";
                    //    Html += "<td height=\"25\"><font size=2 color=blue>" + item.n_contenedor + "</font></td>";
                    //    Html += "</tr>";
                    //    Html += "</font>";
                    //}

                    //Html += "</table>";

               

                    if (mEstado == true)
                    {
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=red>En espera de correcciones por la Naviera </font>";
                        _correo.Subject = string.Format("PASO 3 de 4: Validación PATIO: DENEGADA Listado de Exportación de {0} para el Buque: {1}, # de Viaje {2}", c_navi_corto, d_buque, itemEnca.c_voyage);                       
                        
                    }
                    else
                    {
                        Html += "<br /><br />";
                        Html += "<font style=\"color:#1F497D;\"><b> SIGUIENTE PASO: </b></font><br />";
                        Html += "<font color=blue>En espera autorización de PATIO DE CONTENEDORES </font>";
                        _correo.Subject = string.Format("PASO 3 de 4: Validación PATIO: ACEPTADA Listado de Exportación de {0} para el Buque: {1}, # de Viaje {2}", c_navi_corto,  d_buque, itemEnca.c_voyage);
                        mAuto = true;
                      
                    }

                    


                    _correo.ListaNoti = NotificacionesDAL.ObtenerNotificaciones("b_noti_carga_exp", DBComun.Estado.verdadero, "219");

                    List<Notificaciones> _listaCC = NotificacionesDAL.ObtenerNotificacionesCC("b_noti_carga_exp", DBComun.Estado.verdadero, itemEnca.c_naviera);

                    if (_listaCC == null)
                        _listaCC = new List<Notificaciones>();

                    _listaCC.AddRange(NotificacionesDAL.ObtenerNotificacionesCCN("b_noti_carga_exp", DBComun.Estado.verdadero, itemEnca.c_naviera));
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


            }
        }


        private static void GeneAuto(DocBuque iPendiente)
        {
            int valores = 0;
            int val = 0;
            //int cantidad = cant;
            int Correlativo = 0;
            int cont = 0;
            NotiExport _notiAutorizados = new NotiExport();
            CargarArchivosLINQ _cargar = new CargarArchivosLINQ();
            int _resultado = 0;
            List<DetaNaviera> pLista = new List<DetaNaviera>();


            foreach (var item in iPendiente.IdDeta)
            {
                DetaNaviera _detaNvi = new DetaNaviera
                {
                    IdReg = (int)iPendiente.IdReg,
                    IdDeta = item
                };

                pLista.Add(_detaNvi);                
            }

            _resultado = Convert.ToInt32(DetaNavieraDAL.ActualizarDetaEx(DBComun.Estado.verdadero, pLista));

            if (_resultado > 0)
            {
                valores = _resultado;
                cont = cont + 1;
            }

            if (valores > 0)
            {

                List<DetaNaviera> _listAutorizados = DetaNavieraDAL.ObtenerAutorizadosEx((int)iPendiente.IdReg, DBComun.Estado.verdadero);
                Correlativo = Convert.ToInt32(DetaNavieraDAL.ObtenerCorrelativoEx(iPendiente.c_imo, iPendiente.c_llegada, iPendiente.c_cliente, DBComun.Estado.verdadero)) + 1;


                var _destinos = _listAutorizados.GroupBy(i => i.c_detalle_pais).Select(group => group.First());

                List<DetaNaviera> _list = new List<DetaNaviera>();

                if (_destinos.Count() > 0)
                {
                    foreach (var item in _destinos)
                    {

                        var _trafico = _listAutorizados.Where(c => c.c_detalle_pais.Equals(item.c_detalle_pais)).ToList();

                        for (int d = 1; d <= 2; d++)
                        {
                            List<DetaNaviera> _listaAOrdenar = new List<DetaNaviera>();

                            if(d == 1)
                                _listaAOrdenar = _trafico.Where(f => f.b_emb_dir.Equals('Y')).ToList();
                            else if(d ==2)
                                _listaAOrdenar = _trafico.Where(f => f.b_emb_dir == "").ToList();
                            

                            for (int i = 1; i < 3; i++)
                            {

                                var consulta = DetaNavieraLINQ.AlmacenarArchivoEx(_listaAOrdenar, DBComun.Estado.verdadero, i);

                                if (consulta.Count > 0)
                                    _list.AddRange(consulta);

                                foreach (var item_C in consulta)
                                {
                                    _listaAOrdenar.RemoveAll(rt => rt.n_contenedor.ToUpper() == item_C.n_contenedor.ToUpper());
                                }
                            }
                        }

                    }
                }


                if (_list.Count > 0)
                {
                    foreach (DetaNaviera deta in _list)
                    {
                        int _actCorre = Convert.ToInt32(DetaNavieraDAL.ActualizarCorrelativoEx(DBComun.Estado.verdadero, Correlativo, deta.IdDeta));
                        Correlativo = Correlativo + 1;
                    }
                }

                int _rango = ((Correlativo - 1) - valores) + 1;
                List<ArchivoExport> _listaOrdenada = DetaNavieraDAL.ObtenerResultadoEx((int)iPendiente.IdReg, _rango, Correlativo - 1, DBComun.Estado.verdadero);

               

                //_notiAutorizados.IdDoc = iPendiente.IdDoc;
                //_notiAutorizados.GenerarAplicacionCX(_listaOrdenada, iPendiente.c_cliente, iPendiente.d_cliente, iPendiente.c_llegada, (int)iPendiente.IdReg,
                // iPendiente.d_buque, iPendiente.f_llegada, valores, valores, null);
                
            }
        }
    }
}