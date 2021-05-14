using CEPA.CCO.DAL;
using CEPA.CCO.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using cxExcel = ClosedXML.Excel;

namespace CEPA.CCO.UI.Web.Navieras
{
    public partial class wfDocumentos : System.Web.UI.Page
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Crear libro de Excel
            var oWB = new cxExcel.XLWorkbook();
          
            string c_factura = txtBuscar.Text;
            string c_cliente = Session["c_naviera"].ToString();
            //string c_cliente = "1541";

            if (c_cliente == "1541")
                c_cliente = "1515";

            try
            {
                int validar = EncaBuqueDAL.getDocValid(DBComun.Estado.verdadero, c_factura, c_cliente);

                if (validar > 0)
                {

                    List<Documentos> pLstDoc = new List<Documentos>();
                    pLstDoc = EncaBuqueDAL.getListDocuments(DBComun.Estado.verdadero, c_factura, c_cliente);

                    if (pLstDoc.Count > 0)
                    {
                        var grupoTarifa = (from a in pLstDoc
                                           group a by new
                                           {
                                               a.c_tarifa,
                                               a.d_servicio,
                                               a.v_valor
                                           } into g
                                           select new
                                           {
                                               c_tarifa = g.Key.c_tarifa,
                                               d_servicio = g.Key.d_servicio,
                                               Total = g.Count(),
                                               cuotaTarifa = g.Key.v_valor
                                           }).OrderBy(t => t.c_tarifa).ToList();

                        string d_naviera = null;

                        foreach (var iHeader in pLstDoc)
                        {
                            d_naviera = iHeader.d_cliente;
                            break;
                        }

                        int ROWS_START = 9;
                        int iRow = 1;

                        int d_lineas = pLstDoc.Count;




                        // Creando hoja
                        var oSheet = oWB.Worksheets.Add("Hoja1");

                        //Primer titulo
                        oSheet.Range("A1", "S1").Merge();
                        oSheet.Range("A1", "S1").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A1", "S1").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A1", "S1").Style.Font.FontSize = 18;

                        oSheet.Cell("A1").Value = "COMISION EJECUTIVA PORTUARIA AUTONOMA";
                        oSheet.Cell("A1").Style.Font.Bold = true;
                        oSheet.Row(1).Height = 25;

                        //Segundo titulo
                        oSheet.Range("A2", "S2").Merge();
                        oSheet.Range("A2", "S2").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A2", "S2").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A2", "S2").Style.Font.FontSize = 16;

                        oSheet.Cell("A2").Value = "PUERTO DE ACAJUTLA";
                        oSheet.Cell("A2").Style.Font.Bold = true;
                        oSheet.Row(2).Height = 21;

                        // Tercer titulo
                        oSheet.Range("A3", "S3").Merge();
                        oSheet.Range("A3", "S3").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A3", "S3").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A3", "S3").Style.Font.FontSize = 16;

                        oSheet.Cell("A3").Value = "UNIDAD DE CONTENEDORES";
                        oSheet.Cell("A3").Style.Font.Bold = true;
                        oSheet.Row(3).Height = 21;

