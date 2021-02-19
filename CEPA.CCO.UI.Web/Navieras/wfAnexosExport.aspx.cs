using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using ClosedXML.Excel;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using cxExcel = ClosedXML.Excel;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfAnexosExport : System.Web.UI.Page
    {
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
            //ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.btnBuscar);
        }

        public static List<ExpConte> GetAnexosExport(string c_nul, string c_sadfi)
        {
            List<ExpConte> _contenedores = new List<ExpConte>();
            string apiUrl = WebConfigurationManager.AppSettings["apiFox"].ToString();
            Procedure proceso = new Procedure();
            proceso.NBase = "CONTENEDORES";
            proceso.Procedimiento = "wa_expconte"; // "contenedor_exp"; //"Sqlentllenos"; //contenedor_exp('NYKU3806160') //"lstsalidascarga";// ('NYKU3806160')
            proceso.Parametros = new List<Parametros>();
            proceso.Parametros.Add(new Parametros { nombre = "c_sadfi", valor = c_sadfi });
            proceso.Parametros.Add(new Parametros { nombre = "c_nul", valor = c_nul });
            string inputJson = JsonConvert.SerializeObject(proceso);
            apiUrl = apiUrl + inputJson;
            _contenedores = Conectar(_contenedores, apiUrl);
            return _contenedores;
        }

        private static List<ExpConte> Conectar(List<ExpConte> _contenedores, string apiUrl)
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

                        _contenedores = tabla.ToList<ExpConte>();

                    }
                }
            }
            return _contenedores;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Crear libro de Excel

            string c_factura = txtBuscar.Text;
            string c_cliente = Session["c_naviera"].ToString();
            //string c_cliente = "690";

            //if (c_cliente == "1541")
            //    c_cliente = "1515";


            List<AnexoExport> queryEnca = (from a in EncaBuqueDAL.getAnexoExport(DBComun.Estado.verdadero, c_factura, c_cliente)
                                           join b in DetaNavieraDAL.getNaviValida() on a.c_naviera equals b.c_cliente
                                           select new AnexoExport
                                           {
                                               c_nul = a.c_nul,
                                               d_buque = a.d_buque,
                                               f_arribo = a.f_arribo,
                                               d_naviera = b.c_navi
                                           }).ToList();

            if (queryEnca == null)
                new List<AnexoExport>();

            if (queryEnca.Count > 0)
            {

                List<ExpConte> pLstAnexo = new List<ExpConte>();

                string f_actual = DateTime.Now.ToString("dd/MM/yyyy");
                AnexoExport _anexoCls = new AnexoExport();

                foreach (var item in queryEnca)
                {

                    _anexoCls.c_nul = item.c_nul;
                    _anexoCls.d_buque = item.d_buque;
                    _anexoCls.f_arribo = item.f_arribo;
                    _anexoCls.d_naviera = item.d_naviera;

                    break;
                }

                pLstAnexo = GetAnexosExport(_anexoCls.c_nul, c_cliente).ToList();



                if (pLstAnexo.Count > 0)
                {


                    


                    string s_archivo = Server.MapPath("~/Anexo_export.xlsx");

                    var workbook = new XLWorkbook(s_archivo);

                    var oSheet = workbook.Worksheet(1);


                    int ROWS_START = 11;
                    int iRow = 1;

                    int d_lineas = pLstAnexo.Count;

                    //CABECERA 



                    // CABECERA INICIO
                    oSheet.Cell("A8").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Cell("A8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                    oSheet.Cell("A8").Value = "BUQUE: " + _anexoCls.c_nul + " - " + _anexoCls.d_buque;
                    oSheet.Cell("A8").Style.Font.Bold = true;


                    oSheet.Cell("E8").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Cell("E8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                    oSheet.Cell("E8").Value = "ARRIBO: " + _anexoCls.f_arribo.ToString("dd/MM/yyyy");
                    oSheet.Cell("E8").Style.Font.Bold = true;

                    oSheet.Cell("I8").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Cell("I8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                    oSheet.Cell("I8").Value = "AGENCIA: " + _anexoCls.d_naviera;
                    oSheet.Cell("I8").Style.Font.Bold = true;

                    oSheet.Cell("M8").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                    oSheet.Cell("M8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                    oSheet.Cell("M8").Value = "FECHA: " + f_actual;
                    oSheet.Cell("M8").Style.Font.Bold = true;

                   
                    //CABECERA FIN



                    //BORDES TABLA
                    oSheet.Range("A11", string.Concat("O", ((ROWS_START + d_lineas) - 1))).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                    oSheet.Range("A11", string.Concat("O", ((ROWS_START + d_lineas) - 1))).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                    oSheet.Range("A11", string.Concat("O", ((ROWS_START + d_lineas) - 1))).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                    oSheet.Range("A11", string.Concat("O", ((ROWS_START + d_lineas) - 1))).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);


                    foreach (var item in pLstAnexo)
                    {
                        int iCurrent = 10 + iRow;
                        oSheet.Row(iCurrent).Height = 25;
                        oSheet.Cell(iCurrent, 1).Value = item.Orden;
                        oSheet.Cell(iCurrent, 2).Value = SanitizeXmlString(item.Contenedor);
                        oSheet.Cell(iCurrent, 3).Value = item.Tara;
                        oSheet.Cell(iCurrent, 4).Value = item.Peso == 0 ? 0.00 : item.Peso;
                        oSheet.Cell(iCurrent, 5).Value = item.Tamaño;
                        oSheet.Cell(iCurrent, 6).Value = item.Condicion;
                        oSheet.Cell(iCurrent, 7).Value = item.Dreales;
                        if (item.Tamaño == "20")
                            oSheet.Cell(iCurrent, 8).Value = item.Dcobro;
                        else
                            oSheet.Cell(iCurrent, 8).Value = "";
                        if (item.Tamaño == "40")
                            oSheet.Cell(iCurrent, 9).Value = item.Dcobro;
                        else
                            oSheet.Cell(iCurrent, 9).Value = "";
                        if (item.Tamaño == "45")
                            oSheet.Cell(iCurrent, 10).Value = item.Dcobro;
                        else
                            oSheet.Cell(iCurrent, 10).Value = "";
                        if (item.Tamaño == "48")
                            oSheet.Cell(iCurrent, 11).Value = item.Dcobro;
                        else
                            oSheet.Cell(iCurrent, 11).Value = "";
                        oSheet.Cell(iCurrent, 12).Value = item.Fec_Vacio.ToString().Contains("1899") ? "/ / : : " : item.Fec_Vacio.ToString("dd/MM/yyyy hh:mm"); 
                        oSheet.Cell(iCurrent, 13).Value = item.Fec_Valle.ToString().Contains("1899") ? "/ / : : " : item.Fec_Valle.ToString("dd/MM/yyyy hh:mm");
                        oSheet.Cell(iCurrent, 14).Value = item.Fingresa;
                        oSheet.Cell(iCurrent, 15).Value = item.Fexporta;

                        iRow = iRow + 1;
                    }


                    string _nombre = string.Format("ANEXO_EXPORT_{0}_{1}.xlsx", c_factura, Convert.ToDateTime(f_actual).ToString("ddMMyyyyhhmm"));
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", _nombre));




                    //System.Threading.Thread.Sleep(10000);



                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        workbook.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }



                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('No se poseen registros para el # de factura ingresada, verifique si su número de factura es correcto.');", true);
                }

            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('No se poseen registros para el # de factura ingresada, verifique si su número de factura es correcto.');", true);
            }
        }
    }
}