using CEPA.CCO.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using CEPA.CCO.DAL;
using cxExcel = ClosedXML.Excel;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfAlertEstadia : System.Web.UI.Page
    {
        enum Tipo
        {
            VACIO,
            IMPORTACION,
            EXPORTACION
        }

        public string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        public bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grvDatos.DataSource = null;
                grvDatos.DataBind();

                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "ALERTA! Para obtener un inventario de todos los contenedores ubicados en puerto solo dar clic en CONSULTAR o ingresar directamente el contenedor a buscar" + "');", true);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            var oWB = new cxExcel.XLWorkbook();

            List<EstadiaConte> pList = new List<EstadiaConte>();

            grvDatos.DataSource = pList;
            grvDatos.DataBind();

            string c_contenedor = txtBuscar.Text.Trim().TrimStart().TrimEnd().ToUpper().Replace("-", "");

            

            if (string.IsNullOrEmpty(c_contenedor))
                c_contenedor = "ALL";
            else
            {
                if (c_contenedor.Length != 11)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('El # de contenedor debe tener 11 caracteres');", true);
                    return;
                }

                if (c_contenedor.Length == 11)
                {
                    string valida = DetaNavieraDAL.ValidaContenedor(DBComun.Estado.verdadero, txtBuscar.Text.Trim().TrimEnd().TrimStart(), DBComun.TipoBD.SqlServer);

                    if (valida != "VALIDO")
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('El # de contenedor ingresado no es válido');", true);
                        return;
                    }
                }

            }

            string c_naviera = Session["c_naviera"].ToString();

            if (c_naviera == "289" || c_naviera == "219" || c_naviera == "11" || c_naviera == "216" || c_naviera == "745")
            {
                c_naviera = "ALL";
            }

            if (c_contenedor == "ALL")
            {
                pList = GetContenedor(c_contenedor, c_naviera);

                if (pList.Count > 0)
                {
                    foreach (var iTipo in Enum.GetNames(typeof(Tipo)))
                    {
                        int c_correlativo = 1;
                        var query = pList.Where(x => x.Categoria.Trim().TrimEnd().TrimStart() == iTipo);
                        int d_lineas = 0;
                        if (query.Count() > 0)
                        {
                            int ROWS_START = 6;
                            int iRow = 1;
                            d_lineas = query.Count();

                            var oSheet = oWB.Worksheets.Add(iTipo.ToString());

                            string _rango = null;

                            if (iTipo.ToString() == "VACIO")
                                _rango = "M";
                            else
                                _rango = "L";

                            oSheet.Range("A1", _rango + "1").Merge();
                            oSheet.Range("A1", _rango + "1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                            oSheet.Range("A1", _rango + "1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;


                            oSheet.Cell("A1").Value = "PUERTO DE ACAJUTLA - INFORME DE CONTENEDORES UBICADOS EN PATIO, ORDENADOS POR TIEMPO DE ESTADIA.";
                            oSheet.Cell("A1").Style.Font.Bold = true;
                            oSheet.Cell("A1").Style.Font.FontSize = 7.5;
                            oSheet.Cell("A1").Style.Font.FontName = "Arial";
                            oSheet.Cell("A1").Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(128, 128, 0));
                            oSheet.Row(1).Height = 20;


                            oSheet.Range("A5", _rango + "5").Merge();
                            oSheet.Range("A5", _rango + "5").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                            oSheet.Range("A5", _rango + "5").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                            oSheet.Cell("A5").Value = "CONTENEDORES " + iTipo.ToString();
                            oSheet.Cell("A5").Style.Font.Bold = true;
                            oSheet.Cell("A5").Style.Font.FontSize = 7.5;
                            oSheet.Cell("A5").Style.Font.FontName = "Arial";
                            oSheet.Cell("A5").Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(0, 0, 128));
                            oSheet.Row(5).Height = 25;


                            oSheet.Cell(ROWS_START, 1).Value = "No.";
                            oSheet.Cell(ROWS_START, 2).Value = "AGENCIA";
                            oSheet.Cell(ROWS_START, 3).Value = "MANIFIESTO";
                            oSheet.Cell(ROWS_START, 4).Value = "CONTENEDOR";
                            oSheet.Cell(ROWS_START, 5).Value = "TAMAÑO/TIPO";
                            oSheet.Cell(ROWS_START, 6).Value = "CONDICION";
                            oSheet.Cell(ROWS_START, 7).Value = "INGRESO";
                            if (iTipo == "VACIO")
                            {
                                oSheet.Cell(ROWS_START, 8).Value = "F. VACIADO";
                                oSheet.Cell(ROWS_START, 9).Value = "ESTADIA (Días)";
                                oSheet.Cell(ROWS_START, 10).Value = "UBICACION";
                                oSheet.Cell(ROWS_START, 11).Value = "OBSERVACIONES";
                                oSheet.Cell(ROWS_START, 12).Value = "CONSIGNATARIO";
                                oSheet.Cell(ROWS_START, 13).Value = "ESTADIA BODEGA #3";
                            }
                            else
                            {
                                oSheet.Cell(ROWS_START, 8).Value = "ESTADIA (Días)";
                                oSheet.Cell(ROWS_START, 9).Value = "UBICACION";
                                oSheet.Cell(ROWS_START, 10).Value = "OBSERVACIONES";
                                oSheet.Cell(ROWS_START, 11).Value = "CONSIGNATARIO";
                                oSheet.Cell(ROWS_START, 12).Value = "ESTADIA BODEGA #3";
                            }

                            //BORDES TABLA
                            oSheet.Range("A6", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                            oSheet.Range("A6", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                            oSheet.Range("A6", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                            oSheet.Range("A6", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                            // CABECERA TABLA BORDES Y FONDO
                            oSheet.Range("A6", string.Concat(_rango, ROWS_START)).Style.Font.Bold = true;
                            oSheet.Range("A6", string.Concat(_rango, ROWS_START)).Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                            oSheet.Range("A6", string.Concat(_rango, ROWS_START)).Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(0, 0, 128));
                            oSheet.Range("A6", string.Concat(_rango, d_lineas + ROWS_START)).Style.Font.FontSize = 7.5;
                            oSheet.Range("A6", string.Concat(_rango, d_lineas + ROWS_START)).Style.Font.FontName = "Arial";
                            oSheet.Range("A6", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                            oSheet.Range("A6", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                            if (iTipo.ToString() == "VACIO")
                            {
                                oSheet.Range("J7", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                                oSheet.Range("J7", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                            }
                            else
                            {
                                oSheet.Range("I7", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                                oSheet.Range("I7", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;
                            }

                            oSheet.Column(1).Width = 3;
                            oSheet.Column(2).Width = 30;
                            oSheet.Column(3).Width = 14;
                            oSheet.Column(4).Width = 13;
                            oSheet.Column(5).Width = 12;
                            oSheet.Column(6).Width = 12;
                            oSheet.Column(7).Width = 15;
                            if (iTipo == "VACIO")
                            {
                                oSheet.Column(8).Width = 15;
                                oSheet.Column(9).Width = 13;
                                oSheet.Column(10).Width = 35;
                                oSheet.Column(11).Width = 57;
                                oSheet.Column(12).Width = 40;
                                oSheet.Column(13).Width = 20;
                            }
                            else
                            {
                                oSheet.Column(8).Width = 13;
                                oSheet.Column(9).Width = 35;
                                oSheet.Column(10).Width = 57;
                                oSheet.Column(11).Width = 40;
                                oSheet.Column(12).Width = 20;
                            }

                            foreach (var item in query)
                            {

                                int iCurrent = ROWS_START + iRow;
                                oSheet.Row(iCurrent).Height = 40;
                                oSheet.Cell(iCurrent, 1).Value = c_correlativo;
                                oSheet.Cell(iCurrent, 2).Value = SanitizeXmlString(item.Agencia);
                                oSheet.Cell(iCurrent, 3).Value = SanitizeXmlString(item.Manifiesto);
                                oSheet.Cell(iCurrent, 4).Value = SanitizeXmlString(item.Contenedor);
                                oSheet.Cell(iCurrent, 5).Value = SanitizeXmlString(item.Tipo);
                                oSheet.Cell(iCurrent, 6).Value = SanitizeXmlString(item.Condicion);
                                oSheet.Cell(iCurrent, 7).Value = item.Ingreso == DateTime.MinValue ? "/ / : : " : item.Ingreso.ToString("dd/MM/yyyy hh:mm");
                                if (iTipo.ToString() == "VACIO")
                                {
                                    oSheet.Cell(iCurrent, 8).Value = item.Fec_vacio == DateTime.MinValue ? "/ / : : " : item.Fec_vacio.ToString("dd/MM/yyyy hh:mm");
                                    oSheet.Cell(iCurrent, 9).Value = item.Estadia;
                                    oSheet.Cell(iCurrent, 10).Value = SanitizeXmlString(item.Sitio);
                                    oSheet.Cell(iCurrent, 11).Value = string.IsNullOrEmpty(item.Observa) ? "" : SanitizeXmlString(item.Observa);
                                    oSheet.Cell(iCurrent, 12).Value = string.IsNullOrEmpty(item.Cliente) ? "" : SanitizeXmlString(item.Cliente);
                                    oSheet.Cell(iCurrent, 13).Value = string.IsNullOrEmpty(item.Estb3) ? "" : SanitizeXmlString(item.Estb3);
                                }
                                else
                                {
                                    oSheet.Cell(iCurrent, 8).Value = item.Estadia;
                                    oSheet.Cell(iCurrent, 9).Value = SanitizeXmlString(item.Sitio);
                                    oSheet.Cell(iCurrent, 10).Value = string.IsNullOrEmpty(item.Observa) ? "" : SanitizeXmlString(item.Observa);
                                    oSheet.Cell(iCurrent, 11).Value = string.IsNullOrEmpty(item.Cliente) ? "" : SanitizeXmlString(item.Cliente);
                                    oSheet.Cell(iCurrent, 12).Value = string.IsNullOrEmpty(item.Estb3) ? "" : SanitizeXmlString(item.Estb3);
                                }


                                iRow = iRow + 1;
                                c_correlativo = c_correlativo + 1;
                            }

                            int _pie = (d_lineas + ROWS_START);

                            oSheet.Range("G7", string.Concat("G", _pie.ToString())).Style.NumberFormat.SetFormat("dd/mm/yyyy hh:mm");

                            if (iTipo.ToString() == "VACIO")
                            {
                                oSheet.Range("H7", string.Concat("H", _pie.ToString())).Style.NumberFormat.SetFormat("dd/mm/yyyy hh:mm");
                            }

                            _pie = (d_lineas + ROWS_START) + 2;

                            oSheet.Range("A" + _pie.ToString(), _rango + _pie.ToString()).Merge();
                            oSheet.Range("A" + _pie.ToString(), _rango + _pie.ToString()).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                            oSheet.Range("A" + _pie.ToString(), _rango + _pie.ToString()).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                            oSheet.Cell("A" + _pie.ToString()).Value = "Mensaje generado automáticamente, gratuito y no genera responsabilidades para CEPA  ";
                            oSheet.Cell("A" + _pie.ToString()).Style.Font.Bold = true;
                            oSheet.Cell("A" + _pie.ToString()).Style.Font.FontSize = 7.5;
                            oSheet.Cell("A" + _pie.ToString()).Style.Font.FontName = "Arial";
                            oSheet.Cell("A" + _pie.ToString()).Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(0, 0, 128));



                            if (iTipo.ToString() == "VACIO")
                            {
                                oSheet.Column(13).AdjustToContents();
                            }
                            else
                            {
                                oSheet.Column(12).AdjustToContents();
                            }

                            oSheet.Range("A7", _rango + d_lineas.ToString()).Style.Alignment.WrapText = true;
                        }
                    }
                    string f_actual = DateTime.Now.ToString("ddMMyyhhmm");
                    string _nombre = string.Format("ESTADIA_{0}.xlsx", f_actual);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", _nombre));




                    //System.Threading.Thread.Sleep(10000);



                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        oWB.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Revisar información ya que no devuelve resultados su búsqueda');", true);
                }
            }
            else{
                pList = GetContenedor(c_contenedor, c_naviera);
                grvDatos.DataSource = pList;
                grvDatos.DataBind();

                //grvDatos.HeaderRow.Cells[0].Attributes["data-class"] = "expand";
                if (grvDatos.Rows.Count > 0)
                {
                    // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
                    grvDatos.HeaderRow.Cells[2].Attributes["data-hide"] = "phone";
                    grvDatos.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
                    grvDatos.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
                    //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

                    grvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                    grvDatos.FooterRow.Cells[0].Attributes["text-align"] = "center";
                    grvDatos.FooterRow.TableSection = TableRowSection.TableFooter;
                    //  ViewState["EmployeeList"] = GridView1.DataSource;
                }
                else
                {

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Los datos ingresados no devuelven resultados, verifique la información e intente nuevamente.');", true);
                    return;
                }
            }
        }

        public static List<EstadiaConte> GetContenedor(string c_contenedor, string c_naviera)
        {
            List<EstadiaConte> _contenedores = new List<EstadiaConte>();
            string apiUrl = "http://138.219.156.210:83/api/Ejecutar/?Consulta=";
            Procedure proceso = new Procedure();
            proceso.NBase = "CONTENEDORES";
            proceso.Procedimiento = "sqlconteubica"; // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
            proceso.Parametros = new List<Parametros>();
            proceso.Parametros.Add(new Parametros { nombre = "navsadfi", valor = c_naviera });
            proceso.Parametros.Add(new Parametros { nombre = "_contenedor", valor = c_contenedor });
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<EstadiaConte> Conectar(List<EstadiaConte> _contenedores, string apiUrl)
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
                        _contenedores = tabla.ToList<EstadiaConte>();
                    }
                }
            }
            return _contenedores;
        }

        protected void grvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string Cat = null;
                if (e.Row.RowType != DataControlRowType.DataRow)
                    return;

                if (ArchivoBookingDAL.isFecha(e.Row.Cells[3].Text) == true)
                {
                    if (Convert.ToDateTime(e.Row.Cells[3].Text) == DateTime.MinValue)
                    { }
                    else
                    {
                        e.Row.Cells[3].Text = "/ / : : ";
                    }                   
                }

                HiddenField hCat = (HiddenField)e.Row.FindControl("hCat") as HiddenField;
                Cat = hCat.Value;

                HiddenField hCon = (HiddenField)e.Row.FindControl("hCon") as HiddenField;
                Label lblCon = (Label)e.Row.FindControl("lblCondicion") as Label;

                if (Cat != "VACIO")
                {
                    lblCon.Text = hCon.Value + " / " + Cat;
                }
                else
                {
                    lblCon.Text = hCon.Value;
                }

                if (hCat.Value == "VACIO")
                {
                    //e.Row.Cells[4].Visible = true;
                    //grvDatos.HeaderRow.Cells[4].Visible = true;
                    //grvDatos.Columns[4].Visible = true;

                    Label lblFec = (Label)e.Row.FindControl("lblVacio") as Label;

                    if (ArchivoBookingDAL.isFecha(lblFec.Text) == true)
                    {
                        if (Convert.ToDateTime(lblFec.Text) == DateTime.MinValue)
                        {
                            lblFec.Text = "/ / : : ";
                        }
                        else
                        {
                            
                        }

                    }
                }
                else
                {
                    Label lblFec = (Label)e.Row.FindControl("lblVacio") as Label;

                    if (ArchivoBookingDAL.isFecha(lblFec.Text) == true)
                    {
                        if (Convert.ToDateTime(lblFec.Text) == DateTime.MinValue)
                        {
                            lblFec.Text = "N/A";
                        }
                        else
                        {

                        }

                    }
                }
            }
            catch(Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Información no devuelve resultados verificar y volver a intentar : "+ ex.Message +"');", true);
                return;
            }
        }
    }
}
