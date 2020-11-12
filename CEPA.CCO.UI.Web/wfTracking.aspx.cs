using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using System.Drawing;

using iTextSharp.text;
using iTextSharp.text.pdf;



using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
using HtmlAgilityPack;
using System.Xml;
using Newtonsoft.Json;
using System.Data;
using System.Web.Configuration;

namespace CEPA.CCO.UI.Web
{
    public partial class wfTracking : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["c_naviera"].ToString() == "289")
                    btnConsultar.Visible = true;                    
                else
                    btnConsultar.Visible = false;
            }

        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            //Response.Redirect("wfConsulDecla.aspx");
            //string vtn = "window.open('wfConsulDecla.aspx','Dates','scrollbars=yes,resizable=yes','height=300', 'width=300')";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", vtn, true);
            try
            {
                if (txtBuscar.Text.Length == 11)
                {
                    string valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, txtBuscar.Text.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                    if (valida == "VALIDO")
                    {
                        Session["n_conte"] = txtBuscar.Text;
                        //Response.Write("<script>window.open('wfConsulDecla.aspx','popup','width=800,height=500');</script>");
                        ScriptManager.RegisterStartupScript(this, typeof(string), "", "window.open('wfConsulDecla.aspx','popup','width=800,height=500');", true);
                    }
                }
                else
                {
                    throw new Exception("Debe indicar el número de contenedor a consultar");
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
            finally
            {
                //Session["n_conte"] = "";
            }

        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<TrackingEnca> pLista = new List<TrackingEnca>();
            List<TrackingEnca> pLista1 = new List<TrackingEnca>();
            try
            {

                ScriptManager.RegisterClientScriptBlock(btnBuscar, typeof(Button), "testscript1", "alertError();", true);
                //System.Threading.Thread.Sleep(1000);

                if (radio3.Checked == false)
                {
                    if (txtBuscar.Text.Length == 11)
                    {
                        string valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, txtBuscar.Text.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                        if (valida == "VALIDO")
                        {

                            pLista = DocBuqueLINQ.ObtenerTracking(txtBuscar.Text, Session["c_naviera"].ToString());

                            if (pLista.Count > 0)
                            {
                                grvTracking.DataSource = pLista;
                                grvTracking.DataBind();

                            }
                            else
                            {
                                grvTracking.DataSource = null;
                                grvTracking.DataBind();
                                Label lblEmptyMessage = grvTracking.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                lblEmptyMessage.Text = "No se poseen registros de este contenedor: " + txtBuscar.Text;
                                throw new Exception(lblEmptyMessage.Text);
                            }

                        }
                        else
                        {

                            throw new Exception("Número de contenedor no válido: " + txtBuscar.Text);
                        }

                    }
                    else
                    {
                        grvTracking.DataSource = null;
                        grvTracking.DataBind();
                        throw new Exception("Este número no posee los 11 caracteres " + txtBuscar.Text);
                    }
                }
                else
                {
                    pLista = DocBuqueLINQ.ObtenerTracking(txtBuscar.Text, Session["c_naviera"].ToString());



                    if (pLista.Count > 0)
                    {
                        grvTracking.DataSource = pLista;
                        grvTracking.DataBind();

                    }
                    else
                    {
                        grvTracking.DataSource = null;
                        grvTracking.DataBind();
                        Label lblEmptyMessage = grvTracking.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                        lblEmptyMessage.Text = "No se poseen registros de este contenedor: " + txtBuscar.Text;
                        throw new Exception(lblEmptyMessage.Text);
                    }
                }

                grvTracking.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[9].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[10].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[11].Attributes["data-hide"] = "phone";
                grvTracking.HeaderRow.Cells[12].Attributes["data-hide"] = "phone";


                //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";


                grvTracking.HeaderRow.TableSection = TableRowSection.TableHeader;

                grvTracking.FooterRow.Cells[0].Attributes["text-align"] = "center";
                grvTracking.FooterRow.TableSection = TableRowSection.TableFooter;


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }

        public static string getUbica(string c_contenedor, string c_llegada, string c_naviera)
        {
            string _contenedores = "";
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "Sqlzonas", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "llegada", valor = c_llegada });
            proceso.Parametros.Add(new Parametros { nombre = "_contenedor", valor = c_contenedor });
            proceso.Parametros.Add(new Parametros { nombre = "navi", valor = c_naviera });

            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static string Conectar(string _contenedores, string apiUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file = string.Empty;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            //string idx = "{ "DBase":"CONTENEDORES","Servidor":null,"Procedimiento":"Sqlentllenos","Consulta":true,"Parametros":[{"nombre":"_dia","valor":"15-05-2019"}]}";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
                DataTable tabla = JsonConvert.DeserializeObject<DataTable>(file) as DataTable;
                if (tabla.Rows.Count > 0)
                {
                    if (!tabla.Rows[0][0].ToString().StartsWith("ERROR"))
                    {
                        _contenedores = tabla.Rows[0][0].ToString();
                    }
                }
            }
            return _contenedores;
        }

        public static List<ProvisionalesDeta> getDetaProvi(string c_contenedor, string c_llegada, string c_naviera)
        {
            List<ProvisionalesDeta> _contenedores = new List<ProvisionalesDeta>();
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "Sqlvprovi", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "llegada", valor = c_llegada });
            proceso.Parametros.Add(new Parametros { nombre = "_contenedor", valor = c_contenedor });
            proceso.Parametros.Add(new Parametros { nombre = "navi", valor = c_naviera });

            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = conDetaProvi(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<ProvisionalesDeta> conDetaProvi(List<ProvisionalesDeta> _contenedores, string apiUrl)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json; charset=utf-8";
            string file = string.Empty;
            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            //string idx = "{ "DBase":"CONTENEDORES","Servidor":null,"Procedimiento":"Sqlentllenos","Consulta":true,"Parametros":[{"nombre":"_dia","valor":"15-05-2019"}]}";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                file = sr.ReadToEnd();
                DataTable tabla = JsonConvert.DeserializeObject<DataTable>(file) as DataTable;
                if (tabla.Rows.Count > 0)
                {
                    if (!tabla.Rows[0][0].ToString().StartsWith("ERROR"))
                    {
                        _contenedores = tabla.ToList<ProvisionalesDeta>();
                    }
                }
            }
            return _contenedores;
        }

        protected void grvTracking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;


            if (ArchivoBookingDAL.isFecha(e.Row.Cells[14].Text) == true)
            {
                if (Convert.ToDateTime(e.Row.Cells[14].Text) > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[14].Text = "";
                }

            }

            if (ArchivoBookingDAL.isFecha(e.Row.Cells[13].Text) == true)
            {
                if (Convert.ToDateTime(e.Row.Cells[13].Text) > FIRST_GOOD_DATE)
                { }
                else
                {
                    e.Row.Cells[13].Text = "";
                }

            }

            e.Row.Cells[0].Attributes.Add("style", "word-break:break-word;word-wrap:break-word;");

            TrackingEnca order = (TrackingEnca)e.Row.DataItem;

            DetailsView gvDetails = new DetailsView();
            gvDetails = (DetailsView)e.Row.FindControl("dtTracking");

            gvDetails.DataSource = order.TrackingList;
            gvDetails.DataBind();

            HiddenField hEstado = (HiddenField)e.Row.FindControl("hEstado") as HiddenField;

            if (hEstado.Value == "CANCELADO")
            {
                e.Row.BackColor = Color.FromName("#EB7A7A");
                e.Row.ForeColor = Color.White;
            }

            //ScriptManager.RegisterStartupScript(this, typeof(string), "", "almacenando();", true);
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "testscript1", "almacenando();", true);

            foreach (DetailsViewRow item in gvDetails.Rows)
            {
                item.Cells[0].Attributes.Add("style", "width:23%;");
                if (ArchivoBookingDAL.isFecha(item.Cells[1].Text) == true)
                {
                    if (Convert.ToDateTime(item.Cells[1].Text) > FIRST_GOOD_DATE)
                    { }
                    else
                    {
                        item.Cells[1].Text = "";
                    }

                }

                Label lblUbica = (Label)gvDetails.FindControl("lblUbica") as Label;

                if (lblUbica.Text == "")
                    lblUbica.Text = getUbica(order.n_contenedor, order.c_llegada, order.c_naviera);

            }


            GridView gvProvisionales = (GridView)gvDetails.Rows[26].Cells[1].FindControl("grvProv");

            if (gvProvisionales != null)
            {
                List<ProvisionalesEnca> pList = new List<ProvisionalesEnca>();
                pList = DetaNavieraDAL.getEncaProvi(order.IdDeta);

                if (pList.Count > 0)
                {
                    gvProvisionales.DataSource = pList;
                    gvProvisionales.DataBind();

                    gvProvisionales.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                    // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                    gvProvisionales.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";


                    List<ProvisionalesDeta> pListD = new List<ProvisionalesDeta>();
                    pListD = DetaNavieraDAL.getDetaProvi(order.IdDeta);

                    if (pListD.Count > 0)
                    {
                        if (gvProvisionales.Rows.Count > 0)
                        {
                            GridView gvDetaProvi = (GridView)gvProvisionales.Rows[0].Cells[3].FindControl("grvDetailProvi");

                            if (gvDetaProvi != null)
                            {
                                gvDetaProvi.DataSource = pListD;
                                gvDetaProvi.DataBind();

                                gvDetaProvi.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                                // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                                gvDetaProvi.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                                gvDetaProvi.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                                gvDetaProvi.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
                                gvDetaProvi.HeaderRow.Cells[7].Attributes["data-hide"] = "phone";
                                if (pListD.Count > 0)
                                {
                                    DateTime fec_prv;
                                    DateTime fec_reserva;
                                    DateTime fec_valida;
                                    for (int i = 0; i < gvDetaProvi.Rows.Count; i++)
                                    {

                                        if (ArchivoBookingDAL.isFecha(gvDetaProvi.Rows[i].Cells[0].Text) == true)
                                        {
                                            if (gvDetaProvi.Rows[i].Cells[0].Text == "01/01/1900 00:00:00")
                                                fec_prv = DateTime.MinValue;
                                            else
                                                fec_prv = Convert.ToDateTime(gvDetaProvi.Rows[i].Cells[0].Text);

                                            if (fec_prv == DateTime.MinValue)
                                            {
                                                gvDetaProvi.Rows[i].Cells[0].Text = "";
                                            }
                                            else
                                            {

                                            }
                                        }

                                        if (ArchivoBookingDAL.isFecha(gvDetaProvi.Rows[i].Cells[6].Text) == true)
                                        {
                                            if (gvDetaProvi.Rows[i].Cells[6].Text == "01/01/1900 00:00:00")
                                                fec_reserva = DateTime.MinValue;
                                            else
                                                fec_reserva = Convert.ToDateTime(gvDetaProvi.Rows[i].Cells[6].Text);

                                            if (fec_reserva == DateTime.MinValue)
                                            {
                                                gvDetaProvi.Rows[i].Cells[6].Text = "";
                                            }
                                            else
                                            {

                                            }
                                        }

                                        if (ArchivoBookingDAL.isFecha(gvDetaProvi.Rows[i].Cells[7].Text) == true)
                                        {
                                            if (gvDetaProvi.Rows[i].Cells[7].Text == "01/01/1900 00:00:00")
                                                fec_valida = DateTime.MinValue;
                                            else
                                                fec_valida = Convert.ToDateTime(gvDetaProvi.Rows[i].Cells[7].Text);

                                            if (fec_valida == DateTime.MinValue)
                                            {
                                                gvDetaProvi.Rows[i].Cells[7].Text = "";
                                            }
                                            else
                                            {

                                            }

                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                //}
            }


            //e.Row.Cells[13].Visible = false;
            //e.Row.Cells[14].Visible = false;
            //grvTracking.HeaderRow.Cells[13].Visible = false;
            //grvTracking.HeaderRow.Cells[14].Visible = false;
        }

        public static string ObtenerADUANA(string n_contenedor, string a_mani, string n_mani)
        {
            try
            {
                string _resultado = null;
                string _cadena = null;
                string _mani = null;

                MemoryStream memoryStream = new MemoryStream();
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Encoding = new UTF8Encoding(false);
                xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                xmlWriterSettings.Indent = true;

                XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                int b_sidunea = Convert.ToInt32(DetaNavieraDAL.obt_Sidunea(DBComun.Estado.verdadero, Convert.ToInt32(n_mani), Convert.ToInt32(a_mani), n_contenedor));


                string _Aduana = null;

                xml.WriteStartDocument();

                xml.WriteStartElement("MDS4");

                xml.WriteElementString("CAR_REG_YEAR", a_mani);
                xml.WriteElementString("KEY_CUO", "02");
                xml.WriteElementString("CAR_REG_NBER", n_mani);
                xml.WriteElementString("CAR_CTN_IDENT", n_contenedor.Trim().TrimEnd().TrimStart());



                xml.WriteEndDocument();
                xml.Flush();
                xml.Close();

                //Generar XML para enviar parametros al servicio.
                _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                XmlDocument doc = new XmlDocument();

                if (b_sidunea == 0)
                {
                    //CEPAService.WSManifiestoCEPAClient _proxy = new CEPAService.WSManifiestoCEPAClient();

                    _resultado = "0";
                }
                else
                {
                    CepaSW.WSManifiestoCEPAClient _proxySW = new CepaSW.WSManifiestoCEPAClient();

                    string _usuario = ConfigurationManager.AppSettings["userSidunea"];
                    string _pass = ConfigurationManager.AppSettings["pswSidunea"];

                    _proxySW.ClientCredentials.UserName.UserName = _usuario;
                    _proxySW.ClientCredentials.UserName.Password = _pass;

                    _resultado = _proxySW.getDocumentoInfoDocumento(_Aduana);
                }

                if (_resultado.Contains("encuentran"))
                {
                    _mani = "DENEGADO";
                }
                else
                {
                    if (b_sidunea == 0)
                    {
                        _mani = "NO SE ENCONTRARON DECLARACIONES ASOCIADAS";
                    }
                    else
                    {
                        if (_resultado.Substring(0, 1) == "1")
                        {
                            string _cadenaM = _resultado.Remove(0, 2);
                            doc.LoadXml(_cadenaM);
                            XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4S/MDS4");

                            if (listaCntres.Count > 0)
                            {

                                XmlNode _manifiestos;

                                for (int i = 0; i < listaCntres.Count; i++)
                                {
                                    _manifiestos = listaCntres.Item(i);

                                    _cadena = _manifiestos.SelectSingleNode("DESP_CEPA").InnerText;

                                    if (_cadena.Trim().TrimEnd().TrimStart() == "1")
                                        _mani = "AUTORIZADO";
                                    else
                                        _mani = "DENEGADO";
                                }
                            }
                            else
                            {
                                _mani = "DENEGADO";
                            }
                        }
                        else
                        {
                            _mani = "NO SE ENCONTRARON DECLARACIONES ASOCIADAS";
                        }
                    }
                }
                return _mani;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [System.Web.Services.WebMethod]
        public static string ValidacionTarja(string c_tarja, string n_contenedor, string c_llegada, string f_tarja, string n_manifiesto, double v_peso)
        {
            v_peso = 0.00;
            try
            {
                //Validar que el contenedor sea cancelado y enviar mensaje.

                List<Tarjas> encaTarjas = new List<Tarjas>();

                encaTarjas = EncaBuqueDAL.TarjasLlegada(c_llegada, n_contenedor).ToList();

                List<Pago> pLista = new List<Pago>();

                foreach (var listTarjas in encaTarjas)
                {
                    List<Pago> _lstPagos = PagoDAL.ConsultarPago(listTarjas.c_tarja, n_contenedor);
                    v_peso = v_peso + EncaBuqueDAL.TarjasPeso(listTarjas.c_tarja);

                    if (_lstPagos == null)
                        _lstPagos = new List<Pago>();

                    if (_lstPagos.Count > 0)
                    {
                        pLista.AddRange(_lstPagos);
                    }
                }


                string _Transfer = "Si", _Despacho = "Si", _Manejo = "Si", _Almacenaje = "Si", _Validacion = "Si";


                foreach (var iEvaPagos in pLista)
                {
                    if (iEvaPagos.ValTransfer == "No")
                        _Transfer = "No";
                    if (iEvaPagos.ValDespacho == "No")
                        _Despacho = "No";
                    if (iEvaPagos.ValManejo == "No")
                        _Manejo = "No";
                    if (iEvaPagos.ValAlmacenaje == "No")
                        _Almacenaje = "No";
                    if (iEvaPagos.validacion == "No")
                        _Validacion = "No";
                }

                Pago _clsPago = new Pago()
                {
                    n_contenedor = n_contenedor,
                    ValTransfer = _Transfer,
                    ValDespacho = _Despacho,
                    ValManejo = _Manejo,
                    ValAlmacenaje = _Almacenaje,
                    validacion = _Validacion
                };

                pLista = new List<Pago>();
                pLista.Add(_clsPago);

                /* Verificar Fecha de Tarja */

                DateTime f_tar = Convert.ToDateTime(f_tarja);

                /* Consultando Retencion PNC - DAN */

                var lstReten = (from b in PagoDAL.ConsultaDAN(c_llegada, n_contenedor, DBComun.TipoBD.SqlServer)
                                select new
                                {
                                    b_dan = (b.b_dan == null ? string.Empty : b.b_dan),
                                    tipo = (b.tipoReten == null ? string.Empty : b.tipoReten)
                                }).ToList();

                int tipo = 0;
                string cadenaReten = null, retenResult = null, tiReten = null;

                foreach (var iReten in lstReten)
                {
                    if (iReten.b_dan != "LIBRE")
                    {
                        cadenaReten = iReten.b_dan + "/" + cadenaReten;
                        tiReten = iReten.tipo;
                    }
                    else
                        tipo += 1;
                }

                if (tipo >= 3)
                    retenResult = "LIBRE";
                else
                {
                    retenResult = cadenaReten.Remove(cadenaReten.Length - 1);
                    tiReten = tiReten == null ? string.Empty : tiReten;
                }

                /* Validar Pagos Por Tarjas */

                List<Pago> pLstDetalle = new List<Pago>();

                string detaReten = null;
                if (tiReten == "DAN")
                    detaReten = "PNC - DAN";
                else if (tiReten == "UCC")
                    detaReten = "UCC";
                else if (tiReten == "DGA")
                    detaReten = "ADUANA";
                else
                    detaReten = "";



                pLstDetalle = DetaNavieraDAL.CalcPagos(n_contenedor, c_llegada, f_tar, DBComun.TipoBD.SqlServer, v_peso);



                string[] cadenaADUANA = n_manifiesto.Split('-');

                string n_year = cadenaADUANA[0].ToString();
                string n_mani = cadenaADUANA[1].ToString();

                //string valorADUANA = ObtenerADUANA(n_contenedor, n_year, n_mani);

                string valorADUANA = "DENEGADO";

                var query = (dynamic)null;

                List<Pago> pResult = new List<Pago>();

                if (pLista.Count > 0 && pLstDetalle.Count > 0)
                {
                    query = from a in pLista
                            join b in pLstDetalle on a.n_contenedor equals b.n_contenedor
                            select new
                            {
                                validacion = a.validacion == "Si" ? "Si, El contenedor esta libre de pagos con CEPA" : "No, El contenedor tiene pagos pendientes con CEPA",
                                b_danc = retenResult == "LIBRE" ? "Si, El contenedor esta libre de revisiones con PNC - DAN y UCC" : ("No, El contenedor tiene revisión pendiente con X desde ").Replace("X", detaReten) + retenResult,
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : valorADUANA == "DENEGADO" ? "No, El contenedor no se encuentra autorizado por ADUANA para su retiro" : valorADUANA,



                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_dan = retenResult == "LIBRE" ? "#18D318" : "#FF0000",
                                style_aduana = valorADUANA == "AUTORIZADO" ? "#18D318" : "#FF0000",

                                style_naviero_transfer = b.descripcion == "Transferencia" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_transfer = b.descripcion == "Transferencia" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_manejo = b.descripcion == "Manejo" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_manejo = b.descripcion == "Manejo" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_alma = b.descripcion == "Almacenaje" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_alma = b.descripcion == "Almacenaje" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_despacho = b.descripcion == "Despacho" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_despacho = b.descripcion == "Despacho" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                /*p_tranfer =  b.descripcion == "Transferencia" ? b.s_pagos : 0.00,
                                p_manejo =  b.descripcion == "Manejo" ? b.s_pagos : 0.00,
                                p_alma = b.descripcion == "Almacenaje" ? b.s_pagos : 0.00,
                                p_despacho = b.descripcion == "Despacho" ? b.s_pagos : 0.00,*/
                                descripcion = b.descripcion,
                                f_tarja = b.f_tarja,
                                f_salida = b.f_salida,
                                b_salida = b.b_salida,
                                v_dias = b.v_dias,
                                v_peso = b.v_peso,
                                b_transito = b.b_transito == "N" ? "PATIO CEPA" : "RETIRO DIRECTO",
                                v_teus = b.v_teus,
                                c_tamaño = b.c_tamaño
                            };
                }
                else
                {
                    Pago _clPago = new Pago
                    {
                        n_contenedor = n_contenedor,
                        b_tarifa = "DESCONOCIDA",
                        validacion = "No",
                        ValTransfer = "No",
                        ValDespacho = "No",
                        ValManejo = "No",
                        ValAlmacenaje = "No"

                    };

                    pLista.Add(_clPago);

                    query = from a in pLista
                            join b in pLstDetalle on a.n_contenedor equals b.n_contenedor
                            select new
                            {

                                validacion = a.validacion == "Si" ? "Si, El contenedor esta libre de pagos con CEPA" : "No, El contenedor tiene pagos pendientes con CEPA",

                                b_danc = retenResult == "LIBRE" ? "Si, El contenedor esta libre de revisiones con PNC - DAN y UCC" : ("No, El contenedor tiene revisión pendiente con X desde ").Replace("X", detaReten) + retenResult,
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : valorADUANA == "DENEGADO" ? "No, El contenedor no se encuentra autorizado por ADUANA para su retiro" : valorADUANA,

                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_dan = retenResult == "LIBRE" ? "#18D318" : "#FF0000",
                                style_aduana = valorADUANA == "AUTORIZADO" ? "#18D318" : "#FF0000",


                                style_naviero_transfer = b.descripcion == "Transferencia" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_transfer = b.descripcion == "Transferencia" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_manejo = b.descripcion == "Manejo" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_manejo = b.descripcion == "Manejo" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_alma = b.descripcion == "Almacenaje" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_alma = b.descripcion == "Almacenaje" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_despacho = b.descripcion == "Despacho" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_despacho = b.descripcion == "Despacho" && b.s_cliente == "Y" ? b.s_pagos : 0.00,



                                /*p_tranfer =  b.descripcion == "Transferencia" ? b.s_pagos : 0.00,
                                p_manejo =  b.descripcion == "Manejo" ? b.s_pagos : 0.00,
                                p_alma = b.descripcion == "Almacenaje" ? b.s_pagos : 0.00,
                                p_despacho = b.descripcion == "Despacho" ? b.s_pagos : 0.00,*/
                                descripcion = b.descripcion,
                                f_tarja = b.f_tarja,
                                f_salida = b.f_salida,
                                b_salida = b.b_salida,
                                v_dias = b.v_dias,
                                v_peso = b.v_peso,
                                b_transito = b.b_transito == "N" ? "PATIO CEPA" : "RETIRO DIRECTO",
                                v_teus = b.v_teus,
                                c_tamaño = b.c_tamaño
                            };
                }

                //var c = (Pago)query.ToList();

                string jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

                pResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pago>>(jConsult);




                var consul = (from d in pResult
                              group d by new
                              {
                                  validacion = d.validacion,
                                  b_danc = d.b_danc,
                                  style_va = d.style_va,
                                  style_transfer = d.style_transfer,
                                  style_despac = d.style_despac,
                                  style_manejo = d.style_manejo,
                                  style_alma = d.style_alma,
                                  style_dan = d.style_dan,
                                  style_naviero_transfer = Convert.ToDouble(d.style_naviero_transfer),
                                  style_cliente_transfer = Convert.ToDouble(d.style_cliente_transfer),
                                  style_naviero_manejo = Convert.ToDouble(d.style_naviero_manejo),
                                  style_cliente_manejo = Convert.ToDouble(d.style_cliente_manejo),
                                  style_naviero_alma = Convert.ToDouble(d.style_naviero_alma),
                                  style_cliente_alma = Convert.ToDouble(d.style_cliente_alma),
                                  style_naviero_despacho = Convert.ToDouble(d.style_naviero_despacho),
                                  style_cliente_despacho = Convert.ToDouble(d.style_cliente_despacho),
                                  descripcion = d.descripcion,
                                  style_aduana = d.style_aduana,
                                  b_aduana = d.b_aduana,
                                  f_salida = d.f_salida,
                                  f_tarja = d.f_tarja,
                                  b_salida = d.b_salida,
                                  v_dias = d.v_dias,
                                  v_peso = d.v_peso,
                                  b_transito = d.b_transito,
                                  v_teus = d.v_teus,
                                  c_tamaño = d.c_tamaño

                              } into g
                              select new
                              {

                                  validacion = g.Key.validacion,
                                  b_danc = g.Key.b_danc,

                                  style_va = g.Key.style_va,
                                  style_dan = g.Key.style_dan,

                                  style_descripcion = g.Key.descripcion == "Transferencia" ? g.Key.style_transfer : g.Key.descripcion == "Despacho" ? g.Key.style_despac : g.Key.descripcion == "Manejo" ? g.Key.style_manejo : g.Key.descripcion == "Almacenaje" ? g.Key.style_alma : "",

                                  style_naviero = g.Key.descripcion == "Transferencia" ? g.Key.style_naviero_transfer : g.Key.descripcion == "Despacho" ? g.Key.style_naviero_despacho : g.Key.descripcion == "Manejo" ? g.Key.style_naviero_manejo : g.Key.descripcion == "Almacenaje" ? g.Key.style_naviero_alma : 0.00,
                                  style_cliente = g.Key.descripcion == "Transferencia" ? g.Key.style_cliente_transfer : g.Key.descripcion == "Despacho" ? g.Key.style_cliente_despacho : g.Key.descripcion == "Manejo" ? g.Key.style_cliente_manejo : g.Key.descripcion == "Almacenaje" ? g.Key.style_cliente_alma : 0.00,

                                  style_pendiente = g.Key.descripcion == "Transferencia" ?
                                                           !g.Key.style_transfer.Contains("correcto") ?
                                                                g.Key.style_naviero_transfer > 0.00 ? g.Key.style_naviero_transfer : g.Key.style_cliente_transfer
                                                           : 0.00 : g.Key.descripcion == "Despacho" ? !g.Key.style_despac.Contains("correcto") ? g.Key.style_naviero_despacho > 0.00 ? g.Key.style_naviero_despacho : g.Key.style_cliente_despacho : 0.00 : g.Key.descripcion == "Manejo" ? !g.Key.style_manejo.Contains("correcto") ? g.Key.style_naviero_manejo > 0.00 ? g.Key.style_naviero_manejo : g.Key.style_cliente_manejo : 0.00 : g.Key.descripcion == "Almacenaje" ? !g.Key.style_alma.Contains("correcto") ? g.Key.style_naviero_alma > 0.00 ? g.Key.style_naviero_alma : g.Key.style_cliente_alma : 0.00 : 0.00,

                                  descripcion = g.Key.descripcion,

                                  b_aduana = g.Key.b_aduana,
                                  style_aduana = g.Key.style_aduana,
                                  f_tarja = g.Key.f_tarja,
                                  f_salida = g.Key.f_salida,

                                  //p_tranfer = g.Key.descripcion == "Transferencia" ? g.Sum(x => x.p_tranfer) : g.Key.descripcion == "Despacho" ? g.Sum(x => x.p_despacho) : g.Key.descripcion == "Manejo" ? g.Sum(x => x.p_manejo) : g.Key.descripcion == "Almacenaje" ? g.Sum(x => x.p_alma) : 0.0
                                  b_salida = g.Key.b_salida,
                                  detalle = g.Key.descripcion == "Almacenaje" ? string.Format("5 Días libres; {0} Días a cobrar X {1} tm X tarifa", g.Key.v_dias, g.Key.v_peso) : g.Key.descripcion == "Transferencia" ? string.Format("Ubicación({0}) X tarifa", g.Key.b_transito) : g.Key.descripcion == "Manejo" ? string.Format("{0} equivalente en TEUS ({1}) X tarifa ", g.Key.c_tamaño, g.Key.v_teus) : g.Key.descripcion == "Despacho" ? "Contenedor x Tarifa" : ""

                              }).ToList();

                string _respuesta = Newtonsoft.Json.JsonConvert.SerializeObject(consul);

                return _respuesta;
            }
            catch (Exception ex)
            {
                string _mensaje = "Error: Tiempo de espera agotado, intentelo de nuevo, presione la tecla F5: " + ex.Message;

                return Newtonsoft.Json.JsonConvert.SerializeObject(_mensaje);
            }

        }

        [System.Web.Services.WebMethod]
        public static string ValidacionTarjaRe(string c_tarja, string n_contenedor, string c_llegada, string f_tarja, string n_manifiesto, string f_retiro, double v_peso)
        {
            v_peso = 0.00;
            try
            {

                //Validar que el contenedor sea cancelado y enviar mensaje.



                List<Tarjas> encaTarjas = new List<Tarjas>();

                encaTarjas = EncaBuqueDAL.TarjasLlegada(c_llegada, n_contenedor).ToList();

                List<Pago> pLista = new List<Pago>();

                foreach (var listTarjas in encaTarjas)
                {
                    List<Pago> _lstPagos = PagoDAL.ConsultarPago(listTarjas.c_tarja, n_contenedor);

                    v_peso = v_peso + EncaBuqueDAL.TarjasPeso(listTarjas.c_tarja);


                    if (_lstPagos == null)
                        _lstPagos = new List<Pago>();

                    if (_lstPagos.Count > 0)
                    {
                        pLista.AddRange(_lstPagos);
                    }
                }

                string _Transfer = "Si", _Despacho = "Si", _Manejo = "Si", _Almacenaje = "Si", _Validacion = "Si";



                foreach (var iEvaPagos in pLista)
                {

                    if (iEvaPagos.ValTransfer == "No")
                        _Transfer = "No";

                    if (iEvaPagos.ValDespacho == "No")
                        _Despacho = "No";

                    if (iEvaPagos.ValManejo == "No")
                        _Manejo = "No";

                    if (iEvaPagos.ValAlmacenaje == "No")
                        _Almacenaje = "No";

                    if (iEvaPagos.validacion == "No")
                        _Validacion = "No";


                }

                Pago _clsPago = new Pago()
                {
                    n_contenedor = n_contenedor,
                    ValTransfer = _Transfer,
                    ValDespacho = _Despacho,
                    ValManejo = _Manejo,
                    ValAlmacenaje = _Almacenaje,
                    validacion = _Validacion
                };

                pLista = new List<Pago>();
                pLista.Add(_clsPago);

                /* Verificar Fecha de Tarja */

                DateTime f_tar = Convert.ToDateTime(f_tarja);
                DateTime f_re = Convert.ToDateTime(f_retiro);

                /* Consultando Retencion PNC - DAN */

                var lstReten = (from b in PagoDAL.ConsultaDAN(c_llegada, n_contenedor, DBComun.TipoBD.SqlServer)
                                select new
                                {
                                    b_dan = (b.b_dan == null ? string.Empty : b.b_dan),
                                    tipo = (b.tipoReten == null ? string.Empty : b.tipoReten)
                                }).ToList();

                int tipo = 0;
                string cadenaReten = null, retenResult = null, tiReten = null;

                foreach (var iReten in lstReten)
                {
                    if (iReten.b_dan != "LIBRE")
                    {
                        cadenaReten = iReten.b_dan + "/" + cadenaReten;
                        tiReten = iReten.tipo;
                    }
                    else
                        tipo += 1;
                }

                if (tipo >= 3)
                    retenResult = "LIBRE";
                else
                {
                    retenResult = cadenaReten.Remove(cadenaReten.Length - 1);
                    tiReten = tiReten == null ? string.Empty : tiReten;
                }

                /* Validar Pagos Por Tarjas */

                List<Pago> pLstDetalle = new List<Pago>();

                string detaReten = null;
                if (tiReten == "DAN")
                    detaReten = "PNC - DAN";
                else if (tiReten == "UCC")
                    detaReten = "UCC";
                else if (tiReten == "DGA")
                    detaReten = "ADUANA";
                else
                    detaReten = "";


                pLstDetalle = DetaNavieraDAL.CalcPagos(n_contenedor, c_llegada, f_tar, f_re, DBComun.TipoBD.SqlServer, v_peso);

               
                TimeSpan t = f_re.Date - f_tar.Date;
                int NrOfDays = t.Days + 1;

                if (NrOfDays > 5)
                    _clsPago.ValAlmacenaje = "No";

                string[] cadenaADUANA = n_manifiesto.Split('-');

                string n_year = cadenaADUANA[0].ToString();
                string n_mani = cadenaADUANA[1].ToString();

                //string valorADUANA = ObtenerADUANA(n_contenedor, n_year, n_mani);

                string valorADUANA = "DENEGADO";

                var query = (dynamic)null;

                List<Pago> pResult = new List<Pago>();

                if (pLista.Count > 0 && pLstDetalle.Count > 0)
                {
                    query = from a in pLista
                            join b in pLstDetalle on a.n_contenedor equals b.n_contenedor
                            select new
                            {

                                validacion = a.validacion == "Si" ? "Si, El contenedor esta libre de pagos con CEPA" : "No, El contenedor tiene pagos pendientes con CEPA",
                                b_danc = retenResult == "LIBRE" ? "Si, El contenedor esta libre de revisiones con PNC - DAN" : ("No, El contenedor tiene revisión pendiente con X desde ").Replace("X", detaReten) + retenResult,
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : valorADUANA == "DENEGADO" ? "No, El contenedor no se encuentra autorizado por ADUANA para su retiro" : valorADUANA,



                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_dan = retenResult == "LIBRE" ? "#18D318" : "#FF0000",
                                style_aduana = valorADUANA == "AUTORIZADO" ? "#18D318" : "#FF0000",

                                style_naviero_transfer = b.descripcion == "Transferencia" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_transfer = b.descripcion == "Transferencia" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_manejo = b.descripcion == "Manejo" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_manejo = b.descripcion == "Manejo" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_alma = b.descripcion == "Almacenaje" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_alma = b.descripcion == "Almacenaje" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_despacho = b.descripcion == "Despacho" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_despacho = b.descripcion == "Despacho" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                /*p_tranfer =  b.descripcion == "Transferencia" ? b.s_pagos : 0.00,
                                p_manejo =  b.descripcion == "Manejo" ? b.s_pagos : 0.00,
                                p_alma = b.descripcion == "Almacenaje" ? b.s_pagos : 0.00,
                                p_despacho = b.descripcion == "Despacho" ? b.s_pagos : 0.00,*/
                                descripcion = b.descripcion,
                                f_tarja = b.f_tarja,
                                f_salida = b.f_salida,
                                b_salida = b.b_salida,
                                v_dias = b.v_dias,
                                v_peso = b.v_peso,
                                b_transito = b.b_transito == "N" ? "PATIO CEPA" : "RETIRO DIRECTO",
                                v_teus = b.v_teus,
                                c_tamaño = b.c_tamaño
                            };
                }
                else
                {
                    Pago _clPago = new Pago
                    {
                        n_contenedor = n_contenedor,
                        b_tarifa = "DESCONOCIDA",
                        validacion = "No",
                        ValTransfer = "No",
                        ValDespacho = "No",
                        ValManejo = "No",
                        ValAlmacenaje = "No"

                    };

                    pLista.Add(_clPago);

                    query = from a in pLista
                            join b in pLstDetalle on a.n_contenedor equals b.n_contenedor
                            select new
                            {
                                prueba = tiReten == "DAN" ? "PNC - DAN" : "UCC",
                                validacion = a.validacion == "Si" ? "Si, El contenedor esta libre de pagos con CEPA" : "No, El contenedor tiene pagos pendientes con CEPA",

                                b_danc = retenResult == "LIBRE" ? "Si, El contenedor esta libre de revisiones con PNC - DAN" : ("No, El contenedor tiene revisión pendiente con X desde ").Replace("X", detaReten) + retenResult,
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : valorADUANA == "DENEGADO" ? "No, El contenedor no se encuentra autorizado por ADUANA para su retiro" : valorADUANA,

                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "CSS/Img/icono_correcto.png" : "CSS/Img/ErrorIcon.png",
                                style_dan = retenResult == "LIBRE" ? "#18D318" : "#FF0000",
                                style_aduana = valorADUANA == "AUTORIZADO" ? "#18D318" : "#FF0000",


                                style_naviero_transfer = b.descripcion == "Transferencia" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_transfer = b.descripcion == "Transferencia" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_manejo = b.descripcion == "Manejo" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_manejo = b.descripcion == "Manejo" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_alma = b.descripcion == "Almacenaje" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_alma = b.descripcion == "Almacenaje" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                style_naviero_despacho = b.descripcion == "Despacho" && b.s_naviero == "Y" ? b.s_pagos : 0.00,
                                style_cliente_despacho = b.descripcion == "Despacho" && b.s_cliente == "Y" ? b.s_pagos : 0.00,

                                /*p_tranfer =  b.descripcion == "Transferencia" ? b.s_pagos : 0.00,
                                p_manejo =  b.descripcion == "Manejo" ? b.s_pagos : 0.00,
                                p_alma = b.descripcion == "Almacenaje" ? b.s_pagos : 0.00,
                                p_despacho = b.descripcion == "Despacho" ? b.s_pagos : 0.00,*/
                                descripcion = b.descripcion,
                                f_tarja = b.f_tarja,
                                f_salida = b.f_salida,
                                b_salida = b.b_salida,
                                v_dias = b.v_dias,
                                v_peso = b.v_peso,
                                b_transito = b.b_transito == "N" ? "PATIO CEPA" : "RETIRO DIRECTO",
                                v_teus = b.v_teus,
                                c_tamaño = b.c_tamaño
                            };
                }

                //var c = (Pago)query.ToList();

                string jConsult = Newtonsoft.Json.JsonConvert.SerializeObject(query);

                pResult = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pago>>(jConsult);




                var consul = (from d in pResult
                              group d by new
                              {
                                  validacion = d.validacion,
                                  b_danc = d.b_danc,
                                  style_va = d.style_va,
                                  style_transfer = d.style_transfer,
                                  style_despac = d.style_despac,
                                  style_manejo = d.style_manejo,
                                  style_alma = d.style_alma,
                                  style_dan = d.style_dan,
                                  style_naviero_transfer = Convert.ToDouble(d.style_naviero_transfer),
                                  style_cliente_transfer = Convert.ToDouble(d.style_cliente_transfer),
                                  style_naviero_manejo = Convert.ToDouble(d.style_naviero_manejo),
                                  style_cliente_manejo = Convert.ToDouble(d.style_cliente_manejo),
                                  style_naviero_alma = Convert.ToDouble(d.style_naviero_alma),
                                  style_cliente_alma = Convert.ToDouble(d.style_cliente_alma),
                                  style_naviero_despacho = Convert.ToDouble(d.style_naviero_despacho),
                                  style_cliente_despacho = Convert.ToDouble(d.style_cliente_despacho),
                                  descripcion = d.descripcion,
                                  style_aduana = d.style_aduana,
                                  b_aduana = d.b_aduana,
                                  f_salida = d.f_salida,
                                  f_tarja = d.f_tarja,
                                  b_salida = d.b_salida,
                                  v_dias = d.v_dias,
                                  v_peso = d.v_peso,
                                  b_transito = d.b_transito,
                                  v_teus = d.v_teus,
                                  c_tamaño = d.c_tamaño

                              } into g
                              select new
                              {

                                  validacion = g.Key.validacion,
                                  b_danc = g.Key.b_danc,

                                  style_va = g.Key.style_va,
                                  style_dan = g.Key.style_dan,

                                  style_descripcion = g.Key.descripcion == "Transferencia" ? g.Key.style_transfer : g.Key.descripcion == "Despacho" ? g.Key.style_despac : g.Key.descripcion == "Manejo" ? g.Key.style_manejo : g.Key.descripcion == "Almacenaje" ? g.Key.style_alma : "",

                                  style_naviero = g.Key.descripcion == "Transferencia" ? g.Key.style_naviero_transfer : g.Key.descripcion == "Despacho" ? g.Key.style_naviero_despacho : g.Key.descripcion == "Manejo" ? g.Key.style_naviero_manejo : g.Key.descripcion == "Almacenaje" ? g.Key.style_naviero_alma : 0.00,
                                  style_cliente = g.Key.descripcion == "Transferencia" ? g.Key.style_cliente_transfer : g.Key.descripcion == "Despacho" ? g.Key.style_cliente_despacho : g.Key.descripcion == "Manejo" ? g.Key.style_cliente_manejo : g.Key.descripcion == "Almacenaje" ? g.Key.style_cliente_alma : 0.00,

                                  style_pendiente = g.Key.descripcion == "Transferencia" ?
                                                           !g.Key.style_transfer.Contains("correcto") ?
                                                                g.Key.style_naviero_transfer > 0.00 ? g.Key.style_naviero_transfer : g.Key.style_cliente_transfer
                                                           : 0.00 : g.Key.descripcion == "Despacho" ? !g.Key.style_despac.Contains("correcto") ? g.Key.style_naviero_despacho > 0.00 ? g.Key.style_naviero_despacho : g.Key.style_cliente_despacho : 0.00 : g.Key.descripcion == "Manejo" ? !g.Key.style_manejo.Contains("correcto") ? g.Key.style_naviero_manejo > 0.00 ? g.Key.style_naviero_manejo : g.Key.style_cliente_manejo : 0.00 : g.Key.descripcion == "Almacenaje" ? !g.Key.style_alma.Contains("correcto") ? g.Key.style_naviero_alma > 0.00 ? g.Key.style_naviero_alma : g.Key.style_cliente_alma : 0.00 : 0.00,

                                  descripcion = g.Key.descripcion,

                                  b_aduana = g.Key.b_aduana,
                                  style_aduana = g.Key.style_aduana,
                                  f_tarja = g.Key.f_tarja,
                                  f_salida = g.Key.f_salida,

                                  //p_tranfer = g.Key.descripcion == "Transferencia" ? g.Sum(x => x.p_tranfer) : g.Key.descripcion == "Despacho" ? g.Sum(x => x.p_despacho) : g.Key.descripcion == "Manejo" ? g.Sum(x => x.p_manejo) : g.Key.descripcion == "Almacenaje" ? g.Sum(x => x.p_alma) : 0.0
                                  b_salida = g.Key.b_salida,
                                  detalle = g.Key.descripcion == "Almacenaje" ? string.Format("5 Días libres; {0} Días a cobrar X {1} tm X tarifa", g.Key.v_dias, g.Key.v_peso) : g.Key.descripcion == "Transferencia" ? string.Format("Ubicación({0}) X tarifa", g.Key.b_transito) : g.Key.descripcion == "Manejo" ? string.Format("{0} equivalente en TEUS ({1}) X tarifa ", g.Key.c_tamaño, g.Key.v_teus) : g.Key.descripcion == "Despacho" ? "Contenedor x Tarifa" : ""

                              }).ToList();

                string _respuesta = Newtonsoft.Json.JsonConvert.SerializeObject(consul);

                return _respuesta;
            }
            catch (Exception ex)
            {
                string _mensaje = "Error: Tiempo de espera agotado, intentelo de nuevo, presione la tecla F5: " + ex.Message;

                return Newtonsoft.Json.JsonConvert.SerializeObject(_mensaje);
            }

        }

        [System.Web.Services.WebMethod]
        public static string ObtenerDecla(string n_contenedor, string n_mani)
        {
            try
            {
                string _resultado = null;
                string _mani = "";
                List<Declaracion> pDecla = new List<Declaracion>();

                MemoryStream memoryStream = new MemoryStream();
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Encoding = new UTF8Encoding(false);
                xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                xmlWriterSettings.Indent = true;

                XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                CEPAService.WSManifiestoCEPAClient _proxy = new CEPAService.WSManifiestoCEPAClient();

                string[] cadenaADUANA = n_mani.Split('-');

                string n_year = cadenaADUANA[0].ToString();
                string nmani = cadenaADUANA[1].ToString();
                string _Aduana = null;

                xml.WriteStartDocument();

                xml.WriteStartElement("MDS4");

                xml.WriteElementString("CAR_REG_YEAR", n_year);
                xml.WriteElementString("KEY_CUO", "02");
                xml.WriteElementString("CAR_REG_NBER", nmani);
                xml.WriteElementString("CAR_CTN_IDENT", n_contenedor.Trim().TrimEnd().TrimStart());



                xml.WriteEndDocument();
                xml.Flush();
                xml.Close();

                //Generar XML para enviar parametros al servicio.
                _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                XmlDocument doc = new XmlDocument();
                _resultado = _proxy.getDocumentoInfoDocumento(_Aduana);

                if (_resultado.Contains("encuentran"))
                {
                    _mani = "DENEGADO";

                }
                else
                {

                    doc.LoadXml(_resultado);
                    XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4/MDS5S/MDS5");

                    XmlNode unContenedor;



                    if (listaCntres.Count > 0)
                    {


                        for (int i = 0; i < listaCntres.Count; i++)
                        {
                            unContenedor = listaCntres.Item(i);

                            Declaracion valiDecla = new Declaracion
                            {

                                tipo_doc = unContenedor.SelectSingleNode("TIPO_DOC").InnerText,
                                num_doc = unContenedor.SelectSingleNode("NUM_DOC").InnerText,
                                b_estado = unContenedor.SelectSingleNode("VAL_DOC").InnerText == "1" ? "AUTORIZADA" : "DENEGADA"
                            };


                            pDecla.Add(valiDecla);


                        }
                    }
                    else
                    {
                        _mani = "DENEGADO";
                    }
                }


                string re = null;
                if (pDecla.Count > 0)
                {
                    re = Newtonsoft.Json.JsonConvert.SerializeObject(pDecla);
                    return re;
                }
                else
                    return "No se posee declaraciones a mostrar";
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [System.Web.Services.WebMethod]
        public static string ImprimirPDF(string htmlOut, string n_contenedor)
        {

            string query = "Generado Satisfactoriamente";
            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        [System.Web.Services.WebMethod]
        public static string Imprime(string htmlOut, string n_contenedor)
        {
            string fileName = "Tracking_" + n_contenedor.ToUpper();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/pdf";

            ////define pdf filename
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);


            //Generate PDF
            using (var document = new Document(PageSize.A4, 40, 40, 40, 40))
            {
                htmlOut = FormatImageLinks(htmlOut);
            }

            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Flush();

            string query = "Prueba";
            return Newtonsoft.Json.JsonConvert.SerializeObject(query);
        }

        public static string FormatImageLinks(string input)
        {
            if (input == null)
                return string.Empty;
            string tempInput = input;
            const string pattern = @"<img(.|\n)+?>";
            HttpContext context = HttpContext.Current;

            //Change the relative URL's to absolute URL's for an image, if any in the HTML code.
            foreach (Match m in Regex.Matches(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.RightToLeft))
            {
                if (m.Success)
                {
                    string tempM = m.Value;
                    string pattern1 = "src=[\'|\"](.+?)[\'|\"]";
                    Regex reImg = new Regex(pattern1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    Match mImg = reImg.Match(m.Value);

                    if (mImg.Success)
                    {
                        string src = mImg.Value.ToLower().Replace("src=", "").Replace("\"", "").Replace("\'", "");

                        if (!src.StartsWith("http://") && !src.StartsWith("https://"))
                        {
                            //Insert new URL in img tag
                            src = "src=\"" + context.Request.Url.Scheme + "://" +
                                  context.Request.Url.Authority + "/" + src + "\"";
                            try
                            {
                                tempM = tempM.Remove(mImg.Index, mImg.Length);
                                tempM = tempM.Insert(mImg.Index, src);

                                //insert new url img tag in whole html code
                                tempInput = tempInput.Remove(m.Index, m.Length);
                                tempInput = tempInput.Insert(m.Index, tempM);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            return tempInput;
        }

        protected void btnImprime_Click(object sender, EventArgs e)
        {

            string fileName = "Tracking_" + txtBuscar.Text.ToUpper() + ".pdf";
            string linkCss = "~/bootstrap/csss/bootstrap-complete.css";

            Response.Clear();
            Response.ContentType = "application/pdf";

            ////define pdf filename
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlOut = new HtmlTextWriter(stringWrite);
            printArea.RenderControl(htmlOut);

            //Generate PDF
            using (var document = new Document(PageSize.A4, 40, 40, 40, 40))
            {

                string _convert = FormatImageLinks(htmlOut.InnerWriter.ToString());


                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(_convert);
                //Convert HTML to well-formed XHTML
                htmlDoc.OptionFixNestedTags = true;
                htmlDoc.OptionOutputAsXml = true;
                htmlDoc.OptionCheckSyntax = true;
                HtmlNode bodyNode = htmlDoc.DocumentNode;
                _convert = bodyNode.WriteTo();



                //define output control HTML
                var memStream = new MemoryStream();
                TextReader xmlString = new StringReader(_convert.ToString().Replace("\r", "").Replace("\n", "").Replace("  ", "").Replace("px", "").Replace("\0", "").Replace("%", "").Replace("<br>", "<br />"));

                PdfWriter writer = PdfWriter.GetInstance(document, memStream);

                //open doc
                document.Open();

                // register all fonts in current computer
                FontFactory.RegisterDirectories();

                // Set factories
                var htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                // Set css
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                var rutaCSS = Server.MapPath(linkCss);
                cssResolver.AddCssFile(rutaCSS, true);



                // Export
                IPipeline pipeline = new CssResolverPipeline(cssResolver,
                                                             new HtmlPipeline(htmlContext,
                                                                              new PdfWriterPipeline(document, writer)));
                var worker = new XMLWorker(pipeline, true);
                var xmlParse = new XMLParser(true, worker, Encoding.Unicode);
                xmlParse.Parse(xmlString);
                xmlParse.Flush();

                document.Close();
                document.Dispose();

                Response.BinaryWrite(memStream.ToArray());
            }

            Response.End();
            Response.Flush();



        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
