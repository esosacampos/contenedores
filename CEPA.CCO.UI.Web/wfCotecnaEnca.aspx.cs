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

using System.IO;
using System.Globalization;
using NReco.PdfGenerator;

using cxExcel = ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.Configuration;

using Ionic.Zip;

namespace CEPA.CCO.UI.Web
{
    public partial class wfCotecnaEnca : System.Web.UI.Page
    {
        public static string EscapeXMLValue(string xmlString)
        {

            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("'", "&apos;").Replace("\"", "&quot;").Replace("(", "").Replace(")", "").Replace("&", "&amp;");
        }

        public static string UnescapeXMLValue(string xmlString)
        {
            if (xmlString == null)
                throw new ArgumentNullException("xmlString");

            return xmlString.Replace("&apos;", "'").Replace("&quot;", "\"").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    Cargar();

                }
                catch (Exception Ex)
                {
                    Response.Write("<script>bootbox.alert('" + Ex.Message + "');</script>");
                }
            }

            RegisterPostBackControl();
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

                e.Row.Cells[0].Controls.Add(new LiteralControl("<ul class='pagination pagination-centered hide-if-no-paging'></ul><div class='divider'  style='margin-bottom: 15px;'></div></div><span class='label label-default pie' style='background-color: #dff0d8;border-radius: 25px;font-family: sans-serif;font-size: 18px;color: #468847;border-color: #d6e9c6;margin-top: 18px;'></span>"));
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btnOpen")
            {
                string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                string c_llegada = Convert.ToString(commandArgs[0]);
                DateTime f_llegada = Convert.ToDateTime(commandArgs[1]);
                string f_lleg = f_llegada.ToString("dd/MM/yy HH:mm", CultureInfo.CreateSpecificCulture("es-SV"));
                string d_buque = Convert.ToString(commandArgs[2]);
                string f_corto = f_llegada.ToString("ddMMyy");
                try
                {
                    ExportarExcel(c_llegada, f_lleg, d_buque, f_corto);
                }
                catch (Exception Ex)
                {
                    Response.Write("<script>bootbox.alert('GENERAR LISTADOS:" + Ex.Message + "');</script>");
                }
                try
                {
                    Cargar();
                }
                catch (Exception Ex)
                {
                    Response.Write("<script>bootbox.alert('CARGA PRINCIPAL:" + Ex.Message + "');</script>");
                }
            }         
            return;
        }

        public void ExportarExcel(string c_llegada, string f_llegada, string d_buque, string f_corto)
        {
            List<string> pListaNo = new List<string>();
            List<string> pRespuesta = new List<string>();
            int _resulDeta = 0;            
            string _mensaje = null;

            List<ContenedoresAduana> pListManis = new List<ContenedoresAduana>();

            pListManis = ResulNavieraDAL.ObtenerManiBuque(DBComun.Estado.verdadero, c_llegada);

            if (pListManis.Count > 0)
            {
                //Actualizar manifiesto electronico
                foreach (var iMani in pListManis)
                {
                    insertMani(pListaNo, pRespuesta, ref _resulDeta, iMani.b_sidunea, ref _mensaje, iMani.n_manifiesto, iMani.a_manifiesto);
                }

                //Generar y almacenar listados
                foreach (var iLst in pListManis)
                {
                    List<ArchivoAduana> pCotecna = new List<ArchivoAduana>();
                    pCotecna = DetaNavieraDAL.getCotecnaLstWeb(iLst.a_manifiesto, iLst.n_manifiesto, DBComun.Estado.verdadero);

                    if(pCotecna.Count > 0)
                    {
                        //const int ROWS_FIRST = 1;
                        const int ROWS_START = 1;
                        int Filas = pCotecna.Count + ROWS_START;
                        int iRow = 1;

                        var oWB = new cxExcel.XLWorkbook();

                        List<ArchivoAduana> _list = new List<ArchivoAduana>();

                        string nombreSheet = null;

                        
                            nombreSheet ="MANI_" + iLst.a_manifiesto + iLst.n_manifiesto.ToString();

                            var oSheet = oWB.Worksheets.Add(nombreSheet);

                            string _rango = null;

                            _rango = "O";

                            oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Font.Bold = true;
                            oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;

                            oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                            oSheet.Range("A1", string.Format("{0}1", _rango)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                        oSheet.Range("A2", string.Format("{0}", "E" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                        oSheet.Range("A2", string.Format("{0}", "E" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                        oSheet.Range("G2", string.Format("{0}", "H" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                        oSheet.Range("G2", string.Format("{0}", "H" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                        oSheet.Range("I2", string.Format("{0}", "I" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                        oSheet.Range("I2", string.Format("{0}", "I" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Right);


                        oSheet.Range("J2", string.Format("{0}", "K" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                        oSheet.Range("J2", string.Format("{0}", "K" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                        oSheet.Range("L2", string.Format("{0}", "M" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                        oSheet.Range("L2", string.Format("{0}", "M" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Left);

                        oSheet.Range("N2", string.Format("{0}", "O" + Filas)).Style.Alignment.SetVertical(cxExcel.XLAlignmentVerticalValues.Center);
                        oSheet.Range("N2", string.Format("{0}", "O" + Filas)).Style.Alignment.SetHorizontal(cxExcel.XLAlignmentHorizontalValues.Center);

                        oSheet.Range("A1", string.Format("{0}", "O" + Filas)).Style.Font.FontSize = 8;

                        oSheet.Range("A1", string.Format("{0}", "O" + Filas)).Style.Font.FontName = "Calibri";

                        oSheet.Range("E2", string.Concat("E", Filas)).Style.NumberFormat.SetFormat("@");

                        oSheet.Range("C2", string.Concat("C", Filas)).Style.NumberFormat.SetFormat("@");

                        var oRng = oSheet.Range("A1", string.Concat(string.Format("{0}", _rango), Filas));

                        oRng.Style.Border.DiagonalDown = false;
                        oRng.Style.Border.DiagonalUp = false;
                        oRng.Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                        oRng.Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;


                        oRng.Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oRng.Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Bottom;
                        oRng.Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                        oRng.Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);


                        //Format A1:D1 as bold, vertical alignment = center.
                        oSheet.Cell(ROWS_START, 1).Value = "No.";
                            oSheet.Cell(ROWS_START, 2).Value = "TIPO DECLARACION";
                            oSheet.Cell(ROWS_START, 3).Value = "MANIFIESTO";
                            oSheet.Cell(ROWS_START, 4).Value = "CONSIGNATARIO";
                            oSheet.Cell(ROWS_START, 5).Value = "NUMERO BL";
                            oSheet.Cell(ROWS_START, 6).Value = "DESCRIPCIÓN DE MERCADERÍA";
                            oSheet.Cell(ROWS_START, 7).Value = "BULTOS";
                            oSheet.Cell(ROWS_START, 8).Value = "EMBALAJE";
                            oSheet.Cell(ROWS_START, 9).Value = "PESO EN KG";
                            oSheet.Cell(ROWS_START, 10).Value = "CONTENEDOR";
                            oSheet.Cell(ROWS_START, 11).Value = "EMPRESA";
                            oSheet.Cell(ROWS_START, 12).Value = "PAIS DE PROCEDENCIA";
                            oSheet.Cell(ROWS_START, 13).Value = "PAIS DE DESTINO";
                            oSheet.Cell(ROWS_START, 14).Value = "CONDICION";
                            oSheet.Cell(ROWS_START, 15).Value = "TIPO BL";

                            

                            oSheet.Column(1).Width = 6;
                            oSheet.Column(2).Width = 17;
                            oSheet.Column(3).Width = 9;
                            oSheet.Column(4).Width = 60;
                            oSheet.Column(5).Width = 15;
                            oSheet.Column(6).Width = 70;
                            oSheet.Column(7).Width = 6;
                            oSheet.Column(8).Width = 8;
                            oSheet.Column(9).Width = 9;
                            oSheet.Column(10).Width = 14;
                            oSheet.Column(11).Width = 50;
                            oSheet.Column(12).Width = 20;
                            oSheet.Column(13).Width = 15;
                            oSheet.Column(14).Width = 9;
                            oSheet.Column(15).Width = 6;

                            foreach (var item in pCotecna)
                            {
                                int iCurrent = ROWS_START + iRow;

                                oSheet.Cell(iCurrent, 1).Value = item.c_correlativo;
                                oSheet.Cell(iCurrent, 2).Value = item.tipoDecla;
                                oSheet.Cell(iCurrent, 3).Value = item.a_manifiesto;
                                oSheet.Cell(iCurrent, 4).Value = item.s_consignatario;
                                oSheet.Cell(iCurrent, 5).Value = item.n_BL;
                                oSheet.Cell(iCurrent, 6).Value = item.s_comodity;
                                oSheet.Cell(iCurrent, 7).Value = item.c_paquete;
                                oSheet.Cell(iCurrent, 8).Value = item.c_embalaje;
                                oSheet.Cell(iCurrent, 9).Value = item.v_peso;
                                oSheet.Cell(iCurrent, 10).Value = item.n_contenedor;
                                oSheet.Cell(iCurrent, 11).Value = item.d_cliente;
                                oSheet.Cell(iCurrent, 12).Value = item.c_pais_origen;
                                oSheet.Cell(iCurrent, 13).Value = item.c_pais_destino;
                                oSheet.Cell(iCurrent, 14).Value = item.c_condicion;
                                oSheet.Cell(iCurrent, 15).Value = item.c_tipo_bl;

                                iRow = iRow + 1;
                            }

                            

                        string _nombre = string.Concat("COTEC_", d_buque.Replace(" ", "_"), "_", c_llegada, ".xlsx");

                        string fullPathCO = ConfigurationManager.AppSettings["fullPathCO"];

                        string _f7save = string.Format("{0}{1}", fullPathCO, "COTEC_" + d_buque + "_" + iLst.c_navi_corto + "_" + f_corto + ".xlsx");

                        if (File.Exists(_f7save))
                        {
                            File.Delete(_f7save);
                        }

                        oWB.SaveAs(_f7save);
                        //FileUpload FileUpload1 = new FileUpload();
                        //FileUpload1.SaveAs(_f7save);



                    }
                }

                // Crear zip para descarga
                using (ZipFile zip = new ZipFile())
                {
                    string fullPathCO = ConfigurationManager.AppSettings["fullPathCO"];
                    string[] filePaths = Directory.GetFiles(fullPathCO);
                    
                    foreach (string row in filePaths)
                    {

                        zip.AddFile(row);
                    }

                    
                    

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("{0}.zip", d_buque + "_" + DateTime.Now.ToString("ddMMyy-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);

                    string[] filePaths1 = Directory.GetFiles(fullPathCO);

                    foreach (string iDel in filePaths1)
                    {
                        if (File.Exists(iDel))
                            File.Delete(iDel);
                    }
                    Response.End();
                }


            }



        }

        private void insertMani(List<string> pListaNo, List<string> pRespuesta, ref int _resulDeta, int b_sidunea, ref string _mensaje, int n_manifiesto, string a_manifiesto)
        {
            string resul = ResulNavieraDAL.EliminarManifiesto(DBComun.Estado.verdadero, n_manifiesto, a_manifiesto, b_sidunea);

            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false);
            xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
            xmlWriterSettings.Indent = true;

            XmlWriter xml = XmlWriter.Create(memoryStream, xmlWriterSettings);
            
            string _Aduana = null;

            xml.WriteStartDocument();

            xml.WriteStartElement("MDS4");

            xml.WriteElementString("CAR_REG_YEAR", a_manifiesto);
            xml.WriteElementString("KEY_CUO", "02");
            xml.WriteElementString("CAR_REG_NBER", n_manifiesto.ToString());

            xml.WriteEndDocument();
            xml.Flush();
            xml.Close();

            //Generar XML para enviar parametros al servicio.
            _Aduana = Encoding.UTF8.GetString(memoryStream.ToArray());

            XmlDocument doc = new XmlDocument();

            if (b_sidunea == 0)
            {
                CEPAService.WSManifiestoCEPAClient _proxy = new CEPAService.WSManifiestoCEPAClient();
                string s = EscapeXMLValue(_proxy.getContenedorData(_Aduana));
                //s = UnescapeXMLValue(s);
                doc.LoadXml(s);
            }
            else
            {
                CepaSW.WSManifiestoCEPAClient _proxy = new CepaSW.WSManifiestoCEPAClient();
                string _user = ConfigurationManager.AppSettings["userSidunea"]; 
                string _psw = ConfigurationManager.AppSettings["pswSidunea"];

                

                _proxy.ClientCredentials.UserName.UserName = _user;
                _proxy.ClientCredentials.UserName.Password = _psw;

                _mensaje = _proxy.getContenedorData(_Aduana);
                if (_mensaje.Substring(0, 1) == "0")
                    pRespuesta.Add("PASO 3 de 4: Validación ADUANA: EL MANIFIESTO # " + string.Concat(a_manifiesto, "-", n_manifiesto) + " NO PRODUJO RESULTADOS");
                else
                    doc.LoadXml(_mensaje);
            }

            if (doc.ChildNodes.Count > 0)
            {
                XmlNodeList listEnca = doc.SelectNodes("MdsParts/MDS4");

                XmlNode xEnca;

                xEnca = listEnca.Item(0);

                string _amani = xEnca.SelectSingleNode("CAR_REG_YEAR").InnerText;
                string _nmani = xEnca.SelectSingleNode("CAR_REG_NBER").InnerText;

                XmlNodeList listaCntres = doc.SelectNodes("MdsParts/MDS4/MDS5");

                XmlNode unContenedor;

                List<ArchivoAduanaValid> pGuarda = new List<ArchivoAduanaValid>();

                //Contenedore devueltos por ADUANA
                if (listaCntres.Count > 0)
                {

                    for (int i = 0; i < listaCntres.Count; i++)
                    {
                        unContenedor = listaCntres.Item(i);

                        string _contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText;


                        ArchivoAduanaValid validAduana = new ArchivoAduanaValid
                        {
                            IdValid = -1,
                            n_contenedor = unContenedor.SelectSingleNode("CAR_CTN_IDENT").InnerText.Replace("-", ""),
                            n_manifiesto = Convert.ToInt32(_nmani),
                            n_BL = unContenedor.SelectSingleNode("KEY_BOL_REF").InnerText,
                            a_mani = _amani,
                            c_tipo_bl = unContenedor.SelectSingleNode("CARBOL_TYP_COD").InnerText,
                            b_sidunea = b_sidunea,
                            c_tamaño = unContenedor.SelectSingleNode("CAR_CTN_TYP").InnerText,
                            s_agencia = unContenedor.SelectSingleNode("CAR_CAR_NAM").InnerText,
                            v_peso = Convert.ToDouble(unContenedor.SelectSingleNode("CAR_CTN_GWG").InnerText),
                            c_paquete = Convert.ToInt32(unContenedor.SelectSingleNode("CAR_CTN_NBR").InnerText),
                            c_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_COD").InnerText,
                            d_embalaje = unContenedor.SelectSingleNode("CARBOL_PCK_NAM").InnerText,
                            c_pais_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(0, 2),
                            d_puerto_origen = unContenedor.SelectSingleNode("CARBOL_DEP_COD").InnerText.Substring(2, 3),
                            c_pais_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(0, 2),
                            d_puerto_destino = unContenedor.SelectSingleNode("CARBOL_DEST_COD").InnerText.Substring(2, 3)
                        };

                        //Almacenar manifiesto devuelto por aduana
                        _resulDeta = Convert.ToInt32(DetaNavieraDAL.AlmacenarValid(validAduana, DBComun.Estado.verdadero));

                        if (_resulDeta == 2)
                            pListaNo.Add(validAduana.n_contenedor);
                    }


                }
            }
        }

        private void Cargar()
        {
            EncaBuqueBL _encaBL = new EncaBuqueBL();

            GridView1.DataSource = DocBuqueLINQ.getCotecnaHeader();
            GridView1.DataBind();

            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
        }
        public void RegisterPostBackControl()
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                Button lnkFull = row.FindControl("lnkOpe") as Button;
                ScriptManager current = ScriptManager.GetCurrent(Page);
                if (current != null)
                    current.RegisterPostBackControl(lnkFull);
            }
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Cargar();
            }
            catch (Exception ex)
            {
                Response.Write("<script>bootbox.alert('" + ex.Message + "');</script>");
            }
        }
    }
}