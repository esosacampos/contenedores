using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using cxExcel = ClosedXML.Excel;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfAlertSalidas : System.Web.UI.Page
    {
        private static readonly DateTime FIRST_GOOD_DATE = new DateTime(1899, 12, 30);
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
            DataTable dt = new DataTable();
            if (!IsPostBack)
            {
                grvDatos.DataSource = dt;
                grvDatos.DataBind();
            }
            else
            {
                grvDatos.DataSource = dt;
                grvDatos.DataBind();
            }
        }

        public static List<EntLlenos> getEntLLenos(string c_contenedor, string c_naviera, string f_corta)
        {
            List<EntLlenos> _contenedores = new List<EntLlenos>();
            string apiUrl = "http://138.219.156.210:83/api/Ejecutar/?Consulta=";
            Procedure proceso = new Procedure
            {
                NBase = "CONTENEDORES",
                Procedimiento = "sqlentllenos", // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
                Parametros = new List<Parametros>()
            };
            proceso.Parametros.Add(new Parametros { nombre = "fcorta", valor = f_corta });
            proceso.Parametros.Add(new Parametros { nombre = "navsadfi", valor = c_naviera });
            proceso.Parametros.Add(new Parametros { nombre = "ncontenedor", valor = c_contenedor });
            
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<EntLlenos> Conectar(List<EntLlenos> _contenedores, string apiUrl)
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
                        _contenedores = tabla.ToList<EntLlenos>();
                    }
                }
            }
            return _contenedores;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            var oWB = new cxExcel.XLWorkbook();

            List<EntLlenos> pList = new List<EntLlenos>();

            grvDatos.DataSource = pList;
            grvDatos.DataBind();

            string c_contenedor = txtBuscar.Text.Trim().TrimStart().TrimEnd().ToUpper().Replace("-", "");

            string f_corta = txtDOB.Text.Replace("/","-");

            if (string.IsNullOrEmpty(f_corta))
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Debe seleccionar una fecha a consultar');", true);
                return;
            }


            if (string.IsNullOrEmpty(c_contenedor))
            {
                c_contenedor = "ALL";
            }
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
                pList = getEntLLenos(c_contenedor, c_naviera, f_corta);

                if (pList.Count > 0)
                {
                    int c_correlativo = 1;
                    int d_lineas = 0;

                    int ROWS_START = 4;
                    int iRow = 1;

                    d_lineas = pList.Count;

                    var oSheet = oWB.Worksheets.Add("Salidas");

                    string _rango = "W";

                    oSheet.Range("A1", _rango + "1").Merge();
                    oSheet.Range("A1", _rango + "1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A1", _rango + "1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                    oSheet.Cell("A1").Value = "PUERTO DE ACAJUTLA - INFORME DE CONFIRMACION DE SALIDAS POR LA PUERTA N° 1. ";
                    oSheet.Cell("A1").Style.Font.Bold = true;
                    oSheet.Cell("A1").Style.Font.FontSize = 7.5;
                    oSheet.Cell("A1").Style.Font.FontName = "Arial";
                    oSheet.Cell("A1").Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(128, 128, 0));
                    oSheet.Row(1).Height = 20;

                    oSheet.Range("A2", _rango + "2").Merge();
                    oSheet.Range("A2", _rango + "2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A2", _rango + "2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                    oSheet.Cell("A2").Value = "FECHA GENERACIÓN DE INFORME: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"); 
                    oSheet.Cell("A2").Style.Font.Bold = true;
                    oSheet.Cell("A2").Style.Font.FontSize = 7.5;
                    oSheet.Cell("A2").Style.Font.FontName = "Arial";
                    oSheet.Cell("A2").Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(128, 128, 0));
                    oSheet.Row(2).Height = 20;

                    oSheet.Cell(ROWS_START, 1).Value = "No.";
                    oSheet.Cell(ROWS_START, 2).Value = "AGENCIA";
                    oSheet.Cell(ROWS_START, 3).Value = "MANIFIESTO";
                    oSheet.Cell(ROWS_START, 4).Value = "N° SALIDA";
                    oSheet.Cell(ROWS_START, 5).Value = "CONTENEDOR";
                    oSheet.Cell(ROWS_START, 6).Value = "ESTADO";
                    oSheet.Cell(ROWS_START, 7).Value = "TAMAÑO";
                    oSheet.Cell(ROWS_START, 8).Value = "TIPO DESPACHO";
                    oSheet.Cell(ROWS_START, 9).Value = "FECHA/HORA ELAB. SALIDA DE CARGA (A)";
                    oSheet.Cell(ROWS_START, 10).Value = "FECHA/HORA INGRESO CASETA-PUERTA1";
                    oSheet.Cell(ROWS_START, 11).Value = "FECHA/HORA ASIGNA TURNO DE CARGA (B)";
                    oSheet.Cell(ROWS_START, 12).Value = "TIEMPO (B - A)";
                    oSheet.Cell(ROWS_START, 13).Value = "FECHA/HORA AUTORIZA. DE CARGA (C)";
                    oSheet.Cell(ROWS_START, 14).Value = "TIEMPO (C -  B)";
                    oSheet.Cell(ROWS_START, 15).Value = "FECHA/HORA SALIDA PUERTA N° 1 (D)";
                    oSheet.Cell(ROWS_START, 16).Value = "TIEMPO (D - A)";
                    oSheet.Cell(ROWS_START, 17).Value = "CHASIS";
                    oSheet.Cell(ROWS_START, 18).Value = "TRANSPORTE";
                    oSheet.Cell(ROWS_START, 19).Value = "NOTA(S) DE TARJA";
                    oSheet.Cell(ROWS_START, 20).Value = "POLIZA(S)";
                    oSheet.Cell(ROWS_START, 21).Value = "PESO MERCAD ENTREGADA (Kgs)";
                    oSheet.Cell(ROWS_START, 22).Value = "UBICACION DE DESPACHO";
                    oSheet.Cell(ROWS_START, 23).Value = "CONSIGNATARIO";
                    oSheet.Row(4).Height = 45;

                    //BORDES TABLA
                    oSheet.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                    oSheet.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                    oSheet.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                    oSheet.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);

                    // CABECERA TABLA BORDES Y FONDO
                    oSheet.Range("A4", string.Concat(_rango, ROWS_START)).Style.Font.Bold = true;
                    oSheet.Range("A4", string.Concat(_rango, ROWS_START)).Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                    oSheet.Range("A4", string.Concat(_rango, ROWS_START)).Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(0, 0, 128));
                    oSheet.Range("A4", string.Concat(_rango, d_lineas + ROWS_START)).Style.Font.FontSize = 7.5;
                    oSheet.Range("A4", string.Concat(_rango, d_lineas + ROWS_START)).Style.Font.FontName = "Arial";
                    oSheet.Range("A4", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A4", string.Concat(_rango, d_lineas + ROWS_START)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                    oSheet.Column(1).Width = 3;
                    oSheet.Column(2).Width = 30;
                    oSheet.Column(3).Width = 14;
                    oSheet.Column(4).Width = 13;
                    oSheet.Column(5).Width = 13;
                    oSheet.Column(6).Width = 11;
                    oSheet.Column(7).Width = 13;
                    oSheet.Column(8).Width = 17;
                    oSheet.Column(9).Width = 20;
                    oSheet.Column(10).Width = 20;
                    oSheet.Column(11).Width = 20;
                    oSheet.Column(12).Width = 14;
                    oSheet.Column(13).Width = 22;
                    oSheet.Column(14).Width = 14;
                    oSheet.Column(15).Width = 20;
                    oSheet.Column(16).Width = 14;
                    oSheet.Column(17).Width = 15;
                    oSheet.Column(18).Width = 18;
                    oSheet.Column(19).Width = 16;
                    oSheet.Column(20).Width = 14;
                    oSheet.Column(21).Width = 16;
                    oSheet.Column(22).Width = 20;
                    oSheet.Column(23).Width = 45;

                    DateTime? f_despacho;
                    DateTime? f_Ing_Puerta1;
                    DateTime? f_Caseta_Ing;
                    DateTime? f_Caseta_Sal;
                    DateTime? f_Salidap1;

                    foreach (var item in pList)
                    {

                        if (item.Fec_Caseta_Ing?.ToString("dd/MM/yyyy").Substring(0, 2) == "30")
                            f_Caseta_Ing = DateTime.MinValue;
                        else
                            f_Caseta_Ing = item.Fec_Caseta_Ing;

                        if(item.Despacho?.ToString("dd/MM/yyyy").Substring(0, 2) == "30")
                            f_despacho = DateTime.MinValue;
                        else
                            f_despacho = item.Despacho;

                        if (item.Fec_Caseta_Sal?.ToString("dd/MM/yyyy").Substring(0, 2) == "30")
                            f_Caseta_Sal = DateTime.MinValue;
                        else
                            f_Caseta_Sal = item.Fec_Caseta_Sal;

                        if (item.Fec_Ing_Puerta1?.ToString("dd/MM/yyyy").Substring(0, 2) == "30")
                            f_Ing_Puerta1 = DateTime.MinValue;
                        else
                            f_Ing_Puerta1 = item.Fec_Ing_Puerta1;

                        if (item.Fecha_Salidap1?.ToString("dd/MM/yyyy").Substring(0, 2) == "30")
                            f_Salidap1 = DateTime.MinValue;
                        else
                            f_Salidap1 = item.Fecha_Salidap1;


                        int iCurrent = ROWS_START + iRow;
                        oSheet.Row(iCurrent).Height = 45;

                        oSheet.Cell(iCurrent, 1).Value = c_correlativo;
                        oSheet.Cell(iCurrent, 2).Value = SanitizeXmlString(item.Navcorto);
                        oSheet.Cell(iCurrent, 3).Value = SanitizeXmlString(item.Manifiesto);
                        oSheet.Cell(iCurrent, 4).Value = item.Salida;
                        oSheet.Cell(iCurrent, 5).Value = item.Contenedor;
                        oSheet.Cell(iCurrent, 6).Value = item.Condicion;
                        oSheet.Cell(iCurrent, 7).Value = SanitizeXmlString(item.Tamaño);
                        oSheet.Cell(iCurrent, 8).Value = item.Tipo_Despacho;
                        oSheet.Cell(iCurrent, 9).Value = f_despacho == DateTime.MinValue ? "/ / : : " : item.Despacho?.ToString("dd/MM/yyyy hh:mm");
                        oSheet.Cell(iCurrent, 10).Value = f_Ing_Puerta1 == DateTime.MinValue ? "/ / : : " : item.Fec_Ing_Puerta1?.ToString("dd/MM/yyyy hh:mm");
                        oSheet.Cell(iCurrent, 11).Value = f_Caseta_Ing == DateTime.MinValue ? "/ / : : " : item.Fec_Caseta_Ing?.ToString("dd/MM/yyyy hh:mm");
                        oSheet.Cell(iCurrent, 12).Value = item.Tiempoba;
                        oSheet.Cell(iCurrent, 13).Value = f_Caseta_Sal == DateTime.MinValue ? "/ / : : " : item.Fec_Caseta_Sal?.ToString("dd/MM/yyyy hh:mm");
                        oSheet.Cell(iCurrent, 14).Value = item.Tiempocb;
                        oSheet.Cell(iCurrent, 15).Value = f_Salidap1 == DateTime.MinValue ? "/ / : : " : item.Fecha_Salidap1?.ToString("dd/MM/yyyy hh:mm");
                        oSheet.Cell(iCurrent, 16).Value = item.Tiempoda;
                        oSheet.Cell(iCurrent, 17).Value = SanitizeXmlString(item.Chasis);
                        oSheet.Cell(iCurrent, 18).Value = SanitizeXmlString(item.Transporte);
                        oSheet.Cell(iCurrent, 19).Value = SanitizeXmlString(item.Tarja);
                        oSheet.Cell(iCurrent, 20).Value = SanitizeXmlString(item.Poliza);
                        oSheet.Cell(iCurrent, 21).Value = item.Peso_Entregado;
                        oSheet.Cell(iCurrent, 22).Value = SanitizeXmlString(item.Ubica);
                        oSheet.Cell(iCurrent, 23).Value = SanitizeXmlString(item.Nom_Consigna);

                        iRow = iRow + 1;
                        c_correlativo = c_correlativo + 1;
                    }

                    //DANDO FORMATO
                    oSheet.Range("U5", string.Concat("U", (ROWS_START + d_lineas) + 1)).Style.NumberFormat.SetFormat("#,##0.00");
                    oSheet.Range("A4", string.Concat(_rango, (ROWS_START + d_lineas) + 1)).Style.Alignment.WrapText = true;

                    int _pie = (d_lineas + ROWS_START);

                    _pie = (d_lineas + ROWS_START) + 2;

                    oSheet.Range("A" + _pie.ToString(), _rango + _pie.ToString()).Merge();
                    oSheet.Range("A" + _pie.ToString(), _rango + _pie.ToString()).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Range("A" + _pie.ToString(), _rango + _pie.ToString()).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                    oSheet.Cell("A" + _pie.ToString()).Value = "Mensaje generado automáticamente, gratuito y no genera responsabilidades para CEPA  ";
                    oSheet.Cell("A" + _pie.ToString()).Style.Font.Bold = true;
                    oSheet.Cell("A" + _pie.ToString()).Style.Font.FontSize = 7.5;
                    oSheet.Cell("A" + _pie.ToString()).Style.Font.FontName = "Arial";
                    oSheet.Cell("A" + _pie.ToString()).Style.Font.SetFontColor(cxExcel.XLColor.FromArgb(0, 0, 128));


                    string f_actual = DateTime.Now.ToString("ddMMyyhhmm");
                    string _nombre = string.Format("SALIDAS_{0}.xlsx", f_actual);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", _nombre));
                    

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

                     System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Los datos ingresados no devuelven resultados, verifique la información e intente nuevamente.');", true);
                     return;                    
                }

            }
            else
            {
                pList = getEntLLenos(c_contenedor, c_naviera, "ALL");
                grvDatos.DataSource = pList;
                grvDatos.DataBind();
                
                if (grvDatos.Rows.Count > 0)
                {
                    TableCellCollection cells = grvDatos.HeaderRow.Cells;
                    cells[0].Attributes.Add("data-class", "expand");
                    cells[2].Attributes.Add("data-hide", "phone");
                    cells[3].Attributes.Add("data-hide", "phone");
                    cells[4].Attributes.Add("data-hide", "phone,tablet");
                    cells[5].Attributes.Add("data-hide", "phone,tablet");
                    cells[6].Attributes.Add("data-hide", "phone,tablet");
                    cells[7].Attributes.Add("data-hide", "phone,tablet");
                    cells[8].Attributes.Add("data-hide", "phone,tablet");
                    cells[9].Attributes.Add("data-hide", "phone,tablet");
                    cells[10].Attributes.Add("data-hide", "phone,tablet");
                    cells[11].Attributes.Add("data-hide", "phone,tablet");
                    cells[12].Attributes.Add("data-hide", "phone, tablet");
                    cells[13].Attributes.Add("data-hide", "phone");
                    cells[14].Attributes.Add("data-hide", "phone,tablet");
                    cells[15].Attributes.Add("data-hide", "phone");
                    cells[16].Attributes.Add("data-hide", "phone,tablet");
                    cells[17].Attributes.Add("data-hide", "phone,tablet");
                    cells[18].Attributes.Add("data-hide", "phone,tablet");
                    cells[19].Attributes.Add("data-hide", "phone, tablet");
                    cells[20].Attributes.Add("data-hide", "phone, tablet");
                    cells[21].Attributes.Add("data-hide", "phone");
                                     

                    grvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

                    grvDatos.FooterRow.Cells[0].Attributes["text-align"] = "center";
                    grvDatos.FooterRow.TableSection = TableRowSection.TableFooter;

                }
                else
                {

                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Los datos ingresados no devuelven resultados, verifique la información e intente nuevamente. <br> * Verificar que la fecha indicada sea la correcta');", true);
                    return;
                }
            }
        }

        protected void grvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                DateTime? f_despacho;
                DateTime? f_Ing_Puerta1;
                DateTime? f_Caseta_Ing;
                DateTime? f_Caseta_Sal;
                DateTime? f_Salidap1;

                
                if (ArchivoBookingDAL.isFecha(e.Row.Cells[7].Text) == true)
                {
                    if (e.Row.Cells[7].Text.Substring(0, 2) == "30")
                        f_despacho = DateTime.MinValue;
                    else
                        f_despacho = Convert.ToDateTime(e.Row.Cells[7].Text);

                    if (f_despacho == DateTime.MinValue)
                    {
                        e.Row.Cells[7].Text = "/ / : : ";
                    }
                    else
                    {
                        
                    }
                }
                if (ArchivoBookingDAL.isFecha(e.Row.Cells[8].Text) == true)
                {
                    if (e.Row.Cells[8].Text.Substring(0, 2) == "30")
                        f_Ing_Puerta1 = DateTime.MinValue;
                    else
                        f_Ing_Puerta1 = Convert.ToDateTime(e.Row.Cells[8].Text);

                    if (f_Ing_Puerta1 == DateTime.MinValue)
                    {
                        e.Row.Cells[8].Text = "/ / : : ";
                    }
                    else
                    {

                    }
                }
                if (ArchivoBookingDAL.isFecha(e.Row.Cells[9].Text) == true)
                {
                    if (e.Row.Cells[9].Text.Substring(0, 2) == "30")
                        f_Caseta_Ing = DateTime.MinValue;
                    else
                        f_Caseta_Ing = Convert.ToDateTime(e.Row.Cells[9].Text);

                    if (f_Caseta_Ing == DateTime.MinValue)
                    {
                        e.Row.Cells[9].Text = "/ / : : ";
                    }
                    else
                    {

                    }
                }
                if (ArchivoBookingDAL.isFecha(e.Row.Cells[11].Text) == true)
                {
                    if (e.Row.Cells[11].Text.Substring(0, 2) == "30")
                        f_Caseta_Sal = DateTime.MinValue;
                    else
                        f_Caseta_Sal = Convert.ToDateTime(e.Row.Cells[11].Text);

                    if (f_Caseta_Sal == DateTime.MinValue)
                    {
                        e.Row.Cells[11].Text = "/ / : : ";
                    }
                    else
                    {

                    }
                }
                if (ArchivoBookingDAL.isFecha(e.Row.Cells[13].Text) == true)
                {
                    if (e.Row.Cells[13].Text.Substring(0, 2) == "30")
                        f_Salidap1 = DateTime.MinValue;
                    else
                        f_Salidap1 = Convert.ToDateTime(e.Row.Cells[13].Text);

                    if (f_Salidap1 == DateTime.MinValue)
                    {
                        e.Row.Cells[13].Text = "/ / : : ";
                    }
                    else
                    {

                    }
                }
                
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('Información no devuelve resultados verificar y volver a intentar : " + ex.Message + "');", true);
                return;
            }

        }
    }
}