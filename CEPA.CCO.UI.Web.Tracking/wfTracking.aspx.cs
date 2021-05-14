using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CEPA.CCO.Entidades;
using CEPA.CCO.BL;
using CEPA.CCO.DAL;
using CEPA.CCO.Linq;
using Recaptcha;
using System.Net;
using System.IO;
using System.Xml;
using System.Text;


using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

using System.Drawing;
using System.Configuration;

using System.Web.Optimization;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System.Data;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace CEPA.CCO.UI.Web.Tracking
{
    public partial class wfTracking : System.Web.UI.Page
    {

        private static readonly DateTime FIRST_GOOD_DATE1 = new DateTime(2016, 01, 01, 00, 00, 00);
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1900, 01, 01);
        public int b_sidunea = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            String currurl = HttpContext.Current.Request.RawUrl;


            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            // Check to make sure some query string variables
            // exist and if not add some and redirect.
            int iqs = currurl.IndexOf('?');

            // If query string variables exist, put them in
            // a string.

            if (!IsPostBack)
            {
                //btnVEA.Enabled = false;
            }

            if (iqs >= 0)
            {
                bool flag;
                string contenedor = HttpUtility.ParseQueryString(myUri.Query).Get("contenedor");
                string a_decla = HttpUtility.ParseQueryString(myUri.Query).Get("a_decla");                
                string s_decla = HttpUtility.ParseQueryString(myUri.Query).Get("s_decla");
                string c_decla = HttpUtility.ParseQueryString(myUri.Query).Get("c_decla");
                string b_sidunea = HttpUtility.ParseQueryString(myUri.Query).Get("b_sidunea");

                txtBuscar.Text = contenedor;
                a_declaracion.Text = a_decla;
                n_serial.Text = s_decla;
                n_correlativo.Text = c_decla;
                radio3.Checked = Boolean.TryParse(b_sidunea, out flag);
                
                btnBuscar_Click(btnBuscar, new EventArgs());               
            }   
        }
        protected void recaptcha_demo_submit_Click(object sender, EventArgs e)
        {

        }
        string getIpAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        public bool reCaptchaValidate(string ipAddress)
        {
            bool Valid = false;
            string Response = Request["g-recaptcha-response"];//Getting Response String Append to Post Method
            string url = @"https://www.google.com/recaptcha/api/siteverify?secret=6Lc_mh8TAAAAAG9wxfSWQDNVsENJ60DtqWO05lzK&response=" + Response + @"&remoteip=" + ipAddress;
            //Request to Google Server
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            //Google recaptcha Response
            using (WebResponse wResponse = req.GetResponse())
            {
                using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                {
                    string jsonResponse = readStream.ReadToEnd();
                    if (jsonResponse.Substring(15, 4) == "true")
                    {
                        Valid = true;
                    }
                }
            }

            return Valid;

        }

        private string ValidacionADUANA(string n_contenedor, string a_decla, string c_serie, string c_correlativo, int b_sidunea)
        {
            try
            {
                string _resultado = null;
                string _manifiesto = null;
                string _cadena = null;
                string _mani = null;

                int a_declaracion = 0;
                int s_declaracion = 0;
                int c_declaracion = 0;

                if (a_decla.Trim().TrimStart().TrimEnd().Length == 4)
                {
                    object x;
                    x = a_decla;
                    if (ArchivoBookingDAL.isNumeric(x))
                    {
                        a_declaracion = Convert.ToInt32(x.ToString());
                    }
                    else
                    {
                        throw new Exception("Año declaración no válido Ej. 20XX");
                    }

                }
                else
                {
                    throw new Exception("Año declaración no válido Ej. 20XX");
                }

                if (c_serie.Trim().TrimEnd().TrimStart().Length == 1)
                {
                    object x;
                    x = c_serie;
                    if (ArchivoBookingDAL.isNumeric(x))
                    {
                        s_declaracion = Convert.ToInt32(x.ToString());
                    }
                    else
                    {
                        throw new Exception("Serie declaración no válido Ej. X");
                    }
                }
                else
                {
                    throw new Exception("Serie declaración no posee mas de un dígito");
                }

                if (c_correlativo.Trim().TrimEnd().TrimStart().Length <= 6)
                {
                    object x;
                    x = c_correlativo;
                    if (ArchivoBookingDAL.isNumeric(x))
                    {
                        c_declaracion = Convert.ToInt32(x.ToString());
                    }
                    else
                    {
                        throw new Exception("Correlativo declaración no válido Ej. XXXXXX");
                    }
                }
                else
                {
                    throw new Exception("Correlativo declaración no posee mas que 6 dígitos");
                }

                List<Declaracion> pValidCon = new List<Declaracion>();

                pValidCon = DetaNavieraDAL.getValidConten(n_contenedor, a_decla, c_serie, c_correlativo);

                if (pValidCon.Count > 0)
                {
                    foreach (var decla in pValidCon)
                    {
                        if (decla.n_manifiesto != "0-0")
                            _mani = decla.n_manifiesto;
                        else
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                            xmlWriterSettings.Encoding = new UTF8Encoding(false);
                            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
                            xmlWriterSettings.Indent = true;

                            XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);

                            //CEPAService.WSManifiestoCEPAClient _proxy = new CEPAService.WSManifiestoCEPAClient();


                            string _Aduana = null;

                            xml.WriteStartDocument();

                            xml.WriteStartElement("MDS4");

                            xml.WriteElementString("SAD_REG_YEAR", a_declaracion.ToString());
                            xml.WriteElementString("KEY_CUO", "02");
                            xml.WriteElementString("SAD_REG_SERIAL", s_declaracion.ToString());
                            xml.WriteElementString("SAD_REG_NBER", c_declaracion.ToString());
                            xml.WriteElementString("CAR_CTN_IDENT", n_contenedor.Trim().TrimEnd().TrimStart());

                            xml.WriteEndDocument();
                            xml.Flush();
                            xml.Close();

                            //Generar XML para enviar parametros al servicio.
                            _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

                            ServicePointManager.Expect100Continue = true;
                            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;


                            XmlDocument doc = new XmlDocument();

                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                            if (b_sidunea == 0)
                            {

                                //_resultado = _proxy.validaContenedorDecla(_Aduana);
                                _resultado = "0";
                            }
                            else
                            {
                                CepaSW.WSManifiestoCEPAClient _proxySW = new CepaSW.WSManifiestoCEPAClient();

                                string _usuario = ConfigurationManager.AppSettings["userSidunea"];
                                string _pass = ConfigurationManager.AppSettings["pswSidunea"];

                                _proxySW.ClientCredentials.UserName.UserName = _usuario;
                                _proxySW.ClientCredentials.UserName.Password = _pass;

                                _resultado = _proxySW.validaContenedorDecla(_Aduana);
                            }

                            if (_resultado.Substring(0, 1) == "1")
                            {
                                _cadena = _resultado.Remove(0, 2);
                                doc.LoadXml(_cadena);
                                XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4");

                                XmlNode _manifiestos;

                                for (int i = 0; i < listaCntres.Count; i++)
                                {
                                    _manifiestos = listaCntres.Item(i);

                                    _mani = _manifiestos.SelectSingleNode("MANIFIESTO").InnerText;

                                }
                            }
                            else
                            {
                                _mani = "0";
                            }
                        }
                    }
                }



                return _mani;
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message + "<br><strong>Favor intentar mas tarde, presentamos problemas de comunicación o reportar a Informática ACAJUTLA 2405-3255 con Elsa Sosa</strong>");
            }

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            //txtBuscar.Text = "";
            //n_correlativo.Text = "";
            //n_serial.Text = "";
            //a_declaracion.Text = "";
            //radio3.Checked = false;
            //grvTracking.DataSource = null;
            //grvTracking.DataBind();
            Response.Redirect("wfTracking.aspx");
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            List<TrackingEnca> pLista = new List<TrackingEnca>();
            List<TrackingEnca> pLista1 = new List<TrackingEnca>();
            bool pase = true;
            string valida = null;
            string v_aduana = null;


            try
            {
                //v_aduana = "2016-406";
                //ScriptManager.RegisterClientScriptBlock(btnBuscar, typeof(Button), "testscript1", "alertError();", true);
                //System.Threading.Thread.Sleep(1000);
                if (radio3.Checked == true)
                    b_sidunea = 1;
                else
                    b_sidunea = 0;



                if (txtBuscar.Text.Trim().TrimStart().TrimEnd().ToUpper().Replace("-", "").Length == 11)
                {

                    valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, txtBuscar.Text.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlTracking);
                    //valida = "VALIDO";

                    if (valida == "VALIDO")
                    {
                        v_aduana = ValidacionADUANA(txtBuscar.Text.Trim().TrimEnd().TrimStart().ToUpper().Replace("-", ""), a_declaracion.Text, n_serial.Text, n_correlativo.Text, b_sidunea);
                        //v_aduana = "2019-176";
                        if (v_aduana == "0")
                        {
                            ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "validar", "btnCheck(0);", true);
                            throw new Exception("Alerta!! Revise los párametros de ingreso a está consulta y/o los utilizados en sus declaración de mercancía");
                        }
                        else
                        {
                            pLista = DocBuqueLINQ.ObtenerTracking_Cliente(txtBuscar.Text, "11", v_aduana, Convert.ToInt32(a_declaracion.Text), Convert.ToInt32(n_serial.Text), Convert.ToInt32(n_correlativo.Text));

                            if (pLista.Count > 0)
                            {
                                
                                grvTracking.Visible = true;
                                grvTracking.DataSource = pLista;
                                grvTracking.DataBind();


                                grvTracking.HeaderRow.TableSection = TableRowSection.TableHeader;
                                grvTracking.HeaderRow.Cells[0].Attributes["text-align"] = "center";
                                grvTracking.FooterRow.Cells[0].Attributes["text-align"] = "center";
                                grvTracking.FooterRow.TableSection = TableRowSection.TableFooter;

                                
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "validar", "btnCheck(1);", true);

                            }
                            else
                            {
                             
                                grvTracking.DataSource = null;
                                grvTracking.DataBind();
                                Label lblEmptyMessage = grvTracking.Controls[0].Controls[0].FindControl("lblEmptyMessage") as Label;
                                lblEmptyMessage.Text = "No se poseen registros de este contenedor: " + txtBuscar.Text;
                                ScriptManager.RegisterStartupScript(this.UpdatePanel1, GetType(), "validar", "btnCheck(0);", true);
                                throw new Exception("No se poseen registros de este contenedor: " + txtBuscar.Text);
                            }
                        }
                    }
                    else if (valida == "NO VALIDO")
                    {
                        throw new Exception(string.Format("Este # {0} de contenedor no es un número válido", txtBuscar.Text));
                    }
                    else if (valida == "LONGITUD")
                    {
                        throw new Exception(string.Format("Este # {0} de contenedor posee mas de 11 caracteres", txtBuscar.Text));
                    }
                }
                else
                {
                    throw new Exception("Este # de contenedor no posee los 11 caracteres " + txtBuscar.Text);
                }

                //txtBuscar.Text = "";
                //n_correlativo.Text = "";
                //n_serial.Text = "";
                //a_declaracion.Text = "";
                //radio3.Checked = false;

            }
            catch (Exception ex)
            {
                txtBuscar.Text = "";
                n_correlativo.Text = "";
                n_serial.Text = "";
                a_declaracion.Text = "";
                radio3.Checked = false;               
                grvTracking.DataSource = null;
                grvTracking.DataBind();                
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message.Replace("'", "") + "');", true);

            }
            finally
            {

                ScriptManager.RegisterStartupScript(this, typeof(string), "", "grecaptcha.reset();", true);
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


        protected void grvTracking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                if (ArchivoBookingDAL.isFecha(e.Row.Cells[11].Text) == true)
                {
                    if (Convert.ToDateTime(e.Row.Cells[11].Text) > FIRST_GOOD_DATE)
                    { }
                    else
                    {
                        e.Row.Cells[11].Text = "";
                    }

                }
                if (ArchivoBookingDAL.isFecha(e.Row.Cells[12].Text) == true)
                {
                    if (Convert.ToDateTime(e.Row.Cells[12].Text) > FIRST_GOOD_DATE)
                    { }
                    else
                    {
                        e.Row.Cells[12].Text = "";
                    }

                }


                TrackingEnca order = (TrackingEnca)e.Row.DataItem;

                DetailsView gvDetails = (DetailsView)e.Row.FindControl("dtTracking");

                gvDetails.DataSource = order.TrackingList;
                gvDetails.DataBind();

                Label lblFecha = (Label)gvDetails.Rows[10].Cells[0].FindControl("lblFechaP");
                Label lblSelec = (Label)gvDetails.Rows[10].Cells[0].FindControl("lblSelectividad");

                string vFecha = null;

                if (lblFecha.Text.Contains("VERDE"))
                {

                    vFecha = lblFecha.Text.Remove((lblFecha.Text.Length - 5), 5);
                    lblFecha.Text = vFecha;
                    lblSelec.Text = "VERDE";
                    lblSelec.ForeColor = System.Drawing.Color.Green;
                    lblSelec.Font.Bold = true;
                }
                else if (lblFecha.Text.Contains("AMARILLO"))
                {
                    vFecha = lblFecha.Text.Remove((lblFecha.Text.Length - 8), 8);
                    lblFecha.Text = vFecha;
                    lblSelec.Text = "AMARILLO";
                    lblSelec.ForeColor = System.Drawing.Color.Yellow;
                    lblSelec.Font.Bold = true;
                }
                else if (lblFecha.Text.Contains("ROJO"))
                {
                    vFecha = lblFecha.Text.Remove((lblFecha.Text.Length - 4), 4);
                    lblFecha.Text = vFecha;
                    lblSelec.Text = "ROJO";
                    lblSelec.ForeColor = System.Drawing.Color.Red;
                    lblSelec.Font.Bold = true;
                }
                else if (lblFecha.Text.Contains("AZUL"))
                {
                    vFecha = lblFecha.Text.Remove((lblFecha.Text.Length - 4), 4);
                    lblFecha.Text = vFecha;
                    lblSelec.Text = "AZUL";
                    lblSelec.ForeColor = System.Drawing.Color.Blue;
                    lblSelec.Font.Bold = true;
                }

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
                    item.Cells[0].Attributes.Add("style", "width:30%;");
                    if (ArchivoBookingDAL.isFecha(item.Cells[1].Text) == true)
                    {
                        if (Convert.ToDateTime(item.Cells[1].Text) > FIRST_GOOD_DATE || Convert.ToDateTime(item.Cells[1].Text) > FIRST_GOOD_DATE1)
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


                GridView gvProvisionales = (GridView)gvDetails.Rows[31].Cells[1].FindControl("grvProv");

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
                                GridView gvDetaProvi = (GridView)gvProvisionales.Rows[0].Cells[2].FindControl("grvDetailProvi");

                                if (gvDetaProvi != null)
                                {
                                    gvDetaProvi.DataSource = pListD;
                                    gvDetaProvi.DataBind();

                                    gvDetaProvi.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

                                    // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                                    gvDetaProvi.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                                    gvDetaProvi.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                                    gvDetaProvi.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                                    gvDetaProvi.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                                    gvDetaProvi.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";
                                    gvDetaProvi.HeaderRow.Cells[6].Attributes["data-hide"] = "phone";
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Favor!! Intente de nuevo se produjo un error, o consultar a Informatica CEPA/Acajutla');", true);
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

                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

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

                    //_resultado = _proxySW.getDocumentoInfoDocumento(_Aduana);
                    _resultado = "encuentran";
                }

                if (_resultado.Contains("encuentran"))
                {
                    _mani = "DENEGADO";

                }
                else
                {

                    if (b_sidunea == 0)
                    {
                        doc.LoadXml(_resultado);
                        XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4");

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

                var lstReten = (from b in PagoDAL.ConsultaDAN(c_llegada, n_contenedor, DBComun.TipoBD.SqlTracking)
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


                pLstDetalle = DetaNavieraDAL.CalcPagos(n_contenedor, c_llegada, f_tar, DBComun.TipoBD.SqlTracking, v_peso);

                DateTime f_salidaCalc = DetaNavieraLINQ.FechaBD(); 
                             
                TimeSpan t = f_salidaCalc.Date - f_tar.Date;
                int NrOfDays = t.Days + 1;

                if (NrOfDays > 5)
                    _clsPago.ValAlmacenaje = "No";


                string[] cadenaADUANA = n_manifiesto.Split('-');

                string n_year = cadenaADUANA[0].ToString();
                string n_mani = cadenaADUANA[1].ToString();

                //string valorADUANA = ObtenerADUANA(n_contenedor, n_year, n_mani);
                string valorADUANA = "DENAGADO";

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
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : "No, El contenedor no se encuentra autorizado por ADUANA para su retiro",



                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
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
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : "No, El contenedor no se encuentra autorizado por ADUANA para su retiro",

                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
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
                string _mensaje = "Error: Tiempo de espera agotado, intentelo de nuevo, presione la tecla F5 : Error detectado: " + ex.Message.ToString();

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

                var lstReten = (from b in PagoDAL.ConsultaDAN(c_llegada, n_contenedor, DBComun.TipoBD.SqlTracking)
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



                pLstDetalle = DetaNavieraDAL.CalcPagos(n_contenedor, c_llegada, f_tar, f_re, DBComun.TipoBD.SqlTracking, v_peso);


                TimeSpan t = f_re.Date - f_tar.Date;
                int NrOfDays = t.Days + 1;

                if (NrOfDays > 5)
                    _clsPago.ValAlmacenaje = "No";


                string[] cadenaADUANA = n_manifiesto.Split('-');

                string n_year = cadenaADUANA[0].ToString();
                string n_mani = cadenaADUANA[1].ToString();

                //string valorADUANA = ObtenerADUANA(n_contenedor, n_year, n_mani);
                string valorADUANA = "DENAGADO";

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
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : "No, El contenedor no se encuentra autorizado por ADUANA para su retiro",



                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
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

                                b_danc = retenResult == "LIBRE" ? "Si, El contenedor esta libre de revisiones con PNC - DAN" : ("No, El contenedor tiene revisión pendiente con X desde ").Replace("X", detaReten) + retenResult,
                                b_aduana = valorADUANA == "AUTORIZADO" ? "Si, el contenedor esta autorizado por ADUANA" : "No, El contenedor no se encuentra autorizado por ADUANA para su retiro",



                                style_va = a.validacion == "Si" ? "#18D318" : "#FF0000",
                                style_transfer = a.ValTransfer == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_despac = a.ValDespacho == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_manejo = a.ValManejo == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
                                style_alma = a.ValAlmacenaje == "Si" ? "vendor/bootstrap/Images/icono_correcto.png" : "vendor/bootstrap/Images/ErrorIcon.png",
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
                string _mensaje = "Error: Tiempo de espera agotado, intentelo de nuevo, presione la tecla F5";

                return Newtonsoft.Json.JsonConvert.SerializeObject(_mensaje);
            }

        }

        protected void btnVEA_Click(object sender, EventArgs e)
        {
            string cadena = @"https://test7.mh.gob.sv/VEA/free/InfoDm.do?anio=2020&aduana=02&serial=4&registro=2";
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "show window", "shwwindow('" + cadena + "');", true);
        }

      
    }
}