                        //Cuarto titulo
                        oSheet.Range("A4", "S4").Merge();
                        oSheet.Range("A4", "S4").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A4", "S4").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A4", "S4").Style.Font.FontSize = 16;

                        oSheet.Cell("A4").Value = "ESTADIA PARA CONTENEDORES ENTREGADO EN PATIO";
                        oSheet.Cell("A4").Style.Font.Bold = true;
                        oSheet.Row(4).Height = 21;

                        //CABECERA 

                        oSheet.Range("L7", "O7").Merge();
                        oSheet.Range("L7", "O7").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("L7", "O7").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("L7", "O7").Style.Font.FontSize = 12;

                        oSheet.Cell("L7").Value = "ESTADIA";
                        oSheet.Cell("L7").Style.Font.Bold = true;
                        oSheet.Row(7).Height = 16;

                        //CABECERA  #2

                        oSheet.Range("L8", "O8").Merge();
                        oSheet.Range("L8", "O8").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("L8", "O8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("L8", "O8").Style.Font.FontSize = 12;

                        oSheet.Cell("L8").Value = "PARA COBRO";
                        oSheet.Cell("L8").Style.Font.Bold = true;
                        oSheet.Row(8).Height = 16;

                        //CABECERA  NAVIERA

                        oSheet.Range("A7", "K8").Merge();
                        oSheet.Range("A7", "K8").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A7", "K8").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("A7", "K8").Style.Font.FontSize = 12;

                        oSheet.Cell("A7").Value = "AGENCIA: " + SanitizeXmlString(d_naviera);
                        oSheet.Cell("A7").Style.Font.Bold = true;
                        oSheet.Row(7).Height = 16;

                        // CABECERA TABLA BORDES Y FONDO
                        oSheet.Range("A9:S9").Style.Font.Bold = true;
                        oSheet.Range("A9:S9").Style.Fill.BackgroundColor = cxExcel.XLColor.LightGray;
                        oSheet.Range("A9:S9").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("A9:S9").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                        //BORDES TABLA
                        oSheet.Range("A9", string.Concat("S", ROWS_START + d_lineas)).Style.Border.InsideBorder = cxExcel.XLBorderStyleValues.Thin;
                        oSheet.Range("A9", string.Concat("S", ROWS_START + d_lineas)).Style.Border.OutsideBorder = cxExcel.XLBorderStyleValues.Medium;

                        oSheet.Range("A9", string.Concat("S", ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(cxExcel.XLColor.Black);
                        oSheet.Range("A9", string.Concat("S", ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(cxExcel.XLColor.Black);


                        // NOMBRANDO CABECERA TABLA
                        oSheet.Cell(9, 1).Value = "CONTENEDOR";
                        oSheet.Cell(9, 2).Value = "TARA";
                        oSheet.Cell(9, 3).Value = "20'";
                        oSheet.Cell(9, 4).Value = "40'";
                        oSheet.Cell(9, 5).Value = "45'";
                        oSheet.Cell(9, 6).Value = "48'";
                        oSheet.Cell(9, 7).Value = "FCL";
                        oSheet.Cell(9, 8).Value = "LCL";
                        oSheet.Cell(9, 9).Value = "VAC";
                        oSheet.Cell(9, 10).Value = "TRANS";
                        oSheet.Cell(9, 11).Value = "REAL";
                        oSheet.Cell(9, 12).Value = "20'";
                        oSheet.Cell(9, 13).Value = "40'";
                        oSheet.Cell(9, 14).Value = "45'";
                        oSheet.Cell(9, 15).Value = "48'";

                        oSheet.Range("P9", "R9").Merge();
                        oSheet.Range("P9", "R9").Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("P9", "R9").Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Range("P9", "R9").Style.Font.FontSize = 12;

                        oSheet.Cell("P9").Value = "DATOS DE INGRESO";
                        oSheet.Cell("P9").Style.Font.Bold = true;
                        oSheet.Row(9).Height = 16;

                        oSheet.Cell(9, 19).Value = "DESPACHO";

                        // DANDO FORMATO
                        oSheet.Range("A10", string.Concat("S", ROWS_START + d_lineas)).Style.Font.FontSize = 12;

                        oSheet.Range("B10", string.Concat("B", (ROWS_START + d_lineas) + 1)).Style.NumberFormat.SetFormat("#,##0");

                        oSheet.Range("B10", string.Concat("B", +(ROWS_START + d_lineas))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("B10", string.Concat("B", +(ROWS_START + d_lineas))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;

                        oSheet.Range("C10", string.Concat("O", (ROWS_START + d_lineas) + 1)).Style.NumberFormat.SetFormat("0");

                        oSheet.Range("C10", string.Concat("O", +(ROWS_START + d_lineas))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("C10", string.Concat("O", +(ROWS_START + d_lineas))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                        oSheet.Range("P10", string.Concat("P", +(ROWS_START + d_lineas))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("P10", string.Concat("P", +(ROWS_START + d_lineas))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                        oSheet.Range("Q10", string.Concat("Q", +(ROWS_START + d_lineas))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("Q10", string.Concat("Q", +(ROWS_START + d_lineas))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Left;

                        oSheet.Range("R10", string.Concat("S", +(ROWS_START + d_lineas))).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range("R10", string.Concat("S", +(ROWS_START + d_lineas))).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;


                        oSheet.Column(1).Width = 14;
                        oSheet.Column(2).Width = 6;
                        oSheet.Column(3).Width = 6;
                        oSheet.Column(4).Width = 4;
                        oSheet.Column(5).Width = 4;
                        oSheet.Column(6).Width = 4;
                        oSheet.Column(7).Width = 4;
                        oSheet.Column(8).Width = 4;
                        oSheet.Column(9).Width = 5;
                        oSheet.Column(10).Width = 7;
                        oSheet.Column(11).Width = 6;
                        oSheet.Column(12).Width = 4;
                        oSheet.Column(13).Width = 4;
                        oSheet.Column(14).Width = 4;
                        oSheet.Column(15).Width = 4;
                        oSheet.Column(16).Width = 10;
                        oSheet.Column(17).Width = 20;
                        oSheet.Column(18).Width = 12;
                        oSheet.Column(19).Width = 12;

                        foreach (var item in pLstDoc)
                        {
                            int iCurrent = ROWS_START + iRow;
                            oSheet.Row(iCurrent).Height = 25;
                            oSheet.Cell(iCurrent, 1).Value = SanitizeXmlString(item.n_contenedor);
                            oSheet.Cell(iCurrent, 2).Value = Convert.ToInt32(SanitizeXmlString(item.v_tara.Replace(",", "").Replace(".","")));
                            oSheet.Cell(iCurrent, 3).Value = item.b_20;
                            oSheet.Cell(iCurrent, 4).Value = item.b_40;
                            oSheet.Cell(iCurrent, 5).Value = item.b_45;
                            oSheet.Cell(iCurrent, 6).Value = item.b_48;
                            oSheet.Cell(iCurrent, 7).Value = item.b_fcl;
                            oSheet.Cell(iCurrent, 8).Value = item.b_lcl;
                            oSheet.Cell(iCurrent, 9).Value = item.b_vac;
                            oSheet.Cell(iCurrent, 10).Value = item.b_trans;
                            oSheet.Cell(iCurrent, 11).Value = item.real;
                            oSheet.Cell(iCurrent, 12).Value = item.c_20;
                            oSheet.Cell(iCurrent, 13).Value = item.c_40;
                            oSheet.Cell(iCurrent, 14).Value = item.c_45;
                            oSheet.Cell(iCurrent, 15).Value = item.c_48;
                            oSheet.Cell(iCurrent, 16).Value = SanitizeXmlString(item.c_nul);
                            oSheet.Cell(iCurrent, 17).Value = SanitizeXmlString(item.d_buque);
                            oSheet.Cell(iCurrent, 18).Value = SanitizeXmlString(item.f_ingreso);
                            oSheet.Cell(iCurrent, 19).Value = SanitizeXmlString(item.f_despacho);

                            iRow = iRow + 1;
                        }

                        int valorTarifa = (ROWS_START + d_lineas) + 2;

                        oSheet.Cell(string.Concat("A", valorTarifa)).Value = "TARIFA";
                        oSheet.Cell(string.Concat("A", valorTarifa)).Style.Font.Bold = true;
                        oSheet.Cell(string.Concat("A", valorTarifa)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Cell(string.Concat("A", valorTarifa)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;
                        oSheet.Row(valorTarifa).Height = 16;

                        int valorDetalle = valorTarifa + 1;

                        foreach (var iTarifa in grupoTarifa)
                        {
                            oSheet.Cell(valorDetalle, 1).Value = SanitizeXmlString(iTarifa.c_tarifa);
                            oSheet.Cell(valorDetalle, 2).Value = iTarifa.Total;
                            oSheet.Cell(valorDetalle, 3).Value = SanitizeXmlString(iTarifa.d_servicio);

                            var s_20 = pLstDoc.Where(x => x.c_tarifa == iTarifa.c_tarifa).Sum(y => y.c_20);
                            var s_40 = pLstDoc.Where(x => x.c_tarifa == iTarifa.c_tarifa).Sum(y => y.c_40);
                            var s_45 = pLstDoc.Where(x => x.c_tarifa == iTarifa.c_tarifa).Sum(y => y.c_45);
                            var s_48 = pLstDoc.Where(x => x.c_tarifa == iTarifa.c_tarifa).Sum(y => y.c_48);
                            int sTotal = s_20 + s_40 + s_45 + s_48;

                            oSheet.Cell(valorDetalle, 16).Value = sTotal;
                            oSheet.Cell(valorDetalle, 17).Value = SanitizeXmlString("dias a US");


                            oSheet.Range(string.Concat("R", valorDetalle), string.Concat("R", valorDetalle)).Style.NumberFormat.SetFormat("_($* #,##0.00_);_($* (#,##0.00);_($* \" - \"??_);_(@_)");

                            oSheet.Cell(valorDetalle, 18).Value = iTarifa.cuotaTarifa;
                            oSheet.Cell(valorDetalle, 19).SetFormulaA1(string.Format("=R{0}*P{1}", valorDetalle.ToString(), valorDetalle.ToString()));

                            oSheet.Range(string.Concat("S", valorDetalle), string.Concat("S", valorDetalle)).Style.NumberFormat.SetFormat("_($* #,##0.00_);_($* (#,##0.00);_($* \" - \"??_);_(@_)");

                            valorDetalle = valorDetalle + 1;
                        }

                        int valorTotales = valorDetalle;

                        oSheet.Cell(valorTotales, 19).SetFormulaA1(string.Format("=SUM(S{0}:S{1})", (valorTotales - 2).ToString(), (valorTotales - 1).ToString()));

                        oSheet.Cell(valorTotales + 1, 18).Value = "13%";
                        oSheet.Cell(valorTotales + 1, 18).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Cell(valorTotales + 1, 18).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Right;


                        oSheet.Cell(valorTotales + 1, 19).SetFormulaA1(string.Format("=S{0}*R{1}", valorTotales.ToString(), (valorTotales + 1).ToString()));

                        oSheet.Cell(valorTotales + 2, 19).SetFormulaA1(string.Format("=SUM(S{0}:S{1})", valorTotales.ToString(), (valorTotales + 1).ToString()));

                        oSheet.Range(string.Concat("S", valorTotales - 2), string.Concat("S", valorTotales + 2)).Style.NumberFormat.SetFormat("_($* #,##0.00_);_($* (#,##0.00);_($* \" - \"??_);_(@_)");

                        oSheet.Range(string.Concat("A", valorTotales - 2), string.Concat("B", valorTotales - 1)).Style.NumberFormat.SetFormat("0");

                        oSheet.Range(string.Concat("P", valorTotales - 2), string.Concat("P", valorTotales - 1)).Style.NumberFormat.SetFormat("0");

                        oSheet.Cell(string.Concat("R", valorTotales + 1)).Style.NumberFormat.SetFormat("0%");

                        oSheet.Range(string.Concat("A", valorTotales - 2), string.Concat("B", valorTotales - 1)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range(string.Concat("A", valorTotales - 2), string.Concat("B", valorTotales - 1)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                        oSheet.Range(string.Concat("P", valorTotales - 2), string.Concat("P", valorTotales - 1)).Style.Alignment.Vertical = cxExcel.XLAlignmentVerticalValues.Center;
                        oSheet.Range(string.Concat("P", valorTotales - 2), string.Concat("P", valorTotales - 1)).Style.Alignment.Horizontal = cxExcel.XLAlignmentHorizontalValues.Center;

                        oSheet.Cell(string.Concat("S", valorTotales + 2)).Style.Font.Bold = true;

                        oSheet.Cell(string.Concat("S", valorTotales)).Style.Font.Bold = true;

                        oSheet.Cell(string.Concat("S", (valorTotales - 1))).Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;

                        oSheet.Cell(string.Concat("S", valorTotales + 1)).Style.Border.BottomBorder = cxExcel.XLBorderStyleValues.Medium;

                        string f_actual = DateTime.Now.ToString("ddMMyyhhmm");
                        string _nombre = string.Format("ANEXO_{0}_{1}.xlsx", c_factura, f_actual);
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


                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('No se poseen registros para el # de Documento ingresado');", true);
                }
            }
            catch(Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + ex.Message + "');", true);
            }
        }
    }
}