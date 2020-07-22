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
using System.IO;
using System.Reflection;
using ClosedXML.Excel;

namespace CEPA.CCO.UI.Web
{
    public partial class wfRptIngreso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string prueba = "11";
                if (!IsPostBack)
                {
                    if (prueba == "11")
                    {
                        ddlYear.DataSource = TipoSolicitudDAL.ObtenerYears("DAN");
                        ddlYear.DataTextField = "sYearss";
                        ddlYear.DataValueField = "sYearss";
                        ddlYear.DataBind();

                        ddlYear.Items.Insert(0, new ListItem("-- Año -- "));
                    }
                    else
                    {
                        ddlYear.DataSource = TipoSolicitudDAL.ObtenerYears("UCC");
                        ddlYear.DataTextField = "sYearss";
                        ddlYear.DataValueField = "sYearss";
                        ddlYear.DataBind();

                        ddlYear.Items.Insert(0, new ListItem("-- Año -- "));
                    }

                    ddlMeses.DataSource = TipoSolicitudDAL.ObtenerMeses();
                    ddlMeses.DataTextField = "s_mes";
                    ddlMeses.DataValueField = "Mes";
                    ddlMeses.DataBind();

                    ddlMeses.Items.Insert(0, new ListItem("-- Mes -- "));
                }
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Durante la ejecución recomendamos volverlo intentar, o reportar a Informatica" + "');", true);
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string prueba = "11" == "11" ? "DAN": "UCC";

                Cargar(prueba);
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Por favor verificar los parametros de búsqueda (año y mes) no producen resultados, si son válidos reportar a Informática " + "');", true);
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                Generar();
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "", "bootbox.alert('" + "Error: Por favor verificar los parametros de búsqueda (año y mes) no producen resultados, si son válidos reportar a Informática " + "');", true);
            }

        }

        private void Generar()
        {
            int d_lineas = 0;
            int ROWS_START = 4;
            int iRow = 1;
            List<RptIngreImport> pRpt = new List<RptIngreImport>();
            string pTipo = "11" == "11" ? "DAN" : "UCC";
            pRpt = DocBuqueLINQ.detalleRptIngreso(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMeses.SelectedValue), pTipo);
            int corr = 1;

            if (pRpt.Count > 0)
            {
                string s_archivo = "";


                s_archivo = Server.MapPath("~/RptIngreso.xlsx");


                var workbook = new XLWorkbook(s_archivo);

                var ws = workbook.Worksheet(1);

                d_lineas = pRpt.Count;

                string _rango = null;

                _rango = "J";


                //BORDES TABLA
                ws.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                ws.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                ws.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.SetInsideBorderColor(XLColor.Black);
                ws.Range("A4", string.Concat(_rango, ROWS_START + d_lineas)).Style.Border.SetOutsideBorderColor(XLColor.Black);

                ws.Cell("A1").Value = "REPORTE CONSOLIDADO DEL MES DE " + ddlMeses.SelectedItem.Text + " / " + ddlYear.SelectedValue ;

                var range = ws.Range("A4", string.Concat(_rango, ROWS_START + d_lineas));
                var table = range.CreateTable();
                table.ShowAutoFilter = true;

                foreach (var item in pRpt)
                {
                    int iCurrent = ROWS_START + iRow;
                    if (iCurrent % 2 == 0)
                        ws.Range("A" + iCurrent.ToString(), string.Concat(_rango, iCurrent)).Style.Fill.BackgroundColor = XLColor.FromArgb(221, 235, 247);
                    else
                        ws.Range("A" + iCurrent.ToString(), string.Concat(_rango, iCurrent)).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 255, 255);

                    ws.Row(iCurrent).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Row(iCurrent).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    ws.Cell(iCurrent, 1).Value = corr;
                    ws.Cell(iCurrent, 2).Value = item.n_contenedor;
                    ws.Cell(iCurrent, 3).Value = item.p_procedencia;
                    ws.Cell(iCurrent, 4).Value = item.d_buque;
                    ws.Cell(iCurrent, 5).Value = item.navicorto;
                    ws.Cell(iCurrent, 6).Value = item.s_consignatario;
                    ws.Cell(iCurrent, 7).Value = item.s_mercaderia;
                    ws.Cell(iCurrent, 8).Value = item.f_retencion;
                    ws.Cell(iCurrent, 9).Value = item.f_liberacion;
                    ws.Cell(iCurrent, 10).Value = item.b_cancelado;

                    iRow = iRow + 1;
                    corr = corr + 1;
                }
                string archivo = null;

                archivo = string.Concat("RptIngreso_", ddlMeses.SelectedItem.Text, "-", ddlYear.Text, ".xlsx");

                Response.Clear();
                Response.ContentType =
                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", archivo));

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                }
                try
                {
                    HttpContext.Current.Response.End();
                }
                catch (System.Threading.ThreadAbortException err)
                {
                    System.Threading.Thread.ResetAbort();
                }
                catch (Exception err)
                {
                }
                finally
                {
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                //HttpContext.Current.Response.End();

                //HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                //HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                //HttpContext.Current.ApplicationInstance.CompleteRequest();

            }
            else
            {
                throw new Exception("La búsqueda no devuelve resultados");
            }
        }

        private void Cargar(string pTipo)
        {


            GridView1.DataSource = DocBuqueLINQ.detalleRptIngreso(Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlMeses.SelectedValue), pTipo);
            GridView1.DataBind();


            GridView1.HeaderRow.Cells[0].Attributes["data-class"] = "expand";

            // GridView1.HeaderRow.Cells[1].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[3].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[4].Attributes["data-hide"] = "phone";
            GridView1.HeaderRow.Cells[5].Attributes["data-hide"] = "phone";

            //GridView1.HeaderRow.Cells[8].Attributes["data-hide"] = "phone";

            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;

            GridView1.FooterRow.Cells[0].Attributes["text-align"] = "center";
            GridView1.FooterRow.TableSection = TableRowSection.TableFooter;
            //  ViewState["EmployeeList"] = GridView1.DataSource;
            ///
           
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
    }
}